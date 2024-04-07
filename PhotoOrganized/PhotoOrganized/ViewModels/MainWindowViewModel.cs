using CommunityToolkit.Mvvm.ComponentModel;
using Windows.Storage;

namespace PhotoOrganized.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private StorageFolder? _inputFolder;

    [ObservableProperty]
    private StorageFolder? _outputFolder;

    public MainWindowViewModel()
    {
        
    }
}
