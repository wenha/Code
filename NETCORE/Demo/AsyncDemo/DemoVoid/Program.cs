using System;
using System.Threading;
using System.Threading.Tasks;

namespace DemoVoid
{
    class Program
    {
        static void Main(string[] args)
        {
            CookDinner();
            Console.ReadLine();
        }

        public static int GetThreadId()
        {
            return Thread.CurrentThread.ManagedThreadId;
        }

        public static void CookDinner()
        {
            Console.WriteLine("老婆开始打扫房间了，线程为{0}", GetThreadId());
            Console.WriteLine("垃圾满了，快去仍垃圾");
            CommandDropLitter();
            Console.WriteLine("不管他继续打扫，线程Id为{0}", GetThreadId());
            Thread.Sleep(1000);
            Console.WriteLine("老婆把房间打扫好了，线程Id为{0}", GetThreadId());
        }

        public static async void CommandDropLitter()
        {
            Console.WriteLine("这时我准备去扔垃圾，线程为{0}", GetThreadId());
            await Task.Run(() =>
            {
                Console.WriteLine("屁颠屁颠的去扔垃圾，线程Id为{0}", GetThreadId());
                Thread.Sleep(1000);
            });

            Console.WriteLine("垃圾扔了还有啥吩咐的，线程Id为{0}", GetThreadId());
        }
    }
}
