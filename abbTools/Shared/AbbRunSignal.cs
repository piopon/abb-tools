using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.IOSystemDomain;
using System.Xml;

namespace abbTools.Shared
{
    public class AbbRunSignal
    {
        /********************************************************
         ***  ABB RUN SIGNAL - class properties
         ********************************************************/

        /// <summary>
        /// GET or SET active flag of ABB run signal
        /// </summary>
        public bool active { get; private set; }

        /// <summary>
        /// GET name of ABB run signal
        /// </summary>
        public string name { get; private set; }

        /// <summary>
        /// GET default runtime state (0 or 1) of ABB run signal
        /// </summary>
        public int runtimeState { get; private set; }

        //private reference to itself (singleton pattern)
        private static AbbRunSignal runSignal = null;

        /********************************************************
         ***  ABB RUN SIGNAL - constructor
         ********************************************************/

        /// <summary>
        /// Default constructor (private = singleton pattern)
        /// </summary>
        private AbbRunSignal()
        {
            name = "";
            runtimeState = 1;
        }

        /// <summary>
        /// Method used to get instance of ABB run signal (singleton pattern)
        /// </summary>
        /// <returns>AbbRunSignal instance</returns>
        public static AbbRunSignal getInstance()
        {
            //check if there is no instance created yet (singleton)
            if (runSignal == null) runSignal = new AbbRunSignal();
            return runSignal;
        }

        /********************************************************
         ***  ABB RUN SIGNAL - main functions
         ********************************************************/

        /// <summary>
        /// Method used to update object fields
        /// </summary>
        /// <param name="sigName">New signal name</param>
        /// <param name="runtimeVal">New runtime default value</param>
        public void update(string sigName, int runtimeVal)
        {
            name = sigName;
            runtimeState = runtimeVal;
        }

        /// <summary>
        /// Function used to activate ABB run signal
        /// </summary>
        /// <returns>TRUE if activation succeded, FALSE otherwise</returns>
        public bool activate()
        {
            active = (name != "") ? true : false;
            return active;
        }

        /// <summary>
        /// Method used to deactivate ABB run signal
        /// </summary>
        public void deactivate()
        {
            active = false;
        }

        /// <summary>
        /// Method used to set ABB run signal to default state
        /// </summary>
        /// <param name="cController">ABB controler object to switch run signal to default state</param>
        public void runtimeOn(Controller cController)
        {
            if (active && cController != null) {
                //check if current controller has defined runtime signal
                Signal mySig = cController.IOSystem.GetSignal(name);
                if (mySig != null) {
                    mySig.Value = runtimeState > 0 ? 1 : 0;
                    mySig.Dispose();
                }
            }
        }

        /// <summary>
        /// Method used to reset ABB run signal from default state
        /// </summary>
        /// <param name="cController">ABB controler object to reset run signal to default state</param>
        public void runtimeOff(Controller cController)
        {
            if (active && cController != null) {
                //check if current controller has defined runtime signal
                Signal mySig = cController.IOSystem.GetSignal(name);
                if (mySig != null) {
                    mySig.Value = runtimeState > 0 ? 1 : 0;
                    mySig.Dispose();
                }
            }
        }

        /********************************************************
         ***  ABB RUN SIGNAL - data management
         ********************************************************/

        /// <summary>
        /// Method used to save ABB run signal object components to XML file
        /// </summary>
        /// <param name="xml">XML file to save data to</param>
        public void saveData(XmlWriter xml)
        {

        }

        /// <summary>
        /// Method used to load ABB run signal object components from XML file
        /// </summary>
        /// <param name="xml">XML file to load data from</param>
        public void loadData(XmlReader xml)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{name} [runtime: {runtimeState}";
        }
    }
}
