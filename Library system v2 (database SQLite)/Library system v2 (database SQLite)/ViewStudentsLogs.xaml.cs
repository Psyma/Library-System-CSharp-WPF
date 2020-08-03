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
    /// Interaction logic for StudentsLogs.xaml
    /// </summary>
    public partial class ViewStudentsLogs : Window {
        public ViewStudentsLogs() {
            InitializeComponent();
        }
        private MainWindow window = Application.Current.Windows[0] as MainWindow;
        private void StudentLogsWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
            _searchNameTextBox.FontWeight = FontWeights.Normal;
            _searchNameTextBox.FontSize = 12;
            _searchNameTextBox.Text = "Search Name";
        }

        private void SearchNameTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            if(window != null) {
                List<AddingStudentLogs> tempAddingStudentLogs = new List<AddingStudentLogs>();
                List<EditStudentLogs> tempEditStudentLogs = new List<EditStudentLogs>();
                if(window.addingStudentLogs != null) {
                    foreach(var value in window.addingStudentLogs) {
                        if (value.Name.ToLower().Contains(_searchNameTextBox.Text)) {
                            tempAddingStudentLogs.Add(value);
                        }
                    }
                    _studentAddedLogsListView.ItemsSource = tempAddingStudentLogs;
                }
                if (window.editStudentLogs != null) {
                    foreach (var value in window.editStudentLogs) {
                        if (value.Name.ToLower().Contains(_searchNameTextBox.Text)) {
                            tempEditStudentLogs.Add(value);
                        }
                    }
                    _studentEditLogsListView.ItemsSource = tempEditStudentLogs;
                }
            }
        }

        private void SearchNameTextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ClickCount == 1) {
                _searchNameTextBox.Text = "";
                _searchNameTextBox.FontSize = 12;
                _searchNameTextBox.FontWeight = FontWeights.Bold;
            }
        }

        private void Backutton_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e) {
            var result = MessageBox.Show("Do you want to clear the logs?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (result) {
                case MessageBoxResult.Yes:
                    window.database.DeleteAll("AddingStudentLogs");
                    window.database.DeleteAll("EditStudentLogs");
                    window.addingBookLogs.Clear();
                    window.editBooksLogs.Clear();
                    this.Close();
                    break;
            }
        }
    }
}
