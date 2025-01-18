using Avalonia.Controls;
using Avalonia.Interactivity;
using AnimalShelterPostgreSql.Repositories;
using AnimalShelterCore.Repositories;
using AnimalShelterCore.Models;
using AnimalShelter.AnimalShelterUI.ViewModels;
namespace AnimalShelterUI;
using System;

public partial class GuardianWindow : Window
{
    public GuardianWindowViewModel ViewModel { get; }
    public User User { get; }
    public IDataBase Database { get; }
    public GuardianWindow()
    {
        throw new NotImplementedException();
    }
    public GuardianWindow(IDataBase database, User user)
    {
        InitializeComponent();
        User = user;
        Database = database;
        ViewModel = new GuardianWindowViewModel(database);
        DataContext = ViewModel;
    }
    private void AddGuardianButton(object? sender, RoutedEventArgs e)
    {
        ViewModel.AddGuardian();
        ViewModel.UpdateGuardiansList();
        ClearFields();
    }
    public void ClearFields()
    {
        nameTextBox.Text = null;
        phoneTextBox.Text = null;
        emailTextBox.Text = null;
    }
    private void ChangeGuardianButton(object? sender, RoutedEventArgs e)
    {
        var selectedUser = guardiansList.SelectedItem as User;
        if (selectedUser == null)
        {
            return;
        }
        ClearFields();
        Guardian guardian = Database.GuardianRepository.GetById(selectedUser.GuardianId.Value);
        nameTextBox.Text = selectedUser.Name;
        phoneTextBox.Text = selectedUser.Phone;
        emailTextBox.Text = selectedUser.Mail;
        startGuardiansDate.Text = guardian.StartDate.ToString();
        endGuardiansDate.Text = guardian.EndDate.ToString();
        _user = selectedUser;
        _guardian = guardian;
    }
    private void DeleteGuardianButton(object? sender, RoutedEventArgs e)
    {
        if(guardiansList.SelectedItem != null)
            ViewModel.DeleteGuardian(guardiansList.SelectedIndex);
    }
    private void SaveGuardianButton(object? sender, RoutedEventArgs e)
    {
        ViewModel.SaveGuardian(_user, _guardian);
        ViewModel.UpdateGuardiansList();
        ClearFields();
    }
    private void ClearGuardianButton(object? sender, RoutedEventArgs e)
    {
        ClearFields();
    }
    private User? _user;
    private Guardian? _guardian;
}