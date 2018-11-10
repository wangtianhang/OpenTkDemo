﻿using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum DemoType
{
    None,
    OpenGLTriangle,
    FakeUnity,
}

public interface IDemo
{
    void Init(MainWindow mainWindow);

    void OnUpdateFrame(FrameEventArgs e);

    void OnRenderFrame(FrameEventArgs e);
}

class DemoFactory
{
    
    public static IDemo CreateDemo()
    {
        if (DemoConfig.m_demoType == DemoType.OpenGLTriangle)
        {
            return new OpenGLTriangleDemo();
        }
        else if (DemoConfig.m_demoType == DemoType.FakeUnity)
        {
            return new UnityEngine.Application();
        }
        else
        {
            return null;
        }
    }
}

