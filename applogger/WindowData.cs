using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace applogger
{
    class WindowData
    {
        public int runningTime;
        public bool stillOpen;
        public DateTime starttime;
        public string windowTitle;

        public WindowData(string windowTitle)
        {
            runningTime = 0;
            stillOpen = false;
            starttime = DateTime.Now;
            this.windowTitle = windowTitle;
        }
    }
}
