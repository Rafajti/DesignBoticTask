using System.Windows;

namespace DesignBoticUI.Views
{
    /// <summary>
    /// Interaction logic for PropertiesWindow.xaml
    /// </summary>
    public partial class PropertiesWindow : Window
    {
        public PropertiesWindow(List<string> properties)
        {
            InitializeComponent();

            PropertiesTextBox.Text = string.Join("\n\n", properties);
        }
    }
}
