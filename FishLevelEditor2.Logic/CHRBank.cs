using System.Numerics;

namespace FishLevelEditor2.Logic
{
    public class CHRBank
    {
        public string FilePath { get; set; }

        public string FileName
        {
            get
            {
                return Path.GetFileName(FilePath);
            }
        }
        public int Size { get; set; }

        public List<Tile> Tiles { get; set; }

        public CHRBank(string filePath)
        {
            FilePath = filePath;
            Tiles = [];
            Decode();
        }

        private void Decode()
        {
            const int ONE_KB = 1024;
            const int AMOUNT_PIXELS_PER_TILE = 64;
            const int AMOUNT_TILES_PER_KB = 64;
            // Open the file to read from.
            byte[] chrBytes = File.ReadAllBytes(FilePath);
            uint tileIndex = 0;
            int chrIndex = 0;
            FileInfo chrInfo = new(FilePath);

            Size = (int)chrInfo.Length;
            int amountTiles = (Size / ONE_KB) * AMOUNT_TILES_PER_KB;

            while (tileIndex < amountTiles)
            {
                uint[] tilePixels = new uint[AMOUNT_PIXELS_PER_TILE];

                for (int i = 0; i < 8; i++)
                {
                    // take each bit from byte X and each bit from byte X + 8, then shift the latter left once and OR the two
                    for (int j = 7; j >= 0; j--)
                    {
                        tilePixels[(i * 8) + (7 - j)] = DecodeTileColorValue((uint)chrBytes[chrIndex + i] >> j & 1, (uint)chrBytes[chrIndex + i + 8] >> j & 1);
                    }
                }

                Tile t = new(tilePixels);
                Tiles.Add(t);
                chrIndex += 16;
                tileIndex++;
            }
        }

        // returns a readable color value for each pixel
        private uint DecodeTileColorValue(uint b1, uint b2)
        {

            return (b2 << 1) | b1;
        }
    }
}
