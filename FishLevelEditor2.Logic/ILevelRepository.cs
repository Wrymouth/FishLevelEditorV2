using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishLevelEditor2.Logic
{
    public interface ILevelRepository
    {
        public ExportSettings ExportSettings { get; set; }
        public void Export(Level level, string folderPath);
    }
}
