using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        
        
        public ObservableCollection<BotItem> BotItemsList;


        public MainWindow()
        {
            InitializeComponent();
        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {


            BotItemsList = new ObservableCollection<BotItem>();

            // get info from server about bots
            
            StatusBarText.Text = "Connecting to service";

            /*
            string ipAddress = serviceIPAddress.Text;
            ipAddress = ipAddress.Trim();
             */

            ServiceReference1.ServiceClassClient client;

            try
            {
                client = new BotGUIClient.ServiceReference1.ServiceClassClient("NetTcpBinding_IServiceClass");


            StatusBarText.Text = "Get bots list";
            string[] botsList = client.getBotsLlist();

             foreach (string bot in botsList)
             {
                 StatusBarText.Text = String.Format("Get info about bot with name {0}",bot);
                 string botRowsData = client.getCurrentStatByBotName(bot);

                string []rowsData = Regex.Split( botRowsData, "\n");

                foreach (string rowData in rowsData)
                 {
                     string []colData = Regex.Split(rowData,"\t");

                     if (colData.Length > 1)
                     {

                         BotItemsList.Add(new BotItem
                                 {
                                     BotName = bot,
                                     Key = colData[1],
                                     UserName = colData[3],
                                     Password = colData[4],
                                     XPBoost = colData[5],
                                     Game = colData[6],
                                     Spell1 = colData[7],
                                     Spell2 = colData[8],
                                     Summoner = colData[9],
                                     Lvl = colData[10],
                                     TotalIP = colData[11],
                                     TotalRP = colData[12],
                                     Status = colData[13], 
                                     StatusBar = colData[14],
                                     EventDT = colData[15]
                                 }
                             );
                     }
                 }
            

                }

                // get datagrid source from xaml
                CollectionViewSource BotsStatisticSource = (CollectionViewSource)(FindResource("BotsStatisticSource"));
                BotsStatisticSource.Source = BotItemsList;

                StatusBarText.Text = "Finish";

            }
            catch (Exception eInfo)
            {
                StatusBarText.Text = "Error " + eInfo.Message;
            }

            
            }
            

        }
    
}
