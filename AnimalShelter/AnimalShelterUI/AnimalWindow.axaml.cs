using Avalonia.Controls;
using Avalonia.Interactivity;
using AnimalShelter.AnimalShelterUI.ViewModels;
using AnimalShelterPostgreSql.Repositories;
using Avalonia;
using System;
namespace AnimalShelterUI;
using AnimalShelterCore.Models;
using AnimalShelterCore.Repositories;

public partial class AnimalWindow : Window
{
    public AnimalWindowViewModel ViewModel { get; }
    public User User { get; }
    public IDataBase Database { get; }
    public bool IsVet { get; set; } = false;
    public bool IsAssistent { get; set; } = false;
    public AnimalWindow()
    {
        throw new NotImplementedException();
    }
    public AnimalWindow(IDataBase database, User user)
    {
        InitializeComponent();
        User = user;
        Database = database;
        ViewModel = new AnimalWindowViewModel(database);
        DataContext = ViewModel;
        Employee employee = database.EmployeeRepository.GetById(user.EmployeeId.Value);
        IsAssistent = employee.Job.Equals("assistant", StringComparison.OrdinalIgnoreCase);
        if(IsAssistent)
        {
            dateOfLastVisitTextBox.IsEnabled = false;
            diagnosesTextBox.IsEnabled = false;
            therapylabelTextBox.IsEnabled = false;
        }
        chooseGuardianComboBox.IsEnabled = false;
        addGuardianToAnimalButton.IsEnabled = false;
        removeGuardianButton.IsEnabled = false;

    }
    public AnimalWindow(IDataBase database, User user, Animal animal)
    {
        InitializeComponent();
        User = user;
        Database = database;
        ViewModel = new AnimalWindowViewModel(database, animal);
        DataContext = ViewModel;
        
        Employee employee = database.EmployeeRepository.GetById(user.EmployeeId.Value);
        IsVet = employee.Job.Equals("vet", StringComparison.OrdinalIgnoreCase);
        IsAssistent = employee.Job.Equals("assistant", StringComparison.OrdinalIgnoreCase);
        if (IsVet)
        {
            animalNameTextBox.IsEnabled = false;
            animalTypeComboBox.IsEnabled = false;
            animalBreedComboBox.IsEnabled = false;
            animalMaleRadioButton.IsEnabled = false;
            animalFemaleRadioButton.IsEnabled = false;
            animalAgeTextBox.IsEnabled = false;
            animalWeightTextBox.IsEnabled = false;
            animalDescriptionTextBox.IsEnabled = false;
            chooseGuardianComboBox.IsEnabled = false;
            addGuardianToAnimalButton.IsEnabled = false;
            removeGuardianButton.IsEnabled = false;
        }
        if(IsAssistent)
        {
            dateOfLastVisitTextBox.IsEnabled = false;
            diagnosesTextBox.IsEnabled = false;
            therapylabelTextBox.IsEnabled = false;
        }
    }

    private void SaveAnimalDataButton(object? sender, RoutedEventArgs e)
    {
        ViewModel.Save(IsVet, IsAssistent);
        Close();
    }
    private void CancelAnimalDataButton(object? sender, RoutedEventArgs e)
    {
        Close();
    }
    private void RemoveGuardianButton(object? sender, RoutedEventArgs e)
    {
        if(guardiansList.SelectedItem != null)
            ViewModel.DeleteGuardian(guardiansList.SelectedIndex);
    }
    private void AddGuardianToAnimalButton(object? sender, RoutedEventArgs e)
    {
        if (chooseGuardianComboBox.SelectedItem != null)
        {
            var selectedGuardian = chooseGuardianComboBox.SelectedItem as User;
            ViewModel.AddGuardianToAnimal(selectedGuardian.GuardianId.Value);
            ViewModel.UpdateGuardianList();
        }
    }
}