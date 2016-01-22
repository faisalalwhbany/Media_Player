using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace SmartFplayer
{
    class ID3
    {
        private string artist;
        private string title;
        private string album;
        private string year;
        private Image cover;


        private DirectoryInfo directoryInfo;
        private string searchFileName;
        private string searchDirectoryName;
        private string[] searchKeyWords = { "*front*", "*cover*" }; // , "*albumart*large*" };
        private string[] searchExtensions = { ".jpg", ".jpeg", ".bmp", ".png", ".gif", ".tiff" };
        //private bool skipEmbedPic=true;

        public void GetMediaInfo(string mediaFile)
        {
            byte[] header = new byte[10];
            byte[] buffer = new byte[128];
            int totalSize;
            int frameSize;
            string frameID;
            int found = 0;
            
            byte[] textEncoding = new byte[1];
            int i;
            int totalHeader;

            artist = string.Empty;
            title = string.Empty;
            album = string.Empty;
            year = string.Empty;
            if (cover != null)
            {
                cover.Dispose();
                cover = null;
            }

            if (!new Uri(mediaFile).IsFile)
            {
                album = mediaFile;
                title = Path.GetFileNameWithoutExtension(mediaFile);
                return;
            }

            FileStream fs =File.OpenRead(mediaFile);

            // read tag header ID3v2
            fs.Read(header, 0, 10);
            if (Encoding.Default.GetString(header, 0, 3) == "ID3")
            {
                
                totalSize = header[9] + (0x80 * header[8]) + (0x8000 * header[7]) + (0x800000 * header[6]);
                // check for extended header
                if ((header[5] & 0x40) == 0x40)
                {
                    fs.Read(header, 0, 10);
                    frameSize = header[3] + (0x80 * header[2]) + (0x8000 * header[1]) + (0x800000 * header[0]);
                    fs.Position += frameSize;
                }

                while (fs.Position < totalSize)
                {
                    // read frameheader
                    fs.Read(header, 0, 10);
                    frameID =Encoding.Default.GetString(header, 0, 4);
                    System.Windows.Forms.MessageBox.Show(frameID);
                    if (frameID == "APIC")
                    {
                        
                        frameSize = (0x1000000 * header[4]) + (0x10000 * header[5]) + (0x100 * header[6]) + header[7];
                    } // differs from the description by ID3.org
                    else
                    {
                        
                        frameSize = (0x800000 * (header[4] & 0x7F)) + (0x8000 * (header[5] & 0x7F)) + (0x80 * (header[6] & 0x7F)) + (header[7] & 0x7F); }
                    if ((header[9] & 0x60) != 0) fs.Read(textEncoding, 0, 1);
                    switch (frameID)
                    { 
                        case "APIC": // picture
                            
                            if (cover != null)
                            {
                                fs.Position += frameSize;
                                break;
                            }

                            fs.Read(textEncoding, 0, 1);
                            i = 0;
                            do
                            {
                                fs.Read(textEncoding, 0, 1);
                                buffer[i] = textEncoding[0];
                            }
                            while (buffer[i++] != 0);
                            totalHeader = i;

                            fs.Read(textEncoding, 0, 1);
                            i = 0;
                            do
                            {
                                fs.Read(textEncoding, 0, 1);
                                buffer[i] = textEncoding[0];
                            }
                            while (buffer[i++] != 0);
                            totalHeader += i;

                            byte[] newBuffer = new byte[frameSize-totalHeader];
                            fs.Read(newBuffer, 0, frameSize-totalHeader);

                            MemoryStream ms = new MemoryStream(newBuffer);
                             cover = Image.FromStream(ms);
                            
                            
                            ms.Close();
                            break;
                        case "TALB": // Album
                            
                            if (frameSize > buffer.Length) buffer = new byte[frameSize];
                            fs.Read(buffer, 0, frameSize);
                            if (buffer[1] == 0xFF) album = Encoding.Unicode.GetString(buffer, 1, frameSize - 1).TrimEnd('\0');
                            else if (buffer[1] == 0xFE) album = Encoding.BigEndianUnicode.GetString(buffer, 1, frameSize - 1).TrimEnd('\0');
                            else album = Encoding.Default.GetString(buffer, 1, frameSize - 1).TrimEnd('\0');
                            found++;
                            break;
                        case "TIT2": // Title
                            
                            if (frameSize > buffer.Length) buffer = new byte[frameSize];
                            fs.Read(buffer, 0, frameSize);
                            if (buffer[1] == 0xFF) title = Encoding.Unicode.GetString(buffer, 1, frameSize - 1).TrimEnd('\0');
                            else if (buffer[1] == 0xFE) title = Encoding.BigEndianUnicode.GetString(buffer, 1, frameSize - 1).TrimEnd('\0');
                            else title = Encoding.Default.GetString(buffer, 1, frameSize - 1).TrimEnd('\0');
                            found++;
                            break;
                        case "TPE1": // Lead Performers
                            
                            if (frameSize > buffer.Length) buffer = new byte[frameSize];
                            fs.Read(buffer, 0, frameSize);
                            if (artist == string.Empty)
                            {
                                if (buffer[1] == 0xFF) artist = Encoding.Unicode.GetString(buffer, 1, frameSize - 1).TrimEnd('\0');
                                else if (buffer[1] == 0xFE) artist = Encoding.BigEndianUnicode.GetString(buffer, 1, frameSize - 1).TrimEnd('\0');
                                else artist = Encoding.Default.GetString(buffer, 1, frameSize - 1).TrimEnd('\0');
                            }
                            found++;
                            break;
                        case "TPE2": // Band
                            if (frameSize > buffer.Length) buffer = new byte[frameSize];
                            fs.Read(buffer, 0, frameSize);
                            if (buffer[1] == 0xFF) artist = Encoding.Unicode.GetString(buffer, 1, frameSize - 1).TrimEnd('\0');
                            else if (buffer[1] == 0xFE) artist = Encoding.BigEndianUnicode.GetString(buffer, 1, frameSize - 1).TrimEnd('\0');
                            else artist = Encoding.Default.GetString(buffer, 1, frameSize - 1).TrimEnd('\0');
                            found++;
                            break;
                        default:
                            fs.Position += frameSize;
                            break;
                    }
                   
                }
            }

            // If nothing (or not all wanted info) found, try two other methods:
            if (found < 3)
            {
                // read tag ID3v1
                fs.Seek(-128, SeekOrigin.End);
                fs.Read(buffer, 0, 128);
                if (Encoding.Default.GetString(buffer, 0, 3) == "TAG")
                {
                    if (title == string.Empty) title = Encoding.Default.GetString(buffer, 3, 30).TrimEnd('\0');
                    if (artist == string.Empty) artist = Encoding.Default.GetString(buffer, 33, 30).TrimEnd('\0');
                    if (album == string.Empty) album = Encoding.Default.GetString(buffer, 63, 30).TrimEnd('\0');
                    if (year == string.Empty) year = Encoding.Default.GetString(buffer, 93, 4).TrimEnd('\0');
                    
                }

                // Get info from file path
                if (title == string.Empty) title = Path.GetFileNameWithoutExtension(mediaFile);
                if (album == string.Empty)
                {
                    album = Path.GetDirectoryName(mediaFile);
                    album = album.Substring(album.LastIndexOf("\\") + 1);
                }
            }
            fs.Close();
            fs.Dispose();
            
            //if (cover == null) GetLocalAlbumArt(mediaFile);
           // if (cover != null && reColor) cover = ReColorImage((Bitmap)cover);
        }
        private bool GetLocalAlbumArt(string mediaFile)
        {
            bool retVal = true;
            bool result = false;
            try
            {
                directoryInfo = new DirectoryInfo(System.IO.Path.GetDirectoryName(mediaFile));
                searchFileName = Path.GetFileNameWithoutExtension(mediaFile);
                searchDirectoryName = directoryInfo.Name;

                // 1. search using the file name
                if (!SearchAlbumArt(searchFileName))
                {
                    // 2. search using the directory name
                    if (!SearchAlbumArt(searchDirectoryName))
                    {
                        // 3. search using keywords
                        int i = 0;
                        do result = SearchAlbumArt(searchKeyWords[i++]);
                        while (!result && i < searchKeyWords.Length);

                        if (!result)
                        {
                            // 4. find largest file
                            if (!SearchAlbumArt("*")) retVal = false;
                        }
                    }
                }
            }
            catch { }
            directoryInfo = null;
            return retVal;
        }
        private bool SearchAlbumArt(string searchName)
        {
            bool retVal = false;
            List<FileInfo> filesFound = new List<FileInfo>();

            // find all files
            for (int i = 0; i < searchExtensions.Length; i++) filesFound.AddRange(directoryInfo.GetFiles(searchName + searchExtensions[i]));

            // if any, get the largest
            if (filesFound.Count > 0)
            {
                long length = 0;
                int index = 0;

                for (int j = 0; j < filesFound.Count; j++)
                {
                    if (filesFound[j].Length > length)
                    {
                        length = filesFound[j].Length;
                        index = j;
                    }
                }
                cover = Image.FromFile(filesFound[index].FullName);
                filesFound.Clear();
                retVal = true;
            }
            return retVal;
        }

        //خصائص
        public Image Cover
        {
            get { return cover; }

        }

        public string Album
        {
            get { return album; }

        }
        public string Artist
        {
            get { return artist; }

        }
        public string Title
        {
            get { return title; }

        }
        public string Year
        {
            get { return year; }

        }
    }
}
