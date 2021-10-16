
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTkProject
{
    public class Window : GameWindow
    {
        /* Properties */

        Shader Shader;
        Camera Camera;
        Scene Scene;
        Color4 ClearColor;

        /* Methods */

        public Window(
            GameWindowSettings gameWindowSettings,
            NativeWindowSettings nativeWindowSettings,
            Scene Scene, Color4 ClearColor) 
            : base(gameWindowSettings, nativeWindowSettings)
        {
            this.ClearColor = ClearColor;
            this.Scene = Scene;
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            // OpenGL Initialization
            GL.ClearColor(ClearColor);                                      // Defines the color of the window
            GL.Enable(EnableCap.DepthTest);                                 // DepthTest(Z-Buffer) Enabling

            // Shader
            Shader = new Shader(
                "./OpenTk/Shaders/shader.vert",
                "./OpenTk/Shaders/shader.frag");
            Shader.Use();

            // Scene
            Scene.Initialize();

            // Camera
            Camera = new Camera(Vector3.UnitZ * 3, Size.X / (float)Size.Y); // Initialization
            //CursorGrabbed = true;                                         // Makes cursor invisible.
        }


        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            
            GL.Clear(                                                       // Buffers clearing
                ClearBufferMask.ColorBufferBit                              // before a frame is rendered.
                | ClearBufferMask.DepthBufferBit); 
            
            Shader.Use();                                                   // Renderization
            Shader.SetUniformMatrix4("view", Camera.GetViewMatrix());
            Shader.SetUniformMatrix4("projection", Camera.GetProjectionMatrix());
            
            Scene.Draw(Shader);                                             // Scene drawing
            
            SwapBuffers();                                                  // Swaps areas (Double-Buffer)
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            if (!IsFocused) return;
            if (KeyboardState.IsKeyDown(Keys.Escape)) { Close(); }          // Closes Window on ESC
            Camera.CheckInput(KeyboardState, MouseState, (float)e.Time);    // Checks for Camera Input
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            Camera.SetZoom(e.OffsetY);                                      // Camera Zoom
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);                              // Maps the NDC to the window.
            Camera.SetAspectRatio(Size.X / (float)Size.Y);                  // Camera AspectRatio            
        }

        protected override void OnUnload()
        {            
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);                     // Unbinding the resources
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            Scene.Delete();                                                 // Deleting the resources
            Shader.Delete();

            base.OnUnload();
        }
    }
}
