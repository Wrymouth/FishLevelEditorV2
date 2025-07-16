using FishLevelEditor2.Logic;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;

namespace FishLevelEditor2.ViewModels
{
    public class SelectedMetatileViewModel : INotifyPropertyChanged
    {
        private uint _metatileIndex;

        public event PropertyChangedEventHandler? PropertyChanged;

        public CHRBasedBitmap SelectedMetatileBitmap { get; set; }

        public uint MetatileIndex
        {
            get => _metatileIndex;
            set
            {
                _metatileIndex = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MetatileIndex"));
            }
        }

        public Metatile GetMetatileFromSet(MetatileSet metatileSet)
        {
            return metatileSet.Metatiles[(int) MetatileIndex];
        }
        public SelectedMetatileViewModel(uint metatileIndex, CHRBank chrBank)
        {
            MetatileIndex = metatileIndex;
            SelectedMetatileBitmap = new(16, 16, chrBank);
        }


        public void Display(Palette palette, MetatileSet metatileSet)
        {
            SelectedMetatileBitmap.DrawMetatile(GetMetatileFromSet(metatileSet), 0, 0, palette);
        }
    }
}