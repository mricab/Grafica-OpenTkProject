using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTkProject.Animation;

namespace OpenTkProject.Samples
{
    public class AnimationScript
    {
        static public Script BuildScript()
        {
            // Script
            Script script = new Script();

            // Scene
            Animation.Scene scene = new Animation.Scene();

            // Start Actions
            scene.Add("ant1", new Animation.Action("Ant", Transformation.Translate, Axis.X, 0.6f, 100, 0, 4000));
            scene.Add("bookOpenRotate", new Animation.Action("Book", "Back", Transformation.Rotate, Axis.Z, +180f, 200, 4000, 1000));
            scene.Add("bookOpenTranslate", new Animation.Action("Book", "Back", Transformation.Translate, Axis.Y, -0.06f, 200, 4000, 1000));
            for (int i = 3; i > 1; i--)
            {
                scene.Add("sheetOpenRotate" + i.ToString(), new Animation.Action("Book", "Sheet" + i.ToString(), Transformation.Rotate, Axis.Z, +180f, 200, 4000, 1000));
                scene.Add("sheetOpenTranslate" + i.ToString(), new Animation.Action("Book", "Sheet" + i.ToString(), Transformation.Translate, Axis.Y, -(0.01f * i), 200, 4000, 1000));
            }

            //// Middle Actions
            scene.Add("ant2", new Animation.Action("Ant", Transformation.Translate, Axis.X, 0.6f, 100, 4000, 4000));
            scene.Add("ant3", new Animation.Action("Ant", Transformation.Rotate, Axis.Z, 90.0f, 30, 8500, 500));
            scene.Add("ant4", new Animation.Action("Ant", Transformation.Translate, Axis.Y, 0.02f, 30, 8500, 500));

            //// End Actions
            for (int i = 3; i > 1; i--)
            {
                scene.Add("sheetCloseRotate" + i.ToString(), new Animation.Action("Book", "Sheet" + i.ToString(), Transformation.Rotate, Axis.Z, -180.0f, 10, 9500, 500));
                scene.Add("sheetCloseTranslate" + i.ToString(), new Animation.Action("Book", "Sheet" + i.ToString(), Transformation.Translate, Axis.Y, +(0.01f * i), 10, 9500, 500));
            }
            scene.Add("bookCloseRotate", new Animation.Action("Book", "Back", Transformation.Rotate, Axis.Z, -180.0f, 50, 9500, 500));
            scene.Add("bookCloseTranslate", new Animation.Action("Book", "Back", Transformation.Translate, Axis.Y, (+0.06f), 50, 9500, 500));

            // Scene add
            script.Add(scene);

            return script;
        }
    }
}
