using System;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using AnimalShelterCore.Models;
using AnimalShelterCore.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace AnimalShelter.AnimalShelterUI.ViewModels;

public class AnimalWindowViewModel
{
    public ObservableCollection<User> GuardiansList { get; set; }
    public ObservableCollection<User> GuardiansListComboBox { get; set; }
    public bool IsMale { get; set; }
    public bool IsFemale { get; set; }
    public DateTime DateOfLastVisit { get; set; }
    public string Diagnoses { get; set; }
    public string Therapy  { get; set; }
    public List<AnimalType> AnimalTypes { get; set; }
    public List<AnimalBreed> AnimalBreeds { get; set; }
    public string Name { get; set; }
    public AnimalType AnimalType { get; set; }
    public AnimalBreed AnimalBreed { get; set; }
    public Gender Gender { get; set; }
    public int Age { get; set; }
    public double Weight { get; set; }
    public string Description { get; set; }
    public bool IsSave { get; set; } = false;
    public bool Creation { get; set; } = false;
    public AnimalWindowViewModel(IDataBase database)
    {
        _database = database;
        GuardiansList = new ObservableCollection<User>();
        GuardiansListComboBox = new ObservableCollection<User>(_database.UserRepository.GetAllUsersWithGuardianId());
        _medicalCard = new MedicalCard();
        DateOfLastVisit = DateTime.Now;
        Diagnoses = string.Empty;
        Therapy = string.Empty;
        _animal = new Animal();
        AnimalTypes = database.AnimalTypeRepository.GetAll();
        AnimalBreeds = database.AnimalBreedRepository.GetAll();
        Name = _animal.Name;
        AnimalType = AnimalTypes.Count > 0 ? AnimalTypes[0] : null;
        AnimalBreed = AnimalBreeds.Count > 0 ? AnimalBreeds[0] : null;
        Gender = _animal.Gender;
        Age = 0;
        Weight = 0;
        IsMale = true;
        Description = string.Empty;
        Creation = true;
    }
    public AnimalWindowViewModel(IDataBase database, Animal animal)
    {
        _database = database;
        var guardians = _database.AnimalRepository.GetGuardians(animal.Id);
        var userGuardians = guardians.Select(guardian => _database.GuardianRepository.GetUser(guardian.Id)).ToList();
        GuardiansList = new ObservableCollection<User>(userGuardians);
        GuardiansListComboBox = new ObservableCollection<User>(_database.UserRepository.GetAllUsersWithGuardianId());
        _medicalCard = database.AnimalRepository.GetMedicalCard(animal.Id);
        DateOfLastVisit = _medicalCard.DateOfLastVisit;
        Diagnoses = _medicalCard.Diagnoses;
        Therapy = _medicalCard.Therapy;
        _animal = animal;
        Name = _animal.Name;
        AnimalTypes = database.AnimalTypeRepository.GetAll();
        AnimalBreeds = database.AnimalBreedRepository.GetAll();
        AnimalType = database.AnimalRepository.GetType(animal.Id);
        AnimalBreed = database.AnimalRepository.GetBreed(animal.Id);
        Gender = _animal.Gender;
        Age = _animal.Age;
        Weight = _animal.Weight;
        Description = _animal.Description;
        IsMale = _animal.Gender == Gender.Male;
        IsFemale = _animal.Gender == Gender.Female;
    }
    
    public void Save(bool isVet, bool IsAssistent)
    {
        IsSave = true;
        _medicalCard.DateOfLastVisit = DateOfLastVisit;
        _medicalCard.Diagnoses = Diagnoses;
        _medicalCard.Therapy = Therapy;
        _animal.Name = Name;
        _animal.AnimalTypeId = AnimalType.Id;
        _animal.AnimalBreedId = AnimalBreed.Id;
        _animal.Gender = IsMale? Gender.Male: Gender.Female;
        _animal.Age = Age;
        _animal.Weight = Weight;
        _animal.Description = Description;
        if(Creation)
        {
            _database.AnimalRepository.Add(_animal);
            _medicalCard.AnimalId = _animal.Id;
            _database.MedicalCardRepository.Add(_medicalCard);
        }
            
        else
        {
            if(!isVet)
                _database.AnimalRepository.Update(_animal);
            if(!IsAssistent)
                _database.MedicalCardRepository.Update(_medicalCard);
        }
            
    }

    public void AddGuardianToAnimal(long guardianId)
    {
        _database.GuardianRepository.AddGuardianToAnimal(_animal.Id, guardianId);
        _animal.GuardianIds.Add(guardianId);
        _animal.GuardianIds = _animal.GuardianIds.Distinct().ToList();
    }
    internal void UpdateGuardianList()
    {
        GuardiansList.Clear();
        foreach (var guardian in _database.AnimalRepository.GetGuardians(_animal.Id))
        {
            User userGuardian = _database.GuardianRepository.GetUser(guardian.Id);
            GuardiansList.Add(userGuardian);
        }
    }
    public void DeleteGuardian(int index)
    {
        if(index >= 0 && index < GuardiansList.Count)
        {
            _animal.GuardianIds.Remove(GuardiansList[index].GuardianId.Value);
            _database.GuardianRepository.DeleteAnimalGuardian(_animal.Id, GuardiansList[index].GuardianId.Value);
            GuardiansList.RemoveAt(index);
            UpdateGuardianList();
        }
    }
    private readonly IDataBase _database;
    private readonly Animal _animal;
    private readonly MedicalCard _medicalCard;
}