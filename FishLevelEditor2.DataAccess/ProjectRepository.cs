using FishLevelEditor2.Logic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FishLevelEditor2.DataAccess
{
    public class ProjectRepository : IProjectRepository
    {
        public Project Load(string fileName)
        {
            string json = File.ReadAllText(fileName);
            Project project = JsonConvert.DeserializeObject<Project>(json);
            project.ProjectRepository = this;
            return project;
        }

        public void Save(Project project, string fileName)
        {
            var options = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            string jsonString = JsonConvert.SerializeObject(project, options);
            File.WriteAllText(fileName, jsonString);
        }
    }
}
