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
    /// Interaction logic for EnterPaymentWindow.xaml
    /// </summary>
    public partial class EnterPaymentWindow : Window {
        public EnterPaymentWindow() {
            InitializeComponent();
        }

        private void EnterPaymentWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
            
            _enterPaymentTextBox.Text = string.Empty;
            _totalPriceLabel.Content = string.Empty;
        }

        private void ConfirmToBorrowButton_Click(object sender, RoutedEventArgs e) {
            MainWindow window = Application.Current.Windows[0] as MainWindow;
            try {
                if (string.IsNullOrWhiteSpace(_enterPaymentTextBox.Text)) {
                    MessageBox.Show("Enter your payment!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                Regex regex = new Regex("[^ aA-zZ+]");
                if (!regex.IsMatch(_enterPaymentTextBox.Text)) {
                    MessageBox.Show("Invalid characters!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (float.Parse(_enterPaymentTextBox.Text) >= float.Parse(window.borrowBookWindow._totalPriceLabel.Text)) {
                    string status = string.Empty;
                    string query = string.Empty;

                    // update BookList
                    foreach (var value in window.bookLists) {
                        foreach (var value1 in window.borrowBookWindow.selectedBooks) {
                            if (value.ID == value1.ID) {
                                if (value.Stacks <= 0) {
                                    status = "BORROWED";
                                }
                                else {
                                    status = "AVAILABLE";
                                }
                                query = "UPDATE BookList set Stacks='" + value.Stacks.ToString() +
                                              "',Status='" + status +
                                              "'where ID='" + value.ID.ToString() + "'";
                                window.database.Update(query);
                                break;
                            }
                        }
                    }

                    // update StudentList table
                    float totalPrice = 0.0f;
                    int totalBooks = 0;
                    foreach(var value in window.borrowBookWindow.selectedBooks) {
                        totalPrice += value.Price;
                        totalBooks++;
                    }
                    query = "UPDATE StudentList set TotalBooks='" + (window.studentLists[window.borrowBookWindow._selectNameComboBox.SelectedIndex].TotalBooks += totalBooks).ToString() +
                                              "',AmountPaid='" + (window.studentLists[window.borrowBookWindow._selectNameComboBox.SelectedIndex].AmountPaid  += totalPrice).ToString() +
                                              "'where ID='" + window.studentLists[window.borrowBookWindow._selectNameComboBox.SelectedIndex].ID + "'";
                    window.database.Update(query);

                    // insert to MyBooks table
                    foreach (var value in window.borrowBookWindow.selectedBooks) {
                        query = "INSERT INTO MyBooks" +
                            "('StudentID', 'StudentName', 'BookID', 'BookTitle', 'BookDuration', 'BookPrice', 'BorrowedDate', 'ReturnDate')" +
                            "VALUES" +
                            "(@StudentID, @StudentName, @BookID, @BookTitle, @BookDuration, @BookPrice, @BorrowedDate, @ReturnDate)";
                        string[] items = new string[] {
                            window.studentLists[window.borrowBookWindow._selectNameComboBox.SelectedIndex].ID.ToString(),
                            window.studentLists[window.borrowBookWindow._selectNameComboBox.SelectedIndex].Name,
                            value.ID.ToString(),
                            value.Title,
                            value.Duration.ToString(),
                            value.Price.ToString(),
                            DateTime.Now.ToLongDateString(),
                            DateTime.Now.AddDays(+value.Duration).ToLongDateString()
                        };
                        string[] location = new string[] {
                            "@StudentID","@StudentName", "@BookID", "@BookTitle", "@BookDuration", "@BookPrice", "@BorrowedDate", "@ReturnDate"
                        };
                        window.database.Insert(query, items, location, "MyBooks");
                    }
                    window.borrowBookWindow._selectedBooksListView.ItemsSource = null;
                    window.borrowBookWindow.selectedBooks.Clear();
                    window.borrowBookWindow._selectNameComboBox.SelectedIndex = -1;
                    MessageBox.Show("Changed: " + (float.Parse(_enterPaymentTextBox.Text) - totalPrice) + "\nSuccessfully borrowed the books, Thank you!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    window.borrowBookWindow._totalPriceLabel.Text = "0";
                    this.Close();
                }
                else {
                    MessageBox.Show("Insufficient amount", "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }catch(Exception) {  }
        }

        private void EnterPaymentTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            Regex regex = new Regex("[^.0-9+]");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
