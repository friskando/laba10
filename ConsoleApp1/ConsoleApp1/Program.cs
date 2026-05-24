using System;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        static void Main()
        {

            Test1();
            Test2();
            Test3();

            Console.WriteLine("\nТЕСТИРОВАНИЕ ЗАВЕРШЕНО");
        }

        static void Test1()
        {
            Console.Write("чтение 'abc'");
            File.WriteAllText("C:\\Users\\boxer\\Desktop\\YP\\laba10\\paskal1.txt", "abc");
            InputOutput.Open("C:\\Users\\boxer\\Desktop\\YP\\laba10\\paskal1.txt");

            InputOutput.NextCh(); char c1 = InputOutput.Ch;
            InputOutput.NextCh(); char c2 = InputOutput.Ch;
            InputOutput.NextCh(); char c3 = InputOutput.Ch;

            if (c1 == 'a' && c2 == 'b' && c3 == 'c')
                Console.WriteLine("ПРОЙДЕН");
            else
                Console.WriteLine("НЕ ПРОЙДЕН");

        }

        static void Test2()
        {
            Console.Write("Тест 2 (конец файла): ");
            File.WriteAllText("C:\\Users\\boxer\\Desktop\\YP\\laba10\\paskal2.txt", "x");
            InputOutput.Open("C:\\Users\\boxer\\Desktop\\YP\\laba10\\paskal2.txt");

            InputOutput.NextCh(); char c1 = InputOutput.Ch;
            InputOutput.NextCh(); char c2 = InputOutput.Ch;

            if (c1 == 'x')
                Console.WriteLine("ПРОЙДЕН");
            else
                Console.WriteLine("НЕ ПРОЙДЕН");

        }

        static void Test3()
        {
            Console.Write("пустой файл");
            File.WriteAllText("C:\\Users\\boxer\\Desktop\\YP\\laba10\\paskal3.txt", "");
            InputOutput.Open("C:\\Users\\boxer\\Desktop\\YP\\laba10\\paskal3.txt");

            InputOutput.NextCh(); char c1 = InputOutput.Ch;

            if (c1 == '\0')
                Console.WriteLine("ПРОЙДЕН");
            else
                Console.WriteLine("НЕ ПРОЙДЕН");

        }
    }
}