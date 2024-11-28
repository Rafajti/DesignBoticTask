using DesignBoticUI.Models;
using System.Windows;

namespace DesignBoticUI.Views;

/// <summary>
/// Interaction logic for PropertiesWindow.xaml
/// </summary>
public partial class PropertiesWindow : Window
{
    public PropertiesWindow(List<ElementProperties> elementProperties)
    {
        InitializeComponent();

        // Flatten properties into a list of key-value pairs
        var propertiesToDisplay = elementProperties
            .SelectMany(e => e.Parameters.Select(p => new KeyValuePair<string, string>(
                $"{e.Name} (ID: {e.Id}) - {p.Key}",
                p.Value)))
            .ToList();

        PropertiesDataGrid.ItemsSource = propertiesToDisplay;
    }
}
