using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Library_system_v2__database_SQLite_.Classes;

namespace Library_system_v2__database_SQLite_ {
    /// <summary>
    /// Interaction logic for AddBookWindow.xaml
    /// </summary>
    public partial class AddBookWindow : Window {
        public AddBookWindow() {
            InitializeComponent();
        }
        private bool CheckIfAllDataIsFilledUp() {
            if (string.IsNullOrWhiteSpace(_bookID.Text)) {
                return true;
            }
            else if (string.IsNullOrWhiteSpace(_bookTitle.Text)) {
                return true;
            }
            else if (string.IsNullOrWhiteSpace(_bookAuthor.Text)) {
                return true;
            }
            else if (string.IsNullOrWhiteSpace(_bookDuration.Text)) {
                return true;
            }
            else if (string.IsNullOrWhiteSpace(_bookPrice.Text)) {
                return true;
            }
            else if (string.IsNullOrWhiteSpace(_bookStacks.Text)) {
                return true;
            }
            return false;
        }
        private void AddBookWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
            _bookID.Text = string.Empty;
            _bookTitle.Text = string.Empty;
            _bookAuthor.Text = string.Empty;
            _bookPrice.Text = string.Empty;
            _bookDuration.Text = string.Empty;
            _bookStacks.Text = string.Empty;
        }

        private void AddBookButton_Click(object sender, RoutedEventArgs e) {
            // check if all info is filled up
            if (CheckIfAllDataIsFilledUp()) {
                MessageBox.Show("Missing information fill up all the data", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // check if ID is already added
            MainWindow window = Application.Current.Windows[0] as MainWindow;
            window.database.Retrieve("BookList");
            foreach (var value in window.bookLists) {
                if (value.ID.ToString() == _bookID.Text) {
                    MessageBox.Show("This ID is already added, get a unique ID", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    window.bookLists.Clear();    
                    return;
                }
            }
            try {
                // insert data to BookList table
                string query = "INSERT INTO BookList" +
                               "('ID', 'Title', 'Author', 'Duration', 'Price', 'Stacks', 'Status')" +
                               "VALUES" +
                               "(@ID, @Title, @Author, @Duration, @Price, @Stacks, @Status)";
                string[] items = new string[] {
                    _bookID.Text, _bookTitle.Text, _bookAuthor.Text, _bookDuration.Text, _bookPrice.Text,
                    _bookStacks.Text, "AVAILABLE"
                };
                string[] location = new string[] {
                    "@ID", "@Title", "@Author", "@Duration", "@Price", "@Stacks", "@Status"
                };
                window.database.Insert(query, items, location, "BookList");

                // insert data to AddingBookLogs table
                query = "INSERT INTO AddingBookLogs" +
                               "('ID', 'Title', 'Author', 'Duration', 'Price', 'Stacks', 'Status', 'TimeAdded')" +
                               "VALUES" +
                               "(@ID, @Title, @Author, @Duration, @Price, @Stacks, @Status, @TimeAdded)";
                items = new string[] {
                    _bookID.Text, _bookTitle.Text, _bookAuthor.Text, _bookDuration.Text, _bookPrice.Text,
                    _bookStacks.Text, "AVAILABLE", DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString()
                };
                location = new string[] {
                    "@ID", "@Title", "@Author", "@Duration", "@Price", "@Stacks", "@Status", "TimeAdded"
                };
                window.database.Insert(query, items, location, "AddingBookLogs");
                MessageBox.Show("Book Added, Thank you!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                _bookID.Text = string.Empty;
                _bookTitle.Text = string.Empty;
                _bookAuthor.Text = string.Empty;
                _bookPrice.Text = string.Empty;
                _bookDuration.Text = string.Empty;
                _bookStacks.Text = string.Empty;
            } catch(Exception) {  }
        }

        private void BackToMainButton_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void BookIDTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            Regex regex = new Regex("[^0-9+]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void BookTItleTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e) {

        }

        private void BookAuthorTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            Regex regex = new Regex("[^aA-zZ+]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void BookPriceTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            Regex regex = new Regex("[^0.-9+]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void BookDurationTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            Regex regex = new Regex("[^0-9+]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void BookStacksTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            Regex regex = new Regex("[^0-9+]");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
