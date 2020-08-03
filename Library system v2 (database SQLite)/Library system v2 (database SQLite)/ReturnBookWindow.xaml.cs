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
using Library_system_v2__database_SQLite_.Classes;
 
namespace Library_system_v2__database_SQLite_ {
    /// <summary>
    /// Interaction logic for ReturnBookWindow.xaml
    /// </summary>
    public partial class ReturnBookWindow : Window {
        public ReturnBookWindow() {
            InitializeComponent();
        }
        private MainWindow window = Application.Current.Windows[0] as MainWindow;
        private List<MyBooks> tempBookLists;
        private void ReturnBookWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
            _bookListListView.ItemsSource = null;
            window.myBooks.Clear();
            _searchTitleTextBox.FontWeight = FontWeights.Normal;
            _searchTitleTextBox.FontSize = 12;
            _searchTitleTextBox.Text = "Search Title";
            tempBookLists.Clear();
        }

        private void BackToMainButton_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
        private string studentName = string.Empty;
        private string bookID = string.Empty;
        private string bookTitle = string.Empty;
        private string bookDuration = string.Empty;
        private string bookPrice = string.Empty;
        private string BorrowedDate = string.Empty;
        private string ReturnDate = string.Empty;
        private void ReturnBookButton_Click(object sender, RoutedEventArgs e) {
            // if searchbox is used the return book is not correct, need to fix
            try {  
                bool isEmpty = true; // if tempBookLists is null
                int index = 0;
                if (_bookListListView.SelectedIndex == -1) {
                    MessageBox.Show("Please select a book to return", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                if (_selectNameComboBox.SelectedIndex != -1) {
                    if (tempBookLists.Count != 0) {
                        for (int i = 0; i < window.myBooks.Count; i++) {
                            if (tempBookLists[_bookListListView.SelectedIndex].Title == window.myBooks[i].Title) {
                                index = i;
                                isEmpty = false;
                                break;
                            }
                        }
                    }
                    
                    if (isEmpty) {
                        window.database.DeleteSpecific("MyBooks", window.studentLists[_selectNameComboBox.SelectedIndex].ID, window.myBooks[_bookListListView.SelectedIndex].BookID);
                        int stacks = 0;
                        window.database.Retrieve("BookList");
                        foreach (var value in window.bookLists) {
                            if (value.ID == window.myBooks[_bookListListView.SelectedIndex].BookID) {
                                stacks = value.Stacks + 1;
                                bookID = value.ID.ToString();
                                bookTitle = value.Title;
                                bookDuration = value.Duration.ToString();
                                bookPrice = value.Price.ToString();
                                BorrowedDate = window.myBooks[_bookListListView.SelectedIndex].BorrowedDate;
                                ReturnDate = window.myBooks[_bookListListView.SelectedIndex].ReturnDate;
                                break;
                            }
                        }

                        // update BookList
                        int eid = window.database.GetEid("BookList", window.myBooks[_bookListListView.SelectedIndex].BookID);
                        string query = "UPDATE BookList set Stacks='" + stacks +
                                       "',Status='" + "AVAILABLE" +
                                       "'where eid='" + eid + "'";
                        window.database.Update(query);

                        // update StudentList
                        float amountPaid = window.studentLists[_selectNameComboBox.SelectedIndex].AmountPaid;
                        eid = window.database.GetEid("StudentList", window.studentLists[_selectNameComboBox.SelectedIndex].ID);
                        query = "UPDATE StudentList set TotalBooks='" + (window.studentLists[_selectNameComboBox.SelectedIndex].TotalBooks -= 1) +
                                       "',AmountPaid='" + (amountPaid - window.myBooks[_bookListListView.SelectedIndex].Price) +
                                       "'where eid='" + eid + "'";
                        window.database.Update(query);
                        window.studentLists[_selectNameComboBox.SelectedIndex].AmountPaid -= window.myBooks[_bookListListView.SelectedIndex].Price;
                        MessageBox.Show("You have successfully return the book, Thank you!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else {
                        window.database.DeleteSpecific("MyBooks", window.studentLists[_selectNameComboBox.SelectedIndex].ID, window.myBooks[index].BookID);
                        int stacks = 0;
                        window.database.Retrieve("BookList");
                        foreach (var value in window.bookLists) {
                            if (value.ID == window.myBooks[index].BookID) {
                                stacks = value.Stacks + 1;
                                bookID = value.ID.ToString();
                                bookTitle = value.Title;
                                bookDuration = value.Duration.ToString();
                                bookPrice = value.Price.ToString();
                                BorrowedDate = window.myBooks[index].BorrowedDate;
                                ReturnDate = window.myBooks[index].ReturnDate;
                                break;
                            }
                        }

                        // update BookList
                        int eid = window.database.GetEid("BookList", window.myBooks[index].BookID);
                        string query = "UPDATE BookList set Stacks='" + stacks +
                                       "',Status='" + "AVAILABLE" +
                                       "'where eid='" + eid + "'";
                        window.database.Update(query);

                        // update StudentList
                        float amountPaid = window.studentLists[_selectNameComboBox.SelectedIndex].AmountPaid;
                        eid = window.database.GetEid("StudentList", window.studentLists[_selectNameComboBox.SelectedIndex].ID);
                        query = "UPDATE StudentList set TotalBooks='" + (window.studentLists[_selectNameComboBox.SelectedIndex].TotalBooks -= 1) +
                                       "',AmountPaid='" + (amountPaid - window.myBooks[index].Price) +
                                       "'where eid='" + eid + "'";
                        window.database.Update(query);
                        window.studentLists[_selectNameComboBox.SelectedIndex].AmountPaid -= window.myBooks[index].Price;
                        MessageBox.Show("You have successfully return the book, Thank you!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
 
                    // insert to ReturnBookLogs table
                    string query1 = "INSERT INTO ReturnBookLogs" +
                                   "('StudentName', 'BookID', 'BookTitle', 'Duration', 'BookPrice', 'BorrowedDate', 'ReturnDate', 'ReturnTime')" +
                                   "VALUES" +
                                   "(@StudentName, @BookID, @BookTitle, @Duration, @BookPrice, @BorrowedDate, @ReturnDate, @ReturnTime)";
                    
                    string[] items = new string[] {
                        studentName, bookID, bookTitle, bookDuration, bookPrice, BorrowedDate, ReturnDate,
                        DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString()
                    };
                    string[] location = new string[] {
                        "@StudentName", "@BookID", "@BookTitle", "@Duration", "@BookPrice", "@BorrowedDate", "@ReturnDate", "@ReturnTime"
                    };
                    window.database.Insert(query1, items, location, "ReturnBookLogs");

                    isEmpty = true;
                    window.myBooks.Clear();
                    tempBookLists.Clear();
                    window.database.RetrieveAt("MyBooks", window.studentLists[_selectNameComboBox.SelectedIndex].ID);
                    _bookListListView.ItemsSource = null;
                    _bookListListView.ItemsSource = window.myBooks;
                    //this.Close();
                }
                else {
                    MessageBox.Show("Please select a student", "", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            } catch (Exception) { }
            
        }

        private void SelectNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            
            if (_selectNameComboBox.SelectedIndex != -1) {
                window.myBooks.Clear();
                _bookListListView.ItemsSource = null;
                window.database.RetrieveAt("MyBooks", window.studentLists[_selectNameComboBox.SelectedIndex].ID);
                _bookListListView.ItemsSource = window.myBooks;
                studentName = window.studentLists[_selectNameComboBox.SelectedIndex].Name;
            }
        }

        private void SelectNameComboBox_PreviewKeyDown(object sender, KeyEventArgs e) {
            for (int i = 0; i < window.studentLists.Count; i++) {
                if (e.Key.ToString().ToCharArray()[0] == window.studentLists[i].Name[0]) {
                    _selectNameComboBox.SelectedIndex = i;
                    break;
                }
            }
        }

        private void SearchNameTextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ClickCount == 1) {
                _searchTitleTextBox.Text = "";
                _searchTitleTextBox.FontSize = 12;
                _searchTitleTextBox.FontWeight = FontWeights.Bold;
            }
        }

        private void SearchNameTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            if (window != null) {
                tempBookLists = new List<MyBooks>();
                if (window.studentLists != null) {
                    foreach (var value in window.myBooks) {
                        if (value.Title.ToLower().Contains(_searchTitleTextBox.Text)) {
                            tempBookLists.Add(value);
                        }
                    }
                    _bookListListView.ItemsSource = tempBookLists;
                }
            }
        }
    }
}
