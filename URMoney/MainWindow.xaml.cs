using Microsoft.EntityFrameworkCore;
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
        private ApplicationContext db = new ApplicationContext();
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }
        // при загрузке окна
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // гарантируем, что база данных создана
            db.Database.EnsureCreated();
            // загружаем данные из БД
            db.Operations.Load();
            // и устанавливаем данные в качестве контекста
            DataContext = db.Operations.Local.ToObservableCollection();
            InitializeBD();
            OutputDataGrid();
        }
        // первоначальное добавление элементов, если их нет (без учета модуля валют)
        private void InitializeBD()
        {
            // обязательный человек, не изменяется
            if (db.Peoples.ToArray().Length == 0)
            {
                db.Peoples.Add(new People { Name = "Неизвестно" });
            }
            // обязательные 2 типа, не изменяются
            if (db.Types.ToArray().Length == 0)
            {
                db.Types.Add(new Type { Title = "Доходы" });
                db.Types.Add(new Type { Title = "Расходы" });
            }
            if (db.Categories.ToArray().Length == 0)
            {
                // вставить дефолтные категории
            }
            try
            {
                Dictionary<string, string[]> valutes = Requests.getValutes();
                db.Database.ExecuteSqlRaw("DELETE FROM Valutes");
                foreach (var elem in valutes)
                {
                    Valute Valute = new Valute { Title = elem.Value[0], CharCode=elem.Key, Сource = Convert.ToDecimal(elem.Value[1]) };
                    db.Valutes.Add(Valute);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Невозможно обновить курс валют! Пожалуйста, проверьте Интернет-соединение");
            }
            db.SaveChanges();
        }
        // создаем словарь id - title
        private List<Dictionary<int, string>> InitializeKeysBD()
        {
            List<Dictionary<int, string>> dicts = new List<Dictionary<int, string>>();
            dicts.Add(new Dictionary<int, string>());
            foreach (var elem in db.Categories.ToArray())
                if (elem.TypeId == 2) dicts[0].Add(elem.Id, elem.Title);
            dicts.Add(new Dictionary<int, string>());
            foreach (var elem in db.Peoples.ToArray())
                dicts[1].Add(elem.Id, elem.Name);
            dicts.Add(new Dictionary<int, string>());
            foreach (var elem in db.Transactions.ToArray())
                dicts[2].Add(elem.Id, elem.Title);
            dicts.Add(new Dictionary<int, string>());
            foreach (var elem in db.Valutes.ToArray())
                dicts[3].Add(elem.Id, elem.Title);
            return dicts;
        }
        // выводим диаграммы
        private void InitializeVisual()
        {
            // SELECT total FROM operations INNER JOIN transactions ON transactionid = id INNER JOIN categories ON categoryid = id INNER JOIN types ON typeid = id WHERE type.title = "доходы"
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            List<Dictionary<int, string>> keys = InitializeKeysBD();
            AddWindow AddWindow = new AddWindow(keys, null);
            if (AddWindow.ShowDialog() == true)
            {
                try
                {
                    Operation Operation = new Operation { CategoryId = db.Categories.Where(p => p.Title == AddWindow.output[0]).ToArray()[0].Id, PeopleId = db.Peoples.Where(p => p.Name == AddWindow.output[1]).ToArray()[0].Id, TransactionId = db.Transactions.Where(p => p.Title == AddWindow.output[2]).ToArray()[0].Id, ValuteId = db.Valutes.Where(p => p.Title == AddWindow.output[3]).ToArray()[0].Id, Date = DateOnly.Parse(AddWindow.output[4]), Note = AddWindow.output[5] };
                    db.Operations.Add(Operation);
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    MessageBox.Show("Пожалуйства заполните все поля корректными данными!");
                }
                OutputDataGrid();
            }
        }
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            // получаем выделенный объект
            DataItem? dataItem = table.SelectedItem as DataItem;
            // если ни одного объекта не выделено, выходим
            if (dataItem is null) return;
            List<Dictionary<int, string>> keys = InitializeKeysBD();
            AddWindow AddWindow = new AddWindow(keys, dataItem);
            if (AddWindow.ShowDialog() == true)
            {
                try
                {
                    // получаем измененный объект
                    Operation operation = db.Operations.Find(dataItem.Column1);
                    if (operation != null)
                    {
                        operation.CategoryId = db.Categories.Where(p => p.Title == AddWindow.output[0]).ToArray()[0].Id;
                        operation.PeopleId = db.Peoples.Where(p => p.Name == AddWindow.output[1]).ToArray()[0].Id;
                        operation.TransactionId = db.Transactions.Where(p => p.Title == AddWindow.output[2]).ToArray()[0].Id;
                        operation.ValuteId = db.Valutes.Where(p => p.Title == AddWindow.output[3]).ToArray()[0].Id;
                        operation.Date = DateOnly.Parse(AddWindow.output[4]);
                        operation.Note = AddWindow.output[5];
                        db.SaveChanges();
                        OutputDataGrid();
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Пожалуйства заполните все поля корректными данными!");
                }
            }
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            // получаем выделенный объект
            DataItem? dataItem = table.SelectedItem as DataItem;
            // если ни одного объекта не выделено, выходим
            if (dataItem is null) return;
            Operation operation = db.Operations.Find(dataItem.Column1);
            db.Operations.Remove(operation);
            db.SaveChanges();
            OutputDataGrid();
        }
        private void OutputDataGrid()
        {
            Operation[] elems = db.Operations.ToArray();
            List<DataItem> items = new List<DataItem>();
            foreach (var elem in elems)
                items.Add(new DataItem { Column1 = elem.Id, Column2 = db.Categories.Find(elem.CategoryId).Title, Column3 = db.Transactions.Find(elem.TransactionId).Title, Column4 = db.Peoples.Find(elem.PeopleId).Name, Column5 = db.Transactions.Find(elem.TransactionId).Total, Column6 = db.Valutes.Find(elem.ValuteId).Title, Column7 = elem.Date, Column8 = elem.Note });
            table.ItemsSource = items;
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
            VisualGrid.Visibility = Visibility.Hidden;
        }

        private void visualisationButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Visibility = Visibility.Hidden;
            VisualGrid.Visibility = Visibility.Visible;
            InitializeVisual();
        }
    }
}
