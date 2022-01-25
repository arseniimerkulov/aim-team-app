﻿namespace Core
{
    public class AppSettings
    {
        public string TempDir { get; set; }
        public string Email { get; set; }
        public string EmailDisplayName { get; set; }

        public SmtpSettings SmtpSettings { get; set; }
    }
}
