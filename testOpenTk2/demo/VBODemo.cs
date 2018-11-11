using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.ES30;
using System.Runtime.InteropServices;

class VBODemo : IDemo
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
    //int m_vboBuff1 = 0;
    //int m_vboBuff2 = 0;
    int[] m_vboBuff = new int[2];

    const int VERTEX_POS_SIZE = 3;
    const int VERTEX_COLOR_SIZE = 4;

    const int VERTEX_POS_INDX = 0;
    const int VERTEX_COLOR_INDX = 1;

    bool m_hasInitIntPtr = false;
    IntPtr m_ptr;

    public void Init(MainWindow mainWindow)
    {
        m_program = OpenGLMgr._CompilerShader(vShaderStr, fShaderStr);


    }

    public void OnUpdateFrame(FrameEventArgs e)
    {

    }

    public void OnRenderFrame(FrameEventArgs e)
    {

        float[] vertices = new float[]
        {
            0f, 0.5f, 0.0f, //v0
            1.0f, 0f, 0f, 1f, // c0
            -0.5f, -0.5f, 0f, // v1
            0f, 1f, 0f, 1f, // c1
            0.5f, -0.5f, 0f, //v2
            1f, 1f, 1f, 1f, //c2
        };
        ushort[] indices = new ushort[] { 0, 1, 2 };

        float[] black = new float[] { 0, 0, 0, 1 };
        GL.ClearBuffer(ClearBuffer.Color, 0, black);
        //OpenGLMgr.CheckGLError();

        GL.UseProgram(m_program);
        //OpenGLMgr.CheckGLError();

        int offsetLoc = GL.GetUniformLocation(m_program, "u_offset");
        //OpenGLMgr.CheckGLError();
        GL.Uniform1(offsetLoc, 0f);
        DrawPrimitiveWithoutVBOs(vertices, indices);

        GL.Flush();
        
        GL.Uniform1(offsetLoc, 1f);
        //OpenGLMgr.CheckGLError();
        
        DrawPrimitiveWithVBOs(3, vertices, sizeof(float) * (VERTEX_POS_SIZE + VERTEX_COLOR_SIZE), 3, indices);

        //GL.Finish();
        //OpenGLMgr.CheckGLError();
    }

     void DrawPrimitiveWithoutVBOs(float[] vertices, ushort[] indices)
     {
         GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
         //OpenGLMgr.CheckGLError();
         GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
         //OpenGLMgr.CheckGLError();
 
         GL.EnableVertexAttribArray(VERTEX_POS_INDX);
         //OpenGLMgr.CheckGLError();
         GL.EnableVertexAttribArray(VERTEX_COLOR_INDX);
         //OpenGLMgr.CheckGLError();
 
         int stride = sizeof(float) * (VERTEX_POS_SIZE + VERTEX_COLOR_SIZE);

         if (!m_hasInitIntPtr)
         {
             m_ptr = Marshal.AllocHGlobal(sizeof(float) * vertices.Length);
             Marshal.Copy(vertices, 0, m_ptr, vertices.Length);
             //Marshal.FreeHGlobal(m_ptr);
         }

         GL.VertexAttribPointer(VERTEX_POS_INDX, VERTEX_POS_SIZE, VertexAttribPointerType.Float, false, stride, m_ptr);
         //OpenGLMgr.CheckGLError();
         GL.VertexAttribPointer(VERTEX_COLOR_INDX, VERTEX_COLOR_SIZE, VertexAttribPointerType.Float, false, stride, m_ptr + sizeof(float) * VERTEX_POS_SIZE);
         //OpenGLMgr.CheckGLError();
         GL.DrawElements(PrimitiveType.Triangles, 3, DrawElementsType.UnsignedShort, indices);
         //OpenGLMgr.CheckGLError();
 
         GL.DisableVertexAttribArray(VERTEX_POS_INDX);
         GL.DisableVertexAttribArray(VERTEX_COLOR_INDX);
     }

    void DrawPrimitiveWithVBOs(int numVertices, float[] vtxBuf, int vtxStride, int numIndices, ushort[] indices)
    {
        if(m_vboBuff[0] == 0 && m_vboBuff[1] == 0)
        {
            GL.GenBuffers(2, m_vboBuff);
            //OpenGLMgr.CheckGLError();

            GL.BindBuffer(BufferTarget.ArrayBuffer, m_vboBuff[0]);
            //OpenGLMgr.CheckGLError();
            GL.BufferData(BufferTarget.ArrayBuffer, vtxStride * numIndices, vtxBuf, BufferUsageHint.StaticDraw);
            //OpenGLMgr.CheckGLError();

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, m_vboBuff[1]);
            //OpenGLMgr.CheckGLError();
            GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(ushort) * numIndices, indices, BufferUsageHint.StaticDraw);
            //OpenGLMgr.CheckGLError();
        }

        GL.BindBuffer(BufferTarget.ArrayBuffer, m_vboBuff[0]);
        //OpenGLMgr.CheckGLError();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, m_vboBuff[1]);
        //OpenGLMgr.CheckGLError();

        GL.EnableVertexAttribArray(VERTEX_POS_INDX);
        //OpenGLMgr.CheckGLError();
        GL.EnableVertexAttribArray(VERTEX_COLOR_INDX);
        //OpenGLMgr.CheckGLError();

        int offset = 0;
        GL.VertexAttribPointer(VERTEX_POS_INDX, VERTEX_POS_SIZE, VertexAttribPointerType.Float, false, vtxStride, offset);
        //OpenGLMgr.CheckGLError();
        offset += VERTEX_POS_SIZE * sizeof(float);
        GL.VertexAttribPointer(VERTEX_COLOR_INDX, VERTEX_COLOR_SIZE, VertexAttribPointerType.Float, false, vtxStride, offset);
        //OpenGLMgr.CheckGLError();

        //int test = 0;
        //GL.DrawElements(BeginMode.Triangles, numIndices, DrawElementsType.UnsignedShort, 0);
        GL.DrawElements(PrimitiveType.Triangles, numIndices, DrawElementsType.UnsignedShort, IntPtr.Zero);
        //OpenGLMgr.CheckGLError();

        GL.DisableVertexAttribArray(VERTEX_POS_INDX);
        //OpenGLMgr.CheckGLError();
        GL.DisableVertexAttribArray(VERTEX_COLOR_INDX);
        //OpenGLMgr.CheckGLError();

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        //OpenGLMgr.CheckGLError();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        //OpenGLMgr.CheckGLError();
    }
}

