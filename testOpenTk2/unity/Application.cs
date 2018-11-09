using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.ES30;

namespace UnityEngine
{
    class Application
    {
        MainWindow m_mainWindow = null;
        GoManager m_goMgr = null;
        string m_titlePrefix = null;

        public void _Init(MainWindow mainWindow)
        {
            m_mainWindow = mainWindow;
            m_goMgr = new GoManager();
            m_titlePrefix = "dreamstatecoding" + ": OpenGL Version: " + GL.GetString(StringName.Version);
        }

        public GoManager _GetGoManager()
        {
            return m_goMgr;
        }

        public void OnUpdateFrame(FrameEventArgs e)
        {
            HandleKeyboard();
        }

        public void OnRenderFrame(FrameEventArgs e)
        {
            m_mainWindow.Title = m_titlePrefix + " Vsync " + m_mainWindow.VSync + " FPS " + (1 / e.Time).ToString("f0");

            GL.Finish();
            
            m_mainWindow.SwapBuffers();
        }

        void HandleKeyboard()
        {
            var keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Key.Escape))
            {
                m_mainWindow.Exit();
            }

            MouseState mouseState = Mouse.GetState();
            if (mouseState.IsButtonDown(MouseButton.Left))
            {
                //Console.WriteLine("mouse left btn down");
            }
        }
    }
}

