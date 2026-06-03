using System;

namespace ConsoleApp1
{
    class LexicalAnalyzer
    {
        public const byte dosy = 1, ifsy = 2, insy = 3, ofsy = 4, orsy = 5, tosy = 6,
                          endsy = 7, varsy = 8, divsy = 9, andsy = 10, notsy = 11, forsym = 12,
                          modsy = 13, nilsy = 14, setsy = 16, thensy = 17, elsesy = 18,
                          casesy = 19, filesy = 20, gotosy = 21, typesy = 22, withsy = 23,
                          beginsy = 24, whilesy = 25, arraysy = 26, constsy = 27, labelsy = 28,
                          untilsy = 29, downtosy = 30, packedsy = 31, recordsy = 32,
                          repeatsy = 33, programsy = 34, functionsy = 35, procedurensy = 36;

        // Код для обычного идентификатора (не ключевое слово)
        public const byte identsy = 50;

        private bool inComment = false;
        private bool inBraceComment = false;
        private int commentStartLine = 0;
        private int commentStartPos = 0;
        private int braceStartLine = 0;
        private int braceStartPos = 0;

        // Таблица ключевых слов
        private Keywords keywords;

        // Допустимые специальные символы Паскаля
        private const string validSpecialChars = "+-*/=<>:;,.()[]{}' ^@#$\\";

        public void Analyze()
        {
            InputOutput.ResetErrors();
            inComment = false;
            inBraceComment = false;

            // Инициализация таблицы ключевых слов
            keywords = new Keywords();

            while (InputOutput.Ch != '\0')
            {
                SkipWhitespace();
                if (InputOutput.Ch == '\0') break;

                int line = (int)InputOutput.PositionNow.lineNumber;
                int pos = InputOutput.PositionNow.charNumber;

                // ====== ВНУТРИ БЛОЧНОГО КОММЕНТАРИЯ (* ... *) ======
                if (inComment)
                {
                    if (InputOutput.Ch == '*' && InputOutput.PeekNext() == ')')
                    {
                        inComment = false;
                        InputOutput.NextCh();
                        InputOutput.NextCh();
                    }
                    else
                    {
                        int currentLine = (int)InputOutput.PositionNow.lineNumber;
                        InputOutput.NextCh();
                        int newLine = (int)InputOutput.PositionNow.lineNumber;

                        if (newLine != currentLine)
                        {
                            InputOutput.AddError(commentStartLine, commentStartPos, 205,
                                "Незакрытый блочный комментарий (* ... *)");
                            inComment = false;
                        }
                    }
                    continue;
                }

                // ====== ВНУТРИ БЛОЧНОГО КОММЕНТАРИЯ { ... } ======
                if (inBraceComment)
                {
                    if (InputOutput.Ch == '}')
                    {
                        inBraceComment = false;
                        InputOutput.NextCh();
                    }
                    else
                    {
                        int currentLine = (int)InputOutput.PositionNow.lineNumber;
                        InputOutput.NextCh();
                        int newLine = (int)InputOutput.PositionNow.lineNumber;

                        if (newLine != currentLine)
                        {
                            InputOutput.AddError(braceStartLine, braceStartPos, 207,
                                "Незакрытый альтернативный блочный комментарий { ... }");
                            inBraceComment = false;
                        }
                    }
                    continue;
                }

                // ====== ОТКРЫТИЕ (* ======
                if (InputOutput.Ch == '(' && InputOutput.PeekNext() == '*')
                {
                    inComment = true;
                    commentStartLine = line;
                    commentStartPos = pos;
                    InputOutput.NextCh();
                    InputOutput.NextCh();
                    continue;
                }

                // ====== ОТКРЫТИЕ { ======
                if (InputOutput.Ch == '{')
                {
                    inBraceComment = true;
                    braceStartLine = line;
                    braceStartPos = pos;
                    InputOutput.NextCh();
                    continue;
                }

                // ====== ОДНОСТРОЧНЫЙ // ======
                if (InputOutput.Ch == '/' && InputOutput.PeekNext() == '/')
                {
                    SkipLineComment();
                    continue;
                }

                // ====== СТРОКА ======
                if (InputOutput.Ch == '\'')
                {
                    ScanString();
                    continue;
                }

                // ====== ЧИСЛО ======
                if (Char.IsDigit(InputOutput.Ch))
                {
                    ScanNumber(line, pos);
                    continue;
                }

                // ====== ИДЕНТИФИКАТОР ИЛИ КЛЮЧЕВОЕ СЛОВО ======
                if (Char.IsLetter(InputOutput.Ch))
                {
                    // Собираем полное имя идентификатора
                    string name = "";
                    while (Char.IsLetterOrDigit(InputOutput.Ch))
                    {
                        name += InputOutput.Ch;
                        InputOutput.NextCh();
                    }

                    // ПОИСК ПО ТАБЛИЦЕ КЛЮЧЕВЫХ СЛОВ
                    byte code = keywords.CheckKeyword(name);

                    // Если это не ключевое слово, то это обычный идентификатор
                    if (code == 0)
                    {
                        code = identsy; // код 50
                    }

                    // Здесь code содержит код лексемы (1-36 для ключевых слов, 50 для идентификатора)
                    // В дальнейшем этот код можно записать в файл кодов

                    continue;
                }

                // ====== ТОЧКА С ЗАПЯТОЙ - ОСОБАЯ ОБРАБОТКА ======
                if (InputOutput.Ch == ';')
                {
                    CheckAfterSemicolon(line, pos);
                    continue;
                }

                // ====== ПРОВЕРКА ДОПУСТИМЫХ СПЕЦИАЛЬНЫХ СИМВОЛОВ ======
                if (IsValidPascalChar(InputOutput.Ch))
                {
                    InputOutput.NextCh();
                    continue;
                }

                // ====== НЕДОПУСТИМЫЙ СИМВОЛ ======
                InputOutput.AddError(line, pos, 208,
                    $"Недопустимый символ '{InputOutput.Ch}'");
                InputOutput.NextCh();
            }

            if (inComment)
                InputOutput.AddError(commentStartLine, commentStartPos, 205,
                    "Незакрытый блочный комментарий (* ... *)");
            if (inBraceComment)
                InputOutput.AddError(braceStartLine, braceStartPos, 207,
                    "Незакрытый альтернативный блочный комментарий { ... }");
        }

        // Проверка содержимого после точки с запятой в той же строке
        private void CheckAfterSemicolon(int semicolonLine, int semicolonPos)
        {
            int currentLine = (int)InputOutput.PositionNow.lineNumber;
            InputOutput.NextCh();

            while (InputOutput.Ch != '\0')
            {
                int checkLine = (int)InputOutput.PositionNow.lineNumber;

                if (checkLine != currentLine)
                {
                    return;
                }

                if (Char.IsWhiteSpace(InputOutput.Ch))
                {
                    InputOutput.NextCh();
                    continue;
                }

                if (InputOutput.Ch == '/' && InputOutput.PeekNext() == '/') return;
                if (InputOutput.Ch == '(' && InputOutput.PeekNext() == '*') return;
                if (InputOutput.Ch == '{') return;

                int errPos = (int)InputOutput.PositionNow.charNumber;
                InputOutput.AddError(semicolonLine, errPos, 209,
                    "Недопустимые символы после ';' в одной строке");

                while (InputOutput.Ch != '\0')
                {
                    int skipCheckLine = (int)InputOutput.PositionNow.lineNumber;
                    if (skipCheckLine != currentLine) break;
                    InputOutput.NextCh();
                }
                return;
            }
        }

        private bool IsValidPascalChar(char c)
        {
            return validSpecialChars.IndexOf(c) >= 0;
        }

        private void SkipWhitespace()
        {
            while (InputOutput.Ch != '\0' && Char.IsWhiteSpace(InputOutput.Ch))
                InputOutput.NextCh();
        }

        private void SkipLineComment()
        {
            InputOutput.NextCh();
            InputOutput.NextCh();
            int currentLine = (int)InputOutput.PositionNow.lineNumber;
            while (InputOutput.Ch != '\0')
            {
                int checkLine = (int)InputOutput.PositionNow.lineNumber;
                if (checkLine != currentLine) break;
                InputOutput.NextCh();
            }
        }

        private void ScanString()
        {
            int startLine = (int)InputOutput.PositionNow.lineNumber;
            int startPos = InputOutput.PositionNow.charNumber;
            InputOutput.NextCh();

            bool closed = false;
            int currentLine = startLine;

            while (InputOutput.Ch != '\0')
            {
                int checkLine = (int)InputOutput.PositionNow.lineNumber;
                if (checkLine != currentLine)
                {
                    break;
                }

                if (InputOutput.Ch == '\'')
                {
                    if (InputOutput.PeekNext() == '\'')
                        InputOutput.NextCh();
                    else
                    {
                        closed = true;
                        InputOutput.NextCh();
                        break;
                    }
                }
                InputOutput.NextCh();
            }

            if (!closed)
            {
                InputOutput.AddError(startLine, startPos, 206, "Незакрытая строковая константа");
            }
        }

        private void ScanNumber(int line, int pos)
        {
            long value = 0;
            bool overflow = false;

            while (InputOutput.Ch != '\0' && Char.IsDigit(InputOutput.Ch))
            {
                int digit = InputOutput.Ch - '0';
                if (value > (32767 - digit) / 10)
                    overflow = true;
                value = value * 10 + digit;
                InputOutput.NextCh();
            }

            if (overflow || value > 32767)
            {
                InputOutput.AddError(line, pos, 203, "Число слишком большое для integer (максимум 32767)");
            }
        }
    }
}