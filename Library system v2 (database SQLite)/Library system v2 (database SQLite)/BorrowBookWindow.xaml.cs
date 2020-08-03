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
    /// Interaction logic for BorrowBookWindow.xaml
    /// </summary>
    public partial class BorrowBookWindow : Window {
        public BorrowBookWindow() {
            InitializeComponent();
            selectedBooks = new List<BookList>();
        }
        private MainWindow window = Application.Current.Windows[0] as MainWindow;
        public List<BookList> selectedBooks;
        private List<BookList> tempBookLists;
        private void BorrowBookWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
            _searchTitleTextBox.FontWeight = FontWeights.Normal;
            _searchTitleTextBox.FontSize = 12;
            _searchTitleTextBox.Text = "Search Title";
            _searchSelectedTitleTextBox.FontWeight = FontWeights.Normal;
            _searchSelectedTitleTextBox.FontSize = 12;
            _searchSelectedTitleTextBox.Text = "Search Title";
        }

        private void BackToMainButton_Click(object sender, RoutedEventArgs e) {
            foreach (var value in window.bookLists) {
                foreach(var value1 in selectedBooks) {
                    if(value.Title == value1.Title) {
                        value.Stacks++;
                        break;
                    }
                }
            }
            _bookListListView.ItemsSource = null;
            _selectedBooksListView.ItemsSource = null;
            _totalPriceLabel.Text = "0";
            this.Close();
        }
        
        private void AddSelectedBookButton_Click(object sender, RoutedEventArgs e) {
            try {
                int index = 0;
                bool isEmpty = true; // if tempBookLists is empty
                bool isSearchBoxUsed = false;
                _selectedBooksListView.SelectedIndex = -1;
                float totalPrice = float.Parse(_totalPriceLabel.Text.ToString());
                if (_bookListListView.SelectedIndex != -1 && _selectNameComboBox.SelectedIndex != -1) {
                    if (tempBookLists.Count != 0) {
                        for (int i = 0; i < window.bookLists.Count; i++) {
                            if (tempBookLists[_bookListListView.SelectedIndex].Title == window.bookLists[i].Title) {
                                index = i;
                                isEmpty = false;
                                isSearchBoxUsed = true;
                                break;
                            }
                        }
                    }
                    if (window.bookLists[_bookListListView.SelectedIndex].Stacks <= 0 && !isSearchBoxUsed) {
                        MessageBox.Show("This book is out of stack, sorry!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    else if (window.bookLists[index].Stacks <= 0 && !isEmpty) {
                        MessageBox.Show("This book is out of stack, sorry!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    window.database.RetrieveAt("MyBooks", window.studentLists[_selectNameComboBox.SelectedIndex].ID);
                    foreach (var value in window.myBooks) {
                        if (value.Title == window.bookLists[_bookListListView.SelectedIndex].Title && !isSearchBoxUsed) {
                            MessageBox.Show("You already borrowed this book, sorry!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }
                        else if (value.Title == window.bookLists[index].Title && !isEmpty) {
                            MessageBox.Show("You already borrowed this book, sorry!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }
                    }

                    if (isEmpty) {
                        foreach (var value in selectedBooks) {
                            if (value.Title == window.bookLists[_bookListListView.SelectedIndex].Title) {
                                MessageBox.Show("You already selected this book", "", MessageBoxButton.OK, MessageBoxImage.Information);
                                return;
                            }
                        }
                        _selectedBooksListView.ItemsSource = null;
                        selectedBooks.Add(window.bookLists[_bookListListView.SelectedIndex]);
                        totalPrice += window.bookLists[_bookListListView.SelectedIndex].Price;
                        _totalPriceLabel.Text = totalPrice.ToString();
                        window.bookLists[_bookListListView.SelectedIndex].Stacks--;
                        _bookListListView.ItemsSource = null;
                        _bookListListView.ItemsSource = window.bookLists;
                        _selectedBooksListView.ItemsSource = selectedBooks;
                    }
                    else {
                        // If searchbox is used
                        foreach (var value in selectedBooks) {
                            if (value.Title == window.bookLists[index].Title) {
                                MessageBox.Show("You already selected this book", "", MessageBoxButton.OK, MessageBoxImage.Information);
                                return;
                            }
                        }
                        _selectedBooksListView.ItemsSource = null;
                        selectedBooks.Add(window.bookLists[index]);
                        totalPrice += window.bookLists[index].Price;
                        _totalPriceLabel.Text = totalPrice.ToString();
                        window.bookLists[index].Stacks--;
                        _bookListListView.ItemsSource = null;
                        _bookListListView.ItemsSource = window.bookLists;
                        _selectedBooksListView.ItemsSource = selectedBooks;
                    }
                }
                else if (_selectNameComboBox.SelectedIndex == -1) {
                    MessageBox.Show("Please select a student first", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                else {
                    MessageBox.Show("Please select a book to borrow", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                isEmpty = false;
                isSearchBoxUsed = false;
                tempBookLists.Clear();
            } catch(Exception) {  } 
        }

        private void RemoveSelectedBooksButton_Click(object sender, RoutedEventArgs e) {
            try {
                if(_selectedBooksListView.SelectedIndex == -1) {
                    MessageBox.Show("Please select a book to remove", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                _bookListListView.SelectedIndex = -1;
                float totalPrice = float.Parse(_totalPriceLabel.Text.ToString());
                if (_selectedBooksListView.SelectedIndex != -1) {
                    foreach (var value in window.bookLists) {
                        if (value.ID == selectedBooks[_selectedBooksListView.SelectedIndex].ID) {
                            value.Stacks++;
                            totalPrice -= value.Price;
                            _totalPriceLabel.Text = totalPrice.ToString();
                            break;
                        }
                    }
                    _bookListListView.ItemsSource = null;
                    _bookListListView.ItemsSource = window.bookLists;
                    selectedBooks.RemoveAt(_selectedBooksListView.SelectedIndex);
                    _selectedBooksListView.ItemsSource = null;
                    _selectedBooksListView.ItemsSource = selectedBooks;
                }
            }catch(Exception) {  }
        }

        private void BorrowBookButton_Click(object sender, RoutedEventArgs e) {
            if(_selectNameComboBox.SelectedIndex == -1) {
                MessageBox.Show("Please select a student", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (selectedBooks.Count == 0) {
                MessageBox.Show("Please select a book to borrow", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            window.enterPaymentWindow._totalPriceLabel.Content = _totalPriceLabel.Text;
            window.enterPaymentWindow.ShowDialog();
        }

        private void SelectNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if(_selectNameComboBox.SelectedIndex != -1) {
                try {
                    foreach (var value in window.bookLists) {
                        foreach (var value1 in selectedBooks) {
                            if (value == value1) {
                                value.Stacks++;
                                break;
                            }
                        }
                    }
                    _bookListListView.ItemsSource = null;
                    _bookListListView.ItemsSource = window.bookLists;
                    window.myBooks.Clear();
                    selectedBooks.Clear();
                    _totalPriceLabel.Text = "0";
                    _selectedBooksListView.ItemsSource = null;
                    window.database.RetrieveAt("MyBooks", window.studentLists[_selectNameComboBox.SelectedIndex].ID);
                }catch(Exception) {  }
            }
        }
     
        private void SearchTitleTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            if (window != null) {
                tempBookLists = new List<BookList>();
                if (window.studentLists != null) {
                    foreach (var value in window.bookLists) {
                        
                        if (value.Title.ToLower().Contains(_searchTitleTextBox.Text)) {
                            tempBookLists.Add(value);
                        }
                    }
                    _bookListListView.ItemsSource = tempBookLists;
                }
            }
        }

        private void SearchTitleTextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ClickCount == 1) {
                _searchTitleTextBox.Text = "";
                _searchTitleTextBox.FontSize = 12;
                _searchTitleTextBox.FontWeight = FontWeights.Bold;
            }
        }

        private void SearchSelectedTitleTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            if (window != null) {
                List<BookList> tempSelectedBookLists = new List<BookList>();
                if (selectedBooks != null) {
                    foreach (var value in selectedBooks) {
                        if (value.Title.ToLower().Contains(_searchSelectedTitleTextBox.Text)) {
                            tempSelectedBookLists.Add(value);
                        }
                    }
                    _selectedBooksListView.ItemsSource = tempSelectedBookLists;
                }
            }
        }

        private void SearchSelectedTitleTextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ClickCount == 1) {
                _searchSelectedTitleTextBox.Text = "";
                _searchSelectedTitleTextBox.FontSize = 12;
                _searchSelectedTitleTextBox.FontWeight = FontWeights.Bold;
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
    }
}
