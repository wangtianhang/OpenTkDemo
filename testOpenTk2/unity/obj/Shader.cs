using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL4;

class Shader
{
    public static int _CompilerShader(string vertexShaderSrc, string pixelShaderSrc)
    {
        int vertexShader = 0;
        int pixelShader = 0;
        int shaderProgram = 0;

        vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, vertexShaderSrc);
        GL.CompileShader(vertexShader);

        int length = 0;
        GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out length);
        string errorInfo = GL.GetShaderInfoLog(vertexShader);
        Console.WriteLine("vertexShader " + errorInfo + " " + length);

        pixelShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(pixelShader, pixelShaderSrc);
        GL.CompileShader(pixelShader);

        GL.GetShader(pixelShader, ShaderParameter.CompileStatus, out length);
        errorInfo = GL.GetShaderInfoLog(pixelShader);
        Console.WriteLine("pixelShader " + errorInfo + " " + length);

        shaderProgram = GL.CreateProgram();
        GL.AttachShader(shaderProgram, vertexShader);
        GL.AttachShader(shaderProgram, pixelShader);
        GL.LinkProgram(shaderProgram);

        GL.GetProgram(shaderProgram, GetProgramParameterName.LinkStatus, out length);
        errorInfo = GL.GetProgramInfoLog(shaderProgram);
        Console.WriteLine("shaderProgram " + errorInfo + " " + length);

        GL.DeleteShader(vertexShader);
        GL.DeleteShader(pixelShader);

        return shaderProgram;
    }
}

