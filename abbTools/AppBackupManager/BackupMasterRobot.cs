using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace abbTools.AppBackupManager
{
    class BackupMasterRobot : BackupMasterBase
    {
        /// <summary>
        /// GET and SET signal name backup execute 
        /// </summary>
        public string sigBackupExe { get; set; }

        /// <summary>
        /// GET and SET signal name backup in progress
        /// </summary>
        public string sigBackupInP { get; set; }

        /// <summary>
        /// GET and SET backup suffix
        /// </summary>
        public string suffix { get; set; }

        /// <summary>
        /// GET and SET backup source directory (on robot)
        /// </summary>
        public string sourceDir { get; set; }

        /// <summary>
        /// GET and SET info about if check backup state
        /// </summary>
        public bool checkBackup { get; set; }

        /********************************************************
         ***  BACKUP MASTER ROBOT - constructor
         ********************************************************/

        /// <summary>
        /// Default constructor
        /// </summary>
        public BackupMasterRobot()
        {
            clearData();
        }

        /********************************************************
         ***  BACKUP MASTER ROBOT - data management
         ********************************************************/

        /// <summary>
        /// Method used to zero all instance data
        /// </summary>
        override public void clearData()
        {
            activated = false;
            duplicateMethod = (int)BackupSettings.duplicate.overwrite;
            checkBackup = false;
            suffix = "";
            sourceDir = "";
            sigBackupExe = "";
            sigBackupInP = "";
            lastBackupTime = new DateTime(1, 1, 1);
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
            xmlSubnode.WriteAttributeString("src", sourceDir);
            //last backup time
            xmlSubnode.WriteAttributeString("suffix", suffix);
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
            while (xmlSubnode.Read())
            {
                bool start = xmlSubnode.NodeType == System.Xml.XmlNodeType.Element,
                     robotMaster = xmlSubnode.Name.StartsWith("robotMaster");
                //if we are starting to read client data then get it
                if (start && robotMaster)
                {
                    activated = bool.Parse(xmlSubnode.GetAttribute("active"));
                    lastBackupTime = DateTime.Parse(xmlSubnode.GetAttribute("last"));
                    duplicateMethod = int.Parse(xmlSubnode.GetAttribute("same"));
                    //break from WHILE loop - now will be masters data
                    break;
                }
            }
            //get robot master backup data
            while (xmlSubnode.Read())
            {
                bool start = xmlSubnode.NodeType == System.Xml.XmlNodeType.Element,
                     robotBackup = xmlSubnode.Name.StartsWith("backup");
                //if we are starting to read client data then get it
                if (start && robotBackup)
                {
                    sourceDir = xmlSubnode.GetAttribute("src");
                    suffix = xmlSubnode.GetAttribute("suffix");
                    //break from WHILE loop - now will be masters data
                    break;
                }
            }
            //get robot master signal data
            while (xmlSubnode.Read())
            {
                bool start = xmlSubnode.NodeType == System.Xml.XmlNodeType.Element,
                     robotSignals = xmlSubnode.Name.StartsWith("signals");
                //if we are starting to read client data then get it
                if (start && robotSignals)
                {
                    sigBackupExe = xmlSubnode.GetAttribute("exe");
                    sigBackupInP = xmlSubnode.GetAttribute("doing");
                    //break from WHILE loop - now will be masters data
                    break;
                }
            }
        }

        /// <summary>
        /// Function used to get suffix string from desired backup source
        /// </summary>
        /// <param name="src">Backup source to get suffix from (ROBOT)</param>
        /// <returns>Suffix string</returns>
        override public string getSuffix(BackupSettings.source src)
        {
            string result = "";

            switch (src) {
                case BackupSettings.source.robot:
                    result = suffix;
                    break;
                default:
                    result = "ERROR!";
                    break;
            }

            return result;
        }
    }
}
