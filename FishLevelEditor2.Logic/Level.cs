using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishLevelEditor2.Logic
{
    public class Level
    {
        public string Name { get; set; }
        public CHRBank BackgroundCHR { get; set; }

        public List<List<ScreenMetatile>> ScreenMetatiles { get; set; } // [x][y]
        public int Width { get; set; }
        public int Height { get; set; }

        public int MetatileSetIndex { get; set; }

        [JsonIgnore]
        public MetatileSet MetatileSet
        {
            get
            {
                return Session.Project.MetatileSets[MetatileSetIndex];
            }
        }
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
            ScreenMetatiles = [];
            Objects = [];
            Entries = [];
            Exits = [];
            Width = startingWidth;
            Height = startingHeight;
            InitializeRect(startingHeight, startingWidth, 0, 0);
        }

        // empty constructor for Json Deserialize
        public Level()
        {
            
        }

        public void InitializeRect(int height, int width, uint metatile, uint palette)
        {
            for (int x = 0; x < width; x++)
            {
                ScreenMetatiles.Add([]);
                for (int y = 0; y < height; y++)
                {
                    ScreenMetatiles[x].Add(new(metatile, palette));
                }
            }
        }

        public Metatile GetMetatile(int index)
        {
            return Session.Project.MetatileSets[MetatileSetIndex].Metatiles[index];
        }

        public void AddRowsAbove(int amount)
        {
            foreach (var column in ScreenMetatiles)
            {
                for (int i = 0; i < amount; i++)
                {
                    column.Insert(0, new ScreenMetatile(0, 0));
                }
            }
            Height += amount;
        }

        public void AddRowsBelow(int amount)
        {
            foreach (var column in ScreenMetatiles)
            {
                for (int i = 0; i < amount; i++)
                {
                    column.Add(new ScreenMetatile(0, 0));
                }
            }
            Height += amount;
        }

        public void AddColumnsLeft(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                ScreenMetatiles.Insert(0, []);
                for (int j = 0; j < Height; j++)
                {
                    ScreenMetatiles[0].Add(new ScreenMetatile(0, 0));
                }
            }
            Width += amount;
        }

        public void AddColumnsRight(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                ScreenMetatiles.Add([]);
                for (int j = 0; j < Height; j++)
                {
                    ScreenMetatiles[ScreenMetatiles.Count - 1].Add(new ScreenMetatile(0, 0));
                }
            }
            Width += amount;
        }

        public void RemoveRowsAbove(int amount)
        {
            foreach (var column in ScreenMetatiles)
            {
                for (int i = 0; i < amount; i++)
                {
                    column.RemoveAt(0);
                }
            }
            Height -= amount;
        }

        public void RemoveRowsBelow(int amount)
        {
            foreach (var column in ScreenMetatiles)
            {
                for (int i = 0; i < amount; i++)
                {
                    column.RemoveAt(column.Count-1);
                }
            }
            Height -= amount;
        }

        public void RemoveColumnsLeft(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                ScreenMetatiles.RemoveAt(0);
            }
            Width -= amount;
        }

        public void RemoveColumnsRight(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                ScreenMetatiles.RemoveAt(ScreenMetatiles.Count-1);
            }
            Width -= amount;
        }
    }
}
