using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OpenTkProject
{
    public class Face : IFace
    {
        const int VertexSize = 4;
        const int ColorSize = 4;
        const int TextureCordinatesSize = 2;
        const int Stride = 10;

        Vector4[] RawVertices;
        Vector4[] FinalVertices;
        Vector4[] Colors;
        Vector2[] TextureCoordinates;
        private int Count;

        public int VAO;         // Stores Vertex Array Object id in RAM
        private int VBO;        // Stores Vertex Buffer Object id in RAM
        public uint? Texture;   // Texture id

        public Vector3 Centre { get; set; }
        public Vector3 Rotation { get; set; }

        public Face(
            Vector4[] Vertices, Vector4[] Colors, Vector2[] TextureCoordinates,
            Vector3 Centre, Vector3 Rotation, uint? Texture = null)
        {
            if (!(Vertices.Length == Colors.Length
                && Vertices.Length == TextureCoordinates.Length))
            {
                throw new System.Exception("The Vertices, Colors and " +
                    "TextureCoordinates arrays supplied must be " +
                    "of the same size.");
            }

            this.RawVertices = Vertices;
            this.Centre = Centre;
            this.Rotation = Rotation;
            SetFinalVertices();
            this.Colors = Colors;
            this.TextureCoordinates = TextureCoordinates;
            this.Count = Vertices.Length;
            this.Texture = Texture;
        }

        private void SetFinalVertices()
        {
            List<Vector4> FinalVertices = new List<Vector4>();
            for (int i = 0; i < RawVertices.Length; i++)
            {
                FinalVertices.Add(RawVertices[i]
                    * Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(Rotation.X))
                    * Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(Rotation.Y))
                    * Matrix4.CreateRotationZ((float)MathHelper.DegreesToRadians(Rotation.Z))
                    * Matrix4.CreateTranslation(Centre));
            }
            this.FinalVertices = FinalVertices.ToArray();
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
                Count * 10 * sizeof(float),
                Data(),
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

        private float[] Data()
        {
            List<float> data = new List<float>();

            for (int n = 0; n < Count; n++)
            {
                for (int v = 0; v < VertexSize; v++) { data.Add(FinalVertices[n][v]); }
                for (int c = 0; c < ColorSize; c++) { data.Add(Colors[n][c]); }
                for (int t = 0; t < TextureCordinatesSize; t++) { data.Add(TextureCoordinates[n][t]); }
            }

            return data.ToArray();
        }

        virtual public void Initialize()
        {
            SetVAO();
        }

        virtual public void Draw(bool Polygon = false) { }

        virtual public void Delete()
        {
            GL.DeleteBuffer(VBO);
            GL.DeleteVertexArray(VAO);
        }
    }
}
