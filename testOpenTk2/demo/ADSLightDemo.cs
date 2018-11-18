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
        Matrix4x4 viewUnity = OpenGLHelper.UnityWorldToCameraMatrix(cameraLocalToWorld);
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

        OpenTK.Matrix4 model = OpenGLHelper.GLSRT(OpenTK.Vector3.One, OpenTK.Quaternion.Identity, OpenTK.Vector3.One);
        //OpenTK.Matrix4 cameraLocaltoWorld = TRS(new OpenTK.Vector3(0, 0, 50), new OpenTK.Quaternion(0, 0, 0), OpenTK.Vector3.One);
        //OpenTK.Matrix4 view = worldToCameraMatrix(cameraLocaltoWorld);
        OpenTK.Matrix4 view = OpenGLHelper.GLLookAt(new OpenTK.Vector3(10, 10, 10), OpenTK.Vector3.Zero, new OpenTK.Vector3(0, 1, 0));
        OpenTK.Matrix4 projection = OpenGLHelper.GLPerspective(60, m_width / (float)m_height, 0.1f, 100f);

        //  坑死。。opengl变换从左往右乘
        OpenTK.Matrix4 mv = model * view;
        OpenTK.Matrix4 mv2 = OpenGLHelper.Multiply(model, view);
        OpenTK.Matrix4 mv3 = OpenGLHelper.GLVMathMultiply(view, model);
        OpenTK.Matrix4 mvp = model * view * projection;
        OpenTK.Matrix4 mvp2 = OpenGLHelper.Multiply(mv2, projection);
        OpenTK.Matrix4 mvp3 = OpenGLHelper.GLVMathMultiply(projection, mv3);

        OpenTK.Vector4 testPoint2 = OpenGLHelper.LeftMultiply(OpenTK.Vector3.One, mvp);
        testPoint2 /= testPoint2.W;
        OpenTK.Vector4 testPoint3 = OpenGLHelper.RightMultiply(mvp, OpenTK.Vector3.One);
        //OpenTK.Vector4 testPoint4 = LeftMultiply(OpenTK.Vector3.One, mvp);

        //UnityEngine.Debug.Log(mvp.ToString());
        //OpenTK.Matrix4 mvp2 = ConverToFloat2(mvp);
        //OpenTK.Matrix4 mv2 = ConverToFloat2(mv);
        OpenGLHelper.ClearGLError();
        //GL.UniformMatrix4(m_locMVP, false, ref mvp);


        GL.UniformMatrix4(m_locMVP, 1, false, OpenGLHelper.ConverToFloat(mvp));


        OpenGLHelper.CheckGLError();
        GL.UniformMatrix4(m_locMV, false, ref mv);
        //GL.UniformMatrix4(m_locMVP, 1, false, ConverToFloat(mvp));
        //GL.UniformMatrix4(m_locMV, 1, false, ConverToFloat(mv));

        GL.DrawElements(PrimitiveType.Triangles, m_meshData.m_index.Length, DrawElementsType.UnsignedShort, m_meshData.m_index);
    }


}

