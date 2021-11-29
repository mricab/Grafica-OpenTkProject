using System;
using System.IO;
using System.Drawing;
using OpenTK.Graphics.OpenGL4;

namespace OpenTkProject
{
    public class Texture
    {
        private readonly int TextureObject;     // OpenGL Object's Id
        private readonly byte Unit;             // Unit that stores the texture in GPU Card

        public Texture(string path, byte Unit)
        {
            // Unit number verification
            if (Unit > 15) throw new Exception("Unit number can't be greater than 15.");
            this.Unit = Unit;

            // Handle(Id) initialization
            TextureObject = GL.GenTexture();

            // Texture binding
            GL.ActiveTexture(TextureUnit.Texture0 + Unit);
            GL.BindTexture(TextureTarget.Texture2D, TextureObject);

            // Setting Texture Parameters
            // 1) Texture Wrap (Set how the texture is presented beyond its borders)
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);   // X Axis
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);   // Y Axis
            // 2) Texture Filter (Set how texture's pixels are selected)
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);  // Minifying operation
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);  // Magnifying operation

            // Image loading
            LoadFromFile(path);
        }

        private static void LoadFromFile(string path)
        {
            // Pixelformats
            string[] SupportedExtensions = new string[] { ".jpg", ".png" };            
            PixelInternalFormat[] StorageFormats = new PixelInternalFormat[] { PixelInternalFormat.Rgba, PixelInternalFormat.Rgba };
            PixelFormat[] SourceFormats = new PixelFormat[] { PixelFormat.Bgra, PixelFormat.Bgra };

            // Extension index
            string Extension = Path.GetExtension(path);
            int ExtensionIndex = Array.IndexOf(SupportedExtensions, Extension);

            // Image loading
            using (var image = new Bitmap(path))
            {
                image.RotateFlip(RotateFlipType.RotateNoneFlipY); // Mirrors the loaded image because its origin is at bottom-left corner and must be at top-left corner.
                var data = image.LockBits(new System.Drawing.Rectangle(0, 0, image.Width, image.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D   // Attaches the image to the currently bound texture object.
                (
                    TextureTarget.Texture2D,        // OpenGL's Texture target.
                    0,                              // Mipmap level for the texture. Base = 0.
                    StorageFormats[ExtensionIndex], // Format to store the texture.
                    image.Width,                    // Texture's width.
                    image.Height,                   // Texture's height.
                    0,                              // Legacy value. Must be 0.
                    SourceFormats[ExtensionIndex],  // Format of source image.
                    PixelType.UnsignedByte,         // Datatype of source image.
                    data.Scan0                      // Texture's data.
                );
            }

            // Mipmap generation (To represent textures better on objets on perspective)
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        public void Use(Shader Shader)
        {
            Shader.SetUniformBool("hasTexture", true);
            Shader.SetUniformInt("textureUnit" + Unit, Unit);
            GL.ActiveTexture(TextureUnit.Texture0 + Unit);
            GL.BindTexture(TextureTarget.Texture2D, TextureObject);
        }
    }


}
