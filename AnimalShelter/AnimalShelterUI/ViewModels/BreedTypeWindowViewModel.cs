using System;
using System.Collections.ObjectModel;
using AnimalShelterCore.Models;
using AnimalShelterCore.Repositories;
namespace AnimalShelter.AnimalShelterUI.ViewModels;

public class BreedTypeWindowViewModel
{
    public ObservableCollection<AnimalType> TypesList { get; set; }
    public ObservableCollection<AnimalBreed> BreedsList { get; set; }
    public string Type { get; set; } = null!;
    public string Breed { get; set; } = null!;

    public BreedTypeWindowViewModel(IDataBase database)
    {
        _database = database;
        TypesList = new ObservableCollection<AnimalType>(_database.AnimalTypeRepository.GetAll());
        BreedsList = new ObservableCollection<AnimalBreed>(_database.AnimalBreedRepository.GetAll());
    }
    public void AddType()
    {
        _type = new AnimalType();
        _type.Name = Type;
        _database.AnimalTypeRepository.Add(_type);
        UpdateTypeList();
    }
    public void DeleteType(int index)
    {
        if(index >= 0 && index < TypesList.Count)
        {
            _database.AnimalTypeRepository.Delete(TypesList[index].Id);
            TypesList.RemoveAt(index);
            UpdateTypeList();
        }
    }
    public void AddBreed()
    {
        _breed = new AnimalBreed();
        _breed.Name = Breed;
        _database.AnimalBreedRepository.Add(_breed);
        UpdateBreedList();
    }
    public void DeleteBreed(int index)
    {
        if(index >= 0 && index < BreedsList.Count)
        {
            _database.AnimalBreedRepository.Delete(BreedsList[index].Id);
            BreedsList.RemoveAt(index);
            UpdateBreedList();
        }
    }
    internal void UpdateTypeList()
    {
        TypesList.Clear();
        foreach (var type in _database.AnimalTypeRepository.GetAll())
            TypesList.Add(type);
    }
    internal void UpdateBreedList()
    {
        BreedsList.Clear();
        foreach (var breed in _database.AnimalBreedRepository.GetAll())
            BreedsList.Add(breed);
    }
    private readonly IDataBase _database;
    private AnimalType _type;
    private AnimalBreed _breed;
}