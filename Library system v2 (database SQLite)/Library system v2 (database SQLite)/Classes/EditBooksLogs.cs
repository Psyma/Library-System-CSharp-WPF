using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_system_v2__database_SQLite_.Classes {
    public class EditBooksLogs {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Duration { get; set; }
        public float Price { get; set; }
        public int Stacks { get; set; }
        public string Status { get; set; }
        public string TimeEdited { get; set; }
    }
}
