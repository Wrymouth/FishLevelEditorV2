using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace FishLevelEditor2;

public partial class MessageDialog : Window
{
    public MessageDialog(string message)
    {
        InitializeComponent();
        // TODO configurable icon
        MessageLabel.Content = message;
    }

    // for previewer
    public MessageDialog()
    {
        InitializeComponent();

    }

    private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close();
    }
}