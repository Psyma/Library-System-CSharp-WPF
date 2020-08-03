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
    /// Interaction logic for ViewLoggingLogs.xaml
    /// </summary>
    public partial class ViewLoggingLogs : Window {
        public ViewLoggingLogs() {
            InitializeComponent();
        }
        private MainWindow window = Application.Current.Windows[0] as MainWindow;
        private void ViewLoggingLogsWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
        public bool clearLoggingLogs = false;
        private void ClearButton_Click(object sender, RoutedEventArgs e) {
            var result = MessageBox.Show("Do you want to clear the logs?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (result) {
                case MessageBoxResult.Yes:
                    window.database.DeleteAll("LoggingLogs");
                    window.loggingLogs.Clear();
                    clearLoggingLogs = true;
                    this.Close();
                    break;
            }
        }
    }
}
