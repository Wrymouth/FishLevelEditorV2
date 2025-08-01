using FishLevelEditor2.Logic;
using System.Collections.ObjectModel;

namespace FishLevelEditor2.ViewModels
{
    public class EntriesViewModel
    {
        public ObservableCollection<LevelEntry> Entries { get; set; }
        public LevelEntry? SelectedEntry { get; set; }

        public EntriesViewModel(ObservableCollection<LevelEntry> entries)
        {
            Entries = entries;
            SelectedEntry = null;
        }
    }
}