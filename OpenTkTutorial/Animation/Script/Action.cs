using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenTkProject.Animation
{
    public enum ActionType
    {
        Object,
        Face,
    }

    public class Action
    {
        [JsonInclude] public ActionType type;
        [JsonInclude] public string objectName;
        [JsonInclude] public string faceName;
        [JsonInclude] public Transformation transformation;
        [JsonInclude] public float range;
        [JsonInclude] public Axis axis;
        [JsonInclude] public int start;
        [JsonInclude] public int steps;
        [JsonInclude] public int duration;

        public Action(string objectName,
            Transformation transformation, Axis axis,
            float range, int steps,
            int start, int duration)
        {
            this.type = ActionType.Object;
            this.objectName = objectName;
            this.transformation = transformation;
            this.range = range;
            this.axis = axis;
            this.start = start;
            this.steps = steps;
            this.duration = duration;
        }

        public Action(string objectName, string faceName,
            Transformation transformation, Axis axis,
            float range, int steps,
            int start, int duration)
        {
            this.type = ActionType.Face;
            this.objectName = objectName;
            this.faceName = faceName;
            this.transformation = transformation;
            this.range = range;
            this.axis = axis;
            this.start = start;
            this.steps = steps;
            this.duration = duration;
        }

        [JsonConstructor]
        public Action(string objectName, string faceName, ActionType type,
            Transformation transformation, Axis axis,
            float range, int steps,
            int start, int duration)
        {
            this.type = type;
            this.objectName = objectName;
            this.faceName = faceName;
            this.transformation = transformation;
            this.range = range;
            this.axis = axis;
            this.start = start;
            this.steps = steps;
            this.duration = duration;
        }
    }
}
