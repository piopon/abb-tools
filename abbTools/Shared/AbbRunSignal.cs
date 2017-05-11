using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.IOSystemDomain;
using System.Xml;

namespace abbTools.Shared
{
    class AbbRunSignal
    {
        /********************************************************
         ***  ABB RUN SIGNAL - class properties
         ********************************************************/

        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int runtimeState { get; set; }

        /********************************************************
         ***  ABB RUN SIGNAL - constructor
         ********************************************************/

        /// <summary>
        /// Default constructor
        /// </summary>
        public AbbRunSignal()
        {
            name = "";
            runtimeState = 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sigName"></param>
        /// <param name="runtimeVal"></param>
        public AbbRunSignal(string sigName, int runtimeVal)
        {
            name = sigName;
            runtimeState = runtimeVal;
        }

        /********************************************************
         ***  ABB RUN SIGNAL - main functions
         ********************************************************/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cController"></param>
        public void runtimeOn(Controller cController)
        {
            //check if current controller has defined runtime signal
            Signal mySig = cController.IOSystem.GetSignal(name);
            if (mySig != null) {
                mySig.Value = runtimeState > 0 ? 1 : 0;
                mySig.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cController"></param>
        public void runtimeOff(Controller cController)
        {
            //check if current controller has defined runtime signal
            Signal mySig = cController.IOSystem.GetSignal(name);
            if (mySig != null) {
                mySig.Value = runtimeState > 0 ? 1 : 0;
                mySig.Dispose();
            }
        }

        /********************************************************
         ***  ABB RUN SIGNAL - data management
         ********************************************************/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        public void saveData(XmlWriter xml)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
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
