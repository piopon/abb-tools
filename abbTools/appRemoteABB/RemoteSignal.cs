using System.Xml;
using System.Windows.Forms;
using System.Collections.Generic;
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.IOSystemDomain;

namespace appRemoteABB
{
    /// <summary>
    /// Definition of ABB signal with related: actor, resultant and PC application
    /// </summary>
    class RemoteSignal
    {
        //private fields
        private Signal mySignal;
        private short myTrigger;
        private RemoteAction myAction;
        //backup signal name field (when controller not visible in network)
        private string storedSignalName;

        /// <summary>
        /// Default constructor
        /// </summary>
        public RemoteSignal()
        {
            mySignal = null;
            myTrigger = -1;
            myAction = new RemoteAction();
            storedSignalName = "";
    }

        /// <summary>
        /// Constructor with initialization every class object
        /// </summary>
        /// <param name="signal">Watched signal name</param>
        /// <param name="trig">Signal trigger (change to 1 or 0)</param>
        /// <param name="res">Signal resultant (action to do after trigger) </param>
        /// <param name="mod">Signal modifier (additional data for resultant)</param>
        /// <param name="app">Application affected by resultant</param>
        public RemoteSignal(Signal signal, string trig, string res, string mod, string app)
        {
            mySignal = signal;
            myTrigger = actionToTrigger(trig);
            myAction = new RemoteAction(res,mod,app);
            storedSignalName = mySignal.Name;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="original">Object to copy from</param>
        public RemoteSignal(RemoteSignal original)
        {
            mySignal = original.mySignal;
            myTrigger = original.myTrigger;
            myAction = original.myAction;
            storedSignalName = original.storedSignalName;
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
                storedSignalName = xmlNode.GetAttribute("name");
                if (myParent != null) {
                    mySignal = myParent.IOSystem.GetSignal(storedSignalName);
                } else {
                    mySignal = null; 
                }
                //get actor (short attribute and convert it to useful data)
                myTrigger = short.Parse(xmlNode.GetAttribute("act"));
                //fill myAction data from sub tree node
                XmlReader xmlSubnode = xmlNode.ReadSubtree();
                myAction.loadFromXML(ref xmlSubnode);
            }

            return result;
        }

        public void saveToXML(ref XmlWriter xmlNode, int nodeNo)
        {
            //write start element
            xmlNode.WriteStartElement("signal_" + nodeNo.ToString());
            //signal name
            xmlNode.WriteAttributeString("name", storedSignalName);
            //signal trigger (value 0 or 1)
            xmlNode.WriteAttributeString("act", myTrigger.ToString());
            //write action subnode
            myAction.saveToXML(ref xmlNode);
            //finish element
            xmlNode.WriteEndElement();
        }

        /// <summary>
        /// Method filling selected ListViewItem control
        /// </summary>
        /// <param name="fill">ListViewItem to fill with data</param>
        public void fillWinFormControl(ref ListViewItem fill)
        {
            //fill all subitems
            if (mySignal != null) {
                fill.Text = mySignal.Name;
            } else {
                fill.Text = storedSignalName;
            }
            fill.SubItems.Add("change to \""+myTrigger.ToString() + "\"");
            myAction.fillWinFormControl(ref fill);
            //component work ok properties
            fill.Checked = false;
        }

        public void modify(Signal updSig, string updTrig, string updRes, string updMod, string updApp)
        {
            if (signal.Name != updSig.Name) {
                signal = updSig;
                storedSignalName = signal.Name;
            }
            if (myTrigger != actionToTrigger(updTrig)) myTrigger = actionToTrigger(updTrig);
            //modify RemoteAction component
            action.modify(updRes, updMod, updApp);
        }

        /// <summary>
        /// Clear all internal values
        /// </summary>
        public void clear()
        {
            mySignal = null;
            myTrigger = -1;
            myAction.clear();
        }

        /// <summary>
        /// Auto implemented property - signal name
        /// </summary>
        public Signal signal
        {
            get { return mySignal; }
            set { mySignal = value; }
        }

        public string storedSig
        {
            get { return storedSignalName; }
            set { storedSignalName = value; }
        }

        /// <summary>
        /// Auto implemented property - actor 
        /// </summary>
        public short trigger
        {
            get { return myTrigger; }
            set { myTrigger = value; }
        }

        /// <summary>
        /// Auto implemented property - resultant action
        /// </summary>
        public RemoteAction action
        {
            get { return myAction; }
            set { myAction = value; }
        }

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
    }

    /// <summary>
    /// Collection of watched ABB robot signals
    /// </summary>
    class RemoteSignalCollection : List<RemoteSignal>
    {
        public RemoteSignalCollection()
        {
            Clear();
        }

        public void fillWinFormControl(ref ListView list)
        {
            foreach (RemoteSignal sig in this) {
                ListViewItem cItem = new ListViewItem();
                sig.fillWinFormControl(ref cItem);
                //add curent item to list
                list.Items.Add(cItem);
            }
        }

        public void loadSignals(ref XmlReader xmlSubtree, Controller parent = null)
        {
            //load every element in delivered XML (faster if only interesting xmlSubtree)
            while (xmlSubtree.Read()) {
                if (xmlSubtree.NodeType == XmlNodeType.Element && xmlSubtree.Name.StartsWith("signal_")) {
                    //add new element to collection and fill its data 
                    RemoteSignal loadSig = new RemoteSignal();
                    loadSig.loadFromXML(xmlSubtree,parent);
                    Add(loadSig);
                }
            }
        }

        public void saveSignals(ref XmlWriter xmlSubtree)
        {
            int itemNo = 0;

            //save every element in delivered XML
            foreach (RemoteSignal sig in this) {
                sig.saveToXML(ref xmlSubtree, itemNo++);
            }
        }

        public void clear()
        {
            foreach (RemoteSignal sig in this) {
                sig.clear();
            }
        }

        public RemoteSignal findSignal(Signal abbSig)
        {
            RemoteSignal found = null;

            foreach (RemoteSignal cSig in this) {
                //we can scan non-visible controller with empty signals
                if (cSig.signal != null) {
                    //scanning visible controller - check signal
                    if (cSig.signal.Name == abbSig.Name) {
                        found = cSig;
                        break;
                    }
                }
            }

            return found;
        }

        public RemoteSignal findSignal(int sigIndex)
        {
            RemoteSignal found = null;

            //check if selected index in inside collection
            if (sigIndex>=0 && sigIndex<Capacity)
            {
                found = this[sigIndex];
            }

            return found;
        }

        public int findIndex(RemoteSignal sig)
        {
            int result = -1;

            if (Count > 0) {
                bool sigPresent = false;
                foreach (RemoteSignal cSig in this) {
                    result++;
                    if (cSig.signal.Name == sig.signal.Name) {
                        sigPresent = true;
                        break;
                    }
                }
                if (!sigPresent) {
                    result = -1;
                }
            }

            return result;
        }
    }
}
