﻿using System;
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
            // string resultRow = client.getCurrentStatByBotName("BotAgent_1234");

            string[] botsList = client.getBotsLlist();

            foreach (string currentBot in botsList)
            {
                string resultRow = client.getCurrentStatByBotName(currentBot);
                Console.WriteLine(resultRow);
            }

            

        }
    }
}
