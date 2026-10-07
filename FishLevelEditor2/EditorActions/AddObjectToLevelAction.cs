using FishLevelEditor2.Logic;
using FishLevelEditor2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FishLevelEditor2.EditorActions
{
    public class AddObjectToLevelAction : EditorAction
    {
        public override string LogMessage => $"Place {ObjectDefinition.Name} in level at {PosX}, {PosY}{(!string.IsNullOrWhiteSpace(VarValue) ? $" with {ObjectDefinition.VarName} = {VarValue}" : "")}.";

        public LevelObjectDefinition ObjectDefinition { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
        public string? VarValue { get; set; }

        public AddObjectToLevelAction(LevelObjectDefinition objectDefinition, int posX, int posY, string? varValue)
        {
            PosX = posX;
            PosY = posY;
            ObjectDefinition = objectDefinition;
            VarValue = varValue;
        }


        public override void Do(MainViewModel mvm)
        {
            mvm.LevelViewModel.Level.Objects.Add(new LevelObject(ObjectDefinition, PosX, PosY, VarValue));
        }

        public override void Undo(MainViewModel mvm)
        {
            mvm.LevelViewModel.Level.RemoveObjectByPosition(PosX, PosY);
        }
    }
}
