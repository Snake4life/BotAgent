using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

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

            Process[] process = Process.GetProcessesByName("FulcrumBot");

            // there can be only one proccess of FulcrumBot

            var mainWindow = AutomationElement.FromHandle(process[0].MainWindowHandle);

//            Console.ReadLine();
        }
    }
}
