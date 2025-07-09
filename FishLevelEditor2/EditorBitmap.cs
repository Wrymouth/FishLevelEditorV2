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
    public class EditorBitmap
    {
        const int BYTES_PER_PIXEL = 4; // R,G,B,A
        public SKBitmap Bitmap { get; set; }
        public int Stride { get; private set; }
        public uint[] PixelBuffer { get; set; }

        public EditorBitmap(int width, int height)
        {
            Bitmap = new(width, height, SKColorType.Rgba8888, SKAlphaType.Opaque);
            Stride = width * BYTES_PER_PIXEL;
            PixelBuffer = new uint[width * height];
        }

        public void WriteCHRTile(Tile tile, int startX, int startY, Palette palette)
        {
            for (int tileY = 0; tileY < Tile.TILE_HEIGHT; tileY++)
            {
                for (int tileX = 0; tileX < Tile.TILE_WIDTH; tileX++)
                {
                    int pixelIndex = tileY * Tile.TILE_WIDTH + tileX;
                    Bitmap.SetPixel(startX+tileX, startY+tileY, ResolvePaletteColor(tile.Pixels[pixelIndex], palette));
                }
            }
        }

        /// <summary>
        /// Takes a 2bpp color index, converts it to RGBA
        /// </summary>
        /// <param name="paletteIndex">the 2bpp color index</param>
        /// <param name="palette">the palette used</param>
        /// <returns>RGBA8888 color value</returns>
        private SKColor ResolvePaletteColor(uint paletteIndex, Palette palette)
        {
            uint colorValue = Session.MasterPalette.Colors[palette.Colors[paletteIndex]];
            Color color = Color.FromArgb((byte)(colorValue & 0xFF), (byte)(colorValue >> 24), (byte)((colorValue >> 16) & 0xFF), (byte)((colorValue >> 8) & 0xFF));
            return new SKColor(color.R, color.G, color.B, color.A);
        }
    }
}
