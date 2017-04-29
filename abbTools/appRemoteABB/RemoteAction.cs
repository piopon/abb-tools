using System.Windows.Forms;

namespace abbTools.AppRemoteABB
{
    class RemoteAction
    {
        /********************************************************
         ***  REMOTE ACTION - data & fields
         ********************************************************/

        /// <summary>
        /// Property to GET or SET remote action resultant type (exeApp, killApp, mouseClick, etc.)
        /// </summary>
        public RemoteResultant.type resultant { get; set; }

        /// <summary>
        /// Property to GET or SET remote action modifier (mouse corrds and btn click, etc.)
        /// </summary>
        public string modifier { get; set; }

        /// <summary>
        /// Property to GET or SET remote action used application 
        /// </summary>
        public string appPath { get; set; }

        //process private object
        private RemoteProcess myProcess = null;

        /********************************************************
         ***  REMOTE ACTIONS - constructors
         ********************************************************/
        
        /// <summary>
        /// Default constructor
        /// </summary>
        public RemoteAction()
        {
            //create internal help data 
            myProcess = new RemoteProcess();
            //init fields
            resultant = RemoteResultant.type.appNull;
            modifier = "";
            appPath = "";
        }

        /// <summary>
        /// Filling defined data constructor
        /// </summary>
        /// <param name="action">String action to define what type of action to do</param>
        /// <param name="mod">Modifier to define additional data of mouse or keyboard actions</param>
        /// <param name="program">Application used in current action</param>
        public RemoteAction(string action, string mod, string program)
        {
            //create internal help data 
            myProcess = new RemoteProcess();
            //init fields
            resultant = RemoteResultant.actionToType(action);
            modifier = mod;
            appPath = program;
        }

        /// <summary>
        /// Filling defined data constructor
        /// </summary>
        /// <param name="action">type action to define what type of action to do</param>
        /// <param name="mod">Modifier to define additional data of mouse or keyboard actions</param>
        /// <param name="program">Application used in current action</param>
        public RemoteAction(RemoteResultant.type action, string mod, string program)
        {
            //create internal help data 
            myProcess = new RemoteProcess();
            //init fields
            resultant = action;
            modifier = mod;
            appPath = program;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="original">Original object to get data from</param>
        public RemoteAction(RemoteAction original)
        {
            //copy internal help data 
            myProcess = original.myProcess;
            //copy fields
            resultant = original.resultant;
            modifier = original.modifier;
            appPath = original.appPath;
        }

        /********************************************************
         ***  REMOTE ACTIONS - data management
         ********************************************************/

        /// <summary>
        /// Method used to clear all object data 
        /// </summary>
        public void clear()
        {
            resultant = RemoteResultant.type.appNull;
            modifier = "";
            appPath = "";
            myProcess = null;
        }

        /// <summary>
        /// Method used to modify current object with new values
        /// </summary>
        /// <param name="updAct">New string action to update</param>
        /// <param name="updMod">New action modifier to update</param>
        /// <param name="updApp">New app path to update</param>
        public void modify(string updAct, string updMod, string updApp)
        {
            RemoteResultant.type newType = RemoteResultant.actionToType(updAct);
            if (resultant != newType) resultant = newType;
            if (modifier != updMod) modifier = updMod;
            if (appPath != updApp) appPath = updApp;
        }

        /// <summary>
        /// Function used to get/load remote action data from XML node
        /// </summary>
        /// <param name="xmlNode">node containing info about WatchSignal</param>
        /// <returns>TRUE if load successfull, FALSE otherwise</returns>
        public bool loadFromXML(ref System.Xml.XmlReader xmlNode)
        {
            bool result = false;
            string shortAttr;

            while (result == false && xmlNode.Read()) {
                //only xml element nodes with attributes and "signal_" name are OK
                result = (xmlNode.NodeType == System.Xml.XmlNodeType.Element) && (xmlNode.Name == "action");
                if (result) {
                    if (xmlNode.HasAttributes) {
                        //get result (short attribute and convert it to useful data)
                        shortAttr = xmlNode.GetAttribute("res");
                        resultant = (RemoteResultant.type)short.Parse(shortAttr);
                        //get modifier (short attribute and convert it to useful data)
                        shortAttr = xmlNode.GetAttribute("mod");
                        modifier = shortAttr == "-" ? "" : shortAttr;
                        //get app dir
                        appPath = xmlNode.GetAttribute("app");
                    } else {
                        resultant = RemoteResultant.type.appNull;
                        modifier = "";
                        appPath = "";
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Method used to save remote action data to XML file
        /// </summary>
        /// <param name="xmlNode">XML file to save data to</param>
        public void saveToXML(ref System.Xml.XmlWriter xmlNode)
        {
            string saveAttr;
            //write start element
            xmlNode.WriteStartElement("action");
            //signal resultant (action to do)
            saveAttr = ((int)resultant).ToString();
            xmlNode.WriteAttributeString("res", saveAttr);
            //resultant modifier
            saveAttr = modifier.Length == 0 ? "-" : modifier;
            xmlNode.WriteAttributeString("mod", saveAttr);
            //application path
            xmlNode.WriteAttributeString("app", appPath);
            //finish element
            xmlNode.WriteEndElement();
        }

        /********************************************************
         ***  REMOTE ACTIONS - execute action
         ********************************************************/

        /// <summary>
        /// Function used to execute remote action
        /// </summary>
        /// <returns></returns>
        public bool execute()
        {
            return myProcess.executeAction(resultant,appPath,modifier);
        }

        /********************************************************
         ***  REMOTE ACTIONS - GUI
         ********************************************************/

        /// <summary>
        /// Method used to fill selected ListViewItem control
        /// </summary>
        /// <param name="fill">ListViewItem to fill with data</param>
        public void fillWinFormControl(ref ListViewItem fill)
        {

            string[] myList = RemoteResultant.getActionList();
            fill.SubItems.Add(myList[(int)resultant]);
            fill.SubItems.Add(appPath);
            fill.ToolTipText = modifier.Length == 0 ? "no actor modifier" : "actor modifier: " + modifier;
        }
    }
}
