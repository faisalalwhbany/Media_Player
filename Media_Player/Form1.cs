
/* فيصل الوهباني */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Drawing.Imaging;
using Skin;
using System.Xml;
using System.Windows.Forms;
using ID3;
using System.Drawing.Drawing2D;
using System.IO;
using Microsoft.Win32;


namespace SmartFplayer
{
    public partial class SFplayer : Form
    {

        private enum loop:byte
        {
            none,
            All,
            On

        }

        #region Variables PrayTimer
        private bool IsPraysTimer = false;
        private Boolean IsPlayInPrays = false;
        private DateTime dat = new DateTime();
        private DateTime dat2 = new DateTime();
        private DateTime dat3 = new DateTime();
        private DateTime dat4 = new DateTime();
        private DateTime dat5 = new DateTime();

        private TimeSpan wait = new TimeSpan();

        #endregion



        #region bonice Variables

        // Structure to hold a shape's data
        private struct Shape
        {
            public int xPos;
            public int yPos;
            public int xDelta;
            public int yDelta;
            public Rectangle rect;
        }

        // Enumeration shapes form
        enum ShapeForm
        {
            Round,
            Square,
            Diamond,
            Star
        }

        private Bitmap[] img;
        private Image[] img2;

        private bool isDrawing = false; // indicates shapes are being drawn

        private Shape[] shapes; // the shapes array
        private int shapeCount = 6; // the number of shapes

        private ShapeForm shapeForm = ShapeForm.Square; // the shape form
        Point[] diamondPoints = new Point[4]; // used with drawing Diamond shape
        Point[] starPoints = new Point[8]; // used with drawing Star shape

        private int shapeSize = 3; // the size of the shapes (overlay width or height divided by this)
        

        private bool moveShapes = false; // used to move shapes only by timer tick event (not with resize)

        private const int BASE_XDELTA = 8; // the x movement (speed) of the first shape (in pixels)
        private const int BASE_YDELTA = 10; // the y movement (speed) of the first shape (in pixels)
        // This makes the bubbles move in (slightly) different ways:
        private const int DIFF_XDELTA = 1; // the x speed added to every next shape
        private const int DIFF_YDELTA = 1; // the y speed added to every next shape

        

        //private Timer timer1;
        //private const int TIMER_INTERVAL = 25;

        // Interacting with the player


        #endregion
       

        #region player variable

        private  string[] exp =  {".mp3",".mp4",".flv",".avi",".wav",".amr",".rm",
                                          ".ram",".mov",".wma",".wmv",".mkv",".rmvb",
                                          ".vob",".mpg",".mpeg",".3gp",".3gpp",".dat"};
        private string PathPict;
        private DirectoryInfo directoryInfo;
        private string[] searchExtensions = { ".jpg", ".jpeg", ".bmp", ".png", ".gif" };
        private List<FileInfo> imgBonice = new List<FileInfo>();
        private loop looping = loop.none;
        private string Visual ="none";
        private Boolean TrackScroll = false;
        Fplayer ap= new Fplayer();
        private int current=0;
        private TimeSpan length;
        private Boolean play = false;
       private Boolean mut = false;
        private Boolean list_show = false;
        private string endPlay = "none";
        //private  bool isRunning = false;
        //private bool Holde = false;
        //private bool isTop = false;
        //private bool showWind = false;
        private string theme = "";
        
        public List<string> albume = new List<string>();
        XmlDocument doc = new XmlDocument();
        //float Aspect;
        private int pleft;
        //public const int WM_ERASEBKGND = 0x14;

        private bool isLoop = false;
        private double LoopStart;
        private double loopEnd;
       
        About about = new About();
        private Bitmap bmp_play = new Bitmap(Reso.play2);
        private Bitmap bmp_play2 = new Bitmap(Reso.play1);
        private Bitmap bmp_paus = new Bitmap(Reso.pause2);
        private Bitmap bmp_paus2 = new Bitmap(Reso.pause1);
        private Bitmap bmp_stop = new Bitmap(Reso.stop1);
        private Bitmap bmp_stop2 = new Bitmap(Reso.stop21);
        private Bitmap bmp_back = new Bitmap(Reso.prev1);
        private Bitmap bmp_back2 = new Bitmap(Reso.prev2);
        private Bitmap bmp_forw = new Bitmap(Reso.next11);
        private Bitmap bmp_forw2 = new Bitmap(Reso.next21);
        private Bitmap bmp_full = new Bitmap(Reso.full);
        private Bitmap bmp_full2 = new Bitmap(Reso.full2);
       

        PictureBox pict = new PictureBox();
        
        private skine sk;
        // visualion view = new visualion();

        ID3v2 Mp3Info;
        #endregion variable

        #region constractor

        public SFplayer(string file)
        {
            
            
            InitializeComponent();
            ap.Owner = p1;
            this.SetStyle(ControlStyles.ResizeRedraw|ControlStyles.AllPaintingInWmPaint|ControlStyles.OptimizedDoubleBuffer|ControlStyles.UserPaint, true);
            this.MinimumSize = new Size(this.Width, this.Height);
           // Aspect = (float)p1.ClientSize.Width / (float)p1.ClientSize.Height;
            pleft = p1.Left;
            
            try
            {
                if (file.Length != 0)
                {
                    
                    albume.Add(file);

                    
                    this.Show();
                    LoadFile(file);
                    FillList();
                    //SetTrackBar();
                    PlayFile();
                    //this.Invalidate();
                    
                }
            }
            catch { }
            if (Properties.Settings.Default.pathDict.Equals(""))
            {
                
                Properties.Settings.Default.pathDict = @Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\F_Player\Capture";
                Properties.Settings.Default.Save();

            }
            if (Properties.Settings.Default.VisualMode== "pict")
            ShapesInit();
            
           
        }
        #endregion constractor


        #region method PraysTimer


        private void PraryTime()
        {
            if (((DateTime.Now.ToLongTimeString().CompareTo(dat.ToLongTimeString()) >= 0) &&
                DateTime.Now.ToLongTimeString().CompareTo(dat.Add(wait).ToLongTimeString()) <= 0) ||
                ((DateTime.Now.ToLongTimeString().CompareTo(dat2.ToLongTimeString()) >= 0) &&
                DateTime.Now.ToLongTimeString().CompareTo(dat2.Add(wait).ToLongTimeString()) <= 0) ||
                ((DateTime.Now.ToLongTimeString().CompareTo(dat3.ToLongTimeString()) >= 0) &&
                DateTime.Now.ToLongTimeString().CompareTo(dat3.Add(wait).ToLongTimeString()) <= 0) ||
                ((DateTime.Now.ToLongTimeString().CompareTo(dat4.ToLongTimeString()) >= 0) &&
                DateTime.Now.ToLongTimeString().CompareTo(dat4.Add(wait).ToLongTimeString()) <= 0) ||
                ((DateTime.Now.ToLongTimeString().CompareTo(dat5.ToLongTimeString()) >= 0) &&
                DateTime.Now.ToLongTimeString().CompareTo(dat5.Add(wait).ToLongTimeString()) <= 0))
            {

                if (ap.IsPlaying)
                {
                    PausFile();
                    button2.BackgroundImage = bmp_play2;
                    IsPlayInPrays = true;
                }
                panel_Control.Enabled = false;
                LockControlToolStripMenuItem.Checked = true;
                LockControlToolStripMenuItem.Enabled = false;

            }
            else
            {
                if (IsPlayInPrays)
                {
                    PlayFile();
                    button2.BackgroundImage = bmp_paus;
                    IsPlayInPrays = false;
                }
                panel_Control.Enabled = true;
                LockControlToolStripMenuItem.Checked = false;
                LockControlToolStripMenuItem.Enabled = true;
            }





        }


        #endregion 




        #region method bonice



        private void ShapesInit()
        {
           getPictureBonice(Properties.Settings.Default.pathDict);
            int xDelta = BASE_XDELTA;
            int yDelta = BASE_YDELTA;
            img = new Bitmap[shapeCount];
            img2 = new Bitmap[shapeCount];
            //img[0] = new Bitmap("1.jpg");
            //img[1] = new Bitmap("2.jpg");
            //img[2] = new Bitmap("3.jpg");
            for (int i = 0; i < img.Length; i++)
            {
                if(imgBonice.Count>0)
                {
                img[i] = new Bitmap(imgBonice[i].FullName);
                img2[i] = new Bitmap(img[i], 200, 200);
                img[i].Dispose();
                }
               
            }
            shapes = new Shape[shapeCount];

            for (int i = 0; i < shapes.Length; i++)
            {
                shapes[i].xPos = 0;
                shapes[i].yPos = 0;
                shapes[i].xDelta = xDelta;
                shapes[i].yDelta = yDelta;

                // This makes the shapes move in (slightly) different ways:
                xDelta += DIFF_XDELTA;
                yDelta += DIFF_YDELTA;
                
            }
        }


        private void drawBonice(PaintEventArgs e)
        {

            try
            {
                if (!isDrawing) // don't draw shapes if already drawing
                {
                    isDrawing = true;

                    int theWidth = p1.ClientRectangle.Width;
                    int theHeight = p1.ClientRectangle.Height;
                    int theSize;

                    if (theWidth > theHeight) theSize = theHeight;
                    else theSize = theWidth;

                    theSize /= shapeSize;

                    for (int i = 0; i < shapes.Length; i++)
                    {
                        // Only move the shapes from the timer tick event (not when overlay is resized)...
                        if (moveShapes)
                        {
                            shapes[i].xPos += shapes[i].xDelta;
                            if (shapes[i].xPos <= 0)
                            {
                                shapes[i].xPos = 0;
                                shapes[i].xDelta = -shapes[i].xDelta;
                            }
                            else if (shapes[i].xPos + theSize > theWidth)
                            {
                                shapes[i].xPos = theWidth - theSize;
                                shapes[i].xDelta = -shapes[i].xDelta;
                            }

                            shapes[i].yPos += shapes[i].yDelta;
                            if (shapes[i].yPos <= 0)
                            {
                                shapes[i].yPos = 0;
                                shapes[i].yDelta = -shapes[i].yDelta;
                            }
                            else if (shapes[i].yPos + theSize >= theHeight)
                            {
                                shapes[i].yPos = theHeight - theSize;
                                shapes[i].yDelta = -shapes[i].yDelta;
                            }
                        }

                        // ... but also redraw the shapes after resizing the overlay
                        shapes[i].rect = new Rectangle(shapes[i].xPos, shapes[i].yPos, theSize, theSize);

                        // Drawing shapes, but could also move around controls like panels and pictureboxes
                        // Draw the transparant shape
                        switch (shapeForm)
                        {

                            case ShapeForm.Square:
                                //e.Graphics.FillRectangle(brush2, shapes[i].rect);
                                e.Graphics.DrawImage(img2[i], shapes[i].rect);
                                break;

                        }
                    }
                    isDrawing = false;
                }
            }
            catch { }
        }





        #endregion




        #region effect method

        private void DrawSomething()
        {
            
            
            int PageWidth = p1.Width;
            int PageHeight = p1.Height;
            int yStart = PageHeight - 20;
            double Pi = Math.PI;
            //p1.Controls.Add(pict);
            if (!mute.Checked)
                ap.Volume =(int) this.tv.Value;

            ap.Volume = ap.IsPaused == true ? 0 : (int)this.tv.Value;

            // you can use the following code instead previous line:
            if(ap.IsPaused == true)
                ap.Volume = 0;
            else
                ap.Volume = (int)this.tv.Value;
            

            //ratio between PicView.Height and maximum of player volume
            double yRatio = Convert.ToDouble(PageHeight) / 1000;

            Graphics g;
            // Create graphics object
            g = p1.CreateGraphics();
            //System.Threading.Thread.Sleep(100); // waiting a moment before clear drawing
            g.Clear(p1.BackColor);

            Random randNum = new Random();
            double W = PageWidth / 20;
            
            Pen NewPen = new Pen(Color.FromArgb(randNum.Next(10,255),randNum.Next(10,255),randNum.Next(10,255)));
            SolidBrush brush = new SolidBrush(Color.FromArgb(randNum.Next(10, 255), randNum.Next(10, 255), randNum.Next(10, 255)));
            
            for (double X = -3 * Pi; X < 3 * Pi; X += 0.5)
            {
                //NewPen = new Pen(Color.FromArgb(randNum.Next(100, 255),
                    //randNum.Next(100, 255), randNum.Next(50, 255)));
                //brush = new SolidBrush(Color.FromArgb(randNum.Next(100, 255),
                    //randNum.Next(100, 255), randNum.Next(50, 255)));
                double Z = Math.Sin(X);
                double Y = Math.Abs(Z);
                double PX = PageWidth / 2 + W * X; // Multiply X * W to enlarge X.
                int maxY = (int)(yRatio *ap.Volume);
                if (maxY < 40)
                    maxY = 40;
                double H = randNum.Next(5, maxY - 30); // Randome height:
                double PY = yStart - H * Y; // Multiply Y * H to enlarge Y.
                Point pp1 = new Point((int)PX, PageHeight - 10);
                Point pp2 = new Point((int)PX, (int)PY);
                // draw Line
                g.DrawLine(NewPen, pp1, pp2);
                Point dPoint = new Point((int)PX, (int)PY);
                dPoint.X = dPoint.X - 1;
                dPoint.Y = dPoint.Y - 10;
                Rectangle rect = new Rectangle(dPoint, new Size(3, 3));
                //using FillRectangle to draw a point
                g.FillRectangle(brush, rect);
                
            }

             //Release graphics object
            NewPen.Dispose();
            g.Dispose();
            brush.Dispose();
        }

        private void effectCaptur()//تأثير عند أخذ لقطة للفيديو
        {
            for (int i = 0; i < 3; i++)
            {
                p1.BorderStyle = BorderStyle.Fixed3D;
             this.Refresh();
                
                p1.BorderStyle = BorderStyle.None;
                

            }
        }

        private void listShow()
        {
            //panel_list.Left = panel_list.Right;
            for (int i = panel_list.Right; i >= p1.Right-1; i-=15)
            {
                panel_list.Left = i;
                this.Refresh();
            }
        }
        private void listClos()
        {

            for (int i = p1.Right; i <= p1.Right+panel_list.Width; i+=15)
            {
                panel_list.Left = i;
                this.Refresh();
            }
        }

        private void effectSoundE()
        {
            int x = (int)ap.Volume;
            //int i=5;
            for(int i=x;i>=0;i--)
            {
                ap.Volume = i;
                //this.Refresh();
                //i--;
            }
            
        }

        private void effectSoundSt()
        {
            int x = (int)tv.Value;
            //int i=5;
            for (int i = 0; i <= x; i++)
            {
                ap.Volume = i;
                //this.Refresh();
                //i--;
            }
        }

        private void effectVideo()
        {
           // p1.Left = -501;
            int i = -501;
            
                while (i <= 3)
                {
                    p1.Left = i;
                    this.Refresh();
                    //this.Invalidate();
                    i += 42; ;
                }
                //this.Left = this.Left - 6;
            
        }
        private void effectForm()
        {
            double i = 0.00;
            while (i < 1)
            {
                this.Opacity = i;
                this.Refresh();
                i += 0.08;
            }
        }

        private void effectFormDrop()
        {
             this.Top = -250;

             this.Refresh();
             //this.Invalidate();
             int i = -200;
             while(i<200)
            
             {
                 this.Top = i;
                 i += 2;
             }
        }
        #endregion effect method

        #region method

        private void Regis()
        {

            try
            {
                RegistryKey faisal = Registry.ClassesRoot;
                string ProgPath = Application.ExecutablePath;
                RegistryKey yni;
                for (int i = 0; i < exp.Length; i++)
                {
                    RegistryKey key = Registry.ClassesRoot.OpenSubKey(exp[i]);
                    if (key == null)
                    {
                        yni = faisal.CreateSubKey(exp[i]);
                        yni.SetValue("", "info");
                        RegistryKey keyBass = faisal.CreateSubKey("info");
                        keyBass.SetValue("", "fisal alwhbany");
                        RegistryKey shell = keyBass.CreateSubKey("shell");
                        RegistryKey open = shell.CreateSubKey("open");
                        open.SetValue("", "&Open Fire Player");
                        yni = open.CreateSubKey("command");
                        yni.SetValue("", ProgPath + " %1");
                        yni = keyBass.CreateSubKey("DefaultIcon");
                        yni.SetValue("", ProgPath + ",0");
                    }
                }
            }
            catch { };
                


            
            
        }

        private void getPictureBonice(string pathPict)
        {
            directoryInfo = new DirectoryInfo(pathPict);
           
            
                for (int i = 0; i < searchExtensions.Length; i++)
                {
                    if (imgBonice.Count < shapeCount)
                        imgBonice.AddRange(directoryInfo.GetFiles("*" + searchExtensions[i]));
                    
            }
        }


        //إظهار علامة الإختيار في القائمة
        private void SetMenuCheckMarks(object sender)
        {
            foreach (ToolStripItem item in (((ToolStripMenuItem)sender).GetCurrentParent().Items))
            {
                if (item.GetType() == typeof(ToolStripMenuItem) && item != null)
                {
                    if (item == sender) ((ToolStripMenuItem)item).Checked = true;
                    else ((ToolStripMenuItem)item).Checked = false;
                }
            }
        }

        //get mp3 info

        private void GetMp3Info(string file)
        {
            p1.BackgroundImageLayout = ImageLayout.Zoom;
            try
            {
                p1.BackgroundImage = null;
                p1.BackgroundImage = Image.FromStream(Mp3Info.AttachedPictureFrames.Items[0].Data);
                Title.Text = Mp3Info.GetTextFrame("TIT2");
                Artist.Text = Mp3Info.GetTextFrame("TPE1");
                Year.Text = Mp3Info.GetTextFrame("TYER");
                commite.Text = Mp3Info.GetTextFrame("COMM");
                Album.Text = Mp3Info.GetTextFrame("TALB");
                //Mp3Info.GetTextFrame();
            }
            catch { }
        }

       

        private void LoadSettPraysTimer()
        {
            if (Properties.Settings.Default.OnPraysTimer)
            {
                OnPrayTimerToolStripMenuItem1.Checked = true;
            }
            else
            {
                OffPraysTimerToolStripMenuItem.Checked = true;
            }
            dat = Properties.Settings.Default.DawnTime;
            dat2 = Properties.Settings.Default.NoonTime;
            dat3 = Properties.Settings.Default.ANoonTime;
            dat4 = Properties.Settings.Default.SunsetTime;
            dat5 = Properties.Settings.Default.NightTime;
            wait = Properties.Settings.Default.WaitTime;
            IsPraysTimer = Properties.Settings.Default.OnPraysTimer;
            
           
        }


        //لجلب إعدادات المشغل
        private void loadSettings()
        {
            track_Balance.Value = Properties.Settings.Default.blance;
            track_Speed.Value = Properties.Settings.Default.speed;
            trackBar2.Value = Properties.Settings.Default.currentPos;
            tv.Value = Properties.Settings.Default.volume;
            Visual = Properties.Settings.Default.VisualMode;
            nothinkToolStripMenuItem.Checked = true;
            PathPict = Properties.Settings.Default.pathDict;
            switch (Properties.Settings.Default.VisualMode)
            {
                case "none":
                    NoneVisualToolStripMenuItem.Checked = true;
                    break;
                case "wave":
                    WavVisualToolStripMenuItem.Checked = true;
                    break;
                case "AlbumPicture":
                    AlbumImgToolStripMenuItem.Checked = true;
                    break;
                case "pict":
                    //timer2.Interval = 25;
                    VisualPictToolStripMenuItem.Checked = true;
                    break;
            }
            if (Properties.Settings.Default.TrackScroll)
            {
                MoveToolStripMenuItem.Checked = true;
            }
            else
                UpToolStripMenuItem.Checked = true;
            TrackScroll = Properties.Settings.Default.TrackScroll;
            switch (Properties.Settings.Default.looping)
            {
                case "on":
                    OnToolStripMenuItem.Checked = true;
                    looping = loop.On;
                    break;
                case "none":
                    StopToolStripMenuItem.Checked = true;
                    looping = loop.none;
                    break;
                case "all":
                    AllToolStripMenuItem.Checked = true;
                    looping = loop.All;
                    break;
            }
            //looping = (loop)Properties.Settings.Default.looping;
        }

        private void screenShot()
        {

           // this.Refresh();
            System.IO.DirectoryInfo dir=new System.IO.DirectoryInfo(@Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)+@"\F_Player\Capture\");
            if(dir.Exists==false)
            {
                dir.Create();
            }
            //dir.Exists=
            string imgName = DateTime.Now.Year +"_"+ DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Hour +
                "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second;
            Graphics g;
            Bitmap bmpsc = new Bitmap(this.p1.Width,this.p1.Height);
            g = Graphics.FromImage(bmpsc);
            g.CopyFromScreen(this.Location.X+3,this.Location.Y+32, 0, 0,this.Size);
           
            bmpsc.Save(dir+imgName+".jpg", ImageFormat.Jpeg);
            effectCaptur();
            //MessageBox.Show("لمعاينة الصورة اذهب إلى المسار التالي"+"\n"+dir.ToString());
            screenshootDialog sc = new screenshootDialog();
            sc.pictureBox1.Image = bmpsc;
            if(sc.ShowDialog()==DialogResult.OK)
                System.Diagnostics.Process.Start("explorer.exe", "/select," + dir+imgName+".jpg");
            bmpsc.Dispose();
            g.Dispose();
        }

        //جلب ملف الوسائط

        private void LoadFile(string path)
        {
            if (ap.IsOpened)
                ap.Close();
            string FileName;
            FileName = System.IO.Path.GetFileName(path);
            sk.FileName = FileName;
            this.Invalidate();
            Mp3Info = null;
            if (videozoom.Value > 0)
                videozoom.Value = 0;
            trackBar2.Value = 0;
            try
            {
                Mp3Info = new ID3v2(path, true);
                ap.Open(path);
                ap.Owner = p1;
                ap.Volume = (int)tv.Value;
                ap.Rate = (int)track_Speed.Value;
                ap.Balance = (int)track_Balance.Value;
                trackBar2.Maximum = (int)ap.Duration;
                if (ap.HasVideo)
                {
                    //effectVideo();
                    button10.Enabled = true;
                    button3.Enabled = true;
                    videozoom.Enabled = true;
                    timer2.Enabled = false;
                    this.Invalidate();
                }
                else
                {
                    button10.Enabled = false;
                    button3.Enabled = false;
                    videozoom.Enabled = false;
                    /*switch (Visual)
                    {
                        case "wave":
                            timer2.Enabled = true;
                            break;
                        case "AlbumPicture":
                            //GetMp3Info(path);
                            break;
                        case "none":
                            p1.BackgroundImageLayout = ImageLayout.Center;
                            p1.BackgroundImage = Reso.login;
                            break;
                    }*/
                    this.Invalidate();
                }
            }
            catch { MessageBox.Show("قد يكون الملف غير صالح أو تأكد من مسار الملف أو أن الكودك  غير موجود على الكمبيوتر"); }
            //catch (Exception m) { MessageBox.Show(m.ToString()); }
               
                if (this.WindowState ==FormWindowState.Normal)
                {
                    if (list_show == false)
                    {
                        
                       
                        
                        p1.Width = this.ClientRectangle.Width -sk.Border_Size2*2;
                        p1.Left = this.ClientRectangle.Left+3;
                        this.Left = this.Left -6;
                        
                        panel_Control.Width = ClientRectangle.Width-sk.Border_Size2*2 ;
                        panel_Control.Left = this.ClientRectangle.Left + 3;
                        p1.Height = this.ClientRectangle.Height -5- panel_Control.Height-sk.Panel_Move_Size2-(sk.Border_Size2*2);
                        


                    }
                    else
                    {
                        
                        
                        p1.Width = this.ClientRectangle.Width - panel_list.Width-(sk.Border_Size2*2);
                        panel_Control.Width = this.ClientRectangle.Width - panel_list.Width-11;
                        p1.Height = this.ClientRectangle.Height -5- panel_Control.Height-(sk.Border_Size2*2)-sk.Panel_Move_Size2;
                    }
                }
                else if (this.WindowState == FormWindowState.Maximized)
                {
                    if (list_show == false)
                    {
                        
                        
                        p1.Width = this.ClientRectangle.Width-(sk.Border_Size2 * 2);
                        panel_Control.Width = this.ClientRectangle.Width-6-5;
                        p1.Height = this.ClientRectangle.Height - panel_Control.Height - (sk.Border_Size2 * 2) - sk.Panel_Move_Size2;
                    }
                    else
                    {
                        
                        
                        p1.Width = this.ClientRectangle.Width - panel_list.Width-(sk.Border_Size2 * 2);
                        panel_Control.Width = this.ClientRectangle.Width - panel_list.Width-11;
                        p1.Height = this.ClientRectangle.Height - panel_Control.Height - (sk.Border_Size2 * 2) - sk.Panel_Move_Size2;
                    }
                    //p1.Width = this.ClientRectangle.Width;
                    //p1.Height = this.Height - panel_Control.Height;
                }               
                SetLength();
              
        }
                
    
            
            

        //تشغيل الملف 
        void PlayFile()
        {

            if (!ap.HasVideo)
            {
                
           
            }
            play = true;
            button2.BackgroundImage = bmp_paus2;
            timer1.Enabled = true;
                    ap.Play();
                    //effectSoundSt();
                    //effectVideo();
                    button2.Enabled = true;
                    if (ap.HasVideo)
                    {
                       
                        button10.Enabled = true;
                        button3.Enabled = true;
                        videozoom.Enabled = true;
                        //this.Invalidate();
                    }
                    else
                    {
                        button10.Enabled = false;
                        button3.Enabled = false;
                        videozoom.Enabled = false;
                      switch (Visual)
                        {
                            case "wave":
                                timer2.Enabled = true;
                                break;
                            case "AlbumPicture":
                                GetMp3Info(ap.FileName);
                                break;
                            case "none":
                                p1.BackgroundImageLayout = ImageLayout.Center;
                                p1.BackgroundImage = Reso.login;
                                break;
                            case "pict":
                                timer2.Enabled = true;
                                p1.BackgroundImageLayout = ImageLayout.Center;
                                p1.BackgroundImage =null;
                                break;
                        }
                        //this.Invalidate();
                    }
                    //effectSoundSt();
                   
                   
           
        }

        //للايقاف المؤقت


        void PausFile()
        {
            play = false;
            button2.BackgroundImage = bmp_play2 ;
            timer1.Enabled = false;
                    ap.Pause();
                  
        }

        //لايقاف التشغيل
        void StopFile()
        {
            timer2.Enabled = false;
            timer1.Enabled = false;
            if (play)
            {
                button2.BackgroundImage = bmp_play2;
                play = false;
            }
            trackBar2.Value = 0;
            
                    //effectSoundE();
            if (Visual == "AlbumPicture")
            {
                BackgroundImage = null;
            }
                    ap.Stop();
                    p1.Invalidate();
                   
                   
                    
            
        }


        private void ReleaseMemory()
        { 
                    ap.Stop();
        }

        //ضبط شريط التقديم

        void SetTrackBar()
        {

            //trackBar3.Maximum = Convert.ToInt32(ap.Duration);
            //trackBar1.Maximum = Convert.ToInt32(ap.Duration);
            //trackBar2.Maximum = Convert.ToInt32(ap.Duration);
        }     
            

       //ملء قائمة التشغيل
        void FillList()
        {
            L1.Items.Clear();
            for (int i = 0; i < albume.Count; i++)
            {
                string FileName;
                FileName = System.IO.Path.GetFileName(albume[i]);
                L1.Items.Add(FileName);
            }
        }


        // قراءة الألبومات
        void ReadAlbume(string albumeName)
        {
            try
            {
                System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(
                    @Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                    @"\F_Player\albumes\");
                // path_albumes = dir;
                if (dir.Exists == false)
                {
                    dir.Create();
                }
                System.IO.FileInfo f = new System.IO.FileInfo(@dir+@"albumes.xml");
                if (f.Exists == false)
                {
                    //f.Create();
                    string[] myTasks = {"<?xml version='1.0' encoding='utf-8' ?>",
                                           "<albumes></albumes>"
                };
                   System.IO.File.WriteAllLines(@dir+@"albumes.xml", myTasks);
                }
                doc.Load(@dir+@"\albumes.xml");
                XmlNode Filend = doc["albumes"][albumeName].FirstChild;
                albume.Clear();
                while (Filend != null)
                {
                    albume.Add(Filend.InnerText);
                    Filend = Filend.NextSibling;
                }
                FillList();
            }
            catch
            {
                MessageBox.Show("حدث خطأ أثنأ أسترجاع بيانات الألبومات");
            }
        }


        //لتشغيل الملف الحالي في القائمة

        void PlayCurrent()
        {
            if(ap.IsPlaying)
            {
                ap.Stop();
            }
            LoadFile(albume[current]);
            SetTrackBar();
            PlayFile();
        }

        //المقطع التالي في القائمة
        void SetNext()
        {
            
            if (setrnd.Checked)
            {
                if (current == albume.Count - 1)
                {
                    current = 0;
                }
                else
                {
                    current = current + 1;
                }
            }
            else
            {
                Random rnd = new Random();
                current = rnd.Next(0, albume.Count - 1);
            }
            try
            {
                if (albume.Count > 0)
                {

                L1.SelectedIndex = current;
                }
            }
            catch { }
           /* if (ap.IsPlaying)
            {
                ap.Stop();
                PlayCurrent();
            }
            else
            {
                PlayCurrent();
            }*/

        }



        //للوصول للمقطع السابق في القائمة
        void SetPrev()
        {
            if (setrnd.Checked)
            {
                if (current == 0)
                {
                    current = albume.Count - 1;
                }
                else
                {
                    current = current - 1;
                }
            }
            else
            {
                Random rnd = new Random();
                current = rnd.Next(0, albume.Count - 1);

            }
            try
            {
                if (albume.Count > 0)
                {

                    L1.SelectedIndex = current;
                }
            }
            catch { }
            //PlayCurrent();
            /*if (ap.IsPlaying)
            {
                ap.Stop();
                PlayCurrent();
            }
            else
            {
                PlayCurrent();
            }*/
        }

        //لمعرفة طول الملف

        void SetLength()
        {
            try
            {
                int m = 0;
                int s = 0;
                int h = 0;


                h = Convert.ToInt32(Math.Floor(ap.Duration / 3600000));
                m = Convert.ToInt32(Math.Floor((ap.Duration - (h * 3600000)) / 60000));
                s = Convert.ToInt32((ap.Duration - ((h * 3600000) + (60000 * m)))/1000);

               // length = Convert.ToString(h) + ":" + Convert.ToString(m) + ":" + Convert.ToString(s);
                length = new TimeSpan(h, m, s);
                

            }
            catch { }
        }



        private void SetColorForm()
        {
            switch (theme)
            {
                case"red":
            sk.Panel_Move_Dark2 = Color.DarkRed;
            sk.Panel_Move_light2 = Color.Red;
            sk.Border_LR_Dark2 = sk.Panel_Move_Dark2;
            sk.Border_LR_light2 = sk.Panel_Move_Dark2;
            sk.Border_Top_Dark2 = sk.Panel_Move_Dark2;
            sk.Border_Top_light2 = sk.Panel_Move_Dark2;
            sk.Border_Bottom_Dark2 = sk.Panel_Move_Dark2;
            sk.Border_Bottom_light2 = sk.Panel_Move_Dark2;
            Properties.Settings.Default.panelMoveColorDark = sk.Panel_Move_Dark2;
            Properties.Settings.Default.panelMoveColorLight = sk.Panel_Move_light2;
            Properties.Settings.Default.borderBottomColor = sk.Border_Bottom_Dark2;
            Properties.Settings.Default.borderLRColor = Properties.Settings.Default.borderTopColor =
                sk.Panel_Move_Dark2;
            panel_Control.BackColor = track_Balance.BackColor=videozoom.BackColor = track_Speed.BackColor =
                tv.BackColor = trackBar2.BackColor = sk.Border_Bottom_light2;
            Properties.Settings.Default.Save();
                    break;
                case"blue":
            sk.Panel_Move_Dark2 = Color.DarkBlue;
            sk.Panel_Move_light2 = Color.Blue;
            sk.Border_LR_Dark2 = sk.Panel_Move_Dark2;
            sk.Border_LR_light2 = sk.Panel_Move_Dark2;
            sk.Border_Top_Dark2 = sk.Panel_Move_Dark2;
            sk.Border_Top_light2 = sk.Panel_Move_Dark2;
            sk.Border_Bottom_Dark2 = sk.Panel_Move_Dark2;
            sk.Border_Bottom_light2 = sk.Panel_Move_Dark2;
            Properties.Settings.Default.panelMoveColorDark = sk.Panel_Move_Dark2;
            Properties.Settings.Default.panelMoveColorLight = sk.Panel_Move_light2;
            Properties.Settings.Default.borderBottomColor = sk.Border_Bottom_Dark2;
            Properties.Settings.Default.borderLRColor = Properties.Settings.Default.borderTopColor =
                sk.Panel_Move_Dark2;
            panel_Control.BackColor = track_Balance.BackColor=videozoom.BackColor = track_Speed.BackColor =
        tv.BackColor = trackBar2.BackColor=videozoom.BackColor = sk.Border_Bottom_light2;
            Properties.Settings.Default.Save();
                    break;
                case"green":
                    sk.Panel_Move_Dark2 = Color.DarkGreen;
            sk.Panel_Move_light2 = Color.Green;
            sk.Border_LR_Dark2 = sk.Panel_Move_Dark2;
            sk.Border_LR_light2 = sk.Panel_Move_Dark2;
            sk.Border_Top_Dark2 = sk.Panel_Move_Dark2;
            sk.Border_Top_light2 = sk.Panel_Move_Dark2;
            sk.Border_Bottom_Dark2 = sk.Panel_Move_Dark2;
            sk.Border_Bottom_light2 = sk.Panel_Move_Dark2;
            Properties.Settings.Default.panelMoveColorDark = sk.Panel_Move_Dark2;
            Properties.Settings.Default.panelMoveColorLight = sk.Panel_Move_light2;
            Properties.Settings.Default.borderBottomColor = sk.Border_Bottom_Dark2;
            Properties.Settings.Default.borderLRColor = Properties.Settings.Default.borderTopColor =
                sk.Panel_Move_Dark2;
            panel_Control.BackColor = track_Balance.BackColor = track_Speed.BackColor =
tv.BackColor = trackBar2.BackColor = videozoom.BackColor = sk.Border_Bottom_light2;
            Properties.Settings.Default.Save();
                    break;
                case"defualt":
                    Properties.Settings.Default.Reset();
                    sk.Panel_Move_Dark2 = Properties.Settings.Default.panelMoveColorDark;
                    sk.Panel_Move_light2 = Properties.Settings.Default.panelMoveColorLight;
            sk.Border_LR_Dark2 = sk.Panel_Move_Dark2;
            sk.Border_LR_light2 = sk.Panel_Move_Dark2;
            sk.Border_Top_Dark2 = sk.Panel_Move_Dark2;
            sk.Border_Top_light2 = sk.Panel_Move_Dark2;
            sk.Border_Bottom_Dark2 = sk.Panel_Move_Dark2;
            sk.Border_Bottom_light2 = sk.Panel_Move_Dark2;

            panel_Control.BackColor = track_Balance.BackColor = track_Speed.BackColor =
        tv.BackColor = trackBar2.BackColor=videozoom.BackColor = sk.Border_Bottom_light2;
           
            Properties.Settings.Default.Save();
                    break;
                case "golde":
                    sk.Panel_Move_Dark2 = Color.Goldenrod;
                    sk.Panel_Move_light2 = Color.Gold;
                    sk.Border_LR_Dark2 = sk.Panel_Move_Dark2;
                    sk.Border_LR_light2 = sk.Panel_Move_Dark2;
                    sk.Border_Top_Dark2 = sk.Panel_Move_Dark2;
                    sk.Border_Top_light2 = sk.Panel_Move_Dark2;
                    sk.Border_Bottom_Dark2 = sk.Panel_Move_Dark2;
                    sk.Border_Bottom_light2 = sk.Panel_Move_Dark2;
                    Properties.Settings.Default.panelMoveColorDark = sk.Panel_Move_Dark2;
                    Properties.Settings.Default.panelMoveColorLight = sk.Panel_Move_light2;
                    Properties.Settings.Default.borderBottomColor = sk.Border_Bottom_Dark2;
                    Properties.Settings.Default.borderLRColor = Properties.Settings.Default.borderTopColor =
                        sk.Panel_Move_Dark2;
                    panel_Control.BackColor = track_Balance.BackColor = videozoom.BackColor = track_Speed.BackColor =
                tv.BackColor = trackBar2.BackColor = videozoom.BackColor = sk.Border_Bottom_light2;
                    Properties.Settings.Default.Save();
                    break;
                case "volit":
                    sk.Panel_Move_Dark2 = Color.DarkOrchid;
                    sk.Panel_Move_light2 = Color.MediumOrchid;
                    sk.Border_LR_Dark2 = sk.Panel_Move_Dark2;
                    sk.Border_LR_light2 = sk.Panel_Move_Dark2;
                    sk.Border_Top_Dark2 = sk.Panel_Move_Dark2;
                    sk.Border_Top_light2 = sk.Panel_Move_Dark2;
                    sk.Border_Bottom_Dark2 = sk.Panel_Move_Dark2;
                    sk.Border_Bottom_light2 = sk.Panel_Move_Dark2;
                    Properties.Settings.Default.panelMoveColorDark = sk.Panel_Move_Dark2;
                    Properties.Settings.Default.panelMoveColorLight = sk.Panel_Move_light2;
                    Properties.Settings.Default.borderBottomColor = sk.Border_Bottom_Dark2;
                    Properties.Settings.Default.borderLRColor = Properties.Settings.Default.borderTopColor =
                        sk.Panel_Move_Dark2;
                    panel_Control.BackColor = track_Balance.BackColor = videozoom.BackColor = track_Speed.BackColor =
                tv.BackColor = trackBar2.BackColor = videozoom.BackColor = sk.Border_Bottom_light2;
                    Properties.Settings.Default.Save();
                    break;


            }

        }
        private void loadSettingFormColor()
        {
            sk.Panel_Move_Dark2 = Properties.Settings.Default.panelMoveColorDark;
            sk.Panel_Move_light2 = Properties.Settings.Default.panelMoveColorLight;
            sk.Border_LR_Dark2 = sk.Panel_Move_Dark2;
            sk.Border_LR_light2 = sk.Panel_Move_Dark2;
            sk.Border_Top_Dark2 = sk.Panel_Move_Dark2;
            sk.Border_Top_light2 = sk.Panel_Move_Dark2;
            sk.Border_Bottom_Dark2 = sk.Panel_Move_Dark2;
            sk.Border_Bottom_light2 = sk.Panel_Move_Dark2;
            panel_Control.BackColor = track_Balance.BackColor = track_Speed.BackColor =
                tv.BackColor = trackBar2.BackColor=videozoom.BackColor = sk.Border_Bottom_light2;

        }

        //لمعرفة مكان تشغيل الملف

        void SetPosition()
        {
            try
            {
                int h = 0;
                int m = 0;
                int s = 0;
                 
                       h = Convert.ToInt32(Math.Floor(ap.CurrentPosition / 3600000));
                        m = Convert.ToInt32(Math.Floor((ap.CurrentPosition - (h * 3600000)) / 60000));
                        s = Convert.ToInt32((ap.CurrentPosition - ((h * 3600000) + (60000 * m)))/1000);

                        TimeSpan Position = new TimeSpan(h, m, s);
                label1.Text = length.ToString();
                label2.Text = Position.ToString();
            }
            catch { }
        }
        #endregion method

        #region event

        private void Form1_Load(object sender, EventArgs e)
        {
            Regis();
            
            loadSettings();
            LoadSettPraysTimer();
            panel_list.Hide();
            sk = new skine(this);

            loadSettingFormColor();
            sk.TextColor = Color.Yellow;

            effectFormDrop();
            //effectForm();

            if (setrnd.Checked)
            {
                setrnd.BackgroundImage = Reso.Activ;
            }
            else
            {
                setrnd.BackgroundImage = Reso.noActiv;

            }

            
                
            //pict.SendToBack();
            
            
            
           // panel_list.Hide();
           

            if (list_show == false)
            {

                
               
                p1.Width = this.ClientRectangle.Width - sk.Border_Size2 * 2;
                p1.Left = this.ClientRectangle.Left + 3;
                this.Left = this.Left - 6;
                
                panel_Control.Width = ClientRectangle.Width - sk.Border_Size2 * 2;
                panel_Control.Left = this.ClientRectangle.Left + 3;
                p1.Height = this.ClientRectangle.Height - 5 - panel_Control.Height - sk.Panel_Move_Size2 - (sk.Border_Size2 * 2);
                


            }
            else
            {

               
                p1.Width = this.ClientRectangle.Width - panel_list.Width - (sk.Border_Size2 * 2);
                panel_Control.Width = this.ClientRectangle.Width - panel_list.Width - 11;
                p1.Height = this.ClientRectangle.Height - panel_Control.Height - (sk.Border_Size2 * 2) - sk.Panel_Move_Size2;
            }

            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(
                    @Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                    @"\F_Player\albumes\");
            // path_albumes = dir;
            if (dir.Exists == false)
            {
                dir.Create();
            }
            System.IO.FileInfo f = new System.IO.FileInfo(@dir + @"albumes.xml");
            if (f.Exists == false)
            {
                
               // f.Create();
                string[] myTasks = {"<?xml version='1.0' encoding='utf-8' ?>",
                                           "<albumes></albumes>"
                };
                System.IO.File.WriteAllLines(@dir+@"albumes.xml", myTasks);
                //System.IO.File.
              
            }
            doc.Load(@dir +@"albumes.xml");
            XmlNode albumesnd = doc["albumes"];
            XmlNode alnd = albumesnd.FirstChild;
            while (alnd != null)
            {
                alc.Items.Add(alnd.Name);
                alnd = alnd.NextSibling;
            }
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            if (opf.ShowDialog() == DialogResult.OK)
            {
                albume.AddRange(opf.FileNames);
                LoadFile(opf.FileName);
                FillList();
                SetTrackBar();
                PlayFile();
            }
        }

      


        private void button2_Click(object sender, EventArgs e)
        {
            if (!play)
            {
               
                PlayFile();
                button2.BackgroundImage = bmp_paus;
                
                
                
            }
            else
            {
                
                PausFile();
                button2.BackgroundImage = bmp_play2; 
                
                //PausFile();
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (ap.HasVideo)
                {

                    ap.FullScreen = true;
                    p1.Invalidate();
                    p1.Refresh();
                    this.Refresh();
                }
                
            }
            catch { }
        }

        private void mainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            
            ap.FullScreen = false;
            ap.Play();
           
              
        }

       

        private void button4_Click(object sender, EventArgs e)
        {
            
            StopFile();
        }

        

        private void timer1_Tick(object sender, EventArgs e)
        {
            //DrawSomething();
                //SetTrackBar();

            

                trackBar2.Value = Convert.ToInt32(ap.CurrentPosition);

                
                 
                SetPosition();
            try
            {
                if (isLoop)
                {
                    
                            if (ap.CurrentPosition >= loopEnd)
                            {

                                ap.PlayFromTo((int)LoopStart,(int)loopEnd);
                                ap.CurrentPosition=(int)LoopStart;
                                
                            }
                           

                    }
                
                
                    int last, n;

                    n = (int)trackBar2.Maximum;
                    last = n - 1;



                    if ((int)trackBar2.Value > last)
                    {
                        switch (endPlay)
                        {
                            case "exit":
                                this.Close();
                                break;
                            case "clos":
                                this.Close();
                                System.Diagnostics.Process.Start("shutdown", "/s /t 0");
                                break;
                            case "lock":
                                API.LockWorkStation();
                                break;
                            
                        }
                        switch (looping)
                        {
                            case loop.none:
                                    StopFile();
                                break;
                                
                            case loop.All:
                                if (L1.Items.Count>1)
                                {
                                SetNext();
                                }
                                break;
                            case loop.On:
                                LoadFile(albume[current]);
                                PlayFile();
                                
                                break;

                        
                        
                    }
                }

            }
            catch{}
        
           
            
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                
                SetNext();
            }
            catch
            {
            }
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                SetPrev();
            }
            catch
            {
            }
            
        }


        private void L1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //StopFile();
            current = L1.SelectedIndex;
            PlayCurrent();
        }

       

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {

                Form2 frm = new Form2();
                frm.albume = albume;
                frm.ShowDialog();
                if (frm.DialogResult == DialogResult.OK)
                {
                    albume = frm.albume;
                    FillList();
                    LoadFile(albume[0]);
                    SetTrackBar();
                    PlayFile();
                }
            }
            catch
            {
                MessageBox.Show("حدث خطأ أثنأ تشغيل الملف");
            }
        }

        private void alc_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ReadAlbume(alc.Text);
                FillList();
                LoadFile(albume[0]);
                SetTrackBar();
                PlayFile();
            }
            catch
            {
                MessageBox.Show("حدث خطأ أثنأ محاولة تشغيل الملف المحدد");
            }
        }

        private void mute_CheckedChanged(object sender, EventArgs e)
        {
            if (mute.Checked)
            {
                //ap.Volume = 0;
                mute.BackgroundImage = Reso.Vou3;
                mut = true;
                ap.MuteAll = true;
            }
            else
            {
                
                //ap.Volume = (int)tv.Value;
                mute.BackgroundImage = Reso.Vou1;
                ap.MuteAll = false;
                mut = false;
            }

        }

        private void p1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void mainForm_Resize(object sender, EventArgs e)
        {
            
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void إيقافالتشغيلToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StopFile();
        }

        private void إيقافمؤقتToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PausFile();
        }

        private void تشغيلToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PlayFile();
        }

        private void إغلاقالبرنامجToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //this.Close();
            System.Environment.Exit(0);
        }


       
        

       

        private void mainForm_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            albume.AddRange(files);
            LoadFile(files[0]);
            FillList();
            //SetTrackBar();
            PlayFile(); 
        }
        
       

        private void mainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

       

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            if (play == true)
            {
               
               // button2.BackgroundImage = Image.FromFile("button/12.png");
                button2.BackgroundImage = bmp_paus;
            }
            else
            {
                button2.BackgroundImage = bmp_play;
                
              
            }
           // button2.BackgroundImage = Image.FromFile("button/1.png");
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            if (play)
            {

                button2.BackgroundImage = bmp_paus2;
               
            }
            else
            {
                button2.BackgroundImage = bmp_play2;
                
                
            }
           
        }

        private void tv_Scroll_1(object sender, EventArgs e)
        {
            if (mute.Checked)
            {
            }
            else
            {
               // hv.Volume = (int)tv.Value;
                ap.Volume = (int)(tv.Value);
                Properties.Settings.Default.volume =(int)(tv.Value);
                Properties.Settings.Default.Save();
                //view._volume = hv.Volume;
                                
            }
           // toolTip1.SetToolTip(tv, tv.Value.ToString());
        }

        

        private void button6_MouseEnter(object sender, EventArgs e)
        {
            button6.BackgroundImage = bmp_back2 ;
        }

        private void button6_MouseLeave(object sender, EventArgs e)
        {
            button6.BackgroundImage = bmp_back;
        }

        private void button7_MouseEnter(object sender, EventArgs e)
        {
            button7.BackgroundImage = bmp_forw2;
        }

        private void button7_MouseLeave(object sender, EventArgs e)
        {
            button7.BackgroundImage = bmp_forw;
        }

        private void button3_MouseEnter(object sender, EventArgs e)
        {
            button3.BackgroundImage = bmp_full2;
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            button3.BackgroundImage = bmp_full;
        }

        private void button4_MouseEnter(object sender, EventArgs e)
        {
            button4.BackgroundImage = bmp_stop2;
        }

        private void button4_MouseLeave(object sender, EventArgs e)
        {
            button4.BackgroundImage = bmp_stop;
        }

        private void label3_MouseEnter(object sender, EventArgs e)
        {
            label_about.ForeColor = Color.Red; 
        }

        private void label_about_MouseLeave(object sender, EventArgs e)
        {
            label_about.ForeColor = Color.Yellow;
        }

        private void حولToolStripMenuItem_Click(object sender, EventArgs e)
        {

            about.ShowDialog();
            
        }

        private void label_about_Click(object sender, EventArgs e)
        {
            about.ShowDialog();
        }

        private void label3_Click(object sender, EventArgs e)
        {


            //trackBar1.Refresh();
            
           
            if (list_show==false)
            {
                //panel_list.Hide();
                list_show = true;
                panel_list.Show();
                panel_Control.Width = this.ClientRectangle.Width - panel_list.Width-(sk.Border_Size2*2);
                panel_Control.Left = this.ClientRectangle.Left + 3;
                p1.Width= this.ClientRectangle.Width - panel_list.Width-(sk.Border_Size2*2);
                //this.panel_Control.Size = new Size(this.ClientRectangle.Width - panel_list.Width, this.panel_Control.Height);
                panel_list.BringToFront();
                panel_Control.Refresh();
                listShow();
                
            }
            else
            {
                //panel_list.Hide();
                listClos();
                list_show = false;
                p1.Width = this.ClientRectangle.Width-(sk.Border_Size2*2);
                p1.Left = this.ClientRectangle.Left + 3;
                panel_Control.Width = this.ClientRectangle.Width-(sk.Border_Size2*2); //+ panel_list.Width;
                panel_Control.Left = this.ClientRectangle.Left + 3;


                //this.panel_Control.Size = new Size(this.ClientRectangle.Width + panel_list.Width, this.panel_Control.Height);
                panel_list.SendToBack();
                
                panel_list.Hide();
                this.Refresh();


            }

        }

        private void label3_MouseEnter_1(object sender, EventArgs e)
        {
            BTN_List.BackgroundImage = Reso.list2;
           // BTN_List.BackColor = Color.FromArgb(100,0,0,255); 
        }

        private void label3_MouseLeave(object sender, EventArgs e)
        {
            BTN_List.BackgroundImage = Reso.list1;
           // BTN_List.BackColor = Color.Black;
        }

        private void mute_MouseEnter(object sender, EventArgs e)
        {
            if (mut == true)
            {
                mute.BackgroundImage = Reso.Vou4;
            }
            else
            {
                mute.BackgroundImage = Reso.Vou2;
            }
        }

        private void mute_MouseLeave(object sender, EventArgs e)
        {
            if (mut == true)
            {
                mute.BackgroundImage = Reso.Vou3;
            }
            else
            {
                mute.BackgroundImage = Reso.Vou1;
            }
        }



        

       /* private void BTN_Close_Click(object sender, EventArgs e)
        {

            if (this.WindowState != FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }

        }*/


        private void panel_Control_Resize(object sender, EventArgs e)
        {
            panel_Control.Invalidate();
            
        }

        private void setrnd_MouseMove(object sender, MouseEventArgs e)
        {
            if (setrnd.Checked)
            {
                setrnd.BackgroundImage = Reso.activover;
            }
            else
            {
                setrnd.BackgroundImage = Reso.noactivover2;

            }
        }

        private void setrnd_MouseLeave(object sender, EventArgs e)
        {
            if (setrnd.Checked)
            {
                setrnd.BackgroundImage = Reso.Activ;
            }
            else
            {
                setrnd.BackgroundImage = Reso.noActiv;

            }
        }

        private void setnrnd_MouseMove(object sender, MouseEventArgs e)
        {
            if (setnrnd.Checked)
            {
                setnrnd.BackgroundImage = Reso.actover;
            }
            else
            {
                setnrnd.BackgroundImage = Reso.noactover;

            }
        }

        private void setnrnd_MouseLeave(object sender, EventArgs e)
        {
            if (setnrnd.Checked)
            {
                setnrnd.BackgroundImage = Reso.act;
            }
            else
            {
                setnrnd.BackgroundImage = Reso.noact2;

            }
        }
       

        private void button9_Click(object sender, EventArgs e)
        {
            Loop l = new Loop();
            //TimeSpan tm1=new TimeSpan();
            //TimeSpan tm2=new TimeSpan();
            if (!isLoop)
            {
                if (l.ShowDialog() == DialogResult.OK)

                {

                    button9.BackgroundImage = Reso.loop1;
                    int h = Convert.ToInt32(l.textHSt.Value);
                    int m = Convert.ToInt32(l.textMSt.Value);
                    int s = Convert.ToInt32(l.textSSt.Value);
                    int h2 = Convert.ToInt32(l.textHEn.Value);
                    int m2 = Convert.ToInt32(l.textMEn.Value);
                    int s2 = Convert.ToInt32(l.textSEn.Value);
                    TimeSpan tm1 = new TimeSpan(h, m, s);
                    TimeSpan tm2 = new TimeSpan(h2, m2, s2);
                    LoopStart = tm1.TotalMilliseconds;
                    loopEnd = tm2.TotalMilliseconds;
                    trackBar2.LoopStart1 = LoopStart;
                    trackBar2.LoopEnd1 = loopEnd;
                    isLoop = true;
                    this.Refresh();
                    trackBar2.Refresh();
                }
            }
            else
            {
                button9.BackgroundImage = Reso.NoLoop;
                trackBar2.LoopEnd1 = trackBar2.LoopStart1 = 0;
                isLoop = false;
                this.Refresh();
                trackBar2.Refresh();
            }

           
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (ap.HasVideo)
            {
                screenShot();
            }
        }

        private void button10_MouseEnter(object sender, EventArgs e)
        {
            button10.BackgroundImage = Reso.captur2;
        }

        private void button10_MouseLeave(object sender, EventArgs e)
        {
            button10.BackgroundImage = Reso.captur1;
        }

        private void button9_MouseEnter(object sender, EventArgs e)
        {
            if (isLoop)
            {
                button9.BackgroundImage = Reso.loop1Hover;
            }
            else
            {
                button9.BackgroundImage = Reso.NoLoopHover;
            }
        }

        private void button9_MouseLeave(object sender, EventArgs e)
        {
            if (isLoop)
            {
                button9.BackgroundImage = Reso.loop1;
            }
            else
            {
                button9.BackgroundImage = Reso.NoLoop;
            }
        }
       

        private void trackBar2_Load_2(object sender, EventArgs e)
        {
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            if (TrackScroll)
            {
                ap.CurrentPosition = (int)trackBar2.Value;
            }
        }

        

        private void p1_Resize(object sender, EventArgs e)
        {
            ap.Owner = p1;
        }

        private void track_Speed_ValueChanged(object sender, EventArgs e)
        {
            ap.Rate =(int)track_Speed.Value;
            Properties.Settings.Default.speed = (int)track_Speed.Value;
            Properties.Settings.Default.Save();
        }

        private void track_Balance_ValueChanged(object sender, EventArgs e)
        {
            ap.Balance = (int)track_Balance.Value;
            Properties.Settings.Default.blance = (int)track_Balance.Value;
            Properties.Settings.Default.Save();
        }

        private void videozoom_ValueChanged(object sender, EventArgs e)
        {
            ap.zoom((int)videozoom.Value);
        }

        private void toolStripMenuOpen_Click(object sender, EventArgs e)
        {
            if (opf.ShowDialog() == DialogResult.OK)
            {
                albume.AddRange(opf.FileNames);
                LoadFile(opf.FileName);
                FillList();
                SetTrackBar();
                PlayFile();
            }
        }

        private void إختيارمجلدToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Collections.ArrayList arr = new System.Collections.ArrayList();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(folderBrowserDialog1.SelectedPath);

                System.IO.FileInfo[] f = dir.GetFiles();
                //string[] fs;
                foreach (System.IO.FileInfo s in f)
                {
                    arr.Add(s.FullName);
                    
                }

                albume.AddRange((string[])arr.ToArray(arr[0].GetType()));
                LoadFile(albume[0]);
                FillList();
                SetTrackBar();
                PlayFile();
            }
        }

        private void فتحموقعالملفToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ap.IsOpened)
            System.Diagnostics.Process.Start("explorer.exe", "/select," + ap.FileName);
        }

        private void trackBar2_MouseUped(object sender, EventArgs e)
        {
            if (!TrackScroll)
            {
                ap.CurrentPosition = (int)trackBar2.Value;
            }
            
        }

        private void قفلالتحكمToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (panel_Control.Enabled)
            {
                
                panel_Control.Enabled = false;
                //Holde = false;
            }
            else
            {
                panel_Control.Enabled = true;
                //Holde = true;
            }
        }

        private void فيالأعلىToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.TopMost)
            {
                this.TopMost = false;
                //isTop= false;
            }
            else
            {
                this.TopMost = true;
                //isTop = true;
            }
        }

        private void غلقالشاشةToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ap.HidWnd)
            {
                ap.HidWnd = true;
                //showWind = false;
            }
            else
            {
                ap.HidWnd = false;
                //isTop = true;
            }
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (toolStripComboBox1.SelectedIndex)
            {
                case 0:
                    theme = "red";
                    break;
                case 1:
                    theme = "blue";
                    break;
                case 2:
                    theme = "green";
                    break;
                case 3:
                    theme = "defualt";
                    break;
                case 4:
                    theme = "golde";
                    break;
                case 5:
                    theme = "volit";
                    break;

            }
            SetColorForm();
        }

        private void button1_MouseHover(object sender, EventArgs e)
        {
            button1.BackgroundImage = Reso.open2;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.BackgroundImage = Reso.open1;
        }

        

        private void لاتفعلشيءToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetMenuCheckMarks(sender);
            endPlay = "none";
        }

        private void غلقالبرنامجToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetMenuCheckMarks(sender);
            endPlay ="exit";
            
        }

        private void btn_SettingAlbum_MouseLeave(object sender, EventArgs e)
        {
            btn_SettingAlbum.BackColor = Color.Transparent;
        }

        private void btn_SettingAlbum_MouseHover(object sender, EventArgs e)
        {
            btn_SettingAlbum.BackColor = Color.Blue;
        }

        

        

        private void تقديمبعدإفلاتزرالماوسToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetMenuCheckMarks(sender);
            TrackScroll = false;
            Properties.Settings.Default.TrackScroll = TrackScroll;
            Properties.Settings.Default.Save();
        }

        private void تقديمأثناءالتحريكToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetMenuCheckMarks(sender);
            TrackScroll = true;
            Properties.Settings.Default.TrackScroll = TrackScroll;
            Properties.Settings.Default.Save();
        }

       

        private void إيقافToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetMenuCheckMarks(sender);
            looping = loop.none;
            Properties.Settings.Default.looping = "none";
            Properties.Settings.Default.Save();
        }

        private void ملفواحدToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetMenuCheckMarks(sender);
            looping = loop.On;
            Properties.Settings.Default.looping = "on";
            Properties.Settings.Default.Save();
        }

        private void الكلToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetMenuCheckMarks(sender);
            looping = loop.All;
            Properties.Settings.Default.looping = "all";
            Properties.Settings.Default.Save();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            switch (Visual)
            {
                case "wave":
                    if (ap.IsOpened && !ap.HasVideo)
                    DrawSomething();
                    break;
                case "pict":
                    if (ap.IsOpened && !ap.HasVideo)
                    {
                        moveShapes = true;
                        p1.Refresh();
                        moveShapes = false;
                    }
                    break;
            }

       
        }

        private void p1_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = DefaultCursor;
        }

        private void NoneVisualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (timer2.Enabled)
                timer2.Enabled = false;
            SetMenuCheckMarks(sender);
            Visual ="none";
            p1.BackgroundImageLayout = ImageLayout.Center;
            p1.BackgroundImage = Reso.login;
            Properties.Settings.Default.VisualMode = "none";
            Properties.Settings.Default.Save();
            p1.Invalidate();
        }

        private void WavVisualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!timer2.Enabled)
                timer2.Enabled = true;
            timer2.Interval = 100;
            SetMenuCheckMarks(sender);
            Visual ="wave";
            p1.BackgroundImage = null;
            Properties.Settings.Default.VisualMode = "wave";
            Properties.Settings.Default.Save();
        }

        private void غلقالجهازToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetMenuCheckMarks(sender);
            endPlay = "clos";
            //System.Diagnostics.Process.Start("shutdown","/s /t 0");
        }

        private void تأمينالجهازToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetMenuCheckMarks(sender);
            endPlay = "lock";
        }

        private void لاتفعلشيءToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            SetMenuCheckMarks(sender);
            endPlay = "none";
        }

        

        private void AlbumImgToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (timer2.Enabled)
                timer2.Enabled = false;
            SetMenuCheckMarks(sender);
            Visual = "AlbumPicture";
            if (ap.IsOpened)
            {
                if (!ap.HasVideo)
                {
                    GetMp3Info(ap.FileName);

                }
            }
            Properties.Settings.Default.VisualMode = "AlbumPicture";
            Properties.Settings.Default.Save();
            p1.Invalidate();
        }

       

        private void p1_Paint_1(object sender, PaintEventArgs e)
        {
            if(Visual=="pict"&&ap.IsPlaying)
            drawBonice(e);
        }

        private void VisualPictToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!timer2.Enabled)
                timer2.Enabled = true;
            //timer2.Interval = 25;
            SetMenuCheckMarks(sender);
            Visual = "pict";
            p1.BackgroundImage = null;
            Properties.Settings.Default.VisualMode = "pict";
            Properties.Settings.Default.Save();
            ShapesInit();
        }

        

        private void GetPathPictToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                imgBonice.Clear();
                getPictureBonice(folderBrowserDialog1.SelectedPath);
                Properties.Settings.Default.pathDict = folderBrowserDialog1.SelectedPath;
                Properties.Settings.Default.Save();
                
                /*if (img2!=null||img2.Length>0)
                {
                    for (int i = 0; i < img2.Length; i++)
                    {
                       
                        img2[i].Dispose();
                    }
                }*/
                
                ShapesInit();
            }
        }
       

        private void إOffPraysTimerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetMenuCheckMarks(sender);
            Properties.Settings.Default.OnPraysTimer = false;
            Properties.Settings.Default.Save();
            IsPraysTimer = false;
        }

        private void OnPrayTimerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SetMenuCheckMarks(sender);
            Properties.Settings.Default.OnPraysTimer = true;
            Properties.Settings.Default.Save();
            IsPraysTimer = true;
        }

        private void SettPraysTimerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PraysTimes p = new PraysTimes();
            if (p.ShowDialog() == DialogResult.OK)
            {
                dat = Convert.ToDateTime(p.textDawn.Text);
                Properties.Settings.Default.DawnTime = Convert.ToDateTime(p.textDawn.Text);
                Properties.Settings.Default.NoonTime = Convert.ToDateTime(p.textNoon.Text);
                Properties.Settings.Default.ANoonTime = Convert.ToDateTime(p.textANoon.Text);
                Properties.Settings.Default.SunsetTime = Convert.ToDateTime(p.textSunset.Text);
                Properties.Settings.Default.NightTime = Convert.ToDateTime(p.textNight.Text);
                Properties.Settings.Default.WaitTime = new TimeSpan(0,(int)p.numericUpDown16.Value,0);
                Properties.Settings.Default.Save();
                LoadSettPraysTimer();
               
            }
        }

        private void timerPrays_Tick(object sender, EventArgs e)
        {
            if (IsPraysTimer)
            {
                PraryTime();
            }
        }
        

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            helpe h = new helpe();
            h.ShowDialog();
        }
        #endregion event
    }
}
