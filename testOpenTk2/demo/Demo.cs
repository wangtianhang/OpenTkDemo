using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.ES30;
using OpenTK;

class Demo
{
    const string m_vertexShaderSrc = @"
#version 430 core
layout(location = 0) in vec4 offset;
void main(void)
{
    const vec4 vertices[3] = vec4[3](vec4(0.25, -0.25, 0.5, 1.0),
    vec4(-0.25, -0.25, 0.5, 1.0),
    vec4(0.25, 0.25, 0.5, 1.0));

    gl_Position = vertices[gl_VertexID] + offset;
}
";

    const string m_pixelShaderSrc = @"
#version 430 core
out vec4 color;
void main(void)
{
    color = vec4(0.0, 0.8, 1.0, 1.0f);
}
";

    int m_renderPorgram = 0;
    int m_vertexArrayObject = 0;
    float m_totalTime = 0;

    public void LoadScene()
    {
        m_renderPorgram = OpenGLMgr._CompilerShader(m_vertexShaderSrc, m_pixelShaderSrc);
        m_vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(m_vertexArrayObject);
    }

    public void OnRenderFrame(FrameEventArgs e)
    {
        //Title = $"(Vsync: {VSync}) FPS: {1f / e.Time:0}";
        

        //Color4 backColor;
        //backColor.A = 1.0f;
        //backColor.R = 0.1f;
        //backColor.G = 0.1f;
        //backColor.B = 0.3f;
        //GL.ClearColor(backColor);
        //GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        //float[] color = new float[] { 1.0f, 0.0f, 0.0f, 1.0f};
        m_totalTime += (float)e.Time;
        float[] color = new float[] {(float)Math.Sin(m_totalTime) * 0.5f + 0.5f, 
        (float)Math.Cos(m_totalTime) * 0.5f + 0.5f,
        0, 1.0f};
        GL.ClearBuffer(ClearBuffer.Color, 0, color);

        GL.UseProgram(m_renderPorgram);
        //GL.PointSize(40);
        //GL.DrawArrays(PrimitiveType.Points, 0, 1);
        float[] attrib = new float[] { (float)Math.Sin(m_totalTime) * 0.5f, 
        (float)Math.Cos(m_totalTime) * 0.6f, 
        0.0f, 0.0f};

        GL.VertexAttrib4(0, attrib);

        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

        //SwapBuffers();
    }
}

