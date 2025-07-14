using Avalonia.Controls.Platform;
using FishLevelEditor2.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace FishLevelEditor2.EditorActions
{
    public static class EditorActionHandler
    {
        public const int ACTION_LIST_SIZE = 256;
        public static List<EditorAction> EditorActions { get; set; }

        // index of the last performed action
        public static int ActionListIndex { get; set; }

        static EditorActionHandler()
        {
            EditorActions = [];
            ActionListIndex = -1;
        }

        public static void Do(EditorAction action, Level level)
        {
            ActionListIndex++;
            if (ActionListIndex < EditorActions.Count)
            {
                // remove all elements after the last performed action
                EditorActions.RemoveRange(ActionListIndex, (EditorActions.Count - ActionListIndex));
            }
            EditorActions.Add(action);
            action.Do(level);
        }

        public static void Undo(Level level)
        {
            if (ActionListIndex >= 0)
            {
                EditorActions[ActionListIndex].Undo(level);
                ActionListIndex--;
            }
        }

        public static void Redo(Level level)
        {
            if (ActionListIndex < EditorActions.Count - 1)
            {
                ActionListIndex++;
                EditorActions[ActionListIndex].Do(level);
            }
        }

    }
}
