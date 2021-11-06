using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OpenTkProject
{
    public class Scene
    {
        /* Properties */

        Object Root;

        /* Methods */
    
        public Scene()
        {
            this.Root = new RootObject();
        }

        public void Add(string Name, Object Object)
        {
            this.Root.AddObject(Name, Object);
        }

        public void Traslate(float X, float Y, float Z)
        {
            Root.Traslate(X, Y, Z);
        }

        public void Scale(float Factor)
        {
            Root.Scale(Factor);
        }

        public void Rotate(float XDeg, float YDeg, float ZDeg)
        {
            Root.Rotate(XDeg, YDeg, ZDeg);
        }

        public void Initialize()
        {
            Root.Initialize();
        }

        public void Draw(Shader Shader)
        {
            Root.Draw(Shader, Matrix4.Identity);
        }

        public void Delete()
        {
            Root.Delete();
        }
    }
}
