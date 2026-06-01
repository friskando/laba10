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
                Console.WriteLine("Файл нет!");
                return;
            }

            InputOutput.Init(filePath);

            LexicalAnalyzer la = new LexicalAnalyzer();

            while (InputOutput.Ch != '\0')
            {
                la.NextSym();
            }

            InputOutput.Finish();

            Console.ReadKey();
        }
    }
}