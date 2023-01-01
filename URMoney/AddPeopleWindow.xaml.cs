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
    /// Логика взаимодействия для AddPeopleWindow.xaml
    /// </summary>
    public partial class AddPeopleWindow : Window
    {
        public string output;
        public AddPeopleWindow(DataItemPeople? human)
        {
            InitializeComponent();
            if (human is null) return;
            nameBox.Text = human.Column2;
        }
        void Accept_Click(object sender, RoutedEventArgs e)
        {
            output = nameBox.Text;
            DialogResult = true;
        }
    }
}
