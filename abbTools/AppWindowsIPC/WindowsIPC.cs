using System.Windows.Forms;
using System.Collections.Generic;
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.IOSystemDomain;

namespace abbTools.AppWindowsIPC
{
    //private enumerator for finding data type
    public enum find
    {
        exact = 0,
        controller,
        server,
        message
    }

    class WindowsIPC
    {
        private Controller myController;
        private WindowsIPCClient myClient;
        private WindowsIPCMessagesCollection myData;
        private loggerABB myLogger;
        //backup controller name field (when controller not visible in network)
        private string storedNameController;

        /********************************************************
         ***  IPC DATA - public methods
         ********************************************************/

        /// <summary>
        /// Default constructor;
        /// </summary>
        public WindowsIPC()
        {
            myController = null;
            storedNameController = "";
            myClient = null;
            myData = new WindowsIPCMessagesCollection();
            myLogger = null;
        }

        /// <summary>
        /// Constructor filling ABB controller data
        /// </summary>
        /// <param name="newController">ABB controller</param>
        public WindowsIPC(Controller newController)
        {
            myController = newController;
            storedNameController = newController.SystemName;
            myClient = null;
            myData = new WindowsIPCMessagesCollection();
            myLogger = null;
        }

        /// <summary>
        /// Constructor filling private data
        /// </summary>
        /// <param name="newController">ABB controller</param>
        /// <param name="newClient">IPC (named-pipe) client</param>
        /// <param name="newLogger">ABB logger address</param>
        public WindowsIPC(Controller newController, WindowsIPCClient newClient, loggerABB newLogger)
        {
            myController = newController;
            storedNameController = newController.SystemName;
            myClient = newClient;
            myData = new WindowsIPCMessagesCollection();
            myLogger = newLogger;
        }

        /// <summary>
        /// Function used to check if this object is the same as inputted
        /// </summary>
        /// <param name="reference">WindowsIPC object to compare to</param>
        /// <returns>TRUE if object the same, FALSE otherwise</returns>
        public bool theSame(WindowsIPC reference, find search)
        {
            bool result = false;
            //check if reference exists
            result = reference != null;
            //check controller name
            if (search == find.controller || search == find.exact)
                result = result && myController.SystemName == reference.myController.SystemName;
            //check clients server name
            if (search == find.server || search == find.exact)
                result = result && myClient.serverName == reference.myClient.serverName;
            //check messages
            if (search == find.message || search == find.exact)
                result = result && myData.hasMessage(reference.myData[0]);

            return result;
        }

        public void clearMessages()
        {
            myData.Clear();
        }

        public void removeMessage(int index)
        {
            myData.RemoveAt(index);
        }

        public int countMessages()
        {
            return myData.Count;
        }

        public void addMessageAction(string message, Signal sigData, int sigValue)
        {
            myData.Add(new WindowsIPCMessages(message, sigData, sigValue));
        }

        public void addMessageAction(WindowsIPCMessages messageAction)
        {
            myData.Add(messageAction);
        }

        public void updateMessageAction(int index, WindowsIPCMessages updMessageAction)
        {
            myData[index] = updMessageAction;
        }

        public WindowsIPCMessages getMessageAction(int index)
        {
            return myData[index];
        }

        public int findMessageIndex(string message)
        {
            int result;

            for (result = 0; result < myData.Count; result++) {
                if (myData[result].message == message) {
                    break;
                }
            }
            result = result >= myData.Count ? -1 : result;

            return result;
        }

        public void executeAction(int index)
        {

        }

        public void saveMessages(ref System.Xml.XmlWriter xmlSubtree)
        {
            MessageBox.Show("TODO: saveMessages");
        }

        public void loadMessages(ref System.Xml.XmlReader xmlSubtree, Controller parent = null)
        {
            MessageBox.Show("TODO: loadMessages");
        }

        public void updateGUI(ListView container)
        {
            myData.updateGUI(serverName, container);
        }

        public Controller controller
        {
            get { return myController; }
        }

        public string controllerName
        {
            get {
                if (myController == null) {
                    return storedNameController;
                } else {
                    return myController.SystemName;
                }
            }
        }

        public string serverName
        {
            get { return myClient.serverName; }
        }
    }

    class WindowsIPCCollection : List<WindowsIPC>
    {
        //current edited data
        private WindowsIPC currentData;
        private loggerABB defaultLogger;
        //private container for showing data in GUI
        private ListView myContainer;

        /********************************************************
         ***  IPC DATA COLLECTION - public methods
         ********************************************************/

        /// <summary>
        /// Default constructor
        /// </summary>
        public WindowsIPCCollection()
        {
            //initialize current data element
            currentData = new WindowsIPC();
            defaultLogger = null;
            myContainer = null;
            //clear all elements
            Clear();
        }

        /// <summary>
        /// Constructor with fill logger data
        /// </summary>
        /// <param name="newLogger">New logger data</param>
        public WindowsIPCCollection(loggerABB newLogger)
        {
            //initialize current data element
            currentData = new WindowsIPC();
            defaultLogger = newLogger;
            myContainer = null;
            //clear all elements
            Clear();
        }

        /// <summary>
        /// Connect collection data with logger to output statuses
        /// </summary>
        /// <param name="logger">loggerABB for showing status</param>
        public void connectLogger(loggerABB logger)
        {
            defaultLogger = logger;
        }

        public void disconnectLogger()
        {
            defaultLogger = null;
        }

        /// <summary>
        /// Connect collection data with GUI ListView container
        /// </summary>
        /// <param name="container">ListView item showing collection data for user</param>
        public void connectContainerGUI(ListView container)
        {
            myContainer = container;
        }

        public void disconnectContainerGUI()
        {
            myContainer = null;
        }

        /// <summary>
        /// Check if inputted item exists in collection
        /// </summary>
        /// <param name="cItem">Item to check</param>
        /// <returns>TRUE if item exists, FALSE otherwise</returns>
        public bool itemExists(WindowsIPC cItem)
        {
            return itemIndex(cItem) != -1;
        }

        /// <summary>
        /// Add inputted item to collection (internally checking no duplicates)
        /// </summary>
        /// <param name="cItem">Item to add to collection (checking no duplicates)</param>
        public void itemAdd(WindowsIPC cItem)
        {
            //check if current element exists in collection
            if (itemIndex(cItem) == -1) {
                //no element in collection - add it to parent controller and clientIPC
                int cController = itemIndex(cItem, find.controller);
                if (cController == -1) {
                    //new controller - add item
                    Add(cItem);
                    //log success data
                    defaultLogger.writeLog(logType.info, "Added message to watch list!");
                    currentData = cItem;
                } else {
                    //controller exists - check if clients server name is the same (MUST BE)
                    if (this[cController].serverName == cItem.serverName) {
                        //check if current message doesnt exist
                        if (itemIndex(cItem, find.message) == -1) {
                            //add message to current item
                            this[cController].addMessageAction(cItem.getMessageAction(0));
                            //log success data
                            defaultLogger.writeLog(logType.info, "Added message to watch list!");
                            currentData = this[cController];
                        } else {
                            //message exists - no duplicates
                            defaultLogger.writeLog(logType.warning, "Current message exists!");
                        }
                    } else {
                        defaultLogger.writeLog(logType.error, "Different server name! Each robot (client) must have only one server!");
                    }
                }
                //update GUI component containing data
                updateContainerGUI();
            } else {
                defaultLogger.writeLog(logType.warning, "Current element exists in collection!");
            }
        }

        public void itemModify(WindowsIPC oldItem, WindowsIPC newItem)
        {
            //check if old item exists in collection 
            int oldIndex = itemIndex(oldItem);
            if (oldIndex != -1) {
                //check if new item doesnt exist in collection
                if (itemIndex(newItem) == -1) {
                    //old item is in collection and new one is not - all OK!
                    if(oldItem.controllerName == newItem.controllerName) {
                        //its the same controller - change inside it
                        int cController = itemIndex(oldItem, find.controller);
                        if (cController != -1) {
                            int msgIndex = this[cController].findMessageIndex(oldItem.getMessageAction(0).message);
                            this[cController].updateMessageAction(msgIndex, newItem.getMessageAction(0));
                        }
                    } else {
                        //its a different controller - remove old element
                        itemRemove(oldItem);
                        //insert new element
                        itemAdd(newItem);
                    }
                    //update GUI component containing data
                    updateContainerGUI();
                } else {
                    //new item exists in collection
                    defaultLogger.writeLog(logType.warning, "New item already exists in collection!");
                }
            } else {
                //old item doesnt exist in collection
                defaultLogger.writeLog(logType.warning, "Item to modify doesnt exist in collection!");
            }
        }

        /// <summary>
        /// Remove inputted item to collection (internally checking if exists)
        /// </summary>
        /// <param name="cItem">Item to remove from collection (checking if exists)</param>
        public void itemRemove(WindowsIPC cItem)
        {
            int index = itemIndex(cItem);
            //check if current element exists in collection
            if (index != -1) {
                //element exists in collection - find controller data
                int cController = itemIndex(cItem, find.controller);
                if (cController != -1) {
                    int messagesCount = this[cController].countMessages();
                    if (messagesCount > 0) {
                        int msgIndex = this[cController].findMessageIndex(cItem.getMessageAction(0).message);
                        if (messagesCount == 1 && msgIndex != -1) {
                            RemoveAt(cController);
                        } else {
                            this[cController].removeMessage(msgIndex);
                        }
                    } else {
                        RemoveAt(cController);
                    }
                    //update GUI component containing data
                    updateContainerGUI();
                }
            }
        }

        public int checkMessage(string msg, ref int msgItem)
        {
            int result = -1;

            return result;
        }

        public void clear()
        {
            foreach (WindowsIPC cData in this) {
                //clear all messages
                cData.clearMessages();
            }
            //clear remote abb collection
            Clear();
        }

        public void loadFromXml(ref System.Xml.XmlReader xmlSubnode, Controller parent = null)
        {
            //we should get xml subtree with robot name as parent node
            while (xmlSubnode.Read()) {
                if (xmlSubnode.Name.StartsWith("robot_")) {
                    //create new object in collection (pass string arg if controller not visible [null])
                    if (addController(parent, xmlSubnode.GetAttribute("name"))) {
                        currentData.loadMessages(ref xmlSubnode, currentData.controller);
                        //if user wants to open client to communicate then we do it now
                        MessageBox.Show("TODO: auto-open client");
                    }
                }
            }
        }

        public void saveToXml(ref System.Xml.XmlWriter xmlSubnode, string nodeParentRobName)
        {
            xmlSubnode.WriteStartElement("windowsIPC");
            //check if current controller is on our list
            int cIndex = controllerIndex(nodeParentRobName);
            if (cIndex >= 0) {
                //controller is on list - it might have some signals - save them
                this[cIndex].saveMessages(ref xmlSubnode);
            }
            xmlSubnode.WriteEndElement();
        }

        public bool addController(Controller newController, string storeName = "")
        {
            //add new controller to collection
            WindowsIPC newItem = new WindowsIPC(newController);
            itemAdd(newItem);
            return true;
        }

        public void clearController(Controller toClear)
        {
            MessageBox.Show("TODO: clearController");
        }


        /********************************************************
         ***  IPC DATA COLLECTION - private methods
         ********************************************************/

        /// <summary>
        /// Check inputted item index in collection
        /// </summary>
        /// <param name="cItem">Item to check index</param>
        /// <returns>Item index in collection (-1 if item doesnt exists)</returns>
        private int itemIndex(WindowsIPC cItem, find searchType=find.exact)
        {
            int result = 0;
            //scan all collection
            foreach (WindowsIPC item in this) {
                //check what to search
                if (item.theSame(cItem, searchType)) {
                    break;
                } else {
                    result++;
                }
            }
            //check if something was found
            if (result >= Count) result = -1;
            //exit 
            return result;
        }

        private int controllerIndex(string abbName)
        {
            int result = 0;
            //scan all collection
            foreach (WindowsIPC item in this) {
                if (item.controllerName == abbName) {
                    break;
                } else {
                    result++;
                }
            }
            //check if something was found
            if (result >= Count) result = -1;
            //exit 
            return result;
        }

        /// <summary>
        /// update container GUI
        /// </summary>
        private void updateContainerGUI()
        {
            if (myContainer != null) {
                //update all collection in GUI container
                myContainer.Items.Clear();
                //update GUI for current controller 
                foreach (WindowsIPC item in this) {
                    if (item.controllerName == currentData.controllerName) {
                        item.updateGUI(myContainer);
                    }
                }
            } else {
                if (defaultLogger != null) {
                    defaultLogger.writeLog(logType.warning, "No GUI container connected to data...");
                }
            }
        }
    }
}
