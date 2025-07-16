using FishLevelEditor2.Logic;
using FishLevelEditor2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishLevelEditor2.EditorActions
{
    public class PlaceMetatileInLevelAction : EditorAction
    {
        public ScreenMetatile NewMetatile { get; set; }
        public ScreenMetatile PreviousMetatile { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }

        public override string LogMessage => $"Place metatile {NewMetatile.Metatile} at X = {PosX} Y = {PosY}";

        public PlaceMetatileInLevelAction(ScreenMetatile newMetatile, ScreenMetatile previousMetatile, int posX, int posY)
        {
            NewMetatile = newMetatile;
            PreviousMetatile = new ScreenMetatile(previousMetatile);
            PosX = posX;
            PosY = posY;
        }

        public override void Do(MainViewModel mvm)
        {
            mvm.LevelViewModel.Level.ScreenMetatiles[PosX][PosY] = NewMetatile;
        }

        public override void Undo(MainViewModel mvm)
        {
            mvm.LevelViewModel.Level.ScreenMetatiles[PosX][PosY] = PreviousMetatile;
        }
    }
}
