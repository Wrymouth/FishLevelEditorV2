using FishLevelEditor2.Logic;

namespace FishLevelEditor2.ViewModels;

public class MainViewModel : ViewModelBase
{
    public Level Level { get; set; }
    public CHRBankViewModel CHRBankViewModel { get; set; }

    public MainViewModel(Level level)
    {
        Level = level;
        CHRBankViewModel = new(Level.BackgroundCHR);
    }

    public MainViewModel()
    {
    }
}
