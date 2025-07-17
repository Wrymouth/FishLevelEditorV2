namespace FishLevelEditor2.Logic
{
    public class Palette
    {
        public uint[] Colors { get; set; }

        public Palette(uint color0, uint color1, uint color2, uint color3)
        {
            Colors = [
                color0,
                color1,
                color2,
                color3
                ];
        }

        // empty constructor for Json Deserialize
        public Palette()
        {
            
        }
    }
}