using System;
using System.Collections.Generic;
//using System.Linq;
using System.Windows.Forms;
//using Win32;
//using Microsoft.Win32;
using System.Diagnostics;
//using Microsoft.DirectX.AudioVideoPlayback;
namespace SmartFplayer
{
    static class Program
    {
       
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        ///
        [STAThread]
        static void Main(string[] Args)
        {
            string file = "";
            if (Args.Length != 0)
                file = Args[0];

            //System.Diagnostics.Process.Start(file);
            IsRunning();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new SFplayer(file));
            
           
           


        }
        static void IsRunning()
        {
           // bool bRet = false;
           // IntPtr hFound;
            // Get the current process 
            Process currentProcess = Process.GetCurrentProcess();
            // Check with other process already running 
            foreach (Process p in Process.GetProcesses())
            {
                if (p.Id != currentProcess.Id)
                {
                    if (p.ProcessName.Equals(currentProcess.ProcessName) == true)
                    {
                        p.Kill();
                    }
                }
            }
        } 
            }
           
                
        
        
        }
           
