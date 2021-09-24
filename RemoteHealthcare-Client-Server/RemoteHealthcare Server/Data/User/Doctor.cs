﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare_Server.Data.User
{
    public class Doctor 
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DoctorType { get; set; }

        public string PHDType { get; set; }

        public Doctor(string username, string password, DateTime dateOfBirth, string firstName, string lastName, string doctorType, string pHDType)
        {
            Username = username;
            Password = password;
            DateOfBirth = dateOfBirth;
            FirstName = firstName;
            LastName = lastName;
            DoctorType = doctorType;
            PHDType = pHDType;
        }
    }

}