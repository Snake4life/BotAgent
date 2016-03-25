using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace WCFService
{
    [ServiceContract]
    public interface IServiceClass
    {
        [OperationContract]
        void saveStatistic (string botName,string rows);
        [OperationContract]
        string getCurrentStatByBotName(string botName);
    }


    public static class DataContainer
    {
        public static Dictionary<string, string> data = new Dictionary<string, string>();
    }
    

    public class ServiceClass : IServiceClass
    {
     
        

        /// <summary>
        /// Save statistict about bot
        /// </summary>
        /// <param name="botName"></param>
        /// <param name="rows"></param>
        void IServiceClass.saveStatistic(string botName, string rows)
        {

            Console.WriteLine("Save statistic " + DataContainer.data.Count);

            botName = botName.ToLower();
            if (DataContainer.data.ContainsKey(botName))
            {
                DataContainer.data[botName] = rows;
                Console.WriteLine("Update info");
            }
            else
            {
                Console.WriteLine("Add info");
                DataContainer.data.Add(botName, rows);
            }
        }


        string IServiceClass.getCurrentStatByBotName(string botName)
        {
            botName = botName.ToLower();

            Console.WriteLine(DataContainer.data.Count);
            Console.WriteLine(botName);

            if (DataContainer.data.ContainsKey(botName))
            {
                Console.WriteLine("Get data");
                return DataContainer.data[botName];
            }
            else
            {
                Console.WriteLine("Null");
                return "";
            }
            // return data[botName];
            
        }

 

    }
}
