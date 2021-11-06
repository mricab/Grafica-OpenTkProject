using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OpenTkProject
{
    public enum PolygonType
    {
        VPolygon,      // Basic Polygon. (Only uses Vertex Buffer Object)
        EPolygon,      // Element Polygon. (Uses Element Buffer Object)
    }

    public class Polygon
    {
        /* Properties */

        const int VertexSize = 4;
        const int ColorSize = 4;
        const int TextureCordinatesSize = 2;
        const int Stride = 10;

        [JsonInclude] public PolygonType Type;

        [JsonInclude] public float[][] Vertices;
        [JsonInclude] public uint[] Indices;
        [JsonInclude] public float[][] Colors;
        [JsonInclude] public float[][] TextureCoordinates;

        [JsonIgnore]  private Vector4[] _Vertices;
        [JsonIgnore]  private Vector4[] _Colors;
        [JsonIgnore]  private Vector2[] _TextureCoordinates;
        [JsonIgnore]  private int _Count;
        [JsonInclude] public uint? Texture;   // Texture id

        [JsonInclude] public float[] Centre;
        [JsonInclude] public float[] Rotation;

        [JsonIgnore]  private Vector3 _Centre;
        [JsonIgnore]  private Vector3 _Rotation;

        [JsonIgnore]  public  int VAO;        // Stores Vertex Array Object id in RAM
        [JsonIgnore]  private int VBO;        // Stores Vertex Buffer Object id in RAM
        [JsonIgnore]  private int EBO;        // Stores Element Buffer Object id's in RAM


        /* Methods */

        private void Constructor(                   // General Constructor
            float[][] Vertices,
            float[][] Colors,
            float[][] TextureCoordinates,
                float XCentre,
                float YCentre,
                float ZCentre,
                float XDegRotation,
                float YDegRotation,
                float ZDegRotation,
                uint? Texture = null)
        {
            // Validation
            if (
                !( Vertices.Length == Colors.Length
                && Vertices.Length == TextureCoordinates.Length )
               )
            {
                throw new System.Exception(
                    "The Vertices, Colors and TextureCoordinates arrays " +
                    "supplied must be of the same size."
                    );
            }

            // Vertices, Colors & TextureCoordinates
            this.Vertices = Vertices;
            this.Colors = Colors;
            this.TextureCoordinates = TextureCoordinates;

            // Centre & Rotation
            this.Centre = new float[3] { XCentre, YCentre, ZCentre };
            this.Rotation = new float[3] { XDegRotation, YDegRotation, ZDegRotation };

            // Texture
            this.Texture = Texture;
        }

        public Polygon(                                //VPolygon Constructor
            float[][] Vertices,
            float[][] Colors,
            float[][] TextureCoordinates,
                float XCentre,
                float YCentre,
                float ZCentre,
                float XDegRotation,
                float YDegRotation,
                float ZDegRotation,
                uint? Texture = null)
        {
            this.Type = PolygonType.VPolygon;
            Constructor(
                Vertices, Colors, TextureCoordinates,
                XCentre, YCentre, ZCentre,
                XDegRotation, YDegRotation, ZDegRotation,
                Texture);
        }


        public Polygon(                                //EPolygon Constructor
            float[][] Vertices,
            float[][] Colors,
            float[][] TextureCoordinates,
               uint[] Indices,
                float XCentre,
                float YCentre,
                float ZCentre,
                float XDegRotation,
                float YDegRotation,
                float ZDegRotation,
                uint? Texture = null)
        {
            this.Type = PolygonType.EPolygon;
            Constructor(
                Vertices, Colors, TextureCoordinates,
                XCentre, YCentre, ZCentre,
                XDegRotation, YDegRotation, ZDegRotation,
                Texture);
            this.Indices = Indices;
        }

        [JsonConstructor]
        public Polygon(                                //Json Constructor
             PolygonType Type,
            float[][] Vertices, 
            float[][] Colors,
            float[][] TextureCoordinates,
               uint[] Indices,
              float[] Centre,
              float[] Rotation,
                uint? Texture)
        {
            this.Type = Type;
            this.Vertices = Vertices;
            this.Indices = Indices;
            this.Colors = Colors;
            this.TextureCoordinates = TextureCoordinates;
            this.Centre = Centre;
            this.Rotation = Rotation;
            this.Texture = Texture;
        }

        private void SetFinalData()
        {
            // Vertices
            List<Vector4> FinalVertices = new List<Vector4>();
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vector4 V = new Vector4(Vertices[i][0], Vertices[i][1], Vertices[i][2], Vertices[i][3]);
                FinalVertices.Add(V
                    * Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(_Rotation.X))
                    * Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(_Rotation.Y))
                    * Matrix4.CreateRotationZ((float)MathHelper.DegreesToRadians(_Rotation.Z))
                    * Matrix4.CreateTranslation(_Centre));
            }
            this._Vertices = FinalVertices.ToArray();

            // Colors
            List<Vector4> FinalColors = new List<Vector4>();
            for (int i = 0; i < Colors.Length; i++)
            {
                Vector4 C = new Vector4(Colors[i][0], Colors[i][1], Colors[i][2], Colors[i][3]);
                FinalColors.Add(C);
            }
            this._Colors = FinalColors.ToArray();

            // Texture Coordinates
            List<Vector2> FinalTextureCoordinates = new List<Vector2>();
            for (int i = 0; i < TextureCoordinates.Length; i++)
            {
                Vector2 T = new Vector2(TextureCoordinates[i][0], TextureCoordinates[i][1]);
                FinalTextureCoordinates.Add(T);
            }
            this._TextureCoordinates = FinalTextureCoordinates.ToArray();

            // Count
            this._Count = _Vertices.Length;
        }

        private float[] DataToArray()
        {
            List<float> data = new List<float>();

            for (int n = 0; n < _Count; n++)
            {
                for (int v = 0; v < VertexSize; v++) { data.Add(_Vertices[n][v]); }
                for (int c = 0; c < ColorSize; c++) { data.Add(_Colors[n][c]); }
                for (int t = 0; t < TextureCordinatesSize; t++) { data.Add(_TextureCoordinates[n][t]); }
            }

            return data.ToArray();
        }

        private void SetVAO()
        {
            // VAO creation and binding
            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);

            // VBO creation, binding and data loading.
            VBO = GL.GenBuffer();                       // Creates VBO in GPU's memory

            GL.BindBuffer(                              // Binds the VBO with its id. 
                BufferTarget.ArrayBuffer,               // Sets VBO's type to ArrayBuffer.
                VBO
                );
            GL.BufferData(                              // Passes data from RAM to VBO
                BufferTarget.ArrayBuffer,
                _Count * 10 * sizeof(float),
                DataToArray(),
                BufferUsageHint.StaticDraw              // Indicates GPU to treat it as static.
                );

            // Setting Vertex Attributes Pointers
            GL.VertexAttribPointer(                     // Positions Attribute
                0,
                VertexSize,
                VertexAttribPointerType.Float,
                false,
                Stride * sizeof(float),
                0);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(                     // Color Attribute
                1,
                ColorSize,
                VertexAttribPointerType.Float,
                false,
                Stride * sizeof(float),
                (VertexSize) * sizeof(float));
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(                     // Texture coordinates Attribute
                2,
                TextureCordinatesSize,
                VertexAttribPointerType.Float,
                false,
                Stride * sizeof(float),
                (VertexSize + ColorSize) * sizeof(float));
            GL.EnableVertexAttribArray(2);

            // Unbind VAO
            GL.BindVertexArray(0);
        }

        private void SetEBO()
        {
            // VAO Binding
            GL.BindVertexArray(VAO);

            // EBO creation and binding
            EBO = GL.GenBuffer();
            GL.BindBuffer(
                BufferTarget.ElementArrayBuffer,
                EBO
                );
            GL.BufferData(
                BufferTarget.ElementArrayBuffer,
                Indices.Length * sizeof(uint),
                Indices,
                BufferUsageHint.StaticDraw
                );

            // VAO Unbinding
            GL.BindVertexArray(0);
        }

        public void Initialize()
        {
            // Centre & Rotation
            this._Centre = new Vector3(Centre[0], Centre[1], Centre[2]);
            this._Rotation = new Vector3(Rotation[0], Rotation[1], Rotation[2]);
            // Vertices/Colors/TextureCoordinates & VAO
            SetFinalData();
            SetVAO();
            // EBO
            if (Type == PolygonType.EPolygon) SetEBO();
        }

        public void Draw(bool Polygon = false)
        {
            // VAO binding
            GL.BindVertexArray(VAO);                
            if (Polygon) GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            // Drawing
            if (Type == PolygonType.VPolygon)                        // VPolygon
                GL.DrawArrays(
                    PrimitiveType.Triangles, 0, 36);
            if (Type == PolygonType.EPolygon)                        // EPolygon
                GL.DrawElements(
                    PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
            // VAO unbinding
            GL.BindVertexArray(0);                  
        }

        public void Delete()
        {
            GL.DeleteBuffer(VBO);
            GL.DeleteVertexArray(VAO);
            if (Type == PolygonType.EPolygon) GL.DeleteBuffer(EBO);
        }
    }
}
