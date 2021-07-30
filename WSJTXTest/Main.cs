using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HazTech;
using HazTech.Util;

using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace WSJTXTest
{
    public partial class Main : Form
    {
        public Thread th;
        public ThreadStart ths;
        public WSJTXHazLib db = new WSJTXHazLib();
        public Main()
        {
            InitializeComponent();
            SuppressScriptErrorsOnly(webURL);
            db = new WSJTXHazLib(this);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            th = new Thread(new ThreadStart(db.Start));
            th.Start();
            MessageBox.Show("Starting up!");
        }
        // Hides script errors without hiding other dialog boxes.
        private void SuppressScriptErrorsOnly(WebBrowser browser)
        {
            // Ensure that ScriptErrorsSuppressed is set to false.
            browser.ScriptErrorsSuppressed = false;

            // Handle DocumentCompleted to gain access to the Document object.
            browser.DocumentCompleted +=
                new WebBrowserDocumentCompletedEventHandler(
                    browser_DocumentCompleted);
        }

        private void browser_DocumentCompleted(object sender,
            WebBrowserDocumentCompletedEventArgs e)
        {
            ((WebBrowser)sender).Document.Window.Error +=
                new HtmlElementErrorEventHandler(Window_Error);
        }

        private void Window_Error(object sender,
            HtmlElementErrorEventArgs e)
        {
            // Ignore the error and suppress the error dialog box. 
            e.Handled = true;
        }
        private void Main_Load(object sender, EventArgs e)
        {

        }
        public void DisposeAll()
        {
            db.Dispose();
        }
    }
    public class WSJTXHazLib
    {
        Main obj;

        InvokeLib sform;
        IPEndPoint ipep = null;
        UdpClient newsock = null;
        IPEndPoint sender = null;
        public bool Active = false;
        public bool Connected = false;
        public WSJTXHazLib(Main obj)
        {
            this.obj = obj;
            sform = new InvokeLib(obj);
        }
        public WSJTXHazLib()
        {

        }
        public bool CheckContain(string Contain, string Packets)
        {
            /*(string[] aPacket = Packets.Split(' ');
            string[] aContain = Contain.Split(' ');
            int cPacket = aPacket.Count();
            int cContain = aContain.Count();
            for (int y = 0; y < cContain; y++)
            {
                if (aPacket[y].Equals(aContain[y]))
                    continue;
                else
                    return false;
            }
            return true;*/
            return (Packets.Contains(Contain));

        }
        public string BytetoString(string BytesString)
        {
            string[] bytesStr = BytesString.Split(' ');
            string msg = "";
            foreach (string ch in bytesStr)
            {
                if (ch.Length < 1)
                    continue;
                msg += (char)Convert.ToInt32(ch);
            }
            return msg;
        }
        public string getBetween(string strSource, string strStart, string strEnd)
        {
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                int Start, End;
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return BytetoString(strSource.Substring(Start, End - Start));
            }

            return "";
        }
        public string getAfter(string strSource, string strStart)
        {
            if (strSource.Contains(strStart))
            {
                string code = strSource.Split(new[] { strStart }, StringSplitOptions.None)[1];
                return BytetoString(code);
            }

            return "";
        }
        public string getBefore(string strSource, string strStart)
        {
            if (strSource.Contains(strStart))
            {
                string code = strSource.Split(new[] { strStart }, StringSplitOptions.None)[0];
                return BytetoString(code);
            }

            return "";
        }
        public void Start()
        {
            if (Connected)
                Dispose();
            if (Active)
                Dispose();
            MainStart();
        }
        public void Dispose()
        {
            try
            {
                Active = false;
                Connected = false;
                newsock.Close();
            }
            catch { }
        }
        public void Log(String Msg)
        {
            sform.parse(obj.lstLog, DateTime.Now + ": " + Msg);
            sform.parse(obj.mOut, Msg);
        }
        public void MainStart()
        {
            byte[] data = new byte[99999];
            ipep = new IPEndPoint(IPAddress.Any, 2238);
            newsock = new UdpClient(ipep);
            Active = true;
            sender = new IPEndPoint(IPAddress.Any, 0);
            Log("Waiting for WSJT-X connected");
            data = newsock.Receive(ref sender);
            Log("WSJT - X Connected");

            Connected = true;

            //Console.WriteLine("Message received from {0}:", sender.ToString());
            //Console.WriteLine(Encoding.ASCII.GetString(data, 0, data.Length));

            string welcome = "Welcome to my test server";
            data = Encoding.ASCII.GetBytes(welcome);
            //newsock.Send(data, data.Length, sender);

            string _msg = "";
            string _msgb = "";
            string msg = "";
            string msgb = "";
            while (Active)
            {
                _msg = "";
                _msgb = "";
                msg = "";
                msgb = "";
                bool writefile = false;
                try
                {
                    data = newsock.Receive(ref sender);
                    _msg += Encoding.ASCII.GetString(data, 0, data.Length);
                    _msg += "\r\n\r\n";

                    char[] charArr = _msg.ToCharArray();
                    foreach (char ch in charArr)
                    {
                        _msgb += (int)ch + " ";
                        //msgb += (int)ch + ": " + ch + "\r\n";
                    }
                    if (CheckContain("63 63 63 63 0 0 0 2 0 0 0 1 0 0 0 6 87 83 74 84 45 88 0", _msgb))
                    {
                        //Ignore repeatitive text

                        //Send only 
                        int cCode = 0;

                        //Default - annoying
                        if (CheckContain("7 68 101 102 97 117 108 116 ", _msgb))
                        {
                            cCode = -1;
                            continue;
                        }

                        //Active - Enable TX
                        if (CheckContain("63 63 63 63 0 0 0 2 0 0 0 1 0 0 0 6 87 83 74 84 45 88 0 0 0 0 0 107 63 63 0 0 0 3 70 84 56 0 0 0 6 86 85 50 79 82 81 0 0 0 3 45 49 53 0 0 0 3 70 84 56 1 1 0 0 0 3 117 0 0 2 87 0 0 0 6 57 87 56 72 65 90 0 0 0 6 79 74 53 49 68 79 0 0 0 4 77 75 56 48 0 63 63 63 63 0 0 63 63 63 63 63 63 63 63 0 0 0 7 68 101 102 97 117 108 116 0 0 0 37 67 81 32 57 87 56 72 65 90 32 79 74 53 49 32 32 32 32 32 32 32 32 32 32 32 32 32 32 32 32 32 32 32 32 32 32 32 13 10 13 10 ", _msgb))
                        {
                            cCode = 1;
                        }

                        //Deactive - Enable TX
                        else if (CheckContain("63 63 63 63 0 0 0 2 0 0 0 1 0 0 0 6 87 83 74 84 45 88 0 0 0 0 0 107 63 63 0 0 0 3 70 84 56 0 0 0 6 86 85 50 79 82 81 0 0 0 3 45 49 53 0 0 0 3 70 84 56 1 1 0 0 0 3 117 0 0 2 87 0 0 0 6 57 87 56 72 65 90 0 0 0 6 79 74 53 49 68 79 0 0 0 4 77 75 56 48 0 63 63 63 63 0 0 63 63 63 63 63 63 63 63 0 0 0 7 68 101 102 97 117 108 116 0 0 0 37 67 81 32 57 87 56 72 65 90 32 79 74 53 49 32 32 32 32 32 32 32 32 32 32 32 32 32 32 32 32 32 32 32 32 32 32 32 13 10 13 10 ", _msgb))
                        {
                            cCode = 2;
                        }
                        writefile = true;
                        msg += _msg;
                        msg += _msgb;
                        msg += msgb;
                        msg += "\r\nSENT CODE:" + cCode + " --NONCHECK--------------------------------------------------";
                        //Console.WriteLine(msg);
                        //Log(msg);


                    }
                    else if (CheckContain("63 63 63 63 0 0 0 2 0 0 0 0 0 0 0 6 87 83 74 84 45 88 0", _msgb))
                    {
                        //Ignore repeatitive text "pingtimeout" = c19d62
                    }
                    else if (CheckContain("0 0 13 10 13 10", _msgb)) //EOF NULL NULL
                    {
                        //Receive only 

                        // 0 0 13 10 13 10
                        int cCode = 0;
                        string _bytesString = "";
                        if (CheckContain("67 81 32", _msgb))
                        {
                            cCode = 1;
                            //_bytesString = getAfter(_msgb, "67 81 32 ");
                            _bytesString = getBetween(_msgb, "67 81 32 ", " 0 0 13 10 13 10");
                        }
                        else if (CheckContain("126 0 0 0 17", _msgb)) //DC1
                        {
                            cCode = 17;
                            //126 0 0 0 17
                            _bytesString = getBetween(_msgb, "126 0 0 0 17 ", " 0 0 13 10 13 10");
                        }
                        else if (CheckContain("126 0 0 0 18", _msgb)) //DC2
                        {
                            cCode = 18;
                            //126 0 0 0 18
                            _bytesString = getBetween(_msgb, "126 0 0 0 18 ", " 0 0 13 10 13 10");
                        }
                        else if (CheckContain("126 0 0 0", _msgb)) //??
                        {
                            cCode = 126;
                            //126 0 0 0 17
                            _bytesString = getBetween(_msgb, "126 0 0 0 ", " 0 0 13 10 13 10");
                        }
                        writefile = true;
                        msg += _msg;
                        msg += _msgb;
                        msg += msgb;
                        if (_bytesString.Length > 0)
                        {
                            msg += "\r\n ---Converted--- Code : " + cCode + "\r\n";
                            msg += _bytesString;
                            string _output = "";
                            if (cCode >= 1)
                            {
                                Regex rgx = new Regex("[^a-zA-Z0-9 -]");
                                _bytesString = rgx.Replace(_bytesString, "");
                                if (cCode == 1) //CQ
                                {
                                    string[] cW = _bytesString.Split(' ');
                                    string CS = "";
                                    string Location = "";
                                    string cType = "";
                                    if (cW.Length > 2)
                                    {
                                        _output = "CS: " + cW[1] + " Location: " + cW[2] + " Contest Type: " + cW[0];
                                        CS = cW[1];
                                        Location = cW[2];
                                        cType = cW[0];
                                    }
                                    else
                                    {
                                        _output = "CS: " + cW[0] + " Location: " + cW[1];
                                        CS = cW[0];
                                        Location = cW[1];
                                    }
                                    sform.parse(obj.webURL, new Uri("https://www.haztech.com.my/map/?grid="+Location));
                                    sform.parse(obj.webURL, InvokeLib.WebBrowserType.Update);
                                }
                                else if (cCode >= 17)
                                {
                                    string[] cW = _bytesString.Split(' ');
                                    if (cW.Length > 2)
                                    {
                                        _output = "Tgt: " + cW[0] + " De: " + cW[1] + " Response: " + cW[2];
                                    }
                                }
                                else //Unknown
                                {
                                    string[] cW = _bytesString.Split(' ');
                                    if (cW.Length > 1)
                                    {
                                        _output = "Unknown: " + _bytesString;
                                    }
                                }
                                msg += "\r\n";
                                msg += "Code:" + cCode + " - GENERATED: " + _output;

                                Log("Code:" + cCode + " - GENERATED: " + _output);
                            }
                            //msg += BytetoString(_bytesString);
                        }
                        msg += "\r\n-CHECK---------------------------------------------------";
                        //Console.WriteLine(msg);

                        //Log(msg);
                    }
                    else
                    {
                        //Receive only 

                        writefile = true;
                        msg += _msg;
                        msg += _msgb;
                        msg += "\r\n--NONCHECK--------------------------------------------------";
                        //Console.WriteLine(msg);

                        //Log(msg);
                    }

                    //newsock.Send(data, data.Length, sender);
                }
                catch { }
                if (writefile)
                {
                    try
                    {


                        using (StreamWriter sw = File.AppendText("logout3.txt"))
                        {
                            sw.WriteLine(msg);
                        }

                    }
                    catch (Exception e)
                    {
                        //Console.WriteLine("Exception: " + e.Message);

                        Log(e.Message);
                    }
                    finally
                    {
                    }
                }
            }
        }
    }
}