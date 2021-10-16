using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OpenTkProject
{
    public class Object
    {
        Dictionary<String, Object> Objects;
        //List<Object> Objects;
        uint objects;
        List<Face> Faces;
        uint faces;
        Dictionary<uint, Texture> Textures;
        uint textures;
        Vector3 Centre;
        Matrix4 Model;

        public Object(Vector3 Centre)
        {
            objects = 0; Objects = new Dictionary<string, Object>();
            faces = 0; Faces = new List<Face>();
            textures = 0; Textures = new Dictionary<uint, Texture>();
            Model = Matrix4.Identity;
            this.Centre = Centre;
        }

        public void AddFace(Face Face)
        {
            Faces.Add(Face);
            faces++;
        }

        public void AddTexture(Texture Texture)
        {
            Textures.Add(textures, Texture);
            textures++;
        }

        public void AddObject(String Name, Object Object)
        {
            Objects.Add(Name, Object);
            objects++;
        }

        public void SetModel(Matrix4 Model)
        {
            this.Model = this.Model * Model;
        }

        public void Initialize()
        {
            foreach (var f in Faces)
            {
                f.Initialize();
            }

            foreach (var o in Objects)
            {
                o.Value.Initialize();
            }
        }

        public void Draw(Shader Shader, Matrix4 ParentModel)
        {
            Matrix4 FinalModel =
                Matrix4.CreateTranslation(Centre)
                * ParentModel
                * Model;

            foreach (var f in Faces)
            {
                Shader.SetUniformMatrix4("model", FinalModel);
                LoadTexture(Shader, f.Texture);
                f.Draw();
            }

            foreach (var o in Objects)
            {
                o.Value.Draw(Shader, FinalModel);
            }
        }

        private void LoadTexture(Shader Shader, uint? TextureId)
        {
            if (TextureId != null) Textures[(uint)TextureId].Use(Shader);
            else
            {
                Shader.SetUniformBool("hasTexture", false);
                GL.BindTexture(TextureTarget.Texture2D, 0);
            }
        }

        public void Delete()
        {
            foreach (var f in Faces) f.Delete();
        }
    }
}
