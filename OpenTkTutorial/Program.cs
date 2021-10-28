using System;
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

            var nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(WindowWidth, WindowHeight),
                Title = "OpenTkProject - Casa",                
            };

            var gameWindowSettings = new GameWindowSettings()
            {
                IsMultiThreaded = false,
                RenderFrequency = 60.0,
            };

            GLFW.WindowHint(WindowHintBool.OpenGLForwardCompat, true);  //macOS Requirement

            using (
                var window = new Window(
                    gameWindowSettings, nativeWindowSettings,
                    Design.Get(), ClearColor
                    )
                )
            {
                window.Run();
            }
        }
    }
}