using FishLevelEditor2.Logic;

namespace FishLevelEditor2.ViewModels;

public class MainViewModel : ViewModelBase
{
    public Level Level { get; set; }
    public CHRBankViewModel CHRBankViewModel { get; set; }
    public SelectedMetatileViewModel SelectedMetatileViewModel { get; set; }
    public MetatileSetViewModel MetatileSetViewModel { get; set; }

    public MainViewModel(Level level)
    {
        Level = level;
        CHRBankViewModel = new(Level.BackgroundCHR);
        MetatileSetViewModel = new(Level.MetatileSet, Level.BackgroundCHR);
        SelectedMetatileViewModel = new(Level.MetatileSet.Metatiles[0], Level.BackgroundCHR);
    }
}
