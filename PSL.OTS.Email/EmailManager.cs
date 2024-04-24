using PSL.OTS.Email.Models;
using System;
using System.Net.Mail;
using System.Text;

namespace PSL.OTS.Email
{
    public class EmailManager : IDisposable
    {

        public delegate void MailSendingCompleted(System.ComponentModel.AsyncCompletedEventArgs e);
        public event MailSendingCompleted mailsendingcompleted;
        private ConnectionInfo connectionInfo { get; set; }
        public EmailManager(ConnectionInfo connectionInfo)
        {
            this.connectionInfo = connectionInfo;
        }

        public void SendMail(EMailMessageConfig messageConfig)
        {
            try
            {
                if (connectionInfo == null)
                {
                    throw new Exception("EmailManager, ConnectionInfo Object is null!");
                }
                else if (messageConfig == null)
                {
                    throw new Exception("invalid mail configuration");
                }
                using (SmtpClient client = new SmtpClient())
                {
                    client.Port = connectionInfo.Port;
                    client.Host = connectionInfo.Host;
                    client.EnableSsl = connectionInfo.EnableSSL;
                    client.Timeout = connectionInfo.Timeout;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = connectionInfo.UseDefaultCredentials;
                    client.Credentials = new System.Net.NetworkCredential(connectionInfo.UserName, connectionInfo.Password);
                    using (MailMessage message = new MailMessage())
                    {
                        message.From = new MailAddress(connectionInfo.FromMailID);
                        message.To.Add(string.Join(",", messageConfig.ToList));
                        if (messageConfig.CCList != null)
                        {
                            message.CC.Add(string.Join(",", messageConfig.CCList));
                        }
                        message.Subject = messageConfig.Subject;
                        message.Body = messageConfig.MessageBody;
                        message.IsBodyHtml = messageConfig.IsBodyHTML;
                        message.BodyEncoding = UTF8Encoding.UTF8;
                        message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                        client.Send(message);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async void SendMailAsync(EMailMessageConfig messageConfig)
        {
            if (connectionInfo == null)
                throw new Exception("EmailManager, ConnectionInfo Object is null..!");
            else if (messageConfig == null)
                throw new Exception("invalid mail configuration");
            using (SmtpClient client = new SmtpClient())
            {
                client.Port = connectionInfo.Port;
                client.Host = connectionInfo.Host;
                client.EnableSsl = connectionInfo.EnableSSL;
                client.Timeout = connectionInfo.Timeout;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = connectionInfo.UseDefaultCredentials;
                client.Credentials = new System.Net.NetworkCredential(connectionInfo.UserName, connectionInfo.Password);
                using (MailMessage message = new MailMessage())
                {
                    message.From = new MailAddress(connectionInfo.FromMailID);
                    message.To.Add(string.Join(",", messageConfig.ToList));
                    if (messageConfig.CCList != null)
                    {
                        message.CC.Add(string.Join(",", messageConfig.CCList));
                    }
                    message.Subject = messageConfig.Subject;
                    message.Body = messageConfig.MessageBody;
                    message.IsBodyHtml = messageConfig.IsBodyHTML;
                    message.BodyEncoding = UTF8Encoding.UTF8;
                    message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                    client.SendCompleted += new SendCompletedEventHandler(client_SendCompleted);
                    await client.SendMailAsync(message);
                }
            }
        }

        private void client_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                if (mailsendingcompleted != null)
                {
                    mailsendingcompleted.BeginInvoke(e, null, null);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Dispose()
        {
            try
            {
                GC.SuppressFinalize(this);
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
