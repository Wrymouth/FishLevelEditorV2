using System;

namespace FishLevelEditor2.EditorActions
{
    public class EditorActionLogEventArgs : EventArgs
    {
        public string LogMessage { get; set; }

        public EditorActionLogEventArgs(string logMessage)
        {
            LogMessage = logMessage;
        }
    }
}