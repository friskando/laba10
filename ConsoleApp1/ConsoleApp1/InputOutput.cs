using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp1
{
    struct TextPosition
    {
        public uint lineNumber;
        public byte charNumber;

        public TextPosition(uint ln = 0, byte c = 0)
        {
            lineNumber = ln;
            charNumber = c;
        }
    }

    struct Err
    {
        public TextPosition errorPosition;
        public byte errorCode;

        public Err(TextPosition errorPosition, byte errorCode)
        {
            this.errorPosition = errorPosition;
            this.errorCode = errorCode;
        }
    }

    class InputOutput
    {
        const byte ERRMAX = 9;
        public static char Ch { get; set; }
        public static TextPosition positionNow = new TextPosition();
        static string line;
        static byte lastInLine = 0;
        public static List<Err> err;
        static StreamReader File { get; set; }
        static uint errCount = 0;
        static bool firstLine = true;

        static public void Open(string fileName)
        {
            File = new StreamReader(fileName);
            err = new List<Err>();
            line = null;
            positionNow = new TextPosition(0, 0);
            errCount = 0;
            firstLine = true;
        }

        static public void NextCh()
        {
            if (File == null || (line == null && File.EndOfStream))
            {
                Ch = '\0';
                return;
            }

            if (line == null || positionNow.charNumber >= lastInLine)
            {
                if (!firstLine && !string.IsNullOrEmpty(line))
                    ListThisLine();

                if (err != null && err.Count > 0)
                    ListErrors();

                ReadNextLine();

                if (firstLine)
                {
                    positionNow.lineNumber = 1;
                    firstLine = false;
                }
                else
                {
                    positionNow.lineNumber++;
                }
                positionNow.charNumber = 0;
            }

            if (line != null && positionNow.charNumber < line.Length)
            {
                Ch = line[positionNow.charNumber];
                positionNow.charNumber++;
            }
            else
            {
                Ch = '\0';
            }
        }

        private static void ListThisLine()
        {
            if (!string.IsNullOrEmpty(line))
                Console.WriteLine(line);
        }

        private static void ReadNextLine()
        {
            if (File != null && !File.EndOfStream)
            {
                line = File.ReadLine();
                lastInLine = (byte)(line?.Length ?? 0);
                err = new List<Err>();
            }
            else
            {
                line = null;
                lastInLine = 0;
            }
        }

        static void End()
        {
            Console.WriteLine($"Компиляция завершена: ошибок — {errCount}!");
        }

        static void ListErrors()
        {
            if (err == null) return;

            int pos = 6 - $"{positionNow.lineNumber} ".Length;
            string s;
            foreach (Err item in err)
            {
                ++errCount;
                s = "**";
                if (errCount < 10) s += "0";
                s += $"{errCount}**";
                while (s.Length - 1 < pos + item.errorPosition.charNumber) s += " ";
                s += $"^ ошибка код {item.errorCode}";
                Console.WriteLine(s);
            }
        }

        static public void Error(byte errorCode, TextPosition position)
        {
            Err e;
            if (err != null && err.Count <= ERRMAX)
            {
                e = new Err(position, errorCode);
                err.Add(e);
            }
        }
    }
}