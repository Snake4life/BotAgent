using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;


using System.Collections;
using System.ComponentModel;
using System.Collections.ObjectModel;


namespace BotGUIClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    
    public partial class MainWindow : Window
    {

        public static DependencyProperty StatusTextProperty =
    DependencyProperty.Register("StatusText", typeof(string), typeof(MainWindow));
        public string StatusText
        {
            get { return (string)GetValue(StatusTextProperty); }
            set { SetValue(StatusTextProperty, value); }
        }

        // info about service ip address
        public static DependencyProperty ServiceIPAddressProperty =
DependencyProperty.Register("ServiceIPAddress", typeof(string), typeof(MainWindow));
        public string ServiceIPAddress
        {
            get { return (string)GetValue(ServiceIPAddressProperty); }
            set { SetValue(ServiceIPAddressProperty, value); }
        }


        
        // public ObservableCollection<BotItem> BotItemsList;


        public MainWindow()
        {
            InitializeComponent();
        }


        private void BackgroundWorkerProgressChanged(object Sender, ProgressChangedEventArgs E)
        {

            ObservableCollection<BotItem> BotItemsList = (ObservableCollection<BotItem>)((CollectionViewSource)(FindResource("BotsStatisticSource"))).Source;

            object [] data = (object[])E.UserState;
            string bot = (string) data[0];
            string[] rowsData = (string[]) data[1];
            // ObservableCollection<BotItem> BotItemsList = new  ObservableCollection<BotItem> ();

            foreach (string rowData in rowsData)
            {
                string[] colData = Regex.Split(rowData, "\t");



                if (colData.Length > 1)
                {

                    BotItemsList.Add(new BotItem
                    {
                        BotName = bot,
                        Key = colData[1],
                        UserName = colData[3],
                        Password = colData[4],
                        XPBoost = Int32.Parse(colData[5]),
                        Game = colData[6],
                        Spell1 = colData[7],
                        Spell2 = colData[8],
                        Summoner = colData[9],
                        Lvl = Int32.Parse(colData[10]),
                        TotalIP = Int32.Parse(colData[11]),
                        TotalRP = Int32.Parse(colData[12]),
                        Status = colData[13],
                        StatusBar = colData[14],
                        EventDT = colData[15]
                    }
                        );
                }
            }

            

            // StatusBarText.Text = "Connecting to service";
            // StatusText = "Connecting to service";



        }


        private void BackgroundWorkerDoWork(object Sender, DoWorkEventArgs E)
        {

            BackgroundWorker Worker = Sender as BackgroundWorker;


            ServiceReference1.ServiceClassClient client;

            try
            {
                client = new BotGUIClient.ServiceReference1.ServiceClassClient("NetTcpBinding_IServiceClass");


                


                string[] botsList = client.getBotsLlist();

                int currentBot = 0;

                foreach (string bot in botsList)
                {
                    // StatusText = String.Format("Get info about bot with name {0}", bot);
                    string botRowsData = client.getCurrentStatByBotName(bot);

                    string[] rowsData = Regex.Split(botRowsData, "\n");

                    Worker.ReportProgress(currentBot, new Object[2]{bot,rowsData});

                    currentBot++;
                }

                // get datagrid source from xaml
                // CollectionViewSource BotsStatisticSource = (CollectionViewSource)(FindResource("BotsStatisticSource"));
                //                BotItemsListSource.Source = BotItemsList;

                // StatusText = "Finish";

            }
            catch (Exception eInfo)
            {
                // StatusText = "Error " + eInfo.Message;
            }


            

            // Debug.WriteLine("BackgroundWorkerDoWork");


        }


        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<BotItem> BotItemsList = new ObservableCollection<BotItem>();
            CollectionViewSource BotItemsListSource = (CollectionViewSource)FindResource("BotsStatisticSource");
            BotItemsListSource.Source = BotItemsList;
            //BotItemsList = new ObservableCollection<BotItem>();

            
            BackgroundWorker Worker = new BackgroundWorker();
            Worker.WorkerReportsProgress = true;
            Worker.ProgressChanged += BackgroundWorkerProgressChanged;
            Worker.DoWork += BackgroundWorkerDoWork;

            StatusText = "Get bots list";

            if (Worker.IsBusy != true)
            {
            //    Worker.RunWorkerAsync(BotItemsList);
                Worker.RunWorkerAsync();
            }


            // get info from server about bots
            
            

            /*
            string ipAddress = serviceIPAddress.Text;
            ipAddress = ipAddress.Trim();
             */


            
            }
            

        }
    
}
