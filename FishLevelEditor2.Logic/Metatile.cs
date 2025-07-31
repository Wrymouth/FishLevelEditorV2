namespace FishLevelEditor2.Logic
{
    public class Metatile
    {
        public enum MetatileType : uint
        {
            Air,
            Solid,
            SemiSolid,
            Spike,
            Climb
        }
        public enum Entries : int
        {
            TopLeft = 0,
            TopRight = 1,
            BottomLeft = 2,
            BottomRight = 3,
            Collision = 4
        }

        public string Name { get; set; }
        public MetatileType Type { get; set; }
        public uint[] Tiles { get; set; }

        public Metatile(string name, MetatileType type, uint tile0, uint tile1, uint tile2, uint tile3)
        {
            Name = name;
            Type = type;
            Tiles = [tile0, tile1, tile2, tile3];
        }

        // empty constructor for Json Deserialize
        public Metatile()
        {

        }

    }
}