using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ServiceModel;
using System.ServiceModel.Description;
using WCFService;

namespace BotMonitoringServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri tcpa = new Uri("net.tcp://localhost:8000/TcpBinding");
            var sh = new ServiceHost(typeof(ServiceClass), tcpa);
            NetTcpBinding tcpb = new NetTcpBinding();
            ServiceMetadataBehavior mBehave = new ServiceMetadataBehavior();
            sh.Description.Behaviors.Add(mBehave);
            sh.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexTcpBinding(), "mex");
            sh.AddServiceEndpoint(typeof(IServiceClass), tcpb, tcpa);
            sh.Open();
            Console.ReadLine();
            sh.Close(); 
        }
    }
}
