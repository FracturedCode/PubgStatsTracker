﻿using PubgStatsTracker.Models;
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
            deleteEverythingCheckbox.DataBindings.Add(nameof(CheckBox.Checked), uninstallModel, nameof(uninstallModel.DeleteEverything));
            deleteShortcutsCheckbox.DataBindings.Add(nameof(CheckBox.Checked), uninstallModel, nameof(uninstallModel.DeleteShortcuts));
        }

        private void uninstallButton_Click(object sender, EventArgs e)
        {
            Installer.Uninstall(uninstallModel);
        }
    }
}
