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
        static public List<double> financeDeposit(double total, double percent, int term, int capitalizationPeriod, bool isReInvest)
        {
            List<double> output = new List<double>() { total };
            try
            {
                if (total < 0 || percent < 0 || term < 0 || capitalizationPeriod < 0)
                    throw new Exception();
                for (int i = 0; i < term; i++)
                {
                    // проверка на наличие капитализации
                    if (isReInvest)
                    {
                        // вычисление
                        output.Add(Math.Round(output[output.Count - 1] + output[output.Count - 1] * percent / 100 / capitalizationPeriod, 2));
                    }
                    else
                    {
                        output.Add(Math.Round(output[output.Count - 1] + output[0] * percent / 100 / capitalizationPeriod, 2));
                    }
                }
            }
            catch (Exception)
            {
                // в случае, если возникла ошибка, мы возвращаем -1, указывая смежным модулям о возникших неполадках
                output.Add(-1);
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
