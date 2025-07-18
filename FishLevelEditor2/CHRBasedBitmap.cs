using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using FishLevelEditor2.Logic;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FishLevelEditor2
{
    public class CHRBasedBitmap
    {
        public SKBitmap Bitmap { get; set; }
        public CHRBank CHRBank { get; set; }

        public CHRBasedBitmap(int width, int height, CHRBank chrBank)
        {
            Bitmap = new(width, height, SKColorType.Rgba8888, SKAlphaType.Opaque);
            CHRBank = chrBank;
        }

        public void DrawMetatile(Metatile metatile, int startPosX, int startPosY, Palette palette)
        {
            int relativePosX = 0;
            int relativePosY = 0;
            foreach (var tileIndex in metatile.Tiles)
            {
                DrawCHRTile(tileIndex, startPosX + relativePosX, startPosY + relativePosY, palette);
                relativePosX += 8;
                if (relativePosX >= 16)
                {
                    relativePosX = 0;
                    relativePosY += 8;
                }
            }
        }

        public void DrawCHRTile(uint tileIndex, int startX, int startY, Palette palette)
        {
            Span<byte> buffer = Bitmap.GetPixelSpan();
            int stride = Bitmap.RowBytes;
            int width = Bitmap.Width;
            int height = Bitmap.Height;

            Tile tile = CHRBank.Tiles[(int)tileIndex];

            for (int tileY = 0; tileY < Tile.TILE_HEIGHT; tileY++)
            {
                int destY = startY + tileY;
                if (destY < 0 || destY >= height) continue;

                for (int tileX = 0; tileX < Tile.TILE_WIDTH; tileX++)
                {
                    int destX = startX + tileX;
                    if (destX < 0 || destX >= width) continue;

                    int pixelIndex = tileY * Tile.TILE_WIDTH + tileX;
                    SKColor color = BitmapUtils.ResolvePaletteColor(tile.Pixels[pixelIndex], palette);


                    int offset = destY * stride + destX * 4;

                    // Force alpha to 255 (opaque) since SKAlphaType is Opaque
                    buffer[offset + 0] = color.Red;
                    buffer[offset + 1] = color.Green;
                    buffer[offset + 2] = color.Blue;
                    buffer[offset + 3] = 255;
                }
            }
        }
    }
}
