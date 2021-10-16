using System;
using OpenTK.Mathematics;

namespace OpenTkProject
{
    public class Parallelepiped : Object
    {
        public Parallelepiped(
            float Height, float Width, float Depth,
            Vector3 Centre, Vector4 Color, Texture Texture = null)
            : base(Centre)
        {
            uint? TextureId = null;
            if(Texture!=null) { this.AddTexture(Texture); TextureId = 0; }
            this.AddFace(new Rectangle(Height, Width, Color, new Vector3( 0.0f,        0.0f,         0.5f*Depth), new Vector3(  0.0f,   0.0f,   0.0f), TextureId));  // Front Side
            this.AddFace(new Rectangle(Height, Width, Color, new Vector3( 0.0f,        0.0f,        -0.5f*Depth), new Vector3(  0.0f,   0.0f,   0.0f), TextureId));  // Back Side
            this.AddFace(new Rectangle(Height, Depth, Color, new Vector3(-0.5f*Width,  0.0f,         0.0f      ), new Vector3(  0.0f,  90.0f,   0.0f), TextureId));  // Left Side
            this.AddFace(new Rectangle(Height, Depth, Color, new Vector3( 0.5f*Width,  0.0f,         0.0f      ), new Vector3(  0.0f, -90.0f,   0.0f), TextureId));  // Right Side
            this.AddFace(new Rectangle(Depth,  Width, Color, new Vector3( 0.0f,        0.5f*Height,  0.0f      ), new Vector3( 90.0f,   0.0f,   0.0f), TextureId));  // Top Side
            this.AddFace(new Rectangle(Depth,  Width, Color, new Vector3( 0.0f,       -0.5f*Height,  0.0f      ), new Vector3(-90.0f,   0.0f,   0.0f), TextureId));  // Botton Side
        }
    }
}
