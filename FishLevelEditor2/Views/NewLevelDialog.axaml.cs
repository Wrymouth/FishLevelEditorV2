using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using FishLevelEditor2.Logic;
using FishLevelEditor2.ViewModels;
using System;
using System.Linq;

namespace FishLevelEditor2;

public partial class NewLevelDialog : Window
{
    public NewLevelDialog()
    {
        InitializeComponent();
        ToolTip.SetShowOnDisabled(CreateLevelButton, true);
    }

    private void CreateLevelButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        NewLevelDialogViewModel nldViewModel = DataContext as NewLevelDialogViewModel;
        string levelName = NameTextBox.Text;
        int metatileSetIndex = 0;
        if ((bool)NewMetatileSetRadioButton.IsChecked)
        {
            Session.Project.MetatileSets.Add(new MetatileSet($"{levelName} Metatiles"));
            metatileSetIndex = Session.Project.MetatileSets.Count - 1;
        }
        else if ((bool)OldMetatileSetRadioButton.IsChecked)
        {

        }
        nldViewModel.CreateLevel(levelName, (Level.LevelType)LevelTypeComboBox.SelectedIndex, metatileSetIndex);
        nldViewModel.Result = NewLevelDialogViewModel.ModalResult.Ok;
        Close();
    }

    private void OldMetatileSetRadioButton_IsCheckedChanged(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        MetatileSetComboBox.IsEnabled = OldMetatileSetRadioButton.IsChecked ?? false;
    }

    private void NameTextBox_TextChanged(object? sender, Avalonia.Controls.TextChangedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(NameTextBox.Text))
        {
            CreateLevelButton.IsEnabled = true;
            CreateLevelButton.ClearValue(ToolTip.TipProperty);
        } else
        {
            CreateLevelButton.IsEnabled = false;
            CreateLevelButton.SetValue(ToolTip.TipProperty, "Level name cannot be empty");
        }
    }
}