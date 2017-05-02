using abbTools.AppBackupManager;
using System.IO;
using System.Xml;
using Xunit;

namespace abbTools.UnitTests
{
    public class AppBackupManagerTests
    {
        BackupManagerCollection _backupManagerCollection;
        string srcRobot;
        XmlReader myFile;

        public AppBackupManagerTests()
        {
            //get resource XML test file   
            string myResource = EmbeddedResource.getResource("source.xml");
            using (StringReader stream = new StringReader(myResource)) myFile = XmlReader.Create(stream);
            //update internal fields
            srcRobot = "robSpawR";
            //load data from file
            _backupManagerCollection = new BackupManagerCollection();
        }

        [Fact]
        public void got_no_data_after_constructor()
        {
            Assert.Equal(0, _backupManagerCollection.Count);
        }

        [Fact]
        public void got_correct_data_after_load()
        {
            _backupManagerCollection.loadFromXml(ref myFile, null, srcRobot);
            Assert.Equal(1, _backupManagerCollection.Count);
        }

        [Fact]
        public void got_correct_backup_item()
        {
            _backupManagerCollection.loadFromXml(ref myFile, null, srcRobot);
            BackupManager myItem = _backupManagerCollection.itemGet(null, srcRobot);
            Assert.Equal(srcRobot, myItem.controllerName);
        }

        [Fact]
        public void got_correct_masters_active()
        {
            _backupManagerCollection.loadFromXml(ref myFile, null, srcRobot);
            BackupManager myItem = _backupManagerCollection.itemGet(null, srcRobot);
            //check PC master active
            Assert.Equal(true, myItem.pcMasterActive);
            //check ROBOT master active
            Assert.Equal(true, myItem.robotMasterActive);
        }

        [Fact]
        public void got_correct_masters_change()
        {
            _backupManagerCollection.loadFromXml(ref myFile, null, srcRobot);
            BackupManager myItem = _backupManagerCollection.itemGet(null, srcRobot);
            //change masters settings
            myItem.activateMaster(false, true);
            //check PC master active
            Assert.Equal(false, myItem.pcMasterActive);
            //check ROBOT master active
            Assert.Equal(true, myItem.robotMasterActive);
            //change masters settings
            myItem.activateMaster(true, false);
            //check PC master active
            Assert.Equal(true, myItem.pcMasterActive);
            //check ROBOT master active
            Assert.Equal(false, myItem.robotMasterActive);
        }

        [Fact]
        public void got_correct_pc_suffixes()
        {
            _backupManagerCollection.loadFromXml(ref myFile, null, srcRobot);
            BackupManager myItem = _backupManagerCollection.itemGet(null, srcRobot);
            //check suffix: PC DAILY TIME
            Assert.Equal("_dailyBackup", myItem.pcDailySuffix);
            //check suffix: PC GUI TIME
            Assert.Equal("_gui", myItem.pcGUISuffix);
            //check suffix: PC INTERVAL TIME
            Assert.Equal("_autoBackup", myItem.pcIntervalSuffix);
        }

        [Fact]
        public void got_correct_robot_suffixes()
        {
            _backupManagerCollection.loadFromXml(ref myFile, null, srcRobot);
            BackupManager myItem = _backupManagerCollection.itemGet(null, srcRobot);
            //check suffix: ROBOT SUFFIX
            Assert.Equal("_robot", myItem.robotDirSuffix);
        }

        [Fact]
        public void got_correct_robot_signals()
        {
            _backupManagerCollection.loadFromXml(ref myFile, null, srcRobot);
            BackupManager myItem = _backupManagerCollection.itemGet(null, srcRobot);
            //check signal: DO BACKUP
            Assert.Equal("camPower", myItem.robotSignalExe);
            //check signal: BACKUP IN PROGRESS
            Assert.Equal("camTrigger", myItem.robotSignalInP);
        }

        [Fact]
        public void got_correct_robot_source_dir()
        {
            _backupManagerCollection.loadFromXml(ref myFile, null, srcRobot);
            BackupManager myItem = _backupManagerCollection.itemGet(null, srcRobot);
            //check backup source dir
            Assert.Equal("BACKUP", myItem.robotDirSrc);
        }

        [Fact]
        public void got_correct_pc_interval_total()
        {
            _backupManagerCollection.loadFromXml(ref myFile, null, srcRobot);
            BackupManager myItem = _backupManagerCollection.itemGet(null, srcRobot);
            //check total interval minutes
            Assert.Equal(1621, myItem.pcIntervalInMins);
        }

        [Fact]
        public void got_correct_pc_interval_elements()
        {
            _backupManagerCollection.loadFromXml(ref myFile, null, srcRobot);
            BackupManager myItem = _backupManagerCollection.itemGet(null, srcRobot);
            //check interval DAYS
            Assert.Equal(1,myItem.pcIntervalGet(intervalElement.days));
            //check interval HOURS
            Assert.Equal(3, myItem.pcIntervalGet(intervalElement.hours));
            //check interval MINS
            Assert.Equal(1, myItem.pcIntervalGet(intervalElement.mins));
        }

        [Fact]
        public void set_correct_pc_interval_elements()
        {
            _backupManagerCollection.loadFromXml(ref myFile, null, srcRobot);
            BackupManager myItem = _backupManagerCollection.itemGet(null, srcRobot);
            //set interval data
            myItem.pcIntervalSet(intervalElement.days, 3);
            myItem.pcIntervalSet(intervalElement.hours, 5);
            myItem.pcIntervalSet(intervalElement.mins, 57);
            //check if set data is OK
            Assert.Equal(4677, myItem.pcIntervalInMins);
        }

        [Fact]
        public void got_correct_pc_interval_check()
        {
            _backupManagerCollection.loadFromXml(ref myFile, null, srcRobot);
            BackupManager myItem = _backupManagerCollection.itemGet(null, srcRobot);
            //set interval data
            Assert.Equal(true, myItem.pcIntervalCheck());
        }

        [Fact]
        public void got_correct_duplicated_methods()
        {
            _backupManagerCollection.loadFromXml(ref myFile, null, srcRobot);
            BackupManager myItem = _backupManagerCollection.itemGet(null, srcRobot);
            //get duplicate method PC
            Assert.Equal(1, myItem.duplicateMethodPC);
            //get duplicate method ROBOT
            Assert.Equal(3, myItem.duplicateMethodRobot);
        }

        [Fact]
        public void got_correct_clear_days()
        {
            _backupManagerCollection.loadFromXml(ref myFile, null, srcRobot);
            BackupManager myItem = _backupManagerCollection.itemGet(null, srcRobot);
            //get clear days value
            Assert.Equal(30, myItem.clearDays);
        }

        [Fact]
        public void got_correct_output_dir()
        {
            _backupManagerCollection.loadFromXml(ref myFile, null, srcRobot);
            BackupManager myItem = _backupManagerCollection.itemGet(null, srcRobot);
            //get clear days value
            Assert.Equal("C:\\Users\\pponikowski\\Desktop\\test", myItem.outputDir);
        }

        [Fact]
        public void got_correct_watch_status()
        {
            _backupManagerCollection.loadFromXml(ref myFile, null, srcRobot);
            BackupManager myItem = _backupManagerCollection.itemGet(null, srcRobot);
            //get clear days value
            Assert.Equal(true, myItem.timer);
        }

        [Fact]
        public void got_correct_times()
        {
            _backupManagerCollection.loadFromXml(ref myFile, null, srcRobot);
            BackupManager myItem = _backupManagerCollection.itemGet(null, srcRobot);
            //check if time exists: PC EXACT
            Assert.Equal(true, myItem.timeExists(backupMaster.pc,timeType.exact));
            //check if time exists: PC LAST
            Assert.Equal(true, myItem.timeExists(backupMaster.pc, timeType.last));
            //check if time exists: ROBOT LAST
            Assert.Equal(true, myItem.timeExists(backupMaster.robot, timeType.last));
        }

        [Fact]
        public void clear_backup_data()
        {
            _backupManagerCollection.loadFromXml(ref myFile, null, srcRobot);
            BackupManager myItem = _backupManagerCollection.itemGet(null, srcRobot);
            //check if time exists: PC EXACT
            myItem.clearData();
            //check random data
            Assert.Equal(false, myItem.timeExists(backupMaster.pc, timeType.exact));
            Assert.Equal(0, myItem.clearDays);
            Assert.Equal("", myItem.outputDir);
            Assert.Equal(0, myItem.pcIntervalInMins);
        }

        [Fact]
        public void clear_collection()
        {
            _backupManagerCollection.loadFromXml(ref myFile, null, srcRobot);
            //check if time exists: PC EXACT
            _backupManagerCollection.clear();
            Assert.Empty(_backupManagerCollection);
        }
    }
}
