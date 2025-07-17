using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishLevelEditor2.Logic
{
    public interface IConfigRepository
    {
        public bool Save(Config config);

        public Config Load();
    }
}
