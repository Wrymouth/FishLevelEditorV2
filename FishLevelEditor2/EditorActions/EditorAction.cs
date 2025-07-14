using FishLevelEditor2.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishLevelEditor2.EditorActions
{
    public abstract class EditorAction
    {
        public abstract string LogMessage { get; }

        public abstract void Do(Level level);

        public abstract void Undo(Level level);

    }
}
