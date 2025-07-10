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
            Metatiles.Add(new Metatile("Empty", Metatile.MetatileType.Air, 0,0,0,0));
        }
    }
}