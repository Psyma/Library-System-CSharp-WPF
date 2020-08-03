using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_system_v2__database_SQLite_.Classes {
    public class BookList {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Duration { get; set; }
        public float Price { get; set; }
        public int Stacks { get; set; }
        public string Status { get; set; }
        public BookList(string book1, string book2) {
            book1 = Title;
            book2 = Title;
        }
        public BookList() { }
    }
}
