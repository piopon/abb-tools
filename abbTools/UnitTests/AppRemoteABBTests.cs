using abbTools.AppRemoteABB;
using System.IO;
using System.Xml;
using Xunit;

namespace abbTools.UnitTests
{
    public class AppRemoteABBTests
    {
        RemoteABBCollection _remoteABBCollection;
        string srcRobot;
        XmlReader myFile;

        public AppRemoteABBTests()
        {
            //get resource XML test file   
            string myResource = EmbeddedResource.getResource("source.xml");
            using (StringReader stream = new StringReader(myResource)) myFile = XmlReader.Create(stream);
            //update internal fields
            srcRobot = "robSpawR";
            //load data from file
            _remoteABBCollection = new RemoteABBCollection();
        }

        [Fact]
        public void got_no_data_after_constructor()
        {
            Assert.Equal(0, _remoteABBCollection.Count);
        }

        [Fact]
        public void got_correct_data_after_load()
        {
            _remoteABBCollection.loadFromXml(ref myFile, null, srcRobot);
            Assert.Equal(1, _remoteABBCollection.Count);
        }

        [Fact]
        public void got_correct_signal_data_after_load()
        {
            _remoteABBCollection.loadFromXml(ref myFile, null, srcRobot);
            RemoteSignalCollection mySignals = _remoteABBCollection.getCurrSignals();
            Assert.Equal(2, mySignals.Count);
        }

        [Fact]
        public void got_correct_signal_names_after_load()
        {
            _remoteABBCollection.loadFromXml(ref myFile, null, srcRobot);
            RemoteSignalCollection mySignals = _remoteABBCollection.getCurrSignals();
            Assert.Equal("czujnik_H", mySignals[0].signalStoreName);
            Assert.Equal("spawanie", mySignals[1].signalStoreName);
        }

        [Fact]
        public void got_correct_signal_names_after_change()
        {
            _remoteABBCollection.loadFromXml(ref myFile, null, srcRobot);
            RemoteSignalCollection mySignals = _remoteABBCollection.getCurrSignals();
            Assert.Equal("czujnik_H", mySignals[0].signalStoreName);
            mySignals[0].signalStoreName = "czujnik_H2O";
            Assert.Equal("czujnik_H2O", mySignals[0].signalStoreName);
        }

        [Fact]
        public void got_correct_signal_data_after_remove()
        {
            _remoteABBCollection.loadFromXml(ref myFile, null, srcRobot);
            RemoteSignalCollection mySignals = _remoteABBCollection.getCurrSignals();
            mySignals.RemoveAt(1);
            Assert.Equal(1, mySignals.Count);
        }

        [Fact]
        public void got_correct_actors_after_load()
        {
            _remoteABBCollection.loadFromXml(ref myFile, null, srcRobot);
            RemoteSignalCollection mySignals = _remoteABBCollection.getCurrSignals();
            Assert.Equal(0, mySignals[0].triggerValue);
            Assert.Equal(1, mySignals[1].triggerValue);
        }

        [Fact]
        public void got_correct_resultant_after_load()
        {
            _remoteABBCollection.loadFromXml(ref myFile, null, srcRobot);
            RemoteSignalCollection mySignals = _remoteABBCollection.getCurrSignals();
            Assert.Equal(1, (int)mySignals[0].action.resultant);
            Assert.Equal(0, (int)mySignals[1].action.resultant);
        }

        [Fact]
        public void got_correct_modifier_after_load()
        {
            _remoteABBCollection.loadFromXml(ref myFile, null, srcRobot);
            RemoteSignalCollection mySignals = _remoteABBCollection.getCurrSignals();
            Assert.Empty(mySignals[0].action.modifier);
            Assert.Equal("TEST", mySignals[1].action.modifier);
        }

        [Fact]
        public void got_correct_app_after_load()
        {
            _remoteABBCollection.loadFromXml(ref myFile, null, srcRobot);
            RemoteSignalCollection mySignals = _remoteABBCollection.getCurrSignals();
            Assert.Equal("C:\\powershell_ise.exe", mySignals[0].action.appPath);
            Assert.Equal("C:\\Windows\\notepad.exe", mySignals[1].action.appPath);
        }
    }
}
