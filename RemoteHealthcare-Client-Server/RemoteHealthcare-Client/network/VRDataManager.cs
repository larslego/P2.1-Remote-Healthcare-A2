﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RemoteHealthcare.ClientVREngine.Util;
using RemoteHealthcare_Client.ClientVREngine.Scene;
using RemoteHealthcare_Client.ClientVREngine.Tunnel;
using System.Diagnostics;
using System.Windows;
using System.Threading;

namespace RemoteHealthcare_Client
{
    public class VRDataManager : DataManager
    {
        public GeneralScene Scene { get; set; }
        private bool isConnected;

        public TunnelHandler VRTunnelHandler { get; set; }
        
        public VRDataManager()
        {
            this.VRTunnelHandler = new TunnelHandler();
            //this.Scene = new SimpleScene(this.VRTunnelHandler);
        }

        public void Start(string vrServerID)
        {
            Scene.Handler = VRTunnelHandler;

            this.isConnected = this.VRTunnelHandler.SetUpConnection(vrServerID);

            // Starting a new thread on building the scene, so the UI has no wait
            new Thread(() => {

                this.Scene.InitScene();

                this.Scene.LoadScene();

            }).Start();
            
        }

        public override void ReceivedData(JObject data)
        {
            // The data the VR engine will receive is the ergodata from the ergodevice + messagedata, see dataprotocol

            if (!isConnected)
            {
                Debug.WriteLine("VRManager.ReceiveData: Not receiving data because we have no connection");
                return;
            }
            // command value always gives the action 
            JToken value;

            bool correctCommand = data.TryGetValue("command", StringComparison.InvariantCulture, out value);

            if (!correctCommand)
            {
                Trace.WriteLine("No valid JSON was received to VRDataManager");
                return;
            }

            // Looking at the command and switching what behaviour is required
            switch (value.ToString())
            {

                case "message":
                    Scene.WriteTextToPanel(Scene.HandelTextMessages(8,25,data));
                    break;
                case "ergodata":
                    Trace.WriteLine($"Ergo data received by vr engine{data.GetValue("data")}");
                    Scene.WriteDataToPanel(data);
                    break;
                default:
                    // TODO HANDLE NOT SUPPORTER
                    Trace.WriteLine("Error in VRDataManager, data received does not meet spec");
                    break;
            }
        }
    }
}