using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;


namespace OpenTkProject
{
    class Program
    {
        const int WindowHeight = 600;
        const int WindowWidth = 800;
        static Color4 ClearColor = new Color4(0.2f, 0.3f, 0.3f, 1.0f);

        static void Main(string[] args)
        {
            // Object Design
            Object House = Design.BuildHouse();

            // Design Serialization
            string filename = "house.json";
            House.Serialize(filename);                           // Serialization

            // House 1 - Deserialization
            Object ReadHouse1 = Object.Deserialize(filename);    // Deserialization

            // House 1 - Transformations
            ReadHouse1.Rotate(0f, -45f, 0f);
            ReadHouse1.Traslate(0f, 3f, 0f);
            ReadHouse1.Scale(0.5f);

            //// House 2 - Deserialization
            Object ReadHouse2 = Object.Deserialize(filename);    // Deserialization

            //// House 2 - Transformations
            ReadHouse2.Rotate(0f, 45f, 0f);
            ReadHouse2.Traslate(-2f, -1f, -1f);

            // Scene Creation
            Scene Scene = new Scene();
            Scene.Add("House1", ReadHouse1);
            Scene.Add("House2", ReadHouse2);
            Scene.Rotate(0f, 0f, 0f);

            // Window settings
            var nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(WindowWidth, WindowHeight),
                Title = "OpenTkProject",                
            };

            var gameWindowSettings = new GameWindowSettings()
            {
                IsMultiThreaded = true,
                RenderFrequency = 60.0,
            };

            GLFW.WindowHint(WindowHintBool.OpenGLForwardCompat, true);  //macOS Requirement

            // Window run
            using (
                var window = new Window(
                    gameWindowSettings, nativeWindowSettings,
                    Scene, ClearColor
                    )
                )
            {
                window.Run();
            }
        }
    }
}