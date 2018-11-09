using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

using UnityEngine;

class MainWindow : GameWindow
{
    string m_titlePrefix = null;
    float m_totalTime = 0;
    int m_renderPorgram = 0;
    int m_vertexArrayObject = 0;

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

    public MainWindow()
        : base(800, // initial width
            600, // initial height
            GraphicsMode.Default,
            "dreamstatecoding",  // initial title
            GameWindowFlags.Default,
            DisplayDevice.Default,
            4, // OpenGL major version
            0, // OpenGL minor version
            GraphicsContextFlags.ForwardCompatible)
    {
        m_titlePrefix = "dreamstatecoding" + ": OpenGL Version: " + GL.GetString(StringName.Version);

        LogInfo();

        m_renderPorgram = CompilerShader();
        m_vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(m_vertexArrayObject);

        Application._Init();

        InitScene();
    }

    void InitScene()
    {

    }

    void LogInfo()
    {
        string Version = GL.GetString(StringName.Version);
        Console.WriteLine("Version " + Version);
        string Vendor = GL.GetString(StringName.Vendor);
        Console.WriteLine("Vendor " + Vendor);
        string ShadingLanguageVersion = GL.GetString(StringName.ShadingLanguageVersion);
        Console.WriteLine("ShadingLanguageVersion " + ShadingLanguageVersion);
        string Renderer = GL.GetString(StringName.Renderer);
        Console.WriteLine("Renderer " + Renderer);
        string Extensions = GL.GetString(StringName.Extensions);
        string[] ExtensionsArray = Extensions.Split(' ');
        foreach (var iter in ExtensionsArray)
        {
            Console.WriteLine("Extensions " + iter);
        }
    }

    int CompilerShader()
    {
        int vertexShader = 0;
        int pixelShader = 0;
        int shaderProgram = 0;

        vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, m_vertexShaderSrc);
        GL.CompileShader(vertexShader);
        
        int length = 0;
        GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out length);
        string errorInfo = GL.GetShaderInfoLog(vertexShader);
        Console.WriteLine("vertexShader " + errorInfo + " " + length);

        pixelShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(pixelShader, m_pixelShaderSrc);
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

    protected override void OnResize(EventArgs e)
    {
        GL.Viewport(0, 0, Width, Height);
    }

    protected override void OnLoad(EventArgs e)
    {
        CursorVisible = true;
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        HandleKeyboard();
    }
    private void HandleKeyboard()
    {
        var keyState = Keyboard.GetState();

        if (keyState.IsKeyDown(Key.Escape))
        {
            Exit();
        }

        MouseState mouseState = Mouse.GetState();
        if(mouseState.IsButtonDown(MouseButton.Left))
        {
            //Console.WriteLine("mouse left btn down");
        }
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        //Title = $"(Vsync: {VSync}) FPS: {1f / e.Time:0}";
        Title = m_titlePrefix + " Vsync " + VSync + " FPS " + (1 / e.Time).ToString("f0");

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

        SwapBuffers();
    }
}

