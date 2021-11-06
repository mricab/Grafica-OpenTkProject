using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OpenTkProject
{
    public class Scene
    {
        /* Properties */

        private uint objects;
        private Dictionary<string, Object> Objects;
        public  float ScaleFactor;
        public  float[] Translation;
        public  float[] Rotation;          // X, Y, Z Rotation Angles
        private Matrix4 _TransformModel;

        /* Methods */
    
        public Scene()
        {
            this.objects = 0;
            this.Objects = new Dictionary<string, Object>();
            this.ScaleFactor = 1.0f;
            this.Translation = new float[3] { 0f, 0f, 0f };
            this.Rotation = new float[3] { 0f, 0f, 0f };
        }

        public void Add(string Name, Object Object)
        {
            try
            {
                this.Objects.Add(Name, Object);
                objects++;
            }
            catch (System.ArgumentException ex)
            {
                throw new Exception("There's already an object with the name " + Name + ".");
            }
        }

        public void Remove(string Name)
        {
            this.Objects.Remove(Name);
            objects--;
        }

        /* Transformation Methods */

        public void Traslate(float X, float Y, float Z)
        {
            Translation[0] = X;
            Translation[1] = Y;
            Translation[2] = Z;
            SetTransformModel();
        }

        public void Scale(float Factor)
        {
            if (Factor <= 0) throw new Exception("Scale factor can be zero or less than zero.");
            this.ScaleFactor = Factor;
            SetTransformModel();
        }

        public void Rotate(float XDeg, float YDeg, float ZDeg)
        {
            Rotation[0] = XDeg;
            Rotation[1] = YDeg;
            Rotation[2] = ZDeg;
            SetTransformModel();
        }

        private void SetTransformModel()
        {
            _TransformModel = Matrix4.Identity
                * Matrix4.CreateTranslation(new Vector3(Translation[0], Translation[1], Translation[2]))
                * Matrix4.CreateScale(ScaleFactor)
                * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(Rotation[0]))
                * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(Rotation[1]))
                * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(Rotation[2]));
        }

        /* Drawing Methods */

        public void Initialize()
        {
            SetTransformModel();

            foreach (var o in Objects)
            {
                o.Value.Initialize();
            }
        }

        public void Draw(Shader Shader)
        {
            foreach (var o in Objects)
            {
                o.Value.Draw(Shader, _TransformModel);
            }
        }

        public void Delete()
        {
            foreach (var o in Objects)
            {
                o.Value.Delete();
            }
        }
    }
}
