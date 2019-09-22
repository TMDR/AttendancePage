using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttendancePage.Models
{
    public class workerPerMonth
    {
        public int year;
        public int month;
        public int duration;
        public workerPerMonth(int year,int month,int duration)
        {
            this.duration = duration;
            this.month = month;
            this.year = year;
        }
    }
}
