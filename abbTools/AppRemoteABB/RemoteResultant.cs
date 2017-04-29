namespace abbTools.AppRemoteABB
{
    abstract class RemoteResultant
    {
        /********************************************************
         ***  REMOTE RESULTANT - datas
         ********************************************************/

        public enum type
        {
            appNull = -1,
            appExe = 0,
            appKill = 1,
            appReset = 2,
            appMouse = 3,
            appKey = 4
        };

        //define all available resultants and enums defining them
        private static string[] allResActions = { "app execute","app terminate","app restart","app mouse clik","app key press" };

        /********************************************************
         ***  REMOTE RESULTANT - static methods
         ********************************************************/

        /// <summary>
        /// Function used to convert action type from string
        /// </summary>
        /// <param name="actionStr">String defining action type</param>
        /// <returns>Action type convrted from string (of appNull when string is NOK)</returns>
        public static type actionToType(string actionStr)
        {
            type result = type.appNull;
            //check if input is delivered by full name or by index
            if (actionStr.Length > 1) {
                for (short i = 0; i < allResActions.Length; i++) {
                    if (allResActions[i] == actionStr) {
                        result = (type)i;
                        break;
                    }
                }
            } else {
                result = (type)short.Parse(actionStr);
            }
            return result;
        }

        /// <summary>
        /// Function used to get all used action in list (table) of string 
        /// </summary>
        /// <returns>Table of strings describing possible resultant actions</returns>
        public static string[] getActionList()
        {
            return allResActions;
        }

        /// <summary>
        /// Function used to get index of action in possible resultatn actions string table 
        /// </summary>
        /// <param name="action">String action to search index in string table</param>
        /// <returns>Index of string action in possible resutant actions (or -1 if non-existent)</returns>
        public static int actionsGetIndex(string action)
        {
            int result = -1;
            for (int itemNo = 0; itemNo < allResActions.Length; itemNo++) {
                if (action == allResActions[itemNo]) {
                    result = itemNo;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Function used to get number of possible actions
        /// </summary>
        /// <returns>Number of possible actions</returns>
        public static int actionsCount()
        {
            return allResActions.Length;
        }

    }
}
