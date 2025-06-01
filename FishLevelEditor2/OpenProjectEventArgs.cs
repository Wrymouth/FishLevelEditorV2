using FishLevelEditor2.Logic;
using System;

namespace FishLevelEditor2
{
    public class OpenProjectEventArgs : EventArgs
    {
        public Project Project { get; set; }
        public OpenProjectEventArgs(Project project)
        {
            Project = project;
        }
    }
}
