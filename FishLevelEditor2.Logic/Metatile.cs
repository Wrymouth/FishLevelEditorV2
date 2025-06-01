namespace FishLevelEditor2.Logic
{
    public class Metatile
    {
        public enum Type : uint
        {
            Air,
            Solid,
            SemiSolid,
            Spike,
            Climb
        }

        public string Name { get; set; }
        public Type MetatileType { get; set; }
        public uint[] Tiles { get; set; }

    }
}