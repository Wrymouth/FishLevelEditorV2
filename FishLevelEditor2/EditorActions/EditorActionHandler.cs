using Avalonia.Controls.Platform;
using FishLevelEditor2.Logic;
using FishLevelEditor2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace FishLevelEditor2.EditorActions
{
    public delegate void EditorActionLogEventHandler(object sender, EditorActionLogEventArgs e);
    public static class EditorActionHandler
    {
        public const int ACTION_LIST_SIZE = 256;

        public static event EditorActionLogEventHandler Log;
        public static List<EditorAction> EditorActions { get; set; }

        // index of the last performed action
        public static int ActionListIndex { get; set; }

        static EditorActionHandler()
        {
            EditorActions = [];
            ActionListIndex = -1;
        }

        public static void Do(EditorAction action, MainViewModel mvm)
        {
            ActionListIndex++;
            if (ActionListIndex < EditorActions.Count)
            {
                // remove all elements after the last performed action
                EditorActions.RemoveRange(ActionListIndex, (EditorActions.Count - ActionListIndex));
            }
            EditorActions.Add(action);
            action.Do(mvm);
            Log?.Invoke(null, new(action.LogMessage));
        }

        public static void Undo(MainViewModel mvm)
        {
            if (ActionListIndex < 0)
            {
                Log?.Invoke(null, new("Nothing to undo."));
                return;
            }
            EditorActions[ActionListIndex].Undo(mvm);
            Log?.Invoke(null, new($"Undo {EditorActions[ActionListIndex].LogMessage}"));
            ActionListIndex--;

        }

        public static void Redo(MainViewModel mvm)
        {
            if (ActionListIndex >= EditorActions.Count - 1)
            {
                Log?.Invoke(null, new("Nothing to redo."));
                return;
            }
            ActionListIndex++;
            EditorActions[ActionListIndex].Do(mvm);
            Log?.Invoke(null, new($"Redo {EditorActions[ActionListIndex].LogMessage}"));
        }

    }
}
