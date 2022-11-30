using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace URMoney
{
    static internal class Requests
    {
        // getValutes получает информацию о курсах валют
        static public Dictionary<string, string[]> getValutes()
        {
            // считываем информацию о курсах валют по данным Центрального Банка
            XmlDocument xDoc = new XmlDocument();
            List<string> elems = new List<string>();
            xDoc.Load("https://www.cbr-xml-daily.ru/daily_utf8.xml");
            XmlElement xRoot = xDoc.DocumentElement;
            // проверяем на корректность считывания
            if (xRoot != null)
            {
                // перебираем узлы
                foreach (XmlElement xnode in xRoot)
                {
                    // перебираем дочерние узлы
                    foreach (XmlNode childnode in xnode.ChildNodes)
                    {
                        // добавляем необходимую информацию в список элементов, из которых соствляется словарь
                        if (childnode.Name == "CharCode" || childnode.Name == "Nominal" || childnode.Name == "Name" || childnode.Name == "Value")
                        {
                            elems.Add(childnode.InnerText);
                        }
                    }
                }
            }

            return setDict(elems);
        }
        // setDict из полученных элементов составляет словарь
        static private Dictionary<string, string[]> setDict(List<string> elems)
        {
            // инициализируем переменные
            Dictionary<string, string[]> valutes = new Dictionary<string, string[]>();
            int i = 1, nominal = 1;
            string code = "";
            // перебираем элементы для словаря
            foreach (var elem in elems)
            {
                // собираем словарь (код: [название, курс])
                switch (i)
                {
                    case 1: valutes.Add(elem, new string[2] { "", "" }); code = elem; i++; break;
                    case 2: nominal = Convert.ToInt32(elem); i++; break;
                    case 3: valutes[code][0] = elem; i++; break;
                    case 4: valutes[code][1] = Convert.ToString(Math.Round(Convert.ToDouble(elem) / nominal, 4)); i = 1; break;
                }
            }
            // добавляем в словарь Российский рубль
            valutes.Add("RUS", new string[2] { "Русский рубль", "1" });
            return valutes;
        }
    }
}
