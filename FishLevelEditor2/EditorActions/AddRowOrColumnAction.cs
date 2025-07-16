using FishLevelEditor2.Logic;
using FishLevelEditor2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishLevelEditor2.EditorActions
{
    public class AddRowOrColumnAction : EditorAction
    {
        public enum AddType
        {
            RowsAbove,
            RowsBelow,
            ColumnsLeft,
            ColumnsRight
        }
        public override string LogMessage
        {
            get
            {
                string typeString = "";
                switch (Type)
                {
                    case AddType.RowsAbove:
                        typeString = "rows above";
                        break;
                    case AddType.RowsBelow:
                        typeString = "rows below";
                        break;
                    case AddType.ColumnsLeft:
                        typeString = "columns left";
                        break;
                    case AddType.ColumnsRight:
                        typeString = "columns right";
                        break;
                }
                return $"Add {Amount} {typeString}";
            }
        }

        public AddType Type { get; set; }

        public int Amount { get; set; }

        public AddRowOrColumnAction(AddType type, int amount)
        {
            Type = type;
            Amount = amount;
        }

        public override void Do(MainViewModel mvm)
        {
            switch (Type)
            {
                case AddType.RowsAbove:
                    mvm.LevelViewModel.Level.AddRowsAbove(Amount);
                    break;
                case AddType.RowsBelow:
                    mvm.LevelViewModel.Level.AddRowsBelow(Amount);
                    break;
                case AddType.ColumnsLeft:
                    mvm.LevelViewModel.Level.AddColumnsLeft(Amount);
                    break;
                case AddType.ColumnsRight:
                    mvm.LevelViewModel.Level.AddColumnsRight(Amount);
                    break;
            }
        }

        public override void Undo(MainViewModel mvm)
        {
            switch (Type)
            {
                case AddType.RowsAbove:
                    mvm.LevelViewModel.Level.RemoveRowsAbove(Amount);
                    break;
                case AddType.RowsBelow:
                    mvm.LevelViewModel.Level.RemoveRowsBelow(Amount);
                    break;
                case AddType.ColumnsLeft:
                    mvm.LevelViewModel.Level.RemoveColumnsLeft(Amount);
                    break;
                case AddType.ColumnsRight:
                    mvm.LevelViewModel.Level.RemoveColumnsRight(Amount);
                    break;
            }
        }
    }
}
