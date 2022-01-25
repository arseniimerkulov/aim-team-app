﻿using System;
using System.Collections.Generic;
using System.Drawing;

namespace Core
{
    public class Users
    {
        public List<User> ListOfUsers { get; set; }
    }
    
    public class User : BaseEntity
    {
        private string _filePath;
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsVerified { get; set; }
        public Bitmap Photo { get; set; }

        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                Photo = new Bitmap(_filePath);
            }
        }
    }
}
