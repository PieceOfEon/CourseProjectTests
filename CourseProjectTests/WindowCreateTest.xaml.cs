using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.PortableExecutable;
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

namespace CourseProjectTests
{
    /// <summary>
    /// Interaction logic for WindowCreateTest.xaml
    /// </summary>

    public partial class WindowCreateTest : Window
    {
        private string connect = @"Data Source = DESKTOP-JA41I9L; Initial Catalog = CourseProjectTests; Trusted_connection=True";

        List<string> ListPackItems = new List<string>();
        public WindowCreateTest()
        {
            InitializeComponent();
            ListPackAdd();
            //MessageBox.Show( ListPackItems.Count.ToString());
        }

        private void ConfirmNamePack_Click(object sender, RoutedEventArgs e)
        {
            if (PackNameTextBox.Text != "")
            {
                string sqlExpression = "SELECT * FROM Test";
                string str = "INSERT INTO Test(names) ";
                using (SqlConnection connection = new SqlConnection(connect))
                {
                    string str2 = str + "VALUES('" + PackNameTextBox.Text + "')";
                    //MessageBox.Show(str2);
                    //открываем подклчение
                    connection.Open();

                    SqlCommand command = new SqlCommand(str2, connection);

                    int num = command.ExecuteNonQuery();
                    
                }
                ListPackAdd();

            }
        }
        private void ListPackAdd()
        {
            if(ListPackItems.Count>0)
            {
                ListPackItems.Clear();
                ListPack.ItemsSource = null;
            }
            using (SqlConnection connection = new SqlConnection(connect))
            {
                try
                {
                    string sqlExpression = "SELECT * FROM Test";
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        //string s1 = reader.GetName(3);
                        //string s2 = reader.GetName(2);
                        while (reader.Read())
                        {
                            object name = reader.GetValue(1);
                            
                            ListPackItems.Add(name.ToString());          
                        }
                        
                        
                        reader.Close();
                    }

                }
                catch (Exception s) { MessageBox.Show(s.Message); };
            }
            ListPack.ItemsSource = ListPackItems;
        }

        private void ConfirmQuestion_Click(object sender, RoutedEventArgs e)
        {
            if(VoprosTextBoxOne.Text!="")
            {
                string sqlExpression = "SELECT * FROM Vopros";
                string str = "INSERT INTO Vopros(voprosik) ";
                using (SqlConnection connection = new SqlConnection(connect))
                {
                    string str2 = str + "VALUES('" + VoprosTextBoxOne.Text + "')";
                    //MessageBox.Show(str2);
                    //открываем подклчение
                    connection.Open();

                    SqlCommand command = new SqlCommand(str2, connection);

                    int num = command.ExecuteNonQuery();

                }
            }
          
        }
    }
}
