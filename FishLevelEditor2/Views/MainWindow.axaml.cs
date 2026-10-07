using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Skia;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using FishLevelEditor2.EditorActions;
using FishLevelEditor2.Logic;
using FishLevelEditor2.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using static FishLevelEditor2.Logic.Metatile;

namespace FishLevelEditor2.Views;

public partial class MainWindow : Window
{
    private readonly HashSet<Avalonia.Input.Key> keysHeld = new();
    private readonly DispatcherTimer keyRepeatTimer;

    public WindowManager.NextWindowType NextWindow { get; set; }

    public MainWindow(int levelIndex)
    {
        InitializeComponent();

        Closing += MainWindow_Closing;
        DataContext = new MainViewModel(Session.Project.Levels[levelIndex]);
        MainViewModel mvm = DataContext as MainViewModel;
        MainLevelBitmap.LevelViewModel = mvm.LevelViewModel;
        ObjectsBitmap.ObjectsViewModel = mvm.ObjectsViewModel;
        mvm.SelectedMetatileViewModel.PropertyChanged += SelectedMetatileViewModel_PropertyChanged;
        mvm.SelectedMetatileViewModel.MetatileIndex = 0; // force a PropertyChanged
        LevelScrollViewer.HorizontalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Hidden;
        LevelScrollViewer.VerticalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Hidden;

        Loaded += MainWindow_Loaded;

        mvm.Repaint += HandleRepaint;
        mvm.OpenSaveAs += HandleSaveAs;
        mvm.OpenExport += HandleExport;

        EditorActionHandler.Log += HandleLog;
        EditorActionHandler.UnsavedChangesEvent += HandleUnsavedChanges;

        keyRepeatTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(16) // ~60 FPS
        };
        keyRepeatTimer.Tick += OnKeyRepeat;
        keyRepeatTimer.Start();

        SetLevelImageDimensions();

        SetLogMessage($"Successfully loaded level {mvm.LevelViewModel.Level.Name}");
    }

    private void MainWindow_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        MainViewModel mvm = DataContext as MainViewModel;
        try
        {
            mvm.LevelViewModel.Level.BackgroundCHR.Decode();
        }
        catch (FileNotFoundException fe)
        {
            MessageDialog chrErrorDialog = new MessageDialog("CHR file not found!\nClick \"Replace\" in the main editor to\nselect a new one.");
            chrErrorDialog.Title = "File not found";
            chrErrorDialog.ShowDialog(this);
        }
        catch (DirectoryNotFoundException de)
        {
            MessageDialog chrErrorDialog = new MessageDialog("CHR file not found! Click \"Replace\" in the main editor to select a new one.");
            chrErrorDialog.Title = "File not found";
            chrErrorDialog.ShowDialog(this);
        }
        Repaint();
    }

    private async void OpenLevelCHRFilePicker(Level level)
    {
        // Get top level from the current control. Alternatively, you can use Window reference instead.
        var topLevel = GetTopLevel(this);

        var filter = new FilePickerFileType("CHR Files");
        filter.Patterns = new[] { "*.chr" };

        // Start async operation to open the dialog.
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open CHR file",
            AllowMultiple = false,
            FileTypeFilter = new[] { filter }
        });

        if (files.Count > 0)
        {
            level.BackgroundCHR = new(files[0].Path.AbsolutePath);
        }
        Repaint();
    }

    // constructor for preview window
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void MainWindow_Closing(object? sender, WindowClosingEventArgs e)
    {
        MainViewModel mvm = DataContext as MainViewModel;
        if (EditorActionHandler.UnsavedChanges)
        {
            e.Cancel = true;
            var unsavedChangesDialog = new YesNoCancelDialog("Unsaved changes", "There are unsaved changes. Save before closing?");
            await unsavedChangesDialog.ShowDialog(this);
            switch (unsavedChangesDialog.Result)
            {
                case YesNoCancelDialog.DialogResult.Yes:
                    mvm.Save();
                    Closing -= MainWindow_Closing;
                    Close();
                    break;
                case YesNoCancelDialog.DialogResult.No:
                    Closing -= MainWindow_Closing;
                    Close();
                    break;
                case YesNoCancelDialog.DialogResult.Cancel:
                    e.Cancel = true;
                    break;
                default:
                    e.Cancel = true;
                    break;
            }
        }
    }

    private void HandleUnsavedChanges(object sender, EventArgs e)
    {
        if (EditorActionHandler.UnsavedChanges)
        {
            Title = "Fish Level Editor v2.0.0 *";
        }
        else
        {
            Title = "Fish Level Editor v2.0.0";
        }
    }

    private async void HandleSaveAs(object sender, EventArgs e)
    {
        // Get top level from the current control. Alternatively, you can use Window reference instead.
        var topLevel = GetTopLevel(this);

        // Start async operation to open the dialog.
        var saveFileLocation = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save Project",
            DefaultExtension = ".json",
            ShowOverwritePrompt = true,
        });

        if (saveFileLocation is not null)
        {
            Session.Config.RecentProjectFilePath = saveFileLocation.Path.AbsolutePath;
            Session.Config.Save();
            Session.Project.Save(saveFileLocation.Path.AbsolutePath);
            EditorActionHandler.LastSavedActionIndex = EditorActionHandler.LastPerformedActionIndex;
        }
    }

    private async void HandleExport(object sender, EventArgs e)
    {
        MainViewModel mvm = DataContext as MainViewModel;
        // Get top level from the current control. Alternatively, you can use Window reference instead.
        var topLevel = GetTopLevel(this);

        // Start async operation to open the dialog.
        var openFolderLocation = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions()
        {
            AllowMultiple = false,
            Title = "Select export folder"
        });
        if (openFolderLocation.Count > 0)
        {
            mvm.LevelViewModel.Level.Export(openFolderLocation[0].Path.AbsolutePath);
        }
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

    private void SetLogMessage(string message)
    {
        LogsLabel.Content = message;
    }

    private void Repaint()
    {
        RepaintCHR();
        RepaintSelectedMetatile();
        RepaintMetatileSet();
        RepaintLevel();
        RepaintMasterPalette();
        RepaintPalettes();
        RepaintObjects();
        RepaintSelectedObject();
    }

    private void RepaintPalettes()
    {
        MainViewModel mvm = DataContext as MainViewModel;
        PalettesViewModel pvm = mvm.PalettesViewModel;
        pvm.Display();
        Palette0Bitmap.Bitmap = pvm.Palette0Bitmap;
        Palette1Bitmap.Bitmap = pvm.Palette1Bitmap;
        Palette2Bitmap.Bitmap = pvm.Palette2Bitmap;
        Palette3Bitmap.Bitmap = pvm.Palette3Bitmap;
    }

    private void RepaintMasterPalette()
    {
        MainViewModel mvm = DataContext as MainViewModel;
        MasterPaletteViewModel mpvm = mvm.MasterPaletteViewModel;
        mpvm.Display();
        MasterPaletteBitmap.Bitmap = mpvm.MasterPaletteBitmap;
        MasterPaletteBitmap.InvalidateVisual();
    }

    private void RepaintLevel()
    {
        MainLevelBitmap.InvalidateSurface();
        SetLevelImageDimensions();
    }

    private void RepaintMetatileSet()
    {
        MainViewModel mvm = (DataContext as MainViewModel);
        MetatileSetViewModel metatileSetViewModel = mvm.MetatileSetViewModel;
        metatileSetViewModel.MetatileSetBitmap.CHRBank = mvm.LevelViewModel.Level.BackgroundCHR;
        metatileSetViewModel.Display(mvm.LevelViewModel.Level.BackgroundPalettes[mvm.PalettesViewModel.SelectedPaletteIndex]);
        MetatileSetBitmap.Bitmap = metatileSetViewModel.MetatileSetBitmap.Bitmap;
        MetatileSetBitmap.InvalidateVisual();
    }

    private void RepaintSelectedMetatile()
    {
        MainViewModel mvm = (DataContext as MainViewModel);
        SelectedMetatileViewModel selectedMetatileViewModel = mvm.SelectedMetatileViewModel;
        selectedMetatileViewModel.SelectedMetatileBitmap.CHRBank = mvm.LevelViewModel.Level.BackgroundCHR;
        selectedMetatileViewModel.Display(mvm.LevelViewModel.Level.BackgroundPalettes[mvm.PalettesViewModel.SelectedPaletteIndex], mvm.LevelViewModel.Level.MetatileSet);
        SelectedMetatileBitmap.Bitmap = selectedMetatileViewModel.SelectedMetatileBitmap.Bitmap;
        SelectedMetatileBitmap.InvalidateVisual();
    }

    private void RepaintCHR()
    {
        MainViewModel mvm = (DataContext as MainViewModel);
        CHRBankViewModel chrBankViewModel = mvm.CHRBankViewModel;
        chrBankViewModel.CHRBankBitmap.CHRBank = mvm.LevelViewModel.Level.BackgroundCHR;
        chrBankViewModel.Display(mvm.LevelViewModel.Level.BackgroundPalettes[mvm.PalettesViewModel.SelectedPaletteIndex]);
        CHRBitmap.Bitmap = chrBankViewModel.CHRBankBitmap.Bitmap;
        CHRBitmap.InvalidateVisual();
    }

    private void RepaintObjects()
    {
        ObjectsBitmap.InvalidateSurface();
    }

    private void RepaintSelectedObject()
    {
        MainViewModel mvm = (DataContext as MainViewModel);
        SelectedObjectDefViewModel selectedObjectViewModel = mvm.SelectedObjectViewModel;
        selectedObjectViewModel.Display();
        SelectedObjectBitmap.Bitmap = selectedObjectViewModel.SelectedObjectDefinitionBitmap;
        SelectedObjectBitmap.Width = SelectedObjectBitmap.Bitmap.Width * 2;
        SelectedObjectBitmap.Height = SelectedObjectBitmap.Bitmap.Height * 2;
        SelectedObjectBitmap.InvalidateVisual();
        SelectedObjectDefNameTextBox.Text = selectedObjectViewModel.GetName();
    }

    private void ReplaceCHRButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        MainViewModel mvm = (DataContext as MainViewModel);
        OpenLevelCHRFilePicker(mvm.LevelViewModel.Level);
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

    private void SelectedObjectDefNameTextBox_TextChanged(object? sender, Avalonia.Controls.TextChangedEventArgs e)
    {

    }

    private void SelectedObjectDefVarValueTextBox_TextChanged(object? sender, Avalonia.Controls.TextChangedEventArgs e)
    {

    }

    private void CHRBitmap_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        MainViewModel mvm = (DataContext as MainViewModel);

        mvm.CHRBankViewModel.SelectedTileIndex = (uint)mvm.GetMouseTileIndex(e.GetPosition(CHRBitmap), 8, 16, 255);
    }

    private void SelectedMetatileBitmap_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        MainViewModel mvm = (DataContext as MainViewModel);
        var point = e.GetCurrentPoint(sender as Control);

        uint tileIndex = (uint) mvm.GetMouseTileIndex(e.GetPosition(SelectedMetatileBitmap), 8, 2, 3);
        if (point.Properties.IsLeftButtonPressed)
        {
            if (mvm.CHRBankViewModel.SelectedTileIndex >= 0)
            {
                CHRBankViewModel cvm = mvm.CHRBankViewModel;
                EditorActionHandler.Do(new SetMetatileTileAction(mvm.SelectedMetatileViewModel.MetatileIndex, (int)tileIndex, mvm.SelectedMetatileViewModel.GetMetatileFromSet(mvm.LevelViewModel.Level.MetatileSet).Tiles[tileIndex], cvm.SelectedTileIndex), mvm);
                Repaint();
            }
        }
        else if (point.Properties.IsRightButtonPressed)
        {
            EditorActionHandler.Do(new PickCHRTileAction(mvm.LevelViewModel.Level.MetatileSet.Metatiles[(int)mvm.SelectedMetatileViewModel.MetatileIndex].Tiles[tileIndex], mvm.CHRBankViewModel.SelectedTileIndex), mvm);

        }
    }

    private void MetatileSetBitmap_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        MainViewModel mvm = (DataContext as MainViewModel);
        uint metatileIndex = (uint) mvm.GetMouseTileIndex(e.GetPosition(MetatileSetBitmap), 16, 8, mvm.LevelViewModel.Level.MetatileSet.Metatiles.Count - 1);
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

        if (EditorTabs.SelectedIndex == 0 && point.Properties.IsLeftButtonPressed)
        {
            mvm.PlaceMetatileInLevel(e.GetPosition(MainLevelBitmap));
        }
    }

    private void MainLevelBitmap_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        MainViewModel mvm = (DataContext as MainViewModel);
        var point = e.GetCurrentPoint(sender as Control);
        if (point.Properties.IsLeftButtonPressed)
        {
            switch (EditorTabs.SelectedIndex)
            {
                case 0: // tiles
                    mvm.PlaceMetatileInLevel(e.GetPosition(MainLevelBitmap));
                    break;
                case 1: // objects
                    if (!mvm.GetObjectAtPosition(e.GetPosition(MainLevelBitmap)))
                    {
                        if (mvm.SelectedObjectViewModel.SelectedObject is not null)
                        {
                            mvm.MoveObject(e.GetPosition(MainLevelBitmap));
                        }
                        else if (mvm.SelectedObjectViewModel.SelectedObjectDefinition >= 0)
                        {
                            mvm.PlaceObjectInLevel(e.GetPosition(MainLevelBitmap), SelectedObjectDefVarValueTextBox.Text);
                        }
                    }
                    else
                    {
                        mvm.SelectedObjectViewModel.SelectedObjectDefinition = -1;
                    }
                    break;
                case 2: // entries
                    if (mvm.GetEntryAtPosition(e.GetPosition(MainLevelBitmap)))
                    {
                        EntriesListBox.SelectedItem = mvm.EntriesViewModel.SelectedEntry;
                    }
                    else
                    {
                        mvm.MoveEntry(e.GetPosition(MainLevelBitmap));
                    }
                    break;
                default:
                    break;
            }
        }
        else if (point.Properties.IsRightButtonPressed)
        {
            switch (EditorTabs.SelectedIndex)
            {
                case 0: // tiles
                    mvm.PickMetatile(e.GetPosition(MainLevelBitmap));
                    break;
                case 1: // objects
                    if (mvm.GetObjectAtPosition(e.GetPosition(MainLevelBitmap)))
                    {
                        EditorActionHandler.Do(new DeleteObjectFromLevelAction(mvm.SelectedObjectViewModel.SelectedObject), mvm);
                        mvm.SelectedObjectViewModel.SelectedObject = null;
                        Repaint();
                    }
                    break;
                case 2: // entries
                    break;
                default:
                    break;
            }
        }

    }

    private void PaletteBitmap_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        MainViewModel mvm = (DataContext as MainViewModel);
        SKBitmapControl paletteBitmap = sender as SKBitmapControl;
        if (int.TryParse((string)paletteBitmap.DataContext, out int paletteIndex))
        {
            mvm.PalettesViewModel.SelectedPaletteIndex = (uint)paletteIndex;
            mvm.PalettesViewModel.SelectedPaletteColorIndex = (uint) mvm.GetMouseTileIndex(e.GetPosition(paletteBitmap), 8, 4, 3);
            Repaint();
        }
    }

    private void MasterPaletteBitmap_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        MainViewModel mvm = (DataContext as MainViewModel);
        int selectedColor = mvm.GetMouseTileIndex(e.GetPosition(MasterPaletteBitmap), 8, 16, 63);
        if (selectedColor == 0x0D || selectedColor == 0x1D || (selectedColor & 0x0E) == 0x0E)
        {
            selectedColor = 0x0F;
        }
        if (mvm.PalettesViewModel.SelectedPaletteColorIndex == 0)
        {
            // universal background color
            for (int i = 0; i < mvm.LevelViewModel.Level.BackgroundPalettes.Length; i++)
            {
                mvm.SetPaletteColor((uint)i, mvm.PalettesViewModel.SelectedPaletteColorIndex, (uint) selectedColor);
            }
        }
        else
        {
            mvm.SetPaletteColor(mvm.PalettesViewModel.SelectedPaletteIndex, mvm.PalettesViewModel.SelectedPaletteColorIndex, (uint) selectedColor);
        }
        Repaint();
    }

    private void EntriesListBox_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        MainViewModel mvm = DataContext as MainViewModel;
        mvm.EntriesViewModel.SelectedEntry = EntriesListBox.SelectedItem as LevelEntry;
    }

    private void AddEntryButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        MainViewModel mvm = DataContext as MainViewModel;
        EditorActionHandler.Do(new AddEntryAction(), mvm);
        Repaint();
    }

    private void EntryDeleteButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        MainViewModel mvm = DataContext as MainViewModel;
        var button = sender as Button;
        if (button is not null)
        {
            var levelEntry = button.DataContext as LevelEntry;
            EditorActionHandler.Do(new RemoveEntryAction(levelEntry), mvm);
        }
        Repaint();
    }

    private void ChangeLevelMenuItem_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        NextWindow = WindowManager.NextWindowType.LevelSelect;
        Close();
    }

    private void ChangeProjectMenuItem_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        NextWindow = WindowManager.NextWindowType.OpenProject;
        Close();
    }

    private void ObjectsBitmap_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        MainViewModel mvm = (DataContext as MainViewModel);
        int selectedObjectDefinition = mvm.GetMouseTileIndex(e.GetPosition(ObjectsBitmap), 16, 8, Session.Project.LevelObjectDefinitions.Count - 1);
        mvm.SelectedObjectViewModel.SelectedObjectDefinition = selectedObjectDefinition;
        if (selectedObjectDefinition >= 0)
        {
            SelectedObjectDefNameTextBox.Text = Session.Project.LevelObjectDefinitions[selectedObjectDefinition].Name;
            SelectedObjectDefVarNameLabel.Content = Session.Project.LevelObjectDefinitions[selectedObjectDefinition].VarName;
            mvm.SelectedObjectViewModel.SelectedObject = null; // unset Selected Object
        }
        Repaint();
    }

    private void AddObjectDefinitionButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        NewObjectDefinitionDialogViewModel noddViewModel = new();
        NewObjectDefinitionDialog newObjectDefinitionDialog = new()
        {
            DataContext = noddViewModel
        };
        newObjectDefinitionDialog.Closed += NewObjectDefinitionDialog_Closed;
        newObjectDefinitionDialog.ShowDialog(this);

    }

    private void NewObjectDefinitionDialog_Closed(object? sender, EventArgs e)
    {
        NewObjectDefinitionDialogViewModel noddViewModel = (sender as NewObjectDefinitionDialog).DataContext as NewObjectDefinitionDialogViewModel;
        if (noddViewModel.CreatedObject is not null)
        {
            Session.Project.LevelObjectDefinitions.Add(noddViewModel.CreatedObject);
            MainViewModel mvm = (DataContext as MainViewModel);
            mvm.ObjectsViewModel.ObjectDefinitions = new(Session.Project.LevelObjectDefinitions);
            Repaint();
        }
    }
}
