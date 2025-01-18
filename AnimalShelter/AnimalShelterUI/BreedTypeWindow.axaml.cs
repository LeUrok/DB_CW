using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
using AnimalShelter.AnimalShelterUI.ViewModels;
using AnimalShelterPostgreSql.Repositories;
using AnimalShelterCore.Models;
using AnimalShelterCore.Repositories;
namespace AnimalShelterUI;

public partial class BreedTypeWindow : Window
{
    public BreedTypeWindowViewModel ViewModel { get; }
    public User User { get; }
    public IDataBase Database { get; }
    public BreedTypeWindow()
    {
        throw new NotImplementedException();
    }
    public BreedTypeWindow(IDataBase database, User user)
    {
        InitializeComponent();
        Database = database;
        User = user;
        ViewModel = new BreedTypeWindowViewModel(database);
        DataContext = ViewModel;
    }
    private void AddTypeButton(object? sender, RoutedEventArgs e)
    {
        ViewModel.AddType();
        typeTextBox.Text = string.Empty;
    }
    private void RemoveTypeButton(object? sender, RoutedEventArgs e)
    {
        if(typesList.SelectedItem != null)
            ViewModel.DeleteType(typesList.SelectedIndex);
    }

    private void AddBreedButton(object? sender, RoutedEventArgs e)
    {
        ViewModel.AddBreed();
        breedTextBox.Text = string.Empty;
    }
    private void RemoveBreedButton(object? sender, RoutedEventArgs e)
    {
         if(breedsList.SelectedItem != null)
            ViewModel.DeleteBreed(breedsList.SelectedIndex);
    }
}