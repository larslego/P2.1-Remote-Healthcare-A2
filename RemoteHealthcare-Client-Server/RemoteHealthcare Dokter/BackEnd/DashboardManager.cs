﻿using Newtonsoft.Json.Linq;
using RemoteHealthcare_Client;
using RemoteHealthcare_Dokter.ViewModels;
using RemoteHealthcare_Shared.DataStructs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare_Dokter.BackEnd
{
    class DashboardManager : DataManager
    {
        public event EventHandler<List<SharedPatient>> OnPatientUpdated;

        public override void ReceivedData(JObject data)
        {
            Console.WriteLine($"Data received in the dashboard manager {data}");

            JToken value;

            bool correctCommand = data.TryGetValue("command", StringComparison.InvariantCulture, out value);

            // Return if the parsing of command was not succesfull
            if (!correctCommand) return;
           
            switch (value.ToString())
            {
                case "getactivepatients":
                    ParseIncomingPatients(data);
                    break;

            }
        }

        public void RequestActiveClients()
        {
            // Command to request all the logged in clients, see dataprotocol
            object o = new
            {
                command = "getactivepatients",
            };

            SendToManagers(JObject.FromObject(o));
        }

        private void ParseIncomingPatients(JObject data)
        {
            Trace.WriteLine($"DATA: {data}");
        }

        public void SendAbort(int id)
        {

        }

        public void SendResistance(int id, int res)
        {

        }

        public void BroadcastMessage(string message)
        {

        }

        public void StartSession(int id)
        {

        }

        public void SubToPatient(int id)
        {
            
        }

        private void WrapIncomingClients(JObject data)
        {

        }

        private void WrapIncomingErgoData(JObject data)
        {

        }

    }
}
