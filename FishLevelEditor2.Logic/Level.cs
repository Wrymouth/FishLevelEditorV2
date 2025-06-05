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

        public string Name { get; set; }
        public List<List<ScreenMetatile>> ScreenMetatiles { get; set; }
        public int MetatileSetIndex { get; set; }
        public ObservableCollection<LevelObject> Objects { get; set; }
        public ObservableCollection<LevelEntry> Entries { get; set; }
        public ObservableCollection<LevelExit> Exits { get; set; }

        public Level(string name, LevelType type, int metatileSetIndex)
        {
            Name = name;
            MetatileSetIndex = metatileSetIndex;
        }

        public Metatile GetMetatile(int index)
        {
            return Session.Project.MetatileSets[MetatileSetIndex].Metatiles[index];
        }
    }
}
