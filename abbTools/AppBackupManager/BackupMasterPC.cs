using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace abbTools.AppBackupManager
{
    class BackupMasterPC : BackupMasterBase
    {
        /********************************************************
         ***  BACKUP MASTER PC - DATA 
         ********************************************************/

        /// <summary>
        /// GET and SET suffix INTERVAL string
        /// </summary>
        public string suffixInterval { get; set; }

        /// <summary>
        /// GET and SET suffix GUI string
        /// </summary>
        public string suffixGUI { get; set; }

        /// <summary>
        /// GET and SET suffix EXACT TIME (DAILY) string
        /// </summary>
        public string suffixTime { get; set; }

        // <summary>
        /// GET and SET EXACT TIME (DAILY)
        /// </summary>
        public DateTime exactTime { get; set; }

        ///internal data
        private int offsMin;
        private int offsHour;
        private int offsDay;

        /********************************************************
         ***  BACKUP MASTER PC - constructor
         ********************************************************/

        /// <summary>
        /// Default constructor
        /// </summary>
        public BackupMasterPC()
        {
            clearData();
        }

        /********************************************************
         ***  BACKUP MASTER PC - data management
         ********************************************************/

        /// <summary>
        /// Method used to zero all instance data
        /// </summary>
        override public void clearData()
        {
            activated = false;
            duplicateMethod = (int)BackupSettings.duplicate.overwrite;
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
            while (xmlSubnode.Read())
            {
                bool start = xmlSubnode.NodeType == System.Xml.XmlNodeType.Element,
                     pcMaster = xmlSubnode.Name.StartsWith("pcMaster");
                //if we are starting to read client data then get it
                if (start && pcMaster)
                {
                    activated = bool.Parse(xmlSubnode.GetAttribute("active"));
                    lastBackupTime = DateTime.Parse(xmlSubnode.GetAttribute("last"));
                    duplicateMethod = int.Parse(xmlSubnode.GetAttribute("same"));
                    exactTime = DateTime.Parse(xmlSubnode.GetAttribute("daily"));
                    //break from WHILE loop - now will be masters data
                    break;
                }
            }
            //get robot master backup data
            while (xmlSubnode.Read())
            {
                bool start = xmlSubnode.NodeType == System.Xml.XmlNodeType.Element,
                     pcInterval = xmlSubnode.Name.StartsWith("interval");
                //if we are starting to read client data then get it
                if (start && pcInterval)
                {
                    offsDay = int.Parse(xmlSubnode.GetAttribute("days"));
                    offsHour = int.Parse(xmlSubnode.GetAttribute("hours"));
                    offsMin = int.Parse(xmlSubnode.GetAttribute("mins"));
                    //break from WHILE loop - now will be masters data
                    break;
                }
            }
            //get robot master signal data
            while (xmlSubnode.Read())
            {
                bool start = xmlSubnode.NodeType == System.Xml.XmlNodeType.Element,
                     pcSuffix = xmlSubnode.Name.StartsWith("suffix");
                //if we are starting to read client data then get it
                if (start && pcSuffix)
                {
                    suffixGUI = xmlSubnode.GetAttribute("gui");
                    suffixInterval = xmlSubnode.GetAttribute("interval");
                    suffixTime = xmlSubnode.GetAttribute("exact");
                    //break from WHILE loop - now will be masters data
                    break;
                }
            }
        }

        /********************************************************
         ***  BACKUP MASTER PC - intervals and suffix data
         ********************************************************/

        /// <summary>
        /// Function used to get suffix string from desired backup source
        /// </summary>
        /// <param name="src">Backup source to get suffix from (GUI, INTERVAL or DAILY)</param>
        /// <returns>Suffix string</returns>
        override public string getSuffix(BackupSettings.source src)
        {
            string result = "";

            switch (src) {
                case BackupSettings.source.gui:
                    result = suffixGUI;
                    break;
                case BackupSettings.source.interval:
                    result = suffixInterval;
                    break;
                case BackupSettings.source.daily:
                    result = suffixTime;
                    break;
                default:
                    result = "ERROR!";
                    break;
            }

            return result;
        }

        /// <summary>
        /// Method used to set interval time element in current instance
        /// </summary>
        /// <param name="element">Interval element to set</param>
        /// <param name="val">Interval element value</param>
        public void intervalSet(BackupSettings.interval element, int val)
        {
            switch (element) {
                case BackupSettings.interval.mins:
                    offsMin = val;
                    break;
                case BackupSettings.interval.hours:
                    offsHour = val;
                    break;
                case BackupSettings.interval.days:
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
        public int intervalGet(BackupSettings.interval element)
        {
            int result = -1;

            switch (element) {
                case BackupSettings.interval.mins:
                    result = offsMin;
                    break;
                case BackupSettings.interval.hours:
                    result = offsHour;
                    break;
                case BackupSettings.interval.days:
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
    }
}
