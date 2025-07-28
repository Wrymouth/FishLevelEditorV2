using FishLevelEditor2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishLevelEditor2.EditorActions
{
    public class SetPaletteColorAction : EditorAction
    {
        public override string LogMessage => $"Set color {PaletteColorIndex} of palette {LevelPaletteIndex} to {NewColor}";
        public uint LevelPaletteIndex { get; set; }
        public uint PaletteColorIndex { get; set; }
        public uint PreviousColor { get; set; }
        public uint NewColor { get; set; }

        public SetPaletteColorAction(uint levelPaletteIndex, uint paletteColorIndex, uint previousColor, uint newColor)
        {
            LevelPaletteIndex = levelPaletteIndex;
            PaletteColorIndex = paletteColorIndex;
            PreviousColor = previousColor;
            NewColor = newColor;
        }

        public override void Do(MainViewModel mvm)
        {
            mvm.LevelViewModel.Level.BackgroundPalettes[LevelPaletteIndex].Colors[PaletteColorIndex] = NewColor;
        }

        public override void Undo(MainViewModel mvm)
        {
            mvm.LevelViewModel.Level.BackgroundPalettes[LevelPaletteIndex].Colors[PaletteColorIndex] = PreviousColor;
        }
    }
}
