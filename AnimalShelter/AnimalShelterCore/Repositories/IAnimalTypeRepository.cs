using AnimalShelterCore.Models;

namespace AnimalShelterCore.Repositories;

public interface IAnimalTypeRepository
{
    List<AnimalType> GetAll();
    AnimalType GetById(long id);

    void Add(AnimalType animalType);
    void Update(AnimalType animalType);
    void Delete(long id);
}