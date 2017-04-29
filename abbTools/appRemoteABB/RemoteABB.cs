using abbTools;
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.IOSystemDomain;

namespace abbTools.AppRemoteABB
{
    class RemoteABB
    {
        /********************************************************
         ***  REMOTE ABB - data 
         ********************************************************/

        /// <summary>
        /// class field / property - ABB controller object
        /// </summary>
        public Controller controller { get; set; }

        /// <summary>
        /// class field / property - ABB controller backup name (useful when controller not visible in network)
        /// </summary>
        public string storedControllerName { get; set; }

        /// <summary>
        /// class field / property - remote signals (with triggers and actions) collection
        /// </summary>
        public RemoteSignalCollection signals { get; set; }

        /// <summary>
        /// class field / property - logger object
        /// </summary>
        public loggerABB logger { get; set; }
        

        /********************************************************
         ***  REMOTE ABB - constructors 
         ********************************************************/

        /// <summary>
        /// Default constructor
        /// </summary>
        public RemoteABB()
        {
            controller = null;
            logger = null;
            signals = new RemoteSignalCollection();
            storedControllerName = "";
        }

        /// <summary>
        /// Constructor with fill ABB controller data
        /// </summary>
        /// <param name="newController">ABB controller data</param>
        public RemoteABB(Controller newController)
        {
            controller = newController;
            signals = new RemoteSignalCollection();
            logger = null;
            storedControllerName = newController.SystemName;
        }

        /// <summary>
        /// Constructor with fill ABB controller and add signal data
        /// </summary>
        /// <param name="newController">ABB controller data</param>
        /// <param name="newSignal">Add new remote signal to watch</param>
        public RemoteABB(Controller newController, RemoteSignal newSignal)
        {
            controller = newController;
            signals = new RemoteSignalCollection();
            signals.Add(newSignal);
            logger = null;
            storedControllerName = newController.SystemName;
        }

        /// <summary>
        /// Constructor with fill ABB controller and RemoteSignalCollection objects
        /// </summary>
        /// <param name="newController">ABB controller data</param>
        /// <param name="newCollection">RemoteSignalCollection data</param>
        public RemoteABB(Controller newController, RemoteSignalCollection newCollection)
        {
            controller = newController;
            signals = newCollection;
            logger = null;
            storedControllerName = newController.SystemName;
        }

        /********************************************************
         ***  REMOTE ABB - data management 
         ********************************************************/

        /// <summary>
        /// Method used to add new remote signal to collection for current ABB controller 
        /// </summary>
        /// <param name="newSignal">RemoteSignal object to add to collection</param>
        public void addRemoteSignal(RemoteSignal newSignal)
        {
            signals.Add(newSignal);
        }

        /// <summary>
        /// Function used to count current ABB controller remote signal items
        /// </summary>
        /// <returns>Number of remote signals</returns>
        public int countSignals()
        {
            return signals.Count;
        }
    }

    //================================================================================================================================
    //================================================================================================================================
    //================================================================================================================================

    class RemoteABBCollection : List<RemoteABB>
    {
        /********************************************************
         ***  REMOTE ABB COLLECTION - data
         ********************************************************/

        /// <summary>
        /// class field / property - collection logger object
        /// </summary>        
        public loggerABB logger { get; set; }

        //current collection data (edited in GUI)
        private RemoteABB currentData;

        /********************************************************
         ***  REMOTE ABB COLLECTION - constructors 
         ********************************************************/

        /// <summary>
        /// Default constructor
        /// </summary>
        public RemoteABBCollection()
        {
            currentData = new RemoteABB();
            logger = null;
        }

        /// <summary>
        /// Constructor with logger object definition
        /// </summary>
        /// <param name="collectionLogger">Logger object to set as default for collection</param>
        public RemoteABBCollection(loggerABB collectionLogger)
        {
            currentData = new RemoteABB();
            logger = collectionLogger;
        }

        /********************************************************
         ***  REMOTE ABB COLLECTION - collection data management
         ********************************************************/

        /// <summary>
        /// Method used to load collection data from XML file
        /// </summary>
        /// <param name="xmlSubnode">XML subnode to read data from</param>
        /// <param name="parent">ABB controller as parent of current data</param>
        /// <param name="parentName">ABB controller name as parent of data (useful when controller non-existent in network)</param>
        public void loadFromXml(ref System.Xml.XmlReader xmlSubnode, Controller parent = null, string parentName = "")
        {
            //we should get xml subtree with remotePC as parent node
            while (xmlSubnode.Read()) {
                if (xmlSubnode.Name.StartsWith("remotePC")) {
                    if (xmlSubnode.IsStartElement() && !xmlSubnode.IsEmptyElement) {
                        //create new object in collection (pass string arg if controller not visible [null])
                        if (controllerAdd(parent, parentName)) {
                            currentData.signals.loadSignals(ref xmlSubnode, currentData.controller);
                            //if controller is visible then we can add signal watcher to its signals
                            if (currentData.controller != null) {
                                foreach (RemoteSignal cSig in currentData.signals) {
                                    cSig.signal.Changed += signalWatcher;
                                }
                            }
                            //got all data from current robot - break from while loop
                            break;
                        }
                    } else {
                        //no data for current robot - break from while loop
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Method used to save collection data to XML file
        /// </summary>
        /// <param name="xmlSubnode">XML subnode to save data to</param>
        /// <param name="nodeParentRobName">XML node ABB controller parent name</param>
        public void saveToXml(ref System.Xml.XmlWriter xmlSubnode, string nodeParentRobName)
        {
            //start XML subnode
            xmlSubnode.WriteStartElement("remotePC");
            //check if current controller is on our list
            int abbIndex = controllerIndex(nodeParentRobName);
            //if controller is on list then save its signals
            if (abbIndex >= 0) this[abbIndex].signals.saveSignals(ref xmlSubnode);
            //end XML subnode
            xmlSubnode.WriteEndElement();
        }

        /// <summary>
        /// Method used to fill ListView GUI item with collection data
        /// </summary>
        /// <param name="list">ListView item which will be containing data</param>
        public void fillWinFormControl(ref ListView list)
        {
            list.Items.Clear();
            currentData.signals.fillWinFormControl(ref list);
        }

        /********************************************************
         ***  REMOTE ABB COLLECTION - controller data management
         ********************************************************/

        /// <summary>
        /// Function used to set passed ABB controller as current edited collection item
        /// </summary>
        /// <param name="curr">ABB controller to set as current in collection</param>
        /// <returns>TRUE if selected controller is in collection and is set as current (FALSE otherwise)</returns>
        public bool setCurrController(Controller curr)
        {
            bool result = false;
            //check if current controller is in collection
            foreach (RemoteABB selectData in this) {
                //there can be non-visible controllers in collection
                string selectedName = selectData.controller != null ? selectData.controller.SystemName : selectData.storedControllerName;
                if (selectedName == curr.SystemName) {
                    //found element in collection
                    currentData = selectData;
                    result = true;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Function used to get current edited ABB controller item from collection
        /// </summary>
        /// <returns></returns>
        public Controller getCurrController()
        {
            Controller result = null;
            if (currentData != null) result = currentData.controller;
            return result;
        }

        /// <summary>
        /// Function used to get selected ABB controller index in collection (zero-based)
        /// </summary>
        /// <param name="abbController">ABB controller object to get index of</param>
        /// <returns>Index of specified controller in collection (-1 otherwise)</returns>
        public int controllerIndex(Controller abbController)
        {
            int index = -1;
            //if collection is empty then return
            if (Count <= 0) return -1;
            //scan whole collection for inputted ABB controller
            foreach (RemoteABB cItem in this) {
                index++;
                //if controller was found then return its index
                if (cItem.controller.SystemName == abbController.SystemName) return index;
            }
            //no controller was found
            return -1;
        }

        /// <summary>
        /// Function used to get selected ABB name index in collection (zero-based)
        /// </summary>
        /// <param name="abbController">ABB controller name to get index of</param>
        /// <returns>Index of specified controller in collection (-1 otherwise)</returns>
        public int controllerIndex(string controllerName)
        {
            int index = -1;
            //if collection is empty then return
            if (Count <= 0) return -1;
            //scan whole collection for inputted ABB controller
            foreach (RemoteABB cItem in this) {
                index++;
                //if controller was found then return its index
                if (cItem.controller.SystemName == controllerName) return index;
            }
            //no controller was found
            return -1;
        }

        /// <summary>
        /// Function used to add (or set) current controller in collection
        /// </summary>
        /// <param name="newController">ABB controller object to add to collection</param>
        /// <param name="storeName">ABB controller name to store in collection</param>
        /// <returns>TRUE if controller was added (otherwise FALSE)</returns>
        public bool controllerAdd(Controller newController, string storeName = "")
        {
            bool result = false;

            if (newController != null) {
                //add controller to list (if it doesnt exist)
                RemoteABB currData = null;
                foreach (RemoteABB selectData in this) {
                    //current element might be not visible (only stored info about it)
                    if (selectData.controller != null) {
                        if (selectData.controller.SystemName == newController.SystemName) {
                            currData = selectData;
                            break;
                        }
                    } else {
                        //current element is empty (has only stored name)
                        if (selectData.storedControllerName == newController.SystemName) {
                            //its the one to add so lets update its info (online now)
                            currData = selectData;
                            try {
                                System.Threading.Thread.Sleep(1000);
                                selectData.controller = newController;
                                if (!selectData.controller.Connected) {
                                    selectData.controller.Logoff();
                                    selectData.controller.Logon(UserInfo.DefaultUser);
                                }
                                foreach (RemoteSignal cSig in selectData.signals) {
                                    string storedSigName = cSig.signalStoreName;
                                    cSig.signal = selectData.controller.IOSystem.GetSignal(storedSigName);
                                    cSig.signal.Changed += signalWatcher;
                                }
                            }
                            catch (ABB.Robotics.GenericControllerException e) {
                                selectData.controller.ConnectionChanged += controllerConnChanged;
                                currentData.logger?.writeLog(logType.warning, "controller <bu>" + selectData.controller.SystemName + "</bu> " + e.Message.ToLower() + ".. waiting for connection...");
                            }
                            catch (Exception e) {
                                selectData.controller.ConnectionChanged += controllerConnChanged;
                                currentData.logger?.writeLog(logType.warning, "controller <bu>" + selectData.controller.SystemName + "</bu> " + e.Message.ToLower() + ".. waiting for connection...");
                            }
                        }
                    }
                }
                //check if controller is in collection
                if (currData == null) {
                    //add controller to list only if not there yet
                    currData = new RemoteABB(newController);
                    currData.logger = logger;
                    Add(currData);
                    result = true;
                }
                //update current objects
                currentData = currData;
            } else {
                //parent was null - store it name and continue
                if (storeName.Length > 0) {
                    RemoteABB emptyData = new RemoteABB();
                    emptyData.storedControllerName = storeName;
                    emptyData.logger = logger;
                    Add(emptyData);
                    result = true;
                    //update current objects
                    currentData = emptyData;
                }
            }
            return result;
        }

        /// <summary>
        /// Method triggered when inputted ABB controller was lost
        /// </summary>
        /// <param name="toClear">ABB controller that gone lost</param>
        public void controllerLost(Controller toClear)
        {
            //find if inputted controller is in collection
            foreach (RemoteABB selectData in this) {
                //current element might be not visible (only stored info about it)
                if (selectData.controller != null) {
                    if (selectData.controller.SystemName == toClear.SystemName) {
                        foreach (RemoteSignal cSig in selectData.signals) {
                            if (cSig.signal != null) {
                                cSig.signal.Changed -= signalWatcher;
                                cSig.signal = null;
                            }
                        }
                        //we found it - lets clear its controller data
                        selectData.controller.Logoff();
                        selectData.controller = null;
                        break;
                    }
                } else {
                    //current element is empty (has only stored name)
                    if (selectData.storedControllerName == toClear.SystemName) break;
                }
            }
        }

        /// <summary>
        /// Method used when ABB controller connection change event triggers
        /// </summary>
        /// <param name="sender">ABB Controller which triggered this event</param>
        /// <param name="e">Event arguments</param>
        private void controllerConnChanged(object sender, ConnectionChangedEventArgs e)
        {
            //this event was triggered by busy controller which has shown online
            if (e.Connected) {
                //we dont have access to controller which triggered this event
                //so we have to find it manually - search all collection items
                foreach (RemoteABB collectionItem in this) {
                    //this event will only occur on existing controllers
                    if (collectionItem.controller != null && collectionItem.controller.Connected) {
                        //we found controller if signal doesnt exist
                        if (collectionItem.signals[0].signal == null) {
                            collectionItem.controller.Logon(UserInfo.DefaultUser);
                            foreach (RemoteSignal cSig in collectionItem.signals) {
                                string storedSigName = cSig.signalStoreName;
                                cSig.signal = collectionItem.controller.IOSystem.GetSignal(storedSigName);
                                cSig.signal.Changed += signalWatcher;
                            }
                            //remove event on changed connection status
                            collectionItem.controller.ConnectionChanged -= controllerConnChanged;
                            //log status
                            collectionItem.logger?.writeLog(logType.info, "controller <bu>" + collectionItem.controller.SystemName + "</bu>: "
                                                                        + "connected and updated!");
                            break;
                        }
                    }
                }
            }
        }

        /********************************************************
         ***  REMOTE ABB COLLECTION - signal data management
         ********************************************************/

        /// <summary>
        /// Function used to get remote signal collection data from current edited item
        /// </summary>
        /// <returns>RemoteSignalCollection data (null if collection non-existent)</returns>
        public RemoteSignalCollection getCurrSignals()
        {
            RemoteSignalCollection result = null;
            if (currentData != null)  result = currentData.signals;
            return result;
        }

        /// <summary>
        /// Function used to get remote signals of selected ABB controller item
        /// </summary>
        /// <param name="abbController">ABB Controller item to get remote signals from</param>
        /// <returns>RemoteSignalCollection data (null if item in collection is non-existent)</returns>
        public RemoteSignalCollection getSignals(Controller abbController)
        {
            RemoteSignalCollection result = null;
            foreach (RemoteABB cData in this) {
                if (cData.controller == abbController) {
                    result = cData.signals;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Method used to add new remote signal data to collection
        /// </summary>
        /// <param name="abbController">ABB controller who is parent of current remote signal</param>
        /// <param name="abbSignal">ABB signal object that is watched</param>
        /// <param name="trigger">Signal state that triggers action</param>
        /// <param name="action">Action to be triggered</param>
        /// <param name="mod">Action modifier when using mouse or keyboard actions</param>
        /// <param name="app">Application path that is used when doing action</param>
        public void addSignal(Controller abbController, Signal abbSignal,string trigger, string action, string mod, string app)
        {
            //set current controller to do action on desired element
            if (setCurrController(abbController)) {
                RemoteSignal newSig = new RemoteSignal(abbSignal, trigger, action, mod, app);
                //check if current signal is in collection
                if (currentData.signals.findIndex(newSig) == -1) {
                    //signal doesnt exist - add it to collection
                    abbSignal.Changed += signalWatcher;
                    currentData.signals.Add(newSig);
                    //log new watch event
                    currentData.logger?.writeLog(logType.info, "controller <bu>" + currentData.controller.SystemName + "</bu>:"
                                                             + " signal " + newSig.signal.Name + " subscibtion ON!");
                } else {
                    //signal exists in collection = NO MORE THAN ONE ACTION TO ONE SIGNAL
                    currentData.logger?.writeLog(logType.warning, "controller <bu>" + currentData.controller.SystemName + "</bu>:"
                                                                + " signal " + newSig.signal.Name + " already has subscription...");
                }
            } else {
                currentData.logger?.writeLog(logType.error, "controller <bu>" + currentData.controller.SystemName + "</bu>:"
                                                          + " not present in collection...");
            }
        }

        /// <summary>
        /// Function used to remove selected signal from collection
        /// </summary>
        /// <param name="abbController">ABB controller parent of signal to remove</param>
        /// <param name="abbSignal">ABB signal to remove from collection</param>
        /// <returns>TRUE if signal was removed, FALSE otherwise</returns>
        public bool removeSignal(Controller abbController, Signal abbSignal)
        {
            bool result = false;

            //set current controller to do action on desired element
            if(setCurrController(abbController)) { 
                foreach (RemoteSignal cSig in currentData.signals) {
                    if (cSig.signal.Name == abbSignal.Name) {
                        cSig.signal.Changed -= signalWatcher;
                        //log new watch event
                        currentData.logger?.writeLog(logType.info, "controller <bu>" + currentData.controller.SystemName
                                                                 + "</bu>: signal " + cSig.signal.Name + " subscibtion OFF!");
                        //delete signal from collection
                        currentData.signals.Remove(cSig);
                        break;
                    }
                }
            } else {
                currentData.logger?.writeLog(logType.error, "controller <bu>" + currentData.controller.SystemName + "</bu>:"
                                                          + " not present in collection...");
            }

            return result;
        }

        /// <summary>
        /// Method used to modify selected remote signal data in collection
        /// </summary>
        /// <param name="which">Which signal in collection we want to modify</param>
        /// <param name="abbController">ABB controller who is the new parent of current remote signal</param>
        /// <param name="newSignal">New ABB signal object that is watched</param>
        /// <param name="newTrigger">Modified signal state that triggers action</param>
        /// <param name="newAction">Modified action to be triggered</param>
        /// <param name="newMod">Modified action modifier of mouse or keyboard actions</param>
        /// <param name="newApp">Modified application path</param>
        public void modifySignal(int which, Controller abbController, Signal newSignal,
                                 string newTrigger, string newAction, string newMod, string newApp)
        {
            //set current controller to do action on desired element
            if(setCurrController(abbController)) { 
                //get current signal
                RemoteSignal currSig = currentData.signals.findSignal(which);
                //check if selected signal index (which) is correct
                if (currSig != null) {
                    //get data for logging events
                    string oldSig = currSig.signal.Name,
                           newSig = newSignal.Name;
                    //signal doesnt exist - add it to collection
                    currSig.signal.Changed -= signalWatcher;
                    currSig.modify(newSignal, newTrigger, newAction, newMod, newApp);
                    newSignal.Changed += signalWatcher;
                    //log new watch event
                    currentData.logger?.writeLog(logType.info, "controller <bu>" + abbController.SystemName + "</bu>: "
                                                             + "subscibtion CHANGED ( " + oldSig + " >>> " + newSig + ")!");
                }
            } else {
                currentData.logger?.writeLog(logType.error, "controller <bu>" + currentData.controller.SystemName + "</bu>:"
                                                          + " not present in collection...");
            }
        }

        /// <summary>
        /// Method used when ABB signal state changes
        /// </summary>
        /// <param name="sender">Signal that triggered this event</param>
        /// <param name="e">Event arguments</param>
        private void signalWatcher(object sender, SignalChangedEventArgs e)
        {
            //get triggered signal name nad new value
            Signal caller = (Signal)sender;
            SignalState newValue = e.NewSignalState;
            //find triggered abb signal parent (RemoteSignal)
            RemoteABB parentData = null;
            RemoteSignal remoteSignal = null;
            foreach (RemoteABB cData in this) {
                remoteSignal = cData.signals.findSignal(caller);
                if (remoteSignal != null) {
                    //get parent ABB data object
                    parentData = cData;
                    break;
                } 
            }
            //check if we got all actions
            if (remoteSignal != null) {
                //check trigger
                if (remoteSignal.triggerValue == (short)newValue.Value) {
                    //do action
                    if(remoteSignal.action.execute()) {
                        //log triggerred action
                        parentData.logger?.writeLog(logType.info, "controller <bu>" + parentData.controller.SystemName + "</bu>: "
                                                                + "signal " + caller.Name + " triggered ( value = " + newValue.Value.ToString()
                                                                + ", time = " + DateTime.Now.ToLongTimeString() + " )!");
                    }
                }
            }
        }

        /// <summary>
        /// Function used to clear all data from RemoteABBCollection 
        /// </summary>
        public void clear()
        {
            foreach (RemoteABB cData in this) {
                foreach (RemoteSignal cSig in cData.signals) {
                    //remove signal watcher event
                    if (cSig.signal != null) cSig.signal.Changed -= signalWatcher;
                }
                //clear all sig collection
                cData.signals.clear();
            }
            //clear remote abb collection
            Clear();
        }
    }
}

