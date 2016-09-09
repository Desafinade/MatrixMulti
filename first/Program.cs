using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace first
{
    class Program
    {
        static void f(double[][] A, double[][] B, double[][] C, int N)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    for (int k = 0; k < N; k++)
                        C[i][j] += A[i][k] * B[k][j];
            sw.Stop();
            Console.WriteLine($"Время {sw.ElapsedMilliseconds} ms ");

        }
        static void Reset(double[][] C)
        {
            Array.ForEach(C, row => Array.Clear(row, 0, row.Length));
        }
        static void Main(string[] args)
        {
            double[][] A, B, C;
            int N = 500;

            #region ИНИЦИАЛИЗАЦИЯ
            A = new double[N][];
            B = new double[N][];
            C = new double[N][];
            Random r = new Random();
            for (int i = 0; i < N; i++)
            {
                A[i] = new double[N];
                B[i] = new double[N];
                C[i] = new double[N];
                for (int j = 0; j < N; j++)
                {
                    A[i][j] = r.Next(N);
                    B[i][j] = r.Next(N);
                }
            }
            #endregion

            #region МАТРИЧНОЕ_УМНОЖЕНИЕ
            Console.WriteLine("ПОСЛЕДОВАТЕЛЬНОЕ МАТРИЧНОЕ УМНОЖЕНИЕ");
            for (int it = 0; it < 5; it++)
            {
                Reset(C);
                Stopwatch sw = new Stopwatch();
                sw.Start();
                for (int i = 0; i < N; i++)
                    for (int k = 0; k < N; k++)
                        for (int j = 0; j < N; j++)
                        
                            C[i][j] += A[i][k] * B[k][j];
                sw.Stop();
                Console.WriteLine($"Время {sw.ElapsedMilliseconds} ms, Элемент {C[13][13]}, Сумма {C.Sum(row => row.Sum())} ");
            }
            #endregion

            #region ПОТОЧНОЕ_МАТРИЧНОЕ_УМНОЖЕНИЕ
            Console.WriteLine("---------------------");
            Console.WriteLine("МНОГОПОТОЧНОЕ МАТРИЧНОЕ УМНОЖЕНИЕ");
            int m = 2;
            var threads = new System.Threading.Thread[m];
            int size = N / m;
            for (int it = 0; it < 5; it++)
            {
                Reset(C);
                Stopwatch sw = new Stopwatch();
                sw.Start();
                for (int p = 0; p < m; p++)
                {
                   int y = p;
                   threads[p] = new System.Threading.Thread(() =>
                   {
                       int start = y * size;
                       int end = y < m-1 ?  (y + 1) * size : N;
                       for (int i = start; i < end; i++)
                               for (int k = 0; k < N; k++)
                               for (int j = 0; j < N; j++)
                                    C[i][j] += A[i][k] * B[k][j];
                   });
                    threads[p].Start();
                }
                Array.ForEach(threads, t => t.Join());
                sw.Stop();
                Console.WriteLine($"Время {sw.ElapsedMilliseconds} ms, Элемент {C[13][13]}, Сумма {C.Sum(row => row.Sum())}");
            }
            #endregion

            #region МАТРИЧНОЕ_УМНОЖЕНИЕ_C_TPL
            Console.WriteLine("---------------------------");
            Console.WriteLine("ПАРАЛЛЕЛЬНОЕ МАТРИЧНОЕ УМНОЖЕНИЕ С TPL");

            for (int it = 0; it < 5; it++)
            {
                Reset(C);
                Stopwatch sw = new Stopwatch();
                sw.Start();
                Parallel.For(0, N, i =>
                {
                    for (int k = 0; k < N; k++)
                        for (int j = 0; j < N; j++)
                            C[i][j] += A[i][k] * B[k][j];
                });
                sw.Stop();
                Console.WriteLine($"Время {sw.ElapsedMilliseconds} ms, Элемент {C[13][13]}, Сумма {C.Sum(row => row.Sum())} ");
            }
            #endregion

            #region МАТРИЧНОЕ_УМНОЖЕНИЕ_С_PLINQ

            #endregion


            Console.ReadKey();
        }
    }
}
