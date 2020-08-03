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
    /// Interaction logic for EditBookWindow.xaml
    /// </summary>
    public partial class EditBookWindow : Window {
        public EditBookWindow() {
            InitializeComponent();

        }
        private MainWindow window = Application.Current.Windows[0] as MainWindow;
        private void EditBookWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
            window.bookLists.Clear();
            _selectBookComboBox.SelectedIndex = -1;
            _bookID.Text = string.Empty;
            _bookTitle.Text = string.Empty;
            _bookAuthor.Text = string.Empty;
            _bookDuration.Text = string.Empty;
            _bookPrice.Text = string.Empty;
            _bookStacks.Text = string.Empty;
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
        private int eid = 0;
        private int stacks = 0;
        private string bookID = string.Empty;
        private string Status = string.Empty;
        private void EditBookButton_Click(object sender, RoutedEventArgs e) {
            // check if all info is filled up
            if (CheckIfAllDataIsFilledUp()) {
                MessageBox.Show("Missing information fill up all the data", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // check if ID is already added
            MainWindow window = Application.Current.Windows[0] as MainWindow;
            if (bookID != _bookID.Text) {
                foreach (var value in window.bookLists) {
                    if (value.ID.ToString() == _bookID.Text) {
                        MessageBox.Show("This ID is already added, get a unique ID", "", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
            }
            try {
                // update data BookList table
                stacks = Convert.ToInt32(_bookStacks.Text);
                if (stacks <= 0) {
                    Status = "BORROWED";
                }
                else {
                    Status = "AVAILABLE";
                }
                string query = "UPDATE BookList set ID='" + _bookID.Text +
                               "',Title='" + _bookTitle.Text +
                               "',Author='" + _bookAuthor.Text +
                               "',Duration='" + _bookDuration.Text +
                               "',Price='" + _bookPrice.Text +
                               "',Stacks='" + _bookStacks.Text +
                               "',Status='" + Status +
                               "'where eid='" + eid.ToString() + "'";
                window.database.Update(query);

                // insert data to EditBookLogs table
                query = "INSERT INTO EditBookLogs" +
                               "('ID', 'Title', 'Author', 'Duration', 'Price', 'Stacks', 'Status', 'TimeEdited')" +
                               "VALUES" +
                               "(@ID, @Title, @Author, @Duration, @Price, @Stacks, @Status, @TimeEdited)";
                string[] items = new string[] {
                    _bookID.Text, _bookTitle.Text, _bookAuthor.Text, _bookDuration.Text, _bookPrice.Text,
                    _bookStacks.Text, "AVAILABLE", DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString()
                };
                string[] location = new string[] {
                    "@ID", "@Title", "@Author", "@Duration", "@Price", "@Stacks", "@Status", "TimeEdited"
                };
                window.database.Insert(query, items, location, "EditBookLogs");
                window.bookLists.Clear();
                window.database.Retrieve("BookList");
                MessageBox.Show("Book has edited, Thank you!", "", MessageBoxButton.OK, MessageBoxImage.Information);
            } catch (Exception) { MessageBox.Show("Please enter a valid alphabet letters", "", MessageBoxButton.OK, MessageBoxImage.Error); }
            try {
                // update data to MyBooks table
                string query = "UPDATE MyBooks set BookID='" + _bookID.Text +
                               "',BookTitle='" + _bookTitle.Text +
                               "',BookDuration='" + _bookDuration.Text +
                               "',BookPrice='" + _bookPrice.Text +
                               "'where BookID='" + bookID + "'";
                window.database.Update(query);

                _selectBookComboBox.SelectedIndex = -1;
                _bookID.Text = string.Empty;
                _bookTitle.Text = string.Empty;
                _bookAuthor.Text = string.Empty;
                _bookDuration.Text = string.Empty;
                _bookPrice.Text = string.Empty;
                _bookStacks.Text = string.Empty;
            } catch (Exception) { }       
        }

        private void BackToMainButton_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
        private void SelectBookComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            try {
                if (_selectBookComboBox.SelectedIndex == -1) {
                    return;
                }

                _bookID.Text = window.bookLists[_selectBookComboBox.SelectedIndex].ID.ToString();
                _bookTitle.Text = window.bookLists[_selectBookComboBox.SelectedIndex].Title;
                _bookAuthor.Text = window.bookLists[_selectBookComboBox.SelectedIndex].Author;
                _bookDuration.Text = window.bookLists[_selectBookComboBox.SelectedIndex].Duration.ToString();
                _bookPrice.Text = window.bookLists[_selectBookComboBox.SelectedIndex].Price.ToString();
                _bookStacks.Text = window.bookLists[_selectBookComboBox.SelectedIndex].Stacks.ToString();
                eid = window.database.GetEid("BookList", Convert.ToInt32(_bookID.Text));

                bookID = _bookID.Text;
            } catch (Exception) { }
        }
        private void BookTItleTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e) {

        }

        private void BookAuthorTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e) {

        }

        private void BookPriceTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            Regex regex = new Regex("[^.0-9+]");
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

        private void BookIDTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            Regex regex = new Regex("[^0-9+]");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void SelectBookComboBox_PreviewKeyDown(object sender, KeyEventArgs e) {
            for (int i = 0; i < window.bookLists.Count; i++) {
                if (e.Key.ToString().ToCharArray()[0] == window.bookLists[i].Title[0]) {
                    _selectBookComboBox.SelectedIndex = i;
                    break;
                }
            }
        }
    }
}
