
namespace PubgStatsTracker
{
    partial class PubgStatsTracker
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.trackStatsCheckbox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // trackStatsCheckbox
            // 
            this.trackStatsCheckbox.AutoSize = true;
            this.trackStatsCheckbox.Location = new System.Drawing.Point(13, 13);
            this.trackStatsCheckbox.Name = "trackStatsCheckbox";
            this.trackStatsCheckbox.Size = new System.Drawing.Size(101, 19);
            this.trackStatsCheckbox.TabIndex = 0;
            this.trackStatsCheckbox.Text = "Track My Stats";
            this.trackStatsCheckbox.UseVisualStyleBackColor = true;
            // 
            // PubgStatsTracker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.trackStatsCheckbox);
            this.Name = "PubgStatsTracker";
            this.Text = "PubgStatsTracker";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox trackStatsCheckbox;
    }
}

