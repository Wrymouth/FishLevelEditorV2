using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace FishLevelEditor2;

public partial class YesNoCancelDialog : Window
{
    public enum DialogResult
    {
        Yes,
        No,
        Cancel
    }
    public DialogResult Result { get; set; } = DialogResult.Cancel;
    public YesNoCancelDialog(string title, string message)
    {
        InitializeComponent();
        Title = title;
        MessageLabel.Content = message;
    }

    // for previewer
    public YesNoCancelDialog()
    {
    }

    public void CloseDialog(DialogResult result)
    {
        Result = result;
        Close();
    }

    private void YesButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        CloseDialog(DialogResult.Yes);
    }

    private void NoButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        CloseDialog(DialogResult.No);

    }
    private void CancelButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        CloseDialog(DialogResult.Cancel);

    }

}