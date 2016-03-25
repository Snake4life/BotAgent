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
        void saveStatistic (int botId,ArrayList rows);
        [OperationContract] 
        Dictionary<string,ArrayList> getCurrentStat();
    }

    public class ServiceClass : IServiceClass
    {
        public

        void IServiceClass.saveStatistic()
        {
            return "Hello world!";
        }

        int IServiceClass.MultiplyNumbers(int firstvalue, int secondvalue)
        {
            return firstvalue * secondvalue;
        }

    }
}
