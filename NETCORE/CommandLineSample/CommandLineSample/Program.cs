using Microsoft.Extensions.Configuration;
using System;

namespace CommandLineSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder().AddCommandLine(args);

            var configurationBuilder = configuration.Build();

            Console.WriteLine($"name:{configurationBuilder["name"]}");
            Console.WriteLine($"age:{configurationBuilder["age"]}");

            Console.ReadLine();
        }
    }
}
