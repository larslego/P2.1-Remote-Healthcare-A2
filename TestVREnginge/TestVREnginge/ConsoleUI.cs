﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestVREngine
{
    class ConsoleUI
    {
        private static TunnelHandler handler = new TunnelHandler();
        private static BasicScene scene = new BasicScene(handler);

        static void Main(string[] args)
        {
            // Getting the data for all the available clients
            List<ClientData> Clients =  handler.GetAvailableClients();

            //Lists all the avalable clients and adds the corresponding number in the list
            Console.WriteLine("Avaliable clients:");
            for (int i = 0; i < Clients.Count; i++)
            {
                ClientData C = Clients[i];
                Console.WriteLine("{0}: {1}",i + 1, C.ToString());
            }

            //Ask for userinput
            int Userinput = 0;
            while (Userinput < 1 || Userinput > Clients.Count)
            {
                Console.WriteLine("\nGive a selection number for a tunnel: ");
                Userinput = int.Parse(Console.ReadLine());
            }

            //Call method which sets up a connection
            string ID = handler.SetUpConnection(Clients[Userinput - 1].Adress).Item2;
            Console.WriteLine("ID that was returend: "  +  ID);

            //Loop which calls a method from the BasicScene class and starts the corresponding activity from teh list
            for (int i = 0; i < 7; i++)
            {
                Console.WriteLine(scene.ExecuteNext(i));
                Console.ReadKey();
            }

            Console.WriteLine("All methods have been executed...");
            
        }
    }
}
