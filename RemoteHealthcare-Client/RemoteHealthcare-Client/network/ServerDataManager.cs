﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RemoteHealthcare_Client.TCP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace RemoteHealthcare_Client
{
    public class ServerDataManager : IDataManager
    {
       
        private TCPClientHandler TCPClientHandler { get; set; }

        public IDataManager DeviceDataManager { get; set; }

        public IDataManager VRDataManager { get; set; }

        public ServerDataManager(string ip, int port)
        {
            this.TCPClientHandler = new TCPClientHandler(ip, port);
            this.TCPClientHandler.SetRunning(true);

            this.TCPClientHandler.OnMessageReceived += OnMessageReceived;
        }

        private void OnMessageReceived(object sender, string message)
        {
            // REMOVE
            Trace.WriteLine("---------------------" + message);
            //Reading input
            JObject jobject = JsonConvert.DeserializeObject(message) as JObject;


            HandleIncoming(jobject);

        }

        private void HandleIncoming(JObject jobject)
        {
            // command value always gives the action 
            JToken value;

            bool correctCommand = jobject.TryGetValue("command", StringComparison.InvariantCulture, out value);

            if (!correctCommand)
            {
                // todo, log error and handle correctly
                return;
            }

            // Looking at the command and switching what behaviour is required
            switch(value.ToString())
            {

                case "message":
                    HandleMessageCommand(jobject);
                    break;
                case "abort":
                    this.VRDataManager?.ReceivedData(jobject); //sending the data to the vr manager
                    break;
                case "setresist": 
                    this.DeviceDataManager?.ReceivedData(jobject);  //sending the data to the device manager
                    break;

                default:
                    // TODO HANDLE NOT SUPPORTER
                    break;


            }
        }

        private void HandleMessageCommand(JObject jobject)
        {
            // all message object are required to have flag attribute.
            int flag = (int)jobject.GetValue("flag");

            switch(flag)
            {
                case 0:
                case 1:
                    // TODO flags needed for login, net yet needed
                    break;
                case 2:
                    this.VRDataManager?.ReceivedData(jobject);
                    // Sending the data to the vrmanager, since flag 2 needs to be show in vr
                    break;
                case 3:
                default:
                    Trace.WriteLine($"Error received from server{jobject.GetValue("data")}");
                    break;

            }
        }

        public void ReceivedData(JObject data)
        {

            Trace.WriteLine($"received data from server: {data}");
            this.TCPClientHandler.WriteMessage(data.ToString());
        }
    }
}