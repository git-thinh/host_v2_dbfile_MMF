using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Text.RegularExpressions;
using System.Configuration;
using System.IO;
using System.Reflection;
using host.Forms;
using System.Net;
using model;
using host.pipe;

namespace host
{
    public class main
    {
        public static int processCurrent_PID;

        public static string Domain;

        public static string pathRoot = AppDomain.CurrentDomain.BaseDirectory;
        public static string pathModule;
        public static string pathDB;
        public static string pathSite;
        public static string pathSite_Ext = "";

        public static string page_Site;
        public static string page_Site_Main;
        public static string page_Login;
        public static string page_Socket;


        //private static List<IWebSocketConnection> allSockets = new List<IWebSocketConnection>() { };
        private static fMain fmain;
        public static m_sys_config config;

        #region /// notification ...

        public static Form globalForm;
        public static void show_notification(string msg, int duration_ = 0)
        {
            fNoti form = new fNoti(msg, duration_);
            globalForm.Invoke((MethodInvoker)delegate()
            {
                form.Show();
            });
        }

        #endregion

        #region /// fmain ...

        public static void reset()
        {
            //MessageBox.Show("Reset application ...");
            Application.Restart();
        }

        public static void hide_form_Main()
        {
            fmain.ShowIcon = false;
            fmain.ShowInTaskbar = false;
            fmain.Hide();
        }

        public static void show_form_Main()
        {
            fmain.ShowIcon = true;
            fmain.ShowInTaskbar = true;
            fmain.Show();
            fmain.Activate();
        }

        #endregion

        [STAThread]
        static void Main(string[] args)
        {
            Process currentProcess = Process.GetCurrentProcess();
            processCurrent_PID = currentProcess.Id;

            #region // notification ...

            globalForm = new Form();
            globalForm.Show();
            globalForm.Hide();

            #endregion

            if (File.Exists(pathRoot + "config.mmf"))
            {
                try
                {
                    config = hostFile.read_MMF<m_sys_config>(pathRoot + "config.mmf");
                }
                catch { }
            }

            if (string.IsNullOrEmpty(config.path_db))
            {
                Application.Run(new fSysConfig(true));
            }
            else
            {
                if (!Directory.Exists(main.config.path_module))
                {
                    main.show_notification("Can not find path module: " + main.config.path_module);
                    Application.Run(new fSysConfig(true));
                }
                else
                {
                    Domain = config.site_domain.ToLower();

                    pathDB = config.path_db.ToLower();
                    pathModule = config.path_module.ToLower();
                    pathSite = config.path_site.ToLower();
                    pathSite_Ext = config.site_Ext.ToLower();

                    page_Socket = "http://" + config.socket_ip + ":" + config.socket_port.ToString();
                    page_Login = "http://" + config.login_ip + ":" + config.login_port.ToString();
                    page_Site = "http://" + config.site_ip + ":" + config.site_port.ToString();
                    page_Site = "http://" + config.site_ip + ":" + config.site_port.ToString();
                    page_Site_Main = page_Site + "/" + config.site_path_main + "." + pathSite_Ext;

                    //------------------------------------------------------------------------------------------

                    //db_user.load();
                    //httpCache.init();

                    webServer.init();
                    pipeServer.init(processCurrent_PID);

                    show_notification("PID: " + processCurrent_PID.ToString());
                    //------------------------------------------------------------------------------------------

                    //------------------------------------------------------------------------------------------

                    fmain = new fMain();
                    Application.Run(fmain);
                }
            }//end if has config
        }


    }//end class
}
