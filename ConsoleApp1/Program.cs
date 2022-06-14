using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var date = DateTime.Now - TimeSpan.FromDays(20);
            var datenow = DateTime.Now;
            Console.WriteLine(datenow - date);
            Console.WriteLine(TimeSpan.FromDays(20));
            Console.WriteLine(datenow - date == TimeSpan.FromDays(20));
        }
    }
}
