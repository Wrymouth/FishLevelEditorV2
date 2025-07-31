namespace FishLevelEditor2.Logic
{
    public class LevelObject
    {
        public enum ObjectTypes
        {
            Regular,
            Small
        }
        public int Column { get; set; }
        public int Row { get; set; }
        public ObjectTypes Type { get; set; }
        public string Name { get; set; }
        public int Var { get; set; }

        public LevelObject()
        {
            Type = ObjectTypes.Regular;
            Column = 0;
            Row = 0;
            Name = "object";
            Var = 0;
        }

        public LevelObject(ObjectTypes type, int col, int row, string name, int var)
        {
            Type = type;
            Column = col;
            Row = row;
            Name = name;
            Var = var;
        }
    }
}