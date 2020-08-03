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
using System.Net.Mail;
using System.Net;

namespace Library_system_v2__database_SQLite_ {
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window {
        public RegisterWindow() {
            InitializeComponent();
        }

        private void RegisterWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
        }
        private void RegisterButton_Click(object sender, RoutedEventArgs e) {
            if(_usernameTextBox.Text == string.Empty) {
                MessageBox.Show("Please input username", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (_passwordBox.Password == string.Empty) {
                MessageBox.Show("Please input password", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return; 
            }
            if(_emailTextBox.Text == string.Empty) {
                MessageBox.Show("Please input a valid email", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            try {
                // check if input email is valid
                new MailAddress(_emailTextBox.Text);
            }catch(Exception) { 
                MessageBox.Show("Please input a valid email", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // generate random character
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random();
            for (int i = 0; i < stringChars.Length; i++) {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            var historyPassword = new String(stringChars);
            try {
                // send email
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress("noreplyemail00123@gmail.com");
                msg.To.Add(_emailTextBox.Text);
                msg.Subject = "Library Admin Password";
                msg.Body = "Login - Username: " + _usernameTextBox.Text + "\n" +
                           "Login - Password: " + _passwordBox.Password + "\n\n" +
                           "History - Username: " + _usernameTextBox.Text + "\n" +
                           "History - Password: " + historyPassword;
                //msg.Priority = MailPriority.High;

                using (SmtpClient client = new SmtpClient()) {
                    client.EnableSsl = true;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("noreplyemail00123@gmail.com", "librarypassword");
                    client.Host = "smtp.gmail.com";
                    client.Port = 587;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;

                    client.Send(msg);
                }
                MessageBox.Show("Username and Password sent to your email, please confirm and re-open the application, Thank you!", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }catch(Exception) { 
                MessageBox.Show("Unable to register, please check your internet connection", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // insert data to Admin table
            MainWindow window = Application.Current.Windows[0] as MainWindow;
            string query = "INSERT INTO Admin" +
                           "('LoginUsername', 'LoginPassword', 'HistoryUsername', 'HistoryPassword')" +
                           "VALUES" +
                           "(@LoginUsername, @LoginPassword, @HistoryUsername, @HistoryPassword)";
            string[] items = new string[] {
                _usernameTextBox.Text, _passwordBox.Password, _usernameTextBox.Text, historyPassword
            };
            string[] location = new string[] {
                "@LoginUsername", "@LoginPassword", "@HistoryUsername", "@HistoryPassword"
            };
            window.database.Insert(query, items, location, "Admin");
            window.admin.Clear();
            window.database.Retrieve("Admin");
            window.loginWindow._signUpButton.Opacity = 0.0;
            window.loginWindow._signUpButton.IsEnabled = false;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) {
            _usernameTextBox.Text = string.Empty;
            _passwordBox.Password = string.Empty;
            _emailTextBox.Text = string.Empty;
            this.Close();
        }
    }
}
