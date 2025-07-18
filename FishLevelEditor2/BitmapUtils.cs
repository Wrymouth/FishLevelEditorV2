using Avalonia.Media.Imaging;
using FishLevelEditor2.Logic;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishLevelEditor2
{
    public static class BitmapUtils
    {
        /// <summary>
        /// Takes a 2bpp color index, converts it to RGBA
        /// </summary>
        /// <param name="paletteIndex">the 2bpp color index</param>
        /// <param name="palette">the palette used</param>
        /// <returns>RGBA8888 color value</returns>
        public static SKColor ResolvePaletteColor(uint paletteIndex, Palette palette)
        {
            uint colorValue = Session.MasterPalette.Colors[palette.Colors[paletteIndex]];
            Color color = Color.FromArgb((byte)(colorValue & 0xFF), (byte)(colorValue >> 24), (byte)((colorValue >> 16) & 0xFF), (byte)((colorValue >> 8) & 0xFF));
            return new SKColor(color.R, color.G, color.B, color.A);
        }

        public static void DrawColorTile(SKBitmap bitmap, int startX, int startY, uint colorIndex)
        {
            Span<byte> buffer = bitmap.GetPixelSpan();
            int stride = bitmap.RowBytes;
            int width = bitmap.Width;
            int height = bitmap.Height;

            for (int tileY = 0; tileY < Tile.TILE_HEIGHT; tileY++)
            {
                int destY = startY + tileY;
                if (destY < 0 || destY >= height) continue;

                for (int tileX = 0; tileX < Tile.TILE_WIDTH; tileX++)
                {
                    int destX = startX + tileX;
                    if (destX < 0 || destX >= width) continue;

                    int pixelIndex = tileY * Tile.TILE_WIDTH + tileX;
                    uint colorValue = Session.MasterPalette.Colors[colorIndex];
                    Color color = Color.FromArgb((byte)(colorValue & 0xFF), (byte)(colorValue >> 24), (byte)((colorValue >> 16) & 0xFF), (byte)((colorValue >> 8) & 0xFF));
                    SKColor skColor = new SKColor(color.R, color.G, color.B, color.A);

                    int offset = destY * stride + destX * 4;

                    // Force alpha to 255 (opaque) since SKAlphaType is Opaque
                    buffer[offset + 0] = skColor.Red;
                    buffer[offset + 1] = skColor.Green;
                    buffer[offset + 2] = skColor.Blue;
                    buffer[offset + 3] = 255;
                }
            }
        }
    }
}
