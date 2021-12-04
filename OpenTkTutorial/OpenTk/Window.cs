
using System;
using ImGuiNET;
using Dear_ImGui_OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Collections.Generic;
using OpenTkProject.Model;
using OpenTkProject.Animation;

namespace OpenTkProject
{
    public class Window : GameWindow, IInputListener
    {
        /* Properties */

        Shader Shader;
        Camera Camera;
        private Model.Scene Scene;
        Color4 ClearColor;
        InputListener InputListener;
        ImGuiController ImGuiController;
        AnimationController AnimationController;
        Script Script;


        /* Methods */

        public Window(
            int WindowWidth, int WindowHeight, string Title,
            Model.Scene Scene, Color4 ClearColor) 
            : base(
                  new GameWindowSettings()
                    {
                        IsMultiThreaded = true,
                         RenderFrequency = 60.0,
                    }, 
                  new NativeWindowSettings() {
                        Size = new Vector2i(WindowWidth, WindowHeight),
                        Title = Title,
                    }
                  )
        {
            this.ClearColor = ClearColor;
            this.Scene = Scene;
            this.InputListener = new InputListener(this, 50);
            this.AnimationController = new AnimationController();
        }

        private void CheckInput(KeyboardState KeyboardState)
        {
            TextInputQuit(KeyboardState);
            TextInputTransformation(KeyboardState);
            TextInputPitchAndYaw(KeyboardState);
            Console.WriteLine("(Window Input)\tInput Checked.");
        }

        private void Animate()
        {
            this.Script = Script.Deserialize("script.spt");
            AnimationController.Run(ref Scene, ref Script);
        }

        /* Game Window Methods */

        protected override void OnLoad()
        {
            base.OnLoad();

            // InputListener
            InputListener.RegisterObserver(this);
            InputListener.Start();
            //CursorGrabbed = true;                                         // Makes cursor invisible.

            // OpenGL Initialization
            GL.ClearColor(ClearColor);                                      // Defines the color of the window
            GL.Enable(EnableCap.DepthTest);                                 // DepthTest(Z-Buffer) Enabling

            // GUI
            ImGuiController = new ImGuiController(
                ClientSize.X, 
                ClientSize.Y);

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
            ImGuiController.Update(this, (float)e.Time);

            GL.Clear(                                                       // Buffers clearing
                ClearBufferMask.ColorBufferBit                              // before a frame is rendered.
                | ClearBufferMask.DepthBufferBit);

            // Shader
            Shader.Use();                                                   // Renderization
            Shader.SetUniformMatrix4("view", Camera.GetViewMatrix());
            Shader.SetUniformMatrix4("projection", Camera.GetProjectionMatrix());

            // Scene
            GL.Enable(EnableCap.DepthTest);                                 // DepthTest(Z-Buffer) Enabling
            Scene.Draw(Shader);                                             // Scene drawing

            // ImGUI
            //ImGui.ShowDemoWindow();
            BuildGUI();
            ImGuiController.Render();                                       // GUI drawing
            Util.CheckGLError("End of frame");

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
            ImGuiController.WindowResized(ClientSize.X, ClientSize.Y);      // ImGui
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

        /* Sample GUI Constructor */

        private bool whole_scene = true;
        private int  sel_object = 0;
        private string sel_objectName;
        private bool whole_object = true;
        private int  sel_face = 0;
        private string sel_faceName;
        private Transformation sel_transf = Transformation.Scale;
        private Axis sel_axis = Axis.X;
        private bool repeate = false;

        private void BuildGUI()
        {
            string[] Objects = Scene.GetObjectsNames();
            if (sel_objectName == null) sel_objectName = Objects[sel_object];
            string[] Faces = Scene.GetFacesNames(sel_objectName);
            if (sel_faceName == null) sel_faceName = Faces[sel_face];
            string fileName = "";

            ImGui.Begin("Opciones");

            // Selector
            ImGui.Text("Selector");
            ImGui.Checkbox("Toda la escena", ref whole_scene);
            if (!whole_scene)
            {
                ImGui.Checkbox("Todo el objeto", ref whole_object);
                if (ImGui.Combo("Objeto", ref sel_object, Objects, Objects.Length))
                {
                    sel_objectName = Objects[sel_object];
                    sel_face = 0; sel_faceName = null;
                }
                if (!whole_object)
                {
                    if (ImGui.Combo("Cara", ref sel_face, Faces, Faces.Length)) sel_faceName = Faces[sel_face];
                }
            }

            // Transformation
            ImGui.Separator();
            ImGui.Text("Transformaciones");
            if(ImGui.RadioButton("Escalar",   sel_transf == Transformation.Scale))     sel_transf = Transformation.Scale; ImGui.SameLine();
            if(ImGui.RadioButton("Rotar",     sel_transf == Transformation.Rotate))    sel_transf = Transformation.Rotate; ImGui.SameLine();
            if(ImGui.RadioButton("Trasladar", sel_transf == Transformation.Translate)) sel_transf = Transformation.Translate;

            // Axis
            if (sel_transf == Transformation.Rotate || sel_transf == Transformation.Translate)
            {
                ImGui.Separator();
                ImGui.Text("Eje");
                if(ImGui.RadioButton("X", sel_axis == Axis.X)) sel_axis = Axis.X; ImGui.SameLine();
                if(ImGui.RadioButton("Y", sel_axis == Axis.Y)) sel_axis = Axis.Y; ImGui.SameLine();
                if(ImGui.RadioButton("Z", sel_axis == Axis.Z)) sel_axis = Axis.Z;
            }

            //Delta
            ImGui.Separator();
            ImGui.Text("Incremento");
            if (sel_transf == Transformation.Scale) ImGui.SliderFloat("Factor", ref scaleDelta, 0f, 0.5f);
            if (sel_transf == Transformation.Rotate) ImGui.SliderAngle("Angulo", ref rotateDelta);
            if (sel_transf == Transformation.Translate) ImGui.SliderFloat("Unidades", ref translateDelta, 0f, 5f);

            // Animation
            ImGui.Separator();
            ImGui.Text("Animación");
            ImGui.InputTextWithHint("Archivo", "script.spt", fileName, 100);
            if (ImGui.Button("Animar")) Animate();
            ImGui.Checkbox("Repetir", ref repeate);
            ImGui.End();

        }

        float scaleDelta = 0.01f;
        float rotateDelta = 0.2618f;   // Radians
        float translateDelta = 0.01f;

        private void TextInputTransformation(KeyboardState KeyboardState)
        {
            float delta = 0;
            switch (sel_transf)
            {
                case Transformation.Scale:      delta = scaleDelta; break;
                case Transformation.Translate:  delta = translateDelta; break;
                case Transformation.Rotate:     delta = (rotateDelta*180)/3.1416f; break;
                default: break;
            }

            if (KeyboardState.IsKeyDown(Keys.KeyPadSubtract) || KeyboardState.IsKeyDown(Keys.D1))   // - ó 1
            {
                if      (whole_scene)   Scene.TransformScene(sel_transf, -delta, (Axis)sel_axis);
                else if (whole_object)  Scene.TransformObject(sel_objectName, sel_transf, -delta, (Axis)sel_axis);
                else                    Scene.TransformFace(sel_objectName, sel_faceName, sel_transf, -delta, (Axis)sel_axis);
            }
            if (KeyboardState.IsKeyDown(Keys.KeyPadAdd) || KeyboardState.IsKeyDown(Keys.D2))        // + ó 2
            {
                if      (whole_scene)   Scene.TransformScene(sel_transf, +delta, (Axis)sel_axis);
                else if (whole_object)  Scene.TransformObject(sel_objectName, sel_transf, +delta, (Axis)sel_axis);
                else                    Scene.TransformFace(sel_objectName, sel_faceName, sel_transf, +delta, (Axis)sel_axis);
            }
        }

        private void TextInputQuit(KeyboardState KeyboardState)
        {
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                InputListener.Stop();
                Close();                                                    // Closes Window on ESC
            }
        }

        private void TextInputPitchAndYaw(KeyboardState keyboardState)
        {
            if(keyboardState.IsKeyDown(Keys.X))
            {
                Camera.EnablePitchAndYaw = !Camera.EnablePitchAndYaw; 
            }
        }

        /* IInputListener Methods */

        void IInputListener.OnInputReceived(InputReceivedEvent e)
        {
            Console.WriteLine("(Window)\tEvent received.");
            this.CheckInput(e.keyboard);
            Camera.SetPosition(e.keyboard, e.time);
        }
    }
}
