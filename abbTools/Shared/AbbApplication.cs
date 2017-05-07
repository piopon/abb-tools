using abbTools.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace abbTools.Shared
{
    public interface IAbbApplication
    {
        /********************************************************
         ***  ABB APPLICATION - properties
         ********************************************************/

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
}
