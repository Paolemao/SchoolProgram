using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyList
{
    class Program
    {
        static void Main(string[] args)
        {
            MyList<int> a = new MyList<int>();
            a.Add(1);
            a.Add(2);
            a.Add(3);
            a.Add(4);
            a.Add(5);
            a.Add(6);
            a.Add(7);
            a.Add(8);
            a.Add(9);
            a.Add(10);

            a.Remove(10);
            a.RemoveAt(5);
            a.Insert(5,99);
            for (int i = 0; i < a.Count; ++i)
            {
                if (i == 5)
                {
                    Trace.Assert(a[i] == 99);
                }
                else
                {
                    Trace.Assert(a[i] == i + 1);
                }
                Console.WriteLine(a.Get(i));

            }


            MyList<int> l = new MyList<int>();
            for (int i = 0; i <= 10000; ++i)
            {
                l.Add(i + 100);
            }

            for (int i = 100; i > 0; --i)
            {
                l.Insert(i * 100, 999);
                l.RemoveAt(i * 100 - 1);
            }

            for (int i = 0; i < 10000; ++i)
            {
                int n = i + 100;
                if (n % 100 == 99)
                {
                    Trace.Assert(l[i] == 999);
                }
                else
                {
                    Trace.Assert(l[i] == n);
                }
            }

            Console.WriteLine("测试成功！");
            Console.WriteLine("仅测试了Add, Insert, RemoveAt, Count四个方法");
            Console.ReadKey();
        }
    }
}
