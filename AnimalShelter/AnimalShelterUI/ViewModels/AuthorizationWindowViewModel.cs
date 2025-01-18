using System;
using System.Collections.ObjectModel;
using AnimalShelterCore.Models;
using AnimalShelterCore.Repositories;
using AnimalShelterPostgreSql.Repositories;
namespace AnimalShelter.AnimalShelterUI.ViewModels;

using Npgsql;

public class AuthorizationWindowViewModel
{
    public string Login { get; set; }
    public string Password { get; set; }
    public bool IsOk { get; set; } = false;
    public User User { get; set; }
    public IDataBase DataBase { get; set; }
    public void UserLogin()
    {
        try
        {
            var role = ExtractRole(Login);
            DataBase = new PostgreSqlDataBase("localhost", 5432, "AnimalShelter", role, Password);
            IsOk = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            IsOk = false;
        }
    }
    public string ExtractRole(string login)
    {
        var db = new PostgreSqlDataBase("localhost", 5432, "AnimalShelter", "anonim", "");
        User = db.UserRepository.GetByName(login);
        Employee employee = db.EmployeeRepository.GetById(User.EmployeeId!.Value);
        
        return employee.Job;
    }
        
}
