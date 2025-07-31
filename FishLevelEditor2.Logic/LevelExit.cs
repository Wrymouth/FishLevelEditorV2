namespace FishLevelEditor2.Logic
{
    public class LevelExit
    {
        public int DestLevel { get; set; }
        public int DestEntry { get; set; }
        public TransitionTypes TransitionType { get; set; }

        public enum TransitionTypes
        {
            Cut,
            FadeBlack,
            FadeWhite,
            ScrollLeft,
            ScrollRight,
            ScrollUp,
            ScrollDown
        }

        public LevelExit()
        {
            DestLevel = 0;
            DestEntry = 0;

            TransitionType = TransitionTypes.Cut;
        }

        public LevelExit(int destLevel, int destEntry, TransitionTypes transitionType)
        {
            DestLevel = destLevel;
            DestEntry = destEntry;
            TransitionType = transitionType;
        }
    }
}