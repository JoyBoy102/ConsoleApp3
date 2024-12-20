﻿using System;
using System.Collections.Generic;
using ConsoleTables;

namespace ConsoleApp3
{
    class ExtrapolAndAccurValue
    {
        public double ExtrapolValue;
        public double AccurValue;
        public double n;
        public double ExtrapolValue_Q;
        public ExtrapolAndAccurValue(int n, double extrapolValue, double accurValue, double extrapolValue_Q)
        {
            this.n = n;
            ExtrapolValue = extrapolValue;
            AccurValue = accurValue;
            ExtrapolValue_Q = extrapolValue_Q;
        }
    }

    class Programm
    {
        public static List<ExtrapolAndAccurValue> Values = new List<ExtrapolAndAccurValue>();
        
        static void Main(string[] args)
        {
            var table = new ConsoleTable("n", "zN-zTochone", "zN-zExtrapol(1)", "zExtrapol(1)-zTochonoe", "deltaN", "zExtrapol(2)-zTochnoe", "deltaN(1)");
            double z_tochone = 10.5844484649508098;
            string col1, col2, col3, col4, col5, col6, col7;

            FillExtrapolAndAccurValues(2, 1.1, 0.1);

            int i = 0;
            foreach (var value in Values)
            {
                double z_extrapol2 = 0;
                col1 = $"{value.n, 8}";
                col2 = $"{value.AccurValue - z_tochone, 12:F2}";
                var valueCol2 = value.AccurValue - z_tochone;
                col3 = $"{value.AccurValue - value.ExtrapolValue, 12:F2}"; //Оценка погрешности
                col4 = $"{value.ExtrapolValue - z_tochone,12:E2}";
                var valueCol4 = value.ExtrapolValue - z_tochone;
                col5 = $"{valueCol4 / valueCol2, 12:E2}";
                if (i > 0 && value.n >= 4)
                {
                    z_extrapol2 = RichardsonMethod(value.ExtrapolValue, Values[i - 1].ExtrapolValue, 2, 1.1);
                    col6 = $"{z_extrapol2 - z_tochone,12:E2}";
                    var valueCol6 = z_extrapol2 - z_tochone;
                    col7 = $"{valueCol6 / valueCol4,12:E2}";
                }
                else
                {
                    col6 = "-";
                    col7 = "-";
                }

                //Console.WriteLine($"{col1,8} {col2,12:F2} {col3,12:F2} {col4,12:E2} {col5,12:E2} {col6,12:E2} {col7, 12:E2}");
                table.AddRow(col1, col2, col3, col4, col5, col6, col7);
                
                i++;
            }
            table.Write();
        }

        public static double get_n_PartialSum(int n, Func<int, double, double> f, double a)
        {
            double partial_sum = 0;
            for (int i = 1; i <= n; i++)
                partial_sum += f(i, a);
            return partial_sum;
        }

        public static double f(int i, double a)
        {
            return 1.0 / (Math.Pow(i, a));
        }

        public static double RichardsonMethod(double z_n, double z_n_q, int Q, double k)
        {
            return z_n + (z_n - z_n_q) / (Math.Pow(Q, k) - 1);
        }

        public static void FillExtrapolAndAccurValues(int Q, double a, double k)
        {
            for (int n = 2, i = 1; n <= 131072; i++, n = (int)Math.Pow(2, i))
            {
                double z_n = get_n_PartialSum(n, f, a);
                double z_n_q = get_n_PartialSum(n / Q, f, a);
                double z_n_1 = RichardsonMethod(z_n, z_n_q, Q, k);
                Values.Add(new ExtrapolAndAccurValue(n, z_n_1, z_n, z_n_q));
            }
        }
    }
}
