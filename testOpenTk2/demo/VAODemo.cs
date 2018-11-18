using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.ES30;

class VAODemo : IDemo
{
    const string vShaderStr = @"
#version 300 es
precision highp float;
layout(location = 0) in vec4 a_position;
layout(location = 1) in vec4 a_color;
uniform float u_offset;
out vec4 v_color;
void main()
{
    v_color = a_color;
    gl_Position = a_position;
    gl_Position.x += u_offset;
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

    int[] m_vboBuff = new int[2];

    const int VERTEX_POS_SIZE = 3;
    const int VERTEX_COLOR_SIZE = 4;

    const int VERTEX_POS_INDX = 0;
    const int VERTEX_COLOR_INDX = 1;

    int m_vaoId;

    float[] m_vertices = new float[]
        {
            0f, 0.5f, 0.0f, //v0
            1.0f, 0f, 0f, 1f, // c0
            -0.5f, -0.5f, 0f, // v1
            0f, 1f, 0f, 1f, // c1
            0.5f, -0.5f, 0f, //v2
            1f, 1f, 1f, 1f, //c2
        };
    ushort[] m_indices = new ushort[] { 0, 1, 2 };

    public void Init(MainWindow mainWindow)
    {
        m_program = OpenGLHelper._CompilerShader(vShaderStr, fShaderStr);



        int vtxStride = sizeof(float) * (VERTEX_POS_SIZE + VERTEX_COLOR_SIZE);
        GL.GenBuffers(2, m_vboBuff);
        GL.BindBuffer(BufferTarget.ArrayBuffer, m_vboBuff[0]);
        GL.BufferData(BufferTarget.ArrayBuffer, vtxStride * m_indices.Length, m_vertices, BufferUsageHint.StaticDraw);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, m_vboBuff[1]);
        GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(ushort) * m_indices.Length, m_indices, BufferUsageHint.StaticDraw);

        m_vaoId = GL.GenVertexArray();
        GL.BindVertexArray(m_vaoId);

        GL.BindBuffer(BufferTarget.ArrayBuffer, m_vboBuff[0]);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, m_vboBuff[1]);
        GL.EnableVertexAttribArray(VERTEX_POS_INDX);
        GL.EnableVertexAttribArray(VERTEX_COLOR_INDX);
        int offset = 0;
        GL.VertexAttribPointer(VERTEX_POS_INDX, VERTEX_POS_SIZE, VertexAttribPointerType.Float, false, vtxStride, offset);
        offset += VERTEX_POS_SIZE * sizeof(float);
        GL.VertexAttribPointer(VERTEX_COLOR_INDX, VERTEX_COLOR_SIZE, VertexAttribPointerType.Float, false, vtxStride, offset);

        GL.BindVertexArray(0);
    }

    public void OnUpdateFrame(FrameEventArgs e)
    {

    }

    public void OnRenderFrame(FrameEventArgs e)
    {
        float[] black = new float[] { 0, 0, 0, 1 };
        GL.ClearBuffer(ClearBuffer.Color, 0, black);

        GL.UseProgram(m_program);
        GL.BindVertexArray(m_vaoId);
        GL.DrawElements(PrimitiveType.Triangles, m_indices.Length, DrawElementsType.UnsignedShort, IntPtr.Zero);
        GL.BindVertexArray(0);

    }
}

