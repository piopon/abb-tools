using System.Windows.Forms;
using System.Collections.Generic;
using ABB.Robotics.Controllers.IOSystemDomain;

namespace abbTools.AppWindowsIPC
{
    class WindowsIPCMessages
    {
        private string messageActor;
        private Signal signalResultant;
        private int signalValue;
        //stored signal name if controller not exists
        private string storedSignalName;

        /// <summary>
        /// Default WindowsIPCMessages constructor
        /// </summary>
        public WindowsIPCMessages()
        {
            messageActor = "";
            signalResultant = null;
            storedSignalName = "";
            signalValue = -1;
        }

        /// <summary>
        /// Constructor with filling all private data
        /// </summary>
        /// <param name="newMessage">Message value</param>
        /// <param name="newSignal">Signal data</param>
        /// <param name="sigVal">Signal value (only '0' or '1' accepted!)</param>
        public WindowsIPCMessages(string newMessage, Signal newSignal, int sigVal)
        {
            messageActor = newMessage;
            signalResultant = newSignal;
            storedSignalName = newSignal.Name;
            signalValue = (sigVal == 0 || sigVal == 1) ? sigVal : -1;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="org">WindowsIPCMessages object to copy data from</param>
        public WindowsIPCMessages(WindowsIPCMessages org)
        {
            messageActor = org.messageActor;
            signalResultant = org.signalResultant;
            storedSignalName = org.storedSignalName;
            signalValue = org.signalValue;
        }

        public string message
        {
            get { return messageActor; }
        }

        public string sigName
        {
            get {
                if (signalResultant == null) {
                    return storedSignalName;
                } else {
                    return signalResultant.Name;
                }
            }
        }

        public int sigValue
        {
            get { return signalValue; }
        }
    }

    class WindowsIPCMessagesCollection: List<WindowsIPCMessages>
    {
        private WindowsIPCMessages currentData;

        /// <summary>
        /// Default collection constructor
        /// </summary>
        public WindowsIPCMessagesCollection()
        {
            currentData = new WindowsIPCMessages();
            //clear all data from list
            Clear();
        }

        public bool theSame(WindowsIPCMessagesCollection reference)
        {
            bool result = false;
            int currItem = 0;
            //check if reference object exists
            result = reference != null;
            //first compare sizes
            result = result && Count == reference.Count;
            //finally check every item...
            if (result) {
                foreach (WindowsIPCMessages item in this) {
                    //check message
                    result = item.message == reference[currItem].message;
                    //check signal name
                    result = result && item.sigName == reference[currItem].sigName;
                    //check signal value
                    result = result && item.sigValue == reference[currItem].sigValue;
                    //if its nok then collections are different
                    if (!result) break;
                    //its ok so increment elements no
                    currItem++;
                }
            }
            return result;
        }

        public void updateGUI(string cServerName,ListView container)
        {
            if (container != null) {
                foreach (WindowsIPCMessages item in this) {
                    ListViewItem newItem = new ListViewItem(cServerName);
                    newItem.SubItems.Add(item.message);
                    newItem.SubItems.Add(item.sigName);
                    newItem.SubItems.Add(item.sigValue.ToString());
                    newItem.Checked = false;
                    //add current item to container
                    container.Items.Add(newItem);
                }
            }
        }
    }
}
