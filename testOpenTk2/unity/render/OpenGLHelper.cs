using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.ES30;
using OpenTK;

class OpenGLHelper
{
    public class MeshData
    {
        public float[] m_data;
        public ushort[] m_index;
    }

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

    public static void ClearGLError()
    {
        ErrorCode error = GL.GetError();
        while (error != ErrorCode.NoError)
        {
            error = GL.GetError();
        }
    }

    public static void CheckGLError()
    {
        ErrorCode error = GL.GetError();
        if (error != ErrorCode.NoError)
        {
            Console.WriteLine("CheckGLError " + error);
        }
    }

    public static MeshData GetCubeMesh()
    {
        List<Vector3> vertices = new List<Vector3>();
        vertices.Add(new Vector3(0.5f, -0.5f, 0.5f));
        vertices.Add(new Vector3(-0.5f, -0.5f, 0.5f));
        vertices.Add(new Vector3(0.5f, 0.5f, 0.5f));
        vertices.Add(new Vector3(-0.5f, 0.5f, 0.5f));
        vertices.Add(new Vector3(0.5f, 0.5f, -0.5f));
        vertices.Add(new Vector3(-0.5f, 0.5f, -0.5f));
        vertices.Add(new Vector3(0.5f, -0.5f, -0.5f));
        vertices.Add(new Vector3(-0.5f, -0.5f, -0.5f));
        vertices.Add(new Vector3(0.5f, 0.5f, 0.5f));
        vertices.Add(new Vector3(-0.5f, 0.5f, 0.5f));
        vertices.Add(new Vector3(0.5f, 0.5f, -0.5f));
        vertices.Add(new Vector3(-0.5f, 0.5f, -0.5f));
        vertices.Add(new Vector3(0.5f, -0.5f, -0.5f));
        vertices.Add(new Vector3(0.5f, -0.5f, 0.5f));
        vertices.Add(new Vector3(-0.5f, -0.5f, 0.5f));
        vertices.Add(new Vector3(-0.5f, -0.5f, -0.5f));
        vertices.Add(new Vector3(-0.5f, -0.5f, 0.5f));
        vertices.Add(new Vector3(-0.5f, 0.5f, 0.5f));
        vertices.Add(new Vector3(-0.5f, 0.5f, -0.5f));
        vertices.Add(new Vector3(-0.5f, -0.5f, -0.5f));
        vertices.Add(new Vector3(0.5f, -0.5f, -0.5f));
        vertices.Add(new Vector3(0.5f, 0.5f, -0.5f));
        vertices.Add(new Vector3(0.5f, 0.5f, 0.5f));
        vertices.Add(new Vector3(0.5f, -0.5f, 0.5f));


        List<ushort> index = new List<ushort>();
        index.Add(0);
        index.Add(2);
        index.Add(3);
        index.Add(0);
        index.Add(3);
        index.Add(1);
        index.Add(8);
        index.Add(4);
        index.Add(5);
        index.Add(8);
        index.Add(5);
        index.Add(9);
        index.Add(10);
        index.Add(6);
        index.Add(7);
        index.Add(10);
        index.Add(7);
        index.Add(11);
        index.Add(12);
        index.Add(13);
        index.Add(14);
        index.Add(12);
        index.Add(14);
        index.Add(15);
        index.Add(16);
        index.Add(17);
        index.Add(18);
        index.Add(16);
        index.Add(18);
        index.Add(19);
        index.Add(20);
        index.Add(21);
        index.Add(22);
        index.Add(20);
        index.Add(22);
        index.Add(23);


        List<Vector3> normals = new List<Vector3>();
        normals.Add(new Vector3(0.0f, 0.0f, 1.0f));
        normals.Add(new Vector3(0.0f, 0.0f, 1.0f));
        normals.Add(new Vector3(0.0f, 0.0f, 1.0f));
        normals.Add(new Vector3(0.0f, 0.0f, 1.0f));
        normals.Add(new Vector3(0.0f, 1.0f, 0.0f));
        normals.Add(new Vector3(0.0f, 1.0f, 0.0f));
        normals.Add(new Vector3(0.0f, 0.0f, -1.0f));
        normals.Add(new Vector3(0.0f, 0.0f, -1.0f));
        normals.Add(new Vector3(0.0f, 1.0f, 0.0f));
        normals.Add(new Vector3(0.0f, 1.0f, 0.0f));
        normals.Add(new Vector3(0.0f, 0.0f, -1.0f));
        normals.Add(new Vector3(0.0f, 0.0f, -1.0f));
        normals.Add(new Vector3(0.0f, -1.0f, 0.0f));
        normals.Add(new Vector3(0.0f, -1.0f, 0.0f));
        normals.Add(new Vector3(0.0f, -1.0f, 0.0f));
        normals.Add(new Vector3(0.0f, -1.0f, 0.0f));
        normals.Add(new Vector3(-1.0f, 0.0f, 0.0f));
        normals.Add(new Vector3(-1.0f, 0.0f, 0.0f));
        normals.Add(new Vector3(-1.0f, 0.0f, 0.0f));
        normals.Add(new Vector3(-1.0f, 0.0f, 0.0f));
        normals.Add(new Vector3(1.0f, 0.0f, 0.0f));
        normals.Add(new Vector3(1.0f, 0.0f, 0.0f));
        normals.Add(new Vector3(1.0f, 0.0f, 0.0f));
        normals.Add(new Vector3(1.0f, 0.0f, 0.0f));

        MeshData meshData = new MeshData();
        List<float> dataList = new List<float>();
        for (int i = 0; i < vertices.Count; ++i)
        {
            Vector3 pos = vertices[i];
            Vector3 normal = normals[i];
            dataList.Add(pos.X);
            dataList.Add(pos.Y);
            dataList.Add(pos.Z);
            dataList.Add(normal.X);
            dataList.Add(normal.Y);
            dataList.Add(normal.Z);
        }
        meshData.m_data = dataList.ToArray();
        meshData.m_index = index.ToArray();
        return meshData;
    }
}

