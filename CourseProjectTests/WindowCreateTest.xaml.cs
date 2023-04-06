using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CourseProjectTests
{
    /// <summary>
    /// Interaction logic for WindowCreateTest.xaml
    /// </summary>
    
    public partial class WindowCreateTest : Window
    {
        private string connect = @"Data Source = DESKTOP-JA41I9L; Initial Catalog = CourseProjectTests; Trusted_connection=True";

        List<string> ListPackItems = new List<string>();
        List<string> CompletedListPackItems = new List<string>();
        private string imagePath="";
        private string endTest = "";
        public WindowCreateTest()
        {
            InitializeComponent();
            ListPackAdd();
            CompletedPacks();
            DelEmpty.IsChecked = true;
        }
        private void ConfirmNamePack_Click(object sender, RoutedEventArgs e)
        {
            if (PackNameTextBox.Text != "")
            {   
                //Проверка на одинаковое название
                using (SqlConnection connection = new SqlConnection(connect))
                {
                    try
                    {
                        string sqlExpression2 = "SELECT * FROM Test";
                        connection.Open();
                        SqlCommand command = new SqlCommand(sqlExpression2, connection);
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                object name = reader.GetValue(1);
                                if (name.ToString().ToLower() == PackNameTextBox.Text.ToLower())
                                {
                                    MessageBox.Show("Уже есть такое название пака");
                                    return;
                                }
                            }
                            reader.Close();
                        }
                    }
                    catch (Exception s) { MessageBox.Show(s.Message); };
                }
                //добавление пака
                string sqlExpression = "SELECT * FROM Test";
                string str = "INSERT INTO Test(names) ";
                using (SqlConnection connection = new SqlConnection(connect))
                {
                    string str2 = str + "VALUES('" + PackNameTextBox.Text + "')";
                    connection.Open();
                    SqlCommand command = new SqlCommand(str2, connection);
                    int num = command.ExecuteNonQuery(); 
                }
                //добавление пака в лист
                ListPackAdd();
            }
        }
        private void CompletedPacks()
        {
            if (CompletedListPackItems.Count > 0)
            {
                CompletedListPackItems.Clear();
                completedpacksBox.ItemsSource = null;
            }
            using (SqlConnection connection = new SqlConnection(connect))
            {
                try
                {
                    string sqlExpression = "SELECT * FROM Test where ended = 1";
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            object name = reader.GetValue(1);

                            CompletedListPackItems.Add(name.ToString());
                        }

                        reader.Close();
                    }
                }
                catch (Exception s) { MessageBox.Show(s.Message); };
            }
            completedpacksBox.ItemsSource = CompletedListPackItems;
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
                    string sqlExpression = "SELECT * FROM Test where ended = 0";
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
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
        //Очищаем поля после успешного добавления вопроса
        private void ClearAllStr()
        {
            BoolQuestionFour.Text = "";
            BoolQuestionThird.Text = "";
            BoolQuestionTwo.Text = "";
            BoolQuestionOne.Text = "";
            imagePath = "";
            VoprosTextBoxOne.Text = "";
            QuestionText.Text = "";
            VoprosTextBoxTwo.Text = "";
            VoprosTextBoxThird.Text = "";
            VoprosTextBoxFour.Text = "";
            
        }
        private void ConfirmQuestion_Click(object sender, RoutedEventArgs e)
        {
            string IDVoprosa="";
            string IDTesta = "";
            if(BoolQuestionFour.Text!="" && BoolQuestionThird.Text!="" && BoolQuestionTwo.Text!="" 
                && BoolQuestionOne.Text!="" && imagePath.ToString()!="" && VoprosTextBoxOne.Text!="" 
                && QuestionText.Text!="" && VoprosTextBoxTwo.Text!="" 
                && VoprosTextBoxThird.Text!="" && VoprosTextBoxFour.Text!="" && ListPack.Text!="")
            {
                //Добавляем вопрос в пак вопросов
                //string sqlExpression = "SELECT * FROM Vopros";
                string str = "INSERT INTO Vopros (voprosik, Picture, variant1, variant2, variant3, variant4)\r\n" +
                    "VALUES ('"+ QuestionText.Text +"', \r\n        " +
                    "(SELECT BulkColumn FROM Openrowset( Bulk '"+ imagePath.ToString() + "', Single_Blob) as Image), \r\n" +
                    "        '"+ BoolQuestionOne.Text + "', '"+ BoolQuestionTwo.Text + "', '"+ BoolQuestionThird.Text + "', '"+ BoolQuestionFour.Text + "'); ";
                using (SqlConnection connection = new SqlConnection(connect))
                {
                    string str2 = str;
                    connection.Open();
                    SqlCommand command = new SqlCommand(str2, connection);
                    int num = command.ExecuteNonQuery();

                }

                //Получаем ID созданного вопроса
                using (SqlConnection connection = new SqlConnection(connect))
                {
                    try
                    {
                        string sqlExpression3 = "SELECT * FROM Vopros";
                        connection.Open();
                        SqlCommand command = new SqlCommand(sqlExpression3, connection);
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            //string s1 = reader.GetName(3);
                            //string s2 = reader.GetName(2);
                            while (reader.Read())
                            {
                                object id = reader.GetValue(0);
                                object name = reader.GetValue(1);
                                if (name.ToString().ToLower() == QuestionText.Text.ToLower())
                                {

                                    IDVoprosa = id.ToString();
                                    //MessageBox.Show(IDVoprosa + "\t" + name.ToString() +"\t"+QuestionText.Text.ToLower());
                                }
                            }
                            reader.Close();
                        }
                    }
                    catch (Exception s) { MessageBox.Show(s.Message); };
                }

                //Добавляем ответы в виде стринги в таблицу ОтветСтринг и передаем ID созданного вопроса
                string str3 = "insert into OtvetString(variantString1, variantString2, variantString3, variantString4,voprosID) " +
                    "values('" + VoprosTextBoxOne.Text + "', '" + VoprosTextBoxTwo.Text + "', '" + VoprosTextBoxThird.Text + "', '" + VoprosTextBoxFour.Text+ "', " + IDVoprosa + ")";
                using (SqlConnection connection = new SqlConnection(connect))
                {
                    string str2 = str3;
                    connection.Open();
                    SqlCommand command = new SqlCommand(str2, connection);
                    int num = command.ExecuteNonQuery();

                }

                //Получаем ID пакета вопросов которого мы выбрали для записи вопроса
                using (SqlConnection connection = new SqlConnection(connect))
                {
                    try
                    {
                        string sqlExpression3 = "SELECT * FROM Test";
                        connection.Open();
                        SqlCommand command = new SqlCommand(sqlExpression3, connection);
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            //string s1 = reader.GetName(3);
                            //string s2 = reader.GetName(2);
                            while (reader.Read())
                            {
                                object id = reader.GetValue(0);
                                object name = reader.GetValue(1);
                                if (name.ToString().ToLower() == ListPack.Text.ToLower())
                                {
                                    IDTesta = id.ToString();
                                }
                            }
                            reader.Close();
                        }
                    }
                    catch (Exception s) { MessageBox.Show(s.Message); };
                }
                //присваиваем endTest значение ТРУ и передаем его в БД для маркировки завершенного теста
                //чтоб он больше не отображался в листе не завершенных паков тестов
                if(endTest=="1")
                {
                    string strEnded = "UPDATE Test\r\nSET ended = 1\r\nWHERE id = "+ IDTesta+"; ";
                    using (SqlConnection connection = new SqlConnection(connect))
                    {
                        string str2 = strEnded;
                        connection.Open();
                        SqlCommand command = new SqlCommand(str2, connection);
                        int num = command.ExecuteNonQuery();
                    }
                    ListPackAdd();
                }
                //Добавление ID теста и вопроса в пак
                string str4 = "insert into Pack(testID, voprosID) " +
                    "values(" +IDTesta + ", " + IDVoprosa + ")";
                using (SqlConnection connection = new SqlConnection(connect))
                {

                    string str2 = str4;
                    connection.Open();
                    SqlCommand command = new SqlCommand(str2, connection);
                    int num = command.ExecuteNonQuery();

                }
                ClearAllStr();
                CompletedPacks();
            }
            else
            {
                MessageBox.Show("Все поля обязательны для заполнения");
            }  
        }
        private void ExportImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Изображения (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png|Все файлы (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(openFileDialog.FileName);
                bitmap.EndInit();
                Images.Source = bitmap;
            }
            imagePath = ((BitmapImage)Images.Source).UriSource.LocalPath;
        }
        private void EndedTestCheck_Checked(object sender, RoutedEventArgs e)
        {
            endTest = "1";
        }
        private void EndedTestCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            endTest = "0";
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
        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            DelPuck();
        }
        private void DelPuck()
        {
            if(DelEmpty.IsChecked==true)
            {
                if (ListPack.Text != "")
                {
                    // id теста по имени теста
                    string testName = ListPack.Text;
                    int testId = 0;
                    using (SqlConnection connection = new SqlConnection(connect))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand("SELECT id FROM Test WHERE names = @testName", connection))
                        {
                            command.Parameters.AddWithValue("@testName", testName);
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    testId = reader.GetInt32(0);
                                }
                            }
                        }
                    }

                    // Удаление связанных записей в таблице Pack
                    using (SqlConnection connection = new SqlConnection(connect))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand("DELETE FROM Pack WHERE testID = @testId", connection))
                        {
                            command.Parameters.AddWithValue("@testId", testId);
                            command.ExecuteNonQuery();
                        }
                    }

                    // Удаление связанных записей в таблице Vopros
                    using (SqlConnection connection = new SqlConnection(connect))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand("DELETE FROM Vopros WHERE id IN (SELECT voprosID FROM Pack WHERE testID = @testId)", connection))
                        {
                            command.Parameters.AddWithValue("@testId", testId);
                            command.ExecuteNonQuery();
                        }
                    }

                    // Удаление связанных записей в таблице OtvetString
                    using (SqlConnection connection = new SqlConnection(connect))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand("DELETE FROM OtvetString WHERE voprosID IN (SELECT voprosID FROM Pack WHERE testID = @testId)", connection))
                        {
                            command.Parameters.AddWithValue("@testId", testId);
                            command.ExecuteNonQuery();
                        }
                    }

                    // Удаление записи в таблице Test
                    using (SqlConnection connection = new SqlConnection(connect))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand("DELETE FROM Test WHERE id = @testId", connection))
                        {
                            command.Parameters.AddWithValue("@testId", testId);
                            command.ExecuteNonQuery();
                        }
                    }
                    ListPackAdd();
                }
            }
            if (DelEnded.IsChecked == true)
            {
                if (completedpacksBox.Text != "")
                {
                    // id теста по имени теста
                    string testName = completedpacksBox.Text;
                    int testId = 0;
                    using (SqlConnection connection = new SqlConnection(connect))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand("SELECT id FROM Test WHERE names = @testName", connection))
                        {
                            command.Parameters.AddWithValue("@testName", testName);
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    testId = reader.GetInt32(0);
                                }
                            }
                        }
                    }

                    // Удаление связанных записей в таблице Pack
                    using (SqlConnection connection = new SqlConnection(connect))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand("DELETE FROM Pack WHERE testID = @testId", connection))
                        {
                            command.Parameters.AddWithValue("@testId", testId);
                            command.ExecuteNonQuery();
                        }
                    }

                    // Удаление связанных записей в таблице Vopros
                    using (SqlConnection connection = new SqlConnection(connect))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand("DELETE FROM Vopros WHERE id IN (SELECT voprosID FROM Pack WHERE testID = @testId)", connection))
                        {
                            command.Parameters.AddWithValue("@testId", testId);
                            command.ExecuteNonQuery();
                        }
                    }

                    // Удаление связанных записей в таблице OtvetString
                    using (SqlConnection connection = new SqlConnection(connect))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand("DELETE FROM OtvetString WHERE voprosID IN (SELECT voprosID FROM Pack WHERE testID = @testId)", connection))
                        {
                            command.Parameters.AddWithValue("@testId", testId);
                            command.ExecuteNonQuery();
                        }
                    }

                    // Удаление записи в таблице Test
                    using (SqlConnection connection = new SqlConnection(connect))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand("DELETE FROM Test WHERE id = @testId", connection))
                        {
                            command.Parameters.AddWithValue("@testId", testId);
                            command.ExecuteNonQuery();
                        }
                    }
                    CompletedPacks();
                }
            }
        }
    }
}
