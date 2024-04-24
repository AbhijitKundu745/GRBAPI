namespace PSL.Laundry.CentralService.Models
{
    public class NotificationConfiguration
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string MobileNumber { get; set; }
        public string EmailId { get; set; }
        public bool EmailNotification { get; set; }
        public bool EmailCC { get; set; }
        public bool SMSNotification { get; set; }
    }
}