
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
            this.uninstallButton = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.deleteShortcutsCheckbox = new System.Windows.Forms.CheckBox();
            this.deleteEverythingCheckbox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
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
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Enabled = false;
            this.checkBox1.Location = new System.Drawing.Point(13, 13);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(111, 19);
            this.checkBox1.TabIndex = 5;
            this.checkBox1.Text = "Uninstall service";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // deleteShortcutsCheckbox
            // 
            this.deleteShortcutsCheckbox.AutoSize = true;
            this.deleteShortcutsCheckbox.Checked = true;
            this.deleteShortcutsCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.deleteShortcutsCheckbox.Location = new System.Drawing.Point(13, 39);
            this.deleteShortcutsCheckbox.Name = "deleteShortcutsCheckbox";
            this.deleteShortcutsCheckbox.Size = new System.Drawing.Size(111, 19);
            this.deleteShortcutsCheckbox.TabIndex = 6;
            this.deleteShortcutsCheckbox.Text = "Delete shortcuts";
            this.deleteShortcutsCheckbox.UseVisualStyleBackColor = true;
            // 
            // deleteEverythingCheckbox
            // 
            this.deleteEverythingCheckbox.AutoSize = true;
            this.deleteEverythingCheckbox.Location = new System.Drawing.Point(13, 65);
            this.deleteEverythingCheckbox.Name = "deleteEverythingCheckbox";
            this.deleteEverythingCheckbox.Size = new System.Drawing.Size(118, 19);
            this.deleteEverythingCheckbox.TabIndex = 7;
            this.deleteEverythingCheckbox.Text = "Delete everything";
            this.deleteEverythingCheckbox.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(163, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(215, 30);
            this.label1.TabIndex = 8;
            this.label1.Text = "Delete everything is not recommended;\n you will lose all your match history";
            // 
            // UninstallForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 151);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.deleteEverythingCheckbox);
            this.Controls.Add(this.deleteShortcutsCheckbox);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.uninstallButton);
            this.Name = "UninstallForm";
            this.Text = "UninstallForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button uninstallButton;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox deleteShortcutsCheckbox;
        private System.Windows.Forms.CheckBox deleteEverythingCheckbox;
        private System.Windows.Forms.Label label1;
    }
}