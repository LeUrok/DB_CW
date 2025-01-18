using AnimalShelterCore.Models;

namespace AnimalShelterCore.Repositories;

public interface IAnimalBreedRepository
{
    List<AnimalBreed> GetAll();
    AnimalBreed GetById(long id);
    void Add(AnimalBreed animalBreed);
    void Update(AnimalBreed animalBreed);
    void Delete(long id);
}