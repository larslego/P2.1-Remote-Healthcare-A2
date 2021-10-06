﻿using CommClass;
using Newtonsoft.Json.Linq;
using RemoteHealthcare_Server.Data;
using RemoteHealthcare_Server.Data.User;
using RemoteHealthcare_Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace RemoteHealthcare_Server
{
  

    public class JSONReader
    {

        public event EventHandler<IUser> CallBack;
        private bool Authenticated = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jObject"></param>
        /// <param name="sender"></param>
        /// <param name="user"></param>
        /// <param name="managemet"></param>
        public void DecodeJsonObject(JObject jObject, ISender sender, IUser user, Usermanagement managemet)
        {
            string command = jObject.GetValue("command").ToString();

            MethodInfo[] methods = typeof(JSONReader).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.ExactBinding);
            foreach (MethodInfo method in methods)
            {
                //This if could probably be short but this is much clearer
                if ((method.GetCustomAttribute<AccesManagerAttribute>() != null && method.GetCustomAttribute<AccesManagerAttribute>().GetCommand() == command
                     && user != null && method.GetCustomAttribute<AccesManagerAttribute>().GetUserType() == user.getUserType()) || (method.GetCustomAttribute<AccesManagerAttribute>() != null && method.GetCustomAttribute<AccesManagerAttribute>().GetCommand() == command
                     && user == null && method.GetCustomAttribute<AccesManagerAttribute>().GetUserType() == UserTypes.Unkown))
                {
                    method.Invoke(this, new object[] { jObject, sender, user, managemet });
                } 
            }
        }

        /// <summary>
        /// Login function
        /// </summary>
        /// <param name="Jobject">The object send</param>
        /// <param name="sender">The receiver and sender</param>
        /// <param name="management">The management object</param>
        [AccesManager("login", UserTypes.Unkown)]
        private void LoginAction(JObject Jobject, ISender sender, IUser u, Usermanagement management)
        {
            //Checking op login string
            string command = Jobject.GetValue("command").ToString();
            if (command == "login")
            {
                //Getting alle the amazing data
                JObject data = (JObject)Jobject.GetValue("data");
                string username = data.GetValue("us").ToString();
                string password = data.GetValue("pass").ToString();
                int flag = int.Parse(data.GetValue("flag").ToString());


                //Getting the user
                IUser user = management.Credentials(username, password, flag);
                if (user != null)
                {
                    JSONWriter.LoginWrite(true, sender);
                    Server.PrintToGUI("Authenticated....");
                    Authenticated = true;
                    CallBack?.Invoke(this, user);
                    return;
                }
                else
                {
                    JSONWriter.LoginWrite(false, sender);
                    Server.PrintToGUI("Not a user....");
                    return;
                }
            }

            //Not valid as command
            return;
        }

        /// <summary>
        /// This method does save and send data to subs and session.
        /// </summary>
        /// <param name="Jobject"></param>
        /// <param name="sender"></param>
        /// <param name="user"></param>
        /// <param name="usermanagement"></param>
        [AccesManager("ergodata", UserTypes.Patient)]
        private void ReceiveMeasurement(JObject Jobject, ISender sender, IUser user, Usermanagement usermanagement)
        {
            Server.PrintToGUI("Got your data");
            if (user != null)
            {
                //Bike
                JToken rpm = Jobject.SelectToken("data.rpm");
                JToken speed = Jobject.SelectToken("data.speed");
                JToken dist = Jobject.SelectToken("data.dist");
                JToken pow = Jobject.SelectToken("data.pow");
                JToken accpow = Jobject.SelectToken("data.accpow");

                //Heart
                JToken bpm = Jobject.SelectToken("data.bpm");

                //All
                JToken time = Jobject.SelectToken("data.time");

                //Checks
                if (rpm != null)
                {
                    usermanagement.SessionUpdateBike(int.Parse(rpm.ToString()),
                        (int)double.Parse(speed.ToString()), (int)double.Parse(dist.ToString()), int.Parse(pow.ToString()),
                        int.Parse(accpow.ToString()), DateTime.Parse(time.ToString()), user);
                }
                else if (bpm != null)
                {
                    usermanagement.SessionUpdateHRM(DateTime.Parse(time.ToString()), int.Parse(bpm.ToString()), user);
                }
            }
        }

        /// <summary>
        /// Sends the resistance from the doctor to the client.
        /// </summary>
        /// <param name="jObject">This is the JSON-file to decode were the data is stored</param> 
        /// <param name="sender">This is the sender for sending it to the client</param>
        /// <param name="user">This is the adress for sending it.</param>
        /// <param name="managemet">This is a managment object.</param>
        [AccesManager("setresist", UserTypes.Doctor)]
        private void SettingErgometer(JObject jObject, ISender sender, IUser user, Usermanagement managemet)
        {
            throw new NotImplementedException();
        }

        [AccesManager("abort", UserTypes.Doctor)]
        private void AbortingClient(JObject jObject, ISender sender, IUser user, Usermanagement managemet)
        {
            throw new NotImplementedException();
        }

        [AccesManager("getallclients", UserTypes.Doctor)]
        private void GetAllClients(JObject jObject, ISender sender, IUser user, Usermanagement managemet)
        {
            throw new NotImplementedException();
        }


        [AccesManager("subtopatient", UserTypes.Doctor)]
        private void SubscribeToLiveSession(JObject jObject, ISender sender, IUser user, Usermanagement managemet)
        {
            throw new NotImplementedException();
        }

        [AccesManager("getsessions", UserTypes.Doctor)]
        private void GetHistoricSession(JObject jObject, ISender sender, IUser user, Usermanagement managemet)
        {
            throw new NotImplementedException();
        }
            

        [AccesManager("newsession", UserTypes.Doctor)]
        private void StartNewSession(JObject jObject, ISender sender, IUser user, Usermanagement management)
        {

            //Logic for parsing still needs to be made which user it is and if its on or of...
            management.SessionStart(user);
            management.SessionEnd(user);
        }
    }


    /// <summary>
    /// This is the custom attribute used for jumping to the correct method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AccesManagerAttribute : Attribute
    {
        string command;
        UserTypes type;

        public AccesManagerAttribute(string command, UserTypes type)
        {
            this.command = command;
            this.type = type;
        }

        public string GetCommand()
        {
            return command;
        }

        public UserTypes GetUserType()
        {
            return type;
        }
    }
    
  
}