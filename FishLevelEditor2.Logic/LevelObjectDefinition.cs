using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishLevelEditor2.Logic
{
    public class LevelObjectDefinition
    {
        public enum ObjectTypes
        {
            Regular,
            Small
        }
        public ObjectTypes Type { get; set; }
        public string Name { get; set; }
        public string VarName { get; set; }

        public string SpriteFilePath { get; set; }
        public LevelObjectDefinition()
        {
            Type = ObjectTypes.Regular;
            Name = "object";
        }

        public LevelObjectDefinition(ObjectTypes type, string name, string varName, string spriteFilePath)
        {
            Type = type;
            Name = name;
            VarName = varName;
            SpriteFilePath = spriteFilePath;
        }
    }
}
