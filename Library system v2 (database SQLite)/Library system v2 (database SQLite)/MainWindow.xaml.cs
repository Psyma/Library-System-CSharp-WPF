using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Library_system_v2__database_SQLite_.Classes;

namespace Library_system_v2__database_SQLite_ {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private AddStudentWindow addStudentWindow;
        private AddBookWindow addBookWindow;
        private StudentListWindow studentListWindow;
        private BookListWindow bookListWindow;
        private ViewRecordWindow viewRecordWindow;
        private ReturnBookWindow returnBookWindow;
        private NotificationWindow notificationWindow;

        public ViewReturnBookLogs viewReturnBookLogs;
        public ViewBooksLogs booksLogs;
        public ViewStudentsLogs studentsLogs;
        public ViewLoggingLogs viewLoggingLogs;
        public HistoryWindow historyWindow;
        public LoginWindow loginWindow;
        public BorrowBookWindow borrowBookWindow;
        public EnterPaymentWindow enterPaymentWindow;
        public EditStudentWindow editStudentWindow;
        public EditBookWindow editBookWindow;
        public Database database;

       
        public List<StudentList> studentLists;
        public List<BookList> bookLists;
        public List<MyBooks> myBooks;
        public List<Admin> admin;
        public List<LoggingLogs> loggingLogs;
        public List<AddingStudentLogs> addingStudentLogs;
        public List<EditStudentLogs> editStudentLogs;
        public List<AddingBookLogs> addingBookLogs;
        public List<EditBooksLogs> editBooksLogs;
        public List<ReturnBooksLogs> returnBooksLogs;

        public string LoginTime = string.Empty;
        public MainWindow() {
            InitializeComponent();
            enterPaymentWindow = new EnterPaymentWindow();
            editStudentWindow = new EditStudentWindow();
            addStudentWindow = new AddStudentWindow();
            addBookWindow = new AddBookWindow();
            editBookWindow = new EditBookWindow();
            studentListWindow = new StudentListWindow();
            bookListWindow = new BookListWindow();
            borrowBookWindow = new BorrowBookWindow();
            returnBookWindow = new ReturnBookWindow();
            viewRecordWindow = new ViewRecordWindow();
            viewLoggingLogs = new ViewLoggingLogs();
            booksLogs = new ViewBooksLogs();
            studentsLogs = new ViewStudentsLogs();
            viewReturnBookLogs = new ViewReturnBookLogs();

            returnBooksLogs = new List<ReturnBooksLogs>();
            addingStudentLogs = new List<AddingStudentLogs>();
            editStudentLogs = new List<EditStudentLogs>();
            historyWindow = new HistoryWindow();
            loginWindow = new LoginWindow();
            notificationWindow = new NotificationWindow();
            studentLists = new List<StudentList>();
            bookLists = new List<BookList>();
            myBooks = new List<MyBooks>();
            admin = new List<Admin>();
            database = new Database();
            loggingLogs = new List<LoggingLogs>();
            addingBookLogs = new List<AddingBookLogs>();
            editBooksLogs = new List<EditBooksLogs>();

            // Admin logging in
            database.Retrieve("Admin");
            if (admin.Count != 0) {
                loginWindow._signUpButton.Opacity = 0.0;
                loginWindow._signUpButton.IsEnabled = false;
            }
            loginWindow.ShowDialog();

            // check if there book due date to return
            List<MyBooks> tempMyBooks = new List<MyBooks>();
            database.Retrieve("MyBooks");
            var dateNow = DateTime.Now;
            foreach (var value in myBooks) {
                var returnDate = DateTime.Parse(value.ReturnDate);
                int date = dateNow.CompareTo(returnDate);
                if (date.Equals(1)) {
                    tempMyBooks.Add(value);
                }
            }
            if(tempMyBooks.Count != 0) {
                myBooks.Clear(); 
            }
            else {
                _notificationButton.Opacity = 0.0;
                _notificationButton.IsEnabled = false;
            }
        }
        
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            string query = "UPDATE LoggingLogs set AdminLogoutTime='" + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() +
                           "'where AdminLoginTime='" + LoginTime + "'";
            database.Update(query);
            Application.Current.Shutdown();
        }
        private void ClickButton(string button) {
            this.Hide();
            if (button.Equals("AddStudentButton")) {
                addStudentWindow.ShowDialog();
            }
            else if (button.Equals("EditStudentButton")) {
                database.Retrieve("StudentList");
                editStudentWindow._selectNameComboBox.ItemsSource = studentLists;
                editStudentWindow.ShowDialog();
                editStudentWindow._selectNameComboBox.ItemsSource = null;
            }
            else if(button.Equals("StudentListButton")) {
                database.Retrieve("StudentList");
                studentListWindow._studentListListView.ItemsSource = studentLists;
                studentListWindow.ShowDialog();
                studentListWindow._studentListListView.ItemsSource = null;
            }
            else if(button.Equals("AddBookButton")) {
                
                addBookWindow.ShowDialog();
            }
            else if(button.Equals("EditBookButton")) {
                database.Retrieve("BookList");
                editBookWindow._selectBookComboBox.ItemsSource = bookLists;
                editBookWindow.ShowDialog();
                editBookWindow._selectBookComboBox.ItemsSource = null;
            }
            else if(button.Equals("BookListButton")) {
                database.Retrieve("BookList");
                bookListWindow._bookListListView.ItemsSource = bookLists;
                bookListWindow.ShowDialog();
                bookListWindow._bookListListView.ItemsSource = null;
            }
            else if(button.Equals("BorrowBookButton")) {
                database.Retrieve("BookList");
                database.Retrieve("StudentList");
                borrowBookWindow._bookListListView.ItemsSource = bookLists;
                borrowBookWindow._selectNameComboBox.ItemsSource = studentLists;
                borrowBookWindow.ShowDialog();
                borrowBookWindow._selectNameComboBox.ItemsSource = null;
                borrowBookWindow._bookListListView.ItemsSource = null;
            }
            else if(button.Equals("ReturnBookButton")) {
                database.Retrieve("StudentList");
                returnBookWindow._selectNameComboBox.ItemsSource = studentLists;
                returnBookWindow.ShowDialog();
                returnBookWindow._selectNameComboBox.ItemsSource = null;
            }
            else if(button.Equals("ViewRecordButton")) {
                database.Retrieve("StudentList");
                viewRecordWindow._selectNameComboBox.ItemsSource = studentLists;
                viewRecordWindow.ShowDialog();
                viewRecordWindow._selectNameComboBox.ItemsSource = null;
            }
            else if (button.Equals("NotificationButton")) {
                database.Retrieve("MyBooks");
                notificationWindow._bookListListView.ItemsSource = myBooks;
                notificationWindow.ShowDialog();
                notificationWindow._bookListListView.ItemsSource = null;
            }
            else if(button.Equals("HistoryButton")) {
                loginWindow.ShowDialog();
            }
            studentLists.Clear();
            bookLists.Clear();
            this.Show();
        }
        private void AddStudentButton_Click(object sender, RoutedEventArgs e) {
            ClickButton("AddStudentButton");
        }

        private void EditStudentButton_Click(object sender, RoutedEventArgs e) {
            ClickButton("EditStudentButton");
        }
        private void StudentListButton_Click(object sender, RoutedEventArgs e) {
            ClickButton("StudentListButton");
        }
        private void AddBookButton_Click(object sender, RoutedEventArgs e) {
            ClickButton("AddBookButton");
        }

        private void EditBookButton_Click(object sender, RoutedEventArgs e) {
            ClickButton("EditBookButton");
        }
        private void BookListButton_Click(object sender, RoutedEventArgs e) {
            ClickButton("BookListButton");
        }
        private void ViewRecordButton_Click(object sender, RoutedEventArgs e) {
            ClickButton("ViewRecordButton");
        }

        private void ReturnBookButton_Click(object sender, RoutedEventArgs e) {
            ClickButton("ReturnBookButton");
        }

        private void BorrowBookButton_Click(object sender, RoutedEventArgs e) {
            ClickButton("BorrowBookButton");
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e) {
            
            this.Close();
        }

        private void DeletedItemsButton_Click(object sender, RoutedEventArgs e) {

        }

        private void NotificationButton_Click(object sender, RoutedEventArgs e) {
            ClickButton("NotificationButton");
        }

        private void HistoryButton_Click(object sender, RoutedEventArgs e) {
            ClickButton("HistoryButton");
        }
    }
}
