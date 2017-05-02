using abbTools.AppWindowsIPC;
using System.IO;
using System.Xml;
using Xunit;

namespace abbTools.UnitTests
{
    public class AppWindowsIPCTests
    {
        WindowsIPCCollection _windowsIPCCollection;
        string srcRobot;
        XmlReader myFile;

        public AppWindowsIPCTests()
        {
            //get resource XML test file   
            string myResource = EmbeddedResource.getResource("source.xml");
            using (StringReader stream = new StringReader(myResource)) myFile = XmlReader.Create(stream);
            //update internal fields
            srcRobot = "robSpawR";
            //load data from file
            _windowsIPCCollection = new WindowsIPCCollection();
        }

        [Fact]
        public void got_no_data_after_constructor()
        {
            Assert.Equal(0, _windowsIPCCollection.Count);
        }

        [Fact]
        public void got_correct_data_after_load()
        {
            _windowsIPCCollection.loadFromXml(ref myFile, null, srcRobot);
            Assert.Equal(1, _windowsIPCCollection.Count);
        }

        [Fact]
        public void got_correct_robot_index()
        {
            _windowsIPCCollection.loadFromXml(ref myFile, null, srcRobot);
            Assert.Equal(0, _windowsIPCCollection.controllerIndex(srcRobot));
        }

        [Fact]
        public void got_correct_server_name()
        {
            _windowsIPCCollection.loadFromXml(ref myFile, null, srcRobot);
            int controllerIndex = _windowsIPCCollection.controllerIndex(srcRobot);
            string serverName = _windowsIPCCollection[controllerIndex].ipcClient.server;
            Assert.Equal("abc", serverName);
        }

        [Fact]
        public void got_correct_server_auto_start()
        {
            _windowsIPCCollection.loadFromXml(ref myFile, null, srcRobot);
            int controllerIndex = _windowsIPCCollection.controllerIndex(srcRobot);
            bool autoOpen = _windowsIPCCollection[controllerIndex].ipcClient.autoOpen;
            Assert.Equal(false, autoOpen);
        }

        [Fact]
        public void got_correct_server_auto_recon()
        {
            _windowsIPCCollection.loadFromXml(ref myFile, null, srcRobot);
            int controllerIndex = _windowsIPCCollection.controllerIndex(srcRobot);
            bool autoOpen = _windowsIPCCollection[controllerIndex].ipcClient.autoRecon;
            Assert.Equal(true, autoOpen);
        }

        [Fact]
        public void got_correct_conn_status()
        {
            _windowsIPCCollection.loadFromXml(ref myFile, null, srcRobot);
            int controllerIndex = _windowsIPCCollection.controllerIndex(srcRobot);
            Assert.Equal(false, _windowsIPCCollection[controllerIndex].controllerConnected);
        }

        [Fact]
        public void got_correct_message_data_count()
        {
            _windowsIPCCollection.loadFromXml(ref myFile, null, srcRobot);
            int count = _windowsIPCCollection[_windowsIPCCollection.controllerIndex(srcRobot)].messagesCount();
            Assert.Equal(2, count);
        }

        [Fact]
        public void got_correct_message_data_index()
        {
            _windowsIPCCollection.loadFromXml(ref myFile, null, srcRobot);
            int index = _windowsIPCCollection[_windowsIPCCollection.controllerIndex(srcRobot)].messageIndex("START");
            Assert.Equal(0, index);
        }

        [Fact]
        public void got_correct_message_data_after_clear()
        {
            _windowsIPCCollection.loadFromXml(ref myFile, null, srcRobot);
            _windowsIPCCollection[_windowsIPCCollection.controllerIndex(srcRobot)].messagesClear();
            Assert.Equal(0, _windowsIPCCollection[_windowsIPCCollection.controllerIndex(srcRobot)].messagesCount());
        }

        [Fact]
        public void got_correct_message_data_after_remove()
        {
            _windowsIPCCollection.loadFromXml(ref myFile, null, srcRobot);
            _windowsIPCCollection[_windowsIPCCollection.controllerIndex(srcRobot)].messageRemove(1);
            Assert.Equal(1, _windowsIPCCollection[_windowsIPCCollection.controllerIndex(srcRobot)].messagesCount());
        }

        [Fact]
        public void got_correct_message_data_after_add()
        {
            _windowsIPCCollection.loadFromXml(ref myFile, null, srcRobot);
            _windowsIPCCollection[_windowsIPCCollection.controllerIndex(srcRobot)].messageAdd(new WindowsIPCMessages("TEST", "sig1", 1));
            Assert.Equal(3, _windowsIPCCollection[_windowsIPCCollection.controllerIndex(srcRobot)].messagesCount());
            //check if message was added
            Assert.Equal("TEST", _windowsIPCCollection[0].getMessageAction(2).messageActor);
            Assert.Equal("sig1", _windowsIPCCollection[0].getMessageAction(2).signalResult);
            Assert.Equal(1, _windowsIPCCollection[0].getMessageAction(2).signalValue);
        }

        [Fact]
        public void got_correct_message_data_after_update()
        {
            _windowsIPCCollection.loadFromXml(ref myFile, null, srcRobot);
            _windowsIPCCollection[_windowsIPCCollection.controllerIndex(srcRobot)].messageUpdate(0, new WindowsIPCMessages("TEST", "signal", 0));
            //check if message was updated
            Assert.Equal("TEST", _windowsIPCCollection[0].getMessageAction(0).messageActor);
            Assert.Equal("signal", _windowsIPCCollection[0].getMessageAction(0).signalResult);
            Assert.Equal(0, _windowsIPCCollection[0].getMessageAction(0).signalValue);
        }

        [Fact]
        public void got_correct_signals_after_load()
        {
            _windowsIPCCollection.loadFromXml(ref myFile, null, srcRobot);
            Assert.Equal("RRM_airPresent", _windowsIPCCollection[0].getMessageAction(0).signalResult);
            Assert.Equal("RRM_airPresent2", _windowsIPCCollection[0].getMessageAction(1).signalResult);
        }

        [Fact]
        public void got_correct_signals_after_update()
        {
            _windowsIPCCollection.loadFromXml(ref myFile, null, srcRobot);
            _windowsIPCCollection[_windowsIPCCollection.controllerIndex(srcRobot)].messageUpdate(0, new WindowsIPCMessages("TEST", "mySignal", 1));
            Assert.Equal("mySignal", _windowsIPCCollection[0].getMessageAction(0).signalResult);
        }

        [Fact]
        public void got_correct_values_after_load()
        {
            _windowsIPCCollection.loadFromXml(ref myFile, null, srcRobot);
            Assert.Equal(1, _windowsIPCCollection[0].getMessageAction(0).signalValue);
            Assert.Equal(0, _windowsIPCCollection[0].getMessageAction(1).signalValue);
        }

        [Fact]
        public void got_correct_values_after_update()
        {
            _windowsIPCCollection.loadFromXml(ref myFile, null, srcRobot);
            _windowsIPCCollection[_windowsIPCCollection.controllerIndex(srcRobot)].messageUpdate(0, new WindowsIPCMessages("TEST", "mySignal", 0));
            Assert.Equal(0, _windowsIPCCollection[0].getMessageAction(0).signalValue);
        }
    }
}
