using System.ComponentModel;
using AnimalShelterCore.Models;
using AnimalShelterCore.Repositories;
using Npgsql;

namespace AnimalShelterPostgreSql.Repositories;

class GuardianPostgreSql : IGuardianRepository
{
    private string connectionString;

    public GuardianPostgreSql(string connectionString)
    {
        this.connectionString = connectionString;
    }
    public void Add(Guardian guardian)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        string endDate = "NULL";
        if (guardian.EndDate != null)
            endDate = $"\'{guardian.EndDate.Value.ToString("yyyy-MM-dd")}\'";
        using var command = dataSource.CreateCommand(
            $"INSERT INTO Guardian (UserId, StartDate, EndDate) " +
            $"VALUES ({guardian.UserId}, '{guardian.StartDate.ToString("yyyy-MM-dd")}', " +
            $"{endDate}) RETURNING Id;" 
            );
        var reader = command.ExecuteReader();
        reader.Read();
        guardian.Id = reader.GetInt64(0);
        using var com = dataSource.CreateCommand(
            $"UPDATE ShelterUser SET " +
            $"GuardianId = {guardian.Id} " + 
            $"WHERE Id = {guardian.UserId};");
        com.ExecuteNonQuery();
        for(int i = 0; i < guardian.AnimalIds.Count; i++)
        {
            using var command2 = dataSource.CreateCommand(
            $"INSERT INTO Animal_Guardian (AnimalId, GuardianId) VALUES ({guardian.AnimalIds[i]}, {guardian.Id});");
            command2.ExecuteNonQuery();
        }
    }

    public void Delete(long id)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"DELETE FROM Guardian WHERE Id = {id};" 
            );
        command.ExecuteNonQuery();
    }

    public List<Guardian> GetAll()
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"SELECT * FROM Guardian;" 
            );
        var reader = command.ExecuteReader();
        List<Guardian> guardians = new List<Guardian>();
        while(reader.Read())
        {
            List<long> animalIds = GetAnimalIds(dataSource, reader.GetInt64(0));
            DateOnly? endDate = null;
            if (!reader.IsDBNull(3))
                endDate = reader.GetFieldValue<DateOnly>(3);
            guardians.Add(
                new Guardian 
                {
                    Id = reader.GetInt64(0),
                    UserId = reader.GetInt64(1),
                    StartDate = reader.GetFieldValue<DateOnly>(2),
                    EndDate = endDate,
                    AnimalIds = animalIds
                }
            );
        }
        return guardians;
    }

    public List<Animal> GetAnimals(long id)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        List<long> animalIds = GetAnimalIds(dataSource, id);
        List<Animal>  animals = new List<Animal>();
        foreach (var animalId in animalIds)
        {
            using var command = dataSource.CreateCommand(
                $"SELECT * " +
                $"FROM Animal " +
                $"WHERE Id = {animalId};"
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
                GuardianIds = GetGuardianIdsByAnimal(dataSource, animalId)
            };
            animals.Add(animal);
            
        }

        return animals;
    }

    public Guardian GetById(long id)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"SELECT * " +
            $"FROM Guardian " +
            $"WHERE Id = {id};"
        );
        var reader = command.ExecuteReader();
        reader.Read();
        List<long> animalIds = GetAnimalIds(dataSource, reader.GetInt64(0));
        DateOnly? endDate = null;
        if (!reader.IsDBNull(3))
            endDate = reader.GetFieldValue<DateOnly>(3);
        Guardian guardian = new Guardian
        {
            Id = reader.GetInt64(0),
            UserId = reader.GetInt64(1),
            StartDate = reader.GetFieldValue<DateOnly>(2),
            EndDate = endDate,
            AnimalIds = animalIds
        };

        return guardian;
    }

    public User GetUser(long id)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"SELECT * FROM ShelterUser WHERE GuardianId = {id};"
        );
        var reader = command.ExecuteReader();
        reader.Read();
        long? eId = null;
        if (!reader.IsDBNull(1))
            eId = reader.GetInt64(1);
        long? gId = null;
        if (!reader.IsDBNull(2))
            gId = reader.GetInt64(2);
        return new User
        {
            Id = reader.GetInt64(0),
            EmployeeId = eId,
            GuardianId = gId,
            Name = reader.GetString(3),
            Phone = reader.GetString(4),
            Mail = reader.GetString(5)
        };
    }

    public void Update(Guardian guardian)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        string endDate = "NULL";
        if (guardian.EndDate != null)
            endDate = $"\'{guardian.EndDate.Value.ToString("yyyy-MM-dd")}\'";
        using var command = dataSource.CreateCommand(
            $"UPDATE Guardian SET UserId = {guardian.UserId}, " +
            $"StartDate = '{guardian.StartDate.ToString("yyyy-MM-dd")}', " +
            $"EndDate = {endDate} " +
            $"WHERE Id = {guardian.Id};"
            );
        command.ExecuteNonQuery();
        List<long> irrelevantAnimalIds = GetAnimalIds(dataSource, guardian.Id);
        List<long> removedAnimalIds = irrelevantAnimalIds.Except(guardian.AnimalIds).ToList();
        foreach(var removedId in removedAnimalIds)
        {
            using var removeCommand = dataSource.CreateCommand(
                $"DELETE FROM Animal_Guardian WHERE GuardianId = {guardian.Id} AND AnimalId = {removedId};\n"
            );
            removeCommand.ExecuteNonQuery();
        }
        List<long> addedAnimalIds = guardian.AnimalIds.Except(irrelevantAnimalIds).ToList();
        foreach(var addedId in addedAnimalIds)
        {
            using var addCommand = dataSource.CreateCommand(
                $"INSERT INTO Animal_Guardian (AnimalId, GuardianId) VALUES ({addedId}, {guardian.Id});\n"
            );
            addCommand.ExecuteNonQuery();
        }
    }
    private List<long> GetGuardianIdsByAnimal(NpgsqlDataSource dataSource, long id)
    {
        using var command = dataSource.CreateCommand(
            $"SELECT GuardianId FROM Animal_Guardian WHERE AnimalId = {id};" 
            );
        var reader = command.ExecuteReader();
        List<long> guardianIds = new List<long>();
        while(reader.Read())
        {
            guardianIds.Add(reader.GetInt64(0));
        }
        return guardianIds;
    }
    private List<long> GetAnimalIds(NpgsqlDataSource dataSource, long id)
    {
        using var command = dataSource.CreateCommand(
            $"SELECT AnimalId FROM Animal_Guardian WHERE GuardianId = {id};\n" 
            );
        var reader = command.ExecuteReader();
        List<long> animalIds = new List<long>();
        while(reader.Read())
        {
            animalIds.Add(reader.GetInt64(0));
        }
        return animalIds;
    }
    public void AddGuardianToAnimal(long aid, long gid)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var checkCommand = dataSource.CreateCommand(
            $"SELECT COUNT(*) FROM Animal_Guardian WHERE AnimalId = {aid} AND GuardianId = {gid}");

        object? result = checkCommand.ExecuteScalar();
        bool exists = result != null && (long)result > 0;

        if (!exists)
        {
            using var command = dataSource.CreateCommand(
                $"INSERT INTO Animal_Guardian (AnimalId, GuardianId) " +
                $"VALUES ({aid}, {gid});");
            command.ExecuteNonQuery();
        }    
    }
    public void DeleteAnimalGuardian(long aid, long gid)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"DELETE FROM Animal_Guardian " +
            $"WHERE AnimalId = {aid} AND GuardianId = {gid};");
        command.ExecuteNonQuery(); 
    }
}