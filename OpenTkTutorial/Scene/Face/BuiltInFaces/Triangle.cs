using System;
using OpenTK.Mathematics;

namespace OpenTkProject
{
    public class Triangle : VFace
    {
        public Triangle(
            float Size, Vector4 Color,
            Vector3 Centre, Vector3 Rotation, uint? TextureId = null)
            : base(
                  Vertices(Size), Colors(Color), TextureCoordinates(),
                  Centre, Rotation, TextureId)
        { }

        private static Vector4[] Vertices(float Size)
        {
            float Height = ((float)Math.Sqrt(3) / 2) * Size;
            Vector4[] vertices =
            {
                // Positions
                new Vector4( 0.0f,      -0.5f+Height,  0.0f,  1.0f),  // 0) Top vertex
                new Vector4(-0.5f*Size, -0.5f*Size,    0.0f,  1.0f),  // 1) Bottom-right vertex
                new Vector4( 0.5f*Size, -0.5f*Size,    0.0f,  1.0f),  // 2) Bottom-left vertex
            };
            return vertices;
        }

        private static Vector4[] Colors(Vector4 Color)
        {
            // Colors (RGBA)
            Vector4[] colors = new Vector4[]
            {
                Color, Color, Color,
            };
            return colors;
        }

        private static Vector2[] TextureCoordinates()
        {
            // 2D Texture coordinates
            Vector2[] textureCoordinates = new Vector2[]
            {
                new Vector2(0.5f, 1.0f),    // 0) Top vertex
                new Vector2(0.0f, 0.0f),    // 1) Bottom-right vertex
                new Vector2(1.0f, 0.0f),    // 2) Bottom-left vertex
            };
            return textureCoordinates;
        }
    }
}
