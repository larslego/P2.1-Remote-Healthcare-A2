﻿using CommClass;
using Newtonsoft.Json;
using RemoteHealthcare_Server.Data.User;
using RemoteHealthcare_Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare_Server
{
    class JSONWriter
    {
        /// <summary>
        /// Writes wether an action was succesfull or not..
        /// </summary>
        /// <param name="succes"></param>
        /// <param name="sender"></param>
        public static void LoginWrite(bool succes, ISender sender)
        {
            //Selecting object
            object o;
            if (succes)
            {
                o = new
                {
                    command = "message",
                    data = "succesfull connect",
                    flag = 1
                };
            } else
            {
                o = new
                {
                    command = "message",
                    data = "failed connect",
                    flag = 1
            };
            }

            //Writing answer...
            sender.SendMessage(JsonConvert.SerializeObject(o));
        }

        /// <summary>
        /// Writes a message to the host that is selected...
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="sender"></param>
        public static void MessageWrite(string msg, ISender sender)
        {
            object o = new
            {
                command = "message",
                data = msg,
                flag = 2
            };

            sender.SendMessage(JsonConvert.SerializeObject(o));
        }

        /// <summary>
        /// Writes the resitance to a host that is selected
        /// </summary>
        /// <param name="resistance"></param>
        /// <param name="sender"></param>
        public static void ResistanceWrite(int resistance, ISender sender)
        {
            object o = new
            {
                command = "setresist",
                data = resistance
            };

            
            sender.SendMessage(JsonConvert.SerializeObject(o));
        }

        /// <summary>
        /// Writes an aborot to a host that is selected
        /// </summary>
        /// <param name="sender"></param>
        public static void AbortWrite(ISender sender)
        {
            object o = new
            {
                command = "abort",
                data = new { }
            };

            sender.SendMessage(JsonConvert.SerializeObject(o));
        }

        /// <summary>
        /// Writes all patietns to a selected host..
        /// </summary>
        /// <param name="AllPatients"></param>
        /// <param name="sender"></param>
        public static void AllPatientWrite(List<string> AllPatients, ISender sender)
        {
            object o = new
            {
                command = "getallpatients",
                data = AllPatients
            };

            sender.SendMessage(JsonConvert.SerializeObject(o));
        }


        /// <summary>
        /// Writes active patients to a slected host..
        /// </summary>
        /// <param name="AllPatients"></param>
        /// <param name="sender"></param>
        public static void ActivePatientWrite(List<string> AllPatients, ISender sender)
        {
            object o = new
            {
                command = "getactivepatients",
                data = AllPatients
            };

            sender.SendMessage(JsonConvert.SerializeObject(o));
        }


        /// <summary>
        /// Writes an 
        /// </summary>
        /// <param name="h"></param>
        /// <param name="s"></param>
        public static void DoctorSubWriter(Host h, Session s, string id, ISender sender)
        {
            //Getting lastest measurment...
            BikeMeasurement latestBikeMeasurent = s.BikeMeasurements.Last();
            HRMeasurement latestHeartMeasurent = s.HRMeasurements.Last();

            //Which is the latest....
            var LatestMeasurement = latestHeartMeasurent;
            if ( latestBikeMeasurent.MeasurementTime > latestHeartMeasurent.MeasurementTime)
            LatestMeasurement = latestHeartMeasurent;

            //Sending it over...
            object o = new
            {
                command = "livepatientdata",
                data = new
                {
                    id = id,
                    data = latestBikeMeasurent
                }
            };
            sender.SendMessage(JsonConvert.SerializeObject(o));
        }

        /// <summary>
        /// Sends the detailed patiens over...
        /// </summary>
        /// <param name="patients"></param>
        /// <param name="sender"></param>
        public static void SendDetails(List<Patient> patients, ISender sender)
        {
            object o = new
            {
                command = "detaildata",
                data = new
                {
                    data = patients
                }
            };
            sender.SendMessage(JsonConvert.SerializeObject(o));
        }

        /// <summary>
        /// Writes the history to the doctor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="sessoins"></param>
        /// <param name="patientID"></param>
        public static void HistoryWrite(ISender sender, List<Session> sessoins, string patientID)
        {
            object o = new
            {
                command = "getsessions",
                data = new
                {
                    patientid = patientID,
                    sessions = sessoins
                }
            };

            sender.SendMessage(JsonConvert.SerializeObject(o));
        }
    }
}
