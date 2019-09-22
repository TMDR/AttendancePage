using System;
using System.Collections.Generic;

namespace AttendancePage.Models
{
    public partial class DateTime11
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public DateTime? Date { get; set; }
        public TimeSpan? TimeIn { get; set; }
        public TimeSpan? TimeOut { get; set; }
    }
}
