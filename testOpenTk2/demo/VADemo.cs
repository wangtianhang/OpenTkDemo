
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.ES30;

class VADemo : IDemo
{
    const string vShaderStr = @"
#version 300 es
precision highp float;
layout(location = 0) in vec4 a_color;
layout(location = 1) in vec4 a_position;
out vec4 v_color;
void main()
{
    v_color = a_color;
    gl_Position = a_position;
}
        ";
    const string fShaderStr = @"
#version 300 es
precision highp float;
in vec4 v_color;
out vec4 o_fragColor;
void main()
{
    o_fragColor = v_color;
}
";
    int m_program = 0;

    public void Init(MainWindow mainWindow)
    {
        m_program = OpenGLMgr._CompilerShader(vShaderStr, fShaderStr);
    }

    public void OnUpdateFrame(FrameEventArgs e)
    {
        
    }

    public void OnRenderFrame(FrameEventArgs e)
    {
        float[] black = new float[] { 0, 0, 0, 1 };
        float[] red = new float[] { 1, 0, 0, 1 };
        float[] vertexPos = new float[] {
        0f, 0.5f, 0f, // v0;
        -0.5f, -0.5f, 0f, // v1;
        0.5f, -0.5f, 0f, // v2;
        };
        GL.ClearBuffer(ClearBuffer.Color, 0, black);

        GL.UseProgram(m_program);

        GL.VertexAttrib4(0, red);
        GL.VertexAttribPointer<float>(1, 3, VertexAttribPointerType.Float, false, 0, vertexPos);

        GL.EnableVertexAttribArray(1);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
        GL.DisableVertexAttribArray(1);
    }
}

