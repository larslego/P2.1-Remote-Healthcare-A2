﻿using Avans.TI.BLE;
using RemoteHealthcare.Software;
using RemoteHealthcare.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RemoteHealthcare.Hardware
{
    class HRBLE : BLE, IBLEDevice
    {

        private readonly PhysicalDevice device;
        private readonly string hrMonitorname;
        int errorcode = 1;
        private int connectionAttempts = 0;

        public event EventHandler<Byte[]> onHRData;

        public HRBLE(string hrMonitorname, PhysicalDevice device) : base()
        {
            this.device = device;
            this.hrMonitorname = hrMonitorname;
            // Waiting beforeinitializing
            Thread.Sleep(1000);

            Task task = device.Initialize(errorcode, connectionAttempts, hrMonitorname, "HeartRate", "HeartRateMeasurement", this, this);            
        }

        private async Task Initialize()
        {
            // Open the correct device
            while (this.errorcode == 1)
            {
                this.errorcode = this.errorcode = await OpenDevice(hrMonitorname);
                this.connectionAttempts += 1;
                if (this.errorcode == 0) continue;
            }
            // Try to set the required service to heartRate
            await SetService("HeartRate");

            // Set the method called on data receive to onHeartRate()
            SubscriptionValueChanged += OnDataReceived;
            await SubscribeToCharacteristic("HeartRateMeasurement");
        }

        /*
         * Event method that is called when the BLE receives data.
         * The method checks if the data is correct and sends it to the device class for decoding.
         */
        public void OnDataReceived(object sender, BLESubscriptionValueChangedEventArgs e)
        {
            if (ProtocolConverter.ConfirmPageData(e.Data))
            onHRData?.Invoke(this, e.Data);
        }
    }
}
