using System;

namespace abbTools.AppBackupManager
{
    /********************************************************
     ***  BACKUP MANAGER - global data
     ********************************************************/

    //robot or pc backup master 
    enum backupMaster
    {
        pc = 0,
        robot = 1
    }
    //action type when backup names duplicate
    enum sameNameAction
    {
        overwrite = 0,
        increment = 1,
        additTime = 2
    }
    //save time selection
    enum timeType
    {
        last = 0,
        exact
    }
    //backup source selection
    enum backupSource
    {
        gui = 0,
        interval,
        daily,
        robot
    }
    //interval element selection
    enum intervalElement
    {
        mins = 0,
        hours,
        days
    }

    //================================================================================================================================
    //================================================================================================================================
    //================================================================================================================================

    abstract class BackupMaster
    {
        /********************************************************
         ***  BACKUP MASTER - data
         ********************************************************/

        public DateTime lastBackupTime;
        public int duplicateMethod;
        public bool activated;

        /********************************************************
         ***  BACKUP MASTER - common methods
         ********************************************************/

        abstract public void clearData();
        abstract public string getSuffix(backupSource src);
        abstract public void saveData(ref System.Xml.XmlWriter xmlSubnode);
        abstract public void loadData(ref System.Xml.XmlReader xmlSubnode);
    }

    //================================================================================================================================
    //================================================================================================================================
    //================================================================================================================================

    class BackupMasterPC : BackupMaster
    {
        ///internal data
        private int offsMin;
        private int offsHour;
        private int offsDay;
        private string suffixGUI;
        private string suffixInterval;
        private string suffixTime;
        public DateTime exactTime;

        /// <summary>
        /// Default constructor
        /// </summary>
        public BackupMasterPC()
        {
            clearData();
        }

        /// <summary>
        /// Method used to zero all instance data
        /// </summary>
        override public void clearData()
        {
            activated = false;
            duplicateMethod = (int)sameNameAction.overwrite;
            offsMin = 0;
            offsHour = 0;
            offsDay = 0;
            suffixGUI = "";
            suffixInterval = "";
            suffixTime = "";
            exactTime = new DateTime(1, 1, 1);
            lastBackupTime = new DateTime(1, 1, 1);
        }

        /// <summary>
        /// Function used to get suffix string from desired backup source
        /// </summary>
        /// <param name="src">Backup source to get suffix from (GUI, INTERVAL or DAILY)</param>
        /// <returns>Suffix string</returns>
        override public string getSuffix(backupSource src)
        {
            string result = "";

            switch (src) {
                case backupSource.gui:
                    result = suffixGUI;
                    break;
                case backupSource.interval:
                    result = suffixInterval;
                    break;
                case backupSource.daily:
                    result = suffixTime;
                    break;
                default:
                    result = "ERROR!";
                    break;
            }

            return result;
        }

        /// <summary>
        /// Method used to save master data
        /// </summary>
        /// <param name="xmlSubnode">Subnode to save data to</param>
        override public void saveData(ref System.Xml.XmlWriter xmlSubnode)
        {
            //controller is on list - save backup pc master data
            xmlSubnode.WriteStartElement("pcMaster");
                //master active
                xmlSubnode.WriteAttributeString("active", activated.ToString());
                //last backup time
                xmlSubnode.WriteAttributeString("last", lastBackupTime.ToString());
                //duplicate method
                xmlSubnode.WriteAttributeString("same", duplicateMethod.ToString());
                //daily backup time
                xmlSubnode.WriteAttributeString("daily", exactTime.ToString());
                //save interval offset
                xmlSubnode.WriteStartElement("interval");
                    //master active
                    xmlSubnode.WriteAttributeString("days", offsDay.ToString());
                    //last backup time
                    xmlSubnode.WriteAttributeString("hours", offsHour.ToString());
                    //duplicate method
                    xmlSubnode.WriteAttributeString("mins", offsMin.ToString());
                //end client node (interval)
                xmlSubnode.WriteEndElement();
                //save interval offset
                xmlSubnode.WriteStartElement("suffix");
                    //master active
                    xmlSubnode.WriteAttributeString("gui", suffixGUI);
                    //last backup time
                    xmlSubnode.WriteAttributeString("interval", suffixInterval);
                    //duplicate method
                    xmlSubnode.WriteAttributeString("exact", suffixTime);
                //end client node (suffix)
                xmlSubnode.WriteEndElement();
            //end client node (pcMaster)
            xmlSubnode.WriteEndElement();
        }

        /// <summary>
        /// Method used to load master data
        /// </summary>
        /// <param name="xmlSubnode">Subnode to load data from</param>
        override public void loadData(ref System.Xml.XmlReader xmlSubnode)
        {
            //get robot master main data
            while (xmlSubnode.Read()) {
                bool start = xmlSubnode.NodeType == System.Xml.XmlNodeType.Element,
                     pcMaster = xmlSubnode.Name.StartsWith("pcMaster");
                //if we are starting to read client data then get it
                if (start && pcMaster) {
                    activated = bool.Parse(xmlSubnode.GetAttribute("active"));
                    lastBackupTime = DateTime.Parse(xmlSubnode.GetAttribute("last"));
                    duplicateMethod = int.Parse(xmlSubnode.GetAttribute("same"));
                    exactTime = DateTime.Parse(xmlSubnode.GetAttribute("daily"));
                    //break from WHILE loop - now will be masters data
                    break;
                }
            }
            //get robot master backup data
            while (xmlSubnode.Read()) {
                bool start = xmlSubnode.NodeType == System.Xml.XmlNodeType.Element,
                     pcInterval = xmlSubnode.Name.StartsWith("interval");
                //if we are starting to read client data then get it
                if (start && pcInterval) {
                    offsDay = int.Parse(xmlSubnode.GetAttribute("days"));
                    offsHour = int.Parse(xmlSubnode.GetAttribute("hours"));
                    offsMin = int.Parse(xmlSubnode.GetAttribute("mins"));
                    //break from WHILE loop - now will be masters data
                    break;
                }
            }
            //get robot master signal data
            while (xmlSubnode.Read()) {
                bool start = xmlSubnode.NodeType == System.Xml.XmlNodeType.Element,
                     pcSuffix = xmlSubnode.Name.StartsWith("suffix");
                //if we are starting to read client data then get it
                if (start && pcSuffix) {
                    suffixGUI = xmlSubnode.GetAttribute("gui");
                    suffixInterval = xmlSubnode.GetAttribute("interval");
                    suffixTime = xmlSubnode.GetAttribute("exact");
                    //break from WHILE loop - now will be masters data
                    break;
                }
            }
        }

        /// <summary>
        /// Method used to set interval time element in current instance
        /// </summary>
        /// <param name="el">Interval element to set</param>
        /// <param name="val">Interval element value</param>
        public void intervalSet(intervalElement el, int val)
        {
            switch (el)
            {
                case intervalElement.mins:
                    offsMin = val;
                    break;
                case intervalElement.hours:
                    offsHour = val;
                    break;
                case intervalElement.days:
                    offsDay = val;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Function used to get interval component
        /// </summary>
        /// <param name="element">Select interval time element (MINS, HOURS or DAYS)</param>
        /// <returns>Selected interval element</returns>
        public int intervalGet(intervalElement element)
        {
            int result = -1;

            switch (element)
            {
                case intervalElement.mins:
                    result = offsMin;
                    break;
                case intervalElement.hours:
                    result = offsHour;
                    break;
                case intervalElement.days:
                    result = offsDay;
                    break;
                default:
                    break;
            }

            return result;
        }

        /// <summary>
        /// Function used to check if any interval is set (!=0)
        /// </summary>
        /// <returns>TRUE if !=0 interval is set, FALSE otherwise</returns>
        public bool intervalCheck()
        {
            return (offsMin != 0 || offsHour != 0 || offsDay != 0);
        }

        /// <summary>
        /// Function used to get total interval time in minutes
        /// </summary>
        /// <returns>Total interval time in minutes</returns>
        public int intervalTimeInMins()
        {
            return offsMin + offsHour * 60 + offsDay * 24 * 60;
        }

        /// <summary>
        /// GET and SET suffix INTERVAL string
        /// </summary>
        public string intervalSuffix
        {
            get { return suffixInterval; }
            set { suffixInterval = value; }
        }

        /// <summary>
        /// GET and SET suffix GUI string
        /// </summary>
        public string guiSuffix
        {
            get { return suffixGUI; }
            set { suffixGUI = value; }
        }

        /// <summary>
        /// GET and SET suffix EXACT TIME (DAILY) string
        /// </summary>
        public string timeSuffix
        {
            get { return suffixTime; }
            set { suffixTime = value; }
        }
    }

    //================================================================================================================================
    //================================================================================================================================
    //================================================================================================================================

    class BackupMasterRobot : BackupMaster
    {
        ///internal data
        public bool checkBackup;
        private string suffixRobot;
        private string sourceDir;
        private string sigBackupExe;
        private string sigBackupInP;

        /// <summary>
        /// Default constructor
        /// </summary>
        public BackupMasterRobot()
        {
            clearData();
        }

        /// <summary>
        /// Method used to zero all instance data
        /// </summary>
        override public void clearData()
        {
            activated = false;
            duplicateMethod = (int)sameNameAction.overwrite;
            checkBackup = false;
            suffixRobot = "";
            sourceDir = "";
            sigBackupExe = "";
            sigBackupInP = "";
            lastBackupTime = new DateTime(1, 1, 1);
        }

        /// <summary>
        /// Function used to get suffix string from desired backup source
        /// </summary>
        /// <param name="src">Backup source to get suffix from (ROBOT)</param>
        /// <returns>Suffix string</returns>
        override public string getSuffix(backupSource src)
        {
            string result = "";

            switch (src) {
                case backupSource.robot:
                    result = suffixRobot;
                    break;
                default:
                    result = "ERROR!";
                    break;
            }

            return result;
        }

        /// <summary>
        /// Method used to save master data
        /// </summary>
        /// <param name="xmlSubnode">Subnode to save data to</param>
        override public void saveData(ref System.Xml.XmlWriter xmlSubnode)
        {
            //controller is on list - save backup pc master data
            xmlSubnode.WriteStartElement("robotMaster");
            //master active
            xmlSubnode.WriteAttributeString("active", activated.ToString());
            //last backup time
            xmlSubnode.WriteAttributeString("last", lastBackupTime.ToString());
            //duplicate method
            xmlSubnode.WriteAttributeString("same", duplicateMethod.ToString());
            //save interval offset
            xmlSubnode.WriteStartElement("backup");
                //master active
                xmlSubnode.WriteAttributeString("src",sourceDir);
                //last backup time
                xmlSubnode.WriteAttributeString("suffix", suffixRobot);
            //end client node (interval)
            xmlSubnode.WriteEndElement();
            //save interval offset
            xmlSubnode.WriteStartElement("signals");
                //master active
                xmlSubnode.WriteAttributeString("exe", sigBackupExe);
                //last backup time
                xmlSubnode.WriteAttributeString("doing", sigBackupInP);
            //end client node (suffix)
            xmlSubnode.WriteEndElement();
            //end client node (pcMaster)
            xmlSubnode.WriteEndElement();
        }

        /// <summary>
        /// Method used to load master data
        /// </summary>
        /// <param name="xmlSubnode">Subnode to load data from</param>
        override public void loadData(ref System.Xml.XmlReader xmlSubnode)
        {
            //get robot master main data
            while (xmlSubnode.Read()) {
                bool start = xmlSubnode.NodeType == System.Xml.XmlNodeType.Element,
                     robotMaster = xmlSubnode.Name.StartsWith("robotMaster");
                //if we are starting to read client data then get it
                if (start && robotMaster) {
                    activated = bool.Parse(xmlSubnode.GetAttribute("active"));
                    lastBackupTime = DateTime.Parse(xmlSubnode.GetAttribute("last"));
                    duplicateMethod = int.Parse(xmlSubnode.GetAttribute("same"));
                    //break from WHILE loop - now will be masters data
                    break;
                }
            }
            //get robot master backup data
            while (xmlSubnode.Read()) {
                bool start = xmlSubnode.NodeType == System.Xml.XmlNodeType.Element,
                     robotBackup = xmlSubnode.Name.StartsWith("backup");
                //if we are starting to read client data then get it
                if (start && robotBackup) {
                    sourceDir = xmlSubnode.GetAttribute("src");
                    suffixRobot = xmlSubnode.GetAttribute("suffix");
                    //break from WHILE loop - now will be masters data
                    break;
                }
            }
            //get robot master signal data
            while (xmlSubnode.Read()) {
                bool start = xmlSubnode.NodeType == System.Xml.XmlNodeType.Element,
                     robotSignals = xmlSubnode.Name.StartsWith("signals");
                //if we are starting to read client data then get it
                if (start && robotSignals) {
                    sigBackupExe = xmlSubnode.GetAttribute("exe");
                    sigBackupInP = xmlSubnode.GetAttribute("doing");
                    //break from WHILE loop - now will be masters data
                    break;
                }
            }
        }

        /// <summary>
        /// GET and SET signal name backup execute 
        /// </summary>
        public string sigExecute
        {
            get { return sigBackupExe; }
            set { sigBackupExe = value; }
        }

        /// <summary>
        /// GET and SET signal name backup in progress
        /// </summary>
        public string sigInProgress
        {
            get { return sigBackupInP; }
            set { sigBackupInP = value; }
        }

        /// <summary>
        /// GET and SET backup suffix
        /// </summary>
        public string suffix
        {
            get { return suffixRobot; }
            set { suffixRobot = value; }
        }

        /// <summary>
        /// GET and SET backup source directory (on robot)
        /// </summary>
        public string srcDir
        {
            get { return sourceDir; }
            set { sourceDir = value; }
        }
    }
}
