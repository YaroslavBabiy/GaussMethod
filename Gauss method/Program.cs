using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApp16
{
    class Program
    {
        public const int n = 5; //кількість вузлів
        //струми
        public void Read_I(double[] I)
        {
            string file = @" C:\Users\Yariik\Desktop\Дкр\Струми.txt";


            string[] curr = File.ReadAllLines(file);
            for (int i = 0; i < n; i++)
            {
                I[i] = double.Parse(curr[i]);
                Console.WriteLine("I{1}{0,10}", I[i], i);
            }
        }
        //опори
        public void Read_R(double[,] R)
        {
            string file = @" C:\Users\Yariik\Desktop\Дкр\Опори.txt";
            string[] line = File.ReadAllLines(file);
            for (int i = 0; i < n; i++)
            {
                string[] r = line[i].Split(';');
                for (int j = 0; j < n; j++)
                {
                    R[i, j] = double.Parse(r[j]);
                    Console.Write("{0,5:f2}", R[i, j]);
                }
                Console.WriteLine();
            }
        }
        //матриця провідностей
        public void matrix(double[,] y, double[,] r)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (r[i, j] == 0) y[i, j] = 0;
                    else y[i, j] = 1 / r[i, j];
                }
            }
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i != j)
                        y[i, i] += -y[i, j];
                }
                for (int k = 0; k < n; k++)
                {
                    Console.Write("{0,7:f3}", y[i, k]);
                }
                Console.WriteLine();
            }
        }
        // Gauss
        public void Gaus(double[,] a, double[] b, double[] U0)
        {
            //прямий хід
            for (int k = 1; k < n - 1; k++)
            {
                for (int i = k + 1; i <= n - 1; i++)
                {
                    double c = a[i, k] / a[k, k];
                    a[i, k] = 0;
                    for (int j = k + 1; j <= n - 1; j++)
                    {
                        a[i, j] -= c * a[k, j];
                    }
                    b[i] -= c * b[k];
                }
            }
            //зворотній хід
            U0[n - 1] = b[n - 1] / a[n - 1, n - 1];
            for (int i = n - 2; i > 0; i--)
            {
                double sum = 0;
                for (int j = i + 1; j <= n - 1; j++)
                {
                    sum += a[i, j] * U0[j];
                }
                U0[i] = (b[i] - sum) / a[i, i];
            }
        }
        public void Write(double[] dU)
        {
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine("dU{1} = {0,5:f2}", dU[i], i);
            }
        }
        //Відповідь:напруга в вузлах, струми на ділянках, сумарні втрати потужності
        public void Res(double[] U0, double[] U, double[,] r, double[,] I)
        {
            //напруга у вузлах
            Console.WriteLine("Напруга у вузлах:");
            for (int i = 0; i < n; i++)
            {
                U[i] = 220 + U0[i];
                Console.WriteLine("U{1} = {0:f2}", U[i], i);
            }
            //струми на ділянках
            Console.WriteLine("\nСтруми на дiлянках:");
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (r[i, j] == 0) I[i, j] = 0;
                    else I[i, j] = (U[i] - U[j]) / r[i, j];
                    Console.Write("{0,7:f2}", I[i, j]);
                }
                Console.WriteLine();
            }
            //сумарні втрати активної потужності в ділянках мережі
            double P = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    P += Math.Pow(I[i, j], 2) * r[i, j];
                }
            }
            Console.WriteLine("\nCумарнi втрати активної потужностi в дiлянках мережi:\n{0:f2}", P / 2);
        }
        static void Main(string[] args)
        {
            double[] Indx = new double[n];
            double[,] R = new double[n, n];
            double[,] Y = new double[n, n];
            double[] dU = new double[n];
            double[] Ui = new double[n];
            double[,] Is1 = new double[n, n];
            Program X1 = new Program();
            Console.WriteLine("Струми у вузлах кола:");
            X1.Read_I(Indx);
            Console.WriteLine("\nМатриця опорiв:");
            X1.Read_R(R);
            Console.WriteLine("\nМатриця провiдностей:");
            X1.matrix(Y, R);
            Console.WriteLine("\nВтрати напруги:");
            X1.Gaus(Y, Indx, dU);
            X1.Write(dU);
            Console.WriteLine("\nРезультат:");
            X1.Res(dU, Ui, R, Is1);
            Console.ReadKey();
        }
    }

}
