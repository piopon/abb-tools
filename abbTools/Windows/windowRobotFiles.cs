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
        Controller abbController;
        public string selectedDir;
        private Form overrideParent;
        private bool showDirs;
        private bool showFile;
        //for listing all files
        private List<int> levels; 
        private string currDir;
        private TreeNode parentNode;
        //private delegate to write from other thread
        private delegate void AddNodeToTreeThread(TreeNode child);

        public windowRobotFiles()
        {
            InitializeComponent();
            //update abbController object
            showDirs = true;
            showFile = true;
            levels = new List<int>();
        }

        public windowRobotFiles(Controller currAbb, bool dir, bool file)
        {
            InitializeComponent();
            //update abbController object
            abbController = currAbb;
            showDirs = dir;
            showFile = file;
            levels = new List<int>();
        }

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

        private void button1_Click(object sender, EventArgs e)
        {
            //clear tree
            treeRobotDirs.Nodes.Clear();
            //get list of all files starting from the top
            levels.Clear();
            currDir = abbController.GetEnvironmentVariable("SYSTEM");
            parentNode = null;
            //sync list of all folders structure 
            exploreFiles(currDir, parentNode);
        }

        private void exploreFiles(string dir, TreeNode node)
        {
            abbController.FileSystem.RemoteDirectory = dir;
            ControllerFileSystemInfo[] dirs = abbController.FileSystem.GetFilesAndDirectories("*");
            for (int i = 0; i < dirs.Length; i++) {
                TreeNode childNode = new TreeNode(dirs[i].Name);
                //add node to tree (check if invoke is required)
                addNode(node, childNode);
                //if current info is a directory then fill it too
                if (dirs[i].GetType() == typeof(ControllerDirectoryInfo)) {
                    levels.Add(i);
                    exploreFiles(dir+"/"+ dirs[i].Name, childNode);
                    levels.RemoveAt(levels.Count - 1);
                }
            }
        }

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
                    if (lvl == 0) {
                        currNode = treeRobotDirs.Nodes[levels[lvl]];
                    } else {
                        currNode = currNode.Nodes[levels[lvl]];
                    }
                }
                currNode.Nodes.Add(child);
            }
        }

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
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            selectedDir = treeRobotDirs.SelectedNode.FullPath;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            selectedDir = "";
            Close();
        }

        private void backThread_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            //get list of all files starting from the top
            levels.Clear();
            currDir = abbController.GetEnvironmentVariable("SYSTEM");
            parentNode = null;
            //sync list of all folders structure 
            exploreFiles(currDir, parentNode);
        }

        private void backThread_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            //check if in mean time disconnection occured
            if (abbController != null) {
                //hide progress bar
                treeRobotDirs.BackColor = Color.White;
            } else {
                //robot disconnected - clear all its signals
                treeRobotDirs.Nodes.Clear();
                treeRobotDirs.BackColor = Color.Silver;
            }
        }
    }
}
