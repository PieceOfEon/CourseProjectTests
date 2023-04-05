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
        private bool teacher;
        
        string sqlExpression = "SELECT * FROM Registration";
        public WindowRegistrationAndConnect(bool tiBool)
        {
            InitializeComponent();
            teacher = tiBool;
            str = "INSERT INTO Registration(Nickname, Pass, Teacher) ";
        }

        private async void ButtonRegAndConnect_Click(object sender, RoutedEventArgs e)
        {
            if (CheckUpIn.IsChecked == true)
            {
                ProverkaAutorization();
                // Выполняется вход
            }
            else
            {
                ProverkaRegi();
                // Выполняется registraciya
            }
            
        }
        private void Registration()
        {
            using (SqlConnection connection = new SqlConnection(connect))
            {
                string str2 = str + "VALUES('" + UsernameH.Text + "', '" + PassH.Password.ToString() + "', '" + teacher + "')";
                //MessageBox.Show(str2);
                //открываем подклчение
                connection.Open();

                SqlCommand command = new SqlCommand(str2, connection);

                int num = command.ExecuteNonQuery();
            }
            
        }

        private  void ProverkaAutorization()
        {
            if (UsernameH.Text == "" || PassH.Password.ToString() == "")
            {

                MessageBox.Show("Поля не могут быть пустыми");
                return;
            }
            int kolProverka = 0;
            using (SqlConnection connection = new SqlConnection(connect))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        //string s1 = reader.GetName(3);
                        //string s2 = reader.GetName(2);
                        while (reader.Read())
                        {
                            object nik = reader.GetValue(1);
                            object pass = reader.GetValue(2);
                            
                            if (UsernameH.Text.ToLower() == nik.ToString().ToLower())
                            {
                                //MessageBox.Show(pass.ToString() + " Log" + nik+"\n"+PassH.Password.ToString()+"\tLog2\t"+UsernameH.Text);
                                if (PassH.Password.ToString()!=pass.ToString())
                                {
                                    MessageBox.Show("Incorrect Password, try again");
                                }
                                else
                                {
                                    
                                    
                                    if(teacher==true)
                                    {
                                        bool teachorstudent = (bool)reader.GetValue(3);
                                        if (teachorstudent == true)
                                        {
                                            MessageBox.Show("Successfully");
                                            WindowCreateTest windowCreateTest = new WindowCreateTest();
                                            windowCreateTest.ShowDialog();
                                            reader.Close();
                                            return;
                                        }
                                        else
                                        {
                                            MessageBox.Show("Пёс, не быть тебе учителем!");
                                            return;
                                        }                                    
                                    }
                                    else
                                    {
                                        bool teachorstudent = (bool)reader.GetValue(3);
                                        if (teachorstudent == false)
                                        {
                                            MessageBox.Show("Successfully");
                                            WindowStudent windowStudent = new WindowStudent();
                                            windowStudent.ShowDialog();
                                            reader.Close();
                                            return;

                                        }
                                        else
                                        {
                                            MessageBox.Show("Учитель должен учить а не учиться");
                                            return;
                                        }                                
                                    }       
                                }
                            }
                        }
                    }
                }
                catch (Exception e) { MessageBox.Show(e.Message); };

            }
        }
         void ProverkaRegi()
        {
            if (UsernameH.Text == "" || PassH.Password.ToString() == "")
            {

                MessageBox.Show("Поля не могут быть пустыми");
                return;
            }
            int kolProverka = 0;
            using (SqlConnection connection = new SqlConnection(connect))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        //string s1 = reader.GetName(3);
                        //string s2 = reader.GetName(2);
                        while (reader.Read())
                        {
                            object nik = reader.GetValue(1);                            
                            //MessageBox.Show(nik.ToString());
                            if (UsernameH.Text.ToLower() == nik.ToString().ToLower())
                            {
                                kolProverka++;
                                MessageBox.Show("Этот ник занят. Попробуйте другой.");
                                return;
                            }
                        
                        }
                        if (kolProverka > 0)
                        {
                            return;
                        }
                        else if (kolProverka == 0)
                        {
                            MessageBox.Show("Successfully");
                            Registration();
                        }

                        reader.Close();
                    }

                }
                catch (Exception e) { MessageBox.Show(e.Message); };

            }
        }

        private void CheckUpIn_Checked(object sender, RoutedEventArgs e)
        {
            ButtonRegAndConnect.Content = "Sign In";
        }

        private void CheckUpIn_Unchecked(object sender, RoutedEventArgs e)
        {
            ButtonRegAndConnect.Content = "Sign Up";
        }
    }
}
