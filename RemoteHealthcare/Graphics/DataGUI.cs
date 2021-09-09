﻿using RemoteHealthcare.Hardware;
using RemoteHealthcare.Software;
using RemoteHealthcare.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare.Graphics
{
    class DataGUI
    {
        private static int RPM_Line = 2;
        private static int Speed_Line = 3;
        private static int Heart_Line = 4;
        private static int Time_Line = 5;
        private static int Distance_Line = 6;
        private static int TotalPower_Line = 7;
        private static int CurrentPower_Line = 8;


        public DataGUI()
        {
            Device device = new PhysicalDevice("Tacx Flux 00457", "Decathlon Dual HR");
            //Device device = new SimulatedDevice();
            device.onHeartrate += DrawHeartrate;
            device.onRPM += DrawRPM;
            device.onSpeed += DrawSpeed;
            device.onDistance += DrawDistance;
            device.onElapsedTime += DrawElapsedTime;
            device.onTotalPower += DrawTotalPower;
            device.onCurrentPower += DrawCurrentPower;
        }

        static void Main(string[] args)
        {
            new DataGUI();

            Trace.Listeners.Add(new TextWriterTraceListener("debug.log"));
            Trace.AutoFlush = true;

            GUITools.DrawBasicLayout("Remote Healthcare by A2");
            Console.CursorVisible = false;
       

            Console.Read();
        }

        // Called by onRPM event.
        private void DrawRPM(object sender, int e)
        {
            Console.SetCursorPosition(0, RPM_Line);
            Console.WriteLine($"RPM: {e}     ");
        }

        // Called by onSpeed event.
        private void DrawSpeed(object sender, double e)
        {
            Console.SetCursorPosition(0, Speed_Line);
            Console.WriteLine($"Speed: {e} KM/H     ");
     
        }

        // Called by onHeartrate event.
        public void DrawHeartrate(Object sender, int heartrate)
        {
            Console.SetCursorPosition(0, Heart_Line);
            Console.WriteLine($"Heartrate: {heartrate} BPM     ");
        }

        // Called by onElapsedTime event.
        private void DrawElapsedTime(object sender, double e)
        {
            Console.SetCursorPosition(0, Time_Line);
            Console.WriteLine($"Elapsed time: {e} seconds     ");
        }

        // Called by onDistance event.
        private void DrawDistance(object sender, double e)
        {
            Console.SetCursorPosition(0, Distance_Line);
            Console.WriteLine($"Distance: {e} m     ");
        }

        private void DrawTotalPower(object sender, int e)
        {
            Console.SetCursorPosition(0, TotalPower_Line);
            Console.WriteLine($"Total power: {e} Watt     ");
        }

        private void DrawCurrentPower(object sender, int e)
        {
            Console.SetCursorPosition(0, CurrentPower_Line);
            Console.WriteLine($"Current power: {e} Watt     ");
        }
    }
}
