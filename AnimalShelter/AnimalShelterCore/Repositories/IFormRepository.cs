using AnimalShelterCore.Models;

namespace AnimalShelterCore.Repositories;

public interface IFormRepository
{
    List<Form> GetAll();
    Form GetById(long id);
    Request GetRequestByFormId(long id);
    void Add(Form form);
    void Update(Form form);
    void Delete(long id);
}