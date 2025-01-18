using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
using AnimalShelter.AnimalShelterUI.ViewModels;
using AnimalShelterPostgreSql.Repositories;
using AnimalShelterCore.Models;
namespace AnimalShelterUI;

public partial class AuthorizationWindow : Window
{
    private readonly AuthorizationWindowViewModel _viewModel;
    public AuthorizationWindow()
    {
        InitializeComponent();
        var database = new PostgreSqlDataBase("localhost", 5432, "AnimalShelter", "anonim", "");
        database.Initialize();
        _viewModel = new();
        DataContext = _viewModel;
    }
    private async void AuthorizationButton(object? sender, RoutedEventArgs e)
    {    
        _viewModel.UserLogin();
        if (_viewModel.IsOk)
        {
            var mainWindow = new MainWindow(_viewModel.DataBase, _viewModel.User);
            mainWindow.Show();
            this.Close();
        }
        else
        {
            var window = new ErrorMesgWindow();
            await window.ShowDialog(this);    
        }
        
    }
}