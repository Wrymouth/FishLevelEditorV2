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
    public Level Level { get; set; }
    public CHRBankViewModel CHRBankViewModel { get; set; }
    public SelectedMetatileViewModel SelectedMetatileViewModel { get; set; }
    public MetatileSetViewModel MetatileSetViewModel { get; set; }
    public ReactiveCommand<Unit, Unit> UndoCommand { get; }
    public ReactiveCommand<Unit, Unit> RedoCommand { get; }

    public MainViewModel(Level level)
    {
        Level = level;
        CHRBankViewModel = new(Level.BackgroundCHR);
        MetatileSetViewModel = new(Level.MetatileSet, Level.BackgroundCHR);
        SelectedMetatileViewModel = new(Level.MetatileSet.Metatiles[0], Level.BackgroundCHR);
        UndoCommand = ReactiveCommand.Create(Undo);
        RedoCommand = ReactiveCommand.Create(Redo);
    }

    // empty constructor for previewer
    public MainViewModel()
    {
        
    }

    public void Undo()
    {
        EditorActionHandler.Undo(Level);
        Repaint?.Invoke(this, new EventArgs());
    }

    public void Redo()
    {
        EditorActionHandler.Redo(Level);
        Repaint?.Invoke(this, new EventArgs());
    }
}
