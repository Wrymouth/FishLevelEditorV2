using FishLevelEditor2.Logic;

namespace FishLevelEditor2.ViewModels;

public class MainViewModel : ViewModelBase
{
    public Level Level { get; set; }
    public string Greeting => "Welcome to Avalonia! This is my added text.";

    public MainViewModel(Level level)
    {
        Level = level;
    }
}
