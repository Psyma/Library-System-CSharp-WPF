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
    /// Interaction logic for StudentListWindow.xaml
    /// </summary>
    public partial class StudentListWindow : Window {
        public StudentListWindow() {
            InitializeComponent();
        }
        private MainWindow window = Application.Current.Windows[0] as MainWindow;
        private void StudentListWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
            _searchNameTextBox.FontWeight = FontWeights.Normal;
            _searchNameTextBox.FontSize = 12;
            _searchNameTextBox.Text = "Search Name";
        }

        private void BackToMainButton_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
        private List<StudentList> tempStudentLists;
        private void SearchNameTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            if (window != null) {
                tempStudentLists = new List<StudentList>();
                if (window.studentLists != null) {
                    foreach (var value in window.studentLists) {
                        if (value.Name.ToLower().Contains(_searchNameTextBox.Text)) {
                            tempStudentLists.Add(value);
                        }
                    }
                    _studentListListView.ItemsSource = tempStudentLists;
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
        private void DeleteStudentButton_Click(object sender, RoutedEventArgs e) {
            if (_studentListListView.SelectedIndex == -1) {
                MessageBox.Show("Please select a student to delete", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            var result = MessageBox.Show("Do you want to delete this student?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (result) {
                case MessageBoxResult.Yes:
                    if (_searchNameTextBox.Text.Equals("Search Name")) {
                        if (!window.studentLists[_studentListListView.SelectedIndex].TotalBooks.Equals(0)) {
                            MessageBox.Show("You can't delete student that still borrowing book, sorry!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            return;
                        }
                        window.database.DeleteAt("StudentList", window.studentLists[_studentListListView.SelectedIndex].ID);
                        window.studentLists.RemoveAt(_studentListListView.SelectedIndex);
                        _studentListListView.ItemsSource = null;
                        _studentListListView.ItemsSource = window.studentLists;
                    }
                    else {
                        int index = 0;
                        foreach (var value in window.studentLists) {
                            if (value.ID.Equals(tempStudentLists[_studentListListView.SelectedIndex].ID)) {
                                break;
                            }
                            index++;
                        }
                        if (!window.studentLists[index].TotalBooks.Equals(0)) {
                            MessageBox.Show("You can't delete student that still borrowing book, sorry!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            return;
                        }
                        window.database.DeleteAt("StudentList", window.studentLists[index].ID);
                        window.studentLists.RemoveAt(index);
                        _studentListListView.ItemsSource = null;
                        _studentListListView.ItemsSource = window.studentLists;
                    }
                    break;
            }
        }
    }
}
