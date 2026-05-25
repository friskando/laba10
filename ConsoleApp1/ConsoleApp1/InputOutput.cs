using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp1
{
    struct TextPosition
    {
        private uint _lineNumber;
        private byte _charNumber;

        public uint LineNumber
        {
            get { return _lineNumber; }
            set { _lineNumber = value; }
        }

        public byte CharNumber
        {
            get { return _charNumber; }
            set { _charNumber = value; }
        }

        public TextPosition(uint ln = 0, byte c = 0)
        {
            _lineNumber = ln;
            _charNumber = c;
        }
    }

    struct Err
    {
        private TextPosition _errorPosition;
        private byte _errorCode;

        public TextPosition ErrorPosition
        {
            get { return _errorPosition; }
            set { _errorPosition = value; }
        }

        public byte ErrorCode
        {
            get { return _errorCode; }
            set { _errorCode = value; }
        }

        public Err(TextPosition errorPosition, byte errorCode)
        {
            _errorPosition = errorPosition;
            _errorCode = errorCode;
        }
    }

    class InputOutput
    {
        private const byte ERRMAX = 9;
        private static char _ch;
        private static TextPosition _positionNow = new TextPosition();
        private static string _line;
        private static byte _lastInLine = 0;
        private static List<Err> _err;
        private static StreamReader _file;
        private static uint _errCount = 0;
        private static bool _firstLine = true;

        public static char Ch
        {
            get { return _ch; }
            private set { _ch = value; }
        }

        public static TextPosition PositionNow
        {
            get { return _positionNow; }
            private set { _positionNow = value; }
        }

        static public void Open(string fileName)
        {
            _file = new StreamReader(fileName);
            _err = new List<Err>();
            _line = null;
            _positionNow = new TextPosition(0, 0);
            _errCount = 0;
            _firstLine = true;
        }

        static public void NextCh()
        {
            if (_file == null || (_line == null && _file.EndOfStream))
            {
                Ch = '\0';
                return;
            }

            if (_line == null || _positionNow.CharNumber >= _lastInLine)
            {
                if (!_firstLine && !string.IsNullOrEmpty(_line))
                    ListThisLine();

                if (_err != null && _err.Count > 0)
                    ListErrors();

                ReadNextLine();

                if (_firstLine)
                {
                    _positionNow.LineNumber = 1;
                    _firstLine = false;
                }
                else
                {
                    _positionNow.LineNumber++;
                }

                _positionNow.CharNumber = 0;
            }

            if (_line != null && _positionNow.CharNumber < _line.Length)
            {
                Ch = _line[_positionNow.CharNumber];
                _positionNow.CharNumber++;
            }
            else
            {
                Ch = '\0';
            }
        }

        private static void ListThisLine()
        {
            if (!string.IsNullOrEmpty(_line))
                Console.WriteLine(_line);
        }

        private static void ReadNextLine()
        {
            if (_file != null && !_file.EndOfStream)
            {
                _line = _file.ReadLine();
                _lastInLine = (byte)(_line?.Length ?? 0);
                _err = new List<Err>();
            }
            else
            {
                _line = null;
                _lastInLine = 0;
            }
        }

        static void End()
        {
            Console.WriteLine($"Компиляция завершена: ошибок — {_errCount}!");
        }

        static void ListErrors()
        {
            if (_err == null) return;

            int pos = 6 - $"{_positionNow.LineNumber} ".Length;
            string s;
            foreach (Err item in _err)
            {
                ++_errCount;
                s = "**";
                if (_errCount < 10) s += "0";
                s += $"{_errCount}**";
                while (s.Length - 1 < pos + item.ErrorPosition.CharNumber) s += " ";
                s += $"^ ошибка код {item.ErrorCode}";
                Console.WriteLine(s);
            }
        }

        static public void Error(byte errorCode, TextPosition position)
        {
            Err e;
            if (_err != null && _err.Count <= ERRMAX)
            {
                e = new Err(position, errorCode);
                _err.Add(e);
            }
        }
    }
}