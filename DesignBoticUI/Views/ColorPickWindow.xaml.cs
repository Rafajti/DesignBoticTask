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

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            //SelectedColor = ColorPickerControl.SelectedColor ?? Colors.White;
            //DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            //DialogResult = false;
            Close();
        }
    }
}
