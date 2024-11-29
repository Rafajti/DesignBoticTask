using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using DesignBoticUI.Handlers;
using DesignBoticUI.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DesignBoticUI.Views
{
    /// <summary>
    /// Interaction logic for ShowModelWindow.xaml
    /// </summary>
    public partial class ShowModelWindow : Window
    {
        private readonly List<ElementInformationDto> _allElements;
        private List<ElementInformationDto> _filteredElements;
        private readonly ExternalEvent _updateEvent;
        private readonly UpdateElementHandler _updateHandler;
        private readonly Dictionary<long, string> _modifiedNames = new();

        public ShowModelWindow(List<ElementInformationDto> elements)
        {
            InitializeComponent();

            _allElements = elements;
            _filteredElements = elements;

            _updateHandler = new UpdateElementHandler();
            _updateEvent = ExternalEvent.Create(_updateHandler);

            ElementInfoGrid.ItemsSource = elements;

            // Monitor changes in the grid
            foreach (var element in elements)
            {
                element.PropertyChanged += OnElementPropertyChanged;
            }

            var categories = elements
                .Select(e => e.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            categories.Insert(0, "All Categories");
            CategoryComboBox.ItemsSource = categories;
            CategoryComboBox.SelectedIndex = 0; 

            UpdateElementGrid();
        }

        private void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender is ElementInformationDto dto && e.PropertyName == nameof(dto.Name))
            {
                _modifiedNames[dto.Id] = dto.Name;
            }
        }

        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedCategory = CategoryComboBox.SelectedItem as string;

    
            if (selectedCategory == "All Categories")
            {
                _filteredElements = _allElements;
            }
            else
            {
                _filteredElements = _allElements
                    .Where(e => e.Category == selectedCategory)
                    .ToList();
            }

            UpdateElementGrid();
        }

        private void UpdateElementGrid()
        {
            ElementInfoGrid.ItemsSource = null; 
            ElementInfoGrid.ItemsSource = _filteredElements; 
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _updateHandler.SetAction(app =>
            {
                Document doc = app.ActiveUIDocument.Document;

                using (Transaction trans = new Transaction(doc, "Update Element Names"))
                {
                    trans.Start();

                    foreach (var kvp in _modifiedNames)
                    {
                        Element element = doc.GetElement(new ElementId(kvp.Key));
                        if (element != null && element.Name != kvp.Value)
                        {
                            element.Name = kvp.Value;
                        }
                    }

                    trans.Commit();
                }

                _modifiedNames.Clear();
            });

            _updateEvent.Raise();
        }

        private void ElementInfoGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ElementInfoGrid.SelectedItem is ElementInformationDto selectedElement)
            {
                ParametersWindow detailsWindow = new ParametersWindow(selectedElement.Parameters);
                detailsWindow.ShowDialog();
            }
        }

        private void ParametersColumn_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBlock textBlock && textBlock.DataContext is ElementInformationDto dto)
            {
                var parameterWindow = new ParametersWindow(dto.Parameters);
                parameterWindow.ShowDialog();
            }
        }
    }
}
