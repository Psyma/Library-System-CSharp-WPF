using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_system_v2__database_SQLite_.Classes {
    public class LoggingLogs {
        // logs for logging in && out
        public string AdminLoginTime { get; set; }
        public string AdminLogoutTime { get; set; }
        public string HistoryLoginTime { get; set; }
        public string HistoryLogoutTime { get; set; }
    }
}
