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
    
        public Scene(Object Root)
        {
            this.Root = Root;
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
