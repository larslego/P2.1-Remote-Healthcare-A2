﻿using RemoteHealthcare_Server.Data.User;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare_Server.Data
{
    public class Usermanagement
    {

        //Users
        private List<IUser> users;

        //Sessions
        private List<Session> activeSessions;


        public Usermanagement()
        {
            //Lists
            users = new List<IUser>();
            activeSessions = new List<Session>();

            //Filling
            try
            {
                //first trying filees
                FileProcessing.LoadUsers();
            }
            catch
            {
                //second filling with test data if not found
                users.Add(new Admin("Admin", "Password123"));
                users.Add(new Patient("JHAOogstvogel", "Welkom123", new DateTime(2002, 2, 1), "Joe", "Oogstvogel", "A12345"));
                users.Add(new Patient("RCADuinen", "ElpticCurves", new DateTime(1969, 2, 2), "Ronald", "Duinen", "A12346"));
                users.Add(new Patient("AESPeeren", "AESisTheBest", new DateTime(1969, 2, 2), "Arnold", "Peeren", "A12347"));
                users.Add(new Doctor("COMBomen", "Communication", new DateTime(1969, 2, 2), "Cornee", "Bomen", "Doctor FyssioTherapy", "PHD Avans Hogeschool"));
            }
        }


        public void AddPatient(Patient p)
        {
            //Needs to be implemented
            //Also needs to call to a mthode in file processing for writing to the list.
        }

        public void AddDoctor(Doctor d)
        {
            //Needs to be implemented
            //Also needs to call to a mthode in file processing for writing to the list.
        }

        public void RemovePatient(Patient p)
        {
            //Needs to be implemented
            //Also needs to call to a mthode in file processing for writing to the list.
        }

        public void RemoveDoctor(Doctor d)
        {
            //Needst to be implemented
            //Also needs to call to a mthode in file processing for writing to the list.
        }

        public IUser Credentials(string username, string password, int flag)
        {
            //Finding user
            foreach(IUser user in this.users){
                if (user.getUserType() == UserTypes.Patient && flag == 0)
                {
                    Patient p = (Patient)user;

                    if (p.Password == password && p.Username == username)
                    {
                        return user;
                    }

                } else if (user.getUserType() == UserTypes.Doctor && flag == 1)
                {
                    Doctor d = (Doctor)user;
                    if (d.Password == password && d.Username == username)
                    {
                        return user;
                    }
                } else if (user.getUserType() == UserTypes.Admin && flag == 2)
                {
                    Admin a = (Admin)user;
                    if (a.Password == password && a.Username == username)
                    {
                        return user;
                    }

                }
            }

            //Patient not found
            return null;
        }

        public void SessionUpdateBike(int rpm, int speed, int dist, int pow, int accpow, DateTime time, IUser user)
        {
            foreach (Session s in activeSessions)
            {
                if (s.Patient == (Patient)user)
                {
                    Server.PrintToGUI("Added new measurement");
                    s.BikeMeasurements.Add(new BikeMeasurement(time, rpm, speed, pow, accpow, dist));
                    return;
                }
            }
        }

        public void SessionUpdateHRM( DateTime time, int bpm, IUser user)
        {
            foreach (Session s in activeSessions)
            {
                if (s.Patient == (Patient)user)
                {
                    Server.PrintToGUI("Added new measurement");
                    s.HRMeasurements.Add(new HRMeasurement(time, bpm));
                    return;
                }
            }
        }

        public void SessionStart(IUser user)
        {
            if (user.getUserType() == UserTypes.Patient)
            {
                activeSessions.Add(new Session((Patient)user));
            }
        }

        public void SessionEnd(IUser user)
        {
            if (user.getUserType() == UserTypes.Patient)
            {
                foreach(Session s in activeSessions)
                {
                    if (s.Patient == (Patient)user)
                    {
                        activeSessions.Remove(s);
                        return;
                    }
                }
                
            }
        }

        ~Usermanagement()
        {
            FileProcessing.SaveUsers(users);
        }
    }
}
