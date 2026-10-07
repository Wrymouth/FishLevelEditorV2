using FishLevelEditor2.Logic;
using FishLevelEditor2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishLevelEditor2.EditorActions
{
    public class MoveObjectAction : EditorAction
    {
        public override string LogMessage => $"Move {LevelObject.Definition.Name} to {PosX}, {PosY}";

        public int PrevX { get; set; }
        public int PrevY { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
        public LevelObject LevelObject { get; set; }

        public MoveObjectAction(int prevX, int prevY, int posX, int posY, LevelObject levelObject)
        {
            PrevX = prevX;
            PrevY = prevY;
            PosX = posX;
            PosY = posY;
            LevelObject = levelObject;
        }

        public override void Do(MainViewModel mvm)
        {
            LevelObject.PosX = PosX;
            LevelObject.PosY = PosY;
        }

        public override void Undo(MainViewModel mvm)
        {
            LevelObject.PosX = PrevX;
            LevelObject.PosY = PrevY;
        }
    }
}
