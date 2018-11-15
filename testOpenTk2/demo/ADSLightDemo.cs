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
//uniform mat3   normalMatrix;
uniform vec3   vLightPosition;

// Color to fragment program
smooth out vec3 vVaryingNormal;
smooth out vec3 vVaryingLightDir;

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


    // Don't forget to transform the geometry!
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

out vec4 vFragColor;

uniform vec4    ambientColor;
uniform vec4    diffuseColor;   
uniform vec4    specularColor;

smooth in vec3 vVaryingNormal;
smooth in vec3 vVaryingLightDir;


void main(void)
    { 
    vFragColor.rgb = vec3(1, 1, 1);
return;

    // Dot product gives us diffuse intensity
    float diff = max(0.0, dot(normalize(vVaryingNormal), normalize(vVaryingLightDir)));

    // Multiply intensity by diffuse color, force alpha to 1.0
    vFragColor = diff * diffuseColor;

    // Add in ambient light
    vFragColor += ambientColor;


    // Specular Light
    vec3 vReflection = normalize(reflect(-normalize(vVaryingLightDir), normalize(vVaryingNormal)));
    float spec = max(0.0, dot(normalize(vVaryingNormal), vReflection));
    if(diff != 0f) {
        float fSpec = pow(spec, 128.0);
        vFragColor.rgb += vec3(fSpec, fSpec, fSpec);
        }
    }
";
    public class MeshData
    {
        public float[] m_data;
        public int[] m_index;
    }

    public int m_program;

    int m_locAmbient;			// The location of the ambient color
    int m_locDiffuse;			// The location of the diffuse color
    int m_locSpecular;		// The location of the specular color
    int m_locLight;			// The location of the Light in eye coordinates
    int m_locMVP;				// The location of the ModelViewProjection matrix uniform
    int m_locMV;				// The location of the ModelView matrix uniform
    //int m_locNM;				// The location of the Normal matrix uniform

    public MeshData m_meshData;
    IntPtr m_ptr;

    int m_width;
    int m_height;

    public void Init(MainWindow mainWindow)
    {
        m_program = OpenGLMgr._CompilerShader(vShaderStr, fShaderStr);

        m_meshData = GetCubeMesh();

        m_ptr = Marshal.AllocHGlobal(sizeof(float) * m_meshData.m_data.Length);
        Marshal.Copy(m_meshData.m_data, 0, m_ptr, m_meshData.m_data.Length);

        GL.Enable(EnableCap.DepthTest);
        GL.Enable(EnableCap.CullFace);

        m_locAmbient = GL.GetUniformLocation(m_program, "ambientColor");
        m_locDiffuse = GL.GetUniformLocation(m_program, "diffuseColor");
        m_locSpecular = GL.GetUniformLocation(m_program, "specularColor");
        m_locLight = GL.GetUniformLocation(m_program, "vLightPosition");
        m_locMVP = GL.GetUniformLocation(m_program, "mvpMatrix");
        m_locMV = GL.GetUniformLocation(m_program, "mvMatrix");
        //m_locNM = GL.GetUniformLocation(m_program, "normalMatrix");

        m_width = mainWindow.Width;
        m_height = mainWindow.Height;
    }

    MeshData GetCubeMesh()
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


        List<int> index = new List<int>();
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
        for (int i = 0; i < vertices.Count; ++i )
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

    public void OnUpdateFrame(OpenTK.FrameEventArgs e)
    {

    }

    public void OnRenderFrame(OpenTK.FrameEventArgs e)
    {
        //ushort[] indices = { 0, 1, 2, 0, 2, 3 };

        float[] black = new float[] { 0, 0, 0, 1 };
        GL.ClearBuffer(ClearBuffer.Color, 0, black);

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

        Matrix4x4 model = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
        Matrix4x4 view = Matrix4x4.TRS(new Vector3(-2.36f, 4.47f, -4.57f), Quaternion.identity, Vector3.one);
        Matrix4x4 projection = Matrix4x4.Perspective(60, m_width / (float)m_height, 0.1f, 100f);

        Matrix4x4 mv = view * model;
        Matrix4x4 mvp = projection * view * model;
        OpenTK.Matrix4 mvp2 = ConverToFloat2(mvp);
        OpenTK.Matrix4 mv2 = ConverToFloat2(mv);
        GL.UniformMatrix4(m_locMVP, false, ref mvp2);
        GL.UniformMatrix4(m_locMV, false, ref mv2);

        GL.DrawElements(PrimitiveType.Triangles, m_meshData.m_index.Length, DrawElementsType.UnsignedShort, m_meshData.m_index);
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

    OpenTK.Matrix4 ConverToFloat2(Matrix4x4 mat)
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
}

