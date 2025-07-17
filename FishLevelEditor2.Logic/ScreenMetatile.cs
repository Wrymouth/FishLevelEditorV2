namespace FishLevelEditor2.Logic
{
    public class ScreenMetatile
    {
        // metatile index and palette index, shortened for smaller json file size
        public uint mi { get; set; }
        public uint pi { get; set; }

        public ScreenMetatile(uint metatile, uint palette)
        {
            mi = metatile;
            pi = palette;
        }

        // copy
        public ScreenMetatile(ScreenMetatile screenMetatile)
        {
            mi = screenMetatile.mi;
            pi = screenMetatile.pi;
        }

        // empty constructor for Json Deserialize
        public ScreenMetatile()
        {
            
        }
    }
}