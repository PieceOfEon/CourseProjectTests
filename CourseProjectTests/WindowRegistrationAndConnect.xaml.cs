using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using static Azure.Core.HttpHeader;

namespace CourseProjectTests
{
    /// <summary>
    /// Interaction logic for WindowRegistrationAndConnect.xaml
    /// </summary>
    public partial class WindowRegistrationAndConnect : Window
    {
        private string connect = @"Data Source = DESKTOP-JA41I9L; Initial Catalog = CourseProjectTests; Trusted_connection=True";
        private string str = "";
        //private string sqlExpression = "SELECT * FROM Registration";
        private bool teacher;
        public WindowRegistrationAndConnect(bool tiBool)
        {
            InitializeComponent();
            teacher = tiBool;
            str = "INSERT INTO Registration(Nickname, Pass, Teacher) ";
        }

        private async void ButtonRegAndConnect_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connect))
            {
                string str2 = str + "VALUES('" + UsernameH.Text + "', '" + PassH.Password.ToString() + "', '" + teacher + "')";
                MessageBox.Show(str2);
                //открываем подклчение
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(str2, connection);

                int num = await command.ExecuteNonQueryAsync();
                //Console.WriteLine($"Добавлено объектов {num}");
            }
            Console.Read();
            //str2 = "";
        }
    }
}
