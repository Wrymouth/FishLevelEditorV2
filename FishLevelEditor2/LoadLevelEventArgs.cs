using FishLevelEditor2.Logic;
using System;

namespace FishLevelEditor2
{
    public class LoadLevelEventArgs : EventArgs
    {
        public int LevelIndex { get; set; }

        public LoadLevelEventArgs(int levelIndex)
        {
            LevelIndex = levelIndex;
        }
    }
}