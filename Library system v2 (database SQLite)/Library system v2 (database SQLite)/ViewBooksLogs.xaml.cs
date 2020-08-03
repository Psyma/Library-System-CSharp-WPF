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
    /// Interaction logic for BooksLogs.xaml
    /// </summary>
    public partial class ViewBooksLogs : Window {
        public ViewBooksLogs() {
            InitializeComponent();
        }
        private MainWindow window = Application.Current.Windows[0] as MainWindow;
        private void BooksLogsWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
            _searchTitleTextBox.FontWeight = FontWeights.Normal;
            _searchTitleTextBox.FontSize = 12;
            _searchTitleTextBox.Text = "Search Title";
        }
        private void SearchNameTextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ClickCount == 1) {
                _searchTitleTextBox.Text = "";
                _searchTitleTextBox.FontSize = 12;
                _searchTitleTextBox.FontWeight = FontWeights.Bold;
            }
        }
        private void SearchNameTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            if(window != null) {
                List<AddingBookLogs> tempAddingBookLogs = new List<AddingBookLogs>();
                List<EditBooksLogs> tempEditBookLogs = new List<EditBooksLogs>();
                if(window.addingBookLogs != null) {
                    foreach(var value in window.addingBookLogs) {
                        if (value.Title.ToLower().Contains(_searchTitleTextBox.Text)) {
                            tempAddingBookLogs.Add(value);
                        }
                    }
                    _bookAddedLogsListView.ItemsSource = tempAddingBookLogs;
                    
                }
                if(window.editBooksLogs != null) {
                    foreach(var value in window.editBooksLogs) {
                        if (value.Title.ToLower().Contains(_searchTitleTextBox.Text)) {
                            tempEditBookLogs.Add(value);
                        }
                    }
                    _bookEditedLogsListView.ItemsSource = tempEditBookLogs;
                }
            }
        }

        private void BackToMainButton_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e) {
            var result = MessageBox.Show("Do you want to clear the logs?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (result) {
                case MessageBoxResult.Yes:
                    window.database.DeleteAll("AddingBookLogs");
                    window.database.DeleteAll("EditBookLogs");
                    window.addingBookLogs.Clear();
                    window.editBooksLogs.Clear();
                    this.Close();
                    break;
            }
        }
    }
}
