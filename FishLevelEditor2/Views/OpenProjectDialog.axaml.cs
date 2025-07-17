using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using FishLevelEditor2.ViewModels;

namespace FishLevelEditor2;

public partial class OpenProjectDialog : Window
{
    public OpenProjectDialog()
    {
        InitializeComponent();
    }

    private void NewProjectButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var opdViewModel = DataContext as OpenProjectDialogViewModel;
        opdViewModel.CreateProject();
    }

    private async void LoadProjectButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        // unimplemented, open file dialog
        var opdViewModel = DataContext as OpenProjectDialogViewModel;
        
        // Get top level from the current control. Alternatively, you can use Window reference instead.
        var topLevel = GetTopLevel(this);

        // Start async operation to open the dialog.
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open Project File",
            AllowMultiple = false
        });

        if (files.Count == 1)
        {
            string filePath = files[0].Path.AbsolutePath;
            opdViewModel.LoadProject(filePath);
        }
    }
}