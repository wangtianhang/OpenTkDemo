using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace testOpenTk2
{
    static class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            AllocConsole();
            //ShowSecondWindow();
            new MainWindow().Run(60);
        }

        static void ShowSecondWindow()
        {
            Thread thread = new Thread(SecondWindowThread);
            thread.Start();    
        }

        static void SecondWindowThread()
        {
            new MainWindow().Run();
        }
    }
}
