using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishLevelEditor2.Logic
{
    public class ExportSettings
    {
        public enum ExportFormat
        {
            Screens,
            Columns,
            Rows,
            Map
        }
        public bool SaveAttributes { get; set; }
        public ExportFormat Format { get; set; }

        public ExportSettings(bool saveAttributes, ExportFormat format)
        {
            SaveAttributes = saveAttributes;
            Format = format;
        }
    }
}
