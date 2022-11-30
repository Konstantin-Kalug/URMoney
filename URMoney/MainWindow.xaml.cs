using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace URMoney
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
        // Таблицы Учёта
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            CentralBut.Visibility = Visibility.Visible;
        }
        // Доходы
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Visibility = Visibility.Hidden;
            FrameTables.Visibility = Visibility.Visible;
            expenses.Visibility = Visibility.Hidden;
            income.Visibility = Visibility.Visible;

        }
        // Расходы
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainFrame.Visibility = Visibility.Hidden;
            FrameTables.Visibility = Visibility.Visible;
            expenses.Visibility = Visibility.Visible;
            income.Visibility = Visibility.Hidden;
        }
        //Вернуться на главное окно
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MainFrame.Visibility = Visibility.Visible;
            FrameTables.Visibility = Visibility.Hidden;
            CentralBut.Visibility = Visibility.Hidden;
        }
    }
}
