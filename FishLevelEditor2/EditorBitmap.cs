using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace FishLevelEditor2
{
    public class EditorBitmap
    {
        public WriteableBitmap Bitmap { get; set; }

        public EditorBitmap(int width, int height)
        {

        }
    }
}
