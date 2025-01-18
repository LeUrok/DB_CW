using AnimalShelterCore.Models;

namespace AnimalShelterCore.Repositories;

public interface IUserRepository
{
    List<User> GetAll();
    User GetById(long id);
    public List<User> GetAllUsersWithEmployeeId();
    public List<User> GetAllUsersWithGuardianId();
    User GetByName(string name);
    void Add(User user);
    void Update(User user);
    void Delete(long id);
}