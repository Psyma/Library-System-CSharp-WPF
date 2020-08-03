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
    /// Interaction logic for BookListWindow.xaml
    /// </summary>
    public partial class BookListWindow : Window {
        public BookListWindow() {
            InitializeComponent();
        }
        MainWindow window = Application.Current.Windows[0] as MainWindow;
        private void BookListWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
            _bookListListView.SelectedIndex = -1;
            _searchTitleTextBox.FontWeight = FontWeights.Normal;
            _searchTitleTextBox.FontSize = 12;
            _searchTitleTextBox.Text = "Search Title";
        }
        private void BackToMainButton_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
        private List<BookList> tempBookLists;
        private void SearchNameTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            if (window != null) {
                tempBookLists = new List<BookList>();
                if (window.bookLists != null) {
                    foreach (var value in window.bookLists) {
                        if (value.Title.ToLower().Contains(_searchTitleTextBox.Text)) {
                            tempBookLists.Add(value);
                        }
                    }
                    _bookListListView.ItemsSource = tempBookLists;
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

        private void DeleteBookButton_Click(object sender, RoutedEventArgs e) {
            if (_bookListListView.SelectedIndex == -1) {
                MessageBox.Show("Please select a book to delete", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            var result = MessageBox.Show("Do you want to delete this book?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (result) {
                case MessageBoxResult.Yes:
                    if(_searchTitleTextBox.Text.Equals("Search Title")) {
                        if (!window.bookLists[_bookListListView.SelectedIndex].Stacks.Equals(0)) {
                            MessageBox.Show("You can't delete book that still has stacks, sorry!", "", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        window.database.DeleteAt("BookList", window.bookLists[_bookListListView.SelectedIndex].ID);
                        window.bookLists.RemoveAt(_bookListListView.SelectedIndex);
                        _bookListListView.ItemsSource = null;
                        _bookListListView.ItemsSource = window.bookLists;
                    }
                    else {
                        int index = 0;
                        foreach(var value in window.bookLists) {
                            if (value.ID.Equals(tempBookLists[_bookListListView.SelectedIndex].ID)) {
                                break;
                            }
                            index++;
                        }
                        if (!window.bookLists[index].Stacks.Equals(0)) {
                            MessageBox.Show("You can't delete book that still has stacks, sorry!", "", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        window.database.DeleteAt("BookList", window.bookLists[index].ID);
                        window.bookLists.RemoveAt(index);
                        _bookListListView.ItemsSource = null;
                        _bookListListView.ItemsSource = window.bookLists;
                    }
                    break;
            }
        }
    }
}
