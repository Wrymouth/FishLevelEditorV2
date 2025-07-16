using FishLevelEditor2.Logic;
using FishLevelEditor2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishLevelEditor2.EditorActions
{
    public class SetMetatileTileAction : EditorAction
    {
        public uint MetatileIndex { get; set; }
        public int TileIndex { get; set; }
        public uint PreviousTile { get; set; }
        public uint NewTile { get; set; }

        public override string LogMessage => $"Set Metatile {MetatileIndex} tile index {TileIndex} to tile {NewTile}";

        public SetMetatileTileAction(uint metatileIndex, int tileIndex, uint previousTile, uint newTile)
        {
            MetatileIndex = metatileIndex;
            TileIndex = tileIndex;
            PreviousTile = previousTile;
            NewTile = newTile;
        }

        public override void Do(MainViewModel mvm)
        {
            mvm.LevelViewModel.Level.MetatileSet.Metatiles[(int) MetatileIndex].Tiles[TileIndex] = NewTile;
        }

        public override void Undo(MainViewModel mvm)
        {
            mvm.LevelViewModel.Level.MetatileSet.Metatiles[(int) MetatileIndex].Tiles[TileIndex] = PreviousTile;
        }
    }
}
