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
        public string mainRecv
        {
            get { return mailMessage?.To.Count > 0 ? mailMessage?.To[0]?.Address : ""; }
            private set { mailMessage.To[0] = new MailAddress(value); }
        }

        /// <summary>
        /// GET information about copy receiver
        /// </summary>
        public string copyRecv
        {
            get { return mailMessage?.To.Count > 1 ? mailMessage?.To[1]?.Address : ""; }
            private set { mailMessage.To[1] = new MailAddress(value); }
        }

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
        public string userName
        {
            get { return mailLogin.UserName; }
            private set { mailLogin.UserName = value; }
        }

        /// <summary>
        /// GET information about user password
        /// </summary>
        public string userPass
        {
            get { return mailLogin.Password; }
            private set { mailLogin.Password = value; }
        }

        /// <summary>
        /// GET information about port number
        /// </summary>
        public int portNo
        {
            get { return mailClient.Port; }
            private set { mailClient.Port = value; }
        }

        /// <summary>
        /// GET information about SMTP host domain
        /// </summary>
        public string smtpAddress
        {
            get { return mailClient.Host; }
            private set { mailClient.Host = value; }
        }

        /// <summary>
        /// GET information about current ssl state
        /// </summary>
        public bool sslState
        {
            get { return mailClient.EnableSsl; }
            private set { mailClient.EnableSsl = value; }
        }

        //private fields
        private NetworkCredential mailLogin = null;
        private SmtpClient mailClient = null;
        private MailMessage mailMessage = null;
        private bool mailClientValid, mailLoginValid, mailMessageValid;
        //static AbbMail object for singleton purposes
        private static AbbMail abbMailClient = null;
        //events handling
        public delegate void mailSentDone(System.ComponentModel.AsyncCompletedEventArgs e);
        public event mailSentDone AbbMailSent;

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
        /// Method used to update connection info (myClient active & data)
        /// </summary>
        /// <param name="clientActive">Activation state of mail client</param>
        /// <param name="smtpAddress">SMTP host address</param>
        /// <param name="port">Connection port to host</param>
        /// <param name="sslState">State of SLL</param>
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
        /// Method used to update communication info (receiver and carbon copy)
        /// </summary>
        /// <param name="toRecv">Receivers address</param>
        /// <param name="ccRecv">Carbon copy receivers address</param>
        public void updateCommInfo(string toRecv, string ccRecv)
        {
            mailMessage.From = new MailAddress(mailLogin.UserName + mailClient.Host.Replace("smtp.", "@"), "abbTools", Encoding.UTF8);
            mailMessage.To.Clear();
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
        /// <param name="subject">Subject of message to send</param>
        /// <param name="message">Message of message to send</param>
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
        /// Function used to load email data from XML file
        /// </summary>
        /// <param name="xml">XML file to load data from</param>
        public void loadData(ref XmlReader xml)
        {
            //get email main data
            while (xml.Read()) {
                bool start = xml.NodeType == XmlNodeType.Element,
                     email = xml.Name.StartsWith("email");
                //if we are starting to read email data then get it
                if (start && email) {
                    mailActive = bool.Parse(xml.GetAttribute("active"));
                    testSubject = xml.GetAttribute("subject");
                    testMessage = xml.GetAttribute("message");
                    //break from WHILE loop
                    break;
                }
            }
            //get communication data
            while (xml.Read()) {
                bool start = xml.NodeType == XmlNodeType.Element,
                     comm = xml.Name.StartsWith("comm");
                //if we are starting to read email data then get it
                if (start && comm) {
                    mainRecv = xml.GetAttribute("recv");
                    copyRecv = xml.GetAttribute("cc");
                    //break from WHILE loop
                    break;
                }
            }
            //get connection data
            while (xml.Read()) {
                bool start = xml.NodeType == XmlNodeType.Element,
                     conn = xml.Name.StartsWith("conn");
                //if we are starting to read email data then get it
                if (start && conn) {
                    //read login data
                    if (xml.Read() && xml.Name.StartsWith("login")) {
                        userName = xml.GetAttribute("user");
                        userPass = xml.GetAttribute("pass");
                    }
                    //read params data
                    if (xml.Read() && xml.Name.StartsWith("params")) {
                        smtpAddress = xml.GetAttribute("smtp");
                        portNo = int.Parse(xml.GetAttribute("port"));
                        sslState = bool.Parse(xml.GetAttribute("ssl"));
                    }
                    //break from WHILE loop
                    break;
                }
            }
        }

        /// <summary>
        /// Function used to save email data to XML file
        /// </summary>
        /// <param name="xml">XML file to save data to</param>
        public void saveData(ref XmlWriter xml)
        {
            //save all properties (signal activation, name, default run value)
            xml.WriteStartElement("email");
            xml.WriteAttributeString("active", mailActive.ToString());
            xml.WriteAttributeString("subject", testSubject);
            xml.WriteAttributeString("message", testMessage);
            //write communication receivers address
            xml.WriteStartElement("comm");
            xml.WriteAttributeString("recv", mainRecv);
            xml.WriteAttributeString("cc", copyRecv);
            xml.WriteEndElement();
            //write connection parameters
            xml.WriteStartElement("conn");
            xml.WriteStartElement("login");
            xml.WriteAttributeString("user", userName);
            xml.WriteAttributeString("pass", userPass);
            xml.WriteEndElement();
            xml.WriteStartElement("params");
            xml.WriteAttributeString("smtp", smtpAddress);
            xml.WriteAttributeString("port", portNo.ToString());
            xml.WriteAttributeString("ssl", sslState.ToString());
            xml.WriteEndElement();
            //end of "conn" element
            xml.WriteEndElement();
            //end of "email" element
            xml.WriteEndElement();
        }

        /********************************************************
         ***  ABB MAIL - events
         ********************************************************/

        /// <summary>
        /// Method triggered on message sent event
        /// </summary>
        /// <param name="sender">Parent client that triggered current event</param>
        /// <param name="e">Event arguments</param>
        private void AbbMailClientSendOK(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            // SIMPLIFIED { if (AbbMailSent != null) AbbMailSent(e); }
            AbbMailSent?.Invoke(e);
        }

    }
}
