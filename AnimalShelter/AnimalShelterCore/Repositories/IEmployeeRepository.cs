using AnimalShelterCore.Models;

namespace AnimalShelterCore.Repositories;

public interface IEmpoyeeRepository
{
    List<Employee> GetAll();
    Employee GetById(long id);
    void Add(Employee employee);
    void Update(Employee employee);
    void Delete(long id);
}