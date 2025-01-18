using System;
using System.Collections.ObjectModel;
using AnimalShelterCore.Models;
using AnimalShelterCore.Repositories;
namespace AnimalShelter.AnimalShelterUI.ViewModels;

public class UserWindowViewModel
{
    public ObservableCollection<User> UsersList { get; set; }
    public string UserName { get; set; } 
    public string UserPhone { get; set; }
    public string UserEmail { get; set; }
    public string? Job { get; set; } = null;
    public ObservableCollection<string> Jobs { get; set; }

    public UserWindowViewModel(IDataBase database)
    {
        _database = database;
        _user = new User();
        _employee = new Employee();
        UserName = string.Empty;
        UserPhone = string.Empty;
        UserEmail = string.Empty;
        UsersList = new ObservableCollection<User>(_database.UserRepository.GetAllUsersWithEmployeeId());
        Jobs = new ObservableCollection<string>
                                                {
                                                    "Администратор",
                                                    "Ветеринар",
                                                    "Ассистент"
                                                };
    }
    public void AddUser(string job)
    {
        _user.Name = UserName;
        _user.Phone = UserPhone;
        _user.Mail = UserEmail;
        _employee.Job = ConvertJob(job);
        _database.UserRepository.Add(_user);
        _employee.UserId = _user.Id;
        _database.EmployeeRepository.Add(_employee);
        
    }
    public void SaveUser(string job, User userCur, Employee employeeCur)
    {
        _user = userCur;
        _employee = employeeCur;
        _user.Name = UserName;
        _user.Phone = UserPhone;
        _user.Mail = UserEmail;
        _employee.Job = ConvertJob(job);
        _database.UserRepository.Update(_user);
        _database.EmployeeRepository.Update(_employee);
    }
    public string ConvertJob(string job)
    {
        switch (job)
        {
            case "Администратор":
                return "administrator";
            case "Ветеринар":
                return "vet";
            case "Ассистент":
                return "assistant";
            default:
                return "";
        }
    }
    internal void UpdateUsersList()
    {
        UsersList.Clear();
        foreach (var user in _database.UserRepository.GetAllUsersWithEmployeeId())
            UsersList.Add(user);
    }
    public void DeleteUser(int index, long notDeleteIndex)
    {
        if(index >= 0 && index < UsersList.Count && UsersList[index].Id != notDeleteIndex)
        {
            _database.UserRepository.Delete(UsersList[index].Id);
            UsersList.RemoveAt(index);
            UpdateUsersList();
        }
    }
    private readonly IDataBase _database;
    private User _user;
    private Employee _employee;
}