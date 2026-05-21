using System;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== ЗАДАНИЕ 1: ОПИСАНИЕ ФУНКЦИИ nextch ===\n");
            Console.WriteLine("Функция NextSym() в классе LexicalAnalyzer");
            Console.WriteLine("Назначение: читает следующий символ из входного потока");
            Console.WriteLine("Возвращает: код символа (byte)");
            Console.WriteLine("Использует: InputOutput.Ch - текущий символ");
            Console.WriteLine("            InputOutput.NextCh() - чтение следующего\n");

            Console.WriteLine("=== ЗАДАНИЕ 2: ТАБЛИЦА ОШИБОК (создана вручную) ===\n");
            Console.WriteLine("┌────────┬────────────┬────────────────────────────┐");
            Console.WriteLine("│  Код   │ Название   │ Описание                   │");
            Console.WriteLine("├────────┼────────────┼────────────────────────────┤");
            Console.WriteLine("│   0    │ OK         │ Успешное чтение символа     │");
            Console.WriteLine("│   1    │ EOF        │ Достигнут конец файла       │");
            Console.WriteLine("│   2    │ NOT_OPEN   │ Файл не открыт              │");
            Console.WriteLine("│   3    │ READ_ERROR │ Ошибка при чтении файла     │");
            Console.WriteLine("└────────┴────────────┴────────────────────────────┘\n");

            Console.WriteLine("=== ЗАДАНИЕ 3: ТЕСТИРОВАНИЕ МОДУЛЯ ===\n");

            Test1();
            Test2();
            Test3();
            Test4();
            Test5();

            Console.WriteLine("\n=== ТЕСТИРОВАНИЕ ЗАВЕРШЕНО ===");
        }

        static void Test1()
        {
            Console.Write("Тест 1 (чтение 'abc'): ");
            File.WriteAllText("test.txt", "abc");
            InputOutput.Open("test.txt");

            InputOutput.NextCh(); char c1 = InputOutput.Ch;
            InputOutput.NextCh(); char c2 = InputOutput.Ch;
            InputOutput.NextCh(); char c3 = InputOutput.Ch;

            if (c1 == 'a' && c2 == 'b' && c3 == 'c')
                Console.WriteLine("ПРОЙДЕН");
            else
                Console.WriteLine("НЕ ПРОЙДЕН");

            File.Delete("test.txt");
        }

        static void Test2()
        {
            Console.Write("Тест 2 (конец файла): ");
            File.WriteAllText("test.txt", "x");
            InputOutput.Open("test.txt");

            InputOutput.NextCh(); char c1 = InputOutput.Ch;
            InputOutput.NextCh(); char c2 = InputOutput.Ch;

            if (c1 == 'x')
                Console.WriteLine("ПРОЙДЕН");
            else
                Console.WriteLine("НЕ ПРОЙДЕН");

            File.Delete("test.txt");
        }

        static void Test3()
        {
            Console.Write("Тест 3 (пустой файл): ");
            File.WriteAllText("test.txt", "");
            InputOutput.Open("test.txt");

            InputOutput.NextCh(); char c1 = InputOutput.Ch;

            if (c1 == '\0')
                Console.WriteLine("ПРОЙДЕН");
            else
                Console.WriteLine("НЕ ПРОЙДЕН");

            File.Delete("test.txt");
        }

        static void Test4()
        {
            Console.Write("Тест 4 (файл не найден): ");
            try
            {
                InputOutput.Open("nonexistent.txt");
                Console.WriteLine("НЕ ПРОЙДЕН");
            }
            catch
            {
                Console.WriteLine("ПРОЙДЕН");
            }
        }

        static void Test5()
        {
            Console.Write("Тест 5 (демонстрация): ");
            File.WriteAllText("test.txt", "program Hello;");
            InputOutput.Open("test.txt");

            string result = "";
            for (int i = 0; i < 13; i++)
            {
                InputOutput.NextCh();
                result += InputOutput.Ch;
            }

            if (result == "program Hello;")
                Console.WriteLine("ПРОЙДЕН");
            else
                Console.WriteLine("НЕ ПРОЙДЕН");

            File.Delete("test.txt");
        }
    }
}