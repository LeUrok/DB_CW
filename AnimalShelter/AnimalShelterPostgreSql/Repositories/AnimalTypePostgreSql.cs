using System.Data;
using AnimalShelterCore.Models;
using AnimalShelterCore.Repositories;
using Npgsql;

namespace AnimalShelterPostgreSql.Repositories;

class AnimalTypePostgreSql : IAnimalTypeRepository
{
    private string connectionString;
    public AnimalTypePostgreSql(string connectionString)
    {
        this.connectionString = connectionString;
    }
    public void Add(AnimalType animalType)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"INSERT INTO AnimalType (Name) VALUES " +
            $"('{animalType.Name}') RETURNING Id;");
        var reader = command.ExecuteReader();
        reader.Read();
        animalType.Id = reader.GetInt64(0);
    }

    public void Delete(long id)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"DELETE FROM AnimalType WHERE Id = {id};\n" 
            );
        command.ExecuteNonQuery();
    }

    public List<AnimalType> GetAll()
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            "SELECT * FROM AnimalType;"
            );
        var reader = command.ExecuteReader();
        List<AnimalType> animalTypes = new List<AnimalType>();
        while(reader.Read())
        {
            animalTypes.Add(new AnimalType
            {
                Id = reader.GetInt64(0),
                Name = reader.GetString(1)
            });
        }
    
        return animalTypes;
    }

    public AnimalType GetById(long id)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"SELECT * FROM AnimalType WHERE Id = {id};"
        );
        var reader = command.ExecuteReader();
        reader.Read();

        return new AnimalType
        {
            Id = reader.GetInt64(0),
            Name = reader.GetString(1)
        };
    }

    public void Update(AnimalType animalType)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"UPDATE AnimalType SET Name = {animalType.Id}, " +
            $"WHERE Id = {animalType.Id};"
        );
        command.ExecuteNonQuery();
    }
}