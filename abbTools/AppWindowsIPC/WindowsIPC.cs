using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.IOSystemDomain;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace abbTools.AppWindowsIPC
{
    //enumerator for defining data type to find
    public enum find
    {
        exact = 0,
        controller,
        server,
    }

    class WindowsIPC
    {
        /********************************************************
         ***  IPC DATA - data
         ********************************************************/

        /// <summary>
        /// Get current ABB Controller data
        /// </summary>
        public Controller controller { get; set; }

        /// <summary>
        /// Get current ABB Controller name
        /// </summary>
        public string controllerStoredName { get; }

        /// <summary>
        /// Get or set controller connected flag
        /// </summary>
        public bool controllerConnected { get; set; }

        /// <summary>
        /// Get current named-pipe server name
        /// </summary>
        public WindowsIPCClient ipcClient { get; set; }

        /// <summary>
        /// Get or set logger object
        /// </summary>
        public loggerABB logger { get; set; }

        //collection data
        private WindowsIPCMessagesCollection myData;
        //event delegates
        public delegate void updateClientControl(bool clientStopped);
        public event updateClientControl ClientControlChange;

        /********************************************************
         ***  IPC DATA - object constructors
         ********************************************************/

        /// <summary>
        /// Default constructor;
        /// </summary>
        public WindowsIPC()
        {
            //ABB controller data
            controller = null;
            controllerStoredName = "";
            controllerConnected = false;
            //comm and collection data
            ipcClient = null;
            myData = new WindowsIPCMessagesCollection();
            logger = null;
        }

        /// <summary>
        /// Constructor filling ABB controller data (controller exists in network)
        /// </summary>
        /// <param name="newController">ABB controller data (all available)</param>
        public WindowsIPC(Controller newController)
        {
            //ABB controller data
            controller = newController;
            controllerStoredName = newController.SystemName;
            controllerConnected = false;
            //comm and collection data
            ipcClient = null;
            myData = new WindowsIPCMessagesCollection();
            logger = null;
        }

        /// <summary>
        /// Constructor filling ABB controller name (controller not exists in network)
        /// </summary>
        /// <param name="controllerName">ABB controller name (data no available)</param>
        public WindowsIPC(string controllerName)
        {
            //ABB controller data
            controller = null;
            controllerStoredName = controllerName;
            controllerConnected = false;
            //comm and collection data
            ipcClient = null;
            myData = new WindowsIPCMessagesCollection();
            logger = null;
        }

        /// <summary>
        /// Constructor filling private data
        /// </summary>
        /// <param name="newController">ABB controller object with data</param>
        /// <param name="newClient">IPC (named-pipe) client object with data</param>
        /// <param name="newLogger">ABB logger object</param>
        public WindowsIPC(Controller newController, WindowsIPCClient newClient, loggerABB newLogger)
        {
            //ABB controller data
            controller = newController;
            controllerStoredName = newController.SystemName;
            controllerConnected = false;
            //comm and collection data
            ipcClient = newClient;
            myData = new WindowsIPCMessagesCollection();
            logger = newLogger;
        }

        /// <summary>
        /// Constructor filling ABB controller data and named-pipe client settings
        /// </summary>
        /// <param name="controllerName">ABB controller object with data (controller exists in network)</param>
        /// <param name="ipcServer">IPC (named-pipe) client param: server name</param>
        /// <param name="ipcRecon">IPC (named-pipe) client param: restore communication</param>
        /// <param name="ipcAutoOpen">IPC (named-pipe) client param: auto open</param>
        public WindowsIPC(Controller newController, string ipcServer, bool ipcRecon, bool ipcAutoOpen)
        {
            //ABB controller data
            controller = newController;
            controllerStoredName = newController.SystemName;
            controllerConnected = false;
            //comm and collection data
            ipcClient = new WindowsIPCClient(ipcServer, ipcRecon, ipcAutoOpen);
            myData = new WindowsIPCMessagesCollection();
            logger = null;
        }

        /// <summary>
        /// Constructor filling ABB controller data and named-pipe client settings
        /// </summary>
        /// <param name="controllerName">ABB controller name (data no available)</param>
        /// <param name="ipcServer">IPC (named-pipe) client param: server name</param>
        /// <param name="ipcRecon">IPC (named-pipe) client param: restore communication</param>
        /// <param name="ipcAutoOpen">IPC (named-pipe) client param: auto open</param>
        public WindowsIPC(string controllerName, string ipcServer, bool ipcRecon, bool ipcAutoOpen)
        {
            //ABB controller data
            controller = null;
            controllerStoredName = controllerName;
            controllerConnected = false;
            //comm and collection data
            ipcClient = new WindowsIPCClient(ipcServer, ipcRecon, ipcAutoOpen);
            myData = new WindowsIPCMessagesCollection();
            logger = null;
        }

        /********************************************************
         ***  IPC DATA - message collection management
         ********************************************************/

        /// <summary>
        /// Method to clear all messages from collection
        /// </summary>
        public void messagesClear()
        {
            myData.Clear();
        }

        /// <summary>
        /// Method to count messages in collection 
        /// </summary>
        /// <returns>NUMBER of messages in collection</returns>
        public int messagesCount()
        {
            return myData.Count;
        }

        /// <summary>
        /// Method to clear single message from collection
        /// </summary>
        /// <param name="index">Index of message to remove</param>
        public void messageRemove(int index)
        {
            if (index >= 0) {
                myData.RemoveAt(index);
            }
        }

        /// <summary>
        /// Method used to add single message
        /// </summary>
        /// <param name="message">Message to add to collection</param>
        /// <param name="sigData">Signal name to add to collection (when signal data is not available)</param>
        /// <param name="sigValue">Signal value to add to collection</param>
        public void messageAdd(string message, string sigData, int sigValue)
        {
            myData.Add(new WindowsIPCMessages(message, sigData, sigValue));
        }

        /// <summary>
        /// Method used to add single message
        /// </summary>
        /// <param name="messageAction">Message action object to add to collection</param>
        public void messageAdd(WindowsIPCMessages messageAction)
        {
            myData.Add(messageAction);
        }

        /// <summary>
        /// Method to update selected message with new data
        /// </summary>
        /// <param name="index">Message to update</param>
        /// <param name="updMessage">New data for selected message</param>
        public void messageUpdate(int index, WindowsIPCMessages updMessage)
        {
            //fill new data info
            myData[index] = updMessage;
        }

        /// <summary>
        /// Method to get message DATA from collection
        /// </summary>
        /// <param name="index"></param>
        /// <returns>MESSAGE DATA object (NULL if index out of range)</returns>
        public WindowsIPCMessages getMessageAction(int index)
        {
            //check if index is correct
            if (index >= 0 && index <= myData.Count) {
                //index OK
                return myData[index];
            } else {
                //index NOK
                return null;
            }
        }

        /// <summary>
        /// Function used to find index of inputted messageActions
        /// </summary>
        /// <param name="messageAction">Message and action to find</param>
        /// <returns>INDEX of inputted messageAction (-1 if non-existent)</returns>
        public int messageIndex(WindowsIPCMessages messageAction)
        {
            int result;
            //search all collection for current message
            for (result = 0; result < myData.Count; result++) {
                //firstly check message 
                if (myData[result].messageActor == messageAction.messageActor) {
                    //secondly check signal name
                    if (myData[result].signalResult == messageAction.signalResult) {
                        //finally check signal value
                        if (myData[result].signalValue == messageAction.signalValue) {
                            //got message - break from FOR loop
                            break;
                        }
                    }
                }
            }
            //check if something was found
            result = result >= myData.Count ? -1 : result;

            return result;
        }

        /// <summary>
        /// Function used to find index of inputted message
        /// </summary>
        /// <param name="message">Message to find</param>
        /// <returns>INDEX of inputted message (-1 if non-existent)</returns>
        public int messageIndex(string message)
        {
            int result;
            //search all collection for current message
            for (result = 0; result < myData.Count; result++) {
                if (myData[result].messageActor == message) {
                    //got message - break from FOR loop
                    break;
                }
            }
            //check if something was found
            result = result >= myData.Count ? -1 : result;

            return result;
        }

        /// <summary>
        /// Method used to execute action for selected message
        /// </summary>
        /// <param name="index">Index of message in collection to trigger action</param>
        public void messageExecute(int index)
        {
            //get all signal data
            WindowsIPCMessages selected = getMessageAction(index);
            int signalVal = selected.signalValue;
            string messageName = selected.messageActor, 
                   signalName = selected.signalResult;
            //connect to controller
            try {
                if (controller != null) {
                    //get connection status (and user authentication)
                    bool isConnected = controller.Connected && controller.CurrentUser == UserInfo.DefaultUser;
                    //if no user is connected then do it for now
                    if (!isConnected) controller.Logon(UserInfo.DefaultUser);
                    //get robot signal
                    Signal trigger = controller.IOSystem.GetSignal(signalName);
                    trigger.Value = signalVal;
                    //log off if controller was not connected
                    if (!isConnected) controller.Logoff();
                    ipcClient.stats.messagesExecuted++;
                    logger?.writeLog(logType.info, $"Message {messageName} - event triggered! {controller.SystemName}: {signalName} = {signalVal.ToString()}"); 
                } else {
                    logger?.writeLog(logType.error, $"Message {messageName} - cant execute action... controller {controllerStoredName} non existent!");
                }
            } catch (Exception e) {
                logger?.writeLog(logType.error, $"Message {messageName} - exception: {e.Message}");
            }
        }

        /********************************************************
         ***  IPC DATA - GUI
         ********************************************************/

        /// <summary>
        /// Update ListView GUI container showing data in message collection 
        /// </summary>
        /// <param name="messagesContainer">ListView showing message collection data in GUI for user</param>
        /// <param name="serverNameContainer">TextBox showing server name in GUI for user</param>
        /// <param name="reconContainer">CheckBox showing client auto reconnect status in GUI for user</param>
        /// <param name="openContainer">CheckBox showing client auto open status in GUI for user</param>
        public void updateGUI(ListView messagesContainer)
        {
            //update messages data
            if (myData.Count > 0 && ipcClient != null && messagesContainer != null) {
                myData.updateGUI(ipcClient.server, messagesContainer);
            }
        }


        /********************************************************
         ***  IPC DATA - data management
         ********************************************************/

        /// <summary>
        /// Function used to check if object data is valid (filled with corr data)
        /// </summary>
        /// <returns>TRUE if object data is correct (FALSE otherwise)</returns>
        public bool valid()
        {
            return controller != null;
        }

        /// <summary>
        /// Method to validate data (update found controller object)
        /// </summary>
        /// <param name="corrObj">Object containing correct ABB Controller object</param>
        public void validate(WindowsIPC corrObj)
        {
            //upate controller object (messages should be OK)
            controller = corrObj.controller;
        }

        /// <summary>
        /// Method to invalidate data (clear lost controller object)
        /// </summary>
        public void invalidate()
        {
            //reset my controller object (messages contain strings and int so leave them)
            controller = null;
        }

        /// <summary>
        /// Function used to check if this object is the same as inputted
        /// </summary>
        /// <param name="reference">WindowsIPC object to compare to</param>
        /// <param name="search">Compare data type (all/controller/server the same)</param>
        /// <returns>TRUE if object the same (FALSE otherwise)</returns>
        public bool theSame(WindowsIPC reference, find search)
        {
            bool result = false;
            //check if reference exists
            result = reference != null;
            //check controller name
            if (search == find.controller || search == find.exact)
                //check if reference controller exists in network or not
                result = result && controllerStoredName == reference.controllerStoredName;
            //check clients server name
            if (search == find.server || search == find.exact)
                if (ipcClient != null && reference.ipcClient != null) {
                    result = result && ipcClient.server == reference.ipcClient.server;
                } else {
                    result = search == find.exact;
                }
            //check clients messages
            if (search == find.exact)
                if (reference.myData.Count > 0) {
                    result = result && myData.hasMessage(reference.myData[0]);
                } else {
                    result = false;
                }

            return result;
        }

        /// <summary>
        /// Save all messages to XML file
        /// </summary>
        /// <param name="xmlSubtree">XML subtree containig messages</param>
        public void saveMessages(ref System.Xml.XmlWriter xmlSubtree)
        {
            //save every message to delivered XML
            myData.saveToXML(ref xmlSubtree);
        }

        /// <summary>
        /// Load all messages from XML to collection
        /// </summary>
        /// <param name="xmlSubtree">XML subtree to load data from</param>
        /// <param name="parent">ABB Controller parent of messages (can be not visible during load [null]!)</param>
        public void loadMessages(ref System.Xml.XmlReader xmlSubtree, Controller parent = null)
        {
            //load every message from delivered XML
            myData.loadFromXML(ref xmlSubtree);
        }

        /********************************************************
         ***  IPC DATA - named-pipe client management
         ********************************************************/

        /// <summary>
        /// Method used to update named-pipe client data
        /// </summary>
        /// <param name="server">Update server name parameter</param>
        /// <param name="stateRecon">Update reconnect status</param>
        /// <param name="stateOpen">Update auto open status</param>
        public void ipcClientUpdate(string server, bool stateRecon, bool stateOpen)
        {
            //close client if it isnt running
            if (ipcClient != null && ipcClient.running) ipcClient.close();
            //wait for client closed
            while (ipcClient != null && ipcClient.eventsConn) {
                System.Threading.Thread.Sleep(100);
            }
            //delete all events
            ipcClientEventsDisconn();
            //create new client
            ipcClient = null;
            ipcClient = new WindowsIPCClient(server, stateRecon, stateOpen);
            //subscribe events
            ipcClientEventsConn();
        }

        /// <summary>
        /// Procedure to open named-pipe client with subscribing all events
        /// </summary>
        public void ipcClientOpen()
        {
            //check if client exists
            if (ipcClient != null) {
                //subscribe to events
                ipcClientEventsConn();
                //run event
                if (controllerConnected) eventOnControlChange(false);
                //open communication pipe
                ipcClient.open();
            }
        }

        /// <summary>
        /// Procedure to close named-pipe client 
        /// </summary>
        public void ipcClientClose()
        {
            //close communication pipe
            ipcClient.close();
        }

        /// <summary>
        /// Method used to connect events to ipcClient object
        /// </summary>
        public void ipcClientEventsConn()
        {
            //subscribe only when events arent connected yet
            if (ipcClient != null && !ipcClient.eventsConn) {
                //subscribe all client events
                ipcClient.OnConnect += clientStatusEvent;
                ipcClient.OnDisconnect += clientStatusEvent;
                ipcClient.OnWaiting += clientStatusEvent;
                ipcClient.OnReceived += clientRecvEvent;
                ipcClient.OnSent += clientSentEvent;
                ipcClient.OnEnd += clientEndEvent;
                //update event status
                ipcClient.eventsConn = true;
            }
        }

        /// <summary>
        /// Method used to disconnect events to ipcClient object
        /// </summary>
        public void ipcClientEventsDisconn()
        {
            //unsubscribe only when events are connected
            if (ipcClient != null && ipcClient.eventsConn) {
                //unsubscribe all client events
                ipcClient.OnConnect -= clientStatusEvent;
                ipcClient.OnDisconnect -= clientStatusEvent;
                ipcClient.OnWaiting -= clientStatusEvent;
                ipcClient.OnReceived -= clientRecvEvent;
                ipcClient.OnSent -= clientSentEvent;
                ipcClient.OnEnd -= clientEndEvent;
                //update event status
                ipcClient.eventsConn = false;
            }
        }

        /// <summary>
        /// event on background communication thread end
        /// </summary>
        /// <param name="sender">object which triggered the event</param>
        /// <param name="e">window IPC event args</param>
        private void clientEndEvent(object sender, WindowsIPCEventArgs e)
        {
            //disconnect all events from object
            ipcClientEventsDisconn();
            //run event
            if (controllerConnected) eventOnControlChange(true);
            //show log info
            logger?.writeLog(logType.info, $"[{controllerStoredName} - IPC {e.server}] status: {e.message}");
        }

        /// <summary>
        /// event on change communication status
        /// </summary>
        /// <param name="sender">object which triggered the event</param>
        /// <param name="e">window IPC event args</param>
        private void clientStatusEvent(object sender, WindowsIPCEventArgs e)
        {
            //show log info
            logger?.writeLog(logType.info, $"[{controllerStoredName} - IPC {e.server}] status: {e.message}");
        }

        /// <summary>
        /// event on message received from server
        /// </summary>
        /// <param name="sender">object which triggered the event</param>
        /// <param name="e">window IPC event args</param>
        private void clientRecvEvent(object sender, WindowsIPCEventArgs e)
        {
            //check if we are watching current message
            int msgIndex = messageIndex(e.message);
            if (msgIndex != -1) {
                //execute action from this message
                messageExecute(msgIndex);
            } else {
                //show log info
                logger?.writeLog(logType.info, $"[{controllerStoredName} - IPC {e.server}] received: {e.message}");
            }           
        }

        /// <summary>
        /// event on message sent to server
        /// </summary>
        /// <param name="sender">object which triggered the event</param>
        /// <param name="e">window IPC event args</param>
        private void clientSentEvent(object sender, WindowsIPCEventArgs e)
        {
            //show log info
            logger?.writeLog(logType.info, $"[{controllerStoredName} - IPC {e.server}] sent: {e.message}");
        }

        /********************************************************
         ***  IPC DATA - my events
         ********************************************************/

        /// <summary>
        /// Event triggered on waiting for server
        /// </summary>
        /// <param name="e">event arguments</param>
        protected void eventOnControlChange(bool clientStopped)
        {
            // SIMPLIFIED { if (OnWaiting != null) OnWaiting(this, e); }
            ClientControlChange?.Invoke(clientStopped);
        }
    }

    //================================================================================================================================
    //================================================================================================================================
    //================================================================================================================================

    class WindowsIPCCollection : List<WindowsIPC>
    {
        /********************************************************
         ***  IPC DATA COLLECTION - data
         ********************************************************/

        //current edited data
        private WindowsIPC currentData;
        private loggerABB defaultLogger;
        //create event delegates
        public delegate void updateClientControl(bool clientStopped);
        public event updateClientControl ClientControlChange;

        /********************************************************
         ***  IPC DATA COLLECTION - object constructors
         ********************************************************/

        /// <summary>
        /// Default constructor
        /// </summary>
        public WindowsIPCCollection()
        {
            //initialize current data element
            currentData = new WindowsIPC();
            defaultLogger = null;
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
            //clear all elements
            Clear();
        }

        /********************************************************
         ***  IPC DATA COLLECTION - parent management
         ********************************************************/

        /// <summary>
        /// Connect collection data with logger to output statuses
        /// </summary>
        /// <param name="logger">loggerABB for showing status</param>
        public void connectLogger(loggerABB cLogger)
        {
            defaultLogger = cLogger;
            foreach (WindowsIPC item in this) {
                item.logger = cLogger;
            }
        }

        /// <summary>
        /// Disconnect logger from collection data (no logging available)
        /// </summary>
        public void disconnectLogger()
        {
            defaultLogger = null;
            foreach (WindowsIPC item in this) {
                item.logger = null;
            }
        }

        /********************************************************
         ***  IPC DATA COLLECTION - collection management
         ********************************************************/

        /// <summary>
        /// Get index of inputted item in whole collection
        /// </summary>
        /// <param name="cItem">Item to check index</param>
        /// <returns>ITEM INDEX in collection (-1 if item non-existent)</returns>
        private int itemIndex(WindowsIPC cItem, find searchType = find.exact)
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

        /// <summary>
        /// Check if inputted item exists in collection
        /// </summary>
        /// <param name="cItem">Item to check</param>
        /// <returns>TRUE if item exists (FALSE otherwise)</returns>
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
                    if (cItem.ipcClient == null) {
                        //no client defined yet = first run or loading collection from file
                        Add(cItem);
                        //log success data
                        defaultLogger?.writeLog(logType.info, "Added controller to data collection!");
                    } else {
                        //client defined - create new instance = modifing collection from GUI 
                        WindowsIPC newInstance = new WindowsIPC(cItem.controller, cItem.ipcClient.server, 
                                                                cItem.ipcClient.autoRecon, cItem.ipcClient.autoOpen);
                        newInstance.messageAdd(cItem.getMessageAction(0));
                        //connect method to event (and subscribe to it)
                        newInstance.ClientControlChange += itemClientControlChange;
                        //add new instance to collection
                        Add(newInstance);
                        //log success data
                        defaultLogger?.writeLog(logType.info, "Added controller to data collection and message to watch list!");
                    }
                    //update current data object
                    currentData = cItem;
                } else {
                    //controller exists - check if client exists
                    if (this[cController].ipcClient == null) {
                        //client doesnt exist - update its data
                        this[cController].ipcClientUpdate(cItem.ipcClient.server, cItem.ipcClient.autoRecon, cItem.ipcClient.autoOpen);
                        //connect method to event (and subscribe to it)
                        this[cController].ClientControlChange += itemClientControlChange;
                        //there was no client so no messages must be empty - add first one
                        WindowsIPCMessages cMessageAction = cItem.getMessageAction(0);
                        this[cController].messageAdd(cMessageAction);
                        //log success data
                        defaultLogger?.writeLog(logType.info, "Updated IPC client and added message to watch list!");
                        //update current data object
                        currentData = this[cController];
                        //check if we want to auto run client comm thread
                        if (this[cController].ipcClient.autoOpen && !this[cController].ipcClient.running) {
                            currentData.ipcClientOpen();
                        }
                    } else {
                        //client in collection exists - check if it exists in current item
                        if (cItem.ipcClient != null) {
                            //check if server name is the same (MUST BE)
                            if (this[cController].ipcClient.server == cItem.ipcClient.server) {
                                //check if current element data is valid (filled with correct data)
                                if (!this[cController].valid()) {
                                    //current controller data is not valid - update it
                                    this[cController].validate(cItem);
                                }
                                //current object data is valid - get current item message action
                                WindowsIPCMessages cMessageAction = cItem.getMessageAction(0);
                                //check if current message doesnt exist
                                if (this[cController].messageIndex(cMessageAction) == -1) {
                                    //add message to current item
                                    this[cController].messageAdd(cMessageAction);
                                    //log success data
                                    defaultLogger?.writeLog(logType.info, "Added message to watch list!");
                                } else {
                                    //message exists - no duplicates
                                    defaultLogger?.writeLog(logType.warning, "Current message exists!");
                                }
                            } else {
                                defaultLogger?.writeLog(logType.error, "Different server name! Each robot must have only one client!");
                            }
                            //update current data object
                            currentData = this[cController];
                            //check if we want to auto run client comm thread
                            if (this[cController].ipcClient.autoOpen && !this[cController].ipcClient.running) {
                                currentData.ipcClientOpen();
                            }
                        } else {
                            //client in current item not existent - do nothing but refresh collection GUI
                            currentData = this[cController];
                        }
                    }
                }
            } else {
                defaultLogger?.writeLog(logType.warning, "Current element exists in collection!");
            }
        }

        /// <summary>
        /// Method used to modify oldItem in collection with newItems data
        /// </summary>
        /// <param name="oldItem">Item to modify</param>
        /// <param name="newItem">New data to modify</param>
        public void itemModify(WindowsIPC oldItem, WindowsIPC newItem)
        {
            //check if old item exists in collection 
            int oldIndex = itemIndex(oldItem);
            if (oldIndex != -1) {
                //check if new item doesnt exist in collection
                if (itemIndex(newItem) == -1) {
                    //old item is in collection and new one is not - all OK!
                    if(oldItem.controllerStoredName == newItem.controllerStoredName) {
                        //its the same controller - change inside it
                        int cController = itemIndex(oldItem, find.controller);
                        if (cController != -1) {
                            int msgIndex = this[cController].messageIndex(oldItem.getMessageAction(0));
                            this[cController].messageUpdate(msgIndex, newItem.getMessageAction(0));
                        }
                    } else {
                        //its a different controller - remove old element (and unsubscribe events)
                        oldItem.ClientControlChange -= itemClientControlChange;
                        itemRemove(oldItem);
                        //insert new element (and subscribe events)
                        newItem.ClientControlChange += itemClientControlChange;
                        itemAdd(newItem);
                    }
                    //log success data
                    defaultLogger?.writeLog(logType.info, "Modified message in watch list!");
                } else {
                    //new item exists in collection
                    defaultLogger?.writeLog(logType.warning, "New item already exists in collection!");
                }
            } else {
                //old item doesnt exist in collection
                defaultLogger?.writeLog(logType.warning, "Item to modify doesnt exist in collection!");
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
                    int messagesCount = this[cController].messagesCount();
                    if (messagesCount > 0) {
                        int msgIndex = this[cController].messageIndex(cItem.getMessageAction(0));
                        if (msgIndex != -1) {
                            this[cController].messageRemove(msgIndex);
                            //log success data
                            defaultLogger?.writeLog(logType.info, "Removed message from watch list!");
                        } else {
                            defaultLogger?.writeLog(logType.error, "Cant delete message - it doesnt exist in collecion...");
                        }
                    } else {
                        RemoveAt(cController);
                    }
                }
            }
        }

        /********************************************************
         ***  IPC DATA COLLECTION - GUI
         ********************************************************/

        /// <summary>
        /// Method to update GUIs ListView container 
        /// </summary>
        public void updateContainerGUI(ListView container)
        {
            if (container != null) {
                //update all collection in GUI container
                container.Items.Clear();
                container.BackColor = System.Drawing.Color.Silver;
                //update GUI for current controller 
                foreach (WindowsIPC item in this) {
                    if (item.controllerStoredName == currentData.controllerStoredName) {
                        item.updateGUI(container);
                    }
                }
                if (currentData.messagesCount() > 0) container.BackColor = System.Drawing.Color.White;
            } else {
                defaultLogger?.writeLog(logType.warning, "No GUI container connected to data...");
            }
        }

        /// <summary>
        /// Method used to update GUI of IPC server object
        /// </summary>
        /// <param name="clientNameBox">TextBox containing name of server</param>
        /// <param name="clientReconnBox">CheckBox representing auto reconnect of communication</param>
        /// <param name="clientAutoOpenBox">CheckBox representung auto open communication</param>
        public void updateServerGUI(TextBox clientNameBox,CheckBox clientReconnBox,CheckBox clientAutoOpenBox)
        {
            //update my client text data
            if (currentData.ipcClient != null) {
                if (clientNameBox != null) clientNameBox.Text = currentData.ipcClient.server;
                if (clientReconnBox != null) clientReconnBox.Checked = currentData.ipcClient.autoRecon;
                if (clientAutoOpenBox != null) clientAutoOpenBox.Checked = currentData.ipcClient.autoOpen;
            }
        }

        /********************************************************
         ***  IPC DATA COLLECTION - data management
         ********************************************************/

        /// <summary>
        /// Clear all collection from data
        /// </summary>
        public void clear()
        {
            foreach (WindowsIPC cData in this) {
                //clear all messages
                cData.messagesClear();
                //unsubscribe events
                cData.ClientControlChange -= itemClientControlChange;
            }
            //clear remote abb collection
            Clear();
        }

        /// <summary>
        /// Function used to find inputted message in collection
        /// </summary>
        /// <param name="msg">Message to check if exists in collection</param>
        /// <param name="parentIndex">Parent index containing found message</param>
        /// <param name="messageIndex">Message index in found collection element</param>
        /// <returns>TRUE if message was found in collection (FALSE otherwise)</returns>
        public bool messageFind(string msg, ref int parentIndex, ref int messageIndex)
        {
            bool result = false;
            //init data
            parentIndex = -1;
            messageIndex = -1;
            //scan all collection for inputted message
            foreach (WindowsIPC item in this) {
                //increment current element no
                parentIndex++;
                //find message in current element
                messageIndex = item.messageIndex(msg);
                //if message was found then break from foreach
                if (messageIndex != -1) {
                    //message found break from FOREACH with TRUE
                    result = true;
                    break;
                }
            }
            //check if item was found
            if (parentIndex >= Count) {
                //no item was found
                parentIndex = -1;
                messageIndex = -1;
            }

            return result;
        }

        /// <summary>
        /// Function used to find index of inputted controller name in collection
        /// </summary>
        /// <param name="abbName">ABB Controller name to find in collection</param>
        /// <returns>INDEX of found ABB Controller in collection (-1 otherwise)</returns>
        public int controllerIndex(string abbName)
        {
            int result = 0;
            //scan all collection
            foreach (WindowsIPC item in this) {
                if (item.controllerStoredName == abbName) {
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
        /// Function used to compare two client objects
        /// </summary>
        /// <param name="reference">Reference client object</param>
        /// <param name="curr">Current client object</param>
        /// <returns>TRUE if objects are the same, FALSE otherwise</returns>
        private bool clientCompare(WindowsIPCClient reference,WindowsIPCClient curr)
        {
            bool result = false;

            result = reference.server == curr.server;
            result = result && reference.autoRecon == curr.autoRecon;
            result = result && reference.autoOpen == curr.autoOpen;

            return result;
        }

        /// <summary>
        /// Method used to load collection data from XML subnode
        /// </summary>
        /// <param name="xmlSubnode">XML subnode containing data for collection</param>
        /// <param name="parent">ABB Controller parent of data (NULL if not visible in network)</param>
        /// <param name="parentName">ABB Controller name useful when ABB isnt visible in network</param>
        public void loadFromXml(ref System.Xml.XmlReader xmlSubnode, Controller parent = null, string parentName = "")
        {
            //we should get xml subtree with robot name as parent node
            while (xmlSubnode.Read()) {
                if (xmlSubnode.Name.StartsWith("windowsIPC")) {
                    if (xmlSubnode.IsStartElement() && !xmlSubnode.IsEmptyElement) {
                        //create new object in collection (pass string arg if controller not visible [null])
                        if (controllerUpdate(parent, parentName)) {
                            //get named-pipe client settings data
                            while (xmlSubnode.Read()) {
                                bool start = xmlSubnode.NodeType == System.Xml.XmlNodeType.Element,
                                     clientData = xmlSubnode.Name.StartsWith("clientIPC");
                                //if we are starting to read client data then get it
                                if (start && clientData) {
                                    string serverName = xmlSubnode.GetAttribute("server");
                                    bool reconnActive = bool.Parse(xmlSubnode.GetAttribute("recon")),
                                         openActive = bool.Parse(xmlSubnode.GetAttribute("open"));
                                    //update client object with data
                                    currentData.ipcClientUpdate(serverName, reconnActive, openActive);
                                    //break from WHILE loop - now will be message data
                                    break;
                                }
                            }
                            //check if there is something to read
                            if (!xmlSubnode.EOF) {
                                //load all messages to collection
                                currentData.loadMessages(ref xmlSubnode, currentData.controller);
                                //if user wants to open client and parent controller is visible then do it now
                                if (currentData.ipcClient.autoOpen && parent != null) currentData.ipcClientOpen();
                            }
                        }
                    } else {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Method used to save collection data to XML subnode
        /// </summary>
        /// <param name="xmlSubnode">XML subnode to save collection data</param>
        /// <param name="nodeParentRobName">Subnode parent controller name</param>
        public void saveToXml(ref System.Xml.XmlWriter xmlSubnode, string nodeParentRobName)
        {
            xmlSubnode.WriteStartElement("windowsIPC");
            //check if current controller is on our list
            int cIndex = controllerIndex(nodeParentRobName);
            if (cIndex >= 0 && this[cIndex].ipcClient != null) {
                //controller is on list - save server information
                xmlSubnode.WriteStartElement("clientIPC");
                //server name
                xmlSubnode.WriteAttributeString("server", this[cIndex].ipcClient.server);
                //auto reconnect flag
                xmlSubnode.WriteAttributeString("recon", this[cIndex].ipcClient.autoRecon.ToString());
                //auto open flag
                xmlSubnode.WriteAttributeString("open", this[cIndex].ipcClient.autoOpen.ToString());
                //save all messages in collection
                this[cIndex].saveMessages(ref xmlSubnode);
                //end client node
                xmlSubnode.WriteEndElement();
            }
            xmlSubnode.WriteEndElement();
        }

        /// <summary>
        /// Function used to add or update controller data in collection
        /// </summary>
        /// <param name="newController">controller object to add/update in collection</param>
        /// <param name="storeControllerName">controller object to ADD (only) to collection</param>
        /// <returns>TRUE if adding or updating OK (FALSE otherwise)</returns>
        public bool controllerUpdate(Controller newController, string storeControllerName = "")
        {
            //check input data
            if (newController == null && storeControllerName == "") return false;
            //input data OK - add controller to collection
            try {
                //check if controller exists in network
                WindowsIPC newItem = (newController != null) ? new WindowsIPC(newController) : newItem = new WindowsIPC(storeControllerName);
                //add event subscribtion
                newItem.ClientControlChange += itemClientControlChange;
                //sync logger
                newItem.logger = defaultLogger;
                //add new controller to collection
                itemAdd(newItem);
                //all ok - return true
                return true;
            } catch {
                //exception occured - return false
                return false;
            }
        }

        /// <summary>
        /// Method userd to zero controller data from collection (lost from network)
        /// </summary>
        /// <param name="toClear">Which controller was lost in network</param>
        public void controllerClear(Controller toClear)
        {
            //check input data
            if (toClear != null) {
                WindowsIPC clearItem = new WindowsIPC(toClear);
                int cController = itemIndex(clearItem, find.controller);
                if (cController != -1) {
                    //invalidate existing controller
                    this[cController].controllerConnected = false;
                    this[cController].invalidate();
                    // close named-pipe client
                    this[cController].ipcClientClose();
                }
            } else {
                defaultLogger?.writeLog(logType.error, "Cant clear collection controller null reference!");
            }
        }

        /********************************************************
         ***  IPC DATA COLLECTION - my events
         *********************************************************/

        /// <summary>
        /// method used only to pass item client control change to main GUI thread
        /// </summary>
        /// <param name="clientStopped">input if clients control state is stopped (TRUE) or running (FALSE)</param>
        protected void itemClientControlChange(bool clientStopped)
        {
            //call this event (received from GUI)
            eventOnControlChange(clientStopped);
        }

        /// <summary>
        /// Event triggered on waiting for server
        /// </summary>
        /// <param name="isConnected">if client is running</param>
        protected void eventOnControlChange(bool clientStopped)
        {
            // SIMPLIFIED { if (OnWaiting != null) OnWaiting(this, e); }
            ClientControlChange?.Invoke(clientStopped);
        }

    }
}
