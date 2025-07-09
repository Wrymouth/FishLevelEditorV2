using Avalonia.Controls;
using FishLevelEditor2.Logic;
using FishLevelEditor2.ViewModels;
using ReactiveUI;

namespace FishLevelEditor2.Views;

public partial class MainWindow : Window
{
    public MainWindow(int levelIndex)
    {
        InitializeComponent();
        DataContext = new MainViewModel(Session.Project.Levels[levelIndex]);
        Repaint();
    }

    public void Repaint()
    {
        RepaintCHR();
    }

    public void RepaintCHR()
    {
        CHRBankViewModel chrBankViewModel = (DataContext as MainViewModel).CHRBankViewModel;
        chrBankViewModel.Display();
        CHRBitmap.Bitmap = chrBankViewModel.CHRBankBitmap.Bitmap;
    }

    private void ReplaceCHRButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        LevelSelectDialog levelSelectDialog = new();

    }
}
