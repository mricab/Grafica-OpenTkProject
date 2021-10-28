
using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTkProject
{
    public class Window : GameWindow, IInputListener
    {
        /* Properties */

        Shader Shader;
        Camera Camera;
        Scene Scene;
        Color4 ClearColor;
        InputListener InputListener;


        /* Methods */

        public Window(
            GameWindowSettings gameWindowSettings,
            NativeWindowSettings nativeWindowSettings,
            Scene Scene, Color4 ClearColor) 
            : base(gameWindowSettings, nativeWindowSettings)
        {
            this.ClearColor = ClearColor;
            this.Scene = Scene;
            this.InputListener = new InputListener(this, 50);            
        }

        private void CheckInput()
        {
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                InputListener.Stop();
                Close();                                                    // Closes Window on ESC
            }
            Console.WriteLine("(Window Input)\tInput Checked.");
        }


        /* Game Window Methods */

        protected override void OnLoad()
        {
            base.OnLoad();
            InputListener.RegisterObserver(this);
            InputListener.Start();
            //CursorGrabbed = true;                                         // Makes cursor invisible.

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

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            ;
            base.OnMouseMove(e);
            Camera.SetPerspective(e.X - Size.X / 2, e.Y - Size.Y / 2);
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


        /* IInputListener Methods */

        void IInputListener.OnInputReceived(InputReceivedEvent e)
        {
            Console.WriteLine("(Window)\tEvent received.");
            this.CheckInput();
            Camera.CheckInput(e.keyboard, e.mouse, e.time);
        }
    }
}
