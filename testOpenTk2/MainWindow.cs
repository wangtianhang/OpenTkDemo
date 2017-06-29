using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

class MainWindow : GameWindow
{
    string m_titlePrefix = null;

    public MainWindow()
        : base(1280, // initial width
            720, // initial height
            GraphicsMode.Default,
            "dreamstatecoding",  // initial title
            GameWindowFlags.Default,
            DisplayDevice.Default,
            4, // OpenGL major version
            0, // OpenGL minor version
            GraphicsContextFlags.ForwardCompatible)
    {
        m_titlePrefix = "dreamstatecoding" + ": OpenGL Version: " + GL.GetString(StringName.Version);
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
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        //Title = $"(Vsync: {VSync}) FPS: {1f / e.Time:0}";
        Title = m_titlePrefix + " Vsync " + VSync + " FPS " + (1 / e.Time).ToString("f0");

        Color4 backColor;
        backColor.A = 1.0f;
        backColor.R = 0.1f;
        backColor.G = 0.1f;
        backColor.B = 0.3f;
        GL.ClearColor(backColor);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        SwapBuffers();
    }
}

