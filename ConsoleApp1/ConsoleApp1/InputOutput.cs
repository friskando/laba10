using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp1
{
    class InputOutput
    {
        public struct TextPosition
        {
            public uint lineNumber;
            public byte charNumber;
        }

        public struct Err
        {
            public TextPosition errorPosition;
            public byte errorCode;
        }

        private const byte ERRMAX = 9;
        private static char ch;
        private static TextPosition currentPosition;
        private static string line;
        private static byte lastInLine;
        private static List<Err> err;
        private static StreamReader file;
        private static uint errCount;
        private static uint currentLineNumber;

        public static char Ch
        {
            get { return ch; }
            private set { ch = value; }
        }

        public static TextPosition PositionNow
        {
            get { return currentPosition; }
            private set { currentPosition = value; }
        }

        public static void Init(string fileName)
        {
            file = new StreamReader(fileName);
            err = new List<Err>();
            currentLineNumber = 0;
            errCount = 0;
            currentPosition = new TextPosition();
            ReadNextLine();
        }

        public static void NextCh()
        {
            if (Ch == '\0')
            {
                return;
            }

            if (currentPosition.charNumber >= lastInLine)
            {
                ListThisLine();
                if (err.Count > 0)
                {
                    ListErrors();
                }

                ReadNextLine();

                if (Ch == '\0')
                {
                    return;
                }

                currentPosition.lineNumber++;
                currentPosition.charNumber = 0;
            }
            else
            {
                currentPosition.charNumber++;
                Ch = line[currentPosition.charNumber];
            }
        }

        private static void ReadNextLine()
        {
            if (!file.EndOfStream)
            {
                line = file.ReadLine();
                if (line == null)
                {
                    Ch = '\0';
                    return;
                }

                currentLineNumber++;
                currentPosition.lineNumber = currentLineNumber;

                if (line.Length == 0)
                { 
                    line = " "; 
                }
                
                lastInLine = (byte)(line.Length - 1);
                err = new List<Err>();
                Ch = line[0];
                currentPosition.charNumber = 0;
            }
            else
            {
                Ch = '\0';
            }
        }

        private static void ListThisLine()
        {
            Console.WriteLine($"{currentLineNumber,4} {line}");
        }

        private static void ListErrors()
        {
            foreach (Err item in err)
            {
                errCount++;
                Console.WriteLine($"     ^ ошибка {item.errorCode}");
            }
        }

        public static void Error(byte errorCode, TextPosition position)
        {
            if (err.Count <= ERRMAX)
            {
                err.Add(new Err { errorPosition = position, errorCode = errorCode });
            }
        }

        private static void End()
        {
            Console.WriteLine();
            Console.WriteLine($"Компиляция завершена: ошибок — {errCount}");
        }

        public static void Finish()
        {
            if (line != null && line != " ")
            {
                ListThisLine();
                if (err.Count > 0)
                {
                    ListErrors();
                }
            }
            End();
            if (file != null)
            { 
                file.Close(); 
            }
        }
    }
}