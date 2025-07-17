using Newtonsoft.Json;

namespace FishLevelEditor2.Logic
{
    public class Tile
    {
        public const int TILE_WIDTH = 8;
        public const int TILE_HEIGHT = 8;

        [JsonIgnore]
        public uint[] Pixels { get; private set; }

        public Tile(uint[] pixels)
        {
            Pixels = pixels;
        }
    }
}