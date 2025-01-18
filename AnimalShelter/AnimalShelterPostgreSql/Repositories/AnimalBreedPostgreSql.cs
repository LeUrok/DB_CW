using System.Data;
using AnimalShelterCore.Models;
using AnimalShelterCore.Repositories;
using Npgsql;

namespace AnimalShelterPostgreSql.Repositories;

class AnimalBreedPostgreSql : IAnimalBreedRepository
{
    private string connectionString;
    public AnimalBreedPostgreSql(string connectionString)
    {
        this.connectionString = connectionString;
    }
    public void Add(AnimalBreed animalBreed)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"INSERT INTO AnimalBreed (Name) VALUES " +
            $"('{animalBreed.Name}') RETURNING Id;");
        var reader = command.ExecuteReader();
        reader.Read();
        animalBreed.Id = reader.GetInt64(0);
    }

    public void Delete(long id)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"DELETE FROM AnimalBreed WHERE Id = {id};\n" 
            );
        command.ExecuteNonQuery();
    }

    public List<AnimalBreed> GetAll()
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            "SELECT * FROM AnimalBreed;"
            );
        var reader = command.ExecuteReader();
        List<AnimalBreed> animalBreeds = new List<AnimalBreed>();
        while(reader.Read())
        {
            animalBreeds.Add(new AnimalBreed
            {
                Id = reader.GetInt64(0),
                Name = reader.GetString(1)
            });
        }
    
        return animalBreeds;
    }

    public AnimalBreed GetById(long id)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"SELECT * FROM AnimalBreed WHERE Id = {id};"
        );
        var reader = command.ExecuteReader();
        reader.Read();

        return new AnimalBreed
        {
            Id = reader.GetInt64(0),
            Name = reader.GetString(1)
        };
    }

    public void Update(AnimalBreed animalBreed)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"UPDATE AnimalBreed SET Name = {animalBreed.Id}, " +
            $"WHERE Id = {animalBreed.Id};"
        );
        command.ExecuteNonQuery();
    }
}