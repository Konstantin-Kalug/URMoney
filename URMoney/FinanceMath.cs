using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URMoney
{
    static internal class FinanceMath
    {
        // financeConvert производит конвертацию некоторой суммы из одной валюты в другую согласно данным словаря
        static public double financeConvert(string charCodeInput, string charCodeOutput, double total, Dictionary<string, string[]> valutes)
        {
            // output - запись суммы после конвертации
            double output;
            try
            {
                // вычисление
                output = Math.Round(Convert.ToDouble(valutes[charCodeInput][1]) / Convert.ToDouble(valutes[charCodeOutput][1]) * total, 2);
            }
            catch
            {
                // в случае, если возникла ошибка, мы возвращаем -1, указывая смежным модулям о возникших неполадках
                output = -1;
            }
            return output;
        }
        // financeDeposit расчитывает сумму, которая выйдет после закрытия вклада
        static public double financeDeposit(double total, double percent, int term, int capitalizationPeriod, bool isReInvest)
        {
            double output;
            try
            {
                // проверка на наличие капитализации
                if (isReInvest)
                {
                    // вычисление
                    output = Math.Round(total * Math.Pow(1 + (percent / capitalizationPeriod / 100), Math.Ceiling((double)term / (double)capitalizationPeriod)), 2);
                }
                else
                {
                    output = Math.Round(total + total * percent / capitalizationPeriod / 100 * Math.Ceiling((double)term / (double)capitalizationPeriod), 2);
                }
            }
            catch
            {
                // в случае, если возникла ошибка, мы возвращаем -1, указывая смежным модулям о возникших неполадках
                output = -1;
            }
            return output;
        }
        static public List<double> financeCredit(double total, double percent, int numberOfPeriods, bool dif)
        {
            List<double> output;
            try
            {
                output = new List<double>();
                if (!dif)
                {
                }
                else
                {
                }
            }
            catch
            {
                output = new List<double>() { -1 };
            }
            return output;
        }
    }
}
