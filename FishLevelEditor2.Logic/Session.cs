using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishLevelEditor2.Logic
{
    public static class Session
    {
        public static MasterPalette MasterPalette { get; set; }
        public static Project Project { get; set; }
        public static Config Config { get; set; }
    }
}
