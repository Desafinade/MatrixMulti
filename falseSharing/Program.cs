using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace falseSharing
{
    class Program
    {
        static int[] data;
        static void doSomething1(object idx)
        {
            int k = (int)idx;
            for (int i = 0; i < 100000; i++)
                data[k]++;

        }
        static void doSomething2(object idx)
        {
            int k = (int)idx;
            int local = 0;
            for (int i = 0; i < 100000; i++)
                local++;
            data[k] = local;

        }
        static void Main(string[] args)
        {
            int nThreads = 4;
            int nIterations = 20;
            data = new int[nThreads];
            Thread[] threads = new Thread[nThreads];
            List<double> results = new List<double>();
            for (int it = 0; it < nIterations; it++)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                for (int i = 0; i < nThreads; i++)
                {
                    threads[i] = new Thread(doSomething1);
                    threads[i].Start(i);
                }
                for (int i = 0; i < nThreads; i++)
                    threads[i].Join();
                sw.Stop();
                results.Add(sw.ElapsedMilliseconds);
            }
            Console.WriteLine(results.Average());

            Console.WriteLine("--------------------------");
            results = new List<double>();
            for (int it = 0; it < nIterations; it++)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                for (int i = 0; i < nThreads; i++)
                {
                    threads[i] = new Thread(doSomething2);
                    threads[i].Start(i);
                }
                for (int i = 0; i < nThreads; i++)
                    threads[i].Join();
                sw.Stop();
                results.Add(sw.ElapsedMilliseconds);
            }
            Console.WriteLine(results.Average());

            Console.ReadKey();
        }
    }
}
