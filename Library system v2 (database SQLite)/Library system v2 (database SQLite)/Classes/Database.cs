using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.IO;
using System.Windows;

namespace Library_system_v2__database_SQLite_.Classes {
    public class Database{
        private SQLiteConnection myConnection;
        private SQLiteCommand myCommand;
        private SQLiteDataReader reader;
        public Database() {
            myConnection = new SQLiteConnection("Data Source=LibraryDatabase.sqlite3");
            // if not exists create the SQLite file
            if (!File.Exists("./LibraryDatabase.sqlite3")) {
                SQLiteConnection.CreateFile("LibraryDatabase.sqlite3");
            }
        }
        public void Insert(string query, string[] items, string[] location, string tableName) {
            // insert data to database
            OpenConnection();
            myCommand = new SQLiteCommand(query, myConnection);
            for (int i = 0; i < items.Length; i++) {
                myCommand.Parameters.AddWithValue(location[i], items[i]);
            }
            myCommand.ExecuteNonQuery();
            myCommand.Dispose();
            CloseConnection();
        }
        public void Retrieve(string tableName) {
            // retrieve all data from the specific table
            MainWindow window = Application.Current.Windows[0] as MainWindow;
            OpenConnection();
            if (tableName.Equals("StudentList")) {
                string query = "SELECT * FROM StudentList";
                myCommand = new SQLiteCommand(query, myConnection);
                reader = myCommand.ExecuteReader();
                while (reader.Read()) {
                    window.studentLists.Add(new StudentList { 
                        ID = Convert.ToInt32(reader["ID"]),
                        Name = reader["Name"].ToString(),
                        Gender = reader["Gender"].ToString(),
                        BirthDate = reader["BirthDate"].ToString(),
                        CourseYrLv = reader["CourseYrLv"].ToString(),
                        TotalBooks = Convert.ToInt32(reader["TotalBooks"]),
                        AmountPaid = float.Parse(reader["AmountPaid"].ToString()),
                        Address = reader["Address"].ToString(),
                        PhoneNumber = reader["PhoneNumber"].ToString()
                    });
                }
                myCommand.Dispose();
                reader.Close();
            }
            else if(tableName.Equals("BookList")) {
                string query = "SELECT * FROM BookList";
                myCommand = new SQLiteCommand(query, myConnection);
                reader = myCommand.ExecuteReader();
                while (reader.Read()) {
                    window.bookLists.Add(new BookList {
                        ID = Convert.ToInt32(reader["ID"]),
                        Title = reader["Title"].ToString(),
                        Author = reader["Author"].ToString(),
                        Duration = Convert.ToInt32(reader["Duration"]),
                        Price = float.Parse(reader["Price"].ToString()),
                        Stacks = Convert.ToInt32(reader["Stacks"]),
                        Status = reader["Status"].ToString()
                    });
                }
                myCommand.Dispose();
                reader.Close();
            }
            else if (tableName.Equals("MyBooks")) {
                string query = "SELECT * FROM MyBooks";
                myCommand = new SQLiteCommand(query, myConnection);
                reader = myCommand.ExecuteReader();
                while (reader.Read()) {
                    window.myBooks.Add(new MyBooks {
                        BookID = Convert.ToInt32(reader["BookID"]),
                        StudentName = reader["StudentName"].ToString(),
                        Title = reader["BookTitle"].ToString(),
                        Duration = Convert.ToInt32(reader["BookDuration"]),
                        Price = float.Parse(reader["BookPrice"].ToString()),
                        BorrowedDate = reader["BorrowedDate"].ToString(),
                        ReturnDate = reader["ReturnDate"].ToString()
                    });
                }
                myCommand.Dispose();
                reader.Close();
            }
            else if(tableName.Equals("Admin")) {
                string query = "SELECT * FROM Admin";
                myCommand = new SQLiteCommand(query, myConnection);
                reader = myCommand.ExecuteReader();
                while (reader.Read()) {
                    window.admin.Add(new Admin {
                        HistoryPassword = reader["HistoryPassword"].ToString(),
                        HistoryUsername = reader["HistoryUsername"].ToString(),
                        LoginPassword = reader["LoginPassword"].ToString(),
                        LoginUsername = reader["LoginUsername"].ToString()
                    });
                }
                myCommand.Dispose();
                reader.Close();
            }
            else if(tableName.Equals("LoggingLogs")) {
                string query = "SELECT * FROM LoggingLogs";
                myCommand = new SQLiteCommand(query, myConnection);
                reader = myCommand.ExecuteReader();
                while (reader.Read()) {
                    window.loggingLogs.Add(new LoggingLogs {
                        AdminLoginTime = reader["AdminLoginTime"].ToString(),
                        AdminLogoutTime = reader["AdminLogoutTime"].ToString(),
                        HistoryLoginTime = reader["HistoryLoginTime"].ToString(),
                        HistoryLogoutTime = reader["HistoryLogoutTime"].ToString()
                    });
                }
                myCommand.Dispose();
                reader.Close();
            }
            else if (tableName.Equals("AddingStudentLogs")) {
                string query = "SELECT * FROM AddingStudentLogs";
                myCommand = new SQLiteCommand(query, myConnection);
                reader = myCommand.ExecuteReader();
                while (reader.Read()) {
                    window.addingStudentLogs.Add(new AddingStudentLogs {
                        ID = Convert.ToInt32(reader["ID"]),
                        Name = reader["Name"].ToString(),
                        Gender = reader["Gender"].ToString(),
                        BirthDate = reader["BirthDate"].ToString(),
                        CourseYrLv = reader["CourseYrLv"].ToString(),
                        TotalBooks = Convert.ToInt32(reader["TotalBooks"]),
                        AmountPaid = float.Parse(reader["AmountPaid"].ToString()),
                        Address = reader["Address"].ToString(),
                        PhoneNumber = reader["PhoneNumber"].ToString(),
                        TimeAdded = reader["TimeAdded"].ToString()
                    });
                }
                myCommand.Dispose();
                reader.Close();
            }
            else if (tableName.Equals("EditStudentLogs")) {
                string query = "SELECT * FROM EditStudentLogs";
                myCommand = new SQLiteCommand(query, myConnection);
                reader = myCommand.ExecuteReader();
                while (reader.Read()) {
                    window.editStudentLogs.Add(new EditStudentLogs {
                        ID = Convert.ToInt32(reader["ID"]),
                        Name = reader["Name"].ToString(),
                        Gender = reader["Gender"].ToString(),
                        BirthDate = reader["BirthDate"].ToString(),
                        CourseYrLv = reader["CourseYrLv"].ToString(),
                        TotalBooks = Convert.ToInt32(reader["TotalBooks"]),
                        AmountPaid = float.Parse(reader["AmountPaid"].ToString()),
                        Address = reader["Address"].ToString(),
                        PhoneNumber = reader["PhoneNumber"].ToString(),
                        TimeEdited = reader["TimeEdited"].ToString()
                    });
                }
                myCommand.Dispose();
                reader.Close();
            }
            else if (tableName.Equals("AddingBookLogs")) {
                string query = "SELECT * FROM AddingBookLogs";
                myCommand = new SQLiteCommand(query, myConnection);
                reader = myCommand.ExecuteReader();
                while (reader.Read()) {
                    window.addingBookLogs.Add(new AddingBookLogs {
                        ID = Convert.ToInt32(reader["ID"]),
                        Title = reader["Title"].ToString(),
                        Author = reader["Author"].ToString(),
                        Duration = Convert.ToInt32(reader["Duration"]),
                        Price = float.Parse(reader["Price"].ToString()),
                        Stacks = Convert.ToInt32(reader["Stacks"]),
                        Status = reader["Status"].ToString(),
                        TimeAdded = reader["TimeAdded"].ToString()
                    });
                }
                myCommand.Dispose();
                reader.Close();
            }
            else if (tableName.Equals("EditBookLogs")) {
                string query = "SELECT * FROM EditBookLogs";
                myCommand = new SQLiteCommand(query, myConnection);
                reader = myCommand.ExecuteReader();
                while (reader.Read()) {
                    window.editBooksLogs.Add(new EditBooksLogs {
                        ID = Convert.ToInt32(reader["ID"]),
                        Title = reader["Title"].ToString(),
                        Author = reader["Author"].ToString(),
                        Duration = Convert.ToInt32(reader["Duration"]),
                        Price = float.Parse(reader["Price"].ToString()),
                        Stacks = Convert.ToInt32(reader["Stacks"]),
                        Status = reader["Status"].ToString(),
                        TimeEdited = reader["TimeEdited"].ToString()
                    });
                }
                myCommand.Dispose();
                reader.Close();
            }
            else if (tableName.Equals("ReturnBookLogs")) {
                string query = "SELECT * FROM ReturnBookLogs";
                myCommand = new SQLiteCommand(query, myConnection);
                reader = myCommand.ExecuteReader();
                while (reader.Read()) {
                    window.returnBooksLogs.Add(new ReturnBooksLogs {
                        StudentName = reader["StudentName"].ToString(),
                        BookID = Convert.ToInt32(reader["BookID"]),
                        BookTitle = reader["BookTitle"].ToString(),
                        Duration = Convert.ToInt32(reader["Duration"]),
                        BookPrice = float.Parse(reader["BookPrice"].ToString()),
                        BorrowedDate = reader["BorrowedDate"].ToString(),
                        ReturnDate = reader["ReturnDate"].ToString(),
                        ReturnTime = reader["ReturnTime"].ToString()
                    });
                }
                myCommand.Dispose();
                reader.Close();
            }
            window.bookLists.Sort(delegate (BookList x, BookList y) { return x.Title.CompareTo(y.Title); });
            window.studentLists.Sort(delegate (StudentList x, StudentList y) { return x.Name.CompareTo(y.Name); });
            CloseConnection();
        }
        public void RetrieveAt(string tableName, int studentID) {
            // get data from MyBooks table in a specific studentID
            MainWindow window = Application.Current.Windows[0] as MainWindow;
            OpenConnection();
            if (tableName.Equals("MyBooks")) {
                string query = "SELECT * FROM MyBooks";
                myCommand = new SQLiteCommand(query, myConnection);
                reader = myCommand.ExecuteReader();
                while (reader.Read()) {
                    if(reader["studentID"].ToString() == studentID.ToString()) {
                        window.myBooks.Add(new MyBooks {
                            BookID = Convert.ToInt32(reader["BookID"]),
                            StudentName = reader["StudentName"].ToString(),
                            Title = reader["BookTitle"].ToString(),
                            Duration = Convert.ToInt32(reader["BookDuration"]),
                            Price = float.Parse(reader["BookPrice"].ToString()),
                            BorrowedDate = reader["BorrowedDate"].ToString(),
                            ReturnDate = reader["ReturnDate"].ToString()
                        });
                    }
                }
                myCommand.Dispose();
                reader.Close();
            }
            CloseConnection();
        }
        public int GetEid(string tableName, int ID) {
            // retrieve the eid value  
            OpenConnection();
            string query = "SELECT * FROM " + tableName;
            myCommand = new SQLiteCommand(query, myConnection);
            reader = myCommand.ExecuteReader();
            while (reader.Read()) {
                if (Convert.ToInt32(reader["ID"]).Equals(ID)) {
                    int eid = Convert.ToInt32(reader["eid"]);
                    reader.Close();
                    myCommand.Dispose();
                    CloseConnection();
                    return eid;
                }
            }
            CloseConnection();
            return 0;
        }
        public int GetEid(string tableName, int studentID, int bookID) {
            // retrieve the eid value  
            OpenConnection();
            string query = "SELECT * FROM " + tableName;
            myCommand = new SQLiteCommand(query, myConnection);
            reader = myCommand.ExecuteReader();
            while (reader.Read()) {
                if (Convert.ToInt32(reader["BookID"]).Equals(bookID) && Convert.ToInt32(reader["StudentID"]).Equals(studentID)) {
                    int eid = Convert.ToInt32(reader["eid"]);
                    reader.Close();
                    myCommand.Dispose();
                    CloseConnection();
                    return eid;
                }
            }
            CloseConnection();
            return 0;
        }
        public void DeleteAt(string tableName, int ID) {
            // delete all data in row from specific table
            int eid = GetEid(tableName, ID);
            OpenConnection();
            string query = "DELETE from " + tableName + " WHERE eid='" + eid.ToString() + "' ;";
            myCommand = new SQLiteCommand(query, myConnection);
            reader = myCommand.ExecuteReader();
            while (reader.Read()) {
                reader = myCommand.ExecuteReader();
            }
            reader.Close();
            myCommand.Dispose();
            CloseConnection();
        }
        public void DeleteSpecific(string tableName, int studentID, int bookID) {
            // delete all data in row from specific table
            int eid = GetEid(tableName, studentID, bookID);
            OpenConnection();
            string query = "DELETE from " + tableName + " WHERE eid='" + eid.ToString() + "' ;";
            myCommand = new SQLiteCommand(query, myConnection);
            reader = myCommand.ExecuteReader();
            while (reader.Read()) {
                reader = myCommand.ExecuteReader();
            }
            reader.Close();
            myCommand.Dispose();
            CloseConnection();
        }
        public void DeleteAll(string tableName) {
            // delete all data in a table
            OpenConnection();
            string query = "DELETE from " + tableName;
            myCommand = new SQLiteCommand(query, myConnection);
            reader = myCommand.ExecuteReader();
            while (reader.Read()) {
                reader = myCommand.ExecuteReader();
            }
            reader.Close();
            myCommand.Dispose();
            CloseConnection();
        }
        public void Update(string query) { 
            // update/edit data from the specific table
            OpenConnection();
            myCommand = new SQLiteCommand(query, myConnection);
            myCommand.ExecuteNonQuery();
            myCommand.Dispose();
            CloseConnection();
        }
        private void OpenConnection() {
            // open connection to SQLite
            if(!myConnection.State.Equals(ConnectionState.Open)) {
                myConnection.Open();
            }
        }
        private void CloseConnection() {
            // close connection to SQLite
            if(!myConnection.State.Equals(ConnectionState.Closed)) {
                myConnection.Close();
            }
        }
        public bool IfTableExists(string tableName) {
            // check if the table exists in SQLite database
            OpenConnection();
            SQLiteCommand command = new SQLiteCommand(myConnection);
            command.CommandText = "SELECT name FROM sqlite_master WHERE type = 'table' AND name ='" + tableName+ "'" +";";
            SQLiteDataReader reader = command.ExecuteReader();
            if (reader.HasRows) {
                reader.Close();
                reader.Dispose();
                command.Dispose();
                CloseConnection();
                return true;
            }
            CloseConnection();
            return false;
        }   
    }
}
