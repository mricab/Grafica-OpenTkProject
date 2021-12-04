using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using OpenTkProject.Model;

namespace OpenTkProject.Animation
{
    public class AnimationController
    {
        List<ThreadAction> Threads = new List<ThreadAction>();
        List<bool> IsRunning = new List<bool>();
        Animation.Scene animationScene;
        Model.Scene sceneModel;

        public void Run (ref Model.Scene model, ref Script script)
        {
            Console.WriteLine("Running animation");
            sceneModel = model;

            foreach (Animation.Scene s in script.scenes)
            {
                RunScene(s);
            }
        }

        private void RunScene(Animation.Scene scene)
        {
            Console.WriteLine("Running scene");
            this.animationScene = scene;

            Console.WriteLine("preparing action threads");
            foreach (var a in scene.actions)
            {
                Action action = a.Value;
                ThreadAction threadAction = new ThreadAction(action, ref sceneModel);
                Threads.Add(threadAction);
                IsRunning.Add(false);
            }

            Thread Thread = new Thread(new ThreadStart(MainProcess));
            Thread.Start();
        }

        public void MainProcess()
        {
            DateTime now = DateTime.Now;
            DateTime start = now;
            DateTime end = now.AddMilliseconds(animationScene.Duration);

            Console.WriteLine("running action threads");
            while (now < end)
            {
                int elapsed = (int)(DateTime.Now - start).TotalMilliseconds;
                //Console.WriteLine(elapsed);

                for (int i = 0; i < Threads.Count; i++)
                {
                    if (!IsRunning[i] && Threads[i].Action.start <= elapsed)
                    {
                        IsRunning[i] = true;
                        Thread Thread = new Thread(new ThreadStart(Threads[i].Process));
                        Thread.Start();
                    }
                }

                now = DateTime.Now;
            }

            sceneModel.RemoveTransformations();
        }
    }

    public class ThreadAction
    {
        public Animation.Action Action;
        public Model.Scene scene;

        public ThreadAction(Animation.Action action, ref Model.Scene scene)
        {
            this.Action = action;
            this.scene = scene;
        }

        public void Process()
        {
            DateTime now = DateTime.Now;
            DateTime end = now.AddMilliseconds(Action.duration);
            DateTime step = now;
            float delta = Action.range / Action.steps;
            double stepDuration = Action.duration / Action.steps;
            Console.WriteLine("starting thread process. " + Action.objectName + Action.faceName);

            while (now < end)
            {
                if(now > step)
                {
                    step = step.AddMilliseconds(stepDuration);

                    //Console.WriteLine("transforming");
                    if (Action.type == ActionType.Object) scene.TransformObject(Action.objectName, Action.transformation, delta, Action.axis);
                    if (Action.type == ActionType.Face) scene.TransformFace(Action.objectName, Action.faceName, Action.transformation, delta, Action.axis);
                }

                now = DateTime.Now;
            }
        }
    }
}
