using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.ES30;
//using OpenTK;
using UnityEngine;

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
            dataList.Add(pos.x);
            dataList.Add(pos.y);
            dataList.Add(pos.z);
            dataList.Add(normal.x);
            dataList.Add(normal.y);
            dataList.Add(normal.z);
        }
        meshData.m_data = dataList.ToArray();
        meshData.m_index = index.ToArray();
        return meshData;
    }

    public static OpenTK.Matrix4 GLSRT(OpenTK.Vector3 pos, OpenTK.Quaternion qua, OpenTK.Vector3 scale)
    {
        OpenTK.Matrix4 t = OpenTK.Matrix4.CreateTranslation(pos);
        OpenTK.Matrix4 r = OpenTK.Matrix4.CreateFromQuaternion(qua);
        OpenTK.Matrix4 s = OpenTK.Matrix4.CreateScale(scale);
        return s * r * t;
    }

    public static OpenTK.Matrix4 GLLookAt(OpenTK.Vector3 eye, OpenTK.Vector3 center, OpenTK.Vector3 up)
    {
        OpenTK.Vector3 forward = (center - eye).Normalized();
        OpenTK.Vector3 upN = up.Normalized();
        OpenTK.Vector3 right = OpenTK.Vector3.Cross(forward, upN).Normalized();
        OpenTK.Vector3 u = OpenTK.Vector3.Cross(right, forward);
        OpenTK.Matrix4 ret = new OpenTK.Matrix4();

        ret.M11 = right[0]; ret.M12 = u[0]; ret.M13 = -forward[0]; ret.M14 = 0;
        ret.M21 = right[1]; ret.M22 = u[1]; ret.M23 = -forward[1]; ret.M24 = 0;
        ret.M31 = right[2]; ret.M32 = u[2]; ret.M33 = -forward[2]; ret.M34 = 0;
        ret.M41 = 0; ret.M42 = 0; ret.M43 = 0; ret.M44 = 1;

        OpenTK.Matrix4 tmp = OpenTK.Matrix4.CreateTranslation(-eye);

        OpenTK.Matrix4 finalRet = ret * tmp;
        finalRet = tmp * ret;

        return finalRet;
    }

    public static OpenTK.Matrix4 Multiply(OpenTK.Matrix4 lhs, OpenTK.Matrix4 rhs)
    {
        Matrix4x4 t1 = ConvertOpenTkMatrixToUnityMatrix(lhs);
        Matrix4x4 t2 = ConvertOpenTkMatrixToUnityMatrix(rhs);
        return ConvertUnityMatrixToOpenTkMatrix(t1 * t2);
    }

    public static OpenTK.Matrix4 GLVMathMultiply(OpenTK.Matrix4 lhs, OpenTK.Matrix4 rhs)
    {
        OpenTK.Matrix4 result = new OpenTK.Matrix4();
        for (int j = 0; j < 4; j++)
        {
            for (int i = 0; i < 4; i++)
            {
                float sum = 0;

                for (int n = 0; n < 4; n++)
                {
                    sum += lhs[n, i] * rhs[j, n];
                }

                result[j, i] = sum;
            }
        }
        return result;
    }

    public static OpenTK.Vector4 LeftMultiply(OpenTK.Vector3 tmp, OpenTK.Matrix4 mat)
    {
        OpenTK.Vector4 vec = new OpenTK.Vector4(tmp);
        vec.W = 1;

        OpenTK.Vector4 ret = new OpenTK.Vector4();

        ret.X = vec.X * mat.M11 + vec.Y * mat.M21 + vec.Z * mat.M31 + vec.W * mat.M41;
        ret.Y = vec.X * mat.M12 + vec.Y * mat.M22 + vec.Z * mat.M32 + vec.W * mat.M42;
        ret.Z = vec.X * mat.M13 + vec.Y * mat.M23 + vec.Z * mat.M33 + vec.W * mat.M43;
        ret.W = vec.X * mat.M14 + vec.Y * mat.M24 + vec.Z * mat.M34 + vec.W * mat.M44;

        return ret;
    }

    public static OpenTK.Vector4 RightMultiply(OpenTK.Matrix4 mat, OpenTK.Vector3 tmp)
    {
        OpenTK.Vector4 vec = new OpenTK.Vector4(tmp);
        vec.W = 1;

        OpenTK.Vector4 ret = new OpenTK.Vector4();

        ret.X = mat.M11 * vec.X + mat.M12 * vec.Y + mat.M13 * vec.Z + mat.M14 * vec.W;
        ret.Y = mat.M21 * vec.X + mat.M22 * vec.Y + mat.M23 * vec.Z + mat.M24 * vec.W;
        ret.Z = mat.M31 * vec.X + mat.M32 * vec.Y + mat.M33 * vec.Z + mat.M34 * vec.W;

        ret.W = mat.M41 * vec.X + mat.M42 * vec.Y + mat.M43 * vec.Z + mat.M44 * vec.W;
        return ret;
    }

    //     public static OpenTK.Matrix4 TRS(OpenTK.Matrix4 t, OpenTK.Matrix4 r, OpenTK.Matrix4 s)
    //     {
    //         return t * r * s;
    //     }

    //     public static Matrix4x4 UnityWorldToCameraMatrix(Matrix4x4 cameraLocalToWorld)
    //     {
    //         Matrix4x4 worldToLocal = cameraLocalToWorld.inverse;
    //         //OpenTK.Matrix4.Invert(ref cameraLocalToWorld, out worldToLocal);
    //         worldToLocal.m20 *= -1f;
    //         worldToLocal.m21 *= -1f;
    //         worldToLocal.m22 *= -1f;
    //         worldToLocal.m23 *= -1f;
    //         return worldToLocal;
    //      }

    public static OpenTK.Matrix4 GLPerspective(float fovy, float aspect, float n, float f)
    {
        float q = 1.0f / (float)Math.Tan(Mathf.Deg2Rad * 0.5f * fovy);
        float A = q / aspect;
        float B = (n + f) / (n - f);
        float C = (2.0f * n * f) / (n - f);

        OpenTK.Matrix4 result = new OpenTK.Matrix4();

        //result[0] = new OpenTK.Vector4(A, 0.0f, 0.0f, 0.0f);
        //result[1] = new OpenTK.Vector4(0.0f, q, 0.0f, 0.0f);
        //result[2] = new OpenTK.Vector4(0.0f, 0.0f, B, -1.0f);
        //result[3] = new OpenTK.Vector4(0.0f, 0.0f, C, 0.0f);
        result.M11 = A; result.M12 = 0; result.M13 = 0; result.M14 = 0;
        result.M21 = 0; result.M22 = q; result.M23 = 0; result.M24 = 0;
        result.M31 = 0; result.M32 = 0; result.M33 = B; result.M34 = -1;
        result.M41 = 0; result.M42 = 0; result.M43 = C; result.M44 = 0;

        return result;
    }

    public static Matrix4x4 UnityWorldToCameraMatrix(Matrix4x4 cameraLocalToWorld)
    {
        Matrix4x4 worldToLocal = cameraLocalToWorld.inverse;
        worldToLocal.m20 *= -1f;
        worldToLocal.m21 *= -1f;
        worldToLocal.m22 *= -1f;
        worldToLocal.m23 *= -1f;
        return worldToLocal;
    }

    /// <summary>
    /// 等价于UnityWorldToCameraMatrix
    /// </summary>
    /// <param name="eye"></param>
    /// <param name="target"></param>
    /// <param name="up"></param>
    /// <returns></returns>
    public static Matrix4x4 UnityLookAt(Vector3 eye, Vector3 target, Vector3 up)
    {
        Vector3 z = (eye - target).normalized;
        Vector3 x = Vector3.Cross(z, up).normalized;
        Vector3 y = Vector3.Cross(x, z).normalized;

        Matrix4x4 result = new Matrix4x4();

        result[0] = x.x;
        result[4] = x.y;
        result[8] = x.z;
        result[12] = -Vector3.Dot(x, eye);

        result[1] = y.x;
        result[5] = y.y;
        result[9] = y.z;
        result[13] = -Vector3.Dot(y, eye);

        result[2] = z.x;
        result[6] = z.y;
        result[10] = z.z;
        result[14] = -Vector3.Dot(z, eye);

        result[3] = result[7] = result[11] = 0.0f;
        result[15] = 1.0f;
        return result;
    }

    public static float[] ConverToFloat(OpenTK.Matrix4 mat)
    {
        float[] ret = new float[16];
        ret[0] = mat.M11;
        ret[1] = mat.M12;
        ret[2] = mat.M13;
        ret[3] = mat.M14;

        ret[4] = mat.M21;
        ret[5] = mat.M22;
        ret[6] = mat.M23;
        ret[7] = mat.M24;

        ret[8] = mat.M31;
        ret[9] = mat.M32;
        ret[10] = mat.M33;
        ret[11] = mat.M34;

        ret[12] = mat.M41;
        ret[13] = mat.M42;
        ret[14] = mat.M43;
        ret[15] = mat.M44;
        return ret;
    }

    public static float[] ConverToFloat(Matrix4x4 mat)
    {
        float[] ret = new float[16];
        ret[0] = mat.m00;
        ret[1] = mat.m01;
        ret[2] = mat.m02;
        ret[3] = mat.m03;

        ret[4] = mat.m10;
        ret[5] = mat.m11;
        ret[6] = mat.m12;
        ret[7] = mat.m13;

        ret[8] = mat.m20;
        ret[9] = mat.m21;
        ret[10] = mat.m22;
        ret[11] = mat.m23;

        ret[12] = mat.m30;
        ret[13] = mat.m31;
        ret[14] = mat.m32;
        ret[15] = mat.m33;

        return ret;
    }

    //      float[] ConverToFloat3(Matrix4x4 mat)
    //      {
    //          float[] ret = new float[16];
    //          ret[0] = mat.m00;
    //          ret[1] = mat.m10;
    //          ret[2] = mat.m20;
    //          ret[3] = mat.m30;
    // 
    //          ret[4] = mat.m01;
    //          ret[5] = mat.m11;
    //          ret[6] = mat.m21;
    //          ret[7] = mat.m31;
    // 
    //          ret[8] = mat.m02;
    //          ret[9] = mat.m12;
    //          ret[10] = mat.m22;
    //          ret[11] = mat.m32;
    // 
    //          ret[12] = mat.m03;
    //          ret[13] = mat.m13;
    //          ret[14] = mat.m23;
    //          ret[15] = mat.m33;
    // 
    //          return ret;
    //      }

    public static OpenTK.Matrix4 ConvertUnityMatrixToOpenTkMatrix(Matrix4x4 mat)
    {
        OpenTK.Matrix4 ret = new OpenTK.Matrix4();
        ret.M11 = mat.m00;
        ret.M12 = mat.m01;
        ret.M13 = mat.m02;
        ret.M14 = mat.m03;

        ret.M21 = mat.m10;
        ret.M22 = mat.m11;
        ret.M23 = mat.m12;
        ret.M24 = mat.m13;

        ret.M31 = mat.m20;
        ret.M32 = mat.m21;
        ret.M33 = mat.m22;
        ret.M34 = mat.m23;

        ret.M41 = mat.m30;
        ret.M42 = mat.m31;
        ret.M43 = mat.m32;
        ret.M44 = mat.m33;

        return ret;
    }

    public static Matrix4x4 ConvertOpenTkMatrixToUnityMatrix(OpenTK.Matrix4 ret)
    {
        Matrix4x4 mat = new Matrix4x4();

        mat.m00 = ret.M11;
        mat.m01 = ret.M12;
        mat.m02 = ret.M13;
        mat.m03 = ret.M14;

        mat.m10 = ret.M21;
        mat.m11 = ret.M22;
        mat.m12 = ret.M23;
        mat.m13 = ret.M24;

        mat.m20 = ret.M31;
        mat.m21 = ret.M32;
        mat.m22 = ret.M33;
        mat.m23 = ret.M34;

        mat.m30 = ret.M41;
        mat.m31 = ret.M42;
        mat.m32 = ret.M43;
        mat.m33 = ret.M44;

        return mat;
    }
}

