using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_system_v2__database_SQLite_.Classes {
    public class MyBooks {
        
        public string StudentName { get; set; }  
        public int BookID { get; set; }  
        public string Title { get; set; }
        public int Duration { get; set; }
        public float Price { get; set; }
        public string BorrowedDate { get; set; }
        public string ReturnDate { get; set; }
    }
}
