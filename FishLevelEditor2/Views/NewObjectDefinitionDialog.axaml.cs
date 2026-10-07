using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using FishLevelEditor2.Logic;
using FishLevelEditor2.ViewModels;

namespace FishLevelEditor2;

public partial class NewObjectDefinitionDialog : Window
{
    public NewObjectDefinitionDialog()
    {
        InitializeComponent();
    }

    private void NameTextBox_TextChanged(object? sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameTextBox.Text))
        {
            CreateObjectDefinitionButton.IsEnabled = false;
            CreateObjectDefinitionButton.SetValue(ToolTip.TipProperty, "Object name cannot be empty");
            return;
        } else
        {
            CreateObjectDefinitionButton.ClearValue(ToolTip.TipProperty);
            CreateObjectDefinitionButton.IsEnabled = true;
        }
    }

    private void SpritePathTextBox_TextChanged(object? sender, TextChangedEventArgs e)
    {
        // TODO add placeholder sprite for both "object with null sprite" and "object with sprite file not found"
    }

    private void BrowseSpriteFileButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
    }

    private void CreateObjectDefinitionButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var viewModel = DataContext as NewObjectDefinitionDialogViewModel;
        var type = (bool)TypeCheckBox.IsChecked ? LevelObjectDefinition.ObjectTypes.Small : LevelObjectDefinition.ObjectTypes.Regular;
        viewModel.CreateLevelObjectDefinition(type, NameTextBox.Text, VarNameTextBox.Text ?? "Var", SpritePathTextBox.Text);
        Close();
    }

    private void VarNameTextBox_TextChanged(object? sender, TextChangedEventArgs e)
    {
    }
}