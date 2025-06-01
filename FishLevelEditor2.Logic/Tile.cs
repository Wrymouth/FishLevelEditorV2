using System.Text.Json.Serialization;

namespace FishLevelEditor2.Logic
{
    public class Tile
    {
        public const int TILE_WIDTH = 8;
        public const int TILE_HEIGHT = 8;
        public uint Index { get; set; }

        [JsonIgnore]
        public uint[] Pixels { get; private set; }

        public Tile(uint tileIndex, uint[] pixels)
        {
            Index = tileIndex;
            Pixels = pixels;
        }
    }
}