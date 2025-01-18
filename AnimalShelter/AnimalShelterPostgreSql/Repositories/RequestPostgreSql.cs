using AnimalShelterCore.Models;
using AnimalShelterCore.Repositories;
using Npgsql;
namespace AnimalShelterPostgreSql.Repositories;

class RequestPostgreSql : IRequestRepository
{
    private string connectionString;

    public RequestPostgreSql(string connectionString)
    {
        this.connectionString = connectionString;
    }
    public void Add(Request request)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"INSERT INTO Request (UserId, AnimalId, FormId, Status) " +
            $"VALUES ({request.UserId}, {request.AnimalId}, " +
            $"{request.FormId}, '{request.Status}') RETURNING Id;"
        );
        var reader = command.ExecuteReader();
        reader.Read();
        request.Id = reader.GetInt64(0);
    }

    public void Delete(long id)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"DELETE FROM Request WHERE Id = {id};\n" 
            );
        command.ExecuteNonQuery();
    }

    public List<Request> GetAll()
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"SELECT * FROM Request;" 
            );
        var reader = command.ExecuteReader();
        List<Request> requests = new List<Request>();
        while(reader.Read())
        {
            requests.Add(
                new Request 
                {
                    Id = reader.GetInt64(0),
                    UserId = reader.GetInt64(1),
                    AnimalId = reader.GetInt64(2),
                    FormId = reader.GetInt64(3),
                    Status = (Status)reader.GetInt32(4)
                }
            );
        }
        return requests;
    }

    public Animal GetAnimal(long id)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"SELECT a.Id, a.Name, a.AnimalTypeId, a.AnimalBreedId, a.Gender, " +
            $"a.Age, a.Weight, a.Description, a.MedicalCardId " +
            $"FROM Request rec " +
            $"JOIN Animal a ON rec.AnimalId = a.Id " +
            $"WHERE rec.Id = {id};"
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

    public Request GetById(long id)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"SELECT * FROM Request WHERE Id = {id};"
        );
        var reader = command.ExecuteReader();
        reader.Read();

        return new Request 
            {
                Id = reader.GetInt64(0),
                UserId = reader.GetInt64(1),
                AnimalId = reader.GetInt64(2),
                FormId = reader.GetInt64(3),
                Status = (Status)reader.GetInt32(4)
            };
    }

    public User GetUser(long id)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"SELECT su.Id, su.IdEmployee, su.Name, su.Phone, su.Mail " +
            $"FROM Request rec " +
            $"JOIN ShelterUser su ON rec.UserId = su.Id " +
            $"JOIN Animal a ON rec.AnimalId = a.Id " +
            $"WHERE rec.Id = {id};"
        ); 
        var reader = command.ExecuteReader();
        reader.Read();
        User user = new User
        {
            Id = reader.GetInt64(0),
            EmployeeId = reader.GetInt64(1),
            Name = reader.GetString(2),
            Phone = reader.GetString(3),
            Mail = reader.GetString(4)
        };
        return user;
    }

    public void Update(Request request)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"UPDATE Request SET UserId = {request.UserId}, " +
            $"AnimalId = '{request.AnimalId}', " +
            $"FormId = {request.FormId}, " +
            $"Status = '{request.Status}' " +
            $"WHERE Id = {request.Id};"
        );
        command.ExecuteNonQuery();
    }
}
