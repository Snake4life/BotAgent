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



        static void Main(string[] args)
        {


            Process[] process = Process.GetProcessesByName(ConfigurationManager.AppSettings["botName"]);

            while (process.Length == 0) 
            {
                
                // we haven't found any proccess with desired name, so we need to sleep
                log.Warn(String.Format("Process with name {0} is not found, waiting {1} ms", ConfigurationManager.AppSettings["botName"],
                    ConfigurationManager.AppSettings["sleepIntervaIfBotDidntFound"])
                    );

                Thread.Sleep(Int32.Parse(ConfigurationManager.AppSettings["sleepIntervaIfBotDidntFound"]));
                
                process = Process.GetProcessesByName(ConfigurationManager.AppSettings["botName"]);

            } 
            // there can be only one proccess of FulcrumBot

            var mainWindow = AutomationElement.FromHandle(process[0].MainWindowHandle);

            log.Info(String.Format("Found {0} process, pid={1}",
                ConfigurationManager.AppSettings["botName"],
                process[0].Id
                )
                );

//            Console.ReadLine();
        }
    }
}
