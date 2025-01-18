using System;
using System.Collections.ObjectModel;
using AnimalShelterCore.Models;
using AnimalShelterCore.Repositories;
namespace AnimalShelter.AnimalShelterUI.ViewModels;


public class GuardianWindowViewModel
{
    public ObservableCollection<User> GuardiansList { get; set; }
    public string GuardianName { get; set; }
    public string GuardianPhone { get; set; }
    public string GuardianEmail { get; set; }
    public DateOnly StartGuardiansDate { get; set; }
    public DateOnly? EndGuardiansDate { get; set; }
    public GuardianWindowViewModel(IDataBase database)
    {
        _database = database;
        _user = new User();
        _guardian = new Guardian();
        GuardianName = string.Empty;
        GuardianPhone = string.Empty;
        GuardianEmail = string.Empty;
        StartGuardiansDate = DateOnly.FromDateTime(DateTime.Now);
        EndGuardiansDate = null;
        GuardiansList = new ObservableCollection<User>(_database.UserRepository.GetAllUsersWithGuardianId());
    }
    public void AddGuardian()
    {
        _user.Name = GuardianName;
        _user.Phone = GuardianPhone;
        _user.Mail = GuardianEmail;
        _guardian.StartDate = StartGuardiansDate;
        _guardian.EndDate = EndGuardiansDate;
        _database.UserRepository.Add(_user);
        _guardian.UserId = _user.Id;
        _database.GuardianRepository.Add(_guardian);
        
    }
    public void SaveGuardian(User userCur, Guardian guardianCur)
    {
        _user = userCur;
        _guardian = guardianCur;
        _user.Name = GuardianName;
        _user.Phone = GuardianPhone;
        _user.Mail = GuardianEmail;
        _guardian.StartDate = StartGuardiansDate;
        _guardian.EndDate = EndGuardiansDate;
        _database.UserRepository.Update(_user);
        _database.GuardianRepository.Update(_guardian);
    }
    internal void UpdateGuardiansList()
    {
        GuardiansList.Clear();
        foreach (var user in _database.UserRepository.GetAllUsersWithGuardianId())
            GuardiansList.Add(user);
    }
    public void DeleteGuardian(int index)
    {
        if(index >= 0 && index < GuardiansList.Count)
        {
            _database.UserRepository.Delete(GuardiansList[index].Id);
            GuardiansList.RemoveAt(index);
            UpdateGuardiansList();
        }
    }

    private readonly IDataBase _database;
    private User _user;
    private Guardian _guardian;
}