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
    /// Логика взаимодействия для AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        public string[] output = new string[6];
        public AddWindow(List<Dictionary<int, string>> dicts, DataItem? dataItem)
        {
            InitializeComponent();
            InitializeComboBoxes(dicts, dataItem);
        }
        private void InitializeComboBoxes(List<Dictionary<int, string>> dicts, DataItem dataItem)
        {
            foreach (var elem in dicts[0])
                categoryBox.Items.Add(elem.Value);
            foreach (var elem in dicts[1])
                peopleBox.Items.Add(elem.Value);
            foreach (var elem in dicts[2])
                transactionBox.Items.Add(elem.Value);
            foreach (var elem in dicts[3])
                valuteBox.Items.Add(elem.Value);
            if (dataItem is null) return;
            categoryBox.SelectedItem = dataItem.Column2;
            transactionBox.SelectedItem = dataItem.Column3;
            peopleBox.SelectedItem = dataItem.Column4;
            valuteBox.SelectedItem = dataItem.Column6;
            dateBox.Text = dataItem.Column7.ToString();
            noteBox.Text = dataItem.Column8;
        }
        void Accept_Click(object sender, RoutedEventArgs e)
        {
            output = new string[] { categoryBox.Text, peopleBox.Text, transactionBox.Text, valuteBox.Text, dateBox.Text, noteBox.Text};
            DialogResult = true;
        }
    }
}