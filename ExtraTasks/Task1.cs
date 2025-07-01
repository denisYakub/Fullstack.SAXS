using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraTasks
{
    /// <summary> 
    /// Написать метод, который будет подсчитывать количество цифр «2», используемых в десятичной
    // записи целых чисел. (не менее чем на 2 часа упражнение).
    // Идеальное время O(log10(n)). На входе целое число, на выходе
    // - количество вхождений двойки. Пример: Вход: 23, выход: 7 шт. двоек (2,12,20,21,22,23).
    /// </summary>
    internal static class Task1
    {
        public static void Run()
        {
            int[] phis = [ 1, 2, 3, 4, 6, 5 ];

            var res = 
                from phi1 in phis
                from phi2 in phis
                select phi1;

            foreach (var val in res)
                Console.WriteLine(val);
        }
    }
}
