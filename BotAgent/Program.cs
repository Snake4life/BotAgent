using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Configuration;


using System.Windows.Automation;

using log4net;


[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace BotAgent
{
    

    class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string programVersion = "1.0";

        private static Process currentProcess;
        private static Process[] processes;


        // return true if process is found and false otherwise
        
        static bool refreshProcessInfo(string processName)
        {
            Process[] tmpProcesses = Process.GetProcessesByName(processName);
            
            if (tmpProcesses.Length > 0)
            {
                if (currentProcess == null)
                {
                    // first run
                    log.Debug("refreshProcessInfo: current process is null, try to init it");
                    currentProcess = tmpProcesses[0];
                    return true;
                }
                else if (
                    tmpProcesses[0].Id == currentProcess.Id
                    )
                {
                    // process hasn't changed
                    log.Debug("refreshProcessInfo: process hasn't changed");
                    tmpProcesses[0].Dispose();
                    return true;

                }
                else if (
                    tmpProcesses[0].Id!= currentProcess.Id)
                {
                    log.Debug("refreshProcessInfo: we have new process, change data");
                    currentProcess.Dispose();
                    currentProcess = tmpProcesses[0];

                    return true;
                }
            }

            log.Debug("refreshProcessInfo: process is not found");
            return false;
            
        
        }


        static void Main(string[] args)
        {
            
            // init part
            bool isProcessFound = false;

            do
            {
                // processes = Process.GetProcessesByName(ConfigurationManager.AppSettings["botName"]);
            
                isProcessFound = refreshProcessInfo(ConfigurationManager.AppSettings["botName"]);
                if (!isProcessFound)
                {
                    // we haven't found any proccess with desired name, so we need to sleep
                    log.Warn(String.Format("main: process with name {0} is not found, waiting {1} ms", ConfigurationManager.AppSettings["botName"],
                        ConfigurationManager.AppSettings["sleepIntervaIfBotDidntFound"])
                        );

                    Thread.Sleep(Int32.Parse(ConfigurationManager.AppSettings["sleepIntervaIfBotDidntFound"]));

                }


            } while (!isProcessFound);

            // even if the process is found wee need to wait some time

            while (currentProcess.MainWindowHandle.Equals(IntPtr.Zero))
            {
                Thread.Sleep(100);
                currentProcess.Refresh();
            }



            var mainWindow = AutomationElement.FromHandle(currentProcess.MainWindowHandle);

            log.Info(String.Format("main: found {0} process, pid={1}",
                ConfigurationManager.AppSettings["botName"],
                currentProcess.Id
                )
                );


            // here is infinite loop

            while (true)
            {
                try
                {

                    // before get statistic check if proccess is exist
                    isProcessFound = refreshProcessInfo(ConfigurationManager.AppSettings["botName"]);

                    if (!isProcessFound)
                    {
                        log.Info(String.Format("main: process isn't found, wating"));
                        Thread.Sleep(Int32.Parse(ConfigurationManager.AppSettings["sleepIntervalBetweenSendStat"]));

                    }

                    // get info from WPF application






                    log.Info(String.Format("main: sleeping {0} ms before send statistic", ConfigurationManager.AppSettings["sleepIntervalBetweenSendStat"]));
                    Thread.Sleep(Int32.Parse(ConfigurationManager.AppSettings["sleepIntervalBetweenSendStat"]));
                }
                catch (Exception e)
                {
                    log.Error(String.Format("main: error {0}, stacktrace: {1}", e.Message, e.StackTrace));
                }
            }



//            Console.ReadLine();
        }

    }
}
