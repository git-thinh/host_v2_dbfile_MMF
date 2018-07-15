using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace model
{
    [Serializable] 
    public enum m_pipe_msg
    {
        NOTI_ADD = 1,
        STORE_ADD = 10,
        WARN_ADD = 20,
    }
}
