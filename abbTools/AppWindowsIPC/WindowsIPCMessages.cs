using System.Xml;
using System.Windows.Forms;
using System.Collections.Generic;

namespace abbTools.AppWindowsIPC
{
    class WindowsIPCMessages
    {
        /********************************************************
         ***  WINDOWS IPC MESSAGES - data 
         ********************************************************/

        /// <summary>
        /// class field / property - actor messages
        /// </summary>
        public string messageActor { get; set; }

        /// <summary>
        /// class field / property - signal resultant
        /// </summary>
        public string signalResult { get; set; }

        /// <summary>
        /// class field / property - signal value
        /// </summary>
        public int signalValue { get; set; }

        /********************************************************
         ***  WINDOWS IPC MESSAGES - constructors
         ********************************************************/

        /// <summary>
        /// Default WindowsIPCMessages constructor
        /// </summary>
        public WindowsIPCMessages()
        {
            messageActor = "";
            signalResult = "";
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
            signalResult = newSignal;
            signalValue = (sigVal == 0 || sigVal == 1) ? sigVal : -1;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="org">WindowsIPCMessages object to copy data from</param>
        public WindowsIPCMessages(WindowsIPCMessages org)
        {
            messageActor = org.messageActor;
            signalResult = org.signalResult;
            signalValue = org.signalValue;
        }

        /********************************************************
         ***  WINDOWS IPC MESSAGES - data management
         ********************************************************/

        /// <summary>
        /// Method used to save collection data in XML file
        /// </summary>
        /// <param name="xmlNode">XML node to save data to</param>
        /// <param name="nodeNo">Node number to save</param>
        public void saveToXML(ref XmlWriter xmlNode, int nodeNo)
        {
            //write start element
            xmlNode.WriteStartElement("msg_" + nodeNo.ToString());
            //message name
            xmlNode.WriteAttributeString("message", messageActor);
            //signal name
            xmlNode.WriteAttributeString("signal", signalResult);
            //signal value (0 or 1)
            xmlNode.WriteAttributeString("value", signalValue.ToString());
            //finish element
            xmlNode.WriteEndElement();
        }

        /// <summary>
        /// Method used to load collection data from XML file
        /// </summary>
        /// <param name="xmlNode">XML subnode to load data from</param>
        public void loadFromXML(XmlReader xmlNode)
        {
            //only xml element nodes with attributes and "msg_" name are OK
            bool okNode = (xmlNode.NodeType == XmlNodeType.Element) && (xmlNode.Name.StartsWith("msg_"));
            if (okNode && xmlNode.HasAttributes) {
                //fill data from XML - get signal name
                messageActor = xmlNode.GetAttribute("message");
                //get actor (short attribute and convert it to useful data)
                signalResult = xmlNode.GetAttribute("signal"); 
                //fill myAction data from sub tree node
                signalValue = int.Parse(xmlNode.GetAttribute("value"));
            }
        }
    }

    //================================================================================================================================
    //================================================================================================================================
    //================================================================================================================================

    class WindowsIPCMessagesCollection : List<WindowsIPCMessages>
    {
        /********************************************************
         ***  WINDOWS IPC MESSAGES COLLECTION - fields 
         ********************************************************/

        private WindowsIPCMessages currentData;

        /********************************************************
         ***  WINDOWS IPC MESSAGES COLLECTION - constructor
         ********************************************************/

        /// <summary>
        /// Default collection constructor
        /// </summary>
        public WindowsIPCMessagesCollection()
        {
            currentData = new WindowsIPCMessages();
            //clear all data from list
            Clear();
        }

        /********************************************************
         ***  WINDOWS IPC MESSAGES COLLECTION - data management
         ********************************************************/

        /// <summary>
        /// Function used to check if inputted message is in collection
        /// </summary>
        /// <param name="current">Message to check</param>
        /// <returns>TRUE if message is in collection, FALSE otherwise</returns>
        public bool hasMessage(WindowsIPCMessages current)
        {
            bool result = false;
            //check if reference object exists
            if (current != null) {
                //check every collection item
                foreach (WindowsIPCMessages item in this) {
                    //check message
                    result = item.messageActor == current.messageActor;
                    //check signal name
                    result = result && item.signalResult == current.signalResult;
                    //check signal value
                    result = result && item.signalValue == current.signalValue;
                    //if current exists in collection then break with true
                    if (result) {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Method used to save collection data in XML file
        /// </summary>
        /// <param name="xmlSubtree">XML file to save data to</param>
        public void saveToXML(ref XmlWriter xmlSubtree)
        {
            int itemNo = 0;
            foreach (WindowsIPCMessages saveMsg in this) {
                saveMsg.saveToXML(ref xmlSubtree, itemNo++);
            }
        }

        /// <summary>
        /// Method used to load collection data from XML file
        /// </summary>
        /// <param name="xmlSubtree">XML subnode to load data from</param>
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

        /********************************************************
         ***  WINDOWS IPC MESSAGES COLLECTION - GUI
         ********************************************************/

        /// <summary>
        /// Method used to update GUI wit collection data
        /// </summary>
        /// <param name="cServerName">Server name to update GUI list</param>
        /// <param name="container">ListView which contains collection data</param>
        public void updateGUI(string cServerName, ListView container)
        {
            //check if container is correct
            if (container != null) {
                //scan all collection items and put them into list
                foreach (WindowsIPCMessages item in this) {
                    //generate list view item add fill it with data
                    ListViewItem newItem = new ListViewItem(cServerName);
                    newItem.SubItems.Add(item.messageActor);
                    newItem.SubItems.Add(item.signalResult);
                    newItem.SubItems.Add(item.signalValue.ToString());
                    newItem.Checked = false;
                    //add current item to container
                    container.Items.Add(newItem);
                    //change container color to white
                    container.BackColor = System.Drawing.Color.White;
                }
            }
        }
    }
}
