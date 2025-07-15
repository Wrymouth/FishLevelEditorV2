namespace FishLevelEditor2.Logic
{
    public class ScreenMetatile
    {
        public uint Metatile { get; set; }
        public uint Palette { get; set; }

        public ScreenMetatile(uint metatile, uint palette)
        {
            Metatile = metatile;
            Palette = palette;
        }
    }
}