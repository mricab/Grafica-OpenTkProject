using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenTkProject.Animation
{
    public class Script
    {
        [JsonInclude] public List<Scene> scenes;

        public Script()
        {
            this.scenes = new List<Scene>();
        }


        [JsonConstructor]
        public Script(List<Scene> scenes)
        {
            this.scenes = scenes;
        }

        public void Add(Scene scene)
        {
            scenes.Add(scene);
        }

        /* Serialization Methods */

        public void Serialize(string filename)
        {
            string jsonWriteString = JsonSerializer.Serialize<Script>(this);
            File.WriteAllText(filename, jsonWriteString);
        }

        static public Script Deserialize(string filename)
        {
            string jsonReadString = File.ReadAllText(filename);
            return JsonSerializer.Deserialize<Script>(jsonReadString);
        }
    }
}
