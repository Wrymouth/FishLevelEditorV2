using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishLevelEditor2.Logic
{
    public interface IProjectRepository
    {
        public void Save(Project project, string fileName);

        public Project Load(string fileName);
    }
}
