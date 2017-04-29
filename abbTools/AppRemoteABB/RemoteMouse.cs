using System.Runtime.InteropServices;

namespace abbTools.AppRemoteABB
{
    class RemoteMouse
    {
        /********************************************************
         ***  REMOTE MOUSE - data 
         ********************************************************/

        private enum myMouseBtn
        {
            left = 1,
            middle = 2,
            right = 3,
        }

        //dll method to simulate mouse movement 
        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        //dll method to simulate mouse clicks
        [DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        //definition of mouse button events used in above dll method
        private const int MOUSE_EVENT_LD = 0x02;
        private const int MOUSE_EVENT_LU = 0x04;
        private const int MOUSE_EVENT_RD = 0x08;
        private const int MOUSE_EVENT_RU = 0x10;
        private const int MOUSE_EVENT_MD = 0x20;
        private const int MOUSE_EVENT_MU = 0x40;

        /********************************************************
         ***  REMOTE MOUSE - main actions 
         ********************************************************/

        /// <summary>
        /// Method used to execute mouse actions
        /// </summary>
        /// <param name="actions">Mouse actions to do</param>
        public void doActions(string actions)
        {
            //sim mouse if there are some actions to do
            if (actions.Length > 0) {
                int startPos = 0, commaPos = 0;
                string currAction = "";
                //while there are actions to do
                while (commaPos != actions.Length) {
                    //find button identifier and comma(s)
                    startPos = actions.IndexOf('[', commaPos);
                    commaPos = actions.IndexOf(',', startPos + 1);
                    //if no start sign was found then end loop
                    if (startPos == -1) break;
                    //if no comma was found then this is the last command
                    if (commaPos == -1) commaPos = actions.Length;
                    //get action string
                    currAction = actions.Substring(startPos, commaPos - startPos);
                    //do current action
                    execute(interpret(currAction));
                }
            }
        }

        /// <summary>
        /// Function used to decode givent string action(s)
        /// </summary>
        /// <param name="action">String action to decode</param>
        /// <returns>Decoded mouse actions useful to execute</returns> 
        private int[] interpret(string action)
        {
            int[] corrdsDiv = { action.IndexOf('['), action.IndexOf(';'), action.IndexOf(']') };
            string curr;
            //get coordinates from inputted action string
            curr = action.Substring(corrdsDiv[0] + 1, corrdsDiv[1] - corrdsDiv[0] - 1);
            int coordX = short.Parse(curr);
            curr = action.Substring(corrdsDiv[1] + 1, corrdsDiv[2] - corrdsDiv[1] - 1);
            int coordY = short.Parse(curr);
            //get button ID from inpputed action string
            curr = action.Substring(corrdsDiv[2] + 1, 1);
            int btn = -1;
            switch (curr) {
                case "L":
                    btn = (int)myMouseBtn.left;
                    break;
                case "M":
                    btn = (int)myMouseBtn.middle;
                    break;
                case "R":
                    btn = (int)myMouseBtn.right;
                    break;
                default:
                    break;
            }
            //fill result
            int[] result = { coordX, coordY, btn };

            return result;
        }

        /// <summary>
        /// Main method to simulate mouse move and key input
        /// </summary>
        /// <param name="details">Decoded action parameters [coordX, coordY, btn]</param>
        private void execute(int[] details) 
        {
            //check input data (only positive integers are ok
            if (details[0] > 0 && details[1] > 0 && details[2] > 0) {
                int buttonActionDown = 0x00, buttonActionUp = 0x00;
                //convert int button to dlls accepted format
                switch (details[2]) {
                    case (int)myMouseBtn.left:
                        buttonActionDown = MOUSE_EVENT_LD;
                        buttonActionUp = MOUSE_EVENT_LU;
                        break;
                    case (int)myMouseBtn.middle:
                        buttonActionDown = MOUSE_EVENT_MD;
                        buttonActionUp = MOUSE_EVENT_MU;
                        break;
                    case (int)myMouseBtn.right:
                        buttonActionDown = MOUSE_EVENT_RD;
                        buttonActionUp = MOUSE_EVENT_RU;
                        break;
                    default:
                        break;
                }
                //move mouse cursor to defined position
                SetCursorPos(details[0], details[1]);
                mouse_event(buttonActionDown, details[0], details[1], 0, 0);
                System.Threading.Thread.Sleep(10);
                mouse_event(buttonActionUp, details[0], details[1], 0, 0);
            }
        }
    }
}
