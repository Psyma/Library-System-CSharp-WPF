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
    /// Interaction logic for AddStudentWindow.xaml
    /// </summary>
    public partial class AddStudentWindow : Window {
        public AddStudentWindow() {
            InitializeComponent(); 
        }

        private void AddStudentWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
            _studentID.Text = string.Empty;
            _studentFullName.Text = string.Empty;
            _maleCheckBox.IsChecked = false;
            _femaleCheckBox.IsChecked = false;
            _studentBirthdate.Text = string.Empty;
            _studentCourse.Text = string.Empty;
            _studentPhoneNumber.Text = string.Empty;
            _studentAddress.Text = string.Empty;
        }
        private bool CheckIfAllDataIsFilledUp() {
            if (string.IsNullOrWhiteSpace(_studentID.Text)) {
                return true;
            }
            else if (string.IsNullOrWhiteSpace(_studentFullName.Text)) {
                return true;
            }
            else if (string.IsNullOrWhiteSpace(_studentBirthdate.Text)) {
                return true;
            }
            else if (string.IsNullOrWhiteSpace(_studentCourse.Text)) {
                return true;
            }
            else if (string.IsNullOrWhiteSpace(_studentPhoneNumber.Text)) {
                return true;
            }
            else if (string.IsNullOrWhiteSpace(_studentAddress.Text)) {
                return true;
            }
            else if (_maleCheckBox.IsChecked == false && _femaleCheckBox.IsChecked == false) {
                return true;
            }
            return false;
        }
        private void AddStudentButton_Click(object sender, RoutedEventArgs e) {
            // check if all info is filled up
            if (CheckIfAllDataIsFilledUp()) {
                MessageBox.Show("Missing information fill up all the data", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // check if ID is already added
            MainWindow window = Application.Current.Windows[0] as MainWindow;
            window.database.Retrieve("StudentList");
            foreach (var value in window.studentLists) {
                if(value.ID.ToString() == _studentID.Text) {
                    MessageBox.Show("This ID is already added, get a unique ID", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    window.studentLists.Clear();
                    return;
                }
            }
            try {
                // insert data to StudentList table
                string query = "INSERT INTO StudentList" +
                "('ID', 'Name', 'Gender', 'BirthDate', 'CourseYrLv', 'PhoneNumber', 'TotalBooks', 'AmountPaid','Address')" +
                "VALUES" +
                "(@ID, @Name, @Gender, @BirthDate, @CourseYrLv, @PhoneNumber, @TotalBooks, @AmountPaid, @Address)";
                string gender = string.Empty;
                if (_maleCheckBox.IsChecked == true) {
                    gender = "Male";
                }
                else if (_femaleCheckBox.IsChecked == true) {
                    gender = "Female";
                }

                string[] items = new string[] {
                    _studentID.Text, _studentFullName.Text, gender, _studentBirthdate.Text, _studentCourse.Text,
                    0.ToString(), 0.ToString(), _studentPhoneNumber.Text,_studentAddress.Text
                };
                string[] location = new string[] {
                      "@ID", "@Name", "@Gender", "@BirthDate", "@CourseYrLv",
                      "@TotalBooks", "@AmountPaid", "@PhoneNumber", "@Address"
                };
                window.database.Insert(query, items, location, "StudentList");

                // insert data to AddingStudentLogs table
                query = "INSERT INTO AddingStudentLogs" +
                "('ID', 'Name', 'Gender', 'BirthDate', 'CourseYrLv', 'PhoneNumber', 'TotalBooks', 'AmountPaid', 'Address', 'TimeAdded')" +
                "VALUES" +
                "(@ID, @Name, @Gender, @BirthDate, @CourseYrLv, @PhoneNumber, @TotalBooks, @AmountPaid, @Address, @TimeAdded)";
                items = new string[] {
                    _studentID.Text, _studentFullName.Text, gender, _studentBirthdate.Text, _studentCourse.Text,
                    0.ToString(), 0.ToString(), _studentPhoneNumber.Text,_studentAddress.Text, 
                    DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString()
                };
                location = new string[] {
                      "@ID", "@Name", "@Gender", "@BirthDate", "@CourseYrLv",
                      "@TotalBooks", "@AmountPaid", "@PhoneNumber", "@Address", "@TimeAdded"
                };
                window.database.Insert(query, items, location, "AddingStudentLogs");

                MessageBox.Show("Student Added, Thank you!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                _studentID.Text = string.Empty;
                _studentFullName.Text = string.Empty;
                _maleCheckBox.IsChecked = false;
                _femaleCheckBox.IsChecked = false;
                _studentBirthdate.Text = string.Empty;
                _studentCourse.Text = string.Empty;
                _studentPhoneNumber.Text = string.Empty;
                _studentAddress.Text = string.Empty;
            } catch (Exception) { }
        }

        private void BackToMainButton_Click(object sender, RoutedEventArgs e) {

            this.Close();
        }

        private void MaleCheckBox_Checked(object sender, RoutedEventArgs e) {
            if(_maleCheckBox.IsChecked == true) {
                _femaleCheckBox.IsChecked = false;
            }
        }

        private void FemaleCheckBox_Checked(object sender, RoutedEventArgs e) {
            if (_femaleCheckBox.IsChecked == true) {
                _maleCheckBox.IsChecked = false;
            }
        }

        private void IDTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            Regex regex = new Regex("[^0-9+]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void FullNameTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            Regex regex = new Regex("[^.aA-zZ+]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void PhoneNumberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            Regex regex = new Regex("[^-0-9+]");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
