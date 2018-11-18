using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.ES30;
using System.Runtime.InteropServices;

class DeferredShadingDemo : IDemo
{
    public string prePassVShader = @"
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
    public string prePassFShader = @"
#version 300 es
precision highp float;

uniform vec4    ambientColor;
uniform vec4    diffuseColor; 
uniform vec4 specularColor;

in vec3 vVaryingNormal;
in vec3 vVaryingLightDir;

layout(location = 0) out vec4 color0;
layout(location = 1) out vec4 color1;

void main(void)
    {
        color0 = vec4(vVaryingNormal, 1);
        color1 = vec4(vVaryingLightDir, 1);
    }
";
    public string lightPassVShader = @"
#version 300 es
precision highp float;

void main(void)
{
    const vec4 verts[4] = vec4[4](vec4(-1.0, -1.0, 0.5, 1.0),
                                  vec4( 1.0, -1.0, 0.5, 1.0),
                                  vec4(-1.0,  1.0, 0.5, 1.0),
                                  vec4( 1.0,  1.0, 0.5, 1.0));

    gl_Position = verts[gl_VertexID];
}
";

    public string lightPassFShader = @"
#version 300 es
precision highp float;

layout (location = 0) out vec4 vFragColor;

uniform vec4    ambientColor;
uniform vec4    diffuseColor; 

uniform sampler2D gbuf_tex0;
uniform sampler2D gbuf_tex1;

void main(void)
{
    ivec2 coord = ivec2(gl_FragCoord.xy);
    vec4 data0 = texelFetch(gbuf_tex0, ivec2(coord), 0);
    vec4 data1 = texelFetch(gbuf_tex1, ivec2(coord), 0);

    vec3 vVaryingNormal = data0.xyz;
    vec3 vVaryingLightDir = data1.xyz;
    
            // Dot product gives us diffuse intensity
    float diff = max(0.0, dot(normalize(vVaryingNormal), normalize(vVaryingLightDir)));

    // Multiply intensity by diffuse color, force alpha to 1.0
    vFragColor = diff * diffuseColor;

    // Add in ambient light
    vFragColor += ambientColor;


    // Specular Light
    vec3 vReflection = normalize(reflect(-normalize(vVaryingLightDir), normalize(vVaryingNormal)));
    float spec = max(0.0, dot(normalize(vVaryingNormal), vReflection));
    if(diff != 0.0f) 
    {
        float fSpec = pow(spec, 128.0);
        vFragColor.rgb += vec3(fSpec, fSpec, fSpec);
    }
}

";

    public int m_prePassProgram;
    public int m_lightPassProgram;
    public OpenGLHelper.MeshData m_meshData;
    IntPtr m_ptr;
    
    int m_width;
    int m_height;

    int m_gbuffer;
    int[] m_gbuffer_tex = new int[3];

    public void Init(MainWindow mainWindow)
    {
        m_prePassProgram = OpenGLHelper._CompilerShader(prePassVShader, prePassFShader);
        m_lightPassProgram = OpenGLHelper._CompilerShader(lightPassVShader, lightPassFShader);
        m_meshData = OpenGLHelper.GetCubeMesh();
        m_ptr = Marshal.AllocHGlobal(sizeof(float) * m_meshData.m_data.Length);
        Marshal.Copy(m_meshData.m_data, 0, m_ptr, m_meshData.m_data.Length);
        m_width = mainWindow.Width;
        m_height = mainWindow.Height;

        GL.Enable(EnableCap.DepthTest);
        GL.DepthFunc(All.Lequal);
        GL.Enable(EnableCap.CullFace);

        m_gbuffer = GL.GenFramebuffer();
        GL.BindFramebuffer(All.Framebuffer, m_gbuffer);
        GL.GenTextures(3, m_gbuffer_tex);

        GL.BindTexture(All.Texture2D, m_gbuffer_tex[0]);
        GL.TexStorage2D(All.Texture2D, 1, All.Rgba32f, m_width, m_height);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Nearest);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Nearest);

        GL.BindTexture(All.Texture2D, m_gbuffer_tex[1]);
        GL.TexStorage2D(All.Texture2D, 1, All.Rgba32f, m_width, m_height);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Nearest);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Nearest);

        GL.BindTexture(All.Texture2D, m_gbuffer_tex[2]);
        GL.TexStorage2D(All.Texture2D, 1, All.DepthComponent32f, m_width, m_height);

        GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget2d.Texture2D, m_gbuffer_tex[0], 0);
        GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment1, TextureTarget2d.Texture2D, m_gbuffer_tex[1], 0);
        GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget2d.Texture2D, m_gbuffer_tex[2], 0);

        GL.BindFramebuffer(All.Framebuffer, 0);
    }

    public void OnUpdateFrame(OpenTK.FrameEventArgs e)
    {

    }

    public void OnRenderFrame(OpenTK.FrameEventArgs e)
    {
        float[] black = new float[] { 0, 0, 0, 1 };
        GL.ClearBuffer(ClearBuffer.Color, 0, black);
        float[] ones = new float[] { 1.0f };
        GL.ClearBuffer(ClearBuffer.Depth, 0, ones);

        GL.UseProgram(m_prePassProgram);

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

        GL.DrawElements(PrimitiveType.Triangles, m_meshData.m_index.Length, DrawElementsType.UnsignedShort, m_meshData.m_index);
    }


}

