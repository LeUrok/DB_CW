using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
namespace AnimalShelterUI;

public partial class ErrorMesgWindow : Window
{
    public ErrorMesgWindow()
    {
        InitializeComponent();
    }
    public void OkButtonClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.Close();
    }
}
