using FishLevelEditor2.Logic;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;

namespace FishLevelEditor2.ViewModels
{
    public class SelectedMetatileViewModel
    {
        public CHRBasedBitmap SelectedMetatileBitmap { get; set; }

        public Metatile Metatile { get; set; }

        public SelectedMetatileViewModel(Metatile metatile, CHRBank chrBank)
        {
            Metatile = metatile;
            SelectedMetatileBitmap = new(16,16, chrBank);
        }

        public void Display(Palette palette)
        {
            SelectedMetatileBitmap.DrawMetatile(Metatile, 0, 0, palette);
        }
    }
}