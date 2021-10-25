﻿using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace RemoteHealthcare_Client
{
    /// <summary>
    /// Abstract class that handles the abstration layer above all the different data sources
    /// </summary>
    public abstract class DataManager
    {
        protected static readonly IList<DataManager> NetworkManagers = new List<DataManager>();

        /// <summary>
        /// Constructor for the DataManager
        /// </summary>
        protected DataManager()
        {
            DataManager.NetworkManagers.Add(this);
        }

        /// <summary>
        /// Method that handles the receiving of data from one DataManager to an other
        /// </summary>
        /// <param name="data">The data send to the manager</param>
        public abstract void ReceivedData(JObject data);

        /// <summary>
        /// Sends the given data to all the other dataManagers
        /// </summary>
        /// <param name="data">The data to be send</param>
        protected void SendToManagers(JObject data)
        {
            /*
            foreach(DataManager manager in DataManager.NetworkManagers)
            {
                if (manager.Equals(this))
                    continue;

                manager.ReceivedData(data);
            }
            */

            for (int i = 0; i < DataManager.NetworkManagers.Count; i++)
            {
                if (DataManager.NetworkManagers[i].Equals(this))
                    continue;
                
                DataManager.NetworkManagers[i].ReceivedData(data);
            }
        }
    }
}