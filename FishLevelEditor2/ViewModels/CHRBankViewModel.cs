﻿using Avalonia.Media.Imaging;
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
        public CHRBasedBitmap CHRBankBitmap { get; set; }
        public uint SelectedTileIndex { get; set; }
        public CHRBankViewModel(CHRBank chrBank)
        {
            CHRBank = chrBank;
            CHRBankBitmap = new(CHR_IMAGE_WIDTH, CHR_IMAGE_HEIGHT, chrBank);
            SelectedTileIndex = 0;
        }
        public void Display(Palette palette)
        {
            int posX = 0;
            int posY = 0;

            for (int i = 0; i < CHRBank.Tiles.Count; i++)
            {
                CHRBankBitmap.DrawCHRTile((uint) i, posX, posY, palette);
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
