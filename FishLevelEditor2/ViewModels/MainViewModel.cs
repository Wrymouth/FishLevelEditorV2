using Avalonia;
using FishLevelEditor2.EditorActions;
using FishLevelEditor2.Logic;
using ReactiveUI;
using System;
using System.Diagnostics;
using System.Reactive;
using System.Threading.Tasks;

namespace FishLevelEditor2.ViewModels;

public delegate void RepaintEventHandler(object sender, EventArgs e);
public class MainViewModel : ViewModelBase
{
    public event RepaintEventHandler Repaint;
    public CHRBankViewModel CHRBankViewModel { get; set; }
    public SelectedMetatileViewModel SelectedMetatileViewModel { get; set; }
    public MetatileSetViewModel MetatileSetViewModel { get; set; }
    public LevelViewModel LevelViewModel { get; set; }
    public ReactiveCommand<Unit, Unit> UndoCommand { get; }
    public ReactiveCommand<Unit, Unit> RedoCommand { get; }

    public MainViewModel(Level level)
    {
        LevelViewModel = new(level);
        CHRBankViewModel = new(level.BackgroundCHR);
        MetatileSetViewModel = new(level.MetatileSet, level.BackgroundCHR);
        SelectedMetatileViewModel = new(level.MetatileSet.Metatiles[0], level.BackgroundCHR);
        UndoCommand = ReactiveCommand.Create(Undo);
        RedoCommand = ReactiveCommand.Create(Redo);
    }

    // empty constructor for previewer
    public MainViewModel()
    {
        
    }

    public int GetMouseTileIndex(Point mousePos, int tileSize, int tilesPerRow, int maxTileIndex)
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

    public void PlaceMetatileInLevel(Point mousePos)
    {
        Level level = LevelViewModel.Level;
        int tileIndex = GetMouseTileIndex(mousePos, 16, level.Width, level.Width * level.Height);
        if (SelectedMetatileViewModel.Metatile is not null)
        {
            int posY = tileIndex / level.Width;
            int posX = tileIndex % level.Width;
            if (posY >= level.Height || posX >= level.Width)
            {
                return;
            }
            ScreenMetatile selectedScreenMetatile = level.ScreenMetatiles[posX][posY];
            uint metatileIndex = (uint)level.MetatileSet.Metatiles.FindIndex(0, (m) => m == SelectedMetatileViewModel.Metatile);
            if (selectedScreenMetatile.Metatile == metatileIndex && selectedScreenMetatile.Palette == 0)
            {
                // do not process this action, it's a repeat
                return;
            }

            EditorActionHandler.Do(new PlaceMetatileInLevelAction(new ScreenMetatile(metatileIndex, 0), level.ScreenMetatiles[posX][posY], posX, posY), level);
            Repaint?.Invoke(this, new EventArgs());
        }
    }

    public void Undo()
    {
        EditorActionHandler.Undo(LevelViewModel.Level);
        Repaint?.Invoke(this, new EventArgs());
    }

    public void Redo()
    {
        EditorActionHandler.Redo(LevelViewModel.Level);
        Repaint?.Invoke(this, new EventArgs());
    }
}
