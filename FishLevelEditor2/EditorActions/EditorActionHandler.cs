using Avalonia.Controls.Platform;
using FishLevelEditor2.Logic;
using FishLevelEditor2.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace FishLevelEditor2.EditorActions
{
    public delegate void EditorActionLogEventHandler(object sender, EditorActionLogEventArgs e);
    public delegate void UnsavedChangesEventHandler(object sender, EventArgs e);
    public static class EditorActionHandler
    {
        public const int ACTION_LIST_SIZE = 256;

        public static event EditorActionLogEventHandler Log;
        public static event UnsavedChangesEventHandler UnsavedChangesEvent;

        public static List<EditorAction> EditorActions { get; set; }
        public static int LastPerformedActionIndex { get; set; }


        private static int lastSavedActionIndex;
        public static int LastSavedActionIndex
        {
            get => lastSavedActionIndex; set
            {
                lastSavedActionIndex = value;
                CheckUnsavedChanges();
            }
        }

        private static bool _unsavedChanges;
        public static bool UnsavedChanges
        {
            get { return _unsavedChanges; }
            set
            {
                _unsavedChanges = value;
                UnsavedChangesEvent?.Invoke(null, new EventArgs());
            }
        }


        static EditorActionHandler()
        {
            EditorActions = [];
            LastPerformedActionIndex = -1;
            LastSavedActionIndex = -1;
        }

        private static void CheckUnsavedChanges()
        {
            UnsavedChanges = !(LastPerformedActionIndex == LastSavedActionIndex);

        }

        public static void Do(EditorAction action, MainViewModel mvm)
        {
            LastPerformedActionIndex++;
            if (LastPerformedActionIndex < EditorActions.Count)
            {
                // remove all elements after the last performed action
                EditorActions.RemoveRange(LastPerformedActionIndex, (EditorActions.Count - LastPerformedActionIndex));
            }
            EditorActions.Add(action);
            action.Do(mvm);
            Log?.Invoke(null, new(action.LogMessage));
            UnsavedChanges = true;
        }

        public static void Undo(MainViewModel mvm)
        {
            if (LastPerformedActionIndex < 0)
            {
                Log?.Invoke(null, new("Nothing to undo."));
                return;
            }
            EditorActions[LastPerformedActionIndex].Undo(mvm);
            Log?.Invoke(null, new($"Undo {EditorActions[LastPerformedActionIndex].LogMessage}"));
            LastPerformedActionIndex--;
            CheckUnsavedChanges();
        }

        public static void Redo(MainViewModel mvm)
        {
            if (LastPerformedActionIndex >= EditorActions.Count - 1)
            {
                Log?.Invoke(null, new("Nothing to redo."));
                return;
            }
            LastPerformedActionIndex++;
            CheckUnsavedChanges();
            EditorActions[LastPerformedActionIndex].Do(mvm);
            Log?.Invoke(null, new($"Redo {EditorActions[LastPerformedActionIndex].LogMessage}"));
        }

    }
}
