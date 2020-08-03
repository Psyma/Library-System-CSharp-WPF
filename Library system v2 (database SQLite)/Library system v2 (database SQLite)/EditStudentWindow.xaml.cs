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
    /// Interaction logic for EditStudentWindow.xaml
    /// </summary>
    public partial class EditStudentWindow : Window {
        public EditStudentWindow() {
            InitializeComponent();
        }
        private void EditStudentWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
            _selectNameComboBox.SelectedIndex = -1;
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
            else if(_maleCheckBox.IsChecked == false && _femaleCheckBox.IsChecked == false) {
                return true;
            }
            return false;
        }
        private int eid = 0;
        private string studentID = string.Empty;
        private void EditStudentButton_Click(object sender, RoutedEventArgs e) {
            // check if all info is filled up
            if (CheckIfAllDataIsFilledUp()) {
                MessageBox.Show("Missing information fill up all the data", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if(_selectNameComboBox.SelectedIndex == -1) {
                MessageBox.Show("Student not found", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // check if ID is already added
            MainWindow window = Application.Current.Windows[0] as MainWindow;
            if (studentID != _studentID.Text) {
                foreach (var value in window.studentLists) {
                    if (value.ID.ToString() == _studentID.Text) {
                        MessageBox.Show("This ID is already added, get a unique ID", "", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
            }
            try {
                // update data to StudentList table
                string gender = string.Empty;
                if (_maleCheckBox.IsChecked == true) {
                    gender = "Male";
                }
                else if (_femaleCheckBox.IsChecked == true) {
                    gender = "Female";
                }
                string query = "UPDATE StudentList set ID='" + _studentID.Text +
                               "',Name='" + _studentFullName.Text +
                               "',Gender='" + gender +
                               "',BirthDate='" + _studentBirthdate.Text +
                               "',CourseYrLv='" + _studentCourse.Text +
                               "',PhoneNumber='" + _studentPhoneNumber.Text +
                               "',Address='" + _studentAddress.Text +
                               "'where eid='" + eid.ToString() + "'";
                window.database.Update(query);

                // insert data to EditStudentLogs table
                query = "INSERT INTO EditStudentLogs" +
                   "('ID', 'Name', 'Gender', 'BirthDate', 'CourseYrLv', 'PhoneNumber', 'TotalBooks', 'AmountPaid', 'Address', 'TimeEdited')" +
                   "VALUES" +
                   "(@ID, @Name, @Gender, @BirthDate, @CourseYrLv, @PhoneNumber, @TotalBooks, @AmountPaid, @Address, @TimeEdited)";
                string[] items = new string[] {
                    _studentID.Text, _studentFullName.Text, gender, _studentBirthdate.Text, _studentCourse.Text,
                    0.ToString(), 0.ToString(), _studentPhoneNumber.Text,_studentAddress.Text, 
                    DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString()
                };
                string[] location = new string[] {
                      "@ID", "@Name", "@Gender", "@BirthDate", "@CourseYrLv",
                      "@TotalBooks", "@AmountPaid", "@PhoneNumber", "@Address", "@TimeEdited"
                };
                window.database.Insert(query, items, location, "EditStudentLogs");

                MessageBox.Show("Student information has edited, Thank you!", "", MessageBoxButton.OK, MessageBoxImage.Information);
            } catch (Exception) { }
            try {
                // update data to MyBooks table
                string query = "UPDATE MyBooks set StudentID='" + _studentID.Text +
                               "',StudentName='" + _studentFullName.Text + 
                               "'where StudentID='" + studentID + "'";
                window.database.Update(query);

                _selectNameComboBox.SelectedIndex = -1;
                _studentID.Text = string.Empty;
                _studentFullName.Text = string.Empty;
                _maleCheckBox.IsChecked = false;
                _femaleCheckBox.IsChecked = false;
                _studentBirthdate.Text = string.Empty;
                _studentCourse.Text = string.Empty;
                _studentPhoneNumber.Text = string.Empty;
                _studentAddress.Text = string.Empty;
            } catch(Exception) {  }
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

        private void SelectNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            try {
                if (_selectNameComboBox.SelectedIndex == -1) {
                    return;
                }
                MainWindow window = Application.Current.Windows[0] as MainWindow;
                _studentID.Text = window.studentLists[_selectNameComboBox.SelectedIndex].ID.ToString();
                _studentFullName.Text = window.studentLists[_selectNameComboBox.SelectedIndex].Name;
                if(window.studentLists[_selectNameComboBox.SelectedIndex].Gender == "Male") {
                    _maleCheckBox.IsChecked = true;
                    _femaleCheckBox.IsChecked = false;
                }
                else if (window.studentLists[_selectNameComboBox.SelectedIndex].Gender == "Female") {
                    _femaleCheckBox.IsChecked = true;
                    _maleCheckBox.IsChecked = false;
                }
                _studentBirthdate.Text = window.studentLists[_selectNameComboBox.SelectedIndex].BirthDate;
                _studentCourse.Text = window.studentLists[_selectNameComboBox.SelectedIndex].CourseYrLv;
                _studentPhoneNumber.Text = window.studentLists[_selectNameComboBox.SelectedIndex].PhoneNumber.ToString();
                _studentAddress.Text = window.studentLists[_selectNameComboBox.SelectedIndex].Address;
                // get the eid value from SQLite database
                eid = window.database.GetEid("StudentList", Convert.ToInt32(_studentID.Text));
                studentID = _studentID.Text;
            } catch (Exception) { }
        }

        private void IDTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            Regex regex = new Regex("[^0-9+]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void FullNameTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            Regex regex = new Regex("[^aA-zZ+]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void PhoneNumberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            Regex regex = new Regex("[^0-9+]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void SelectNameComboBox_PreviewKeyDown(object sender, KeyEventArgs e) {
            MainWindow window = Application.Current.Windows[0] as MainWindow;
            for (int i = 0; i < window.studentLists.Count; i++) {
                if (e.Key.ToString().ToCharArray()[0] == window.studentLists[i].Name[0]) {
                    _selectNameComboBox.SelectedIndex = i;
                    break;
                }
            }
        }
    }
}
