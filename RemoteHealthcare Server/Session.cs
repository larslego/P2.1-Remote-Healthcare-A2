﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteHealthcare_Server
{
    public class Session
    {
        /// <summary>
        /// List of HRMeasurements in this current session
        /// </summary>
        public List<HRMeasurement> HRMeasurements
        {
            get => default;
            set
            {
            }
        }

        /// <summary>
        /// List of BikeMeasurements in this current session
        /// </summary>
        public List<BikeMeasurement> BikeMeasurements
        {
            get => default;
            set
            {
            }
        }

        public int SessionID
        {
            get => default;
            set
            {
            }
        }

        public HRMeasurement HRMeasurement
        {
            get => default;
            set
            {
            }
        }

        public BikeMeasurement BikeMeasurement
        {
            get => default;
            set
            {
            }
        }

        public Patient Patient
        {
            get => default;
            set
            {
            }
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
            get => default;
            set
            {
            }
        }
    }
}