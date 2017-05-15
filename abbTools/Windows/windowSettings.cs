using System;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Xml;
using abbTools.Shared;

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
        /// GET flag about showing curr project path
        /// </summary>
        public bool showCurrProject { get; private set; }

        /// <summary>
        /// GET info about loading last project
        /// </summary>
        public bool loadLastProject { get; private set; }

        /// <summary>
        /// GET info about always on top property
        /// </summary>
        public bool alwaysOnTop { get; private set; }

        /// <summary>
        /// GET information about runtime signal
        /// </summary>
        public AbbRunSignal runtimeSig { get; private set; }

        /// <summary>
        /// GET information about mail
        /// </summary>
        public AbbMail mailService { get; private set; }
        
        //path to settings XML file
        private string settingsPath = "";      
        private Form overrideParent;

        /********************************************************
         ***  WINDOWS SETTINGS - constructor
         ********************************************************/

        /// <summary>
        /// Default constructor
        /// </summary>
        public windowSettings(int clientHeight, int clientWidth)
        {
            InitializeComponent();
            //default settings path
            settingsPath = Application.StartupPath + "\\settings.xml";
            //semi-transparent background
            Height = clientHeight;
            Width = clientWidth;
            //create abbTools runtime controller signal object
            runtimeSig = AbbRunSignal.getInstance();
            //create mail service object
            mailService = AbbMail.getInstance();
            //path data
            currProject = "";
            showCurrProject = false;
            loadLastProject = false;
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
                //create new xmlFile
                XmlReader xmlFile = XmlReader.Create(settingsPath);
                while (xmlFile.Read()) {
                    //read every node from XML document
                    if ((xmlFile.NodeType == XmlNodeType.Element) && (xmlFile.Name.StartsWith("settings"))) {
                        if (xmlFile.HasAttributes) {
                            //load GENERAL SETTINGS
                            loadMainWindowSettings(ref xmlFile);
                            loadRunSignal(ref xmlFile);
                            //load APPS SETTINGS
                            loadAppsSettings(ref xmlFile);
                            //load EMAIL SETTINGS
                            loadEmailSettings(ref xmlFile);
                        }
                    }
                }
                //close file
                xmlFile.Close();
            }
        }

        /// <summary>
        /// Method used to load all main window settings 
        /// </summary>
        private void loadMainWindowSettings(ref XmlReader xml)
        {

        }

        /// <summary>
        /// Method used to load abbTools runtime controller signal
        /// </summary>
        private void loadRunSignal(ref XmlReader xml)
        {
            runtimeSig?.loadData(ref xml);
        }

        /// <summary>
        /// Method used to load my applications settings
        /// </summary>
        private void loadAppsSettings(ref XmlReader xml)
        {

        }

        /// <summary>
        /// Method used to load email settings and data
        /// </summary>
        private void loadEmailSettings(ref XmlReader xml)
        {
            mailService?.loadData(ref xml);   
        }

        /********************************************************
         ***  WINDOWS SETTINGS - save data
         ********************************************************/

        /// <summary>
        /// Main method used to save abbTools data to settings file
        /// </summary>
        public void saveData()
        {
            //create new xmlFile
            XmlWriter xmlFile = XmlWriter.Create(settingsPath, new XmlWriterSettings { Indent = true });
            xmlFile.WriteStartElement("settings");
            //save GENERAL SETTINGS
            saveMainWindowSettings(ref xmlFile);
            saveRunSignal(ref xmlFile);
            //save APPS SETTINGS
            saveAppsSettings(ref xmlFile);
            //save EMAIL SETTINGS
            saveEmailSettings(ref xmlFile);
            //close up file
            xmlFile.WriteEndElement();
            xmlFile.Close();
        }
        
        /// <summary>
        /// Method used to save all main window settings 
        /// </summary>
        private void saveMainWindowSettings(ref XmlWriter xml)
        {

        }

        /// <summary>
        /// Method used to save abbTools runtime controller signal
        /// </summary>
        private void saveRunSignal(ref XmlWriter xml)
        {
            runtimeSig?.saveData(ref xml);
        }
        
        /// <summary>
        /// Method used to save my applications settings
        /// </summary>
        private void saveAppsSettings(ref XmlWriter xml)
        {

        }

        /// <summary>
        /// Method used to save email settings and data
        /// </summary>
        private void saveEmailSettings(ref XmlWriter xml)
        {
            mailService?.saveData(ref xml);
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
            //save application data
            saveData();
            //close this window
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Method triggered on click of button APPLY
        /// </summary>
        /// <param name="sender">Parent button which triggered this event</param>
        /// <param name="e">Event arguments</param>
        private void buttonApply_Click(object sender, EventArgs e)
        {
            //save application data
            saveData();
        }

        /// <summary>
        /// Method triggered on click of button CANCEL
        /// </summary>
        /// <param name="sender">Parent button which triggered this event</param>
        /// <param name="e">Event arguments</param>
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            //close this window
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        /// Method triggered on close of windows settings window
        /// </summary>
        /// <param name="sender">windowSettings object that triggered this event</param>
        /// <param name="e">Event arguments</param>
        private void windowSettings_FormClosed(object sender, FormClosedEventArgs e)
        {
            overrideParent.Controls.Clear();
            overrideParent.Close();
            overrideParent = null;
        }

        /// <summary>
        /// Method triggered on load of windows settings window
        /// </summary>
        /// <param name="sender">windowSettings object that triggered this event</param>
        /// <param name="e">Event arguments</param>
        private void windowSettings_Load(object sender, EventArgs e)
        {
            //set the panel position
            panelTemplate.Left = (Width - panelTemplate.Width) / 2;
            panelTemplate.Top = (Height - panelTemplate.Height) / 2;
            //we want semi-transparent background and opaque controls - we override it with new form
            overrideParent = new Form();
            overrideParent.ShowInTaskbar = false;
            overrideParent.FormBorderStyle = FormBorderStyle.None;
            overrideParent.Opacity = 100;
            overrideParent.Size = panelTemplate.Size;
            overrideParent.StartPosition = FormStartPosition.Manual;
            overrideParent.Location = panelTemplate.PointToScreen(new Point(0, 0));
            overrideParent.Controls.Add(panelBackground);
            overrideParent.Show(panelTemplate);
            //new form contains user GUI panel
            panelBackground.Dock = DockStyle.Fill;
            overrideParent.ResumeLayout(false);
            //load my data
            loadData();
            //update window GUI
            updateGUI();
        }

        /********************************************************
         ***  WINDOWS SETTINGS - settings changed
         ********************************************************/

        /// <summary>
        /// Method triggered on change of CheckBox "show project path" check status 
        /// </summary>
        /// <param name="sender">CheckBox that triggered current event</param>
        /// <param name="e">Event arguments</param>
        private void checkShowProjPath_CheckedChanged(object sender, EventArgs e)
        {
            showCurrProject = checkShowProjPath.Checked;
        }
    }
}
