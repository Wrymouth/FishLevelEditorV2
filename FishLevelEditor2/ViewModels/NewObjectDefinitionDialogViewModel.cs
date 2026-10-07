using FishLevelEditor2.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishLevelEditor2.ViewModels
{
    public class NewObjectDefinitionDialogViewModel : ViewModelBase
    {
        public LevelObjectDefinition CreatedObject { get; set; }

        public void CreateLevelObjectDefinition(LevelObjectDefinition.ObjectTypes type, string name, string varName, string spriteFilePath)
        {
            CreatedObject = new(type, name, varName, spriteFilePath);
        }
    }
}
