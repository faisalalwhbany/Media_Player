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
    public partial class Loop : Form
    {

        Point isPoint = new Point();
        public Loop()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int h =Convert.ToInt32(textHSt.Value);
            int m = Convert.ToInt32(textMSt.Value);
            int s = Convert.ToInt32(textSSt.Value);
            int h2 = Convert.ToInt32(textHEn.Value);
            int m2 = Convert.ToInt32(textMEn.Value);
            int s2 = Convert.ToInt32(textSEn.Value);
            int x = (h * 3600000) + (m * 60000) + (s*1000);
            int x1 = (h2 * 3600000) + (m2 * 60000) + (s2*1000);
            if (x >= x1)
            {
                MessageBox.Show("يجب أن يكون زمن البداية أقل من زمن النهاية");

            }
            else
            {
               this.button1.DialogResult = DialogResult.OK;
            }

        }

        private void Loop_Paint(object sender, PaintEventArgs e)
        {
            using (GraphicsPath Path = new GraphicsPath())
            {
                //LinearGradientBrush lgb=new LinearGradientBrush(this.Region,
                using (LinearGradientBrush lgb = new LinearGradientBrush(ClientRectangle,
                   Properties.Settings.Default.panelMoveColorLight, Properties.Settings.Default.panelMoveColorDark, 90, true))
                {
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

        private void Loop_Load(object sender, EventArgs e)
        {
            //panel1.BackColor = Color.FromArgb(100, 255, 0, 255);
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            button1.BackColor = Color.Red;
            button1.ForeColor = Color.Yellow;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.BackColor = Color.Blue;
            button1.ForeColor = Color.White;
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
        
            button2.BackColor = Color.Red;
            button2.ForeColor = Color.Yellow;
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
        button2.BackColor = Color.Blue;
            button2.ForeColor = Color.White;
        }
        

        
    }
}
