using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishLevelEditor2.Logic
{
    public class Project
    {
        public ObservableCollection<Level> Levels { get; set; }
        public List<MetatileSet> MetatileSets { get; set; }

        public int MostRecentLevelIndex { get; set; }

        [JsonIgnore]
        public IProjectRepository ProjectRepository { get; set; }

        public Project(IProjectRepository projectRepository)
        {
            ProjectRepository = projectRepository;
            Levels = [];
            MetatileSets = [];
            MostRecentLevelIndex = -1;
        }

        public Project(IProjectRepository projectRepository, ObservableCollection<Level> levels, ObservableCollection<CHRBank> chrBanks, List<MetatileSet> metatileSets)
        {
            ProjectRepository = projectRepository;
            Levels = levels;
            MetatileSets = metatileSets;
        }

        // empty constructor for Json Deserialize
        public Project()
        {

        }

        public void Save(string filePath)
        {
            ProjectRepository.Save(this, filePath);
        }
    }
}
