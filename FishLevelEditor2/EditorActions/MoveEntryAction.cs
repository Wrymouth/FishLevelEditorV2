using FishLevelEditor2.Logic;
using FishLevelEditor2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishLevelEditor2.EditorActions
{
    public class MoveEntryAction : EditorAction
    {
        public override string LogMessage => $"Move entry to {PosX}, {PosY}";
        public int PrevX { get; set; }
        public int PrevY { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
        public LevelEntry Entry { get; set; }

        public MoveEntryAction(int prevX, int prevY, int posX, int posY, LevelEntry entry)
        {
            PrevX = prevX;
            PrevY = prevY;
            PosX = posX;
            PosY = posY;
            Entry = entry;
        }

        public override void Do(MainViewModel mvm)
        {
            Entry.PosX = PosX;
            Entry.PosY = PosY;
        }

        public override void Undo(MainViewModel mvm)
        {
            Entry.PosX = PrevX;
            Entry.PosY = PrevY;
        }
    }
}
