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

namespace Library_system_v2__database_SQLite_ {
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window {
        public LoginWindow() {
            InitializeComponent();
        }
        private bool loginAccess = false;
        private void ClickButton(string button) {
            this.Hide();
            if(button.Equals("SignUpButton")) {
                    RegisterWindow registerWindow = new RegisterWindow();
                    registerWindow.ShowDialog();
            }
            this.ShowDialog();
        }
        private void LoginWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
            if (!loginAccess && !adminLogin) {
                Application.Current.Shutdown();
            }
            _userNameTextBox.Text = string.Empty;
            _passwordTextBox.Password = string.Empty;
        }

        private void UserNameTextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ClickCount == 1) {
                _userNameTextBox.Text = "";
                _userNameTextBox.Foreground = Brushes.Black;
                _userNameTextBox.FontWeight = FontWeights.Bold;
                _userNameTextBox.FontSize = 12;
            }
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e) {
            ClickButton("SignUpButton");
        }
        private bool adminLogin = false;
        private void LoginButton_Click(object sender, RoutedEventArgs e) {
            MainWindow window = Application.Current.Windows[0] as MainWindow;
            if (window.admin.Count != 0) {
                // admin login
                if (_userNameTextBox.Text.Equals(window.admin[0].LoginUsername)
                    && _passwordTextBox.Password.Equals(window.admin[0].LoginPassword)
                    && !adminLogin) {
                    loginAccess = true;
                    adminLogin = true;
                    // insert to LoggingLogs table
                    string query = "INSERT INTO LoggingLogs" +
                                   "('AdminLoginTime', 'AdminLogoutTime', 'HistoryLoginTime', 'HistoryLogoutTime')" +
                                   "VALUES" +
                                   "(@AdminLoginTime, @AdminLogoutTime, @HistoryLoginTime, @HistoryLogoutTime)";
                    string[] items = new string[] {
                        DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString(),
                        string.Empty,
                        string.Empty,
                        string.Empty
                    };
                    string[] location = new string[] {
                        "@AdminLoginTime", "@AdminLogoutTime", "@HistoryLoginTime", "@HistoryLogoutTime"
                    };
                    window.database.Insert(query, items, location, "LoggingLogs");
                    window.LoginTime = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
                    this.Close();
                }
                // history login
                else if (_userNameTextBox.Text.Equals(window.admin[0].HistoryUsername)
                    && _passwordTextBox.Password.Equals(window.admin[0].HistoryPassword)
                    && adminLogin) {
                    if (window.viewLoggingLogs.clearLoggingLogs) {
                        MessageBox.Show("Please logout and re-login, Thank you", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        window.loggingLogs.Clear();
                        return;
                    }
                    string query = "UPDATE LoggingLogs set HistoryLoginTime='" + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() +
                           "'where AdminLoginTime='" + window.LoginTime + "'";
                    window.database.Update(query);
                    this.Close();
                    window.historyWindow.ShowDialog();
                }
                else if (!_userNameTextBox.Text.Equals(window.admin[0].HistoryUsername) && adminLogin) {
                    MessageBox.Show("Incorrect username", "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (!_passwordTextBox.Password.Equals(window.admin[0].HistoryPassword) && adminLogin) {
                    MessageBox.Show("Incorrect password, Please use the history password check your email", "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (!_userNameTextBox.Text.Equals(window.admin[0].LoginUsername) && !adminLogin) {
                    MessageBox.Show("Incorrect username", "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (!_passwordTextBox.Password.Equals(window.admin[0].LoginPassword) && !adminLogin) {
                    MessageBox.Show("Incorrect password", "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else {
                if (window.admin.Count == 0) {
                    MessageBox.Show("Please Sign up first", "", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}
