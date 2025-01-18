using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using AnimalShelterCore.Models;
using AnimalShelterCore.Repositories;
using Npgsql;

namespace AnimalShelterPostgreSql.Repositories;

class EmployeePostgreSql : IEmpoyeeRepository
{
    private string connectionString;

    public EmployeePostgreSql(string connectionString)
    {
        this.connectionString = connectionString;
    }
    public void Add(Employee employee)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"INSERT INTO Employee (UserId, Job) VALUES " +
            $"({employee.UserId}, " +
            $"'{employee.Job}') RETURNING Id;");
        var reader = command.ExecuteReader();
        reader.Read();
        employee.Id = reader.GetInt64(0);
        using var com = dataSource.CreateCommand(
            $"UPDATE ShelterUser SET " +
            $"IdEmployee = {employee.Id} " + 
            $"WHERE Id = {employee.UserId};");
        com.ExecuteNonQuery();
    }

    public void Delete(long id)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"DELETE FROM Employee WHERE Id = {id};");
        command.ExecuteNonQuery();
    }

    public List<Employee> GetAll()
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand("SELECT * FROM Employee;");
        var reader = command.ExecuteReader();
        List<Employee> employees = new List<Employee>();
        while(reader.Read())
        {
            employees.Add(new Employee
            {
                Id = reader.GetInt64(0),
                UserId = reader.GetInt64(1),
                Job = reader.GetString(5)
            });
        }
        
        return employees;
    }

    public User GetUserByGuardian(long id)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"SELECT * FROM User WHERE EmployeeId = {id};");
        var reader = command.ExecuteReader();
        reader.Read();
        return new User
        {
            Id = reader.GetInt64(0),
            EmployeeId = reader.GetInt64(1),
            Name = reader.GetString(2),
            Phone = reader.GetString(3),
            Mail = reader.GetString(4)
        };
    }
    public Employee GetById(long id)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"SELECT * FROM Employee WHERE Id = {id};");
        var reader = command.ExecuteReader();
        reader.Read();
        return new Employee
        {
            Id = reader.GetInt64(0),
            UserId = reader.GetInt64(1),
            Job = reader.GetString(2)
        };
    }

    public void Update(Employee employee)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"UPDATE Employee SET UserId = {employee.UserId}, " +
            $"Job = '{employee.Job}' " +
            $"WHERE Id = {employee.Id};"
        );
        command.ExecuteNonQuery();
    }
}