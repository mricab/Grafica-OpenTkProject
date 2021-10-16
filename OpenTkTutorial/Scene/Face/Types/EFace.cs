using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OpenTkProject
{
    // EBO Face
    public class EFace : Face, IFace
    {
        uint[] Indices;
        int EBO;            // Stores Element Buffer Object id's in RAM

        public EFace(
            Vector4[] Vertices, Vector4[] Colors, Vector2[] TextureCoordinates,
            uint[] Indices, Vector3 Centre, Vector3 Rotation, uint? TextureId = null)
            : base(
                  Vertices, Colors, TextureCoordinates,
                  Centre, Rotation, TextureId)
        {
            this.Indices = Indices;
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

        public override void Initialize()
        {
            base.Initialize();            
            SetEBO();            
        }

        override public void Draw(bool Polygon = false)
        {
            GL.BindVertexArray(VAO);                // VAO binding
            if (Polygon) GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.DrawElements(                        // Draw
                PrimitiveType.Triangles,
                6,
                DrawElementsType.UnsignedInt,
                0);
            GL.BindVertexArray(0);                  // VAO unbinding
        }

        override public void Delete()
        {
            base.Delete();            
            GL.DeleteBuffer(EBO);
        }
    }
}
