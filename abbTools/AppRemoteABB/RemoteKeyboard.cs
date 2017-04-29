using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace abbTools.AppRemoteABB
{
    class RemoteKeyboard
    {
        /********************************************************
         ***  REMOTE KEYBOARD - data 
         ********************************************************/

        //dll method to simulate keyboard actions
        [DllImport("user32.dll")]
        public static extern int ToUnicode(uint virtualKeyCode, uint scanCode, byte[] keyboardState,
                                           [Out, MarshalAs(UnmanagedType.LPWStr, SizeConst = 64)] StringBuilder receivingBuffer,
                                           int bufferSize, uint flags);

        /********************************************************
         ***  REMOTE KEYBOARD - main actions
         ********************************************************/

        /// <summary>
        /// Method used to execute keyboard actions
        /// </summary>
        /// <param name="actions">Keyboard actions to do</param>
        public void doActions(string actions)
        {
            //sim keyboard as log as there are some actions to do
            if (actions.Length > 0) {
                int startPos = 0, commaPos = 0;
                string currAction = "";
                //while there are actions to do
                while (commaPos != actions.Length) {
                    //find button identifier and comma(s)
                    commaPos = actions.IndexOf(',', startPos);
                    //if no comma was found then this is the last command
                    if (commaPos == -1) commaPos = actions.Length;
                    //get action string
                    currAction = actions.Substring(startPos, commaPos - startPos);
                    //do current action
                    execute(interpret(currAction));
                    //start pos is next char after comma
                    if (commaPos != actions.Length) startPos = commaPos + 1;
                }
            }
        }

        /// <summary>
        /// Function used to decode givent string action(s)
        /// </summary>
        /// <param name="action">String action to decode</param>
        /// <returns>Decoded keyboard actions useful to execute</returns>
        private string interpret(string action)
        {
            string result = "";
            //to use function ToUnicode
            Keys myKeyCode = (Keys)Enum.Parse(typeof(Keys), action, true);
            var kState = new byte[256];
            var buffer = new StringBuilder(256);

            int converted = ToUnicode((uint)myKeyCode, 0, kState, buffer, 256, 0);
            //convert keys using 
            if (converted > 0) {
                result = buffer.ToString();
            } else {
                //functional keys cant be converted to unicode (F1, left arrow, etc)
                result = fnKeysToString(action);
            }

            return result;
        }

        /// <summary>
        /// Main method to send keyboard key input
        /// </summary>
        /// <param name="key">Key input to send / execute</param>
        private void execute(string key)
        {
            SendKeys.Send(key);
        }

        /********************************************************
         ***  REMOTE KEYBOARD - library
         ********************************************************/

        /// <summary>
        /// Method used to convert functional keys (Shift, Arrows, Ctrl, etc.) to string data
        /// </summary>
        /// <param name="functionKey">String function key to convert</param>
        /// <returns>String containing keys expressed in string format handled by execute method</returns>
        private string fnKeysToString(string functionKey)
        {
            string result = "";
            string[] notUsedList = { "ShiftKey", "ControlKey", "LWin", "RWin", "Menu" };
            bool functionNotUsed = false;

            //check if selected function key is not used
            for (int i = 0; i < notUsedList.Length; i++) {
                if (functionKey.Contains(notUsedList[i])) {
                    functionNotUsed = true;
                    break;
                }
            }
            //output result
            if (functionNotUsed) {
                result = "";
            } else {
                if (functionKey == "Capital") {
                    result = "{CAPSLOCK}";
                } else if (functionKey == "PageUp") {
                    result = "{PGUP}";
                } else if (functionKey == "PageDown" || functionKey == "Next") {
                    result = "{PGDN}";
                } else {
                    result = "{" + functionKey + "}";
                }
            }
            return result;
        }
    }
}
