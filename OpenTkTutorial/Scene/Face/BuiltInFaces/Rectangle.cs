using System;
using OpenTK.Mathematics;

namespace OpenTkProject
{
    public class Rectangle : Face
    {
        public Rectangle(
            float Height, float Width, float[] Color,
            float XCentre, float YCentre, float ZCentre,
            float XDegRotation, float YDegRotation, float ZDegRotation,
            uint? TextureId = null)
            : base(
                  CreateVertices(Width, Height),
                  CreateColors(Color),
                  CreateTextureCoordinates(),
                  CreateIndices(),
                  XCentre, YCentre, ZCentre,
                  XDegRotation, YDegRotation, ZDegRotation,
                  TextureId)
        { }

        private static float[][] CreateVertices(float Width, float Height)
        {
            // Positions
            float[][] vertices = new float[4][]
            {
                new float[] {  0.5f*Width,    0.5f*Height,   0.0f,  1.0f },   // 0) Top right vertex
                new float[] {  0.5f*Width,   -0.5f*Height,   0.0f,  1.0f },   // 1) Bottom-right vertex
                new float[] { -0.5f * Width, -0.5f * Height, 0.0f,  1.0f },   // 2) Bottom-left vertex
                new float[] { -0.5f*Width,    0.5f*Height,   0.0f,  1.0f },   // 3) Top left vertex
            };
            return vertices;
        }

        private static float[][] CreateColors(float[] Color)
        {
            // Colors (RGBA)
            float[][] colors = new float[4][]
            {
                Color, Color, Color, Color,
            };
            return colors;
        }

        private static float[][] CreateTextureCoordinates()
        {
            // 2D Texture coordinates
            float[][] textureCoordinates = new float[4][]
            {
                new float[] { 1.0f, 1.0f },    // 0) Top right vertex
                new float[] { 1.0f, 0.0f },    // 1) Bottom-right vertex
                new float[] { 0.0f, 0.0f },    // 2) Bottom-left vertex
                new float[] { 0.0f, 1.0f },    // 3) Top left vertex
            };
            return textureCoordinates;
        }

        private static uint[] CreateIndices()
        {
            // Polygons Indices
            uint[] indices =
            {
                0, 1, 3,    // first triangle
                1, 2, 3,    // second triangle
            };
            return indices;
        }
    }
}
