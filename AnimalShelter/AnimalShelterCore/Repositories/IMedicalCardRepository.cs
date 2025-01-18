using AnimalShelterCore.Models;

namespace AnimalShelterCore.Repositories;

public interface IMedicalCardRepository
{
    List<MedicalCard> GetAll();
    MedicalCard GetById(long id);
    Animal GetAnimal(long id);
    void Add(MedicalCard medicalCard);
    void Update(MedicalCard medicalCard);
    void Delete(long id);
}