using AnimalShelterCore.Models;
using AnimalShelterCore.Repositories;
using Npgsql;
namespace AnimalShelterPostgreSql.Repositories;

class UserPostgreSql : IUserRepository
{
    private string connectionString;

    public UserPostgreSql(string connectionString)
    {
        this.connectionString = connectionString;
    }
    public void Add(User user)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        string eId = "NULL";
        string gId = "NULL";
        if (user.EmployeeId != null)
            eId = user.EmployeeId.ToString()!;
        if (user.GuardianId != null)
            gId = user.GuardianId.ToString()!;
        using var command = dataSource.CreateCommand(
            $"INSERT INTO ShelterUser (IdEmployee, GuardianId, Name, Phone, Mail) " +
            $"VALUES ({eId}, {gId}, '{user.Name}', " +
            $"'{user.Phone}', '{user.Mail}') RETURNING Id;"
        );
        var reader = command.ExecuteReader();
        reader.Read();
        user.Id = reader.GetInt64(0);
    }

    public void Delete(long id)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"DELETE FROM ShelterUser WHERE Id = {id};\n" 
            );
        command.ExecuteNonQuery();
    }

    public List<User> GetAll()
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"SELECT * FROM ShelterUser;" 
            );
        var reader = command.ExecuteReader();
        List<User> users = new List<User>();
        while(reader.Read())
        {
            long? eId = null;
            if (!reader.IsDBNull(1))
                eId = reader.GetInt64(1);
            long? gId = null;
            if (!reader.IsDBNull(2))
                gId = reader.GetInt64(2);
            users.Add(
                new User 
                {
                    Id = reader.GetInt64(0),
                    EmployeeId = eId,
                    GuardianId = gId,
                    Name = reader.GetString(3),
                    Phone = reader.GetString(4),
                    Mail = reader.GetString(5)
                }
            );
        }
        return users;
    }
    public List<User> GetAllUsersWithEmployeeId()
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            "SELECT * FROM ShelterUser WHERE IdEmployee IS NOT NULL;" 
            );
        var reader = command.ExecuteReader();
        List<User> users = new List<User>();
        while(reader.Read())
        {
            long? eId = null;
            if (!reader.IsDBNull(1))
                eId = reader.GetInt64(1);
            long? gId = null;
            if (!reader.IsDBNull(2))
                gId = reader.GetInt64(2);
            users.Add(
                new User 
                {
                    Id = reader.GetInt64(0),
                    EmployeeId = eId,
                    GuardianId = gId,
                    Name = reader.GetString(3),
                    Phone = reader.GetString(4),
                    Mail = reader.GetString(5)
                }
            );
        }
        return users;
    }
    public List<User> GetAllUsersWithGuardianId()
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            "SELECT * FROM ShelterUser WHERE GuardianId IS NOT NULL;" 
            );
        var reader = command.ExecuteReader();
        List<User> users = new List<User>();
        while(reader.Read())
        {
            long? eId = null;
            if (!reader.IsDBNull(1))
                eId = reader.GetInt64(1);
            long? gId = null;
            if (!reader.IsDBNull(2))
                gId = reader.GetInt64(2);
            users.Add(
                new User 
                {
                    Id = reader.GetInt64(0),
                    EmployeeId = eId,
                    GuardianId = gId,
                    Name = reader.GetString(3),
                    Phone = reader.GetString(4),
                    Mail = reader.GetString(5)
                }
            );
        }
        return users;
    }

    public User GetById(long id)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"SELECT * FROM ShelterUser WHERE Id = {id};"
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
    public User GetByName(string name)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"SELECT * FROM ShelterUser WHERE Name = '{name}';"
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

    public void Update(User user)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        string eId = "NULL";
        string gId = "NULL";
        if (user.EmployeeId != null)
            eId = user.EmployeeId.ToString()!;
        if (user.GuardianId != null)
            gId = user.GuardianId.ToString()!;
        using var command = dataSource.CreateCommand(
            $"UPDATE ShelterUser SET IdEmployee = {eId}, " +
            $"GuardianId = {gId}, Name = '{user.Name}', " +
            $"Phone = '{user.Phone}', " +
            $"Mail = '{user.Mail}' " +
            $"WHERE Id = {user.Id};"
        );
        command.ExecuteNonQuery();
    }
}
