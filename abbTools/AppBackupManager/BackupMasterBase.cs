using System;

namespace abbTools.AppBackupManager
{
    abstract class BackupMasterBase
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
        abstract public string getSuffix(BackupSettings.source src);
        abstract public void saveData(ref System.Xml.XmlWriter xmlSubnode);
        abstract public void loadData(ref System.Xml.XmlReader xmlSubnode);
    }
}
