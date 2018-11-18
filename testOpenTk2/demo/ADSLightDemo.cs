using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using OpenTK;
using OpenTK.Graphics.ES30;
using System.Runtime.InteropServices;
using UnityEngine;

class ADSLightDemo : IDemo
{
    string vShaderStr = @"
// ADS Point lighting Shader
// Vertex Shader
// Richard S. Wright Jr.
// OpenGL SuperBible
#version 300 es
precision highp float;

// Incoming per vertex... position and normal
layout(location = 0) in vec4 vVertex;
layout(location = 1) in vec3 vNormal;

uniform mat4   mvpMatrix;
uniform mat4   mvMatrix;
uniform vec3   vLightPosition;

out vec3 vVaryingNormal;
out vec3 vVaryingLightDir;

void main(void) 
    { 
// Get surface normal in eye coordinates
    //mat3 normalMatrix = mvMatrix;
    vec4 normalWrap = vec4(vNormal.x, vNormal.y, vNormal.z, 0);
    vVaryingNormal = (mvMatrix * normalWrap).xyz;

    // Get vertex position in eye coordinates
    vec4 vPosition4 = mvMatrix * vVertex;
    vec3 vPosition3 = vPosition4.xyz / vPosition4.w;

    // Get vector to light source
    vVaryingLightDir = normalize(vLightPosition - vPosition3);

    gl_Position = mvpMatrix * vVertex;

    }
";

    string fShaderStr = @"
// ADS Point lighting Shader
// Fragment Shader
// Richard S. Wright Jr.
// OpenGL SuperBible
#version 300 es
precision highp float;


uniform vec4    ambientColor;
uniform vec4    diffuseColor; 
uniform vec4 specularColor;

in vec3 vVaryingNormal;
in vec3 vVaryingLightDir;

out vec4 vFragColor;

void main(void)
    { 
        // Dot product gives us diffuse intensity
    float diff = max(0.0, dot(normalize(vVaryingNormal), normalize(vVaryingLightDir)));

    // Multiply intensity by diffuse color, force alpha to 1.0
    vFragColor = diff * diffuseColor;

    // Add in ambient light
    vFragColor += ambientColor;


    // Specular Light
    vec3 vReflection = normalize(reflect(-normalize(vVaryingLightDir), normalize(vVaryingNormal)));
    float spec = max(0.0, dot(normalize(vVaryingNormal), vReflection));
    if(diff != 0.0f) {
        float fSpec = pow(spec, 128.0);
        vFragColor.rgb += vec3(fSpec, fSpec, fSpec);
        }
    }
";
//     public class MeshData
//     {
//         public float[] m_data;
//         public ushort[] m_index;
//     }

    public int m_program;

    int m_locAmbient;			// The location of the ambient color
    int m_locDiffuse;			// The location of the diffuse color
    int m_locSpecular;		// The location of the specular color
    int m_locLight;			// The location of the Light in eye coordinates
    int m_locMVP;				// The location of the ModelViewProjection matrix uniform
    int m_locMV;				// The location of the ModelView matrix uniform
    //int m_locNM;				// The location of the Normal matrix uniform

    public OpenGLHelper.MeshData m_meshData;
    IntPtr m_ptr;

    int m_width;
    int m_height;

    public double m_accTime = 0;

    public void Init(MainWindow mainWindow)
    {
        m_program = OpenGLHelper._CompilerShader(vShaderStr, fShaderStr);

        m_meshData = OpenGLHelper.GetCubeMesh();

        m_ptr = Marshal.AllocHGlobal(sizeof(float) * m_meshData.m_data.Length);
        Marshal.Copy(m_meshData.m_data, 0, m_ptr, m_meshData.m_data.Length);

        GL.Enable(EnableCap.DepthTest);
        GL.DepthFunc(All.Lequal);
        GL.Enable(EnableCap.CullFace);

        OpenGLHelper.ClearGLError();
        m_locAmbient = GL.GetUniformLocation(m_program, "ambientColor");
        Debug.Log("m_locAmbient" + m_locAmbient);
        OpenGLHelper.CheckGLError();
        m_locDiffuse = GL.GetUniformLocation(m_program, "diffuseColor");
        Debug.Log("m_locDiffuse" + m_locDiffuse);
        m_locSpecular = GL.GetUniformLocation(m_program, "specularColor");
        Debug.Log("m_locSpecular" + m_locSpecular);
        m_locLight = GL.GetUniformLocation(m_program, "vLightPosition");
        Debug.Log("m_locLight" + m_locLight);
        m_locMVP = GL.GetUniformLocation(m_program, "mvpMatrix");
        Debug.Log("m_locMVP" + m_locMVP);
        m_locMV = GL.GetUniformLocation(m_program, "mvMatrix");
        Debug.Log("m_locMV" + m_locMV);
        //m_locNM = GL.GetUniformLocation(m_program, "normalMatrix");

        m_width = mainWindow.Width;
        m_height = mainWindow.Height;
    }

//     MeshData GetCubeMesh()
//     {
//         List<Vector3> vertices = new List<Vector3>();
//         vertices.Add(new Vector3(0.5f, -0.5f, 0.5f));
//         vertices.Add(new Vector3(-0.5f, -0.5f, 0.5f));
//         vertices.Add(new Vector3(0.5f, 0.5f, 0.5f));
//         vertices.Add(new Vector3(-0.5f, 0.5f, 0.5f));
//         vertices.Add(new Vector3(0.5f, 0.5f, -0.5f));
//         vertices.Add(new Vector3(-0.5f, 0.5f, -0.5f));
//         vertices.Add(new Vector3(0.5f, -0.5f, -0.5f));
//         vertices.Add(new Vector3(-0.5f, -0.5f, -0.5f));
//         vertices.Add(new Vector3(0.5f, 0.5f, 0.5f));
//         vertices.Add(new Vector3(-0.5f, 0.5f, 0.5f));
//         vertices.Add(new Vector3(0.5f, 0.5f, -0.5f));
//         vertices.Add(new Vector3(-0.5f, 0.5f, -0.5f));
//         vertices.Add(new Vector3(0.5f, -0.5f, -0.5f));
//         vertices.Add(new Vector3(0.5f, -0.5f, 0.5f));
//         vertices.Add(new Vector3(-0.5f, -0.5f, 0.5f));
//         vertices.Add(new Vector3(-0.5f, -0.5f, -0.5f));
//         vertices.Add(new Vector3(-0.5f, -0.5f, 0.5f));
//         vertices.Add(new Vector3(-0.5f, 0.5f, 0.5f));
//         vertices.Add(new Vector3(-0.5f, 0.5f, -0.5f));
//         vertices.Add(new Vector3(-0.5f, -0.5f, -0.5f));
//         vertices.Add(new Vector3(0.5f, -0.5f, -0.5f));
//         vertices.Add(new Vector3(0.5f, 0.5f, -0.5f));
//         vertices.Add(new Vector3(0.5f, 0.5f, 0.5f));
//         vertices.Add(new Vector3(0.5f, -0.5f, 0.5f));
// 
// 
//         List<ushort> index = new List<ushort>();
//         index.Add(0);
//         index.Add(2);
//         index.Add(3);
//         index.Add(0);
//         index.Add(3);
//         index.Add(1);
//         index.Add(8);
//         index.Add(4);
//         index.Add(5);
//         index.Add(8);
//         index.Add(5);
//         index.Add(9);
//         index.Add(10);
//         index.Add(6);
//         index.Add(7);
//         index.Add(10);
//         index.Add(7);
//         index.Add(11);
//         index.Add(12);
//         index.Add(13);
//         index.Add(14);
//         index.Add(12);
//         index.Add(14);
//         index.Add(15);
//         index.Add(16);
//         index.Add(17);
//         index.Add(18);
//         index.Add(16);
//         index.Add(18);
//         index.Add(19);
//         index.Add(20);
//         index.Add(21);
//         index.Add(22);
//         index.Add(20);
//         index.Add(22);
//         index.Add(23);
// 
// 
//         List<Vector3> normals = new List<Vector3>();
//         normals.Add(new Vector3(0.0f, 0.0f, 1.0f));
//         normals.Add(new Vector3(0.0f, 0.0f, 1.0f));
//         normals.Add(new Vector3(0.0f, 0.0f, 1.0f));
//         normals.Add(new Vector3(0.0f, 0.0f, 1.0f));
//         normals.Add(new Vector3(0.0f, 1.0f, 0.0f));
//         normals.Add(new Vector3(0.0f, 1.0f, 0.0f));
//         normals.Add(new Vector3(0.0f, 0.0f, -1.0f));
//         normals.Add(new Vector3(0.0f, 0.0f, -1.0f));
//         normals.Add(new Vector3(0.0f, 1.0f, 0.0f));
//         normals.Add(new Vector3(0.0f, 1.0f, 0.0f));
//         normals.Add(new Vector3(0.0f, 0.0f, -1.0f));
//         normals.Add(new Vector3(0.0f, 0.0f, -1.0f));
//         normals.Add(new Vector3(0.0f, -1.0f, 0.0f));
//         normals.Add(new Vector3(0.0f, -1.0f, 0.0f));
//         normals.Add(new Vector3(0.0f, -1.0f, 0.0f));
//         normals.Add(new Vector3(0.0f, -1.0f, 0.0f));
//         normals.Add(new Vector3(-1.0f, 0.0f, 0.0f));
//         normals.Add(new Vector3(-1.0f, 0.0f, 0.0f));
//         normals.Add(new Vector3(-1.0f, 0.0f, 0.0f));
//         normals.Add(new Vector3(-1.0f, 0.0f, 0.0f));
//         normals.Add(new Vector3(1.0f, 0.0f, 0.0f));
//         normals.Add(new Vector3(1.0f, 0.0f, 0.0f));
//         normals.Add(new Vector3(1.0f, 0.0f, 0.0f));
//         normals.Add(new Vector3(1.0f, 0.0f, 0.0f));
// 
//         MeshData meshData = new MeshData();
//         List<float> dataList = new List<float>();
//         for (int i = 0; i < vertices.Count; ++i )
//         {
//             Vector3 pos = vertices[i];
//             Vector3 normal = normals[i];
//             dataList.Add(pos.x);
//             dataList.Add(pos.y);
//             dataList.Add(pos.z);
//             dataList.Add(normal.x);
//             dataList.Add(normal.y);
//             dataList.Add(normal.z);
//         }
//         meshData.m_data = dataList.ToArray();
//         meshData.m_index = index.ToArray();
//         return meshData;
//     }

    public void OnUpdateFrame(OpenTK.FrameEventArgs e)
    {

    }

    public void OnRenderFrame(OpenTK.FrameEventArgs e)
    {
        //ushort[] indices = { 0, 1, 2, 0, 2, 3 };

        float[] black = new float[] { 0, 0, 0, 1 };
        GL.ClearBuffer(ClearBuffer.Color, 0, black);
        float[] ones = new float[] { 1.0f };
        GL.ClearBuffer(ClearBuffer.Depth, 0, ones);

        GL.UseProgram(m_program);

        const int POS_INDEX = 0;
        const int NORMAL_INDEX = 1;
        const int POS_SIZE = 3;
        const int NORMAL_SIZE = 3;
        GL.VertexAttribPointer(POS_INDEX, POS_SIZE, All.Float, false, (POS_SIZE + NORMAL_SIZE) * sizeof(float), m_ptr);
        GL.VertexAttribPointer(NORMAL_INDEX, NORMAL_SIZE, All.Float, false, (POS_SIZE + NORMAL_SIZE) * sizeof(float), m_ptr + sizeof(float) * POS_SIZE);

        GL.EnableVertexAttribArray(POS_INDEX);
        GL.EnableVertexAttribArray(NORMAL_INDEX);

        float[] vEyeLight = { -100.0f, 100.0f, 100.0f };
		float[] vAmbientColor = { 0.1f, 0.1f, 0.1f, 1.0f };
		float[] vDiffuseColor = { 0.0f, 0.0f, 1.0f, 1.0f };
		float[] vSpecularColor = { 1.0f, 1.0f, 1.0f, 1.0f };

        GL.Uniform4(m_locAmbient, 1, vAmbientColor);
        GL.Uniform4(m_locDiffuse, 1, vDiffuseColor);
        GL.Uniform4(m_locSpecular, 1, vSpecularColor);
        GL.Uniform3(m_locLight, 1, vEyeLight);

        m_accTime += e.Time;

        Matrix4x4 modelUnity = Matrix4x4.TRS(Vector3.one, Quaternion.identity, Vector3.one);
        Transform cameraTrans = new Transform();
        cameraTrans.position = new Vector3(10, 10, 10);
        cameraTrans.forward = Vector3.zero - cameraTrans.position;
        Matrix4x4 cameraLocalToWorld = cameraTrans.localToWorldMatrix;
        Matrix4x4 viewUnity = UnityWorldToCameraMatrix(cameraLocalToWorld);
        Matrix4x4 projectionUnity = Matrix4x4.Perspective(60, m_width / (float)m_height, 0.1f, 100f);
        Matrix4x4 mvUnity = viewUnity * modelUnity;
        Matrix4x4 mvpUnity = projectionUnity * viewUnity * modelUnity;
        Matrix4x4 mvUnity2 = viewUnity.transpose * modelUnity.transpose;
        Matrix4x4 mvpUnity2 = projectionUnity.transpose * viewUnity.transpose * modelUnity.transpose;

        //Vector3 testPoint = Vector3.one;
        Vector4 testPoint = mvpUnity.MultiplyPoint(Vector3.one); 
        //Vector4 testPoint = mvpUnity * Vector3.one; 
        //Matrix4x4 cameraLocalToWorldUnity = Matrix4x4.TRS(new Vector3(10, 10, 10), Quaternion.Euler(45, 0, 0), Vector3.one);
        //Matrix4x4 view = UnityWorldToCameraMatrix(cameraLocalToWorld);
        //Matrix4x4 projection = Matrix4x4.Perspective(60, m_width / (float)m_height, 0.1f, 100f);
        //Matrix4x4 mv = view * model;
        //Matrix4x4 mvp = projection * view * model;

        OpenTK.Matrix4 model = SRT(OpenTK.Vector3.One, OpenTK.Quaternion.Identity, OpenTK.Vector3.One);
        //OpenTK.Matrix4 cameraLocaltoWorld = TRS(new OpenTK.Vector3(0, 0, 50), new OpenTK.Quaternion(0, 0, 0), OpenTK.Vector3.One);
        //OpenTK.Matrix4 view = worldToCameraMatrix(cameraLocaltoWorld);
        OpenTK.Matrix4 view = LookAt(new OpenTK.Vector3(10, 10, 10), OpenTK.Vector3.Zero, new OpenTK.Vector3(0, 1, 0));
        OpenTK.Matrix4 projection = Perspective(60, m_width / (float)m_height, 0.1f, 100f);

        //  坑死。。opengl变换从左往右乘
        OpenTK.Matrix4 mv = model * view;
        OpenTK.Matrix4 mv2 = Multiply(model, view);
        OpenTK.Matrix4 mv3 = GLVMathMultiply(view, model);
        OpenTK.Matrix4 mvp = model * view * projection;
        OpenTK.Matrix4 mvp2 = Multiply(mv2, projection);
        OpenTK.Matrix4 mvp3 = GLVMathMultiply(projection, mv3);

        OpenTK.Vector4 testPoint2 = LeftMultiply(OpenTK.Vector3.One, mvp);
        testPoint2 /= testPoint2.W;
        OpenTK.Vector4 testPoint3 = RightMultiply(mvp, OpenTK.Vector3.One);
        //OpenTK.Vector4 testPoint4 = LeftMultiply(OpenTK.Vector3.One, mvp);

        //UnityEngine.Debug.Log(mvp.ToString());
        //OpenTK.Matrix4 mvp2 = ConverToFloat2(mvp);
        //OpenTK.Matrix4 mv2 = ConverToFloat2(mv);
        OpenGLHelper.ClearGLError();
        //GL.UniformMatrix4(m_locMVP, false, ref mvp);


        GL.UniformMatrix4(m_locMVP, 1, false, ConverToFloat(mvp));


        OpenGLHelper.CheckGLError();
        GL.UniformMatrix4(m_locMV, false, ref mv);
        //GL.UniformMatrix4(m_locMVP, 1, false, ConverToFloat(mvp));
        //GL.UniformMatrix4(m_locMV, 1, false, ConverToFloat(mv));

        GL.DrawElements(PrimitiveType.Triangles, m_meshData.m_index.Length, DrawElementsType.UnsignedShort, m_meshData.m_index);
    }

    public static OpenTK.Matrix4 SRT(OpenTK.Vector3 pos, OpenTK.Quaternion qua, OpenTK.Vector3 scale)
    {
        OpenTK.Matrix4 t = OpenTK.Matrix4.CreateTranslation(pos);
        OpenTK.Matrix4 r = OpenTK.Matrix4.CreateFromQuaternion(qua);
        OpenTK.Matrix4 s = OpenTK.Matrix4.CreateScale(scale);
        return s * r * t;
    }

    public static OpenTK.Matrix4 LookAt(OpenTK.Vector3 eye, OpenTK.Vector3 center, OpenTK.Vector3 up)
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

    public static OpenTK.Matrix4 Perspective(float fovy, float aspect, float n, float f)
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

    float[] ConverToFloat(OpenTK.Matrix4 mat)
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
        ret[10] =mat.M33;
        ret[11] =mat.M34;

        ret[12] =mat.M41;
        ret[13] =mat.M42;
        ret[14] =mat.M43;
        ret[15] = mat.M44;
        return ret;
    }

    float[] ConverToFloat(Matrix4x4 mat)
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

    static OpenTK.Matrix4 ConvertUnityMatrixToOpenTkMatrix(Matrix4x4 mat)
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

    static Matrix4x4 ConvertOpenTkMatrixToUnityMatrix(OpenTK.Matrix4 ret)
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

