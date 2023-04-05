﻿using ControlzEx.Standard;
using Microsoft.Win32;
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
using static System.Net.Mime.MediaTypeNames;

namespace CourseProjectTests
{
    /// <summary>
    /// Interaction logic for WindowCreateTest.xaml
    /// </summary>
    
    public partial class WindowCreateTest : Window
    {
        private string connect = @"Data Source = DESKTOP-JA41I9L; Initial Catalog = CourseProjectTests; Trusted_connection=True";

        List<string> ListPackItems = new List<string>();

        private string imagePath="";
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
            //LabelTrue.Content = BoolQuestionOne.Text;
            //MessageBox.Show("Otvet4БУЛ->" +BoolQuestionFour.Text + "Otvet3БУЛ->" + BoolQuestionThird.Text + "Otvet2БУЛ->" + BoolQuestionTwo.Text 
            //    +
            //     "Otvet1БУЛ->" + BoolQuestionOne.Text + "картинка->" + imagePath.ToString()
            //     +
            //     "Вопрос->" + QuestionText.Text + "Ответ1->" + VoprosTextBoxOne.Text + "Ответ2->" + VoprosTextBoxTwo.Text +
            //     "Ответ3->" + VoprosTextBoxThird.Text + "Ответ4->" + VoprosTextBoxFour.Text);
            string IDVoprosa="";
            if(BoolQuestionFour.Text!="" && BoolQuestionThird.Text!="" && BoolQuestionTwo.Text!="" 
                && BoolQuestionOne.Text!="" && imagePath.ToString()!="" && VoprosTextBoxOne.Text!="" 
                && QuestionText.Text!="" && VoprosTextBoxTwo.Text!="" 
                && VoprosTextBoxThird.Text!="" && VoprosTextBoxFour.Text!="")
            {
                //string sqlExpression = "SELECT * FROM Vopros";
                string str = "INSERT INTO Vopros (voprosik, Picture, variant1, variant2, variant3, variant4)\r\n" +
                    "VALUES ('"+ QuestionText.Text +"', \r\n        " +
                    "(SELECT BulkColumn FROM Openrowset( Bulk '"+ imagePath.ToString() + "', Single_Blob) as Image), \r\n" +
                    "        '"+ BoolQuestionOne.Text + "', '"+ BoolQuestionTwo.Text + "', '"+ BoolQuestionThird.Text + "', '"+ BoolQuestionFour.Text + "'); ";
                using (SqlConnection connection = new SqlConnection(connect))
                {
                    string str2 = str;
                    //MessageBox.Show(str2);
                    //открываем подклчение
                    connection.Open();

                    SqlCommand command = new SqlCommand(str2, connection);

                    int num = command.ExecuteNonQuery();

                }


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
                //MessageBox.Show(IDVoprosa);
                //string sqlExpression2 = "SELECT * FROM Vopros";
                string str3 = "insert into OtvetString(variantString1, variantString2, variantString3, variantString4,voprosID) " +
                    "values('" + VoprosTextBoxOne.Text + "', '" + VoprosTextBoxTwo.Text + "', '" + VoprosTextBoxThird.Text + "', '" + VoprosTextBoxFour.Text+ "', " + IDVoprosa + ")";
                using (SqlConnection connection = new SqlConnection(connect))
                {
                    
                    string str2 = str3;
                    //MessageBox.Show(str2);
                    //открываем подклчение
                    connection.Open();

                    SqlCommand command = new SqlCommand(str2, connection);

                    int num = command.ExecuteNonQuery();

                }
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
            LabelTestSource.Content = imagePath;
        }
    }
}
