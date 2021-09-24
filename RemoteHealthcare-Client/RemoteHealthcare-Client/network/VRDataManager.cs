﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RemoteHealthcare.ClientVREngine.Util;
using RemoteHealthcare_Client.ClientVREngine.Scene;
using RemoteHealthcare_Client.ClientVREngine.Tunnel;

namespace RemoteHealthcare_Client
{
    public class VRDataManager : DataManager
    {
        private readonly SimpleScene simpleScene;

        public TunnelHandler VRTunnelHandler { get; set; }

        public VRDataManager()
        {
            VRTunnelHandler = new TunnelHandler();
            this.simpleScene = new SimpleScene(this.VRTunnelHandler);
        }

        public void Start(string vrServerID)
        {
            this.VRTunnelHandler.SetUpConnection(vrServerID);

            this.simpleScene.InitScene();

            this.simpleScene.LoadScene();
        }

        public void HandleIncoming(JObject data)
        {
            string message = data.GetValue("data").ToString();
            this.VRTunnelHandler.SendToTunnel(JSONCommandHelper.WrapPanelText(simpleScene.getOrDefaultPanelUuid(), 
                message, new double[] {5, 5, 0}, 10, "arial"));

        }

        public void PrepareVRData(JObject data)
        {
            throw new NotImplementedException();
        }

        public override void ReceivedData(JObject data)
        {
            throw new NotImplementedException();
        }
    }
}