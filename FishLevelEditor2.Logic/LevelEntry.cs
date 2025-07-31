namespace FishLevelEditor2.Logic
{
    public class LevelEntry
    {
        public int PosX { get; set; }
        public int PosY { get; set; }

        public LevelEntry()
        {
            PosX = 0;
            PosY = 0;
        }

        public LevelEntry(int posX, int posY)
        {
            PosX = posX;
            PosY = posY;
        }
    }
}