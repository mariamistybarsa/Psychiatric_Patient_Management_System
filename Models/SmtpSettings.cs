﻿namespace Psychiatrist_Management_System.Models
{
    public class SmtpSettings
    {
        public string? Server { get; set; }
        public int? Port { get; set; }
        public string? SenderEmail { get; set; }
        public string? Password { get; set; }
        public bool? EnableSsl { get; set; }
    }
}
