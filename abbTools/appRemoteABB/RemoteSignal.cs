using System.Xml;
using System.Windows.Forms;
using System.Collections.Generic;
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.IOSystemDomain;

namespace abbTools.AppRemoteABB
{
    /// <summary>
    /// Definition of ABB signal with related: actor, resultant and PC application
    /// </summary>
    class RemoteSignal
    {
        /********************************************************
         ***  REMOTE SIGNAL - data 
         ********************************************************/
        
        /// <summary>
        /// class field / property - signal object
        /// </summary>
        public Signal signal { get; set; }

        /// <summary>
        /// class field / property - stored signal name (useful if saved controller is offline)
        /// </summary>
        public string signalStoreName { get; set; }

        /// <summary>
        /// class field / property - signal value that triggers action
        /// </summary>
        public short triggerValue { get; set; }

        /// <summary>
        /// class field / property  - resultant action
        /// </summary>
        public RemoteAction action { get; set; }

        /********************************************************
         ***  REMOTE SIGNAL - constructors
         ********************************************************/

        /// <summary>
        /// Default constructor
        /// </summary>
        public RemoteSignal()
        {
            signal = null;
            triggerValue = -1;
            action = new RemoteAction();
            signalStoreName = "";
    }

        /// <summary>
        /// Constructor with init of class fields
        /// </summary>
        /// <param name="signal">Watched signal name</param>
        /// <param name="trig">Signal trigger (change to 1 or 0)</param>
        /// <param name="res">Signal resultant (action to do after trigger) </param>
        /// <param name="mod">Signal modifier (additional data for resultant)</param>
        /// <param name="app">Application affected by resultant</param>
        public RemoteSignal(Signal sig, string trig, string res, string mod, string app)
        {
            signal = sig;
            triggerValue = actionToTrigger(trig);
            action = new RemoteAction(res,mod,app);
            signalStoreName = signal.Name;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="original">Object to copy from</param>
        public RemoteSignal(RemoteSignal original)
        {
            signal = original.signal;
            triggerValue = original.triggerValue;
            action = original.action;
            signalStoreName = original.signalStoreName;
        }

        /********************************************************
         ***  REMOTE SIGNAL - data management
         ********************************************************/

        /// <summary>
        /// Clear all internal values
        /// </summary>
        public void clear()
        {
            signal = null;
            triggerValue = -1;
            action.clear();
        }

        /// <summary>
        /// Fill WatchSignal definiton from XML node
        /// </summary>
        /// <param name="xmlNode">node containing info about RemoteSignal</param>
        /// <param name="myParent">robot containing info about RemoteSignal</param>
        /// <returns>TRUE if load successfull, FALSE otherwise</returns>
        public bool loadFromXML(XmlReader xmlNode, Controller myParent = null)
        {
            bool result, ifSignalNode;

            //only xml element nodes with attributes and "signal_" name are OK
            ifSignalNode = (xmlNode.NodeType == XmlNodeType.Element) && (xmlNode.Name.StartsWith("signal_"));
            result = ifSignalNode && xmlNode.HasAttributes;
            if (result) {
                //fill data from XML
                //get signal name
                signalStoreName = xmlNode.GetAttribute("name");
                if (myParent != null) {
                    signal = myParent.IOSystem.GetSignal(signalStoreName);
                } else {
                    signal = null; 
                }
                //get actor (short attribute and convert it to useful data)
                triggerValue = short.Parse(xmlNode.GetAttribute("act"));
                //fill myAction data from sub tree node
                XmlReader xmlSubnode = xmlNode.ReadSubtree();
                action.loadFromXML(ref xmlSubnode);
            }

            return result;
        }

        /// <summary>
        /// Method used to save remote signal data to XML file
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <param name="nodeNo"></param>
        public void saveToXML(ref XmlWriter xmlNode, int nodeNo)
        {
            //write start element
            xmlNode.WriteStartElement("signal_" + nodeNo.ToString());
            //signal name
            xmlNode.WriteAttributeString("name", signalStoreName);
            //signal trigger (value 0 or 1)
            xmlNode.WriteAttributeString("act", triggerValue.ToString());
            //write action subnode
            action.saveToXML(ref xmlNode);
            //finish element
            xmlNode.WriteEndElement();
        }

        /// <summary>
        /// Methd used to modify remote signal item with new values
        /// </summary>
        /// <param name="updSig">new signal object and stored name</param>
        /// <param name="updTrig">new trigger value</param>
        /// <param name="updRes">new response action</param>
        /// <param name="updMod">new modifier</param>
        /// <param name="updApp">new program path</param>
        public void modify(Signal updSig, string updTrig, string updRes, string updMod, string updApp)
        {
            if (signal.Name != updSig.Name) {
                signal = updSig;
                signalStoreName = signal.Name;
            }
            if (triggerValue != actionToTrigger(updTrig)) triggerValue= actionToTrigger(updTrig);
            //modify RemoteAction component
            action.modify(updRes, updMod, updApp);
        }

        /********************************************************
         ***  REMOTE SIGNAL - other
         ********************************************************/

        /// <summary>
        /// Convert action delivered in string to trigger value (short int)
        /// </summary>
        /// <param name="actionStr"></param>
        /// <returns></returns>
        private short actionToTrigger(string actionStr)
        {
            short result = -1;            
            //check if input is delivered by full name or by index
            if (actionStr.Length > 1) {
                string descr = actionStr.Substring(actionStr.IndexOf('"') + 1, 1);
                result = short.Parse(descr);
            }
            else {
                result = short.Parse(actionStr);
            }
            return result;
        }

        /// <summary>
        /// Method filling selected ListViewItem control
        /// </summary>
        /// <param name="fill">ListViewItem to fill with data</param>
        public void fillWinFormControl(ref ListViewItem fill)
        {
            //fill all subitems
            if (signal != null) {
                fill.Text = signal.Name;
            } else {
                fill.Text = signalStoreName;
            }
            fill.SubItems.Add("change to \"" + triggerValue.ToString() + "\"");
            action.fillWinFormControl(ref fill);
            //component work ok properties
            fill.Checked = false;
        }
    }

    //================================================================================================================================
    //================================================================================================================================
    //================================================================================================================================

    /// <summary>
    /// Collection of watched ABB robot signals
    /// </summary>
    class RemoteSignalCollection : List<RemoteSignal>
    {
        /********************************************************
         ***  REMOTE SIGNAL COLLECTION - constructor
         ********************************************************/

        /// <summary>
        /// Default constructor
        /// </summary>
        public RemoteSignalCollection()
        {
            Clear();
        }

        /********************************************************
         ***  REMOTE SIGNAL COLLECTION - data management
         ********************************************************/

        /// <summary>
        /// Load all signals to collection from XML file
        /// </summary>
        /// <param name="xmlSubtree">XML subtree containing signals data</param>
        /// <param name="parent">ABB controller parent to get signals from</param>
        public void loadSignals(ref XmlReader xmlSubtree, Controller parent = null)
        {
            //load every element in delivered XML (faster if only interesting xmlSubtree)
            while (xmlSubtree.Read()) {
                //only "signal_" elements are interesting
                if (xmlSubtree.Name.StartsWith("signal_")) {
                    //if element is not empty then get data from it
                    if (xmlSubtree.IsStartElement() && !xmlSubtree.IsEmptyElement) {
                        //add new element to collection and fill its data 
                        RemoteSignal loadSig = new RemoteSignal();
                        loadSig.loadFromXML(xmlSubtree, parent);
                        //add loaded remote signal data to collection
                        Add(loadSig);
                    } 
                }
                //get out of reading if we are out of reading signals (to speed up a little bit)
                if (xmlSubtree.Name.StartsWith("remotePC") && (xmlSubtree.NodeType == XmlNodeType.EndElement)) break;
            }
        }

        /// <summary>
        /// Save signals in collection to XML file
        /// </summary>
        /// <param name="xmlSubtree">XML file to save remote signals data</param>
        public void saveSignals(ref XmlWriter xmlSubtree)
        {
            int itemNo = 0;
            //save every element in delivered XML
            foreach (RemoteSignal sig in this) {
                sig.saveToXML(ref xmlSubtree, itemNo++);
            }
        }

        /// <summary>
        /// Method used to clear all remote signals collection data
        /// </summary>
        public void clear()
        {
            foreach (RemoteSignal sig in this) sig.clear();
            Clear();
        }

        /// <summary>
        /// Function used to find remote signal element in collection (specified by abb signal object)
        /// </summary>
        /// <param name="abbSig">ABB signal object used to find remote signal </param>
        /// <returns>RemoteSignal data containing ABB signal object (or null if non-existent)</returns>
        public RemoteSignal findSignal(Signal abbSig)
        {
            foreach (RemoteSignal cSig in this) {
                //we can scan non-visible controller with empty signals
                if (cSig.signal != null) {
                    //visible controller - check if signal name matchs
                    if (cSig.signal.Name == abbSig.Name) return cSig;
                }
            }
            return null;
        }

        /// <summary>
        /// Function used to get remote signal element in collection (specified by index)
        /// </summary>
        /// <param name="sigIndex">index of element to get from collection</param>
        /// <returns>RemoteSignal data specified by index (or null if non-existent)</returns>
        public RemoteSignal findSignal(int sigIndex)
        {
            RemoteSignal found = (sigIndex >= 0 && sigIndex < Capacity) ? this[sigIndex] : null;
            return found;
        }

        /// <summary>
        /// Function used to find collection index of specified remote signal object
        /// </summary>
        /// <param name="sig">RemoteSignal data to get index of</param>
        /// <returns>Collection index of RemoteSignal object (or -1 if non-existent)</returns>
        public int findIndex(RemoteSignal sig)
        {
            int result = -1;
            //if collection empty then return -1
            if (Count <= 0) return -1;
            //scan collection
            foreach (RemoteSignal cSig in this) {
                //increment collection item counter
                result++;
                //compare signal name if the same then return counter
                if (cSig.signal.Name == sig.signal.Name) return result;
            }
            //whole collection scanned and no match item found
            return -1;
        }

        /// <summary>
        /// Method used to fill selected ListView item
        /// </summary>
        /// <param name="list">ListView item address</param>
        public void fillWinFormControl(ref ListView list)
        {
            //update all collection items
            foreach (RemoteSignal sig in this) {
                ListViewItem cItem = new ListViewItem();
                sig.fillWinFormControl(ref cItem);
                //add curent item to list
                list.Items.Add(cItem);
            }
        }
    }
}
