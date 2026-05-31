using System;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        static void Main()
        {
            string filePathTest1 = "C:\\Users\\boxer\\Desktop\\YP\\laba10\\paskal1.txt";
            string filePathTest2 = "C:\\Users\\boxer\\Desktop\\YP\\laba10\\paskal2.txt";
            string filePath = "C:\\Users\\boxer\\Desktop\\YP\\laba10\\paskal1.txt";

            // ТЕСТ 1
            Test1(filePathTest1);

            // ТЕСТ 2
            Test2(filePathTest2);

            // ЗАДАНИЕ 1: ВЫВОД КОДОВ СИМВОЛОВ
            if (!File.Exists(filePath))
            {
                Console.WriteLine("ОШИБКА: Файл не найден!");
                Console.WriteLine("Путь: " + filePath);
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\n ЗАДАНИЕ 1\n");
            Console.WriteLine("КОДЫ СИМВОЛОВ");

            string content = File.ReadAllText(filePath);
            foreach (char c in content)
            {
                Console.Write((int)c + " ");
            }

            // ЗАДАНИЕ 0: ЛЕКСИЧЕСКИЙ АНАЛИЗ
            Console.WriteLine("\n ЗАДАНИЕ 0\n");
            Console.WriteLine("ТОКЕНЫ:");

            InputOutput.Open(filePath);
            LexicalAnalyzer analyzer = new LexicalAnalyzer();

            byte symbol;
            int tokenCount = 0;

            while ((symbol = analyzer.NextSym()) != 0)
            {
                tokenCount++;
                Console.WriteLine($"Токен {tokenCount}: {symbol} - '{analyzer.CurrentLexeme}'");
            }

            Console.WriteLine($"\nВсего токенов: {tokenCount}");
            Console.WriteLine("\nВСЕ ТЕСТЫ ЗАВЕРШЕНЫ");
            Console.ReadKey();
        }

        static void Test1(string filePathTest1)
        {
            if (!File.Exists(filePathTest1))
            {
                Console.WriteLine("ОШИБКА: Файл не найден!");
                Console.WriteLine("Путь: " + filePathTest1);
                return;
            }
            Console.WriteLine("\nТест 1:\n");
            string content = File.ReadAllText(filePathTest1);
            Console.WriteLine(content);
        }

        static void Test2(string filePathTest2)
        {
            if (!File.Exists(filePathTest2))
            {
                Console.WriteLine("ОШИБКА: Файл не найден!");
                Console.WriteLine("Путь: " + filePathTest2);
                return;
            }
            Console.WriteLine("\nТест 2:\n");
            string content = File.ReadAllText(filePathTest2);
            Console.WriteLine(content);
        }
    }
}