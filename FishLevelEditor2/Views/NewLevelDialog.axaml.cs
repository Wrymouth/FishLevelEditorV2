using Avalonia.Controls;
using Avalonia.Platform.Storage;
using FishLevelEditor2.Logic;
using FishLevelEditor2.ViewModels;
using System;

namespace FishLevelEditor2;

public partial class NewLevelDialog : Window
{
    public NewLevelDialog()
    {
        InitializeComponent();
        ToolTip.SetShowOnDisabled(CreateLevelButton, true);
    }

    private void DisableCreateLevelButton(string message)
    {
        CreateLevelButton.IsEnabled = false;
        CreateLevelButton.SetValue(ToolTip.TipProperty, message);
    }

    private void ValidateLevelDataInputs()
    {
        if (string.IsNullOrWhiteSpace(NameTextBox.Text))
        {
            DisableCreateLevelButton("Level name cannot be empty");
            return;
        }
        // TODO also check if the CHR file is valid
        if (string.IsNullOrWhiteSpace(FilePathTextBox.Text))
        {
            DisableCreateLevelButton("Level requires CHR data");
            return;
        }
        if (WidthNumberInput.Value is null || !WidthNumberInput.Value.HasValue)
        {
            DisableCreateLevelButton("Width must be a valid value");
            return;
        }
        if (HeightNumberInput.Value is null || !HeightNumberInput.Value.HasValue)
        {
            DisableCreateLevelButton("Height must be a valid value");
            return;
        }

        CreateLevelButton.ClearValue(ToolTip.TipProperty);
        CreateLevelButton.IsEnabled = true;
    }

    private void CreateLevelButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        NewLevelDialogViewModel nldViewModel = DataContext as NewLevelDialogViewModel;

        string levelName = NameTextBox.Text;
        string chrFilePath = FilePathTextBox.Text;
        int initialWidth = decimal.ToInt32((decimal)WidthNumberInput.Value);
        int initialHeight = decimal.ToInt32((decimal)HeightNumberInput.Value);

        int metatileSetIndex = 0;
        if ((bool)NewMetatileSetRadioButton.IsChecked)
        {
            Session.Project.MetatileSets.Add(new MetatileSet($"{levelName} Metatiles"));
            metatileSetIndex = Session.Project.MetatileSets.Count - 1;
        }
        else if ((bool)OldMetatileSetRadioButton.IsChecked)
        {
            throw new NotImplementedException("Reusing metatile sets is unimplemented");
        }

        nldViewModel.CreateLevel(levelName, chrFilePath, initialWidth, initialHeight, metatileSetIndex);
        nldViewModel.Result = NewLevelDialogViewModel.ModalResult.Ok;
        Close();
    }

    private void OldMetatileSetRadioButton_IsCheckedChanged(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        MetatileSetComboBox.IsEnabled = OldMetatileSetRadioButton.IsChecked ?? false;
    }

    private void LevelDialogTextBox_TextChanged(object? sender, TextChangedEventArgs e)
    {
        if (IsInitialized)
        {
            ValidateLevelDataInputs();
        }
    }

    private async void BrowseCHRFileButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        // Get top level from the current control. Alternatively, you can use Window reference instead.
        var topLevel = GetTopLevel(this);

        // Start async operation to open the dialog.
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open CHR file",
            AllowMultiple = false
        });

        if (files.Count == 1)
        {
            FilePathTextBox.Text = files[0].Path.AbsolutePath;
        }
    }

    private void NumericUpDown_ValueChanged(object? sender, NumericUpDownValueChangedEventArgs e)
    {
        if (IsInitialized)
        {
            ValidateLevelDataInputs();
        }
    }
}