using FishLevelEditor2.Logic;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishLevelEditor2.ViewModels
{
    public class MasterPaletteViewModel
    {
        public const int MASTER_PALETTE_DISPLAY_WIDTH = 128;
        public const int MASTER_PALETTE_DISPLAY_HEIGHT = 32;
        public SKBitmap MasterPaletteBitmap { get; set; }

        public MasterPaletteViewModel()
        {
            MasterPaletteBitmap = new(MASTER_PALETTE_DISPLAY_WIDTH, MASTER_PALETTE_DISPLAY_HEIGHT, SKColorType.Rgba8888, SKAlphaType.Opaque);
        }

        public void Display()
        {
            int posX = 0;
            int posY = 0;

            for (uint i = 0; i < MasterPalette.AMOUNT_COLORS; i++)
            {
                BitmapUtils.DrawColorTile(MasterPaletteBitmap, posX, posY, i);
                posX += 8;
                if (posX >= MASTER_PALETTE_DISPLAY_WIDTH)
                {
                    posX = 0;
                    posY += 8;
                }
            }
        }
    }
}
