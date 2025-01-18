using AnimalShelterCore.Models;
using AnimalShelterCore.Repositories;
using Npgsql;

namespace AnimalShelterPostgreSql.Repositories;

class MedicalCardPostgreSql : IMedicalCardRepository
{
    private string connectionString;
    private IAnimalRepository animalRepository;
    public MedicalCardPostgreSql(string connectionString, IAnimalRepository animalRepository)
    {
        this.connectionString = connectionString;
        this.animalRepository = animalRepository;
    }
    public void Add(MedicalCard medicalCard)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"INSERT INTO MedicalCard (AnimalId, DateOfLastVisit, Diagnoses, Therapy) VALUES ({medicalCard.AnimalId}, " +
            $"'{medicalCard.DateOfLastVisit.ToString("yyyy-MM-dd HH:mm:ss")}', '{medicalCard.Diagnoses}', " +
            $"'{medicalCard.Therapy}') RETURNING Id;"); 
        var reader = command.ExecuteReader();
        reader.Read();
        medicalCard.Id = reader.GetInt64(0);

        using var com = dataSource.CreateCommand(
            $"UPDATE Animal SET " +
            $"MedicalCardId = {medicalCard.Id} " + 
            $"WHERE Id = {medicalCard.AnimalId};");
        com.ExecuteNonQuery();
    }

    public void Delete(long id)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"DELETE FROM MedicalCard WHERE Id = {id};\n" 
            );
        command.ExecuteNonQuery();
    }

    public List<MedicalCard> GetAll()
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"SELECT * FROM MedicalCard;" 
            );
        var reader = command.ExecuteReader();
        List<MedicalCard> medicalCards = new List<MedicalCard>();
        while(reader.Read())
        {
            medicalCards.Add(new MedicalCard
            {
                Id = reader.GetInt64(0),
                AnimalId = reader.GetInt64(1),
                DateOfLastVisit = reader.GetDateTime(2),
                Diagnoses = reader.GetString(3),
                Therapy = reader.GetString(4)
            });
        }
        
        return medicalCards;
    }

    public Animal GetAnimal(long id)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"SELECT a.Id, a.Name, a.AnimalTypeId, a.AnimalBreedId, a.Gender, " +
            $"a.Age, a.Weight, a.Description, a.MedicalCardId " +
            $"FROM MedicalCard mc " +
            $"JOIN Animal a ON mc.AnimalId = a.Id " +
            $"WHERE mc.Id = {id};"
        );
        var reader = command.ExecuteReader();
        reader.Read();
        Animal animal = new Animal
        {
            Id = reader.GetInt64(0),
            Name = reader.GetString(1),
            AnimalTypeId = reader.GetInt64(2),
            AnimalBreedId = reader.GetInt64(3),
            Gender = (Gender)reader.GetInt32(4),
            Age = reader.GetInt32(5),
            Weight = reader.GetDouble(6),
            Description = reader.GetString(7),
            MedicalCardId = reader.GetInt64(8),
        };
        //добавить guardianids
        return animal;
    }

    public MedicalCard GetById(long id)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"SELECT * " +
            $"FROM MedicalCard " +
            $"WHERE Id = {id};"
        );
        var reader = command.ExecuteReader();
        reader.Read();
        MedicalCard medicalCard = new MedicalCard
        {
            Id = reader.GetInt64(0),
            AnimalId = reader.GetInt64(2),
            DateOfLastVisit = reader.GetDateTime(3),
            Diagnoses = reader.GetString(4),
            Therapy = reader.GetString(5)
        };
        
        return medicalCard;
    }

    public void Update(MedicalCard medicalCard)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"UPDATE MedicalCard SET " +
            $"AnimalId = {medicalCard.AnimalId}, " +
            $"DateOfLastVisit = '{medicalCard.DateOfLastVisit.ToString("yyyy-MM-dd HH:mm:ss")}', " +
            $"Diagnoses = '{medicalCard.Diagnoses}', " +
            $"Therapy = '{medicalCard.Therapy}' " + 
            $"WHERE Id = {medicalCard.Id};"
            );
        command.ExecuteNonQuery();
    }
}
