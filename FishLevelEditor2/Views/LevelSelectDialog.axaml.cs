using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using FishLevelEditor2.Logic;
using FishLevelEditor2.ViewModels;
using System.IO;

namespace FishLevelEditor2;

public partial class LevelSelectDialog : Window
{
    public LevelSelectDialog()
    {
        InitializeComponent();
        // test level + metatile set until saving/loading a project works
        Session.Project.MetatileSets.Add(new MetatileSet("Test Metatiles"));
        Session.Project.Levels.Add(new("Test", @"C:\Users\jacco\Documenten\Projects\NES\FF-City\ff-city\src\chr\highway.chr", 16, 15, 0));
        // ---
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
            Session.Project.Levels.Add(nldViewModel.Level);
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