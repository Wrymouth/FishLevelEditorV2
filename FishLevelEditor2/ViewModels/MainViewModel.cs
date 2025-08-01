using Avalonia;
using Avalonia.Platform.Storage;
using FishLevelEditor2.DataAccess;
using FishLevelEditor2.EditorActions;
using FishLevelEditor2.Logic;
using ReactiveUI;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;

namespace FishLevelEditor2.ViewModels;

public delegate void RepaintEventHandler(object sender, EventArgs e);
public delegate void OpenSaveAsDialogEventHandler(object sender, EventArgs e);
public delegate void OpenExportDialogEventHandler(object sender, EventArgs e);
public class MainViewModel : ViewModelBase
{
    public event RepaintEventHandler Repaint;
    public event OpenSaveAsDialogEventHandler OpenSaveAs;
    public event OpenExportDialogEventHandler OpenExport;
    public CHRBankViewModel CHRBankViewModel { get; set; }
    public SelectedMetatileViewModel SelectedMetatileViewModel { get; set; }
    public MetatileSetViewModel MetatileSetViewModel { get; set; }
    public MasterPaletteViewModel MasterPaletteViewModel { get; set; }
    public PalettesViewModel PalettesViewModel { get; set; }
    public LevelViewModel LevelViewModel { get; set; }
    public EntriesViewModel EntriesViewModel { get; set; }
    public ReactiveCommand<Unit, Unit> UndoCommand { get; }
    public ReactiveCommand<Unit, Unit> RedoCommand { get; }
    public ReactiveCommand<Unit, Unit> SaveCommand { get; }
    public ReactiveCommand<Unit, Unit> SaveAsCommand { get; }
    public ReactiveCommand<Unit, Unit> ExportCommand { get; }

    public MainViewModel(Level level)
    {
        level.LevelRepository = new LevelRepository(new ExportSettings(false, ExportSettings.ExportFormat.Columns));
        LevelViewModel = new(level);
        CHRBankViewModel = new(level.BackgroundCHR);
        MetatileSetViewModel = new(level.MetatileSet, level.BackgroundCHR);
        SelectedMetatileViewModel = new(0, level.BackgroundCHR);
        MasterPaletteViewModel = new();
        PalettesViewModel = new(level.BackgroundPalettes);
        EntriesViewModel = new(level.Entries);
        UndoCommand = ReactiveCommand.Create(Undo);
        RedoCommand = ReactiveCommand.Create(Redo);
        SaveCommand = ReactiveCommand.Create(Save);
        SaveAsCommand = ReactiveCommand.Create(SaveAs);
        ExportCommand = ReactiveCommand.Create(Export);
    }

    // empty constructor for previewer
    public MainViewModel()
    {

    }

    public uint GetMouseTileIndex(Point mousePos, int tileSize, int tilesPerRow, uint maxTileIndex)
    {
        const int BITMAP_SCALE = 2;
        int posX = (int)mousePos.X / BITMAP_SCALE;
        int posY = (int)mousePos.Y / BITMAP_SCALE;
        uint tileIndex = (uint)(posY / tileSize * tilesPerRow + (posX / tileSize));
        if (tileIndex > maxTileIndex)
        {
            tileIndex = maxTileIndex;
        }
        return tileIndex;
    }

    public void PlaceMetatileInLevel(Point mousePos)
    {
        Level level = LevelViewModel.Level;
        uint tileIndex = GetMouseTileIndex(mousePos, 16, level.Width, (uint)(level.Width * level.Height));
        if (SelectedMetatileViewModel.MetatileIndex >= 0)
        {
            int posY = (int)tileIndex / level.Width;
            int posX = (int)tileIndex % level.Width;
            if (posY >= level.Height || posX >= level.Width)
            {
                return;
            }
            ScreenMetatile selectedScreenMetatile = level.ScreenMetatiles[posX][posY];
            uint metatileIndex = SelectedMetatileViewModel.MetatileIndex;
            if (selectedScreenMetatile.mi == metatileIndex && selectedScreenMetatile.pi == 0)
            {
                // do not process this action, it's a repeat
                return;
            }

            EditorActionHandler.Do(new PlaceMetatileInLevelAction(new ScreenMetatile(metatileIndex, PalettesViewModel.SelectedPaletteIndex), level.ScreenMetatiles[posX][posY], posX, posY), this);
            Repaint?.Invoke(this, new EventArgs());
        }
    }
    public void PickMetatile(Point mousePos)
    {
        Level level = LevelViewModel.Level;
        uint tileIndex = GetMouseTileIndex(mousePos, 16, level.Width, (uint)(level.Width * level.Height));
        int posY = (int)tileIndex / level.Width;
        int posX = (int)tileIndex % level.Width;
        if (posY >= level.Height || posX >= level.Width)
        {
            return;
        }
        EditorActionHandler.Do(new PickMetatileAction(SelectedMetatileViewModel.MetatileIndex, 0, posX, posY), this);
        Repaint?.Invoke(this, new EventArgs());
    }
    
    public void MoveEntry(Point mousePos)
    {
        Level level = LevelViewModel.Level;
        uint tileIndex = GetMouseTileIndex(mousePos, 16, level.Width, (uint)(level.Width * level.Height));
        int posY = (int)tileIndex / level.Width;
        int posX = (int)tileIndex % level.Width;
        if (EntriesViewModel.SelectedEntry is not null)
        {
            EditorActionHandler.Do(new MoveEntryAction(EntriesViewModel.SelectedEntry.PosX, EntriesViewModel.SelectedEntry.PosY, posX, posY, EntriesViewModel.SelectedEntry), this);
        }
        Repaint?.Invoke(this, new EventArgs());
    }

    public void Undo()
    {
        EditorActionHandler.Undo(this);
        Repaint?.Invoke(this, new EventArgs());
    }

    public void Redo()
    {
        EditorActionHandler.Redo(this);
        Repaint?.Invoke(this, new EventArgs());
    }

    public void Save()
    {
        string filePath = Session.Config.RecentProjectFilePath;
        if (string.IsNullOrEmpty(filePath))
        {
            SaveAs();
            return;
        }
        Session.Project.Save(filePath);
        EditorActionHandler.LastSavedActionIndex = EditorActionHandler.LastPerformedActionIndex;
    }

    public void SaveAs()
    {
        // not a fan of this, but the Command structure is kinda forcing my hand
        OpenSaveAs?.Invoke(this, new EventArgs());
    }

    public void Export()
    {
        OpenExport?.Invoke(this, new EventArgs());
    }
}
