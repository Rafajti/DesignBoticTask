using System.Windows;
using System.Windows.Media;

namespace DesignBoticUI.Views
{
    /// <summary>
    /// Interaction logic for ColorPickWindow.xaml
    /// </summary>

    public partial class ColorPickWindow : Window
    {
        public Color SelectedColor { get; private set; }

        public ColorPickWindow()
        {
            InitializeComponent();
        }

        private void OnOkClick(object sender, RoutedEventArgs e)
        {
            SelectedColor = (Color)colorPicker.SelectedColor;
            DialogResult = true;
            Close();
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
