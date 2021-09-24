﻿using CommClass;
using Newtonsoft.Json;
using System;

using System.Net.Sockets;

using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace other
{
    class Programm
    {
        static void Main(string[] args)
        {
            TcpClient client = new TcpClient("localhost", 8080);
            if (client.Connected)
            {
                Console.WriteLine("Connected to the amazing server...");
                NetworkStream stream = client.GetStream();
                EncryptedSender sender = new EncryptedSender(stream);


                Console.WriteLine("Try to connect with given credentials...");
                //Sending test logins
                object o = new
                {
                    command = "login",
                    data = new
                    {
                        us = "JHAOogstvogel",
                        pass = "Welkom123",
                        flag = 0
                    }
                };
                sender.SendMessage(JsonConvert.SerializeObject(o), client.GetStream());

                //Console.WriteLine(sender.ReadMessage(stream));


                if (sender.ReadMessage(stream).Contains("succesfull connect"))
                {
                    while (true)
                    {
                        Console.WriteLine("Athenticated");
                        object f = new
                        {
                            command = "ergometer",
                            data = new
                            {
                                time = new DateTime(1, 2, 3),
                                bpm = 100
                            }

                        };
                        sender.SendMessage(JsonConvert.SerializeObject(f), client.GetStream());

                       
                        Console.WriteLine("Send data");
                        Thread.Sleep(1000);
                    }

                }



            }


        }


        }
   
    }

