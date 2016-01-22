using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace SmartFplayer
{
    public partial class PraysTimes : Form
    {
        Point isPoint = new Point();
        public PraysTimes()
        {
            InitializeComponent();
            textDawn.Text = Properties.Settings.Default.DawnTime.ToLongTimeString();
            textNoon.Text = Properties.Settings.Default.NoonTime.ToLongTimeString();
            textANoon.Text = Properties.Settings.Default.ANoonTime.ToLongTimeString();
            textSunset.Text = Properties.Settings.Default.SunsetTime.ToLongTimeString();
            textNight.Text = Properties.Settings.Default.NightTime.ToLongTimeString();
            numericUpDown16.Value = Properties.Settings.Default.WaitTime.Minutes;
        }

        private void PraysTimes_Paint(object sender, PaintEventArgs e)
        {

            using (GraphicsPath Path = new GraphicsPath())
            {
                
                using (LinearGradientBrush lgb = new LinearGradientBrush(ClientRectangle,
                   Properties.Settings.Default.panelMoveColorLight,
                   Properties.Settings.Default.panelMoveColorDark, 90, true))
                {
                    lgb.SetBlendTriangularShape(0.5f);
                    Path.AddArc(0, 0, 30, 30, 180, 90);
                    Path.AddArc(this.ClientRectangle.Width - 30, 0, 30, 30, 270, 90);
                    Path.AddArc(this.ClientRectangle.Width - 30, this.ClientRectangle.Height - 30, 30, 30, 360, 90);
                    Path.AddArc(0, this.ClientRectangle.Height - 30, 30, 30, 90, 90);
                    this.Region = new Region(Path);
                    
                    e.Graphics.FillRegion(lgb, this.Region);
                }
            }
            
        }
        private void Loop_MouseMove(object sender, MouseEventArgs e)
        {
            if ((MouseButtons.Left & e.Button) == MouseButtons.Left)
            {
                int x = this.Location.X + (e.X - isPoint.X);
                int y = this.Location.Y + (e.Y - isPoint.Y);

                this.Location = new Point(x, y);
            }
        }

        private void Loop_MouseDown(object sender, MouseEventArgs e)
        {
            if ((MouseButtons.Left & e.Button) == MouseButtons.Left)
            {
                isPoint.X = e.X;
                isPoint.Y = e.Y;
            }
        }
    }
}
