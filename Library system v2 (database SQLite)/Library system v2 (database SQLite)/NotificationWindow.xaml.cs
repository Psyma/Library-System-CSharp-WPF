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
    /// Interaction logic for NotificationWindow.xaml
    /// </summary>
    public partial class NotificationWindow : Window {
        public NotificationWindow() {
            InitializeComponent();
        }
        private MainWindow window = Application.Current.Windows[0] as MainWindow;
        private void NotificationWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
            window.myBooks.Clear();
            _searchTitleTextBox.FontWeight = FontWeights.Normal;
            _searchTitleTextBox.FontSize = 12;
            _searchTitleTextBox.Text = "Search Name";
        }

        private void BackToMainButton_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
        private void SearchNameTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            if (window != null) {
                List<MyBooks> tempBookLists = new List<MyBooks>();
                if (window.studentLists != null) {
                    foreach (var value in window.myBooks) {
                        if (value.StudentName.ToLower().Contains(_searchTitleTextBox.Text)) {
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
    }
}
