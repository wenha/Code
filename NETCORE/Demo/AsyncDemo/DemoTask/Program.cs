using System;
using System.Threading;
using System.Threading.Tasks;

namespace DemoTask
{
    class Program
    {
        static void Main(string[] args)
        {
            OpenMainsSwithch();

            Console.ReadLine();
        }

        public static int GetThreadId()
        {
            return Thread.CurrentThread.ManagedThreadId;
        }

        public static void OpenMainsSwithch()
        {
            Console.WriteLine("我和老婆在看电视，线程Id为{0}", GetThreadId());
            Console.WriteLine("突然停电了，快去看看是不是跳闸了");
            Task task = CommandOpenMainsSwithch();
            Console.WriteLine("没电了先玩会手机吧，线程Id为{0}", GetThreadId());
            Thread.Sleep(1000);
            Console.WriteLine("手机也没电了只等电源打开，线程Id为{0}", GetThreadId());

            while (!task.IsCompleted)
            {
                Thread.Sleep(100);
            }

            Console.WriteLine("又有电了我们继续看电视，线程Id为{0}", GetThreadId());
        }

        public static async Task CommandOpenMainsSwithch()
        {
            Console.WriteLine("这时我准备去打开电源开关，线程Id为{0}", GetThreadId());
            await Task.Run(() =>
            {
                Console.WriteLine("屁颠屁颠的去打开电源开关，线程Id为{0}", GetThreadId());
                Thread.Sleep(10000);
            });

            Console.WriteLine("电源开关打开了，线程Id为{0}", GetThreadId());
        }
    }
}
