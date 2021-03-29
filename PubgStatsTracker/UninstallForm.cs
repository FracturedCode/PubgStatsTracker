using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PubgStatsTracker
{
    public partial class UninstallForm : Form
    {
        private UninstallModel uninstallModel { get; init; }
        public UninstallForm()
        {
            uninstallModel = new();
            InitializeComponent();
            deleteLogsCheckbox.DataBindings.Add(nameof(CheckBox.Checked), uninstallModel, nameof(UninstallModel.DeleteLogs));
            deleteConfigCheckbox.DataBindings.Add(nameof(CheckBox.Checked), uninstallModel, nameof(UninstallModel.DeleteConfig));
            deleteHistoryCheckbox.DataBindings.Add(nameof(CheckBox.Checked), uninstallModel, nameof(UninstallModel.DeleteMatchHistory));
        }

        private void uninstallButton_Click(object sender, EventArgs e)
        {
            string deleteLogsCmd = "rmdir /s logs";
            string deleteConfigCmd = $"del {AppConfig.ConfigFile}";
            string deleteHistoryCmd = $""; //TODO
            List<string> arguments = new(){ "/c ping localhost -n 3 > nul", $"cd {AppDomain.CurrentDomain.BaseDirectory}" };
            if (uninstallModel.DeleteLogs)
                arguments.Add(deleteLogsCmd);
            if (uninstallModel.DeleteConfig)
                arguments.Add(deleteConfigCmd);
            //if (uninstallModel.DeleteMatchHistory)
            // arguments.
            // TODO shortcuts
            arguments.Add($"del {AppConfig.DefaultName}.exe");
            string concatdArguments = arguments.Aggregate((x, y) => $"{x} & {y}");

            ProcessStartInfo psi = new()
            {
                UseShellExecute = true,
                Verb = "runas",
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = concatdArguments
            };
            Process.Start(psi);
        }
    }
}
