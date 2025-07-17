using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishLevelEditor2.Logic
{
    public class Config
    {
        public string? MasterPaletteFilePath { get; set; }
        public string? RecentProjectFilePath { get; set; }

        [JsonIgnore]
        public IConfigRepository ConfigRepository { get; set; }

        // empty constructor for Json Deserialize
        public Config() { }
        public Config(IConfigRepository configRepository)
        {
            ConfigRepository = configRepository;
            Config cfg = ConfigRepository.Load();
            MasterPaletteFilePath = cfg.MasterPaletteFilePath;
            RecentProjectFilePath = cfg.RecentProjectFilePath;
        }

        public Config(IConfigRepository configRepository, string? masterPaletteFilePath, string? recentProjectFilePath)
        {
            ConfigRepository = configRepository;
            MasterPaletteFilePath = masterPaletteFilePath;
            RecentProjectFilePath = recentProjectFilePath;
        }

        public void Save()
        {
            ConfigRepository.Save(this);
        }
    }
}
