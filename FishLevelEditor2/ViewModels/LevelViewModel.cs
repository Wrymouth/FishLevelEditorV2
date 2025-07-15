using FishLevelEditor2.Logic;

namespace FishLevelEditor2.ViewModels
{
    public class LevelViewModel
    {
        public const int MAX_LEVEL_WIDTH = 256;
        public const int MAX_LEVEL_HEIGHT = 240;
        public Level Level { get; set; }
        public CHRBasedBitmap LevelBitmap { get; set; }

        public LevelViewModel(Level level)
        {
            Level = level;
            LevelBitmap = new(level.Width * 16, level.Height * 16, level.BackgroundCHR);
        }

        public void Display()
        {
            int posX = 0;
            int posY = 0;

            LevelBitmap = new(Level.Width * 16, Level.Height * 16, Level.BackgroundCHR);

            for (int y = 0; y < Level.Height; y++)
            {
                for (int x = 0; x < Level.Width; x++)
                {
                    ScreenMetatile screenMetatile = Level.ScreenMetatiles[x][y];
                    LevelBitmap.DrawMetatile(Level.MetatileSet.Metatiles[(int)screenMetatile.Metatile], posX, posY, Level.BackgroundPalettes[screenMetatile.Palette]);
                    posX += 16;
                }
                posX = 0;
                posY += 16;
            }
        }

    }
}