﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare_Server.Data.User
{
    public class Admin : IUser
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public Admin(string username, string password)
        {
            Username = username;
            Password = password;
        }


        public UserTypes getUserType()
        {
            return UserTypes.Admin;
        }
    }
}
