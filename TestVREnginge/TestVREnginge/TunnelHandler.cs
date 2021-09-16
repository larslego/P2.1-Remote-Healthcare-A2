﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestVREnginge.TCP;

namespace TestVREnginge
{
    class TunnelHandler
    {
        private Dictionary<string, Action> SerialMap;
        private TCPClientHandler tcpHandler;

        public TunnelHandler()
        {
            this.SerialMap = new Dictionary<string, Action>();
            this.tcpHandler = new TCPClientHandler();

            this.tcpHandler.OnMessageReceived += onMessageReceived;

        }


        /// <summary>
        /// gives a list of all the available clients.
        /// </summary>
        /// <returns></returns>
        public List<ClientData> GetAvailableClients()
        {
            List<ClientData> clients = new List<ClientData>();

            //Writing for connection
            //TODO fixing hardcode json.
            string startingCode = "{\r\n\"id\" : \"session/list\"\r\n}";
            tcpHandler.WriteMessage(startingCode);

            //Reading for clients
            string jsonString = this.tcpHandler.ReadMessage();
            JObject jsonData = (JObject)JsonConvert.DeserializeObject(jsonString);

            //Adding it to the list
            JArray computers = (JArray)jsonData.GetValue("data");
            foreach (JObject objects in computers)
            {
                string id = objects.GetValue("id").ToString();
                JObject clientinfo = (JObject)objects.GetValue("clientinfo");
                string pcName = clientinfo.GetValue("host").ToString();
                string pcUser = clientinfo.GetValue("user").ToString();
                string pcGPU = clientinfo.GetValue("renderer").ToString();

                clients.Add(new ClientData(id, pcName, pcUser, pcGPU));
            }


            //Returning it.
            return clients;
        }


        /// <summary>
        /// setup up the connection and returns the id.
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public (bool, string) SetUpConnection(string connection)
        {
            //Sending tunneling request to vps
            //TODO fixing hardcode json.
            string requestingCode = "{\r\n\"id\" : \"tunnel/create\",\r\n\"data\" :\r\n	{\r\n\"session\" : \"" + connection + "\"\r\n}\r\n}";
            this.tcpHandler.WriteMessage(requestingCode);

            //Receiving ok or error
            string jsonString = this.tcpHandler.ReadMessage();
            JObject jsonData = (JObject)JsonConvert.DeserializeObject(jsonString);

           
            JObject jsonFile = (JObject)jsonData.GetValue("data");
            if (jsonFile.GetValue("status").ToString() != "ok")
            {
                return (false, null);
            }
            else { 
                string id = jsonFile.GetValue("id").ToString();
                this.tcpHandler.SetRunning(true);
                return (true, id);
            }
        }

        private int serialNumber = 0;

        //TODO discuss if it should be JOBject or else
        public void SendToTunnel(JObject message, Action action)
        {
            this.serialNumber += 1;

            string serial = this.serialNumber.ToString();

            message.Add("serial", message);
            this.SerialMap.Add(serial, action);

            SentToTunnel(message);
        }

        public void SentToTunnel(JObject message)
        {

        }


        //Example function for controlling the appliction
        public void exampleFunction(string json)
        {
            this.tcpHandler.WriteMessage(json);
        }

        private void onMessageReceived(object sender, string e)
        {
            Console.WriteLine(e);
        }

    }

    

}