using abbTools.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace abbTools.Shared
{
    public interface IAbbApplication
    {
        /// <summary>
        /// GET or SET property defining application name
        /// </summary>
        string appName { get; }

        /// <summary>
        /// GET or SET property defining application index
        /// </summary>
        int appIndex { get; set; }

        /// <summary>
        /// GET or SET property defininig application icon
        /// </summary>
        string appIcon { get; set; }

        /// <summary>
        /// GET or SET property containing application description
        /// </summary>
        string appDescr { get; set; }

        /// <summary>
        /// GET or SET application window height property
        /// </summary>
        int appHeight { get; set; }

        /// <summary>
        /// GET or SET application window height property
        /// </summary>
        int appWidth { get; set; }
    }

    public class AbbApplicationCollection : List<IAbbApplication>
    {
        object parentPages = null;

        public AbbApplicationCollection(object parent)
        {
            parentPages = parent;
        }

        public void registerApp(IAbbApplication newApp, string descr, string iconName, int height = 709, int width = 1010)
        {
            newApp.appIndex = Count;
            newApp.appIcon = iconName;
            newApp.appHeight = height;
            newApp.appWidth = width;
            newApp.appDescr = descr;
            //add app to collection
            Add(newApp);
        }

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

        private Tuple<int,int> currOffset(int whichElement)
        {
            int threshold = 4, 
                start = (Count <= threshold) ? Count : threshold,
                totalCols = start, 
                totalRows = Count / start;

            int resultCols = whichElement<0 ? totalCols : whichElement % totalCols;
            int resultRows = whichElement<0 ? totalRows : whichElement / totalCols;

            return new Tuple<int,int>(resultCols, resultRows);
        }

        private void AppButton_Click(object sender, System.EventArgs e)
        {
            Button clicked = (Button)sender;
            IAbbApplication app = (IAbbApplication)clicked.Tag;
            //change page of parent
            if (parentPages != null && parentPages.GetType().FullName.Contains("TabControl")) {
                TabControl myPar = (TabControl)parentPages;
                myPar.CausesValidation = false;
                myPar.SelectedIndex = app.appIndex + 1;
                //myPar.CausesValidation = true;
            }
        }

        private Bitmap getBitmapIcon(string name)
        {
            Bitmap res = (Bitmap)Resources.ResourceManager.GetObject(name);
            return res;
        }

        public void reorderPages()
        {
            if (parentPages != null && parentPages.GetType().FullName.Contains("TabControl")) {
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
                    myPar.TabPages[page+1] = newOrder[page];
                }
                //show dashboard
                myPar.SelectTab(0);
            }
        }

    }
}
