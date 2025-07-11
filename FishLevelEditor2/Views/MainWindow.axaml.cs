using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Platform;
using FishLevelEditor2.Logic;
using FishLevelEditor2.ViewModels;
using ReactiveUI;
using System;
using System.Diagnostics;

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

    private void Repaint()
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

    private void RepaintCHR()
    {
        MainViewModel mainViewModel = (DataContext as MainViewModel);
        CHRBankViewModel chrBankViewModel = mainViewModel.CHRBankViewModel;
        chrBankViewModel.Display(mainViewModel.Level.BackgroundPalettes[0]);
        CHRBitmap.Bitmap = chrBankViewModel.CHRBankBitmap.Bitmap;
        CHRBitmap.InvalidateVisual();
    }

    private int GetMouseTileIndex(Point mousePos, int tileSize, int tilesPerRow, int maxTileIndex)
    {
        const int BITMAP_SCALE = 2;
        int posX = (int)mousePos.X / BITMAP_SCALE;
        int posY = (int)mousePos.Y / BITMAP_SCALE;
        int tileIndex = posY / tileSize * tilesPerRow + (posX / tileSize);
        if (tileIndex > maxTileIndex)
        {
            tileIndex = maxTileIndex;
        }
        return tileIndex;
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

    private void CHRBitmap_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        MainViewModel mainViewModel = (DataContext as MainViewModel);

        mainViewModel.CHRBankViewModel.SelectedTileIndex = (uint) GetMouseTileIndex(e.GetPosition(CHRBitmap), 8, 16, 255);
    }
}
