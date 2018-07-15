using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace model
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct m_permission
    {
        public byte ALL;
        public byte DATA;
        public byte VIEW;
        public byte PRINT;
        public byte DOWNLOAD;
        public byte ADD;
        public byte ACTIVE;
        public byte EDIT;
        public byte DELETE;
        public byte CONFIG;
        public byte PERMISSION;

        public static string render(m_permission p, string s) {
            if (p.ALL != 0)
                s = s.Replace("", "");
            else
                s = s.Replace("", "");
            return s;
        }

        public override string ToString()
        {
            return "ALL_" + ALL.ToString() + "DATA_" + DATA.ToString() + ";VIEW_" + VIEW.ToString() + ";PRINT_" + PRINT.ToString() + ";DOWNLOAD_" + DOWNLOAD.ToString() +
                ";ADD_" + ADD.ToString() + ";ACTIVE_" + ACTIVE.ToString() + ";EDIT_" + EDIT.ToString() +
                ";DELETE_" + DELETE.ToString() + ";CONFIG_" + CONFIG.ToString() + ";PERMISSION_" + PERMISSION.ToString() + ";";
        }
    }
}
