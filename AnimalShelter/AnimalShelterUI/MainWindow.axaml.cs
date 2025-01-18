using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
using AnimalShelter.AnimalShelterUI.ViewModels;
using System.Collections.Generic;
using AnimalShelterPostgreSql.Repositories;
using AnimalShelterCore.Repositories;
using AnimalShelterCore.Models;

namespace AnimalShelterUI;

public partial class MainWindow : Window
{
    private readonly MainWindowViewModel _viewModel;
    public IDataBase Database { get; set; }
    public User User { get; set; }
    public MainWindow()
    {
        throw new NotImplementedException();
    }
    public MainWindow(IDataBase database, User user)
    {
        InitializeComponent();
        Database = database;
        User = user;
        _viewModel = new(database);
        DataContext = _viewModel;
        Employee employee = database.EmployeeRepository.GetById(user.EmployeeId.Value);
        if (employee.Job != "administrator")
        {
            employeeButton.IsEnabled = false;
        }
        if (employee.Job == "vet")
        {
            addButton.IsEnabled = false;
            deleteButton.IsEnabled = false;
            changeBreedTypeButton.IsEnabled = false;
            guardianButton.IsEnabled = false;
        }
    }


    private async void AddAnimalButton(object? sender, RoutedEventArgs e)
    {
        var window = new AnimalWindow(Database, User);
        await window.ShowDialog(this);
        _viewModel.UpdateLists();
    }
    private async void ChangeAnimalButton(object? sender, RoutedEventArgs e)
    {
        var selectedAnimal = animalsList.SelectedItem as Animal;
        if (selectedAnimal == null)
        {
            return;
        }
        var window = new AnimalWindow(Database, User, selectedAnimal);
        await window.ShowDialog(this);
        _viewModel.UpdateLists();
    }
    private void DeleteAnimalButton(object? sender, RoutedEventArgs e)
    {
        if(animalsList.SelectedItem != null)
            _viewModel.DeleteAnimal(animalsList.SelectedIndex);
    }

    private void SearchAnimalButton(object? sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(searchTextbox.Text) && 
            typeComboBox.SelectedItem == null && 
            breedComboBox.SelectedItem == null)
        {
            _viewModel.GetAnimalsByName(searchTextbox.Text);
        }
        if (typeComboBox.SelectedItem != null && 
            breedComboBox.SelectedItem == null &&
            string.IsNullOrWhiteSpace(searchTextbox.Text)) 
        {
            var selectedType = typeComboBox.SelectedItem as AnimalType;
            _viewModel.GetAnimalsByType(selectedType!.Id);
        }
        if (breedComboBox.SelectedItem != null &&
            typeComboBox.SelectedItem == null &&
            string.IsNullOrWhiteSpace(searchTextbox.Text))
        {
            var selectedBreed = breedComboBox.SelectedItem as AnimalBreed; 
            _viewModel.GetAnimalsByBreed(selectedBreed!.Id);
        }
        if (breedComboBox.SelectedItem != null && 
            typeComboBox.SelectedItem != null &&
            string.IsNullOrWhiteSpace(searchTextbox.Text))
        {
            var selectedType = typeComboBox.SelectedItem as AnimalType;
            var selectedBreed = breedComboBox.SelectedItem as AnimalBreed; 
            _viewModel.GetAnimalsByBreedAndType(selectedBreed!.Id, selectedType!.Id);
        }
    }
    private void ResetSearchButton(object? sender, RoutedEventArgs e)
    {
        typeComboBox.SelectedItem = null;
        breedComboBox.SelectedItem = null;
        searchTextbox.Text = null;
        _viewModel.UpdateLists();
    }
    private async void ChangeBreedTypeButton(object? sender, RoutedEventArgs e)
    {
        var window = new BreedTypeWindow(Database, User);
        await window.ShowDialog(this);
        _viewModel.UpdateBreedTypeLists();
        _viewModel.UpdateLists();
    }
    private async void EmployeeButton(object? sender, RoutedEventArgs e)
    {
        var window = new UserWindow(Database, User);
        await window.ShowDialog(this);
    }
    private async void GuardianButton(object? sender, RoutedEventArgs e)
    {
        var window = new GuardianWindow(Database, User);
        await window.ShowDialog(this);
    }
}