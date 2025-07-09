using Avalonia.Media.Imaging;
using FishLevelEditor2.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishLevelEditor2.ViewModels
{
    public class CHRBankViewModel
    {
        public const int CHR_IMAGE_WIDTH  = 128;
        public const int CHR_IMAGE_HEIGHT = 128;
        public CHRBank CHRBank { get; set; }
        public EditorBitmap CHRBankBitmap { get; set; }
        public CHRBankViewModel(CHRBank chrBank)
        {
            CHRBank = chrBank;
            CHRBankBitmap = new(CHR_IMAGE_WIDTH, CHR_IMAGE_HEIGHT);
        }
        public void Display()
        {
            int posX = 0;
            int posY = 0;

            foreach (var tile in CHRBank.Tiles)
            {
                CHRBankBitmap.WriteCHRTile(tile, posX, posY, new Palette(0x0F, 0x00, 0x10, 0x30));
                posX += 8;
                if (posX >= CHR_IMAGE_WIDTH)
                {
                    posX = 0;
                    posY += 8;
                }
            }
        }
    }
}
