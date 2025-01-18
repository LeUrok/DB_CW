using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using AnimalShelterCore.Models;
using AnimalShelterCore.Repositories;

namespace AnimalShelter.AnimalShelterUI.ViewModels;

public class MainWindowViewModel
{
    public ObservableCollection<Animal> AnimalsList { get; set; }
    public ObservableCollection<AnimalType> TypesList { get; set; }
    public ObservableCollection<AnimalBreed> BreedsList { get; set; }
    public string AnimalName { get; set; }
    public MainWindowViewModel(IDataBase database)
    {
        _database = database;
        AnimalsList = new ObservableCollection<Animal>(_database.AnimalRepository.GetAll());
        TypesList = new ObservableCollection<AnimalType>(_database.AnimalTypeRepository.GetAll());
        BreedsList = new ObservableCollection<AnimalBreed>(_database.AnimalBreedRepository.GetAll());
    }

    public void DeleteAnimal(int index)
    {
        if(index >= 0 && index < AnimalsList.Count)
        {
            _database.AnimalRepository.Delete(AnimalsList[index].Id);
            AnimalsList.RemoveAt(index);
            UpdateLists();
        }
    }
    internal void UpdateLists()
    {
        AnimalsList.Clear();
        foreach (var animal in _database.AnimalRepository.GetAll())
            AnimalsList.Add(animal);
    }
    internal void UpdateBreedTypeLists()
    {
        TypesList.Clear();
        BreedsList.Clear();
        foreach (var breed in _database.AnimalBreedRepository.GetAll())
            BreedsList.Add(breed);
        foreach (var type in _database.AnimalTypeRepository.GetAll())
            TypesList.Add(type);
    }

    public void GetAnimalsByName(string name)
    {
        var filteredAnimals = AnimalsList.Where(animal => animal.Name.Contains(name)).ToList();
        AnimalsList.Clear();
        foreach (var animal in filteredAnimals)
            AnimalsList.Add(animal);
    }
    public void GetAnimalsByType(long typeId)
    {
        AnimalsList.Clear();
        foreach (var animal in _database.AnimalRepository.GetAllOfType(typeId))
            AnimalsList.Add(animal);
    }
    public void GetAnimalsByBreed(long typeId)
    {
        AnimalsList.Clear();
        foreach (var animal in _database.AnimalRepository.GetAllOfBreed(typeId))
            AnimalsList.Add(animal);
    }
    public void GetAnimalsByBreedAndType(long typeId, long breedId)
    {
        AnimalsList.Clear();
        foreach (var animal in _database.AnimalRepository.GetAllOfBreedAndType(typeId, breedId))
            AnimalsList.Add(animal);
    }
    private readonly IDataBase _database;
}