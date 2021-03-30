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
    public partial class PubgStatsTrackerForm : Form
    {
        public PubgStatsTrackerForm()
        {
            InitializeComponent();
            installButton.Text = AppConfig.Config.Installed ? "Uninstall" : "Install";
            if (!AppConfig.IsElevated)
            {
                SetButtonShield(installButton, true);
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(HandleRef hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        private static void SetButtonShield(Button btn, bool showShield)
        {
            // from https://wyday.com/blog/2009/using-shield-icons-uac-and-process-elevation-in-csharp-vb-net-on-windows-2000-xp-vista-and-7/
            //Note: make sure the button FlatStyle = FlatStyle.System
            // BCM_SETSHIELD = 0x0000160C
            SendMessage(new HandleRef(btn, btn.Handle), 0x160C, IntPtr.Zero, showShield ? new IntPtr(1) : IntPtr.Zero);
        }

        private void installButton_Click(object sender, EventArgs e)
        {
            const string adminMessage = "The program must be started with admin privileges to (un)install. Click OK to relaunch with admin privileges";
            if (AppConfig.IsElevated)
            {
                if (AppConfig.DoesServiceExist)
                {
                    new UninstallForm().ShowDialog();
                }
                else
                {
                    new InstallForm().ShowDialog();
                }
            }
            else if (MessageBox.Show(adminMessage, "Run as admin?", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Program.RestartElevated();
            }
        }
    }
}