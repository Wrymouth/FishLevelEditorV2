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
        public int SelectedObjectDefinition { get; 
            set; }
        public SKBitmap SelectedObjectDefinitionBitmap { get; set; }

        public SelectedObjectDefViewModel()
        {
            SelectedObject = null;
            SelectedObjectDefinition = -1;
        }

        public void Display()
        {
            if (SelectedObjectDefinition >= 0)
            {
                LevelObjectDefinition objectDefinition = Session.Project.LevelObjectDefinitions[SelectedObjectDefinition];
                SelectedObjectDefinitionBitmap = BitmapUtils.LoadSKBitmapFromFile(objectDefinition.SpriteFilePath);
            }
            else if (SelectedObject is not null)
            {
                SelectedObjectDefinitionBitmap = BitmapUtils.LoadSKBitmapFromFile(SelectedObject.Definition.SpriteFilePath);
            }
            else
            {
                SelectedObjectDefinitionBitmap = new();
            }
        }

        public string? GetName()
        {
            if (SelectedObject is not null)
            {
                return SelectedObject.Definition.Name;
            }
            else if (SelectedObjectDefinition >= 0)
            {
                return Session.Project.LevelObjectDefinitions[SelectedObjectDefinition].Name;
            }
            else
            {
                return "";
            }
        }
    }
}
