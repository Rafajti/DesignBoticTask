using System.Windows;

namespace DesignBoticUI.Views;

/// <summary>
/// Interaction logic for ParametersWindow.xaml
/// </summary>
public partial class ParametersWindow : Window
{
    public ParametersWindow(List<KeyValuePair<string, string>> parameters)
    {
        InitializeComponent();

        // Bind parameter list to the DataGrid
        ParameterGrid.ItemsSource = parameters;
    }
}
