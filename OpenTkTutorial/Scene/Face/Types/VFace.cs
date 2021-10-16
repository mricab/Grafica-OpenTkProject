using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OpenTkProject
{
    // VBO Face
    public class VFace : Face, IFace
    {
        public VFace(
            Vector4[] Vertices, Vector4[] Colors, Vector2[] TextureCoordinates,
            Vector3 Centre, Vector3 Rotation, uint? TextureId = null)
            : base(
                  Vertices, Colors, TextureCoordinates,
                  Centre, Rotation, TextureId)
        { }

        public override void Draw(bool Polygon = false)
        {
            GL.BindVertexArray(VAO);                // VAO binding
            if(Polygon) GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.DrawArrays(                          // Draw
                PrimitiveType.Triangles,
                0,
                36);
            GL.BindVertexArray(0);                  // VAO unbinding
        }
    }
}
