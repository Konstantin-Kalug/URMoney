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
using System.Windows.Shapes;

namespace URMoney
{
    /// <summary>
    /// Логика взаимодействия для AddTransactionWindow.xaml
    /// </summary>
    public partial class AddTransactionWindow : Window
    {
        public string[] output = new string[2];
        public AddTransactionWindow(DataItemTransaction? dataItem)
        {
            InitializeComponent();
            if (dataItem == null) return;
            titleBox.Text = dataItem.Column2;
            totalBox.Text = dataItem.Column3.ToString();
        }

        void Accept_Click(object sender, RoutedEventArgs e)
        {
            output[0] = titleBox.Text;
            output[1] = totalBox.Text;
            DialogResult = true;
        }
    }
}
