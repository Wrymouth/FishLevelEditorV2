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
        public const int PALETTE_DISPLAY_WIDTH = 32;
        public const int PALETTE_DISPLAY_HEIGHT = 8;
        public const int AMOUNT_COLORS_PALETTE = 4;
        public Palette[] Palettes { get; set; }

        public SKBitmap Palette0Bitmap { get; set; }
        public SKBitmap Palette1Bitmap { get; set; }
        public SKBitmap Palette2Bitmap { get; set; }
        public SKBitmap Palette3Bitmap { get; set; }

        public PalettesViewModel(Palette[] palettes)
        {
            Palettes = palettes;
            Palette0Bitmap = new(PALETTE_DISPLAY_WIDTH, PALETTE_DISPLAY_HEIGHT, SKColorType.Rgba8888, SKAlphaType.Opaque);
            Palette1Bitmap = new(PALETTE_DISPLAY_WIDTH, PALETTE_DISPLAY_HEIGHT, SKColorType.Rgba8888, SKAlphaType.Opaque);
            Palette2Bitmap = new(PALETTE_DISPLAY_WIDTH, PALETTE_DISPLAY_HEIGHT, SKColorType.Rgba8888, SKAlphaType.Opaque);
            Palette3Bitmap = new(PALETTE_DISPLAY_WIDTH, PALETTE_DISPLAY_HEIGHT, SKColorType.Rgba8888, SKAlphaType.Opaque);
        }

        public void Display()
        {
            int posX = 0;
            int posY = 0;

            for (uint i = 0; i < AMOUNT_COLORS_PALETTE; i++)
            {
                BitmapUtils.DrawColorTile(Palette0Bitmap, posX, posY, Palettes[0].Colors[i]);
                BitmapUtils.DrawColorTile(Palette1Bitmap, posX, posY, Palettes[1].Colors[i]);
                BitmapUtils.DrawColorTile(Palette2Bitmap, posX, posY, Palettes[2].Colors[i]);
                BitmapUtils.DrawColorTile(Palette3Bitmap, posX, posY, Palettes[3].Colors[i]);
                posX += 8;
            }
        }
    }
}
