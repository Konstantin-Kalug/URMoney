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
            total = Math.Round(total, 2);
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
            total = Math.Round(total, 2);
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
        // financeCredit вычисляет ежемесячные платежи и их сумму
        static public List<double> financeCredit(double total, double percent, int numberOfPeriods, bool dif)
        {
            total = Math.Round(total, 2);
            List<double> output = new List<double> { total };
            try
            {
                if (total < 0 || percent < 0 || numberOfPeriods < 0)
                    throw new Exception();
                double remains = total;
                for (int i = 0; i < numberOfPeriods; i++)
                {
                    // вычисление аннуитетного платежа
                    if (!dif)
                    {
                        output.Add(Math.Round(total * percent / 100 / 12 / (1 - Math.Pow(1 + percent / 100 / 12, -numberOfPeriods)), 2));
                    }
                    else
                    // вычисление дифференцированного платежа
                    {
                        output.Add(Math.Round(total / numberOfPeriods + remains * percent / 100 / 12, 2));
                        remains -= output[output.Count - 1];
                    }
                }
                output.Add(output.Sum() - total);
            }
            catch
            {
                output.Add(-1);
            }
            return output;
        }
        // financePercent находит процент от числа, число от процента, сумму и разность числа с процентом в указанном порядке
        static public List<double> financePercent(double total, double percent, int numberOfOperation)
        {
            total = Math.Round(total, 2);
            List<double> output = new List<double> { total };
            try
            {
                if (total < 0 || percent < 0)
                    throw new Exception();
                switch (numberOfOperation)
                {
                    case 0: output.Add(total * percent / 100); break;
                    case 1: output.Add(total / percent * 100); break;
                    case 2: output.Add(total * (1 + percent / 100)); break;
                    case 3: output.Add(total * (1 - percent / 100)); break;
                }
            }
            catch (Exception)
            {
                output.Add(-1);
            }
            return output;
        }
    }
}
