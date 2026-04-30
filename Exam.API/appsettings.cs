namespace Exam.API
{


    public class Rootobject
    {
        public Logging Logging { get; set; }
        public string AllowedHosts { get; set; }
        public Connectionstrings ConnectionStrings { get; set; }
        public Jwt Jwt { get; set; }
        public Emailsettings EmailSettings { get; set; }
    }

    public class Logging
    {
        public Loglevel LogLevel { get; set; }
    }

    public class Loglevel
    {
        public string Default { get; set; }
        public string MicrosoftAspNetCore { get; set; }
        public string ExamAPIControllers { get; set; }
    }

    public class Connectionstrings
    {
        public string ExamDb { get; set; }
    }

    public class Jwt
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpireMinutes { get; set; }
    }

    public class Emailsettings
    {
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }


}
