using System.Collections.Generic;

namespace PSL.OTS.Email.Models
{
    public class EMailMessageConfig
    {
        public List<string> ToList { get; set; }
        public List<string> CCList { get; set; }
        public List<string> BCCList { get; set; }
        public string Subject { get; set; }
        public string MessageBody { get; set; }
        public bool IsBodyHTML { get; set; }
        public short LocationID { get; set; }
        public string ReportAdapter { get; set; }
    }

    public class ConnectionInfo
    {
        public int Port { get; set; }
        public string Host { get; set; }
        /// <summary>
        /// User for ESMTP authentication
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// password for ESMTP authentication  
        /// </summary>
        public string Password { get; set; }
        public bool EnableSSL { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public int Timeout { get; set; } = 10000;
        public string FromMailID { get; set; }
    }
}
