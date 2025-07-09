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
        public string Name { get; set; }
        public CHRBank BackgroundCHR { get; set; }

        public List<List<ScreenMetatile>> ScreenMetatiles { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public int MetatileSetIndex { get; set; }
        public Palette[] BackgroundPalettes { get; set; }
        public Palette[] ObjectPalettes { get; set; }
        public ObservableCollection<LevelObject> Objects { get; set; }
        public ObservableCollection<LevelEntry> Entries { get; set; }
        public ObservableCollection<LevelExit> Exits { get; set; }

        public Level(string name, string chrFilePath, int startingWidth, int startingHeight, int metatileSetIndex)
        {
            Name = name;
            MetatileSetIndex = metatileSetIndex;
            BackgroundCHR = new(chrFilePath);
            // todo fill with default palettes
            BackgroundPalettes =
            [
                new Palette(0x0F, 0x00, 0x10, 0x30),
                new Palette(0x0F, 0x05, 0x16, 0x27),
                new Palette(0x0F, 0x0C, 0x21, 0x32),
                new Palette(0x0F, 0x0B, 0x1A, 0x29),
            ];
            ObjectPalettes =
            [
                new Palette(0x0F, 0x00, 0x10, 0x30),
                new Palette(0x0F, 0x05, 0x16, 0x27),
                new Palette(0x0F, 0x0C, 0x21, 0x32),
                new Palette(0x0F, 0x0B, 0x1A, 0x29),
            ];
            ScreenMetatiles = [[]];
            Objects = [];
            Entries = [];
            Exits = [];
        }

        public Metatile GetMetatile(int index)
        {
            return Session.Project.MetatileSets[MetatileSetIndex].Metatiles[index];
        }
    }
}
