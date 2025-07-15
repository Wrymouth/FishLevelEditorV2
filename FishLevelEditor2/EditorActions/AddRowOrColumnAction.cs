using FishLevelEditor2.Logic;
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

        public override void Do(Level level)
        {
            switch (Type)
            {
                case AddType.RowsAbove:
                    level.AddRowsAbove(Amount);
                    break;
                case AddType.RowsBelow:
                    level.AddRowsBelow(Amount);
                    break;
                case AddType.ColumnsLeft:
                    level.AddColumnsLeft(Amount);
                    break;
                case AddType.ColumnsRight:
                    level.AddColumnsRight(Amount);
                    break;
            }
        }

        public override void Undo(Level level)
        {
            switch (Type)
            {
                case AddType.RowsAbove:
                    level.RemoveRowsAbove(Amount);
                    break;
                case AddType.RowsBelow:
                    level.RemoveRowsBelow(Amount);
                    break;
                case AddType.ColumnsLeft:
                    level.RemoveColumnsLeft(Amount);
                    break;
                case AddType.ColumnsRight:
                    level.RemoveColumnsRight(Amount);
                    break;
            }
        }
    }
}
