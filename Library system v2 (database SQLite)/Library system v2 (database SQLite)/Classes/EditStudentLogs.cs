using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_system_v2__database_SQLite_.Classes {
    public class EditStudentLogs {
        // logs for students 
        public int ID { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string BirthDate { get; set; }
        public string CourseYrLv { get; set; }
        public int TotalBooks { get; set; }
        public float AmountPaid { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string TimeEdited { get; set; }
    }
}
