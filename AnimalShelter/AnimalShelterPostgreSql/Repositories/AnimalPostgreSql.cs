using System.Data;
using AnimalShelterCore.Models;
using AnimalShelterCore.Repositories;
using Npgsql;

namespace AnimalShelterPostgreSql.Repositories;

class AnimalPostgreSql : IAnimalRepository
{
    private string connectionString;

    public AnimalPostgreSql(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public void Add(Animal animal)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        string medicalCardId = animal.MedicalCardId == null? "NULL": animal.MedicalCardId.ToString()!;
        using var command = dataSource.CreateCommand(
            $"INSERT INTO Animal (Name, AnimalTypeId, AnimalBreedId, " +
            $"Gender, Age, Weight, Description, MedicalCardId) VALUES" +
            $"('{animal.Name}', {animal.AnimalTypeId}, {animal.AnimalBreedId},  " +
            $"{(int)animal.Gender}, {animal.Age}, {animal.Weight}, " +
            $"'{animal.Description}', {medicalCardId}) RETURNING Id;\n" 
            );
        var reader = command.ExecuteReader();
        reader.Read();
        animal.Id = reader.GetInt64(0);
        for(int i = 0; i < animal.GuardianIds.Count; i++)
        {
            using var command2 = dataSource.CreateCommand(
            $"INSERT INTO Animal_Guardian (AnimalId, GuardianId) VALUES ({animal.Id}, {animal.GuardianIds[i]});");
            command2.ExecuteNonQuery();
        }
    }

    public void Delete(long id)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"DELETE FROM Animal WHERE Id = {id};\n" 
            );
        command.ExecuteNonQuery();
    }

    public List<Animal> GetAll()
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"SELECT * FROM Animal;\n" 
            );
        var reader = command.ExecuteReader();
        List<Animal> animals = new List<Animal>();
        while(reader.Read())
        {
            List<long> guardianIds = GetGuardianIds(dataSource, reader.GetInt64(0));
            animals.Add(
                new Animal 
                {
                    Id = reader.GetInt64(0),
                    Name = reader.GetString(1),
                    AnimalTypeId = reader.GetInt64(2),
                    AnimalBreedId = reader.GetInt64(3),
                    Gender = (Gender)reader.GetInt32(4),
                    Age = reader.GetInt32(5),
                    Weight = reader.GetDouble(6),
                    Description = reader.GetString(7),
                    MedicalCardId = reader.IsDBNull(8) ? (long?)null : reader.GetInt64(8),
                    GuardianIds = guardianIds
                }
            );
        }
        return animals;
    }

    public List<Animal> GetAllOfBreed(long breedId)
    {
        List<Animal> animals = new List<Animal>();
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"SELECT * FROM GetAnimalsByBreed({breedId});\n" 
            );
        var reader = command.ExecuteReader();
        while (reader.Read())
        {
            List<long> guardianIds = GetGuardianIds(dataSource, reader.GetInt64(0));
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
                GuardianIds = guardianIds
            };

            animals.Add(animal);
        }

        return animals;
    }

    public List<Animal> GetAllOfBreedAndType(long breedId, long typeId)
    {
        List<Animal> animals = new List<Animal>();
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"select * from GetAnimalsByTypeAndBreed({typeId}, {breedId});\n" 
            );
        var reader = command.ExecuteReader();
        while (reader.Read())
        {
            List<long> guardianIds = GetGuardianIds(dataSource, reader.GetInt64(0));
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
                GuardianIds = guardianIds
            };

            animals.Add(animal);
        }

        return animals;
    }

    public List<Animal> GetAllOfType(long typeId)
    {
        List<Animal> animals = new List<Animal>();
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"select * from GetAnimalsByType({typeId});\n" 
            );
        var reader = command.ExecuteReader();
        while (reader.Read())
        {
            List<long> guardianIds = GetGuardianIds(dataSource, reader.GetInt64(0));
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
                GuardianIds = guardianIds
            };

            animals.Add(animal);
        }

        return animals;
    }

    public AnimalBreed GetBreed(long id)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"SELECT ab.Id, ab.Name " +
            $"FROM Animal a " +
            $"JOIN AnimalBreed ab ON a.AnimalBreedId = ab.Id " +
            $"WHERE a.Id = {id};"
        );
        var reader = command.ExecuteReader();
        reader.Read();
        AnimalBreed animalBreed = new AnimalBreed
            {
                Id = reader.GetInt64(0),
                Name = reader.GetString(1),
            };

        return animalBreed;
    }

    public Animal GetById(long id)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"SELECT * " +
            $"FROM Animal " +
            $"WHERE Id = {id};"
        );
        var reader = command.ExecuteReader();
        reader.Read();
        List<long> guardianIds = GetGuardianIds(dataSource, reader.GetInt64(0));
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
            GuardianIds = guardianIds
        };

        return animal;
    }


    public List<Guardian> GetGuardians(long id)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        List<long> guardianIds = GetGuardianIds(dataSource, id);
        List<Guardian>  guardians = new List<Guardian>();
        foreach (var guardianId in guardianIds)
        {
            using var command = dataSource.CreateCommand(
                $"SELECT * " +
                $"FROM Guardian " +
                $"WHERE Id = {guardianId};"
            );
            var reader = command.ExecuteReader();
            reader.Read();
            DateOnly? endDate = null;
            if (!reader.IsDBNull(3))
                endDate = reader.GetFieldValue<DateOnly>(3);
            Guardian guardian = new Guardian
            {
                Id = reader.GetInt64(0),
                AnimalIds = GetAnimalIdsByGuardian(dataSource, guardianId),
                UserId = reader.GetInt64(1),
                StartDate = reader.GetFieldValue<DateOnly>(2),
                EndDate = endDate
            };
            guardians.Add(guardian);
            
        }

        return guardians;
    }
    public MedicalCard GetMedicalCard(long id)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"SELECT * " +
            $"FROM MedicalCard " +
            $"WHERE AnimalId = {id};\n"
        );
        var reader = command.ExecuteReader();
        reader.Read();
        MedicalCard medicalCard = new MedicalCard
        {
            Id = reader.GetInt64(0),
            AnimalId = reader.GetInt64(1),
            DateOfLastVisit = reader.GetDateTime(2),
            Diagnoses = reader.GetString(3),
	        Therapy =reader.GetString(4)
        };

        return medicalCard;
    }


    public AnimalType GetType(long id)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"SELECT atype.Id, atype.Name " +
            $"FROM Animal a " +
            $"JOIN AnimalType atype ON a.AnimalTypeId = atype.Id " +
            $"WHERE a.Id = {id};"
        );
        var reader = command.ExecuteReader();
        reader.Read();
        AnimalType animalType = new AnimalType
            {
                Id = reader.GetInt64(0),
                Name = reader.GetString(1),
            };

        return animalType;
    }

    public void Update(Animal animal)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        string medicalCardId = animal.MedicalCardId == null? "NULL": animal.MedicalCardId.ToString()!;
        using var command = dataSource.CreateCommand(
            $"UPDATE Animal SET Name = '{animal.Name}', " +
            $"AnimalTypeId = {animal.AnimalTypeId}, " +
            $"AnimalBreedId = {animal.AnimalBreedId}, " +
            $"Gender = {(int)animal.Gender}, Age = {animal.Age}, " +
            $"Weight = {animal.Weight}, " +
            $"Description = '{animal.Description}', " +
            $"MedicalCardId = {medicalCardId} " +
            $"WHERE Id = {animal.Id};"
            );
        command.ExecuteNonQuery();
        List<long> irrelevantGuardianIds = GetGuardianIds(dataSource, animal.Id);
        List<long> removedGuardianIds = irrelevantGuardianIds.Except(animal.GuardianIds).ToList();
        foreach(var removedId in removedGuardianIds)
        {
            using var removeCommand = dataSource.CreateCommand(
                $"DELETE FROM Animal_Guardian WHERE AnimalId = {animal.Id} AND GuardianId = {removedId};\n"
            );
            removeCommand.ExecuteNonQuery();
        }
        List<long> addedGuardianIds = animal.GuardianIds.Except(irrelevantGuardianIds).ToList();
        foreach(var addedId in addedGuardianIds)
        {
            using var addCommand = dataSource.CreateCommand(
                $"INSERT INTO Animal_Guardian (AnimalId, GuardianId) VALUES ({animal.Id}, {addedId});\n"
            );
            addCommand.ExecuteNonQuery();
        }

    }
    private List<long> GetAnimalIdsByGuardian(NpgsqlDataSource dataSource, long id)
    {
        using var command = dataSource.CreateCommand(
            $"SELECT AnimalId FROM Animal_Guardian WHERE GuardianId = {id};" 
            );
        var reader = command.ExecuteReader();
        List<long> animalIds = new List<long>();
        while(reader.Read())
        {
            animalIds.Add(reader.GetInt64(0));
        }
        return animalIds;
    }
    private List<long> GetGuardianIds(NpgsqlDataSource dataSource, long id)
    {
        using var command = dataSource.CreateCommand(
            $"SELECT GuardianId FROM Animal_Guardian WHERE AnimalId = {id};\n" 
            );
        var reader = command.ExecuteReader();
        List<long> guardianIds = new List<long>();
        while(reader.Read())
        {
            guardianIds.Add(reader.GetInt64(0));
        }
        return guardianIds;
    }
}