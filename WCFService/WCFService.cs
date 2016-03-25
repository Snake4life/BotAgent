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
        Dictionary<string,string> getCurrentStat();
    }

    public class ServiceClass : IServiceClass
    {
        public Dictionary<string, string> data = new Dictionary<string, string>();

        /// <summary>
        /// Save statistict about bot
        /// </summary>
        /// <param name="botName"></param>
        /// <param name="rows"></param>
        void IServiceClass.saveStatistic(string botName, string rows)
        {
            botName = botName.ToLower();
            if (data.ContainsKey(botName))
            {
                data[botName] = rows;
            }
            else
            {
                data.Add(botName, rows);
            }
        }


        Dictionary<string, string> IServiceClass.getCurrentStat()
        {
            return data;
        }

 

    }
}
