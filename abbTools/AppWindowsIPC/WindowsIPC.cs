using System.Windows.Forms;
using System.Collections.Generic;
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.IOSystemDomain;

namespace abbTools.AppWindowsIPC
{
    class WindowsIPC
    {
        private Controller myController;
        private WindowsIPCClient myClient;
        private WindowsIPCMessagesCollection myData;
        private loggerABB myLogger;
        //backup controller name field (when controller not visible in network)
        private string storedControllerName;

        /********************************************************
         ***  IPC DATA - public methods
         ********************************************************/

        /// <summary>
        /// Default constructor;
        /// </summary>
        public WindowsIPC()
        {
            myController = null;
            storedControllerName = "";
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
            storedControllerName = newController.SystemName;
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
            storedControllerName = newController.SystemName;
            myClient = newClient;
            myData = new WindowsIPCMessagesCollection();
            myLogger = newLogger;
        }

        /// <summary>
        /// Function used to check if this object is the same as inputted
        /// </summary>
        /// <param name="reference">WindowsIPC object to compare to</param>
        /// <returns>TRUE if object the same, FALSE otherwise</returns>
        public bool theSame(WindowsIPC reference)
        {
            bool result = false;
            //check if reference exists
            result = reference != null;
            //check controller name
            result = result && myController.SystemName == reference.myController.SystemName;
            //check clients server name
            result = result && myClient.serverName == reference.myClient.serverName;
            //check messages
            result = result && myData.theSame(reference.myData);

            return result;
        }

        public void addMessageAction(string message, Signal sigData, int sigValue)
        {
            myData.Add(new WindowsIPCMessages(message, sigData, sigValue));
        }

        public void updateGUI(ListView container)
        {
            myData.updateGUI(serverName, container);
        }

        public string controllerName
        {
            get {
                if (myController == null) {
                    return storedControllerName;
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
        /// Connect collection data with GUI ListView container
        /// </summary>
        /// <param name="container">ListView item showing collection data for user</param>
        public void connectContainerGUI(ListView container)
        {
            myContainer = container;
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
            if (itemIndex(cItem) == -1)
            {
                //no element in collection - add it
                currentData = cItem;
                Add(cItem);
                //update GUI component containing data
                updateGUI();
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
            if (index != -1)
            {
                //element exists in collection - remove it
                RemoveAt(index);
                currentData = null;
                //update GUI component containing data
                updateGUI();
            }
        }

        /********************************************************
         ***  IPC DATA COLLECTION - private methods
         ********************************************************/

        /// <summary>
        /// Check inputted item index in collection
        /// </summary>
        /// <param name="cItem">Item to check index</param>
        /// <returns>Item index in collection (-1 if item doesnt exists)</returns>
        private int itemIndex(WindowsIPC cItem)
        {
            int result = 0;
            //scan all collection
            foreach (WindowsIPC item in this) {
                if (cItem.theSame(item)) {
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

        private void updateGUI()
        {
            if (myContainer != null) {
                //update all collection in GUI container
                myContainer.Items.Clear();
                currentData.updateGUI(myContainer);
            } else {
                if (defaultLogger != null) {
                    defaultLogger.writeLog(logType.warning, "No GUI container connected to data...");
                }
            }
        }
    }
}
