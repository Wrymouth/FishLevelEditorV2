namespace FishLevelEditor2.Logic
{
    public class CHRBank
    {
        public string FilePath { get; set; }
        public int Size { get; set; }

        public Tile[] Tiles { get; set; }

        public CHRBank(string filePath, uint size)
        {
            FilePath = filePath;
            Decode();
        }

        private void Decode()
        {
            const int ONE_KB = 1024;
            const int AMOUNT_TILE_PIXELS = 64;
            // Open the file to read from.
            byte[] chrBytes = File.ReadAllBytes(FilePath);
            uint tileIndex = 0;
            int chrIndex = 0;
            FileInfo chrInfo = new FileInfo(FilePath);

            Size = (int) chrInfo.Length / ONE_KB;

            while (tileIndex < Size)
            {
                uint[] tilePixels = new uint[AMOUNT_TILE_PIXELS];

                for (int i = 0; i < 8; i++)
                {
                    // take each bit from byte X and each bit from byte X + 8, then shift the latter left once and OR the two
                    for (int j = 7; j >= 0; j--)
                    {
                        tilePixels[(i * 8) + (7 - j)] = DecodeTileColorValue((uint)chrBytes[chrIndex + i] >> j & 1, (uint)chrBytes[chrIndex + i + 8] >> j & 1);
                    }
                }

                Tile t = new(tileIndex, tilePixels);
                Tiles[tileIndex] = t;
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
