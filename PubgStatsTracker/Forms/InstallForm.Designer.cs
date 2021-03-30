
namespace PubgStatsTracker
{
    partial class InstallForm
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
            this.installLocationTextbox = new System.Windows.Forms.TextBox();
            this.browseButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.startMenuShortcutCheckbox = new System.Windows.Forms.CheckBox();
            this.desktopShortcutCheckbox = new System.Windows.Forms.CheckBox();
            this.installButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // installLocationTextbox
            // 
            this.installLocationTextbox.Location = new System.Drawing.Point(106, 12);
            this.installLocationTextbox.Name = "installLocationTextbox";
            this.installLocationTextbox.Size = new System.Drawing.Size(331, 23);
            this.installLocationTextbox.TabIndex = 0;
            // 
            // browseButton
            // 
            this.browseButton.Location = new System.Drawing.Point(443, 11);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(75, 23);
            this.browseButton.TabIndex = 1;
            this.browseButton.Text = "Browse";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Install Location";
            // 
            // startMenuShortcutCheckbox
            // 
            this.startMenuShortcutCheckbox.AutoSize = true;
            this.startMenuShortcutCheckbox.Checked = true;
            this.startMenuShortcutCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.startMenuShortcutCheckbox.Location = new System.Drawing.Point(12, 44);
            this.startMenuShortcutCheckbox.Name = "startMenuShortcutCheckbox";
            this.startMenuShortcutCheckbox.Size = new System.Drawing.Size(131, 19);
            this.startMenuShortcutCheckbox.TabIndex = 3;
            this.startMenuShortcutCheckbox.Text = "Start menu shortcut";
            this.startMenuShortcutCheckbox.UseVisualStyleBackColor = true;
            // 
            // desktopShortcutCheckbox
            // 
            this.desktopShortcutCheckbox.AutoSize = true;
            this.desktopShortcutCheckbox.Location = new System.Drawing.Point(12, 69);
            this.desktopShortcutCheckbox.Name = "desktopShortcutCheckbox";
            this.desktopShortcutCheckbox.Size = new System.Drawing.Size(116, 19);
            this.desktopShortcutCheckbox.TabIndex = 4;
            this.desktopShortcutCheckbox.Text = "Desktop shortcut";
            this.desktopShortcutCheckbox.UseVisualStyleBackColor = true;
            // 
            // installButton
            // 
            this.installButton.Location = new System.Drawing.Point(443, 65);
            this.installButton.Name = "installButton";
            this.installButton.Size = new System.Drawing.Size(75, 23);
            this.installButton.TabIndex = 5;
            this.installButton.Text = "Install";
            this.installButton.UseVisualStyleBackColor = true;
            this.installButton.Click += new System.EventHandler(this.installButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 15);
            this.label2.TabIndex = 6;
            // 
            // InstallForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(528, 89);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.installButton);
            this.Controls.Add(this.desktopShortcutCheckbox);
            this.Controls.Add(this.startMenuShortcutCheckbox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.installLocationTextbox);
            this.Name = "InstallForm";
            this.Text = "InstallForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox installLocationTextbox;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox startMenuShortcutCheckbox;
        private System.Windows.Forms.CheckBox desktopShortcutCheckbox;
        private System.Windows.Forms.Button installButton;
        private System.Windows.Forms.Label label2;
    }
}