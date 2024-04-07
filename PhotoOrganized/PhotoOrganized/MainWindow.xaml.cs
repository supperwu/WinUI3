using System;
using System.Globalization;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PhotoOrganized.ViewModels;
using Windows.Storage;
using Windows.Storage.Pickers;


namespace PhotoOrganized;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        this.InitializeComponent();
        Title = "Photo Organizer";
        ExtendsContentIntoTitleBar = true;
        SetTitleBar(TitleBar);

        ViewModel = Ioc.Default.GetService<MainWindowViewModel>();
        UpdateExample();
    }

    public MainWindowViewModel? ViewModel { get; set; }
    public StorageFolder? SelectedInputFolder { get; private set; }
    public StorageFolder? SelectedOutputFolder { get; private set; }


    private async void StartButton_Click(object sender, RoutedEventArgs e)
    {
        var result = await StartSettingDialog.ShowAsync();
        if (result is ContentDialogResult.Primary && ViewModel is not null)
        {
            ViewModel.InputFolder = SelectedInputFolder;
            ViewModel.OutputFolder = SelectedOutputFolder;
        }
    }

    private async Task<StorageFolder?> SelectedFolderAsync()
    {
        FolderPicker picker = new();
        picker.FileTypeFilter.Add("*");
        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
        WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);
        return await picker.PickSingleFolderAsync();
    }

    private async void SelectedInputFolderButton_Click(object sender, RoutedEventArgs e)
    {
        StorageFolder? folder = await SelectedFolderAsync();
        if (folder is not null && ViewModel is not null)
        {
            SelectedInputFolder = folder;
            SelectedInputFolderTextBox.Text = folder.Path;
        }
    }

    private async void SelectedOutputFolderButton_Click(object sender, RoutedEventArgs e)
    {
        StorageFolder? folder = await SelectedFolderAsync();
        if (folder is not null && ViewModel is not null)
        {
            SelectedOutputFolder = folder;
            SelectedOutputFolderTextBox.Text = folder.Path;
        }
    }

    private void CheckBox_Click(object sender, RoutedEventArgs e) => UpdateExample();

    private void UpdateExample()
    {
        string example = @"[Output]";
        if (SelectedOutputFolder?.Path.Length > 0)
        {
            example = SelectedOutputFolder.Path;
        }

        string format = CreateFormat();
        if (format.Length > 0)
        {
            example += DateTime.Now.ToString(format, CultureInfo.InvariantCulture);
        }
        example += @"\[Filename]";
        ExampleTextBlock.Text = example;
    }

    private string CreateFormat()
    {
        string format =string.Empty;
        if (YearCheckBox.IsChecked is true)
            format += @"\\yyyy";
        if (MonthCheckBox.IsChecked is true)
            format += @"\\MM";
        if (DayCheckBox.IsChecked is true)
            format += @"\\dd";
        if (DateCheckBox.IsChecked is true)
            format += @"\\yyyy-MM-dd";
        return format;
    }
}
