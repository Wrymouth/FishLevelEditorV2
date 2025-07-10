using Avalonia.Media.Imaging;
using FishLevelEditor2.Logic;

namespace FishLevelEditor2.ViewModels
{
    public class MetatileSetViewModel
    {
        public const int METATILE_SET_IMAGE_WIDTH = 128;
        public const int METATILE_SET_IMAGE_HEIGHT = 512;
        public MetatileSet MetatileSet { get; set; }
        public CHRBasedBitmap MetatileSetBitmap { get; set; }

        public MetatileSetViewModel(MetatileSet metatileSet, CHRBank chrBank)
        {
            MetatileSet = metatileSet;
            MetatileSetBitmap = new(METATILE_SET_IMAGE_WIDTH, METATILE_SET_IMAGE_HEIGHT, chrBank);
        }

        public void Display(Palette palette)
        {
            int posX = 0;
            int posY = 0;
            foreach (var metatile in MetatileSet.Metatiles)
            {
                MetatileSetBitmap.DrawMetatile(metatile, posX, posY, palette);
                posX += 16;
                if (posX >= METATILE_SET_IMAGE_WIDTH)
                {
                    posX = 0;
                    posY += 16;
                }
            }
        }
    }
}