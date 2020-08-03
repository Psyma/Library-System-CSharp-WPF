using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_system_v2__database_SQLite_.Classes {
    public class StudentList {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string BirthDate { get; set; }
        public string CourseYrLv { get; set; }
        public int TotalBooks { get; set; }
        public float AmountPaid { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public StudentList(string name1, string name2) {
            name1 = Name;
            name2 = Name;
        }
        public StudentList() { }
    }
}
