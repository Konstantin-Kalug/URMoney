using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private Grid[] speciallyFrames;
        private CheckBox[] percentCheckBoxes;
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            speciallyFrames = new Grid[] { FrameTables, DepositFrame, CreditFrame, PercentFrame, VisualFrame, PeopleFrame, CategoryFrame, TransactionFrame };
            percentCheckBoxes = new CheckBox[] { percent0CheckBox, percent1CheckBox, percent2CheckBox, percent3CheckBox };
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
                db.Types.Add(new Type { Title = "Доход" });
                db.Types.Add(new Type { Title = "Расход" });
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
                if (!(db.Categories.Find(elem.CategoryId) is null || db.Transactions.Find(elem.TransactionId) is null || db.Peoples.Find(elem.PeopleId) is null || db.Valutes.Find(elem.ValuteId) is null))
                    items.Add(new DataItem { Column1 = elem.Id, Column2 = db.Categories.Find(elem.CategoryId).Title, Column3 = db.Transactions.Find(elem.TransactionId).Title, Column4 = db.Peoples.Find(elem.PeopleId).Name, Column5 = db.Transactions.Find(elem.TransactionId).Total, Column6 = db.Valutes.Find(elem.ValuteId).Title, Column7 = elem.Date, Column8 = elem.Note });
                else
                {
                    db.Operations.Remove(elem);
                    db.SaveChanges();
                }
            table.ItemsSource = items;
        }
        private void OutputDataGridPeople()
        {
            People[] elems = db.Peoples.ToArray();
            List<DataItemPeople> items = new List<DataItemPeople>();
            foreach (var elem in elems)
                items.Add(new DataItemPeople { Column1 = elem.Id, Column2 = elem.Name });
            tablePeople.ItemsSource = items;
        }
        private void OutputDataGridCategory()
        {
            Category[] elems = db.Categories.ToArray();
            List<DataItemCategory> items = new List<DataItemCategory>();
            foreach (var elem in elems)
                items.Add(new DataItemCategory { Column1 = elem.Id, Column2 = elem.Title, Column3 = db.Types.Find(elem.TypeId).Title});
            tableCategory.ItemsSource = items;
        }
        private void OutputDataGridTransaction()
        {
            Transaction[] elems = db.Transactions.ToArray();
            List<DataItemTransaction> items = new List<DataItemTransaction>();
            foreach (var elem in elems)
                items.Add(new DataItemTransaction { Column1 = elem.Id, Column2 = elem.Title, Column3 = elem.Total });
            tableTransaction.ItemsSource = items;
        }
        // Таблицы Учёта
        private void tablesButton_Click(object sender, RoutedEventArgs e)
        {
            tablesStackPanel.Visibility = Visibility.Visible;
            calcStackPanel.Visibility = Visibility.Hidden;
        }
        // Доходы
        private void incomeButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Visibility = Visibility.Hidden;
            FrameTables.Visibility = Visibility.Visible;
            expenses.Visibility = Visibility.Hidden;
            income.Visibility = Visibility.Visible;
            OutputDataGrid();

        }
        // Расходы
        private void expensesButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Visibility = Visibility.Hidden;
            FrameTables.Visibility = Visibility.Visible;
            expenses.Visibility = Visibility.Visible;
            income.Visibility = Visibility.Hidden;
        }
        //Вернуться на главное окно
        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Visibility = Visibility.Visible;
            foreach (var frame in speciallyFrames)
                frame.Visibility = Visibility.Hidden;
            tablesStackPanel.Visibility = Visibility.Hidden;
            calcStackPanel.Visibility = Visibility.Hidden;
        }

        private void visualisationButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Visibility = Visibility.Hidden;
            VisualFrame.Visibility = Visibility.Visible;
            InitializeVisual();
        }

        private void depositButton_Click(object sender, RoutedEventArgs e)
        {
            DepositFrame.Visibility = Visibility.Visible;
        }

        private void percentButton_Click(object sender, RoutedEventArgs e)
        {
            PercentFrame.Visibility = Visibility.Visible;
        }

        private void creditButton_Click(object sender, RoutedEventArgs e)
        {
            CreditFrame.Visibility = Visibility.Visible;
        }

        private void convertButton_Click(object sender, RoutedEventArgs e)
        {
            //ConvertFrame.Visibility = Visibility.Visible;
        }

        private void calcsButton_Click(object sender, RoutedEventArgs e)
        {
            calcStackPanel.Visibility = Visibility.Visible;
            tablesStackPanel.Visibility = Visibility.Hidden;
        }

        private void calcCreditButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<double> elems = FinanceMath.financeCredit(Convert.ToDouble(totalCreditTextBox.Text), Convert.ToDouble(percentCreditTextBox.Text), Convert.ToInt32(numberOfPeriodsTextBox.Text), (bool)difRationButton.IsChecked);
                if (elems.Count == 2 && elems[elems.Count - 1] == -1)
                    throw new Exception();
                outputCreditTextBox.Text = "Изначальная сумма: " + elems[0].ToString();
                for (int i = 1; i < elems.Count - 1; i++)
                    outputCreditTextBox.Text += $"\nПлатеж за {i}-й месяц: {elems[i]}";
                outputCreditTextBox.Text += $"\nСумма платежей: {elems[elems.Count - 1]}";
            }
            catch
            {
                MessageBox.Show("Произошла ошибка, пожалуйства, проверьте корректность внесенных данных!");
            }
        }

        private void calcPercentButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                outputPercentTextBox.Text = "Изначальная сумма: " + Math.Round(Convert.ToDouble(totalPercentTextBox.Text), 2);
                for (int i = 0; i < percentCheckBoxes.Length; i++)
                    if (percentCheckBoxes[i].IsChecked == true)
                    {
                        switch (i)
                        {
                            case 0: outputPercentTextBox.Text += "\nПроцент от числа:"; break;
                            case 1: outputPercentTextBox.Text += "\nЧисло от процента:"; break;
                            case 2: outputPercentTextBox.Text += "\nСумма числа и процента:"; break;
                            case 3: outputPercentTextBox.Text += "\nРазность числа и процента:"; break;
                        }
                        outputPercentTextBox.Text += $"{FinanceMath.financePercent(Convert.ToDouble(totalPercentTextBox.Text), Convert.ToDouble(percentPercentTextBox.Text), i)[1]}";
                    }
            }
            catch
            {
                MessageBox.Show("Произошла ошибка, пожалуйства, проверьте корректность внесенных данных!");
            }
        }

        private void calcDepositButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<double> elems = FinanceMath.financeDeposit(Convert.ToDouble(totalDepositTextBox.Text), Convert.ToDouble(percentDepositTextBox.Text), Convert.ToInt32(termDepositTextBox.Text), Convert.ToInt32(capitalizationPeriodTextBox.Text), (bool)reInvestCheckBox.IsChecked);
                if (elems.Count == 2 && elems[elems.Count - 1] == -1)
                    throw new Exception();
                outputDepositTextBox.Text = "Изначальная сумма: " + elems[0].ToString();
                for (int i = 1; i < elems.Count - 1; i++)
                    outputDepositTextBox.Text += $"\n{i}-й месяц: {elems[i]}";
                outputDepositTextBox.Text += $"\nКонечная сумма: {elems[elems.Count - 1]}";
            }
            catch
            {
                MessageBox.Show("Произошла ошибка, пожалуйства, проверьте корректность внесенных данных!");
            }
        }

        private void sibsiuButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://sibsiu.ru/") { UseShellExecute = true });
        }

        private void familyButton_Click(object sender, RoutedEventArgs e)
        {
            PeopleFrame.Visibility = Visibility.Visible;
            OutputDataGridPeople();
        }

        private void addHumansButton_Click(object sender, RoutedEventArgs e)
        {
            AddPeopleWindow AddPeopleWindow = new AddPeopleWindow(null);
            if (AddPeopleWindow.ShowDialog() == true)
            {
                try
                {
                    People People = new People { Name = AddPeopleWindow.output};
                    db.Peoples.Add(People);
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    MessageBox.Show("Пожалуйства заполните все поля корректными данными!");
                }
                OutputDataGridPeople();
            }
        }

        private void editHumansButton_Click(object sender, RoutedEventArgs e)
        {
            // получаем выделенный объект
            DataItemPeople? dataItem = tablePeople.SelectedItem as DataItemPeople;
            // если ни одного объекта не выделено, выходим
            if (dataItem is null) return;
            AddPeopleWindow AddPeopleWindow = new AddPeopleWindow(dataItem);
            if (AddPeopleWindow.ShowDialog() == true)
            {
                try
                {
                    // получаем измененный объект
                    People people = db.Peoples.Find(dataItem.Column1);
                    if (people != null)
                    {
                        people.Name = AddPeopleWindow.output;
                        db.SaveChanges();
                        OutputDataGridPeople();
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Пожалуйства заполните все поля корректными данными!");
                }
            }
        }

        private void deleteHumansButton_Click(object sender, RoutedEventArgs e)
        {
            // получаем выделенный объект
            DataItemPeople? dataItem = tablePeople.SelectedItem as DataItemPeople;
            // если ни одного объекта не выделено, выходим
            if (dataItem is null) return;
            People people = db.Peoples.Find(dataItem.Column1);
            db.Peoples.Remove(people);
            db.SaveChanges();
            OutputDataGridPeople();
        }

        private void addCategoriesButton_Click(object sender, RoutedEventArgs e)
        {
            AddCategoryWindow AddCategoryWindow = new AddCategoryWindow(null);
            if (AddCategoryWindow.ShowDialog() == true)
            {
                try
                {
                    Category Category = new Category { Title = AddCategoryWindow.output[0], TypeId = db.Types.Where(p => p.Title == AddCategoryWindow.output[1]).ToArray()[0].Id };
                    db.Categories.Add(Category);
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    MessageBox.Show("Пожалуйства заполните все поля корректными данными!");
                }
                OutputDataGridCategory();
            }
        }

        private void editCategoriesButton_Click(object sender, RoutedEventArgs e)
        {
            // получаем выделенный объект
            DataItemCategory? dataItem = tableCategory.SelectedItem as DataItemCategory;
            // если ни одного объекта не выделено, выходим
            if (dataItem is null) return;
            AddCategoryWindow AddCategoryWindow = new AddCategoryWindow(dataItem);
            if (AddCategoryWindow.ShowDialog() == true)
            {
                try
                {
                    // получаем измененный объект
                    Category category = db.Categories.Find(dataItem.Column1);
                    if (category != null)
                    {
                        category.Title = AddCategoryWindow.output[0];
                        category.TypeId = db.Types.Where(p => p.Title == AddCategoryWindow.output[1]).ToArray()[0].Id;
                        db.SaveChanges();
                        OutputDataGridCategory();
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Пожалуйства заполните все поля корректными данными!");
                }
            }
        }

        private void deleteCategoriesButton_Click(object sender, RoutedEventArgs e)
        {
            // получаем выделенный объект
            DataItemCategory? dataItem = tableCategory.SelectedItem as DataItemCategory;
            // если ни одного объекта не выделено, выходим
            if (dataItem is null) return;
            Category category = db.Categories.Find(dataItem.Column1);
            db.Categories.Remove(category);
            db.SaveChanges();
            OutputDataGridCategory();
        }

        private void categoryButton_Click(object sender, RoutedEventArgs e)
        {
            CategoryFrame.Visibility = Visibility.Visible;
            OutputDataGridCategory();
        }

        private void transactionButton_Click(object sender, RoutedEventArgs e)
        {
            TransactionFrame.Visibility = Visibility.Visible;
            OutputDataGridTransaction();
        }

        private void deleteTransactionsButton_Click(object sender, RoutedEventArgs e)
        {
            // получаем выделенный объект
            DataItemTransaction? dataItem = tableTransaction.SelectedItem as DataItemTransaction;
            // если ни одного объекта не выделено, выходим
            if (dataItem is null) return;
            Transaction transaction = db.Transactions.Find(dataItem.Column1);
            db.Transactions.Remove(transaction);
            db.SaveChanges();
            OutputDataGridTransaction();
        }

        private void editTransactionsButton_Click(object sender, RoutedEventArgs e)
        {
            // получаем выделенный объект
            DataItemTransaction? dataItem = tableTransaction.SelectedItem as DataItemTransaction;
            // если ни одного объекта не выделено, выходим
            if (dataItem is null) return;
            AddTransactionWindow AddTransactionWindow = new AddTransactionWindow(dataItem);
            if (AddTransactionWindow.ShowDialog() == true)
            {
                try
                {
                    // получаем измененный объект
                    Transaction transaction = db.Transactions.Find(dataItem.Column1);
                    if (transaction != null)
                    {
                        transaction.Title = AddTransactionWindow.output[0];
                        transaction.Total = Math.Round(Convert.ToDecimal(AddTransactionWindow.output[1]), 2);
                        db.SaveChanges();
                        OutputDataGridTransaction();
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Пожалуйства заполните все поля корректными данными!");
                }
            }
        }

        private void addTransactionsButton_Click(object sender, RoutedEventArgs e)
        {
            AddTransactionWindow AddTransactionWindow = new AddTransactionWindow(null);
            if (AddTransactionWindow.ShowDialog() == true)
            {
                try
                {
                    Transaction Transaction = new Transaction { Title = AddTransactionWindow.output[0], Total = Math.Round(Convert.ToDecimal(AddTransactionWindow.output[1]), 2) };
                    db.Transactions.Add(Transaction);
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    MessageBox.Show("Пожалуйства заполните все поля корректными данными!");
                }
                OutputDataGridTransaction();
            }
        }
    }
}
