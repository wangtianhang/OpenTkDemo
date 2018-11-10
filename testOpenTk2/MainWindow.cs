using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.ES30;
using OpenTK.Input;

using UnityEngine;

class MainWindow : GameWindow
{
    public enum DemoType
    {
        None,
        OpenGLTriangle,
        FakeUnity,
    }

    Application m_application = null;
    OpenGLTriangleDemo m_demo = null;


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
        

        LogInfo();

        CreateDemo(DemoType.OpenGLTriangle);
    }

    void CreateDemo(DemoType demoType)
    {

        if (demoType == DemoType.OpenGLTriangle)
        {
            m_demo = new OpenGLTriangleDemo();
            m_demo.Init(this);
        }
        else if (demoType == DemoType.FakeUnity)
        {
            m_application = new Application();
            m_application._Init(this);
        }
    }

//     void InitScene()
//     {
// 
//     }

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
        if (m_application != null)
        {
            m_application.OnUpdateFrame(e);
        }
        
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        if (m_application != null)
        {
            //m_demo.OnRenderFrame(e);
            m_application.OnRenderFrame(e);
        }
        if (m_demo != null)
        {
            m_demo.OnRenderFrame(e);
        }
        SwapBuffers();
    }
}

