using System;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        static void Main()
        {
            string filePath = "C:\\Users\\boxer\\Desktop\\YP\\laba10\\paskal1.txt";

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

            Console.WriteLine("\n ЗАДАНИЕ 0\n");

            TestReadPascalProgram(filePath);
            TestReadPascalWithErrors(filePath);

            Console.WriteLine("\nВСЕ ТЕСТЫ ЗАВЕРШЕНЫ");
            Console.ReadKey();
        }

        static void TestReadPascalProgram(string filePath)
        {
            Console.Write("ТЕСТ 1 : ");
            InputOutput.Open(filePath);
            InputOutput.NextCh(); char c1 = InputOutput.Ch;
            InputOutput.NextCh(); char c2 = InputOutput.Ch;
            InputOutput.NextCh(); char c3 = InputOutput.Ch;
            InputOutput.NextCh(); char c4 = InputOutput.Ch;
            InputOutput.NextCh(); char c5 = InputOutput.Ch;
            InputOutput.NextCh(); char c6 = InputOutput.Ch;
            InputOutput.NextCh(); char c7 = InputOutput.Ch;

            if (c1 == 'p' && c2 == 'r' && c3 == 'o' && c4 == 'g' && c5 == 'r' && c6 == 'a' && c7 == 'm')
                Console.WriteLine(" ПРОЙДЕН");
            else
                Console.WriteLine($" НЕ ПРОЙДЕН (найдено: '{c1}{c2}{c3}{c4}{c5}{c6}{c7}')");
        }

        static void TestReadPascalWithErrors(string filePath)
        {
            Console.Write("ТЕСТ 2 : ");
            InputOutput.Open(filePath);

            bool foundVar = false;
            bool foundBegin = false;
            bool foundEnd = false;

            string word = "";
            InputOutput.NextCh();

            while (InputOutput.Ch != '\0')
            {
                if ((InputOutput.Ch >= 'a' && InputOutput.Ch <= 'z') ||
                    (InputOutput.Ch >= 'A' && InputOutput.Ch <= 'Z'))
                {
                    word = "";
                    while ((InputOutput.Ch >= 'a' && InputOutput.Ch <= 'z') ||
                           (InputOutput.Ch >= 'A' && InputOutput.Ch <= 'Z'))
                    {
                        word += InputOutput.Ch;
                        InputOutput.NextCh();
                    }

                    if (word == "var") foundVar = true;
                    if (word == "begin") foundBegin = true;
                    if (word == "end") foundEnd = true;
                }
                else
                {
                    InputOutput.NextCh();
                }
            }

            if (foundVar && foundBegin && foundEnd)
                Console.WriteLine(" ПРОЙДЕН ");
            else
                Console.WriteLine(" НЕ ПРОЙДЕН ");
        }
    }
}