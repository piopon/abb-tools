using System;
using System.Drawing;
using System.Windows.Forms;
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.FileSystemDomain;
using System.Collections.Generic;

namespace abbTools.Windows
{
    public partial class windowRobotFiles : Form
    {
        /********************************************************
         ***  WINDOW ROBOT FILES - fields
         ********************************************************/

        //ABB controller parent of files
        Controller abbController;
        //current path
        public string selectedDir;
        private TreeNode myNode;
        //viewing semi-transparent forms
        private Form overrideParent;
        private bool showDirs;
        private bool showFile;
        //for listing all files
        private List<int> levels; 
        private string currDir;
        private TreeNode parentNode;
        //private delegate to write from other thread
        private delegate void AddNodeToTreeThread(TreeNode child);
        private delegate void SetNodeImageThread(TreeNode node, int index);

        /********************************************************
         ***  WINDOW ROBOT FILES - constructors
         ********************************************************/

        /// <summary>
        /// Default constructor
        /// </summary>
        public windowRobotFiles()
        {
            InitializeComponent();
            //clear list (design tests)
            treeRobotDirs.Nodes.Clear();
            treeRobotDirs.BackColor = Color.Silver;
            //current path
            myNode = null;
            //update abbController object
            showDirs = true;
            showFile = true;
            levels = new List<int>();
        }

        /// <summary>
        /// Constructor with field inits
        /// </summary>
        /// <param name="currAbb">ABB Controller object as parent of dirs and files</param>
        /// <param name="dir">Input if show directories</param>
        /// <param name="file">Input if show files</param>
        public windowRobotFiles(Controller currAbb, bool dir, bool file)
        {
            InitializeComponent();
            //clear list (design tests)
            treeRobotDirs.Nodes.Clear();
            treeRobotDirs.BackColor = Color.Silver;
            //current path
            myNode = null;
            //update abbController object
            abbController = currAbb;
            showDirs = dir;
            showFile = file;
            levels = new List<int>();
        }

        /********************************************************
         ***  WINDOW ROBOT FILES - form events
         ********************************************************/

        /// <summary>
        /// Event triggerd on panel tempate paint
        /// </summary>
        /// <param name="sender">Panel parent object which triggered tihs event</param>
        /// <param name="e">Event arguments</param>
        private void panelTemplate_Paint(object sender, PaintEventArgs e)
        {
            //set the panel position
            panelTemplate.Left = (Width - panelTemplate.Width) / 2;
            panelTemplate.Top = (Height - panelTemplate.Height) / 2;
            //we want semi-transparent background and opaque controls - we override it with new form
            overrideParent = new Form();
            overrideParent.ShowInTaskbar = false;
            overrideParent.FormBorderStyle = FormBorderStyle.None;
            overrideParent.Opacity = 100;
            overrideParent.Size = panelTemplate.Size;
            overrideParent.StartPosition = FormStartPosition.Manual;
            overrideParent.Location = panelTemplate.PointToScreen(new Point(0, 0));
            overrideParent.Show(this);
            //new form contains user GUI panel
            panelContent.Parent = overrideParent;
            panelContent.Dock = DockStyle.Fill;
            //set focus to tree
            treeRobotDirs.SelectedNode = myNode;
            treeRobotDirs.Focus();
        }

        /// <summary>
        /// Event triggered on robot files form shown
        /// </summary>
        /// <param name="sender">Form parent object that triggered this event</param>
        /// <param name="e">Event arguments</param>
        private void windowRobotFiles_Shown(object sender, EventArgs e)
        {
            //select current path
            treeRobotDirs.SelectedNode = myNode;
        }

        /// <summary>
        /// Event triggered on robot files form closed
        /// </summary>
        /// <param name="sender">Form parent object that triggered this event</param>
        /// <param name="e">Event arguments</param>
        private void windowRobotFiles_FormClosed(object sender, FormClosedEventArgs e)
        {
            overrideParent.Controls.Clear();
            overrideParent.Close();
            overrideParent = null;
        }

        /********************************************************
         ***  WINDOW ROBOT FILES - form methods
         ********************************************************/

        /// <summary>
        /// Method used to get ABB controller files and dirs
        /// </summary>
        /// <param name="cController">ABB Controller parent object to get dirs and files from</param>
        public void updateFiles(Controller cController)
        {
            //update controller address
            abbController = cController;
            //check if background worker is running
            if (!backThread.IsBusy) {
                //clear tree
                treeRobotDirs.Nodes.Clear();
                //run background thread
                backThread.RunWorkerAsync(treeRobotDirs);
            }
        }

        /// <summary>
        /// Method used to show whole files and dirs structure in tree control
        /// </summary>
        /// <param name="dir">Current robot path</param>
        /// <param name="node">Current node to be added</param>
        private void exploreFiles(string dir, TreeNode node)
        {
            abbController.FileSystem.RemoteDirectory = dir;
            ControllerFileSystemInfo[] dirs = abbController.FileSystem.GetFilesAndDirectories("*");
            //scan all folders in current
            for (int i = 0; i < dirs.Length; i++) {
                TreeNode childNode = new TreeNode(dirs[i].Name);
                //if current directory is our backup source then remember current node as selected
                if (isSrc(dir + "/" + dirs[i].Name)) myNode = childNode;
                //add node to tree (check if invoke is required)
                addNode(node, childNode);
                //if current info is a directory then fill it too
                if (dirs[i].GetType() == typeof(ControllerDirectoryInfo)) {
                    levels.Add(i);
                    exploreFiles(dir + "/" + dirs[i].Name, childNode);
                    levels.RemoveAt(levels.Count - 1);
                    //update node image = folder
                    nodeImage(childNode, 1);
                } else {
                    //update node image = files
                    nodeImage(childNode, 2);
                }
            }
        }

        /// <summary>
        /// Method used to add node to selected parent tree
        /// </summary>
        /// <param name="nodeParent">Parent tree node</param>
        /// <param name="nodeChild">Node to be added to tree</param>
        private void addNode(TreeNode nodeParent, TreeNode nodeChild)
        {
            if (treeRobotDirs.InvokeRequired) {
                AddNodeToTreeThread threadSafe = new AddNodeToTreeThread(addNodeThreadSafe);
                treeRobotDirs.Invoke(threadSafe, new object[] { nodeChild });
            } else {
                if (nodeParent == null) {
                    treeRobotDirs.Nodes.Add(nodeChild);
                } else {
                    nodeParent.Nodes.Add(nodeChild);
                }
            }
        }

        /// <summary>
        /// Method used to change image of selected tree node
        /// </summary>
        /// <param name="node">Node parent to change image index of</param>
        /// <param name="imgIndex">New image index</param>
        private void nodeImage(TreeNode node, int imgIndex)
        {
            if (treeRobotDirs.InvokeRequired) {
                SetNodeImageThread threadSafe = new SetNodeImageThread(nodeImageThreadSafe);
                treeRobotDirs.Invoke(threadSafe, new object[] { node, imgIndex });
            } else {
                node.ImageIndex = imgIndex;
                node.SelectedImageIndex = imgIndex;
            }
        }

        /// <summary>
        /// Function used to check if inputted path is the same as backup source path
        /// </summary>
        /// <param name="path">Path to check</param>
        /// <returns>TRUE if path the same as backup source, FALSE otherwise</returns>
        private bool isSrc(string path)
        {
            return path.Substring(currDir.Length + 1).Replace("/", "\\") == selectedDir;
        }

        /********************************************************
         ***  WINDOW ROBOT FILES - button events
         ********************************************************/

        /// <summary>
        /// Event triggered by user click button OK
        /// </summary>
        /// <param name="sender">Button that triggered current event</param>
        /// <param name="e">Event arguments</param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            //update myNode object
            myNode = treeRobotDirs.SelectedNode;
            //update selectedDir object
            selectedDir = myNode?.FullPath;
            Close();
        }

        /// <summary>
        /// Event triggered by user click button CANCEL
        /// </summary>
        /// <param name="sender">Button that triggered current event</param>
        /// <param name="e">Event arguments</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            //dont change selected dir
            Close();
        }

        /// <summary>
        /// Event triggered by user click button CLICK
        /// </summary>
        /// <param name="sender">Button that triggered current event</param>
        /// <param name="e">Event arguments</param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            selectedDir = "";
            treeRobotDirs.SelectedNode = null;
        }

        /// <summary>
        /// Event triggered by user click button UPDATE
        /// </summary>
        /// <param name="sender">Button that triggered current event</param>
        /// <param name="e">Event arguments</param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //clear tree
            treeRobotDirs.Nodes.Clear();
            treeRobotDirs.BackColor = Color.Silver;
            //get list of all files starting from the top
            levels.Clear();
            currDir = abbController.GetEnvironmentVariable("SYSTEM");
            parentNode = null;
            //sync list of all folders structure 
            exploreFiles(currDir, parentNode);
        }

        /********************************************************
         ***  WINDOW ROBOT FILES - background thread functions
         ********************************************************/

        /// <summary>
        /// Event triggerd on start background thread action
        /// </summary>
        /// <param name="sender">Background thread parent that triggered current action</param>
        /// <param name="e">Event arguments</param>
        private void backThread_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            //get list of all files starting from the top
            levels.Clear();
            currDir = abbController.GetEnvironmentVariable("SYSTEM");
            parentNode = null;
            //sync list of all folders structure 
            exploreFiles(currDir, parentNode);
        }

        /// <summary>
        /// Event triggerd on end background thread action
        /// </summary>
        /// <param name="sender">Background thread parent that triggered current action</param>
        /// <param name="e">Event arguments</param>
        private void backThread_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            //check if in mean time disconnection occured
            if (abbController != null) {
                //hide progress bar
                treeRobotDirs.BackColor = Color.White;
                //set focus to tree and refresh selected node (useful if user shows window before it loads files)
                treeRobotDirs.SelectedNode = myNode;
                treeRobotDirs.Focus();
            } else {
                //robot disconnected - clear all its signals
                treeRobotDirs.Nodes.Clear();
                treeRobotDirs.BackColor = Color.Silver;
            }
        }

        /// <summary>
        /// Method used to add node from background thread (thread safe method)
        /// </summary>
        /// <param name="child">Node to be added to tree</param>
        private void addNodeThreadSafe(TreeNode child)
        {
            TreeNode currNode = null; ;
            int levelsCount = levels.Count;
            //check how deep we are
            if (levelsCount == 0) {
                //if only one level then we are at top - add directly to tree
                treeRobotDirs.Nodes.Add(child);
            } else {
                //more than one level - add it to path
                for (int lvl = 0; lvl < levelsCount; lvl++) {
                    currNode = (lvl == 0) ? treeRobotDirs.Nodes[levels[lvl]] : currNode.Nodes[levels[lvl]];
                }
                currNode.Nodes.Add(child);
            }
        }

        /// <summary>
        /// Method used to change node image from background thread (thread safe method)
        /// </summary>
        /// <param name="myNode">Node parent to change image index of</param>
        /// <param name="myIndex">New image index</param>
        private void nodeImageThreadSafe(TreeNode myNode, int myIndex)
        {
            myNode.ImageIndex = myIndex;
            myNode.SelectedImageIndex = myIndex;
        }
    }
}
