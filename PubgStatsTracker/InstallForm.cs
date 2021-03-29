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
            string exeName = AppConfig.DefaultName + ".exe";
            string installExe = Path.Combine(installModel.InstallLocation, exeName);

            System.IO.File.Copy(
                AppConfig.ExePath,
                installExe
            );

            new UserConfiguration().Save(installModel.InstallLocation);

            if (installModel.CreateDesktopShortcut)
            {
                string shortcutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), AppConfig.DefaultName + ".url");
                using StreamWriter writer = new(shortcutPath);
                writer.WriteLine("[InternetShortcut]");
                writer.WriteLine("URL=file:///" + installExe);
                writer.WriteLine("IconIndex=0");
                writer.WriteLine("IconFile=" + installExe.Replace('\\', '/'));
            }

            if (installModel.CreateStartMenuShortcut)
            {
                WshShellClass shellClass = new();
                string shortcutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Programs), AppConfig.DefaultName + ".lnk");
                IWshShortcut shortcut = (IWshShortcut)shellClass.CreateShortcut(shortcutPath);
                shortcut.TargetPath = installExe;
                shortcut.IconLocation = installExe.Replace('\\', '/');
                shortcut.Save();
            }

            if (!AppConfig.DoesServiceExist)
            {
                ProcessStartInfo serviceCreator = new("sc.exe", $"create {AppConfig.ServiceName} start= delayed-auto displayName= \"{AppConfig.ServiceName}\" binpath= \"{installExe} -s\"");
                Process.Start(serviceCreator);
                ProcessStartInfo startService = new("sc.exe", $"start {AppConfig.ServiceName}");
                Process.Start(startService);
            }
        }
    }
}
