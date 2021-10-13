﻿using Newtonsoft.Json.Linq;
using RemoteHealthcare_Client;
using RemoteHealthcare_Dokter.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare_Dokter.BackEnd
{
    class LoginManager : DataManager
    {
        public event EventHandler<bool> OnLoginResponseReceived;

        public LoginManager()
        {

        }

        public override void ReceivedData(JObject data)
        {
            Trace.WriteLine($"received data from server: {data}");
            HandleIncoming(data);
        }

        private void HandleIncoming(JObject data)
        {
            JToken value;

            bool correctCommand = data.TryGetValue("command", StringComparison.InvariantCulture, out value);

            if (!correctCommand)
            {
                // todo, log error and handle correctly
                return;
            }

            // Looking at the command and switching what behaviour is required
            if (value.ToString() == "message")
                HandleLoginResponse(data);
        }

        public void SendLogin(string username, string password)
        {
            object o = new
            {
                command = "login",
                data = new
                {
                    us = username,
                    pass = password,
                    flag = 1
                }
            };

            SendToManagers(JObject.FromObject(o));
        }

        private void HandleLoginResponse(JObject data)
        {
            this.OnLoginResponseReceived?.Invoke(this, data.GetValue("data").ToString().Contains("succesfull connect"));
        }
    }
}