﻿using CommClass;
using Newtonsoft.Json;
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
        public static void LoginWrite(bool succes, ISender sender)
        {
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


        public static void MessageWrite(string msg, ISender sender)
        {
            object o = new
            {
                command = "message",
                data = msg,
                flag = 2
            };

            //sender.SendMessage(JsonConvert.SerializeObject(o));
        }

        public static void ResistanceWrite(int resistance, TcpClient client)
        {
            object o = new
            {
                command = "setresist",
                data = resistance
            };
            
            //.WriteMessage(JsonConvert.SerializeObject(o), client.GetStream());
        }

        public static void AbortWrite(TcpClient client)
        {
            object o = new
            {
                command = "abort",
                data = new { }
            };

            //ComClass.WriteMessage(JsonConvert.SerializeObject(o), client.GetStream());
        }
    }
}
