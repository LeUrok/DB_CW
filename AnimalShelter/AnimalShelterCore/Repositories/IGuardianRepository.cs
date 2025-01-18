using AnimalShelterCore.Models;

namespace AnimalShelterCore.Repositories;

public interface IGuardianRepository
{
    List<Guardian> GetAll();
    Guardian GetById(long id);
    List<Animal> GetAnimals(long id);
    public void AddGuardianToAnimal(long gid, long aid);
    public void DeleteAnimalGuardian(long aid, long gid);
    User GetUser(long id);
    void Add(Guardian guardian);
    void Update(Guardian guardian);
    void Delete(long id);
}