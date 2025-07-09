using System.Globalization;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace FishLevelEditor2.Logic
{
    public class MasterPalette
    {
        const int AMOUNT_COLORS = 64;
        public uint[] Colors { get; set; }

        public MasterPalette(string filePath)
        {
            Colors = new uint[AMOUNT_COLORS];
            SetMasterPaletteFromFile(filePath);
        }
        private void SetMasterPaletteFromFile(string filePath)
        {
            FileStream fileStream = File.OpenRead(filePath);
            for (int i = 0; i < AMOUNT_COLORS; i++)
            {
                int streamTest = fileStream.ReadByte();
                if (streamTest == -1)
                {
                    Colors[i] = 0xFF000000;
                }
                else
                {
                    uint red = (uint)streamTest;
                    uint green = (uint)fileStream.ReadByte();
                    uint blue = (uint)fileStream.ReadByte();
                    Colors[i] = (red << 24) | (green << 16) | (blue << 8) | 0xFF;
                }
            }
        }
    }
}
