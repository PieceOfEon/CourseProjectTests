using System;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;

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
            byte[] hashedPassword = HashPassword(PassH.Password);
            string hashedPasswordString = Convert.ToBase64String(hashedPassword);

            using (SqlConnection connection = new SqlConnection(connect))
            {
                string str2 = str + "VALUES('" + UsernameH.Text + "', '" + hashedPasswordString + "', '" + teacher + "')";
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
                        while (reader.Read())
                        {
                            object nik = reader.GetValue(1);
                            object pass = reader.GetValue(2);
                            
                            if (UsernameH.Text.ToLower() == nik.ToString().ToLower())
                            {
                                string password = PassH.Password.ToString();
                                byte[] hashedPassword = HashPassword(password);
                                byte[] storedPassword = Convert.FromBase64String(pass.ToString()); // преобразование строки в массив байт
                                //MessageBox.Show(pass.ToString() + " Log" + nik+"\n"+PassH.Password.ToString()+"\tLog2\t"+UsernameH.Text);
                                //if (PassH.Password.ToString()!=pass.ToString())
                                //{
                                //    MessageBox.Show("Incorrect Password, try again");
                                //}
                                if (!hashedPassword.SequenceEqual(storedPassword))
                                {
                                    MessageBox.Show("Incorrect Password, try again");
                                }
                                else
                                {
                                   
                                    if (teacher == true)
                                    {
                                        bool teachorstudent = (bool)reader.GetValue(3);

                                        if (teachorstudent == true)
                                        {
                                            if (hashedPassword.SequenceEqual(storedPassword))
                                            {
                                                MessageBox.Show("Successfully");
                                                WindowCreateTest windowCreateTest = new WindowCreateTest();
                                                windowCreateTest.ShowDialog();
                                                reader.Close();
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Вход разрешен только учителям");
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        bool teachorstudent = (bool)reader.GetValue(3);
                                        if (teachorstudent == false)
                                        {
                                            if (hashedPassword.SequenceEqual(storedPassword))
                                            {
                                                MessageBox.Show("Successfully");
                                                WindowStudent windowStudent = new WindowStudent();
                                                windowStudent.ShowDialog();
                                                reader.Close();
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Вход только для студентов");
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

        private byte[] HashPassword(string password)
        {
            byte[] passwordBytes = Encoding.Unicode.GetBytes(password);

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(passwordBytes);
                return hash;
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
