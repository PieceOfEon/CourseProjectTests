using System.Windows;
using System.Windows.Input;


namespace CourseProjectTests
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void ButtonTeacherReg_Click(object sender, RoutedEventArgs e)
        {
            WindowRegistrationAndConnect windowRegistrationAndConnect = new WindowRegistrationAndConnect(true);
            windowRegistrationAndConnect.ShowDialog();
            
        }
        private void ButtonStudentReg_Click(object sender, RoutedEventArgs e)
        {
            WindowRegistrationAndConnect windowRegistrationAndConnect = new WindowRegistrationAndConnect(false);
            windowRegistrationAndConnect.ShowDialog();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
