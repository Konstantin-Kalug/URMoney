﻿using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        public HelpWindow()
        {
            InitializeComponent();
            InitializeTexts();
        }

        public void InitializeTexts()
        {
            tablesText.Text = "Финансовое приложение URMoney предоставляет возможность вести учет доходов и расходов с помощью соответствующих таблиц.\n\nВ данном разделе представлены основные рекомендации по пользованию:\n-Перед добавлением новой траты / приобретения необходимо заполнить соответствующие списки:\n  * Люди\n  * Категории\n  * Транзакции\n- При добавлении или изменении строк необходимо заполнить все поля(поле \"примечание\" - необязательное)\n-Дата указывается в формате ДД.ММ.ГГГГ либо ММ.ДД.ГГГГ, желательно всегда использовать один формат.\n- Любые изменения(в том числе удаления строк) необратимы\n\nТакже пользователю предоставляется возможность наглядно сравнить соотношение расходов и доходов в разделе \"Визуализация\".";
            calcsText.Text = "Финансовые приложение URMoney предоставляет возможность производить финансовые расчеты в следующий категориях:\n- Расчет капитализации от вклада\n- Расчет погашения кредитной карты\n- Расчет процентов\n- Конвертация валют(по курсу Цб РФ)\nВ данном разделе представлены основные рекомендации по пользованию:\n-При использовании калькуляторов необходимо заполнить все текстовые поля(выбрать элементы из всех выпадающих списков)\n- Все числа принудительно округляются до 2 знаков после запятой";
            listsText.Text = "Финансовое приложение URMoney предоставляет возможность хранить следующие компоненты таблиц учета:\n- Люди\n- Категории\n- Транзакции\n\nВ данном разделе представлены основные рекомендации по пользованию:\n-При добавлении / изменении компонента необходимо заполнять все доступные поля\n-При удалении любого компонента из списков сотрутся все записи из таблиц учета , поэтому рекомендуется либо изменить параметры компонента, либо заменить в таблицах учета данных компонент на другой, если вы не хотите потерять данные.";
            programmText.Text = "Финансовое приложение URMoney предназначено для учета финансовой семейного бюджета и проведения основных финансовых вычислений. Создано в рамках выполнения курсового проекта по дисциплине \"Проектная деятельность 2\" в Университетском Колледже СибГИУ студентами:\n* Калугин Константин\n* Батманов Павел\n* Ахрамович Никита\n* Яковлев Владислав\n\n\nСайт Сибирского Государственного Индустриального Университета расположен по ссылке: sibsiu.ru";
        }
    }
}
