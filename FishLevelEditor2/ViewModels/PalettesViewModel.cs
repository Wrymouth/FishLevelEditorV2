using FishLevelEditor2.Logic;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishLevelEditor2.ViewModels
{
    public class PalettesViewModel
    {
        public Palette[] Palettes { get; set; }

        public SKBitmap Palette0Bitmap { get; set; }
        public SKBitmap Palette1Bitmap { get; set; }
        public SKBitmap Palette2Bitmap { get; set; }
        public SKBitmap Palette3Bitmap { get; set; }

        public PalettesViewModel(Palette[] palettes)
        {
            Palettes = palettes;
            Palette0Bitmap = new(32, 8, SKColorType.Rgba8888, SKAlphaType.Opaque);
            Palette1Bitmap = new(32, 8, SKColorType.Rgba8888, SKAlphaType.Opaque);
            Palette2Bitmap = new(32, 8, SKColorType.Rgba8888, SKAlphaType.Opaque);
            Palette3Bitmap = new(32, 8, SKColorType.Rgba8888, SKAlphaType.Opaque);
        }

        public void Display()
        {

        }
    }
}
