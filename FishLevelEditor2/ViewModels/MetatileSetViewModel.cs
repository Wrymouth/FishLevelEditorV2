using FishLevelEditor2.Logic;

namespace FishLevelEditor2.ViewModels
{
    public class MetatileSetViewModel
    {
        public MetatileSet MetatileSet { get; set; }
        public CHRBasedBitmap MetatileSetBitmap { get; set; }

        public MetatileSetViewModel(MetatileSet metatileSet, CHRBank chrBank)
        {
            MetatileSet = metatileSet;
            MetatileSetBitmap = new(128, 512, chrBank);
        }

        public void Display()
        {

        }
    }
}