using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace abbTools.Shared
{
    public class AppsManager : List<IAbbApplication>
    {
        /********************************************************
         ***  APPS MANAGER - data
         ********************************************************/

        //pages containing abbTools applications
        private object parentPages = null;

        /********************************************************
         ***  APPS MANAGER - constructor
         ********************************************************/

        /// <summary>
        /// Constructor with parent pages data init
        /// </summary>
        /// <param name="parent"></param>
        public AppsManager(object parent)
        {
            parentPages = parent;
        }

        /********************************************************
         ***  APPS MANAGER  - public data
         ********************************************************/

        /// <summary>
        /// Method used to register abbTool application and add it to collection
        /// </summary>
        /// <param name="newApp">abbTool application object</param>
        /// <param name="descr">Description of current application</param>
        /// <param name="iconName">Application icon</param>
        /// <param name="height">Application height</param>
        /// <param name="width">Application width</param>
        public void registerApp(IAbbApplication newApp, string iconName, int height, int width)
        {
            newApp.appIndex = Count;
            newApp.appIcon = iconName;
            newApp.appHeight = height;
            newApp.appWidth = width;
            //add app to collection
            Add(newApp);
        }

        /// <summary>
        /// Function used to generate and populate application dashboard
        /// </summary>
        /// <returns>New generated dashboard Panel containing application buttons</returns>
        public Panel generateDashboard()
        {
            Panel mainDashboard = new Panel();
            mainDashboard.Size = new Size(600, 450);
            mainDashboard.Top = 30;
            mainDashboard.Left = 30;
            //populate panel with buttons
            populateDashboard(mainDashboard);

            return mainDashboard;
        }

        /// <summary>
        /// Method used to reorder parent pages in order defined by registration
        /// </summary>
        public void reorderPages()
        {
            if (parentPages != null && parentPages.GetType().FullName.Contains("TabControl"))
            {
                TabControl myPar = (TabControl)parentPages;
                List<TabPage> newOrder = new List<TabPage>(Count);
                //remember collection order
                foreach (IAbbApplication app in this) {
                    string tabName = $"action{app.appName.Substring(3)}";
                    myPar.SelectTab(tabName);
                    newOrder.Add(myPar.SelectedTab);
                }
                //apply order to GUI
                for (int page = 0; page < Count; page++) {
                    myPar.TabPages[page + 1] = newOrder[page];
                }
                //show dashboard
                myPar.SelectTab(0);
            }
        }

        /********************************************************
         ***  APPS MANAGER - private data
         ********************************************************/

        /// <summary>
        /// Method generating buttons and labels to input parent dashboard
        /// </summary>
        /// <param name="parent">Parent dashboard Panel containing icons of apps</param>
        private void populateDashboard(Panel parent)
        {
            //contant buttons dimensions
            const int btnHeight = 140, btnWidth = 140, btnOffset = 20;
            //get total number of columns + rows and calc start point dims to center buttons
            var dims = currOffset(-1);
            int startX = (parent.Width - (dims.Item1 * btnWidth + (dims.Item1 - 1) * btnOffset)) / 2,
                startY = (parent.Height - (dims.Item2 * btnHeight + (dims.Item2 - 1) * btnOffset)) / 2;
            //add buttons and labels
            foreach (var app in this) {
                //calc button offset
                var offset = currOffset(app.appIndex);
                /*********************************************************************** BUTTON **********/
                Button appButton = new Button();
                appButton.Parent = parent;
                appButton.Height = btnHeight;
                appButton.Width = btnWidth;
                appButton.Left = startX + offset.Item1 * (btnWidth + btnOffset);
                appButton.Top = startY + offset.Item2 * (btnHeight + btnOffset);
                //style
                appButton.FlatStyle = FlatStyle.Flat;
                appButton.BackColor = Color.WhiteSmoke;
                appButton.BackgroundImage = getBitmapIcon(app.appIcon);
                appButton.BackgroundImageLayout = ImageLayout.Center;
                appButton.Text = "";
                appButton.TextImageRelation = TextImageRelation.Overlay;
                //behaviour
                appButton.Tag = app;
                appButton.Click += AppButton_Click;
                appButton.MouseHover += AppButton_MouseHover;
                /*********************************************************************** LABEL **********/
                Label appLabel = new Label();
                appLabel.Parent = parent;
                appLabel.Left = startX + offset.Item1 * (btnWidth + btnOffset);
                appLabel.Top = startY + appButton.Height - 1;
                appLabel.AutoSize = false;
                appLabel.Width = btnWidth;
                appLabel.BorderStyle = BorderStyle.FixedSingle;
                appLabel.BackColor = Color.DarkOrange;
                appLabel.Text = app.appName;
                appLabel.TextAlign = ContentAlignment.MiddleCenter;
            }
        }

        /// <summary>
        /// Function used to calculate offset between application buttons
        /// </summary>
        /// <param name="whichElement">Which element to calculate offset (-1 to get cols and rows number)</param>
        /// <returns>Tuple containing cols and rows offset (or number if input = -1)</returns>
        private Tuple<int, int> currOffset(int whichElement)
        {
            int threshold = 4,
                start = (Count <= threshold) ? Count : threshold,
                totalCols = start,
                totalRows = Count / start;

            int resultCols = whichElement < 0 ? totalCols : whichElement % totalCols;
            int resultRows = whichElement < 0 ? totalRows : whichElement / totalCols;

            return new Tuple<int, int>(resultCols, resultRows);
        }

        /// <summary>
        /// Generate event method connected to created app buttons onclick method (switch main tabs)
        /// </summary>
        /// <param name="sender">Button parent that triggered current event</param>
        /// <param name="e">Event arguments</param>
        private void AppButton_Click(object sender, System.EventArgs e)
        {
            Button clicked = (Button)sender;
            IAbbApplication app = (IAbbApplication)clicked.Tag;
            //change page of parent
            if (parentPages != null && parentPages.GetType().FullName.Contains("TabControl")) {
                TabControl myPar = (TabControl)parentPages;
                myPar.CausesValidation = false;
                myPar.SelectedIndex = app.appIndex + 1;
            }
        }

        /// <summary>
        /// Method triggered by mouse rests ober created app button (show tooltip)
        /// </summary>
        /// <param name="sender">Button parent that triggered current event</param>
        /// <param name="e">Event arguments</param>
        private void AppButton_MouseHover(object sender, EventArgs e)
        {
            //get sender application detials
            Button clicked = (Button)sender;
            IAbbApplication app = (IAbbApplication)clicked.Tag;
            //create tooltip
            ToolTip myTip = new ToolTip();
            myTip.Active = true;
            myTip.ShowAlways = false;
            myTip.Show(app.appDescr, clicked, 10, 10, 1000);
        }

        /// <summary>
        /// Function used to get icon Bitmap object from application resources
        /// </summary>
        /// <param name="name">Application resource name</param>
        /// <returns>Bitmap object containing inputted icon name</returns>
        private Bitmap getBitmapIcon(string name)
        {
            Bitmap res = (Bitmap)Properties.Resources.ResourceManager.GetObject(name);
            return res;
        }
    }

}
