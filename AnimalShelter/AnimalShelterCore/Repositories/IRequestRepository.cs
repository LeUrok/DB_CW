using AnimalShelterCore.Models;

namespace AnimalShelterCore.Repositories;

public interface IRequestRepository
{
    List<Request> GetAll();
    Request GetById(long id);
    Animal GetAnimal(long id);
    User GetUser(long id);
    void Add(Request request);
    void Update(Request request);
    void Delete(long id);
}