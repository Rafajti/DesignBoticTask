using System.Windows;

namespace DesignBoticUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(List<string> properties)
        {
            InitializeComponent();

            PropertiesTextBox.Text = string.Join("\n\n", properties);
        }
    }
}