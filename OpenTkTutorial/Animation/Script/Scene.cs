using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenTkProject.Animation
{
    public class Scene
    {
        [JsonInclude] public Dictionary<string, Animation.Action> actions;
        [JsonInclude] public int Duration;

        public Scene()
        {
            this.actions = new Dictionary<string, Animation.Action>();
            this.Duration = 0;
        }

        [JsonConstructor]
        public Scene(Dictionary<string, Animation.Action> actions)
        {
            this.actions = actions;
        }

        public void Add(string name, Animation.Action action)
        {
            actions.Add(name, action);
            if(action.start + action.duration > Duration) this.Duration = action.start + action.duration;
        }
        public void Remove(string name)
        {
            actions.Remove(name);
            Duration = 0;
            foreach (var item in actions)
            {
                Action action = item.Value;
                if(action.start + action.duration > Duration) this.Duration = action.start + action.duration;
            }

        }
    }
}
