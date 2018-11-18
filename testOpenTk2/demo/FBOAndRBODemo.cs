﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.ES30;
using System.Runtime.InteropServices;


class FBOAndRBODemo : IDemo
{
    const string vShaderStr = @"
#version 300 es
precision highp float;
layout(location = 0) in vec4 a_position;
layout(location = 1) in vec2 a_texCoord;
out vec2 v_texCoord;
void main()
{
    gl_Position = a_position;
    v_texCoord = a_texCoord;
}
";

    const string fShaderStr = @"
#version 300 es
precision highp float;
in vec2 v_texCoord;      
out vec4 outColor; 
uniform sampler2D s_texture; 
void main()    
{
    outColor = texture( s_texture, v_texCoord );
}
";
    public int m_program;
    public int m_textureId;
    IntPtr m_ptr;

    public int m_fboId = 0;
    public int m_depthRbo = 0;
    public int m_colorRbo = 0;

    int m_width;
    int m_height;

    public void Init(MainWindow mainWindow)
    {
        

        m_program = OpenGLHelper._CompilerShader(vShaderStr, fShaderStr);

        m_textureId = CreateSimpleTexture2D();

        float[] vVertices = new float[] { -0.5f,  0.5f, 0.0f,  // Position 0
                        0.0f,  0.0f,        // TexCoord 0 
                        -0.5f, -0.5f, 0.0f,  // Position 1
                        0.0f,  1.0f,        // TexCoord 1
                        0.5f, -0.5f, 0.0f,  // Position 2
                        1.0f,  1.0f,        // TexCoord 2
                        0.5f,  0.5f, 0.0f,  // Position 3
                        1.0f,  0.0f         // TexCoord 3
                        };
        m_ptr = Marshal.AllocHGlobal(sizeof(float) * vVertices.Length);
        Marshal.Copy(vVertices, 0, m_ptr, vVertices.Length);

        m_width = mainWindow.Width;
        m_height = mainWindow.Height;

        InitFBOAndRBO();
        
    }


    void InitFBOAndRBO()
    {
        m_fboId = GL.GenFramebuffer();

        m_depthRbo = GL.GenRenderbuffer();
        GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, m_depthRbo);
        GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferInternalFormat.DepthComponent32f, m_width, m_height);

        m_colorRbo = GL.GenRenderbuffer();
        GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, m_colorRbo);
        GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferInternalFormat.Rgba8, m_width, m_height);

        GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, m_fboId);
        GL.FramebufferRenderbuffer(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, m_depthRbo);
        GL.FramebufferRenderbuffer(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.ColorAttachment0, RenderbufferTarget.Renderbuffer, m_colorRbo);

        GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);
    }

    public int CreateSimpleTexture2D()
    {
        int textureId;

        byte[] pixels = new byte[] 
        {
            255,   0,   0, // Red
            0, 255,   0, // Green
            0,   0, 255, // Blue
          255, 255,   0  // Yellow
        };

        GL.PixelStore(All.UnpackAlignment, 1);

        textureId = GL.GenTexture();

        GL.BindTexture(All.Texture2D, textureId);

        GL.TexImage2D(All.Texture2D, 0, All.Rgb8, 2, 2, 0, All.Rgb, All.UnsignedByte, pixels);

        GL.TexParameter(All.Texture2D, All.TextureMinFilter, (int)All.Nearest);
        GL.TexParameter(All.Texture2D, All.TextureMagFilter, (int)All.Nearest);

        return textureId;
    }

    public void OnUpdateFrame(FrameEventArgs e)
    {

    }

    public void OnRenderFrame(FrameEventArgs e)
    {
        //int[] fboArray = new int[1];
        All[] fboBuffs = new All[] { All.ColorAttachment0 };
        //fboArray[0] = m_fboId;
        GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, m_fboId);
        GL.DrawBuffers(1, fboBuffs);

        ushort[] indices = { 0, 1, 2, 0, 2, 3 };

        float[] black = new float[] { 0, 0, 0, 1 };
        GL.ClearBuffer(ClearBuffer.Color, 0, black);

        GL.UseProgram(m_program);

        const int POS_INDEX = 0;
        const int UV_INDEX = 1;
        const int POS_SIZE = 3;
        const int UV_SIZE = 2;
        GL.VertexAttribPointer(POS_INDEX, POS_SIZE, All.Float, false, (POS_SIZE + UV_SIZE) * sizeof(float), m_ptr);
        GL.VertexAttribPointer(UV_INDEX, UV_SIZE, All.Float, false, (POS_SIZE + UV_SIZE) * sizeof(float), m_ptr + sizeof(float) * POS_SIZE);

        GL.EnableVertexAttribArray(POS_INDEX);
        GL.EnableVertexAttribArray(UV_INDEX);

        GL.ActiveTexture(All.Texture0);
        GL.BindTexture(All.Texture2D, m_textureId);
        int index = GL.GetUniformLocation(m_program, "s_texture");
        GL.Uniform1(index, (int)All.Texture0);

        GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedShort, indices);

        GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);
        GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, m_fboId);
        GL.ReadBuffer(All.ColorAttachment0);
        GL.BlitFramebuffer(0, 0, m_width, m_height, 0, 0, m_width / 2, m_height / 2, All.ColorBufferBit, All.Nearest);
    }
}

