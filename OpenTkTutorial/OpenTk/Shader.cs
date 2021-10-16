using System;
using System.IO;
using System.Text;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace OpenTkProject
{
    public class Shader
    {
        // Properties
        public int ShaderProgramObject; // SPO Id

        // Methods
        public Shader(string vertexPath, string fragmentPath)
        {
            // Creating shader's handles.
            int VertexShader;   // Shader object's id.
            int FragmentShader; // Shader object's id.

            // Loading the source codes.
            string VertexShaderSource = ReadShaderSourceCode(vertexPath);
            string FragmentShaderSource = ReadShaderSourceCode(fragmentPath);

            // Shaders' creation and biding.
            VertexShader = CreateAndBindShader(VertexShaderSource, ShaderType.VertexShader);
            FragmentShader = CreateAndBindShader(FragmentShaderSource, ShaderType.FragmentShader);

            // Compilation and error checking
            CompileShader(VertexShader);
            CompileShader(FragmentShader);

            // Linking the compiled programs to the GPU
            ShaderProgramObject = GL.CreateProgram();
            GL.AttachShader(ShaderProgramObject, VertexShader);
            GL.AttachShader(ShaderProgramObject, FragmentShader);
            LinkProgram(ShaderProgramObject);

            // Files cleanup
            GL.DetachShader(ShaderProgramObject, VertexShader);
            GL.DetachShader(ShaderProgramObject, FragmentShader);
            GL.DeleteShader(FragmentShader);    // Deletes the Shader Object from GPU's memory.
            GL.DeleteShader(VertexShader);      // Deletes the Shader Object from GPU's memory.
        }

        public void Use()
        {
            GL.UseProgram(ShaderProgramObject);
        }

        public void Delete()
        {
            GL.DeleteProgram(ShaderProgramObject);
        }


        // Helpher methods
        private string ReadShaderSourceCode(string shaderPath)
        {
            string Source;
            using (StreamReader reader = new StreamReader(shaderPath, Encoding.UTF8))
            {
                Source = reader.ReadToEnd();
            }
            return Source;
        }

        private int CreateAndBindShader(String shaderSource, ShaderType type)
        {
            int ShaderId = GL.CreateShader(type);       // Shader creation (in GPU's memory)
            GL.ShaderSource(ShaderId, shaderSource);    // Shader biding with source code.
            return ShaderId;
        }

        private void CompileShader(int ShaderId)
        {
            GL.CompileShader(ShaderId); // Compilation
            string infoLogVert = GL.GetShaderInfoLog(ShaderId); // Error checking
            if (infoLogVert != System.String.Empty)
                throw new Exception("Shader compilation failed. " + infoLogVert);
        }

        private void LinkProgram(int ProgramId)
        {
            GL.LinkProgram(ShaderProgramObject);
            string infoLogVert = GL.GetProgramInfoLog(ProgramId); // Error checking
            if (infoLogVert != System.String.Empty)
                throw new Exception("Shader Program linking failed. " + infoLogVert);
        }


        // Uniform setters
        public int GetUniformLocation(string attribName)
        {
            return GL.GetUniformLocation(ShaderProgramObject, attribName);
        }
        public void SetUniformInt(string name, int data)
        {
            GL.UseProgram(ShaderProgramObject);
            GL.Uniform1(GetUniformLocation(name), data);
        }
        public void SetUniformBool(string name, bool data)
        {
            GL.UseProgram(ShaderProgramObject);
            if(data) GL.Uniform1(GetUniformLocation(name), 15.0f);
            else GL.Uniform1(GetUniformLocation(name), 0.0f);
        }
        public void SetUniformFloat(string name, float data)
        {
            GL.UseProgram(ShaderProgramObject);
            GL.Uniform1(GetUniformLocation(name), data);
        }

        public void SetUniformMatrix4(string name, Matrix4 data)
        {
            GL.UseProgram(ShaderProgramObject);
            GL.UniformMatrix4(GetUniformLocation(name), true, ref data);
        }

        public void SetUniformVector3(string name, Vector3 data)
        {
            GL.UseProgram(ShaderProgramObject);
            GL.Uniform3(GetUniformLocation(name), data);
        }
    }
}
