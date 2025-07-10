using Avalonia;
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

    // constructor for preview window
    public MainWindow()
    {
        InitializeComponent();
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
        MetatileSetBitmap.InvalidateVisual();
    }

    private void RepaintSelectedMetatile()
    {
        MainViewModel mainViewModel = (DataContext as MainViewModel);
        SelectedMetatileViewModel selectedMetatileViewModel = mainViewModel.SelectedMetatileViewModel;
        selectedMetatileViewModel.Display(mainViewModel.Level.BackgroundPalettes[0]);
        SelectedMetatileBitmap.Bitmap = selectedMetatileViewModel.SelectedMetatileBitmap.Bitmap;
        SelectedMetatileBitmap.InvalidateVisual();
    }

    public void RepaintCHR()
    {
        MainViewModel mainViewModel = (DataContext as MainViewModel);
        CHRBankViewModel chrBankViewModel = mainViewModel.CHRBankViewModel;
        chrBankViewModel.Display(mainViewModel.Level.BackgroundPalettes[0]);
        CHRBitmap.Bitmap = chrBankViewModel.CHRBankBitmap.Bitmap;
        CHRBitmap.InvalidateVisual();
    }

    private void ReplaceCHRButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        LevelSelectDialog levelSelectDialog = new();

    }

    private void AddMetatileButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        MainViewModel mainViewModel = (DataContext as MainViewModel);
        mainViewModel.Level.MetatileSet.Metatiles.Add(new Metatile("", Metatile.MetatileType.Air, 0,0,0,0));
        Repaint();
    }

    private void MetatileNameTextBox_TextChanged(object? sender, Avalonia.Controls.TextChangedEventArgs e)
    {
        MainViewModel mainViewModel = (DataContext as MainViewModel);

        if (mainViewModel.SelectedMetatileViewModel.Metatile is not null)
        {
            mainViewModel.SelectedMetatileViewModel.Metatile.Name = MetatileNameTextBox.Text;
        }
    }

    private void MetatileTypeComboBox_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        MainViewModel mainViewModel = (DataContext as MainViewModel);

        if (mainViewModel.SelectedMetatileViewModel.Metatile is not null)
        {
            mainViewModel.SelectedMetatileViewModel.Metatile.Type = (Metatile.MetatileType)MetatileTypeComboBox.SelectedIndex; ;
        }
    }
}
