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
