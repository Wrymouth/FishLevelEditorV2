using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishLevelEditor2.Logic
{
    public class Project
    {
        public List<Level> Levels { get; set; }

        public int MostRecentLevelIndex { get; set; }
        public List<CHRBank> CHRBanks { get; set; }

        public IProjectRepository ProjectRepository { get; set; }

        public Project(IProjectRepository projectRepository)
        {
            ProjectRepository = projectRepository;
            Levels = [];
            CHRBanks = [];
        }

        public Project(IProjectRepository projectRepository, List<Level> levels, List<CHRBank> chrBanks)
        {
            ProjectRepository = projectRepository;
            Levels = levels;
            CHRBanks = chrBanks;
        }
    }
}
