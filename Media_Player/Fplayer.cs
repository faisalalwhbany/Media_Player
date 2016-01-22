using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ap = SmartFplayer.API;
namespace SmartFplayer
{
    class Fplayer
    {
        private string scommand, filname, shandle;
        private bool Opened, Playing, Paused, Looping, MutedAll, MutedLeft, MutedRight, fullscreen;
        private int avolume = 0;
        private int balance = 0;
        private int err;
        private double Lng;
        private int speed = 1000;
        private System.Windows.Forms.Panel owner;
        private double position;
        //private double seek;
        private string source;
        private bool hasvideo = false;
        private bool hidwnd = false;

        #region constructor
        public Fplayer()
        {
            Opened = false;
            scommand = string.Empty;
            filname = string.Empty;
            Playing = false;
            Paused = false;
            Looping = false;
            MutedAll = MutedLeft = MutedRight = false;
            avolume = 1000;
            speed = 1000;
            Lng = 0;
            balance = 0;
            err = 0;
            shandle = "SFplayer";
            fullscreen = false;
        }
        public Fplayer(string FileName)
        {
            Opened = false;
            scommand = string.Empty;
            filname = FileName;
            Playing = false;
            Paused = false;
            Looping = false;
            MutedAll = MutedLeft = MutedRight = false;
            avolume = 1000;
            speed = 1000;
            Lng = 0;
            balance = 0;
            err = 0;
            shandle = FileName;
            fullscreen = false;
            Open(FileName);
        }
        ~Fplayer()
        {
            Close();
        }
        #endregion constructor
        
        
        #region Properties
        public bool HasVideo
        {
            get { return hasvideo; }
        }

        public string VideoSize
        {
            get { return source; }
            set
            {
                source = value;
                if (Opened)
                {
                    scommand = string.Format("put {0} source at " + 0 + " " + 0 +
                                     " " + Owner.Width + " " + Owner.Height, shandle);
                    ap.mciSendString(scommand, null, 0, IntPtr.Zero);
                }
            }
        }
        public bool FullScreen
        {
            get { return fullscreen; }
            set
            {


                fullscreen = value;
                if (Opened && Playing)
                {
                    if (fullscreen)
                    {
                        scommand = String.Format("play {0}{1} fullscreen notify", shandle, Looping ? " repeat" : string.Empty);
                        if ((err = ap.mciSendString(scommand, null, 0, IntPtr.Zero)) != 0)
                            error(err);
                    }
                    else
                    {
                        scommand = String.Format("play {0}{1}  notify", shandle, Looping ? " repeat" : string.Empty);
                        if ((err = ap.mciSendString(scommand, null, 0, IntPtr.Zero)) != 0)
                            error(err);
                    }

                }
            }

        }
        public System.Windows.Forms.Panel Owner
        {

            get { return owner; }
            set
            {
                owner = value;
                if (Opened)
                {
                    scommand = string.Format("put {0} window at " + 0 + " " + 0 +
                                 " " + Owner.Width + " " + Owner.Height, shandle);
                    ap.mciSendString(scommand, null, 0, IntPtr.Zero);
                        //error(err);

                }
            }
        }
        public int Volume
        {
            get
            {

                return avolume;

            }
            set
            {
                avolume = value;
                if (Opened)
                {
                    scommand = string.Format("setaudio {0} volume to {1}", shandle, avolume);
                    if ((err = ap.mciSendString(scommand, null, 0, IntPtr.Zero)) != 0)
                        error(err);
                }

                //this.Invalidate();
            }
        }
        public int Balance
        {

            get { return balance; }
            set
            {
                if (Opened && (value >= -1000 && value <= 1000))
                {
                    balance = value;
                    double vPct = Convert.ToDouble(avolume) / 1000.0;

                    if (value < 0)
                    {
                        scommand = string.Format("setaudio {0} left volume to {1:#}", shandle, avolume);
                        if ((err = ap.mciSendString(scommand, null, 0, IntPtr.Zero)) != 0)
                            error(err);
                        scommand = string.Format("setaudio {0} right volume to {1:#}", shandle, (1000 + value) * vPct);
                        if ((err = ap.mciSendString(scommand, null, 0, IntPtr.Zero)) != 0)
                            error(err);
                    }
                    else
                    {
                        scommand = string.Format("setaudio {0} right volume to {1:#}", shandle, avolume);
                        if ((err = ap.mciSendString(scommand, null, 0, IntPtr.Zero)) != 0)
                            error(err);
                        scommand = string.Format("setaudio {0} left volume to {1:#}", shandle, (1000 - value) * vPct);
                        if ((err = ap.mciSendString(scommand, null, 0, IntPtr.Zero)) != 0)
                            error(err);
                    }
                }
            }
        }

        public bool MuteLeft
        {
            get
            {
                return MutedLeft;
            }
            set
            {
                MutedLeft = value;
                if (MutedLeft)
                {
                    scommand = string.Format("setaudio {0} left off", shandle);
                    if ((err = ap.mciSendString(scommand, null, 0, IntPtr.Zero)) != 0)
                        error(err);
                }
                else
                {
                    scommand = string.Format("setaudio {0} left on", shandle);
                    if ((err = ap.mciSendString(scommand, null, 0, IntPtr.Zero)) != 0)
                        error(err);

                }
            }
        }
        public bool HidWnd
        {

            get
            {
                return hidwnd;
            }
            set
            {
                hidwnd = value;
                if (hidwnd)
                {
                    scommand = String.Format("window {0} state hide", shandle);
                    if ((err = ap.mciSendString(scommand, null, 0, IntPtr.Zero)) != 0)
                        error(err);
                }
                else
                {
                    scommand = String.Format("window {0} state show", shandle);
                    if ((err = ap.mciSendString(scommand, null, 0, IntPtr.Zero)) != 0)
                        error(err);
                }
            }
        }

        public bool MuteAll
        {
            get
            {
                return MutedAll;
            }
            set
            {
                MutedAll = value;
                if (MutedAll)
                {
                    scommand = String.Format("setaudio {0} off", shandle);
                    if ((err = ap.mciSendString(scommand, null, 0, IntPtr.Zero)) != 0)
                        error(err);
                }
                else
                {
                    scommand = String.Format("setaudio {0} on", shandle);
                    if ((err = ap.mciSendString(scommand, null, 0, IntPtr.Zero)) != 0)
                        error(err);
                }
            }

        }

        public bool MuteRight
        {
            get
            {
                return MutedRight;
            }
            set
            {
                MutedRight = value;
                if (MutedLeft)
                {
                    scommand = string.Format("setaudio {0} right off", shandle);
                    if ((err = ap.mciSendString(scommand, null, 0, IntPtr.Zero)) != 0)
                        error(err);
                }
                else
                {
                    scommand = string.Format("setaudio {0} right on", shandle);
                    if ((err = ap.mciSendString(scommand, null, 0, IntPtr.Zero)) != 0)
                        error(err);

                }
            }
        }

        public double Duration
        {
            get
            {
                if (Opened)
                    return Lng;
                else
                    return 0;
            }
        }

        

        public double CurrentPosition
        {
            get
            {
                if (Opened && Playing)
                {
                    StringBuilder s = new StringBuilder(128);
                    scommand = String.Format("status {0} position ", shandle);
                    if ((err = ap.mciSendString(scommand, s, 128, IntPtr.Zero)) != 0)
                        error(err);
                    position = Convert.ToUInt64(s.ToString());
                    return position;
                }
                else
                    return 0;
            }
            set
            {
                //seek = value;
                if (Opened && value <= Lng)
                {
                    scommand = String.Format("seek {0} to {1}", shandle, value);
                    if ((err = ap.mciSendString(scommand, null, 0, IntPtr.Zero)) != 0)
                        error(err);
                    scommand = String.Format("play {0}{1} notify", shandle, Looping ? " repeat" : string.Empty);
                    if ((err = ap.mciSendString(scommand, null, 0, IntPtr.Zero)) != 0)
                        error(err);

                }
            }
        }

        public string FileName
        {
            get
            {
                return filname;
            }
        }

        public string SHandle
        {
            get
            {
                return shandle;
            }
        }

        public bool IsOpened
        {
            get
            {
                return Opened;
            }
        }

        public bool IsPlaying
        {
            get
            {
                return Playing;
            }
        }

        public bool IsPaused
        {
            get
            {
                return Paused;
            }
        }

        public bool IsLooping
        {
            get
            {
                return Looping;
            }
            set
            {
                Looping = value;
                if (Opened && Playing && !Paused)
                {
                    if (Looping)
                    {
                        scommand = String.Format("play {0} notify repeat", shandle);
                        if ((err = ap.mciSendString(scommand, null, 0, IntPtr.Zero)) != 0)
                            error(err);
                    }
                    else
                    {
                        scommand = String.Format("play {0} notify", shandle);
                        if ((err = ap.mciSendString(scommand, null, 0, IntPtr.Zero)) != 0)
                            error(err);
                    }
                }
            }
        }

        public int Rate
        {
            get
            {
                return speed;
            }
            set
            {
                if (Opened && value >= 3 && value <= 4353)
                {
                    speed = value;

                    scommand = String.Format("set {0} speed {1}", shandle, speed);
                    if ((err = ap.mciSendString(scommand, null, 0, IntPtr.Zero)) != 0)
                        error(err);
                }
            }
        }
        #endregion Properties



        #region method
        public void Open(string sFileName)
        {
            if (!Opened)
            {
                
                //scommand = String.Format("open \"" + sFileName + "\" type mpegvideo alias {0}" + " parent " + owner.Handle.ToString() + " style child", shandle);
                scommand = "open \"" + sFileName + "\""+ " type mpegvideo alias " + shandle + " parent " + owner.Handle.ToString() + " style child ";
                if ((err = ap.mciSendString(scommand, null, 0, IntPtr.Zero)) != 0)
                {
                    error(err);


                }
                
                    StringBuilder ret = new StringBuilder(1024);
                    scommand = string.Format("status {0} volume", shandle);
                    if ((err = ap.mciSendString(scommand, ret, ret.Capacity, IntPtr.Zero)) != 0)
                    {
                        error(err);


                    }
                    avolume = Convert.ToInt32(ret.ToString());
                    filname = sFileName;
                    Opened = true;
                    Playing = false;
                    Paused = false;
                    scommand = String.Format("set {0} time format milliseconds", shandle);
                    if ((err = ap.mciSendString(scommand, null, 0, IntPtr.Zero)) != 0)
                    {
                        error(err);


                    }
                    scommand = String.Format("set {0} seek exactly on", shandle);
                    if ((err = ap.mciSendString(scommand, null, 0, IntPtr.Zero)) != 0)
                    {
                        error(err);

                    }
                    CalculateLength();
                }

            
            else
            {
                this.Close();
                this.Open(sFileName);

            }

        }

        private bool CalculateLength()
        {
            try
            {

                StringBuilder str = new StringBuilder(1024);
                scommand = string.Format("status {0} length", shandle);
                if ((err = ap.mciSendString(scommand, str, str.Capacity, IntPtr.Zero)) != 0)
                    error(err);
               Lng = Convert.ToDouble(str.ToString());
                return true;
            }
            catch
            {
                return false;
                
            }
        }

        public void Play()
        {
            if (Opened)
            {

                if (MutedAll)
                {
                    scommand = String.Format("setaudio {0} off", shandle);
                    if ((err = ap.mciSendString(scommand, null, 0, IntPtr.Zero)) != 0)
                        error(err);
                }
                else
                {
                    scommand = String.Format("setaudio {0} on", shandle);
                    if ((err = ap.mciSendString(scommand, null, 0, IntPtr.Zero)) != 0)
                        error(err);
                }

                if (!Playing)
                {
                   scommand = string.Format("put {0} window at " + 0 + " " + 0 +
                         " " + owner.Width + " " + owner.Height, shandle);
                   if ((err = ap.mciSendString(scommand, null, 0, IntPtr.Zero)) != 0)
                   {
                       error(err);
                       hasvideo = false;

                   }
                   else
                   {
                       hasvideo = true;
                   }

                    Playing = true;
                    scommand = String.Format("play {0}{1}  notify", shandle, Looping ? " repeat" : string.Empty);
                    if ((err = ap.mciSendString(scommand, null, 0, IntPtr.Zero)) != 0)
                        error(err);


                }
                else
                {
                    if (Paused)
                    {
                        Paused = false;
                        scommand = String.Format("play {0}{1} notify", shandle, Looping ? " repeat" : string.Empty);
                        if ((err = ap.mciSendString(scommand, null, 0, IntPtr.Zero)) != 0)
                            error(err);
                    }
                }
            }
        }
        public void Pause()
        {
            if (Opened)
            {
                if (!Paused)
                {
                    Paused = true;
                    scommand = String.Format("pause {0}", shandle);
                    if ((err = ap.mciSendString(scommand, null, 0, IntPtr.Zero)) != 0)
                        error(err);
                }
            }
        }
        public void Close()
        {
            if (Opened)
            {
                scommand = String.Format("close {0}  Wait",shandle);
                if ((err = ap.mciSendString(scommand, null, 0, IntPtr.Zero)) != 0)
                    error(err);
                filname = string.Empty;
                Opened = false;
                Playing = false;
                Paused = false;
            }
        }

       /* public void Seek(double milliseconds)
        {
            if (Opened && milliseconds <= Lng)
            {
                if (Playing)
                {
                    if (Paused)
                    {
                        scommand = String.Format("seek {0} to {1}", shandle, milliseconds);
                        if ((err = ap.mciSendString(scommand, null, 0, IntPtr.Zero)) != 0)
                            error(err);
                    }
                    else
                    {
                        scommand = String.Format("seek {0} to {1}", shandle, milliseconds);
                        if ((err = ap.mciSendString(scommand, null, 0, IntPtr.Zero)) != 0)
                            error(err);
                        scommand = String.Format("play {0}{1} notify", shandle, Looping ? " repeat" : string.Empty);
                        if ((err = ap.mciSendString(scommand, null, 0, IntPtr.Zero)) != 0)
                            error(err);
                    }
                }
            }
        }*/

        public void Stop()
        {
            if (Opened && Playing)
            {
                Playing = false;
                Paused = false;
                scommand = String.Format("seek {0} to start", shandle);
                if ((err = ap.mciSendString(scommand, null, 0, IntPtr.Zero)) != 0)
                    error(err);
                scommand = String.Format("stop {0}", shandle);
                if ((err = ap.mciSendString(scommand, null, 0, IntPtr.Zero)) != 0)
                    error(err);

            }
        }
        public void PlayFromTo(int start, int end)
        {
            if (Opened)
            {
                Playing = true;
                //string re = "repeat";
                scommand = String.Format("play {0} from {1} to {2} notify", shandle, start, end);
                if ((err = ap.mciSendString(scommand, null, 0, IntPtr.Zero)) != 0)
                    error(err);
            }
        }
        public void error(int erro)
        {
            StringBuilder s = new StringBuilder(1024);
            ap.mciGetErrorString(erro, s, s.Capacity);
            

        }
        public void zoom(int factor)
        {
            if (Opened&&hasvideo)
            {
                int wi = owner.Width + factor;
                int he = owner.Height + (factor);
                int x1 = 0 - (factor / 2);
                int y1 = 0 - (factor / 2);
                scommand = string.Format("put {0} window at " + x1 + " " + y1 +
                     " " + wi + " " + he, shandle);
                if ((err = ap.mciSendString(scommand, null, 0, IntPtr.Zero)) != 0)
                    error(err);

            }

        }
        #endregion method
    }
}
