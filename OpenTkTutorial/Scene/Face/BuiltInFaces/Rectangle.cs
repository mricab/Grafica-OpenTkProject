using System;
using OpenTK.Mathematics;

namespace OpenTkProject
{
    public class Rectangle : EFace
    {
        public Rectangle(
            float Height, float Width, Vector4 Color,
            Vector3 Centre, Vector3 Rotation, uint? TextureId = null)
            : base(
                  Vertices(Width, Height), Colors(Color), TextureCoordinates(),
                  Indices(), Centre, Rotation, TextureId)
        { }

        private static Vector4[] Vertices(float Width, float Height)
        {
            // Positions
            Vector4[] vertices =
            {
                new Vector4( 0.5f*Width,  0.5f*Height,  0.0f,  1.0f),   // 0) Top right vertex
                new Vector4( 0.5f*Width, -0.5f*Height,  0.0f,  1.0f),   // 1) Bottom-right vertex
                new Vector4(-0.5f*Width, -0.5f*Height,  0.0f,  1.0f),   // 2) Bottom-left vertex
                new Vector4(-0.5f*Width,  0.5f*Height,  0.0f,  1.0f),   // 3) Top left vertex
            };
            return vertices;
        }

        private static Vector4[] Colors(Vector4 Color)
        {
            // Colors (RGBA)
            Vector4[] colors = new Vector4[]
            {
                Color, Color, Color, Color,
            };
            return colors;
        }

        private static Vector2[] TextureCoordinates()
        {
            // 2D Texture coordinates
            Vector2[] textureCoordinates = new Vector2[]
            {
                new Vector2(1.0f, 1.0f),    // 0) Top right vertex
                new Vector2(1.0f, 0.0f),    // 1) Bottom-right vertex
                new Vector2(0.0f, 0.0f),    // 2) Bottom-left vertex
                new Vector2(0.0f, 1.0f),    // 3) Top left vertex
            };
            return textureCoordinates;
        }

        private static uint[] Indices()
        {
            uint[] indices =
            {
                0, 1, 3,    // first triangle
                1, 2, 3,    // second triangle
            };
            return indices;
        }
    }
}
