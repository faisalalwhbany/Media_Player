namespace SmartFplayer
{
    partial class VISTA_Track
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.BLUE_Thumb = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.BLUE_Thumb)).BeginInit();
            this.SuspendLayout();
            // 
            // toolTip
            // 
            this.toolTip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(134)))), ((int)(((byte)(153)))), ((int)(((byte)(190)))));
            this.toolTip.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(73)))), ((int)(((byte)(76)))), ((int)(((byte)(99)))));
            // 
            // BLUE_Thumb
            // 
            this.BLUE_Thumb.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BLUE_Thumb.BackColor = System.Drawing.Color.Blue;
            this.BLUE_Thumb.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BLUE_Thumb.Location = new System.Drawing.Point(102, 0);
            this.BLUE_Thumb.Margin = new System.Windows.Forms.Padding(0);
            this.BLUE_Thumb.Name = "BLUE_Thumb";
            this.BLUE_Thumb.Size = new System.Drawing.Size(10, 10);
            this.BLUE_Thumb.TabIndex = 0;
            this.BLUE_Thumb.TabStop = false;
            this.BLUE_Thumb.Paint += new System.Windows.Forms.PaintEventHandler(this.BLUE_Thumb_Paint);
            this.BLUE_Thumb.MouseDown += new System.Windows.Forms.MouseEventHandler(this.THUMB_MouseDown);
            this.BLUE_Thumb.MouseMove += new System.Windows.Forms.MouseEventHandler(this.THUMB_MouseMove);
            this.BLUE_Thumb.MouseUp += new System.Windows.Forms.MouseEventHandler(this.THUMB_MouseUp);
            // 
            // VISTA_Track
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(29)))), ((int)(((byte)(41)))));
            this.Controls.Add(this.BLUE_Thumb);
            this.Name = "VISTA_Track";
            this.Size = new System.Drawing.Size(227, 10);
            this.Load += new System.EventHandler(this.TRACK_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TRACK_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.THUMB_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.THUMB_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.BLUE_Thumb)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox BLUE_Thumb;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
