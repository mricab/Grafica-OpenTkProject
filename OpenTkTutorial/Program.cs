using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTkProject.Samples;
using OpenTkProject.Model;
using OpenTkProject.Animation;


namespace OpenTkProject
{
    class Program
    {
        static void Main(string[] args)
        {
            // Scene Creation
            Model.Scene Scene = new Model.Scene();
            Scene.Add("Table", AnimationObjects.Table());
            Scene.Add("Ant", AnimationObjects.Ant());
            Scene.Add("Book", AnimationObjects.Book());

            // Script Creation
            Script OriginalScript = AnimationScript.BuildScript();
            OriginalScript.Serialize("script.spt");

            // Window run
            using (
                var window = new Window(
                    1600, 900, "OpenTK Project",
                    Scene, new Color4(0.2f, 0.3f, 0.3f, 1.0f)
                    )
                )
            {
                window.Run();
            }
        }
    }
}