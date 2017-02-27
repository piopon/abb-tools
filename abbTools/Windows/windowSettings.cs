using System;
using System.Windows.Forms;
using System.Net.Mail;
using System.IO;


namespace abbTools
{
    public partial class windowSettings : Form
    {
        /****************************************
         *** APP SETTINGS DATA
         ****************************************/
        //file paths data
        private string settingsPath = "";
        private string lastProject = "";
        private string lastAppPath = "";
        //email data
        private bool mailActive = false;
        public SmtpClient mailClient = null;
        public MailMessage mailMessage = null;

        /****************************************
         *** GENERAL
         ****************************************/
        public windowSettings()
        {
            InitializeComponent();
            //initialize settings path
            settingsPath = Application.ExecutablePath + "\\settings.xml";
        }

        public void loadData()
        {
            //load all paths (if non-existent dont do anything
            if (loadPathObjs()) {
                //load mail settings
                updateEmailObjs();
            }
            //fill all form elements from object-data
            loadGUI();
        }

        public void loadGUI()
        {
            //update e-mail GUI
            updateEmailGUI();
        }

        public void saveData()
        {
            //update objects
            saveEmailObjs();
            //save paths
            savePathObjs();
        }

        /****************************************
         *** FILE PATHS
         ****************************************/
        public void updateAppPath(string newPath)
        {
            lastAppPath = newPath;
        }

        public void updateProjPath(string newPath)
        {
            lastProject = newPath;
        }

        public string getProjPath()
        {
            return lastProject;
        }

        private bool loadPathObjs()
        {
            //check if settings file exists
            bool result = File.Exists(settingsPath);
            if (result) {

            } else {
                lastProject = "";
                lastAppPath = "C:\\";
    }
            return result;
        }

        private void savePathObjs()
        {
            //save all paths to file
        }

        /****************************************
         *** E-MAIL 
         ****************************************/

        private void updateEmailGUI()
        {

        }

        private void updateEmailObjs()
        {
            //fill client data GUI and update object
            if (mailClient == null) mailClient = new SmtpClient();

            //fill mail data and update object
            if (mailMessage == null) mailMessage = new MailMessage();
        }

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
        
        private void saveEmailObjs()
        {

        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
