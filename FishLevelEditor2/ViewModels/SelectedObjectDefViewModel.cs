using FishLevelEditor2.Logic;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishLevelEditor2.ViewModels
{
    public class SelectedObjectDefViewModel : ViewModelBase
    {
        public LevelObject? SelectedObject { get; set; }
        public int SelectedObjectDefinition { get; set; }
        public SKBitmap SelectedObjectDefinitionBitmap { get; set; }

        public SelectedObjectDefViewModel()
        {
            SelectedObjectDefinition = -1;
        }

        internal void Display()
        {
            if (SelectedObjectDefinition >= 0)
            {
                LevelObjectDefinition objectDefinition = Session.Project.LevelObjectDefinitions[SelectedObjectDefinition];
                SelectedObjectDefinitionBitmap = BitmapUtils.LoadSKBitmapFromFile(objectDefinition.SpriteFilePath);
            }
            else
            {
                SelectedObjectDefinitionBitmap = new();
            }

        }
    }
}
