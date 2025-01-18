using AnimalShelterCore.Models;

namespace AnimalShelterCore.Repositories;

public interface IAnimalRepository
{
    List<Animal> GetAll();
    Animal GetById(long id);
    AnimalBreed GetBreed(long id);
    AnimalType GetType(long id);
    void Add(Animal animal);
    void Update(Animal animal);
    void Delete(long id);
    MedicalCard GetMedicalCard(long id);
    List<Guardian> GetGuardians(long id);
    List<Animal> GetAllOfBreed(long breedId);
    List<Animal> GetAllOfType(long typeId);
    List<Animal> GetAllOfBreedAndType(long breedId, long typeId);
}