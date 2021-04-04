using MaterialSkin;
using MaterialSkin.Controls;
using PubgStatsTracker.BusinessLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PubgStatsTracker
{
    public partial class PubgStatsTrackerForm : MaterialForm
    {
        public PubgStatsTrackerForm()
        {
            MaterialSkinManager.Instance.AddFormToManage(this);
            InitializeComponent();
            
            installButton.Text = AppState.DoesServiceExist ? "Uninstall" : "Install";
        }

        private void installButton_Click(object sender, EventArgs e)
        {
            if (AppState.DoesServiceExist)
            {
                new UninstallForm().ShowDialog();
            }
            else
            {
                new InstallForm().ShowDialog();
            }
        }
    }
}