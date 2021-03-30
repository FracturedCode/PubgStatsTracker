using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IWshRuntimeLibrary;

namespace PubgStatsTracker
{
    public partial class InstallForm : Form
    {
        private InstallModel installModel { get; init; }
        public InstallForm()
        {
            installModel = new();
            InitializeComponent();
            desktopShortcutCheckbox.DataBindings.Add(nameof(CheckBox.Checked), installModel, nameof(InstallModel.CreateDesktopShortcut));
            startMenuShortcutCheckbox.DataBindings.Add(nameof(CheckBox.Checked), installModel, nameof(InstallModel.CreateStartMenuShortcut));
            installLocationTextbox.DataBindings.Add(nameof(TextBox.Text), installModel, nameof(InstallModel.InstallLocation));
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new();
            folderBrowser.Description = "Install Location";
            folderBrowser.ShowNewFolderButton = true;
            if (folderBrowser.ShowDialog(this) == DialogResult.OK)
            {
                installLocationTextbox.Text = Path.Combine(folderBrowser.SelectedPath, AppConfig.DefaultName);
            }
        }

        private void installButton_Click(object sender, EventArgs e)
        {
            Program.Install(installModel);
        }
    }
}
