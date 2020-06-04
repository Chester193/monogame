using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG2D_2020_Dzienni_FD_Projekt
{
    public class Timer
    {
        public int Time { get; set; }

        public Timer()
        {
            Time = 0;
        }

        public bool Count()
        {
            if(Time > 0)
            {
                Time--;
                return true;
            }
            return false;
        }
    }
}
