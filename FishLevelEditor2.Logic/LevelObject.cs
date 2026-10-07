namespace FishLevelEditor2.Logic
{
    public class LevelObject
    {
        public LevelObjectDefinition Definition { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
        public string? VarValue { get; set; }

        public LevelObject()
        {
            Definition = new();
            PosX = 0;
            PosY = 0;
        }

        public LevelObject(LevelObjectDefinition definition, int col, int row, string? varValue)
        {
            Definition = definition;
            PosX = col;
            PosY = row;
            VarValue = varValue;
        }
    }
}