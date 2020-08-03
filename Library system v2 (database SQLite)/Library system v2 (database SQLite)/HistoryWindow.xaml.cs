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
using System.Windows.Shapes;

namespace Library_system_v2__database_SQLite_ {
    /// <summary>
    /// Interaction logic for HistoryWindow.xaml
    /// </summary>
    public partial class HistoryWindow : Window {
        public HistoryWindow() {
            InitializeComponent();
        }
        private MainWindow window = Application.Current.Windows[0] as MainWindow;
        private void ClickButton(string button) {
            this.Hide();
            if (button.Equals("ViewLoggingLogs")) {
                window.database.Retrieve("LoggingLogs");
                window.viewLoggingLogs._loggingLogsListView.ItemsSource = window.loggingLogs;
                window.viewLoggingLogs.ShowDialog();
                window.loggingLogs.Clear();
                window.viewLoggingLogs._loggingLogsListView.ItemsSource = null;
            }
            else if (button.Equals("ViewStudentLogs")) {
                window.database.Retrieve("AddingStudentLogs");
                window.database.Retrieve("EditStudentLogs");
                window.studentsLogs._studentAddedLogsListView.ItemsSource = window.addingStudentLogs;
                window.studentsLogs._studentEditLogsListView.ItemsSource = window.editStudentLogs;
                window.studentsLogs.ShowDialog();
                window.addingStudentLogs.Clear();
                window.editStudentLogs.Clear();
                window.studentsLogs._studentAddedLogsListView.ItemsSource = null;
                window.studentsLogs._studentEditLogsListView.ItemsSource = null;
            }
            else if (button.Equals("ViewBooksLogs")) {
                window.database.Retrieve("AddingBookLogs");
                window.database.Retrieve("EditBookLogs");
                window.booksLogs._bookAddedLogsListView.ItemsSource = window.addingBookLogs;
                window.booksLogs._bookEditedLogsListView.ItemsSource = window.editBooksLogs;
                window.booksLogs.ShowDialog();
                window.addingBookLogs.Clear();
                window.editBooksLogs.Clear();
                window.booksLogs._bookAddedLogsListView.ItemsSource = null;
                window.booksLogs._bookEditedLogsListView.ItemsSource = null;
            }
            else if (button.Equals("ReturnBooksLogs")) {
                window.database.Retrieve("ReturnBookLogs");
                window.viewReturnBookLogs._returnBookLogsListView.ItemsSource = window.returnBooksLogs;
                window.viewReturnBookLogs.ShowDialog();
                window.returnBooksLogs.Clear();
                window.viewReturnBookLogs._returnBookLogsListView.ItemsSource = null ;
            }
            this.ShowDialog();
        }
        private void HistoryWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
           
        }

        private void ViewLoggingLogsButton_Click(object sender, RoutedEventArgs e) {
            ClickButton("ViewLoggingLogs");
        }

        private void ViewStudentLogsButton_Click(object sender, RoutedEventArgs e) {
            ClickButton("ViewStudentLogs");
        }

        private void ViewBooksLogsButton_Click(object sender, RoutedEventArgs e) {
            ClickButton("ViewBooksLogs");
        }

        private void ReturnBooksLogsButton_Click(object sender, RoutedEventArgs e) {
            ClickButton("ReturnBooksLogs");
        }

        private void BackButton_Click(object sender, RoutedEventArgs e) {
            string query = "UPDATE LoggingLogs set HistoryLogoutTime='" + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() +
                          "'where AdminLoginTime='" + window.LoginTime + "'";
            window.database.Update(query);
            this.Close();
        }
    }
}
