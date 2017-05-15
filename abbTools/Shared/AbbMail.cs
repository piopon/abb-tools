using System.Net;
using System.Net.Mail;
using System.Text;
using System.Xml;

namespace abbTools.Shared
{
    public class AbbMail
    {
        /********************************************************
         ***  ABB MAIL - class properties and fields
         ********************************************************/

        /// <summary>
        /// GET informaction about active state of abbTools mail client
        /// </summary>
        public bool mailActive { get; private set; }

        /// <summary>
        /// GET information about main receiver
        /// </summary>
        public string mainRecv { get { return mailMessage?.To[0]?.Address; } }

        /// <summary>
        /// GET information about copy receiver
        /// </summary>
        public string copyRecv { get { return mailMessage?.To[1]?.Address; } }

        /// <summary>
        /// GET infromation about test subject
        /// </summary>
        public string testSubject { get; private set; }

        /// <summary>
        /// GET information about test message
        /// </summary>
        public string testMessage { get; private set; }

        /// <summary>
        /// GET information about user name
        /// </summary>
        public string userName { get { return mailLogin.UserName; } }

        /// <summary>
        /// GET information about user password
        /// </summary>
        public string userPass { get { return mailLogin.Password; } }

        /// <summary>
        /// GET information about port number
        /// </summary>
        public int portNo { get { return mailClient.Port; } }

        /// <summary>
        /// GET information about SMTP host domain
        /// </summary>
        public string smtpAddress { get { return mailClient.Host; } }

        /// <summary>
        /// GET information about current ssl state
        /// </summary>
        public bool sslState { get { return mailClient.EnableSsl; } }

        //private fields
        private NetworkCredential mailLogin = null;
        private SmtpClient mailClient = null;
        private MailMessage mailMessage = null;
        private bool mailClientValid, mailLoginValid, mailMessageValid;
        //static AbbMail object for singleton purposes
        private static AbbMail abbMailClient = null;

        /********************************************************
         ***  ABB MAIL - constructor
         ********************************************************/

        /// <summary>
        /// Default constructor (private = singleton)
        /// </summary>
        private AbbMail()
        {
            mailLogin = new NetworkCredential();
            mailMessage = new MailMessage();
            //create email client and connect to event
            mailClient = new SmtpClient();
            mailClient.SendCompleted += AbbMailClientSendOK;
            //reset valid flags
            mailClientValid = false;
            mailLoginValid = false;
            mailMessageValid = false;
        }

        /// <summary>
        /// Method used to get instance of abbMail client object 
        /// </summary>
        /// <returns>Instance address of AbbMail object</returns>
        static public AbbMail getInstance()
        {
            if (abbMailClient == null) abbMailClient = new AbbMail();
            return abbMailClient;
        }

        /********************************************************
         ***  ABB MAIL - settings
         ********************************************************/
        
        /// <summary>
        /// Method used to update login data
        /// </summary>
        /// <param name="newUserName">New login user name </param>
        /// <param name="newUserPass">New login user password</param>
        public void updateLogonInfo(string newUserName, string newUserPass)
        {
            mailLogin.UserName = newUserName;
            mailLogin.Password = newUserPass;
            //update credentials in SmtpClient object
            mailClient.Credentials = mailLogin;
            //validate login data flag
            mailLoginValid = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientActive"></param>
        /// <param name="smtpAddress"></param>
        /// <param name="port"></param>
        /// <param name="sslState"></param>
        public void updateConnInfo(bool clientActive,string smtpAddress, int port, bool sslState)
        {
            mailActive = clientActive;
            mailClient.Host = smtpAddress;
            mailClient.Port = port;
            mailClient.EnableSsl = sslState;
            //validate connection data flag
            mailClientValid = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toRecv"></param>
        /// <param name="ccRecv"></param>
        public void updateCommInfo(string toRecv, string ccRecv)
        {
            mailMessage.From = new MailAddress(mailLogin.UserName + mailClient.Host.Replace("smtp.", "@"), "abbTools", Encoding.UTF8);
            mailMessage.To.Add(new MailAddress(toRecv));
            if (ccRecv != "") mailMessage.To.Add(new MailAddress(ccRecv));
            //message formatting
            mailMessage.BodyEncoding = Encoding.UTF8;
            mailMessage.IsBodyHtml = true;
            mailMessage.Priority = MailPriority.Normal;
            mailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            //validate message data flag
            mailMessageValid = true;
        }

        /********************************************************
         ***  ABB MAIL - main methods
         ********************************************************/

        /// <summary>
        /// Method used to send message from email client
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        public void send(string subject, string message)
        {
            if (mailActive) {
                //check if login data was updated
                if (!mailLoginValid) return;
                //check if client data was updated
                if (!mailClientValid) return;
                //check if message data was updated
                if (!mailMessageValid) return;
                //build new message
                mailMessage.Subject = subject;
                mailMessage.Body = message;
                //send data               
                mailClient.SendAsync(mailMessage, "sending...");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        public void loadData(ref XmlReader xml)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        public void saveData(ref XmlWriter xml)
        {

        }

        /********************************************************
         ***  ABB MAIL - events
         ********************************************************/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AbbMailClientSendOK(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

    }
}
