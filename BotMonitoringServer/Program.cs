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
            Uri tcpa = new Uri("net.tcp://0.0.0.0:8000/TcpBinding");


            var sh = new ServiceHost(typeof(ServiceClass), tcpa);

            
            NetTcpBinding tcpb = new NetTcpBinding();
            tcpb.Security.Mode = SecurityMode.None;
            tcpb.Security.Transport.ClientCredentialType = TcpClientCredentialType.None;
            tcpb.Security.Message.ClientCredentialType = MessageCredentialType.None;
            
            

            
            
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
