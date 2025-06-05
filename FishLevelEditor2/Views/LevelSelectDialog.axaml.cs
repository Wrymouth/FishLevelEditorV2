using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using FishLevelEditor2.Logic;
using FishLevelEditor2.ViewModels;

namespace FishLevelEditor2;

public partial class LevelSelectDialog : Window
{
    public LevelSelectDialog()
    {
        InitializeComponent();
    }

    private void LoadLevelButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        LevelSelectDialogViewModel lsdViewModel = DataContext as LevelSelectDialogViewModel;
        int selectedLevelIndex = LevelsListBox.SelectedIndex;
        lsdViewModel.LoadLevel(selectedLevelIndex);
    }

    private async void CreateLevelButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        LevelSelectDialogViewModel lsdViewModel = DataContext as LevelSelectDialogViewModel;
        NewLevelDialogViewModel nldViewModel = new();
        NewLevelDialog newLevelDialog = new()
        {
            DataContext = nldViewModel
        };
        await newLevelDialog.ShowDialog(this);
        if (nldViewModel.Result == NewLevelDialogViewModel.ModalResult.Ok)
        {
            lsdViewModel.Project.Levels.Add(nldViewModel.Level);
        }

    }

    private void LevelsListBox_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        if (LevelsListBox.SelectedItem is not null)
        {
            LoadLevelButton.IsEnabled = true;
        }
        else
        {
            LoadLevelButton.IsEnabled = false;
        }
    }
}