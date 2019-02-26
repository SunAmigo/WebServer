using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWebApp.Services
{
    class TimeService
    {
        public String GetTime()
        {
            return DateTime.Now.ToString("hh:mm:ss");
        }
    }
}
