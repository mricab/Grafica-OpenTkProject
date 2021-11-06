using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OpenTkProject
{
    public class Object
    {
        [JsonInclude] public Dictionary<String, Object> Objects;
        [JsonInclude] public uint objects;
        [JsonInclude] public List<Face> Faces;
        [JsonInclude] public uint faces;
        [JsonInclude] public Dictionary<uint, Texture> Textures;
        [JsonInclude] public uint textures;
        [JsonInclude] public float[] Centre;
        [JsonInclude] public float ScaleFactor;
        [JsonInclude] public float[] Translation;
        [JsonInclude] public float[] Rotation;          // X, Y, Z Rotation Angles
        [JsonIgnore]  private Vector3 _Centre;

        public Object(float[] Centre)
        {
            this.objects = 0;
            this.Objects = new Dictionary<string, Object>();
            this.faces = 0;
            this.Faces = new List<Face>();
            this.textures = 0;
            this.Textures = new Dictionary<uint, Texture>();
            this.Centre = Centre;
            this.ScaleFactor = 1.0f;
            this.Translation = new float[3] { 0f, 0f, 0f };
            this.Rotation = new float[3] { 0f, 0f, 0f };
        }

        [JsonConstructor]
        public Object(
            float[] Centre, Dictionary<String, Object> Objects,
            uint objects, List<Face> Faces, uint faces,
            Dictionary<uint, Texture> Textures, uint textures,
            float ScaleFactor, float[] Translation, float[] Rotation) 
        {
            this.objects = objects;
            this.Objects = Objects;
            this.faces = faces;
            this.Faces = Faces;
            this.textures = textures;
            this.Textures = Textures;
            this.Centre = Centre;
            this.Centre = Centre;
            this.ScaleFactor = ScaleFactor;
            this.Translation = Translation;
            this.Rotation = Rotation;
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

        public void Traslate(float X, float Y, float Z)
        {
            Translation[0] = X;
            Translation[1] = Y;
            Translation[2] = Z;
        }

        public void Scale(float Factor)
        {
            if (Factor <= 0) throw new Exception("Scale factor can be zero or less than zero.");
            this.ScaleFactor = Factor;
        }

        public void Rotate(float XDeg, float YDeg, float ZDeg)
        {
            Rotation[0] = XDeg;
            Rotation[1] = YDeg;
            Rotation[2] = ZDeg;
        }

        private Matrix4 TransformModel()
        {
            return Matrix4.Identity
                * Matrix4.CreateTranslation(new Vector3(Translation[0], Translation[1], Translation[2]))
                * Matrix4.CreateScale(ScaleFactor)
                * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(Rotation[0]))
                * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(Rotation[1]))
                * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(Rotation[2]));
        }

        public void Initialize()
        {
            _Centre = new Vector3(Centre[0], Centre[1], Centre[2]);

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
                Matrix4.CreateTranslation(_Centre)
                * ParentModel
                * TransformModel();

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

        /* Serialization & Deserialization */

        public void Serialize(string filename)
        {
            string jsonWriteString = JsonSerializer.Serialize<Object>(this);
            File.WriteAllText(filename, jsonWriteString);
        }

        static public Object Deserialize(string filename)
        {
            string jsonReadString = File.ReadAllText(filename);
            return JsonSerializer.Deserialize<Object>(jsonReadString);
        }

    }
}
