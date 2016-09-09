using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace threadStart
{
    class Program
    {
        static int[] data;
        static int nThreads = 4;
        static void f(object o)
        {
            int m = (int)o;
          //  Console.WriteLine("Число потоков" + nThreads);
          //  Console.WriteLine("Текущий номер" + m);          
        }
        static void Main(string[] args)
        {
            int n = 4;
           // string s = Console.ReadLine();
            nThreads = 4;
            Thread[] arThreads = new Thread[nThreads];
            DateTime dt1 = DateTime.Now; 
            for(int i =0; i < nThreads; i++)
            {
                arThreads[i] = new Thread(f);
                arThreads[i].Start(i);
            }
            for (int i = 0; i < nThreads; i++)
            {
                arThreads[i].Join();
            }
            DateTime dt2 = DateTime.Now;
            var ts = dt2 - dt1;
            Console.WriteLine((dt2-dt1).TotalMilliseconds);
            Console.WriteLine("end");
            Console.ReadKey();
        }
    }
}
