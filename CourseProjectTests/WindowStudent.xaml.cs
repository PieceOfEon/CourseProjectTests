using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
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
    /// Interaction logic for WindowStudent.xaml
    /// </summary>
    public partial class WindowStudent : Window
    {
        private string connect = @"Data Source = DESKTOP-JA41I9L; Initial Catalog = CourseProjectTests; Trusted_connection=True";
        List<string> ListTestPackItems = new List<string>();
        List<Pack> packs = new List<Pack>();

        int kolTest = 0;
        public WindowStudent()
        {
            InitializeComponent();
            TestPack();
            PrevButton.Visibility = Visibility.Hidden;
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
                kolTest = 0;
                packs.Clear();
                string IDVoprosa = "";
                //Question.Text="ALOOO";
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
                            //string s1 = reader.GetName(3);
                            //string s2 = reader.GetName(2);
                            while (reader.Read())
                            {
                                object id = reader.GetValue(0);
                                object name = reader.GetValue(1);
                                if (name.ToString().ToLower() == PackTests.Text.ToLower())
                                {

                                    IDVoprosa = id.ToString();
                                    //MessageBox.Show(IDVoprosa + "\t" + name.ToString() + "\t" + PackTests.Text.ToLower());
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
                        //string s1 = reader.GetName(3);
                        //string s2 = reader.GetName(2);
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

                            string var1 = reader.GetValue(7).ToString();
                            string var2 = reader.GetValue(8).ToString();
                            string var3 = reader.GetValue(9).ToString();
                            string var4 = reader.GetValue(10).ToString();


                            //lab.Content = id + "\t" + vopros + "\t" + BitObj + "\t" + var1 + "\t" + var2 + "\t" + var3 + "\t" + var4 + "\t" + varBool1 + "\t" + varBool2 + "\t" + varBool3 + "\t" + varBool4;
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




            }
        }
        private void CheckCountVOpros()
        {
            if (packs.Count == 1)
            {
                NextButton.Visibility = Visibility.Hidden;
                PrevButton.Visibility = Visibility.Hidden;
            }

            if (packs.Count > 1)
            {
                NextButton.Visibility = Visibility.Visible;
                PrevButton.Visibility = Visibility.Hidden;
            }
        }
        private void NextVopros()
        {
            kolTest++;
            if (kolTest >= packs.Count - 1)
            {
                NextButton.Visibility = Visibility.Hidden;
                PrevButton.Visibility = Visibility.Visible;
                kolTest = packs.Count - 1;
            }
            if (kolTest < packs.Count)
            {
                PrevButton.Visibility = Visibility.Visible;
                Question.Text = packs[kolTest].vopros;

                variant1.Content = packs[kolTest].otvet1;
                variant2.Content = packs[kolTest].otvet2;
                variant3.Content = packs[kolTest].otvet3;
                variant4.Content = packs[kolTest].otvet4;

                PictureImage.Source = packs[kolTest].Imges;

            }
        }
        private void PrevVopros()
        {
            kolTest--;
            if (kolTest <= 0)
            {
                PrevButton.Visibility = Visibility.Hidden;
                NextButton.Visibility = Visibility.Visible;
                kolTest = 0;
            }
            if (kolTest < packs.Count)
            {
                NextButton.Visibility = Visibility.Visible;
                Question.Text = packs[kolTest].vopros;

                variant1.Content = packs[kolTest].otvet1;
                variant2.Content = packs[kolTest].otvet2;
                variant3.Content = packs[kolTest].otvet3;
                variant4.Content = packs[kolTest].otvet4;

                PictureImage.Source = packs[kolTest].Imges;

            }
        }
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            NextVopros();
        }

        private void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            PrevVopros();
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
