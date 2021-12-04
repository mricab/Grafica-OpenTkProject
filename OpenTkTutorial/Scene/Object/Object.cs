using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace OpenTkProject
{
    public class Object
    {
        /* Properties */

        [JsonInclude] public uint faces;
        [JsonInclude] public Dictionary<string, Face> Faces;
        [JsonInclude] public float[] Centre;
        [JsonInclude] public float ScaleFactor;
        [JsonInclude] public float[] Translation;
        [JsonInclude] public float[] Rotation;          // X, Y, Z Rotation Angles
        [JsonIgnore]  private Vector3 _Centre;
        [JsonIgnore]  private Matrix4 _TransformModel;
        [JsonIgnore]  public float _OriginalScaleFactor;
        [JsonIgnore]  public float[] _OriginalTranslation;
        [JsonIgnore]  public float[] _OriginalRotation;          // X, Y, Z Rotation Angles

        /* Methods */

        public Object(float[] Centre)
        {
            this.faces = 0;
            this.Faces = new Dictionary<string, Face>();
            this.Centre = Centre;
            this.ScaleFactor = 1.0f;
            this.Translation = new float[3] { 0f, 0f, 0f };
            this.Rotation = new float[3] { 0f, 0f, 0f };
            this._OriginalScaleFactor = 1.0f;
            this._OriginalTranslation = new float[3] { 0f, 0f, 0f };
            this._OriginalRotation = new float[3] { 0f, 0f, 0f };
        }

        [JsonConstructor]
        public Object(
            float[] Centre,
            uint faces, Dictionary<String, Face> Faces,
            float ScaleFactor, float[] Translation, float[] Rotation)
        {
            this.faces = faces;
            this.Faces = Faces;
            this.Centre = Centre;
            this.ScaleFactor = ScaleFactor;
            this.Translation = Translation;
            this.Rotation = Rotation;
            this._OriginalScaleFactor = (float)this.ScaleFactor;
            this._OriginalTranslation = new float[3] { Translation[0], Translation[1], Translation[2] };
            this._OriginalRotation = new float[3] { Rotation[0], Rotation[1], Rotation[2] };
        }

        public void AddFace(string Name, Face Face)
        {
            try
            {
                Faces.Add(Name, Face);
                faces++;
            }
            catch (System.ArgumentException ex)
            {
                throw new Exception("There's already a face with the name " + Name + ".");
            }
        }

        public void RemoveFace(string Name)
        {
            Faces.Remove(Name);
            faces--;
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
                * Matrix4.CreateScale(ScaleFactor)
                * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(Rotation[0]))
                * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(Rotation[1]))
                * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(Rotation[2]))
                * Matrix4.CreateTranslation(new Vector3(Translation[0], Translation[1], Translation[2]));
        }

        public void RemoveTransformations()
        {
            Console.WriteLine("MO:" + Rotation[2]);
            Console.WriteLine("OR:" + _OriginalRotation[2]);
            this.ScaleFactor = _OriginalScaleFactor;
            this.Translation = _OriginalTranslation;
            this.Rotation = _OriginalRotation; 
            SetTransformModel();

            foreach (var f in Faces)
            {
                f.Value.RemoveTransformations();
            }
        }

        /* Faces transformation methods */
        public void TransformFace(string faceName, Transformation transformation, float delta, Axis? axis = null)
        {
            if (transformation == Transformation.Rotate || transformation == Transformation.Translate)
            {
                if (axis == null) throw new Exception("Axis expected.");
            }

            try
            {
                Face f;
                Faces.TryGetValue(faceName, out f);
                if (transformation == Transformation.Scale)     { f.ModifyScaleFactor(delta); };
                if (transformation == Transformation.Rotate)    { f.ModifyRotation((Axis)axis, delta); };
                if (transformation == Transformation.Translate) { f.ModifyTranslation((Axis)axis, delta); };
                Faces[faceName] = f;
            }
            catch (Exception)
            {
                throw new Exception("Unknown face name.");
            }
        }

        /* Drawing Methods */

        public void Initialize()
        {
            _Centre = new Vector3(Centre[0], Centre[1], Centre[2]);
            SetTransformModel();

            foreach (var f in Faces)
            {
                f.Value.Initialize();
            }
        }

        public void Draw(Shader Shader, Matrix4 ParentModel)
        {
            Matrix4 FinalModel =
                _TransformModel
                * Matrix4.CreateTranslation(_Centre)
                * ParentModel;

            foreach (var f in Faces)
            {
                f.Value.Draw(Shader, FinalModel);
            }
        }

        public void Delete()
        {
            foreach (var f in Faces)
            {
                f.Value.Delete();
            }
        }

        /* Serialization Methods */

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
