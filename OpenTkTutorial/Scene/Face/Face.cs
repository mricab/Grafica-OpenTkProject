using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OpenTkProject
{
    public class Face
    {
        [JsonInclude] public Dictionary<uint, Polygon> Polygons;
        [JsonInclude] public uint polygons;
        [JsonInclude] public uint nextPolygonId;
        [JsonInclude] public Dictionary<uint, Texture> Textures;
        [JsonInclude] public uint textures;
        [JsonInclude] public uint nextTextureId;
        [JsonInclude] public float[] Centre;
        [JsonInclude] public float[] Pivot;
        [JsonInclude] public float ScaleFactor;
        [JsonInclude] public float[] Translation;
        [JsonInclude] public float[] Rotation;          // X, Y, Z Rotation Angles
        [JsonIgnore]  private Vector3 _Centre;
        [JsonIgnore]  private Vector3 _Pivot;
        [JsonIgnore]  private Matrix4 _TransformModel;
        [JsonIgnore] public float _ScaleFactor;
        [JsonIgnore] public float[] _Translation;
        [JsonIgnore] public float[] _Rotation;          // X, Y, Z Rotation Angles

        public Face(float[] Centre, float[] Pivot)
        {
            this.polygons = 0;
            this.nextPolygonId = 0;
            this.Polygons = new Dictionary<uint,Polygon>();
            this.textures = 0;
            this.nextTextureId = 0;
            this.Textures = new Dictionary<uint, Texture>();
            this.Centre = Centre;
            this.Pivot = Pivot;
            this.ScaleFactor = 1.0f;
            this.Translation = new float[3] { 0f, 0f, 0f };
            this.Rotation = new float[3] { 0f, 0f, 0f };
            this._ScaleFactor = 1.0f;
            this._Translation = new float[3] { 0f, 0f, 0f };
            this._Rotation = new float[3] { 0f, 0f, 0f };
        }

        [JsonConstructor]
        public Face(
            float[] Centre,
            Dictionary<uint, Polygon> Polygons, uint polygons,
            Dictionary<uint, Texture> Textures, uint textures,
            float ScaleFactor, float[] Translation, float[] Rotation) 
        {
            this.polygons = polygons;
            this.Polygons = Polygons;
            this.textures = textures;
            this.Textures = Textures;
            this.Centre = Centre;
            this.ScaleFactor = ScaleFactor;
            this.Translation = Translation;
            this.Rotation = Rotation;
            this._ScaleFactor = ScaleFactor;
            this._Translation = Translation;
            this._Rotation = Rotation;
        }

        public uint AddPolygon(Polygon Polygon)
        {
            uint id = nextPolygonId;
            Polygons.Add(id, Polygon);
            polygons++;
            nextPolygonId++;
            return id;
        }

        public void RemovePolygon(uint PolygonId)
        {
            Polygons.Remove(PolygonId);
        }

        public uint AddTexture(Texture Texture)
        {
            uint id = nextTextureId;
            Textures.Add(nextTextureId, Texture);
            textures++;
            nextPolygonId++;
            return id;
        }

        public void RemoveTexture(uint TextureId)
        {
            Textures.Remove(TextureId);
        }

        /* Transformation Methods */

        public void Traslate(float X, float Y, float Z)
        {
            Translation[0] = X;
            Translation[1] = Y;
            Translation[2] = Z;
            SetTransformModel();
        }
        public void ModifyTranslation(Axis axis, float delta)
        {
            if (axis == Axis.X) Translation[0] += delta;
            if (axis == Axis.Y) Translation[1] += delta;
            if (axis == Axis.Z) Translation[2] += delta;
            SetTransformModel();
        }

        public void Scale(float Factor)
        {
            if (Factor <= 0) throw new Exception("Scale factor can be zero or less than zero.");
            this.ScaleFactor = Factor;
            SetTransformModel();
        }

        public void ModifyScaleFactor(float delta)
        {
            this.ScaleFactor += delta;
            SetTransformModel();
        }

        public void Rotate(float XDeg, float YDeg, float ZDeg)
        {
            Rotation[0] = XDeg;
            Rotation[1] = YDeg;
            Rotation[2] = ZDeg;
            SetTransformModel();
        }

        public void ModifyRotation(Axis axis, float delta)
        {
            if (axis == Axis.X) Rotation[0] += delta;
            if (axis == Axis.Y) Rotation[1] += delta;
            if (axis == Axis.Z) Rotation[2] += delta;
            SetTransformModel();
        }

        private void SetTransformModel()
        {
            _TransformModel = Matrix4.Identity
                * Matrix4.CreateTranslation(new Vector3(-Pivot[0], -Pivot[1], -Pivot[2]))
                * Matrix4.CreateScale(ScaleFactor)
                * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(Rotation[0]))
                * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(Rotation[1]))
                * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(Rotation[2]))
                * Matrix4.CreateTranslation(new Vector3(Translation[0], Translation[1], Translation[2]))
                * Matrix4.CreateTranslation(new Vector3(Pivot[0], Pivot[1], Pivot[2]));
        }

        public void RemoveTransformations()
        {
            this.ScaleFactor = _ScaleFactor;
            this.Translation = _Translation;
            this.Rotation = _Rotation;
            SetTransformModel();
        }

        /* Drawing Methods */

        public void Initialize()
        {
            _Centre = new Vector3(Centre[0], Centre[1], Centre[2]);
            SetTransformModel();

            foreach (var p in Polygons)
            {
                p.Value.Initialize();
            }
        }

        public void Draw(Shader Shader, Matrix4 ParentModel)
        {
            Matrix4 FinalModel =
                _TransformModel
                * Matrix4.CreateTranslation(_Centre)
                * ParentModel;

            foreach (var p in Polygons)
            {
                Shader.SetUniformMatrix4("model", FinalModel);
                LoadTexture(Shader, p.Value.Texture);
                p.Value.Draw();
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
            foreach (var p in Polygons)
            {
                p.Value.Delete();
            }
        }
    }
}
