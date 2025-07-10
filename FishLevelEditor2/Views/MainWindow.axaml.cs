using Avalonia.Controls;
using FishLevelEditor2.Logic;
using FishLevelEditor2.ViewModels;
using ReactiveUI;
using System;

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
        RepaintSelectedMetatile();
        RepaintMetatileSet();
    }

    private void RepaintMetatileSet()
    {
        MainViewModel mainViewModel = (DataContext as MainViewModel);
        MetatileSetViewModel metatileSetViewModel = mainViewModel.MetatileSetViewModel;
        metatileSetViewModel.Display(mainViewModel.Level.BackgroundPalettes[0]);
        MetatileSetBitmap.Bitmap = metatileSetViewModel.MetatileSetBitmap.Bitmap;
    }

    private void RepaintSelectedMetatile()
    {
        MainViewModel mainViewModel = (DataContext as MainViewModel);
        SelectedMetatileViewModel selectedMetatileViewModel = mainViewModel.SelectedMetatileViewModel;
        selectedMetatileViewModel.Display(mainViewModel.Level.BackgroundPalettes[0]);
        SelectedMetatileBitmap.Bitmap = selectedMetatileViewModel.SelectedMetatileBitmap.Bitmap;
    }

    public void RepaintCHR()
    {
        MainViewModel mainViewModel = (DataContext as MainViewModel);
        CHRBankViewModel chrBankViewModel = mainViewModel.CHRBankViewModel;
        chrBankViewModel.Display(mainViewModel.Level.BackgroundPalettes[0]);
        CHRBitmap.Bitmap = chrBankViewModel.CHRBankBitmap.Bitmap;
    }

    private void ReplaceCHRButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        LevelSelectDialog levelSelectDialog = new();

    }
}
