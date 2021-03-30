
namespace PubgStatsTracker
{
    partial class UninstallForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.deleteLogsCheckbox = new System.Windows.Forms.CheckBox();
            this.deleteHistoryCheckbox = new System.Windows.Forms.CheckBox();
            this.deleteConfigCheckbox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.uninstallButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // deleteLogsCheckbox
            // 
            this.deleteLogsCheckbox.AutoSize = true;
            this.deleteLogsCheckbox.Checked = true;
            this.deleteLogsCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.deleteLogsCheckbox.Location = new System.Drawing.Point(12, 12);
            this.deleteLogsCheckbox.Name = "deleteLogsCheckbox";
            this.deleteLogsCheckbox.Size = new System.Drawing.Size(84, 19);
            this.deleteLogsCheckbox.TabIndex = 0;
            this.deleteLogsCheckbox.Text = "Delete logs";
            this.deleteLogsCheckbox.UseVisualStyleBackColor = true;
            // 
            // deleteHistoryCheckbox
            // 
            this.deleteHistoryCheckbox.AutoSize = true;
            this.deleteHistoryCheckbox.Location = new System.Drawing.Point(12, 64);
            this.deleteHistoryCheckbox.Name = "deleteHistoryCheckbox";
            this.deleteHistoryCheckbox.Size = new System.Drawing.Size(364, 19);
            this.deleteHistoryCheckbox.TabIndex = 1;
            this.deleteHistoryCheckbox.Text = "Delete my match history (nuclear option, NOT RECOMMENDED)";
            this.deleteHistoryCheckbox.UseVisualStyleBackColor = true;
            // 
            // deleteConfigCheckbox
            // 
            this.deleteConfigCheckbox.AutoSize = true;
            this.deleteConfigCheckbox.Checked = true;
            this.deleteConfigCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.deleteConfigCheckbox.Location = new System.Drawing.Point(12, 39);
            this.deleteConfigCheckbox.Name = "deleteConfigCheckbox";
            this.deleteConfigCheckbox.Size = new System.Drawing.Size(134, 19);
            this.deleteConfigCheckbox.TabIndex = 2;
            this.deleteConfigCheckbox.Text = "Delete configuration";
            this.deleteConfigCheckbox.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 98);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(329, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "The PubgStatsTrackerService will be uninstalled automatically";
            // 
            // uninstallButton
            // 
            this.uninstallButton.Location = new System.Drawing.Point(303, 122);
            this.uninstallButton.Name = "uninstallButton";
            this.uninstallButton.Size = new System.Drawing.Size(75, 23);
            this.uninstallButton.TabIndex = 4;
            this.uninstallButton.Text = "Uninstall";
            this.uninstallButton.UseVisualStyleBackColor = true;
            this.uninstallButton.Click += new System.EventHandler(this.uninstallButton_Click);
            // 
            // UninstallForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 151);
            this.Controls.Add(this.uninstallButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.deleteConfigCheckbox);
            this.Controls.Add(this.deleteHistoryCheckbox);
            this.Controls.Add(this.deleteLogsCheckbox);
            this.Name = "UninstallForm";
            this.Text = "UninstallForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox deleteLogsCheckbox;
        private System.Windows.Forms.CheckBox deleteHistoryCheckbox;
        private System.Windows.Forms.CheckBox deleteConfigCheckbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button uninstallButton;
    }
}