using System.ComponentModel;

namespace DesignBoticUI.Models;

public class ElementInformationDto : INotifyPropertyChanged
{
    private string _name;

    public long Id { get; set; }

    public string Name
    {
        get => _name;
        set
        {
            if (_name != value)
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
    }

    public string Category { get; set; }
    public List<KeyValuePair<string, string>> Parameters { get; set; }
    public int ParameterCount => Parameters?.Count ?? 0;

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
