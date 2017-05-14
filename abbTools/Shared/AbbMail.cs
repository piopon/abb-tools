using System.Net.Mail;
using System.Xml;

namespace abbTools.Shared
{
    public class AbbMail
    {
        private bool mailActive = false;
        private SmtpClient mailClient = null;
        private MailMessage mailMessage = null;

        public AbbMail()
        {
            //fill client data GUI and update object
            if (mailClient == null) mailClient = new SmtpClient();

            //fill mail data and update object
            if (mailMessage == null) mailMessage = new MailMessage();

        }

        /// <summary>
        /// Muthod used to update email settings status 
        /// </summary>
        /// <param name="myMail">Mail type to update</param>
        /// <returns>TRUE if mail is active and ready to go, FALSE otherwise</returns>
        public bool getMailStatus(abbStatus.mail myMail)
        {
            if (mailActive) {
                if (myMail == abbStatus.mail.openApp) {
                    mailMessage.Subject = "abbTools::openApp";
                } else if (myMail == abbStatus.mail.closeApp) {
                    mailMessage.Subject = "abbTools::closeApp";
                } else if (myMail == abbStatus.mail.exception) {
                    mailMessage.Subject = "abbTools::exception";
                }
            }
            return mailActive;
        }

        public void loadData(ref XmlReader xml)
        {

        }

        public void saveData(ref XmlWriter xml)
        {

        }
    }
}
