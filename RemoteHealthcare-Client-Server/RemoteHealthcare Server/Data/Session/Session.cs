﻿using RemoteHealthcare_Server.Data.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteHealthcare_Server
{
    public class Session
    {
        public List<Doctor> Subscribers { get; set; }

        public Session(Patient patient)
        {
            Patient = patient;
            Subscribers = new List<Doctor>();
            this.StartTime = DateTime.Now;
            this.HRMeasurements = new List<HRMeasurement>();
            this.BikeMeasurements = new List<BikeMeasurement>();
        }




        /// <summary>
        /// List of HRMeasurements in this current session
        /// </summary>
        public List<HRMeasurement> HRMeasurements
        {
            get ;
            set;
        }

        /// <summary>
        /// List of BikeMeasurements in this current session
        /// </summary>
        public List<BikeMeasurement> BikeMeasurements
        {
            get ;
            set;
           
        }




        public int SessionID
        {
            get => default;
            set
            {
            }
        }

        public Patient Patient
        {
            get ;
            set;
            
        }

        public DateTime StartTime
        {
            get => default;
            set
            {
            }
        }

        public DateTime EndTime
        {
            get => DateTime.Now;
            set
            {
            }
        }
    }
}