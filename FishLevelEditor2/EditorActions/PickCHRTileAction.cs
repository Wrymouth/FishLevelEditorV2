using FishLevelEditor2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace FishLevelEditor2.EditorActions
{
    public class PickCHRTileAction : EditorAction
    {
        public override string LogMessage => $"Picked CHR tile {TileIndex}";

        public uint PreviousTileIndex { get; set; }
        public uint TileIndex { get; set; }

        public PickCHRTileAction(uint tileIndex, uint previousTileIndex)
        {
            TileIndex = tileIndex;
            PreviousTileIndex = previousTileIndex;
        }

        public override void Do(MainViewModel mvm)
        {
            mvm.CHRBankViewModel.SelectedTileIndex = TileIndex;
        }

        public override void Undo(MainViewModel mvm)
        {
            mvm.CHRBankViewModel.SelectedTileIndex = PreviousTileIndex;
        }
    }
}
