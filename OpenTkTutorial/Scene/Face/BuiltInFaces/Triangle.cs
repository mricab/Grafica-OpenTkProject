using System;
using OpenTK.Mathematics;

namespace OpenTkProject
{
    public class Triangle : Face
    {
        public Triangle(
            float Size, float[] Color,
            float XCentre, float YCentre, float ZCentre,
            float XDegRotation, float YDegRotation, float ZDegRotation,
            uint? TextureId = null)
            : base(
                  CreateVertices(Size),
                  CreateColors(Color),
                  CreateTextureCoordinates(),
                  XCentre, YCentre, ZCentre,
                  XDegRotation, YDegRotation, ZDegRotation,
                  TextureId)
        { }

        private static float[][] CreateVertices(float Size)
        {
            float Height = ((float)Math.Sqrt(3) / 2) * Size;
            float[][] vertices = new float[3][]
            {
                // Positions
                new float[] {  0.0f,      -0.5f+Height,  0.0f,  1.0f },  // 0) Top vertex
                new float[] { -0.5f*Size, -0.5f*Size,    0.0f,  1.0f },  // 1) Bottom-right vertex
                new float[] {  0.5f*Size, -0.5f*Size,    0.0f,  1.0f },  // 2) Bottom-left vertex
            };
            return vertices;
        }

        private static float[][] CreateColors(float[] Color)
        {
            // Colors (RGBA)
            float[][] colors = new float[3][]
            {
                Color, Color, Color,

            };
            return colors;
        }

        private static float[][] CreateTextureCoordinates()
        {
            // 2D Texture coordinates
            float[][] textureCoordinates = new float[3][]
            {
                new float[] { 0.5f, 1.0f },    // 0) Top vertex
                new float[] { 0.0f, 0.0f },    // 1) Bottom-right vertex
                new float[] { 1.0f, 0.0f },    // 2) Bottom-left vertex
            };
            return textureCoordinates;
        }
    }
}
