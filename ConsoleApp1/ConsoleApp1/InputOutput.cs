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

        private static char ch;
        private static TextPosition currentPosition;
        private static string line;
        private static byte lastInLine;
        private static StreamReader file;
        private static uint currentLineNumber;
        private static List<string> allLines;
        private static Dictionary<int, List<ErrorInfo>> errors;

        private class ErrorInfo
        {
            public int CharPos;
            public int Code;
            public string Message;
        }

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
            allLines = new List<string>();
            errors = new Dictionary<int, List<ErrorInfo>>();
            currentLineNumber = 0;
            currentPosition = new TextPosition();
            ReadNextLine();
        }

        public static char PeekNext()
        {
            if (currentPosition.charNumber + 1 <= lastInLine)
                return line[currentPosition.charNumber + 1];
            return '\0';
        }

        public static void NextCh()
        {
            if (Ch == '\0') return;

            if (currentPosition.charNumber >= lastInLine)
            {
                ReadNextLine();
                if (Ch == '\0') return;

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
                allLines.Add(line);
                currentLineNumber++;
                currentPosition.lineNumber = currentLineNumber;
                if (line.Length == 0) line = " ";
                lastInLine = (byte)(line.Length - 1);
                Ch = line[0];
                currentPosition.charNumber = 0;
            }
            else
            {
                Ch = '\0';
            }
        }

        public static void ResetErrors()
        {
            errors.Clear();
        }

        public static void AddError(int lineNumber, int charPos, int code, string message)
        {
            if (!errors.ContainsKey(lineNumber))
                errors[lineNumber] = new List<ErrorInfo>();
            errors[lineNumber].Add(new ErrorInfo { CharPos = charPos, Code = code, Message = message });
        }

        public static void Finish()
        {
            for (int i = 0; i < allLines.Count; i++)
            {
                int lineNum = i + 1;

                string prefix = $"{lineNum,4} ";
                Console.WriteLine($"{prefix}{allLines[i]}");

                if (errors.ContainsKey(lineNum))
                {
                    foreach (var err in errors[lineNum])
                    {
                        int visualPos = 0;
                        for (int j = 0; j < err.CharPos && j < allLines[i].Length; j++)
                        {
                            if (allLines[i][j] == '\t')
                                visualPos += 4 - (visualPos % 4);
                            else
                                visualPos++;
                        }

                        string arrow = new string(' ', visualPos) + "^";

                        Console.WriteLine($"{new string(' ', prefix.Length)}{arrow} ошибка {err.Code}: {err.Message}");
                    }
                }
            }

            int totalErrors = 0;
            foreach (var list in errors.Values)
                totalErrors += list.Count;

            Console.WriteLine();
            Console.WriteLine($"Компиляция завершена: ошибок — {totalErrors}");
            file.Close();
        }
    }
}