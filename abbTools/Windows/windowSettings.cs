using System;
using System.Windows.Forms;
using System.Net.Mail;
using System.IO;


namespace abbTools
{
    public partial class windowSettings : Form
    {
        /********************************************************
         ***  WINDOWS SETTINGS - class properties and fields
         ********************************************************/

        /// <summary>
        /// GET or SET current project path
        /// </summary>
        public string currProject { get; set; }

        /// <summary>
        /// GET info about loading last project
        /// </summary>
        public bool loadLastProject { get; private set; }
        
        //path to settings XML file
        private string settingsPath = "";
        //private email data
        private bool mailActive = false;
        public SmtpClient mailClient = null;
        public MailMessage mailMessage = null;

        /********************************************************
         ***  WINDOWS SETTINGS - constructor
         ********************************************************/

        /// <summary>
        /// Default constructor
        /// </summary>
        public windowSettings()
        {
            InitializeComponent();
            //path data
            currProject = "";
            loadLastProject = false;
            settingsPath = Application.ExecutablePath + "\\settings.xml";
        }

        /********************************************************
         ***  WINDOWS SETTINGS - load data
         ********************************************************/

        /// <summary>
        /// Main method to load all abbTools data
        /// </summary>
        public void loadData()
        {
            if (File.Exists(settingsPath)) { 
                //load path data
                loadPathData();
                //load mail settings
                loadEmailData();
            }
            //fill all form elements from object-data
            updateGUI();
        }

        /// <summary>
        /// Method used to load all project paths
        /// </summary>
        private void loadPathData()
        {

        }

        /// <summary>
        /// Method used to load all email settings
        /// </summary>
        private void loadEmailData()
        {
            //fill client data GUI and update object
            if (mailClient == null) mailClient = new SmtpClient();

            //fill mail data and update object
            if (mailMessage == null) mailMessage = new MailMessage();

        }

        /********************************************************
         ***  WINDOWS SETTINGS - save data
         ********************************************************/

        /// <summary>
        /// Main method used to save abbTools data to settings file
        /// </summary>
        public void saveData()
        {
            //update objects
            saveEmailObjs();
            //save paths
            savePathObjs();
        }
        
        /// <summary>
        /// Method used to save all applications paths
        /// </summary>
        private void savePathObjs()
        {
            //save all paths to file
        }

        /// <summary>
        /// Method used to save email settings and data
        /// </summary>
        private void saveEmailObjs()
        {

        }

        /********************************************************
         ***  WINDOWS SETTINGS - data management
         ********************************************************/

        /// <summary>
        /// Main method used to update window GUI
        /// </summary>
        public void updateGUI()
        {
            //update e-mail GUI
            updateEmailGUI();
        }

        /// <summary>
        /// Method used to update email GUI
        /// </summary>
        private void updateEmailGUI()
        {

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

        /********************************************************
         ***  WINDOWS SETTINGS - internal events
         ********************************************************/

        /// <summary>
        /// Method triggered on click of button OK
        /// </summary>
        /// <param name="sender">Parent button which triggered this event</param>
        /// <param name="e">Event arguments</param>
        private void buttonOK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
