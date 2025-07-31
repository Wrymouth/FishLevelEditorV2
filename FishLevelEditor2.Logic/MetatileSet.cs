using Newtonsoft.Json;

namespace FishLevelEditor2.Logic
{
    public class MetatileSet
    {
        public const int MAX_METATILES_IN_SET = 256;
        public string Name { get; set; }
        public List<Metatile> Metatiles { get; set; }

        [JsonIgnore]
        public string FileName { get => Name.ToLower().Replace(" ", "_").Replace("-", ""); }

        public MetatileSet(string name)
        {
            Name = name;
            Metatiles = [];
            Metatiles.Add(new Metatile("Empty", Metatile.MetatileType.Air, 0, 0, 0, 0));
        }

        // empty constructor for Json Deserialize
        public MetatileSet()
        {

        }
    }
}