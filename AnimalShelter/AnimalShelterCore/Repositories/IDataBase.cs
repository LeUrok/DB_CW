namespace AnimalShelterCore.Repositories;

public interface IDataBase
{
    IAnimalRepository AnimalRepository { get; }
    IAnimalTypeRepository AnimalTypeRepository { get; }
    IAnimalBreedRepository AnimalBreedRepository { get;}
    IEmpoyeeRepository EmployeeRepository { get; }
    IFormRepository FormRepository { get; }
    IGuardianRepository GuardianRepository { get; }
    IMedicalCardRepository MedicalCardRepository { get; }
    IRequestRepository RequestRepository { get; }
    IUserRepository UserRepository { get; }
    void Initialize();
}