using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace host.Forms
{
    public partial class fMain : Form
    {
        #region // Variable ...

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public static string reduce_select_id = "";

        //-------------------------------------------------------------------
         

        #endregion

        #region // Main load, close, system tray ...

        private System.Windows.Forms.NotifyIcon notifyIcon1;

        public fMain()
        {

            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fMain));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);

            InitializeComponent();
            
            this.notifyIcon1.Icon = host.Resources.icon_tray;
            this.notifyIcon1.Text = "IFC Amiss v1.0";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseClick);
            
            this.Icon = Resources.icon_tray;
            this.Text = main.Domain;
            label_Domain.Text = main.Domain;
        }

        private void fMain_Load(object sender, EventArgs e)
        {
            label_URI_SITE.Text = main.page_Site;
            this.Hide();
        }

        private void panelBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }


        private void buttonClose_Click(object sender, EventArgs e)
        {
            main.hide_form_Main();
        }

        private void notifyIcon1_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            ////hostServer.openBrowser();
            //hostModule.CacheRefresh();
            //main.show_notification("Cache OK", 5000);
            main.show_form_Main();
        }

        private void button_Config_Click(object sender, EventArgs e)
        {
            fSysConfig f = new fSysConfig();
            f.ShowDialog();
        }

        #endregion

        #region // LABEL URI SITE ...

        private void label_URI_SITE_MouseHover(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.Hand;
        }

        private void label_URI_SITE_MouseLeave(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.Default;
        }
        
        private void label_URI_SITE_Click(object sender, EventArgs e)
        {
            string browser = GetDefaultBrowserPath();

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = browser;
            startInfo.Arguments = label_URI_SITE.Text;
            Process.Start(startInfo);
        }

        private static string GetDefaultBrowserPath()
        {
            string urlAssociation = @"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http";
            string browserPathKey = @"$BROWSER$\shell\open\command";

            RegistryKey userChoiceKey = null;

            try
            {
                //Read default browser path from userChoiceLKey
                userChoiceKey = Registry.CurrentUser.OpenSubKey(urlAssociation + @"\UserChoice", false);

                //If user choice was not found, try machine default
                if (userChoiceKey == null)
                {
                    //Read default browser path from Win XP registry key
                    var browserKey = Registry.ClassesRoot.OpenSubKey(@"HTTP\shell\open\command", false);

                    //If browser path wasn’t found, try Win Vista (and newer) registry key
                    if (browserKey == null)
                    {
                        browserKey =
                        Registry.CurrentUser.OpenSubKey(
                        urlAssociation, false);
                    }
                    //string s_bro = browserKey.GetValue(null) as string;
                    //var path = CleanifyBrowserPath(s_bro);

                    //Remove quotation marks
                    string browserPath1 = (browserKey.GetValue(null) as string).ToLower().Replace("\"", "");

                    //Cut off optional parameters
                    if (!browserPath1.EndsWith("exe"))
                    {
                        browserPath1 = browserPath1.Substring(0, browserPath1.LastIndexOf(".exe") + 4);
                    }

                    browserKey.Close();
                    return browserPath1;
                }
                else
                {
                    // user defined browser choice was found
                    string progId = (userChoiceKey.GetValue("ProgId").ToString());
                    userChoiceKey.Close();

                    // now look up the path of the executable
                    string concreteBrowserKey = browserPathKey.Replace("$BROWSER$", progId);
                    var kp = Registry.ClassesRoot.OpenSubKey(concreteBrowserKey, false);


                    //Remove quotation marks
                    string browserPath2 = (kp.GetValue(null) as string).ToLower().Replace("\"", "");

                    //Cut off optional parameters
                    if (!browserPath2.EndsWith("exe"))
                    {
                        browserPath2 = browserPath2.Substring(0, browserPath2.LastIndexOf(".exe") + 4);
                    }

                    kp.Close();
                    return browserPath2;
                }
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        #endregion

    }
}
