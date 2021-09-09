﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace RemoteHealthcare.Software
{
    abstract class Device {

    public abstract event EventHandler<double> OnSpeed;
    public abstract event EventHandler<int> OnRPM;
    public abstract event EventHandler<int> OnHeartrate;
    public abstract event EventHandler<double> OnDistance;
    public abstract event EventHandler<double> OnElapsedTime;
    public abstract event EventHandler<int> OnTotalPower;
    public abstract event EventHandler<int> OnCurrentPower;

    public DateTime StartTime { get; set; }

        /// <summary>
        /// Event that is called by DataGUI when a user enters a resistance value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
        public virtual void OnResistanceCall(Object sender, int data)
        {
            System.Diagnostics.Debug.WriteLine($"Resistance of the bike set to {data}");
        }
    }
}
