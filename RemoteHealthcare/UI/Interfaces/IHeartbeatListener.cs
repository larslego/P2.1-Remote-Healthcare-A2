﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare.UI.Interfaces
{
    interface IHeartbeatListener
    {
        void OnHeartBeatChanged(int heartBeat);
    }
}
