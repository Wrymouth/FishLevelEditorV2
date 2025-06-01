using FishLevelEditor2.Logic;
using Newtonsoft.Json;

namespace FishLevelEditor2.DataAccess
{
    // saves configs to/loads configs from a disk file
    public class ConfigRepository : IConfigRepository
    {
        const string DEFAULT_MASTER_PALETTE_LOCATION = @"Content/ntscpalette.pal";
        const string CONFIG_FILEPATH = @"Content/editorcfg.json";

        /// <summary>
        /// Saves the file to disk
        /// </summary>
        /// <returns>whether the save was successful or not</returns>
        public bool Save()
        {
            return false;
        }

        public Config Load()
        {
            string fileName = CONFIG_FILEPATH;
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                Config? c = JsonConvert.DeserializeObject<Config>(json);
                if (c is not null)
                {
                    return new Config(this, c.MasterPaletteFilePath, c.RecentProjectFilePath);
                }
                else
                {
                    return new Config(this, DEFAULT_MASTER_PALETTE_LOCATION, null);
                }
            }
            else
            {
                // create the file
                File.Create(filePath);
                Config c = new(this, DEFAULT_MASTER_PALETTE_LOCATION, null);
                string json = JsonConvert.SerializeObject(c);
                File.WriteAllText(filePath, json);
                return c;
            }
        }
    }
}
