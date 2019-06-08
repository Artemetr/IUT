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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CPark
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        car[] cars = new car[19];

        public MainWindow()
        {
            InitializeComponent();
            list_notifications.Items.Add("Загрузка...");
            list_notifications.Items.Add("Программа успешно загружена и готова к использованию!");
            for (int i = 0; i < 19; i++)
            {
                cars[i] = new car();
                cars[i].id = i + 1;
            }
            getCar();
            updatePanel();
            //r1.Fill = new SolidColorBrush(Color.FromArgb(100, 255, 0, 0));
        }

        public string get_cs()
        {
            return "Data Source=123456; Initial Catalog = Garage; User ID = sa; Password = 123456";
        }

        private void btn_out_Click(object sender, RoutedEventArgs e)
        {
            string regNumber = textBox.Text;
            if (regNumber.Length < 6)
            {
                MessageBox.Show("Некорректный ввод регистрационного номера машины!");
                return;
            }
            bool bl = false;
            for (int i = 0; i < 19; i++)
            {
                if (cars[i].regNumber == regNumber)
                {
                    if(cars[i].onf == 0)
                    {
                        list_notifications.Items.Add("ERROR: Машина с регистрационным номером \"" + regNumber + "\" не находится на территории стоянки.");
                        bl = true;
                        break;
                    }
                    cars[i].onf = 0;
                    list_notifications.Items.Add("Машина с регистрационным номером \"" + regNumber + "\" покинула стоянку.");
                    list_notifications.Items.Add("Число выездов для облегчения проезда: " + cars[i].q + ".");
                    bl = true;
                    break;
                }
                if (cars[i].onf == 1) cars[i].q = cars[i].q + 1;
            }
            if (bl)
            {                
                setCar();
                updatePanel();
            }
            else
            {
                MessageBox.Show("Машины с указанным номером не существует!");
            }
        }

        class car
        {
            public int id;
            public string regNumber;
            public int onf;
            public int q;
        }
        void updatePanel()
        {
            getCar();
            int t = 0;
            list_car_out.Items.Clear();
            list_car_in.Items.Clear();
            for (int i = 0; i < 19; i++)
            {
                if (cars[i].regNumber != "      " && cars[i].onf == 1)
                {
                    list_car_in.Items.Add(cars[i].id + "   " + cars[i].regNumber);
                    t++;
                }
                else if (cars[i].regNumber != "      " && cars[i].onf == 0) list_car_out.Items.Add(cars[i].id + "   " + cars[i].regNumber);
            }
            tb_num.Text = "Машин на стоянке " + t.ToString() + "/19";
        }

        void getCar()
        {
            using (var connection = new SqlConnection(get_cs()))
            {
                connection.Open();

                using (var cmd = new SqlCommand("SELECT * FROM [Garage].[dbo].[TableCars]", connection))
                {
                    using (var rd = cmd.ExecuteReader())
                    {
                        int i = 0;
                        while (rd.Read())
                        {                           
                                cars[i].regNumber = rd.GetValue(1).ToString();
                                if (rd.GetValue(2).ToString() == "") cars[i].onf = -1; else cars[i].onf = int.Parse(rd.GetValue(2).ToString());
                                if (rd.GetValue(3).ToString() == "") cars[i].q = -1; else cars[i].q = int.Parse(rd.GetValue(3).ToString());
                            i++;
                        }
                    }
                }

                connection.Close();
            }
        }

        void setCar()
        {
            using (var connection = new SqlConnection(get_cs()))
            {
                string cmdS = null;
                for (int i = 0; i < 19; i++)
                {
                    cmdS += String.Format("UPDATE [Garage].[dbo].[TableCars] SET regNumber = '{0}', onf = {1}, q = {2} WHERE id = {3} ", cars[i].regNumber.ToString(), cars[i].onf.ToString(), cars[i].q.ToString(), (i + 1).ToString());
                }
                connection.Open();
                using (var cmd = new SqlCommand(cmdS, connection))
                {
                    cmd.ExecuteNonQuery();
                }

                connection.Close();
            }           
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            string regNumber = textBox.Text;
            if (regNumber.Length < 6)
            {
                MessageBox.Show("Некорректный ввод регистрационного номера машины!");
                return;
            }
            bool bl = false;
            for(int i = 0; i < 19; i++)
            {
                if (cars[i].regNumber == "      ")
                {
                    cars[i].regNumber = regNumber;
                    cars[i].onf = 0;
                    cars[i].q = 0;
                    list_notifications.Items.Add("Машина с регистрационным номером \"" + regNumber + "\" теперь может пользоваться услугами стоянки.");
                    bl = true;
                    break;
                }
            }
            if (bl)
            {
                setCar();
                getCar();
                updatePanel();
            } else
            {
                MessageBox.Show("Отсутствуют свободные места!");
            }
        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            string regNumber = textBox.Text;
            if (regNumber.Length < 6)
            {
                MessageBox.Show("Некорректный ввод регистрационного номера машины!");
                return;
            }
            bool bl = false;
            for (int i = 0; i < 19; i++)
            {
                if (cars[i].regNumber == regNumber)
                {
                    cars[i].regNumber = "      ";
                    cars[i].onf = -1;
                    cars[i].q = -1;
                    list_notifications.Items.Add("Машина с регистрационным номером \"" + regNumber + "\" больше не может пользоваться услугами стоянки.");
                    bl = true;
                    break;
                }
            }
            if (bl)
            {
                setCar();
                getCar();
                updatePanel();
            }
            else
            {
                MessageBox.Show("Машины с указанным номером не существует!");
            }
        }

        private void btn_in_Click(object sender, RoutedEventArgs e)
        {
            string regNumber = textBox.Text;
            if (regNumber.Length < 6)
            {
                MessageBox.Show("Некорректный ввод регистрационного номера машины!");
                return;
            }
            bool bl = false;
            for (int i = 0; i < 19; i++)
            {
                if (cars[i].regNumber == regNumber)
                {
                    if (cars[i].onf == 1)
                    {
                        MessageBox.Show("Машина с указанным регистрационным номером уже находится на стоянке!");
                        bl = true;
                        break;
                    }
                    cars[i].onf = 1;
                    list_notifications.Items.Add("Машина с регистрационным номером \"" + regNumber + "\" прибыла на стоянку.");
                    bl = true;
                    break;
                }
            }
            if (bl)
            {
                setCar();
                getCar();
                updatePanel();
            }
            else
            {
                MessageBox.Show("Машины с указанным номером не существует!");
            }
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            setCar();
            getCar();
            MessageBox.Show("Сохранение текущей конфигурации стоянки выполнено успешно!");
        }

        private void btn_clear_Click(object sender, RoutedEventArgs e)
        {
            using (var connection = new SqlConnection(get_cs()))
            {
                string cmdS = null;
                for (int i = 0; i < 19; i++)
                {
                    cmdS += String.Format("UPDATE [Garage].[dbo].[TableCars] SET regNumber = '{0}', onf = {1}, q = {2} WHERE id = {3} ", null, -1, -1, (i + 1).ToString());
                }
                connection.Open();
                using (var cmd = new SqlCommand(cmdS, connection))
                {
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
            setCar();
            getCar();
            updatePanel();
            MessageBox.Show("Текущая конфигурация стоянки отчищена успешно!");
        }

        private void list_car_in_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int index = this.list_car_in.SelectedIndex;
            string[] str = list_car_in.Items[index].ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            textBox.Text = str[1];
        }

        private void list_car_out_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int index = this.list_car_out.SelectedIndex;
            string[] str = list_car_out.Items[index].ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            textBox.Text = str[1];
        }
    }

}
