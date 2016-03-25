using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceReference1.ServiceClassClient client = new BotConsoleClient.ServiceReference1.ServiceClassClient("NetTcpBinding_IServiceClass");
            Dictionary<string,string> currentStatus = client.getCurrentStat();

            foreach (KeyValuePair<string, string> entry in currentStatus)
            {
                Console.WriteLine(entry.Key + "=>" + entry.Value);
            }
        }
    }
}
