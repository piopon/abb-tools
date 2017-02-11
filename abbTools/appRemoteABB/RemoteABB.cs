using abbTools;
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.IOSystemDomain;

namespace appRemoteABB
{
    class RemoteABB
    {
        private Controller myController;
        private RemoteSignalCollection mySignals;
        private loggerABB myLogger;
        //backup signal name field (when controller not visible in network)
        private string storedControllerName;

        public RemoteABB()
        {
            myController = null;
            myLogger = null;
            mySignals = new RemoteSignalCollection();
            storedControllerName = "";
        }

        public RemoteABB(Controller newController)
        {
            myController = newController;
            mySignals = new RemoteSignalCollection();
            myLogger = null;
            storedControllerName = newController.SystemName;
        }

        public RemoteABB(Controller newController, RemoteSignal newSignal)
        {
            myController = newController;
            mySignals = new RemoteSignalCollection();
            mySignals.Add(newSignal);
            myLogger = null;
            storedControllerName = newController.SystemName;
        }

        public RemoteABB(Controller newController, RemoteSignalCollection newCollection)
        {
            myController = newController;
            mySignals = newCollection;
            myLogger = null;
            storedControllerName = newController.SystemName;
        }

        public void attachLogger(loggerABB logger)
        {
            if(logger != null) myLogger = logger;
        }

        public void detachLogger()
        {
            myLogger = null;
        }

        public void log(logType type, string msg)
        {
            if (myLogger != null) {
                myLogger.writeLog(type, msg);
            }
        }

        public void addRemoteSignal(RemoteSignal newSignal)
        {
            mySignals.Add(newSignal);
        }

        public int countSignals()
        {
            return mySignals.Count;
        }

        public Controller controller
        {
            get { return myController; }
            set { myController = value; }
        }

        public string storedName
        {
            get { return storedControllerName; }
            set { storedControllerName = value; }
        }

        public RemoteSignalCollection signals
        {
            get { return mySignals; }
            set { mySignals = value; }
        }
    }

    class RemoteABBCollection : List<RemoteABB>
    {
        //current edited controller
        private RemoteABB currentData;
        private loggerABB defaultLogger;

        public RemoteABBCollection()
        {
            currentData = new RemoteABB();
            defaultLogger = null;
        }

        public RemoteABBCollection(loggerABB collectionLogger)
        {
            currentData = new RemoteABB();
            defaultLogger = collectionLogger;
        }

        public void syncLogger(loggerABB collectionLogger)
        {
            defaultLogger = collectionLogger;
        }

        public void desyncLogger()
        {
            defaultLogger = null;
        }

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

        public void fillWinFormControl(ref ListView list)
        {
            list.Items.Clear();
            currentData.signals.fillWinFormControl(ref list);
        }

        public bool setCurrController(Controller curr)
        {
            bool result = false;
            //check if current controller is in collection
            foreach (RemoteABB selectData in this) {
                //there can be non-visible controllers in collection
                string selectedName = selectData.controller != null ? selectData.controller.SystemName : selectData.storedName;
                if (selectedName == curr.SystemName) {
                    //found element in collection
                    currentData = selectData;
                    result = true;
                    break;
                }
            }
            return result;
        }

        public Controller getCurrController()
        {
            Controller result = null;
            if (currentData != null) result = currentData.controller;
            return result;
        }

        public int getControllerIndex(Controller abbController)
        {
            int result = -1;

            if (Count > 0) {
                bool gotController = false;
                foreach (RemoteABB cItem in this) {
                    result++;
                    if (cItem.controller.SystemName == abbController.SystemName) {
                        gotController = true;
                        break;
                    }
                }
                if (!gotController) {
                    result = -1;
                }
            }
            return result;
        }

        public int getControllerIndex(string controllerName)
        {
            int result = -1;

            if (Count > 0) {
                bool gotController = false;
                foreach (RemoteABB cItem in this) {
                    result++;
                    string cItemName = cItem.controller != null ? cItem.controller.SystemName : cItem.storedName;
                    if (cItemName == controllerName) {
                        gotController = true;
                        break;
                    }
                }
                if (!gotController) {
                    result = -1;
                }
            }
            return result;
        }

        public RemoteSignalCollection getCurrSignals()
        {
            RemoteSignalCollection result = null;
            if (currentData != null)  result = currentData.signals;
            return result;
        }

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

        public bool addController(Controller newController, string storeName="")
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
                        if (selectData.storedName == newController.SystemName) {
                            //its the one to add so lets update its info (online now)
                            currData = selectData;
                            try {
                                selectData.controller = newController;
                                if (!selectData.controller.Connected) {
                                    selectData.controller.Logoff();
                                    selectData.controller.Logon(UserInfo.DefaultUser);
                                }
                                foreach (RemoteSignal cSig in selectData.signals) {
                                    string storedSigName = cSig.storedSig;
                                    cSig.signal = selectData.controller.IOSystem.GetSignal(storedSigName);
                                    cSig.signal.Changed += signalWatcher;
                                }
                            } catch (ABB.Robotics.GenericControllerException e) {
                                selectData.controller.ConnectionChanged += busyControllerConnChanged;
                                currentData.log(logType.error, "controller <bu>" + selectData.controller.SystemName + "</bu> "+e.Message.ToLower()+".. waiting for connection..."); 
                            } catch (System.Exception e) {
                                selectData.controller.ConnectionChanged += busyControllerConnChanged;
                                currentData.log(logType.error, "controller <bu>" + selectData.controller.SystemName + "</bu> "+e.Message.ToLower()+".. waiting for connection...");
                            }
                        }
                    }
                }
                //check if controller is in collection
                if (currData == null) {
                    //add controller to list only if not there yet
                    currData = new RemoteABB(newController);
                    currData.attachLogger(defaultLogger);
                    Add(currData);
                    result = true;
                }
                //update current objects
                currentData = currData;
            } else {
                //parent was null - store it name and continue
                if (storeName.Length > 0) {
                    RemoteABB emptyData = new RemoteABB();
                    emptyData.storedName = storeName;
                    emptyData.attachLogger(defaultLogger);
                    Add(emptyData);
                    result = true;
                    //update current objects
                    currentData = emptyData;
                } 
            }

            return result;
        }

        private void busyControllerConnChanged(object sender, ConnectionChangedEventArgs e)
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
                                string storedSigName = cSig.storedSig;
                                cSig.signal = collectionItem.controller.IOSystem.GetSignal(storedSigName);
                                cSig.signal.Changed += signalWatcher;
                            }
                            //remove event on changed connection status
                            collectionItem.controller.ConnectionChanged -= busyControllerConnChanged;
                            //log status
                            collectionItem.log(logType.error, "controller <bu>" + collectionItem.controller.SystemName + "</bu> connected and updated!");
                            break;
                        }
                    }
                }                
            }
        }

        public void clearController(Controller toClear)
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
                    if (selectData.storedName == toClear.SystemName) {
                        //selected element is already cleared
                        break;
                    }
                }
            }
        }

        public void addSignal(Controller abbController, Signal abbSignal,
                              string trigger, string action, string mod, string app)
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
                    currentData.log(logType.info, "controller <bu>" + currentData.controller.SystemName + "</bu>:"
                                                  + " signal " + newSig.signal.Name + " subscibtion ON!");
                } else {
                    //signal exists in collection = NO MORE THAN ONE ACTION TO ONE SIGNAL
                    currentData.log(logType.info, "controller <bu>" + currentData.controller.SystemName + "</bu>:"
                                                  + " signal " + newSig.signal.Name + " already has subscription...");
                }
            } else {
                currentData.log(logType.info, "controller <bu>" + currentData.controller.SystemName + "</bu>:"
                                              + " not present in collection...");
            }
        }

        public bool removeSignal(Controller abbController, Signal abbSignal)
        {
            bool result = false;

            //set current controller to do action on desired element
            if(setCurrController(abbController)) { 
                foreach (RemoteSignal cSig in currentData.signals) {
                    if (cSig.signal.Name == abbSignal.Name) {
                        cSig.signal.Changed -= signalWatcher;
                        //log new watch event
                        currentData.log(logType.info, "controller <bu>" + currentData.controller.SystemName
                                                    + "</bu>: signal " + cSig.signal.Name + " subscibtion OFF!");
                        //delete signal from collection
                        currentData.signals.Remove(cSig);
                        break;
                    }
                }
            } else {
                currentData.log(logType.info, "controller <bu>" + currentData.controller.SystemName + "</bu>:"
                                                + " not present in collection...");
            }

            return result;
        }

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
                    currentData.log(logType.info, "controller <bu>" + abbController.SystemName + "</bu>: "
                                                + "subscibtion CHANGED ( " + oldSig + " >>> " + newSig + ")!");
                }
            } else {
                currentData.log(logType.info, "controller <bu>" + currentData.controller.SystemName + "</bu>:"
                                                + " not present in collection...");
            }
        }

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
                if (remoteSignal.trigger == (short)newValue.Value) {
                    //do action
                    if(remoteSignal.action.executeAction()) {
                        //log triggerred action
                        parentData.log(logType.info, "controller <bu>" + parentData.controller.SystemName + "</bu>: "
                                        + "signal " + caller.Name + " triggered ( value = " + newValue.Value.ToString()
                                        + ", time = " + DateTime.Now.ToLongTimeString() + " )!");
                    }
                }
            }
        }

        public void loadFromXml(ref System.Xml.XmlReader xmlSubnode, Controller parent = null)
        {
            //we should get xml subtree with robot name as parent node
            while (xmlSubnode.Read()) {
                if (xmlSubnode.Name.StartsWith("robot_")) {
                    //create new object in collection (pass string arg if controller not visible [null])
                    if(addController(parent, xmlSubnode.GetAttribute("name"))) {
                        currentData.signals.loadSignals(ref xmlSubnode, currentData.controller);
                        //if controller is visible then we can add signal watcher to its signals
                        if (currentData.controller != null) {
                            foreach (RemoteSignal cSig in currentData.signals) {
                                cSig.signal.Changed += signalWatcher;
                            }
                        }
                    }
                }
            }
        }

        public void saveToXml(ref System.Xml.XmlWriter xmlSubnode,string nodeParentRobName)
        {
            xmlSubnode.WriteStartElement("remotePC");
            //check if current controller is on our list
            int controllerIndex = getControllerIndex(nodeParentRobName);
            if (controllerIndex>=0) {
                //controller is on list - it might have some signals - save them
                this[controllerIndex].signals.saveSignals(ref xmlSubnode);
            }
            xmlSubnode.WriteEndElement();
        }

    }
}

