using Avalonia.Controls;
using Avalonia.Interactivity;
using AnimalShelter.AnimalShelterUI.ViewModels;
using AnimalShelterPostgreSql.Repositories;
using AnimalShelterCore.Repositories;
using AnimalShelterCore.Models;
using System;
namespace AnimalShelterUI;

public partial class UserWindow : Window
{
    public UserWindowViewModel ViewModel { get; }
    public User User { get; }
    public IDataBase Database { get; }
    public UserWindow()
    {
        throw new NotImplementedException();
    }
    public UserWindow(IDataBase database, User user)
    {
        InitializeComponent();
        User = user;
        Database = database;
        ViewModel = new UserWindowViewModel(database);
        DataContext = ViewModel;
    }
    private void ClearUserButton(object? sender, RoutedEventArgs e)
    {
        ClearFields();
    }
    private async void ChangeUserButton(object? sender, RoutedEventArgs e)
    {
        var selectedUser = usersList.SelectedItem as User;
        if (selectedUser == null)
        {
            return;
        }
        ClearFields();
        Employee employee = Database.EmployeeRepository.GetById(selectedUser.EmployeeId.Value);
        jobComboBox.SelectedItem = ConvertJobComboBox(employee.Job);
        nameTextBox.Text = selectedUser.Name;
        phoneTextBox.Text = selectedUser.Phone;
        emailTextBox.Text = selectedUser.Mail;
        _user = selectedUser;
        _employee = employee;
    }
    private void DeleteUserButton(object? sender, RoutedEventArgs e)
    {
        if(usersList.SelectedItem != null)
            ViewModel.DeleteUser(usersList.SelectedIndex, User.Id);
    }
    private void SaveUserButton(object? sender, RoutedEventArgs e)
    {
        var selectedJob = jobComboBox.SelectedItem as string;
        ViewModel.SaveUser(selectedJob, _user, _employee);
        ViewModel.UpdateUsersList();
        ClearFields();
    }
    private void AddUserButton(object? sender, RoutedEventArgs e)
    {
        var selectedJob = jobComboBox.SelectedItem as string;
        ViewModel.AddUser(selectedJob);
        ViewModel.UpdateUsersList();
        ClearFields();
    }
    public void ClearFields()
    {
        nameTextBox.Text = null;
        phoneTextBox.Text = null;
        emailTextBox.Text = null;
        jobComboBox.SelectedItem = null;
    }
    public string ConvertJobComboBox(string job)
    {
        switch (job)
        {
            case "administrator":
                return "Администратор";
            case "vet":
                return "Ветеринар";
            case "assistant":
                return "Ассистент";
            default:
                return "";
        }
    }
    private User? _user;
    private Employee? _employee;
}