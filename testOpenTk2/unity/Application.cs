using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityEngine
{
    class Application
    {
        static GoManager m_goMgr = null;

        public static void _Init()
        {
            m_goMgr = new GoManager();
        }

        public static GoManager _GetGoManager()
        {
            return m_goMgr;
        }
    }
}

