using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace SmartFplayer
{
    class API
    {
        [DllImport("winmm.dll",CharSet=CharSet.Ansi)]
        public static extern int mciSendString(string Scomm,
            StringBuilder returnval, int returnlength,
            IntPtr winhandl);
        [DllImport("winmm.dll")]
        public static extern int mciGetErrorString(int errCode, StringBuilder errMsg, int buflen);
        [DllImport("user32")]
        public static extern void LockWorkStation();//تأمين الكمبيوتر
        [DllImport("user32")]
        public static extern int ExitWindowsEx(uint uflag,uint reasion);//تسجيل خروج للمستخدم

    }
}
