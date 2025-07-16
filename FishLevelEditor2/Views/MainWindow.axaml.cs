using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Platform;
using Avalonia.Threading;
using FishLevelEditor2.EditorActions;
using FishLevelEditor2.Logic;
using FishLevelEditor2.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using static FishLevelEditor2.Logic.Metatile;

namespace FishLevelEditor2.Views;

public partial class MainWindow : Window
{
    private readonly HashSet<Avalonia.Input.Key> keysHeld = new();
    private readonly DispatcherTimer keyRepeatTimer;

    public MainWindow(int levelIndex)
    {
        InitializeComponent();
        DataContext = new MainViewModel(Session.Project.Levels[levelIndex]);
        MainViewModel mvm = DataContext as MainViewModel;
        mvm.SelectedMetatileViewModel.PropertyChanged += SelectedMetatileViewModel_PropertyChanged;
        mvm.SelectedMetatileViewModel.MetatileIndex = 0; // force a PropertyChanged
        LevelScrollViewer.HorizontalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Hidden;
        LevelScrollViewer.VerticalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Hidden;


        mvm.Repaint += HandleRepaint;
        EditorActionHandler.Log += HandleLog;

        keyRepeatTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(16) // ~60 FPS
        };
        keyRepeatTimer.Tick += OnKeyRepeat;
        keyRepeatTimer.Start();

        SetLevelImageDimensions();

        Repaint();
        SetLogMessage($"Successfully loaded level {mvm.LevelViewModel.Level.Name}");
    }

    private void SelectedMetatileViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        MainViewModel mvm = DataContext as MainViewModel;
        Metatile metatile = mvm.SelectedMetatileViewModel.GetMetatileFromSet(mvm.LevelViewModel.Level.MetatileSet);
        MetatileNameTextBox.Text = metatile.Name;
        MetatileTypeComboBox.SelectedIndex = (int)metatile.Type;
        Repaint();
    }

    private void SetLevelImageDimensions()
    {
        MainViewModel mvm = DataContext as MainViewModel;

        MainLevelBitmap.Height = mvm.LevelViewModel.Level.Height * 16 * 2;
        MainLevelBitmap.Width = mvm.LevelViewModel.Level.Width * 16 * 2;
    }

    private void OnKeyRepeat(object? sender, EventArgs e)
    {
        int moveSpeed = 10;
        if (keysHeld.Contains(Avalonia.Input.Key.LeftShift))
        {
            moveSpeed *= 2;
        }
        if (keysHeld.Contains(Avalonia.Input.Key.A))
        {
            MoveLevelView(-moveSpeed, 0);
        }

        if (keysHeld.Contains(Avalonia.Input.Key.D))
        {
            MoveLevelView(moveSpeed, 0);

        }

        if (keysHeld.Contains(Avalonia.Input.Key.W))
        {
            MoveLevelView(0, -moveSpeed);

        }

        if (keysHeld.Contains(Avalonia.Input.Key.S))
        {
            MoveLevelView(0, moveSpeed);
        }

        // Add more keys/actions as needed
    }

    private void MoveLevelView(int x, int y)
    {
        LevelScrollViewer.Offset = new Vector(LevelScrollViewer.Offset.X + x, LevelScrollViewer.Offset.Y + y);
    }

    private void HandleLog(object sender, EditorActionLogEventArgs e)
    {
        SetLogMessage(e.LogMessage);
    }

    private void HandleRepaint(object sender, EventArgs e)
    {
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
        RepaintLevel();
    }

    private void SetLogMessage(string message)
    {
        LogsLabel.Content = message;
    }

    private void RepaintLevel()
    {
        MainViewModel mvm = DataContext as MainViewModel;
        LevelViewModel lvm = mvm.LevelViewModel;
        lvm.Display();
        SetLevelImageDimensions();
        MainLevelBitmap.Bitmap = lvm.LevelBitmap.Bitmap;
        MainLevelBitmap.InvalidateVisual();
    }

    private void RepaintMetatileSet()
    {
        MainViewModel mvm = (DataContext as MainViewModel);
        MetatileSetViewModel metatileSetViewModel = mvm.MetatileSetViewModel;
        metatileSetViewModel.Display(mvm.LevelViewModel.Level.BackgroundPalettes[0]);
        MetatileSetBitmap.Bitmap = metatileSetViewModel.MetatileSetBitmap.Bitmap;
        MetatileSetBitmap.InvalidateVisual();
    }

    private void RepaintSelectedMetatile()
    {
        MainViewModel mvm = (DataContext as MainViewModel);
        SelectedMetatileViewModel selectedMetatileViewModel = mvm.SelectedMetatileViewModel;
        selectedMetatileViewModel.Display(mvm.LevelViewModel.Level.BackgroundPalettes[0], mvm.LevelViewModel.Level.MetatileSet);
        SelectedMetatileBitmap.Bitmap = selectedMetatileViewModel.SelectedMetatileBitmap.Bitmap;
        SelectedMetatileBitmap.InvalidateVisual();
    }

    private void RepaintCHR()
    {
        MainViewModel mainViewModel = (DataContext as MainViewModel);
        CHRBankViewModel chrBankViewModel = mainViewModel.CHRBankViewModel;
        chrBankViewModel.Display(mainViewModel.LevelViewModel.Level.BackgroundPalettes[0]);
        CHRBitmap.Bitmap = chrBankViewModel.CHRBankBitmap.Bitmap;
        CHRBitmap.InvalidateVisual();
    }

    private void ReplaceCHRButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        LevelSelectDialog levelSelectDialog = new();

    }

    private void AddMetatileButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        MainViewModel mvm = (DataContext as MainViewModel);
        if (mvm.LevelViewModel.Level.MetatileSet.Metatiles.Count >= MetatileSet.MAX_METATILES_IN_SET)
        {
            return;
        }
        EditorActionHandler.Do(new AddMetatileAction(mvm.SelectedMetatileViewModel.MetatileIndex), mvm);
        Repaint();
    }

    private void MetatileNameTextBox_TextChanged(object? sender, Avalonia.Controls.TextChangedEventArgs e)
    {
        MainViewModel mvm = (DataContext as MainViewModel);

        if (mvm.SelectedMetatileViewModel.MetatileIndex >= 0)
        {
            mvm.SelectedMetatileViewModel.GetMetatileFromSet(mvm.LevelViewModel.Level.MetatileSet).Name = MetatileNameTextBox.Text;
        }
    }

    private void MetatileTypeComboBox_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        MainViewModel mvm = (DataContext as MainViewModel);

        if (mvm.SelectedMetatileViewModel.MetatileIndex >= 0)
        {
            mvm.SelectedMetatileViewModel.GetMetatileFromSet(mvm.LevelViewModel.Level.MetatileSet).Type = (MetatileType)MetatileTypeComboBox.SelectedIndex;
        }
    }

    private void CHRBitmap_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        MainViewModel mvm = (DataContext as MainViewModel);

        mvm.CHRBankViewModel.SelectedTileIndex = (uint)mvm.GetMouseTileIndex(e.GetPosition(CHRBitmap), 8, 16, 255);
    }

    private void SelectedMetatileBitmap_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        MainViewModel mvm = (DataContext as MainViewModel);

        uint tileIndex = mvm.GetMouseTileIndex(e.GetPosition(SelectedMetatileBitmap), 8, 2, 3);
        if (mvm.CHRBankViewModel.SelectedTileIndex >= 0)
        {
            CHRBankViewModel cvm = mvm.CHRBankViewModel;
            EditorActionHandler.Do(new SetMetatileTileAction(mvm.SelectedMetatileViewModel.MetatileIndex, (int) tileIndex, mvm.SelectedMetatileViewModel.GetMetatileFromSet(mvm.LevelViewModel.Level.MetatileSet).Tiles[tileIndex], cvm.SelectedTileIndex), mvm);
            Repaint();
        }
    }

    private void MetatileSetBitmap_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        MainViewModel mvm = (DataContext as MainViewModel);
        uint metatileIndex = mvm.GetMouseTileIndex(e.GetPosition(MetatileSetBitmap), 16, 8, (uint) mvm.LevelViewModel.Level.MetatileSet.Metatiles.Count - 1);
        mvm.SelectedMetatileViewModel.MetatileIndex = metatileIndex;
        Repaint();
    }

    private void LevelScrollViewer_KeyDown(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        keysHeld.Add(e.Key);
    }

    private void LevelScrollViewer_KeyUp(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        keysHeld.Remove(e.Key);
    }

    private void AddRowAboveButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        MainViewModel mvm = (DataContext as MainViewModel);
        EditorActionHandler.Do(new AddRowOrColumnAction(AddRowOrColumnAction.AddType.RowsAbove, 1), mvm);
        Repaint();
    }

    private void AddRowBelowButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {

        MainViewModel mvm = (DataContext as MainViewModel);
        EditorActionHandler.Do(new AddRowOrColumnAction(AddRowOrColumnAction.AddType.RowsBelow, 1), mvm);
        Repaint();
    }

    private void AddColumnLeftButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        MainViewModel mvm = (DataContext as MainViewModel);
        EditorActionHandler.Do(new AddRowOrColumnAction(AddRowOrColumnAction.AddType.ColumnsLeft, 1), mvm);
        Repaint();
    }

    private void AddColumnRightButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        MainViewModel mvm = (DataContext as MainViewModel);
        EditorActionHandler.Do(new AddRowOrColumnAction(AddRowOrColumnAction.AddType.ColumnsRight, 1), mvm);
        Repaint();
    }

    private void MainLevelBitmap_PointerMoved(object? sender, Avalonia.Input.PointerEventArgs e)
    {
        MainViewModel mvm = (DataContext as MainViewModel);
        var point = e.GetCurrentPoint(sender as Control);

        if (point.Properties.IsLeftButtonPressed)
        {
            mvm.PlaceMetatileInLevel(e.GetPosition(MainLevelBitmap));
        }
    }

    private void MainLevelBitmap_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        MainViewModel mvm = (DataContext as MainViewModel);
        mvm.PlaceMetatileInLevel(e.GetPosition(MainLevelBitmap));

    }
}
