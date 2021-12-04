using System;
using OpenTK.Mathematics;

namespace OpenTkProject
{
    public class Cube : Face
    {
        public Cube(
            float Size, 
            float[] Centre, float[] Pivot,
            float[] Color, Texture Texture = null)
            : base(Centre, Pivot)
        {
            uint? TextureId = null;
            if (Texture != null) { this.AddTexture(Texture); TextureId = 0; }
                                                   // Centre                   // Rotation          // TextureId
            this.AddPolygon(new Square(Size, Color,  0.0f,  0.0f,  0.5f * Size,  0.0f,   0.0f, 0.0f, TextureId));  // Front Side
            this.AddPolygon(new Square(Size, Color,  0.0f,  0.0f, -0.5f * Size,  0.0f,   0.0f, 0.0f, TextureId));  // Back Side
            this.AddPolygon(new Square(Size, Color, -0.5f * Size,  0.0f,  0.0f,  0.0f,  90.0f, 0.0f, TextureId));  // Left Side
            this.AddPolygon(new Square(Size, Color,  0.5f * Size,  0.0f,  0.0f,  0.0f, -90.0f, 0.0f, TextureId));  // Right Side
            this.AddPolygon(new Square(Size, Color,  0.0f,  0.5f * Size,  0.0f,  90.0f,  0.0f, 0.0f, TextureId));  // Top Side
            this.AddPolygon(new Square(Size, Color,  0.0f, -0.5f * Size,  0.0f, -90.0f,  0.0f, 0.0f, TextureId));  // Botton Side
        }
    }
}
