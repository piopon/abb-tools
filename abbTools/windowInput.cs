using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace abbTools
{
    public partial class windowInput : Form
    {
        public enum type
        {
            empty = -1,
            txt = 0,
            yesNo = 1,      
        };
        //message to be displayed for user
        private type myType;
        private string btnPressed;
        //internal flags to auto read keyboard keys or mouse position and button
        private bool autoKeyboard;
        private bool autoMouse;
        //address of parent application form
        private Form myApp;
        //obects to create semi-transparent mask to get click position
        private Form myMask;
        private Label myMaskInfo;

        public windowInput()
        {
            InitializeComponent();
            btnPressed = "";
            //no keyboard or mouse auto fill activated (only on user demand)
            autoKeyboard = false;
            autoMouse = false;
            //no type - assume its input text
            adjustGUI("DEFAULT","Default input window!");
        }

        public windowInput(string head, string msg, type inputType, string fill="", Form topParent=null)
        {
            InitializeComponent();
            btnPressed = "";
            //no keyboard or mouse auto fill activated (only on user demand)
            autoKeyboard = false;
            autoMouse = false;
            //adjust layout dependent on type of input
            adjustGUI(head,msg,inputType);
            //check if fill is needed
            textBoxInput.Clear();
            if (inputType == type.txt && fill!="") textBoxInput.Text = fill;
            //resize texbox component if to many chars for single line
            if (fill.Length > 45) {
                textBoxInput.Multiline = true;
                textBoxInput.Height = 70;
                textBoxInput.Top = 25;
                buttonClearInput.Top = 97;
                checkAutoKeyboard.Top = 97 + 5;
                checkAutoMouse.Top = 97 + 5;
            } else {
                textBoxInput.Multiline = false;
                textBoxInput.Height = 35;
                textBoxInput.Top = 40;
                buttonClearInput.Top = 85;
                checkAutoKeyboard.Top = 85 + 5;
                checkAutoMouse.Top = 85 + 5;
            }
            //get top parent address
            myApp = topParent;
        }

        private void adjustGUI(string userHead, string userMsg, type inType = type.empty)
        {
            //remember input data
            myType = inType;
            //update header and message
            labelHeader.Text = userHead;
            labelMessage.Text = userMsg;
            //check if selected type is input text
            if(inType == type.txt) {
                groupInputText.Visible = true;
                this.Height = 375;
            } else {
                groupInputText.Visible = false;
                this.Height = 235;
            }
            //check buttons
            if (inType == type.yesNo) {
                buttonOK.Visible = false;
                buttonOK.Left = 0;
                buttonCancel.Visible = false;
                buttonCancel.Left = 0;
                buttonYes.Visible = true;
                buttonYes.Left = 140;
                buttonNo.Visible = true;
                buttonNo.Left = 270;
            } else {
                buttonOK.Visible = true;
                buttonOK.Left = 140;
                buttonCancel.Visible = true;
                buttonCancel.Left = 270;
                buttonYes.Visible = false;
                buttonYes.Left = 0;
                buttonNo.Visible = false;
                buttonNo.Left = 0;
            }
            if (autoKeyboard) {
                labelNoteInputText.Text = "NOTE: press keyboard buttons (in action order)";
            } else {
                labelNoteInputText.Text = "NOTE: use commas to separate several inputs (in action order)";
            }
            if (autoMouse) {
                labelNoteInputText.Text = "NOTE: press on text box to provide input";
            } else {
                labelNoteInputText.Text = "NOTE: use commas to separate several inputs (in action order)";
            }
        }

        public string getUserInput()
        {
            string result = "";

            if (myType == type.txt) {
                //result is text from input box
                result = textBoxInput.Text;
            } else if (myType == type.yesNo) {
                //result is button pressed
                result = btnPressed;
            }

            return result;
        }

        public void autoFillInput(bool keyboardOn, bool mouseOn)
        {
            autoKeyboard = keyboardOn;
            autoMouse = mouseOn;
            //update location of checkboxes
            checkAutoKeyboard.Left = 23;
            checkAutoMouse.Left = 23;
            //update check of checkboxes
            checkAutoKeyboard.Checked = autoKeyboard;
            checkAutoMouse.Checked = autoMouse;
            //update visibility of checkboxes
            checkAutoKeyboard.Visible = autoKeyboard;
            checkAutoMouse.Visible = autoMouse;
        }

        private void inputButton_Click(object sender, EventArgs e)
        {
            Button clicked = (Button)sender;
            btnPressed = clicked.DialogResult.ToString();
            this.Close();
        }

        private void textBoxInput_KeyUp(object sender, KeyEventArgs e)
        {
            //print key pressed
            if (autoKeyboard)
            {
                int txtLen = textBoxInput.Text.Length,
                    alphaLen = e.KeyCode.ToString().Length;
                //if something is defined then add comma to end
                if (txtLen != 0) textBoxInput.Text += ",";
                textBoxInput.AppendText(e.KeyCode.ToString());
                //resize texbox component if to many chars for single line
                textBoxInputResize(txtLen, alphaLen);
            }
        }

        private void textBoxInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            //do not write new char if printing key pressed
            if (autoKeyboard) e.Handled = true;
        }

        private void textBoxInputResize(int currTextLen, int addTextLen)
        {
            if (currTextLen + addTextLen > 45) {
                textBoxInput.Multiline = true;
                textBoxInput.Height = 70;
                textBoxInput.Top = 25;
                buttonClearInput.Top = 97;
                checkAutoKeyboard.Top = 97 + 5;
                checkAutoMouse.Top = 97 + 5;
            } else {
                textBoxInput.Multiline = false;
                textBoxInput.Height = 35;
                textBoxInput.Top = 40;
                buttonClearInput.Top = 85;
                checkAutoKeyboard.Top = 85 + 5;
                checkAutoMouse.Top = 85 + 5;
            }
        }


        private void buttonClearInput_Click(object sender, EventArgs e)
        {
            textBoxInput.Clear();
            //restore single line textbox size
            textBoxInput.Multiline = false;
            textBoxInput.Height = 35;
            textBoxInput.Top = 40;
            buttonClearInput.Top = 85;
            checkAutoKeyboard.Top = 85 + 5;
            checkAutoMouse.Top = 85 + 5;
            //set focus on textbox
            this.ActiveControl = textBoxInput;
        }

        private void checkAuto_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox myCheck = (CheckBox)sender;
            if (myCheck.Name.Contains("Keyboard")) {
                autoKeyboard = myCheck.Checked;
                if (autoKeyboard) {
                    labelNoteInputText.Text = "NOTE: press keyboard buttons (in action order)";
                } else {
                    labelNoteInputText.Text = "NOTE: use commas to separate several inputs (in action order)";
                }
            } else {
                autoMouse = myCheck.Checked;
                if (autoMouse) {
                    labelNoteInputText.Text = "NOTE: press on text box to provide input";
                } else {
                    labelNoteInputText.Text = "NOTE: use commas to separate several inputs (in action order)";
                }
            }
            //set focus on textbox
            this.ActiveControl = textBoxInput;
        }

        private void textBoxInput_Enter(object sender, EventArgs e)
        {
            if (autoKeyboard || autoMouse) {
                textBoxInput.BackColor = Color.Silver;
            } else {
                textBoxInput.BackColor = Color.White;
            }
        }

        private void textBoxInput_Leave(object sender, EventArgs e)
        {
            textBoxInput.BackColor = Color.White;
        }

        private void textBoxInput_MouseClick(object sender, MouseEventArgs e)
        {
            if (autoMouse && myApp != null) {
                //hide application
                hideApp();
                //show semi-transparent form mask to get position and button
                showMask();
            }
        }

        private void showMask()
        {
            //check if mask is already created
            if (myMask == null) {
                //mask is not created yet
                myMask = new Form();
                //mask fill all screen
                myMask.FormBorderStyle = FormBorderStyle.None;
                myMask.WindowState = FormWindowState.Maximized;
                //its semi-transparent
                myMask.BackColor = Color.Black;
                myMask.Opacity = 0.75;
                myMask.ShowInTaskbar = false;
                //event on click
                myMask.MouseClick += maskFormClickEvent;
                //add labels informing to use Alt+Tab to place desired window in front
            }
            //check if mask label is already created
            if (myMaskInfo == null) {
                myMaskInfo = new Label();
                myMaskInfo.BackColor = Color.Orange;
                myMaskInfo.ForeColor = Color.Gray;
                myMaskInfo.Text = "click on target place..." + Environment.NewLine +
                                     "[alt + tab to view your app]";
                myMaskInfo.Font = new Font("GOST Common", 18F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(238)));
                myMaskInfo.AutoSize = true;
                myMaskInfo.TextAlign = ContentAlignment.MiddleCenter;
                myMaskInfo.Parent = myMask;
                //center label
                myMaskInfo.Location = new Point((Screen.GetWorkingArea(Cursor.Position).Width - myMaskInfo.Width) / 2,
                                                   (Screen.GetWorkingArea(Cursor.Position).Height - myMaskInfo.Height) / 2);
                myMaskInfo.Enabled = false;
            }
            //bring mask label to front
            myMaskInfo.Show();
            //show mask form
            myMask.BringToFront();
            myMask.ShowDialog();
        }

        private void hideMask()
        {
            //hide mask label
            if (myMaskInfo != null) myMaskInfo.Hide();
            //hide mask
            if (myMask != null) myMask.Hide();
        }

        private void deleteMask()
        {
            //delete mask info it and set it to null
            if (myMaskInfo != null) {
                myMaskInfo.Dispose();
                myMaskInfo = null;
            }
            //delete mask form it and set it to null
            if (myMask != null) {
                myMask.Dispose();
                myMask = null;
            }
        }

        private void windowInput_FormClosing(object sender, FormClosingEventArgs e)
        {
            //delete dynamically created form mask
            deleteMask();
        }

        private void maskFormClickEvent(object sender, MouseEventArgs e)
        {
            string myResult = "";

            //add comma if other positions were clicked
            if (textBoxInput.Text.Length > 0) myResult += ",";
            //get clicked position
            myResult += "[" + e.Location.X.ToString() + ";" + e.Location.Y.ToString() + "]";
            //get clicked button
            myResult += e.Button.ToString().Substring(0,1);
            //update textBox element
            textBoxInputResize(textBoxInput.Text.Length, myResult.Length);
            textBoxInput.AppendText(myResult);
            //hide semi-transparent form mask
            hideMask();
            //show application
            showApp();
        }

        private void showApp()
        {
            //to "show" appliaction make it non-transparent
            myApp.Opacity = 1;
            this.Opacity = 1;
            //bring windows to front if user used alt+tab to place
            //desired application in front
            myApp.TopMost = true;
            myApp.Focus();
            myApp.BringToFront();
            this.BringToFront();
            myApp.TopMost = false;
        }

        private void hideApp()
        {
            //to "hide" appliaction make it transparent
            this.Opacity = 0;
            myApp.Opacity = 0;           
        }
    }
}
