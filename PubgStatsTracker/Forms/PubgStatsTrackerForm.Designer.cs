
namespace PubgStatsTracker
{
    partial class PubgStatsTrackerForm
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
            this.installButton = new MaterialSkin.Controls.MaterialButton();
            this.SuspendLayout();
            // 
            // installButton
            // 
            this.installButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.installButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.installButton.Depth = 0;
            this.installButton.DrawShadows = true;
            this.installButton.HighEmphasis = true;
            this.installButton.Icon = null;
            this.installButton.Location = new System.Drawing.Point(635, 405);
            this.installButton.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.installButton.MouseState = MaterialSkin.MouseState.HOVER;
            this.installButton.Name = "installButton";
            this.installButton.Size = new System.Drawing.Size(158, 36);
            this.installButton.TabIndex = 0;
            this.installButton.Text = "materialButton1";
            this.installButton.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.installButton.UseAccentColor = false;
            this.installButton.UseVisualStyleBackColor = true;
            this.installButton.Click += new System.EventHandler(this.installButton_Click);
            // 
            // PubgStatsTrackerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.installButton);
            this.Name = "PubgStatsTrackerForm";
            this.Text = "PubgStatsTracker";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MaterialSkin.Controls.MaterialButton installButton;
    }
}

