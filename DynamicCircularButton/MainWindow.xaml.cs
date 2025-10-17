using System.Windows;

namespace DynamicCircularButton
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CircularButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Circular button clicked!", "Info",
                          MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}