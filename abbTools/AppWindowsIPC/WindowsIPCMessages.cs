using System.Xml;
using System.Windows.Forms;
using System.Collections.Generic;

namespace abbTools.AppWindowsIPC
{
    class WindowsIPCMessages
    {
        private string messageActor;
        private string signalResultant;
        private int signalValue;

        /// <summary>
        /// Default WindowsIPCMessages constructor
        /// </summary>
        public WindowsIPCMessages()
        {
            messageActor = "";
            signalResultant = "";
            signalValue = -1;
        }

        /// <summary>
        /// Constructor with filling all private data
        /// </summary>
        /// <param name="newMessage">Message value</param>
        /// <param name="newSignal">Signal data</param>
        /// <param name="sigVal">Signal value (only '0' or '1' accepted!)</param>
        public WindowsIPCMessages(string newMessage, string newSignal, int sigVal)
        {
            messageActor = newMessage;
            signalResultant = newSignal;
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
            signalValue = org.signalValue;
        }

        public void saveToXML(ref XmlWriter xmlNode, int nodeNo)
        {
            //write start element
            xmlNode.WriteStartElement("msg_" + nodeNo.ToString());
            //message name
            xmlNode.WriteAttributeString("message", messageActor);
            //signal name
            xmlNode.WriteAttributeString("signal", signalResultant);
            //signal value (0 or 1)
            xmlNode.WriteAttributeString("value", signalValue.ToString());
            //finish element
            xmlNode.WriteEndElement();
        }

        public void loadFromXML(XmlReader xmlNode)
        {
            //only xml element nodes with attributes and "msg_" name are OK
            bool okNode = (xmlNode.NodeType == XmlNodeType.Element) && (xmlNode.Name.StartsWith("msg_"));
            if (okNode && xmlNode.HasAttributes) {
                //fill data from XML - get signal name
                messageActor = xmlNode.GetAttribute("message");
                //get actor (short attribute and convert it to useful data)
                signalResultant = xmlNode.GetAttribute("signal"); 
                //fill myAction data from sub tree node
                signalValue = int.Parse(xmlNode.GetAttribute("value"));
            }
        }

        public string message
        {
            get { return messageActor; }
        }

        public string sigName
        {
            get { return signalResultant; }
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

        public bool hasMessage(WindowsIPCMessages current)
        {
            bool result = false;
            //check if reference object exists
            if (current != null) {
                //check every collection item
                foreach (WindowsIPCMessages item in this) {
                    //check message
                    result = item.message == current.message;
                    //check signal name
                    result = result && item.sigName == current.sigName;
                    //check signal value
                    result = result && item.sigValue == current.sigValue;
                    //if current exists in collection then break with true
                    if (result) {
                        result = true;
                        break;
                    }
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
                    //change container color to white
                    container.BackColor = System.Drawing.Color.White;
                }
            }
        }

        public void saveToXML(ref XmlWriter xmlSubtree)
        {
            int itemNo = 0;
            foreach (WindowsIPCMessages saveMsg in this) {
                saveMsg.saveToXML(ref xmlSubtree, itemNo++);
            }
        }

        public void loadFromXML(ref XmlReader xmlSubtree)
        {
            //load every element in delivered XML (faster if only interesting xmlSubtree)
            while (xmlSubtree.Read()) {
                if (xmlSubtree.NodeType == XmlNodeType.Element && xmlSubtree.Name.StartsWith("msg_")) {
                    //add new element to collection and fill its data 
                    WindowsIPCMessages loadMsg = new WindowsIPCMessages();
                    loadMsg.loadFromXML(xmlSubtree);
                    Add(loadMsg);
                }
                //break from while loop if we are at end element of clientIPC
                if (xmlSubtree.NodeType == XmlNodeType.EndElement && xmlSubtree.Name.StartsWith("clientIPC")) break;
            }
        }
    }
}
