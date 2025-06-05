using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
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

    private void LoadProjectButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        // unimplemented, open file dialog
        string filePath = "";
        var opdViewModel = DataContext as OpenProjectDialogViewModel;
        opdViewModel.LoadProject(filePath);
    }
}