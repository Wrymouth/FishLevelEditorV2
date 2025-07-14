using FishLevelEditor2.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishLevelEditor2.EditorActions
{
    public class SetMetatileTileAction : EditorAction
    {
        public Metatile Metatile { get; set; }
        public int TileIndex { get; set; }
        public uint PreviousTile { get; set; }
        public uint NewTile { get; set; }

        public override string LogMessage => $"Set Metatile {Metatile.Name} index {TileIndex} to tile {NewTile}";

        public SetMetatileTileAction(Metatile metatile, int tileIndex, uint previousTile, uint newTile)
        {
            Metatile = metatile;
            TileIndex = tileIndex;
            PreviousTile = previousTile;
            NewTile = newTile;
        }

        public override void Do(Level level)
        {
            Metatile.Tiles[TileIndex] = NewTile;
        }

        public override void Undo(Level level)
        {
            Metatile.Tiles[TileIndex] = PreviousTile;
        }
    }
}
