namespace FishLevelEditor2.Logic
{
    public class MetatileSet
    {
        public string Name { get; set; }
        public List<Metatile> Metatiles { get; set; }

        public MetatileSet(string name)
        {
            Name = name;
            Metatiles = [];
        }
    }
}