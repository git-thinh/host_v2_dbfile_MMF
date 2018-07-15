using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Linq.Dynamic; 
using model;

namespace host
{
    public static class hostContext
    {
        public static string pathRoot = AppDomain.CurrentDomain.BaseDirectory;

        public static string lang_Default = "vi";
        public static string theme_Default = "dark";
        public static string device_Default = "pc";
        public static int browser_width_Default = 1366;




















        private readonly static object lock_lsUser = new object();
        private readonly static object lock_token = new object();
        private readonly static object lock_R = new object();

        //private static List<m_user> lsUser = new List<m_user>() { };
        private static DictionaryList<string, string> dicUserToken = new DictionaryList<string, string>() { };
        private static Dictionary<string, string> dicToken_R = new Dictionary<string, string>() { };
        private static Dictionary<string, string> dicToken_LangUser = new Dictionary<string, string>() { };

        private static Dictionary<string, string> dicSession_refUri = new Dictionary<string, string>() { };
        private static Dictionary<string, string> dicSession_DeviceType = new Dictionary<string, string>() { };
        private static Dictionary<string, string> dicSession_ThemeKey = new Dictionary<string, string>() { };
        private static Dictionary<string, int> dicSession_BrowserWidth = new Dictionary<string, int>() { };

        public static void init()
        {  
            //lsUser.AddRange(new m_user[] {
            //      new m_user() { user_id = Guid.NewGuid().ToString(), username = "ifc", pass  = "123", status = true }
            //    , new m_user() { user_id = Guid.NewGuid().ToString(), username = "admin", pass  = "admin", status = true }
            //    , new m_user() { user_id = Guid.NewGuid().ToString(), username = "nga", pass  = "nga", status =true }
            //    , new m_user() { user_id = Guid.NewGuid().ToString(), username = "tuan", pass  = "tuan", status = true }
            //    , new m_user() { user_id = Guid.NewGuid().ToString(), username = "thanh", pass  = "thanh", status = true }
            //    , new m_user() { user_id = Guid.NewGuid().ToString(), username = "thinh", pass  = "thinh", status = true }
            //    , new m_user() { user_id = Guid.NewGuid().ToString(), username = "duy", pass  = "duy", status = true }
            //});
        }

        #region >> Session >> theme_key : dark ; light ...

        public static string session_themeKey_Get(string session_id)
        {
            string theme_key = "";
            dicSession_ThemeKey.TryGetValue(session_id, out theme_key);
            if (string.IsNullOrEmpty(theme_key)) theme_key = theme_Default;
            return theme_key;
        }

        public static void session_themeKey_Set(string session_id, string theme_key)
        {
            if (dicSession_ThemeKey.ContainsKey(session_id))
                dicSession_ThemeKey[session_id] = theme_key;
            else
                dicSession_ThemeKey.Add(session_id, theme_key);
        }

        #endregion

        #region >> Session >> browser width : dark ; light ...

        public static int session_BrowserWidth_Get(string session_id)
        {
            int browser_width = 0;
            dicSession_BrowserWidth.TryGetValue(session_id, out browser_width);
            if (browser_width == 0) browser_width = browser_width_Default;
            return browser_width;
        }

        public static void session_BrowserWidth_Set(string session_id, int browser_width)
        {
            if (dicSession_BrowserWidth.ContainsKey(session_id))
                dicSession_BrowserWidth[session_id] = browser_width;
            else
                dicSession_BrowserWidth.Add(session_id, browser_width);
        }

        #endregion

        #region >> Session >> device type: pc, tablet, mobi, mobimi ...

        public static string session_deviceType_Get(string session_id)
        {
            string device_type = "";
            dicSession_DeviceType.TryGetValue(session_id, out device_type);
            if (string.IsNullOrEmpty(device_type)) device_type = "pc";
            return device_type;
        }

        public static void session_deviceType_Set(string session_id, string device_type)
        {
            if (dicSession_DeviceType.ContainsKey(session_id))
                dicSession_DeviceType[session_id] = device_type;
            else
                dicSession_DeviceType.Add(session_id, device_type);
        }

        #endregion

        #region >> Session >> Uri closest login, logout ...

        /// <summary>
        /// Get uri closest session logout or login 
        /// </summary>
        /// <param name="session_id"></param>
        /// <returns></returns>
        public static string user_refUri_Closest_Get(string session_id)
        {
            string uri = "";
            dicSession_refUri.TryGetValue(session_id, out uri);
            return uri;
        }

        public static void user_refUri_Closest_Set(string session_id, string uri)
        {
            if (dicSession_refUri.ContainsKey(session_id))
                dicSession_refUri[session_id] = uri;
            else
                dicSession_refUri.Add(session_id, uri);
        }

        #endregion

        #region >> Token - Language choose ...

        public static void langSet(string token_id, string lang_key)
        {
            if (dicToken_LangUser.ContainsKey(token_id))
                dicToken_LangUser[token_id] = lang_key;
            else
                dicToken_LangUser.Add(token_id, lang_key);
        }

        public static string langGet(string token_id)
        {
            string lang = "";

            dicToken_LangUser.TryGetValue(token_id, out lang);
            if (string.IsNullOrEmpty(lang)) lang = lang_Default;

            return lang;
        }

        #endregion


        #region >> Token ...

        public static void tokenRemove(string user, string token)
        {
            if (dicUserToken.ContainsKey(user))
                dicUserToken.Remove(user, token);
        }

        public static void tokenAdd(string user, string token, DateTime dateExpiry)
        {
            if (dicUserToken.ContainsKey(user))
            {
                var ls = dicUserToken[user];

                if (ls.IndexOf(token) == -1)
                {
                    lock (lock_token)
                    {
                        dicUserToken.Add(user, token);
                    }
                }
            }
            else
            {
                lock (lock_token)
                {
                    dicUserToken.Add(user, token);
                }
            }
        }

        #endregion

        #region >> User ...

        //public static string Add(m_user u)
        //{
        //    string id = "";
        //    var ls = lsUser.Where(x => x.username == u.username).ToList();
        //    if (ls.Count == 0)
        //    {
        //        id = Guid.NewGuid().ToString();
        //        u.id = id;
        //        u.status = false;
        //        lock (lock_lsUser)
        //        {
        //            lsUser.Add(u);
        //        }
        //    }
        //    return id;
        //}

        //public static IEnumerable<m_user> getUser()
        //{
        //    return lsUser;
        //}

        //public static m_user login(string username, string password)
        //{
        //    m_user u = new m_user();
        //    var dt = lsUser.Where(x => x.username == username && x.pass == password && x.status == true).ToList();

        //    if (dt.Count > 0)
        //    {
        //        u = dt[0];
        //    }

        //    return u;
        //}

        //public static m_user login(m_user o)
        //{
        //    m_user u = new m_user();
        //    if (o.username == null || o.pass == null) return u;
        //    u = login(o.username, o.pass);
        //    return u;
        //}

        #endregion
    }
}
