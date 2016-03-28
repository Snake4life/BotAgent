using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Configuration;
using System.Text.RegularExpressions;


using System.Windows.Automation;
using log4net;


[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace BotAgent
{

    // 
    /// <summary>
    /// Program to collect information from FulcrumBot.exe and send to service
    /// we need to run this program with admin privilegies
    /// </summary>

    class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string programVersion = "1.0";

        private static Process currentProcess;
        private static Process[] processes;
        private static ServiceReference1.ServiceClassClient client;


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


            // here is infinite loop

            while (true)
            {
                Dictionary<string, string> collectedInfo = new Dictionary<string, string>();


                try
                {

                    // before get statistic check if proccess is exist
                    isProcessFound = refreshProcessInfo(ConfigurationManager.AppSettings["botName"]);

                    if (!isProcessFound)
                    {
                        log.Info(String.Format("main: process isn't found, wating"));
                        Thread.Sleep(Int32.Parse(ConfigurationManager.AppSettings["sleepIntervalBetweenSendStat"]));

                    }
                    else
                    {
                        // get info from WPF application
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

                        // get status bar info
                        var statusBar = mainWindow.FindFirst(TreeScope.Children,
                               new PropertyCondition(AutomationElement.ControlTypeProperty,
                            System.Windows.Automation.ControlType.StatusBar));

                        AutomationElement statusBarCurrentStatus = statusBar.FindFirst(
                            TreeScope.Children,
                                new PropertyCondition(AutomationElement.ControlTypeProperty,
                                    System.Windows.Automation.ControlType.Text));

                        string currentStatusBarValue = statusBarCurrentStatus.Current.Name.ToString();

                        collectedInfo.Add("currentStatusBarValue", currentStatusBarValue);

                        log.Warn(string.Format("data: status bar value: {0}", currentStatusBarValue));


                        // get datagrid info
                        var dataGrid = mainWindow.FindFirst(TreeScope.Children,
                            new PropertyCondition(AutomationElement.ControlTypeProperty,
                                System.Windows.Automation.ControlType.DataGrid));

                        GridPattern patternForGrid = GetGridPattern(dataGrid);

                        // int rowCount = patternForGrid.Current.RowCount;

                        int currentRow = 0;

                        ArrayList rowsData = new ArrayList();

                        // go trough all rows and collect information
                        foreach (AutomationElement row in dataGrid.FindAll(TreeScope.Descendants,
                                        new PropertyCondition(AutomationElement.ClassNameProperty, "DataGridRow"))
                                    )
                        {
                            // now go through all cells
                            int currentField = 0;

                            string dataLine = "";

                            foreach (AutomationElement child in row.FindAll(
                                          TreeScope.Descendants,
                                          new PropertyCondition(AutomationElement.ClassNameProperty, "DataGridCell"))
                                          )
                            {
                                
                                currentField++;




                                // dataLine += child.Current.Name.ToString();

                                // log.Info(child.Current.Name.ToString());

                                ValuePattern pattern = child.GetCurrentPattern(ValuePattern.Pattern) as ValuePattern;

                                dataLine += pattern.Current.Value + "\t";
                                

                                // 0 - run, 1 - key, 2 - region, 3 - username, 4 - password, 5 - XP Boost, 6 - Game, 7 - Spell1, 8 - Spell2 , 9- Summoner, 10 - Lvl, 11 - Total IP, 12 - Total RP, 13 - status
                                // data[currentField] = child.Current.Name.ToString();

                                if (currentField == 14)
                                {
                                    // if we found last columnn
                                    collectedInfo.Add(currentRow.ToString(), dataLine);
                                    rowsData.Add(dataLine);
                                }

                                
                            }

                            currentRow++;
                            // log.Info(dataLine);

                            
                        }


                        // format is like
                        /*
                            bot1, "row1data1\t row1data2 \t \n row2data1 \t row2data2 and so on
                         */
                        string currentStringWithData = "";

                        


                        
                        foreach (string row in rowsData)
                        {
                            DateTime myDateTime = DateTime.Now;
                            string sqlFormattedDate = myDateTime.ToString("yyyy-MM-dd HH:mm:ss");

                            

                            string rowForSending = Regex.Replace(row, @"\t\s*$", "");

                            currentStringWithData += rowForSending;
                            /*
                            if (firstTime)
                            {
                                currentStringWithData += currentStatusBarValue;
                                firstTime = false;
                            }
                            else
                            {
                                currentStringWithData += "\t" + currentStatusBarValue;
                            
                            }
                             */
                            currentStringWithData += "\t" + currentStatusBarValue;

                            currentStringWithData += "\t" + sqlFormattedDate;
                            currentStringWithData += "\n";

                        }

                        log.Info(currentStringWithData);

                        // log.Info(currentStringWithData);

                        // send data to service 
                        try
                        {
                            // we need instantenate client here
                            client = new BotAgent.ServiceReference1.ServiceClassClient("NetTcpBinding_IServiceClass"); 
                            client.saveStatistic(ConfigurationManager.AppSettings["botAgentName"], currentStringWithData);
                            // log.Info(String.Format("data info: {0}",ToDebugString(collectedInfo)));
                        }
                        catch (Exception e)
                        {
                            log.Error(String.Format("Error  {0}, stacktrace: {1}", e.Message, e.StackTrace));
                        }


                    }

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
    

        /// <summary>
        /// get grid pattern to work with
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>

        public static GridPattern GetGridPattern(AutomationElement element)
        {
            object currentPattern;
            if (!element.TryGetCurrentPattern(GridPattern.Pattern, out currentPattern))
            {
                try
                {
                    throw new Exception(string.Format("Element with AutomationId '{0}' and Name '{1}' does not support the GridPattern.",
                     element.Current.AutomationId, element.Current.Name));
                }
                catch (Exception e)
                {
                    log.Error(string.Format("{0} {1}", e.Message, e.StackTrace));
                    Console.ReadLine();
                    Environment.Exit(0);

                }
            }
            return currentPattern as GridPattern;
        }

        /// <summary>
        /// Convert dictinary to string
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        /// 

        
        public static string ToDebugString<TKey, TValue>(IDictionary<TKey, TValue> dictionary)
        {
            return "{" + string.Join(",", dictionary.Select(kv => kv.Key.ToString() + "=" + kv.Value.ToString()).ToArray()) + "}";
        }
         
    }

}
