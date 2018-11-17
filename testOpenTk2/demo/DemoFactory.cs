using OpenTK;
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
    VADemo,
    VBODemo,
    VAODemo,
    Texture2D,
    PBODemo,
    FBOAndRBODemo,
    ADSLightDemo,
    DeferredShadingDemo,
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
        else if (DemoConfig.m_demoType == DemoType.VADemo)
        {
            return new VADemo();
        }
        else if (DemoConfig.m_demoType == DemoType.VBODemo)
        {
            return new VBODemo();
        }
        else if (DemoConfig.m_demoType == DemoType.VAODemo)
        {
            return new VAODemo();
        }
        else if (DemoConfig.m_demoType == DemoType.Texture2D)
        {
            return new Texture2DDemo();
        }
        else if (DemoConfig.m_demoType == DemoType.PBODemo)
        {
            return new PBODemo();
        }
        else if (DemoConfig.m_demoType == DemoType.FBOAndRBODemo)
        {
            return new FBOAndRBODemo();
        }
        else if (DemoConfig.m_demoType == DemoType.ADSLightDemo)
        {
            return new ADSLightDemo();
        }
        else if(DemoConfig.m_demoType == DemoType.DeferredShadingDemo)
        {
            return new DeferredShadingDemo();
        }
        else
        {
            return null;
        }
    }
}

