using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishLevelEditor2.Logic
{
    public class Level
    {
        public enum LevelType
        {
            Horizontal,
            Vertical,
            LargeHorizontal,
            Diagonal
        }

        public LevelType Type { get; set; }

        public List<List<ScreenMetatile>> Metatiles { get; set; }
        public ObservableCollection<LevelObject> Objects { get; set; }
        public ObservableCollection<LevelEntry> Entries { get; set; }
        public ObservableCollection<LevelExit> Exits { get; set; }
    }
}
