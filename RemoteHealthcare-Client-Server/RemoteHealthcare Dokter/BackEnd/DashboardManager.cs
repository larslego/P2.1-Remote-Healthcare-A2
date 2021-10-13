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

        public DashboardManager()
        {
            // Oncreation the server wil ask the server for all active clients
            RequestActiveClients();
        }

        public override void ReceivedData(JObject data)
        {
            JToken value;

            bool correctCommand = data.TryGetValue("command", StringComparison.InvariantCulture, out value);

            // Return if the parsing of command was not succesfull
            if (!correctCommand) return;
           
            switch (value.ToString())
            {
                case "getactivepatients":
                    // Setting the command to the command to ask for detailed data and sending to server
                    data["command"] = "getdetailpatient";
                    SendToManagers(data);
                    break;
                case "detaildata":
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
            JArray patientIDs = data.GetValue("data") as JArray;

            List<SharedPatient> patients = new List<SharedPatient>();
            foreach(JObject jo in patientIDs)
            {
                patients.Add(jo.ToObject<SharedPatient>());
            }

            this.OnPatientUpdated.Invoke(this, patients);
        }

        public void BroadcastMessage(string message)
        {
            object o = new
            {
                command = "message",
                data = new
                {
                    message = message,
                },
                flag = "2"
            };

            this.SendToManagers(JObject.FromObject(o));
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
