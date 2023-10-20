using System.Windows;
using Infections.Controller;

namespace Infections;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new InfectionController(PeopleCanvas);
    }
}