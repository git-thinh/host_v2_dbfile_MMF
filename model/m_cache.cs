using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace model
{

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct m_cache
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 255)]
        public string modkey;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
        public string theme_key;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
        public string device_type;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
        public string lang_key;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
        public string browser_name;


        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
        public string Type; // module, layout, page

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 255)]
        public string Key;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 255)]
        public string Name;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string MimeType;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 255)]
        public string PathFile;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
        public string Ext;



        public bool Cache { set; get; }

        public long Length { set; get; }
        public byte[] Content { set; get; }
    }
}
