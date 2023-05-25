using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CourseProjectTests
{
    /// <summary>
    /// Interaction logic for WindowStudent.xaml
    /// </summary>
    public partial class WindowStudent : Window
    {
        private string connect = @"Data Source = DESKTOP-JA41I9L; Initial Catalog = CourseProjectTests; Trusted_connection=True";
        List<string> ListTestPackItems = new List<string>();
        List<Pack> packs = new List<Pack>();

        int kolTest = 0;
        int kolBallov = 0;
        int kolStudBall = 0;
        public WindowStudent()
        {
            InitializeComponent();
            TestPack();
            HiddenInterfaceTest();
            Question.Text = "Добро пожаловать!";
        }
        private void TestPack()
        {
            if (ListTestPackItems.Count > 0)
            {
                ListTestPackItems.Clear();
                PackTests.ItemsSource = null;
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

                            ListTestPackItems.Add(name.ToString());
                        }
                        reader.Close();
                    }
                }
                catch (Exception s) { MessageBox.Show(s.Message); };
            }
            PackTests.ItemsSource = ListTestPackItems;
        }
        private void ConfirmTest_Click(object sender, RoutedEventArgs e)
        {
            if (PackTests.Text != "")
            {
                kolBallov = 0;
                kolStudBall = 0;
                kolTest = 0;
                packs.Clear();
                string IDVoprosa = "";
                //Получаем ID выбранного теста
                using (SqlConnection connection = new SqlConnection(connect))
                {
                    try
                    {
                        string sqlExpression3 = "SELECT * FROM Test where ended = 1";
                        connection.Open();
                        SqlCommand command = new SqlCommand(sqlExpression3, connection);
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                object id = reader.GetValue(0);
                                object name = reader.GetValue(1);
                                if (name.ToString().ToLower() == PackTests.Text.ToLower())
                                {
                                    IDVoprosa = id.ToString();
                                }
                            }
                            reader.Close();
                        } 
                    }
                    catch (Exception s) { MessageBox.Show(s.Message); };
                }

                string sqlExpression4 = "SELECT Vopros.* FROM Pack INNER JOIN Vopros ON Pack.voprosID = Vopros.id WHERE Pack.testID = " + IDVoprosa + ";";
                using (SqlConnection connection = new SqlConnection(connect))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression4, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            object id = reader.GetValue(0);
                            object vopros = reader.GetValue(1);

                            byte[] Images = (byte[])reader.GetValue(2);
                            Stream StreamObj = new MemoryStream(Images);
                            BitmapImage BitObj = new BitmapImage();
                            BitObj.BeginInit();
                            BitObj.StreamSource = StreamObj;
                            BitObj.EndInit();
                            PictureImage.Source = BitObj;

                            object var1 = reader.GetValue(3);
                            object var2 = reader.GetValue(4);
                            object var3 = reader.GetValue(5);
                            object var4 = reader.GetValue(6);

                        }
                        reader.Close();
                    }
                }
                string sqlExpression5 = "SELECT Vopros.*, OtvetString.variantString1, OtvetString.variantString2, OtvetString.variantString3, " +
                    "OtvetString.variantString4 FROM Pack INNER JOIN Vopros ON Pack.voprosID = Vopros.id" +
                    " LEFT JOIN OtvetString ON Vopros.id = OtvetString.voprosID " +
                    "WHERE Pack.testID =" + IDVoprosa + ";";

                using (SqlConnection connection = new SqlConnection(connect))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression5, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            object id = reader.GetValue(0);
                            string vopros = reader.GetValue(1).ToString();
                           
                            byte[] Images = (byte[])reader.GetValue(2);
                            Stream StreamObj = new MemoryStream(Images);
                            BitmapImage BitObj = new BitmapImage();
                            BitObj.BeginInit();
                            BitObj.StreamSource = StreamObj;
                            BitObj.EndInit();
                            PictureImage.Source = BitObj;

                            bool varBool1 = (bool)reader.GetValue(3);
                            bool varBool2 = (bool)reader.GetValue(4);
                            bool varBool3 = (bool)reader.GetValue(5);
                            bool varBool4 = (bool)reader.GetValue(6);
 
                            kolBallov++;
                            
                            string var1 = reader.GetValue(7).ToString();
                            string var2 = reader.GetValue(8).ToString();
                            string var3 = reader.GetValue(9).ToString();
                            string var4 = reader.GetValue(10).ToString();

                            packs.Add(new Pack(var1, var2, var3, var4, varBool1, varBool2, varBool3, varBool4, BitObj, vopros));
                        }
                        reader.Close();
                    }
                }
                CheckCountVOpros();

                Question.Text = packs[0].vopros;

                variant1.Content = packs[0].otvet1;
                variant2.Content = packs[0].otvet2;
                variant3.Content = packs[0].otvet3;
                variant4.Content = packs[0].otvet4;

                PictureImage.Source = packs[0].Imges;

                VisibilityInterfaceTest();
                HiddenDownloadTest();
            }
        }
        private void CheckCountVOpros()
        {
            if (packs.Count == 1)
            {
                NextButton.Visibility = Visibility.Hidden;
            }
            if (packs.Count > 1)
            {
                NextButton.Visibility = Visibility.Visible;
            }
        }
        private void Points()
        {
            if (packs[kolTest].bool1 == true)
            {
                if (variant1.IsChecked == true)
                {
                    kolStudBall++;
                }
            }
            if (packs[kolTest].bool2 == true)
            {
                if (variant2.IsChecked == true)
                {
                    kolStudBall++;
                }
            }
            if (packs[kolTest].bool3 == true)
            {
                if(variant3.IsChecked==true)
                {
                    kolStudBall++;
                } 
            }
            if (packs[kolTest].bool4 == true)
            {
                if (variant4.IsChecked == true)
                {
                    kolStudBall++;
                }
            }
        }
        private void NextVopros()
        {
            ConfirmAnswerButton.Visibility = Visibility.Visible;
            kolTest++;
            if (kolTest >= packs.Count - 1)
            {
                NextButton.Visibility = Visibility.Hidden;
                kolTest = packs.Count - 1;
            }
            if (kolTest < packs.Count)
            {
                Question.Text = packs[kolTest].vopros;

                variant1.Content = packs[kolTest].otvet1;
                variant2.Content = packs[kolTest].otvet2;
                variant3.Content = packs[kolTest].otvet3;
                variant4.Content = packs[kolTest].otvet4;

                PictureImage.Source = packs[kolTest].Imges;

                variant1.IsChecked = false;
                variant2.IsChecked = false;
                variant3.IsChecked = false;
                variant4.IsChecked = false;
            }
        }
        private void HiddenDownloadTest()
        {
            CompetedPack.Visibility = Visibility.Hidden;
            PackTests.Visibility = Visibility.Hidden;
            ConfirmTest.Visibility = Visibility.Hidden;
        }
        private void VisibilityDownloadTest()
        {
            CompetedPack.Visibility = Visibility.Visible;
            PackTests.Visibility = Visibility.Visible;
            ConfirmTest.Visibility = Visibility.Visible;
        }
        private void HiddenInterfaceTest()
        {
            variant1.Visibility = Visibility.Hidden;
            variant2.Visibility = Visibility.Hidden;
            variant3.Visibility = Visibility.Hidden;
            variant4.Visibility = Visibility.Hidden;
            ConfirmAnswerButton.Visibility = Visibility.Hidden;
            NextButton.Visibility = Visibility.Hidden;
            Endtest.Visibility = Visibility.Hidden;
            PictureImage.Visibility = Visibility.Hidden;
        }
        private void VisibilityInterfaceTest()
        {
            Question.Visibility = Visibility.Visible;
            variant1.Visibility = Visibility.Visible;
            variant2.Visibility = Visibility.Visible;
            variant3.Visibility = Visibility.Visible;
            variant4.Visibility = Visibility.Visible;
            ConfirmAnswerButton.Visibility = Visibility.Visible;
            Endtest.Visibility = Visibility.Visible;
            PictureImage.Visibility = Visibility.Visible;
        }
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            NextVopros();
        }
        private void ConfirmAnswerButton_Click(object sender, RoutedEventArgs e)
        {
            Points();
            ConfirmAnswerButton.Visibility = Visibility.Hidden;
        }
        private void Endtest_Click(object sender, RoutedEventArgs e)
        {
            Question.Text="Вы заработали: " + kolStudBall.ToString() + " из " + kolBallov.ToString() + " баллов.";
            VisibilityDownloadTest();
            HiddenInterfaceTest();
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
    class Pack
    {
        public Pack(string o1, string o2, string o3, string o4, bool b1, bool b2, bool b3, bool b4, BitmapImage Im, string vop)
        {
            otvet1 = o1;
            otvet2 = o2;
            otvet3= o3;
            otvet4 = o4;

            bool1 = b1;
            bool2 = b2;
            bool3 = b3;
            bool4 = b4;

            Imges = Im;

            vopros = vop;
        }
        
        public string otvet1;
        public string otvet2;
        public string otvet3;
        public string otvet4;

        public bool bool1;
        public bool bool2;
        public bool bool3;
        public bool bool4;

        public BitmapImage Imges;

        public string vopros;

    }
}
