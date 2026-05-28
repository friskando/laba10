using System;

namespace ConsoleApp1
{
    class LexicalAnalyzer
    {
        public const byte
            star = 21,
            slash = 60,
            equal = 16,
            comma = 20,
            semicolon = 14,
            colon = 5,
            point = 61,
            arrow = 62,
            leftpar = 9,
            rightpar = 4,
            lbracket = 11,
            rbracket = 12,
            flpar = 63,
            frpar = 64,
            later = 65,
            greater = 66,
            laterequal = 67,
            greaterequal = 68,
            latergreater = 69,
            plus = 70,
            minus = 71,
            lcomment = 72,
            rcomment = 73,
            assign = 51,
            twopoints = 74,
            ident = 2,
            floatc = 82,
            intc = 15,
            casesy = 31,
            elsesy = 32,
            filesy = 57,
            gotosy = 33,
            thensy = 52,
            typesy = 34,
            untilsy = 53,
            dosy = 54,
            withsy = 37,
            ifsy = 56,
            insy = 100,
            ofsy = 101,
            orsy = 102,
            tosy = 103,
            endsy = 104,
            varsy = 105,
            divsy = 106,
            andsy = 107,
            notsy = 108,
            forsy = 109,
            modsy = 110,
            nilsy = 111,
            setsy = 112,
            beginsy = 113,
            whilesy = 114,
            arraysy = 115,
            constsy = 116,
            labelsy = 117,
            downtosy = 118,
            packedsy = 119,
            recordsy = 120,
            repeatsy = 121,
            programsy = 122,
            functionsy = 123,
            procedurensy = 124;

        byte symbol;
        TextPosition token;
        string addrName;
        int nmb_int;
        float nmb_float;
        char one_symbol;

        // ДОБАВЛЕНО: для хранения текущей лексемы и таблицы ключевых слов
        private string currentLexeme;
        private Keywords keywordsTable;

        public string CurrentLexeme { get { return currentLexeme; } }

        public LexicalAnalyzer()
        {
            keywordsTable = new Keywords();
        }

        public byte NextSym()
        {
            while (InputOutput.Ch == ' ') InputOutput.NextCh();
            token.LineNumber = InputOutput.PositionNow.LineNumber;
            token.CharNumber = InputOutput.PositionNow.CharNumber;

            currentLexeme = "";

            if (InputOutput.Ch >= '0' && InputOutput.Ch <= '9')
            {
                byte digit;
                Int16 maxint = Int16.MaxValue;
                nmb_int = 0;
                while (InputOutput.Ch >= '0' && InputOutput.Ch <= '9')
                {
                    digit = (byte)(InputOutput.Ch - '0');
                    currentLexeme += InputOutput.Ch;
                    if (nmb_int < maxint / 10 ||
                    (nmb_int == maxint / 10 &&
                    digit <= maxint % 10))
                        nmb_int = 10 * nmb_int + digit;
                    else
                    {
                        InputOutput.Error(203, InputOutput.PositionNow);
                        nmb_int = 0;
                        while (InputOutput.Ch >= '0' && InputOutput.Ch <= '9') InputOutput.NextCh();
                    }
                    InputOutput.NextCh();
                }
                symbol = intc;
                return symbol;
            }
            else if ((InputOutput.Ch >= 'a' && InputOutput.Ch <= 'z') ||
                     (InputOutput.Ch >= 'A' && InputOutput.Ch <= 'Z'))
            {
                string name = "";
                while ((InputOutput.Ch >= 'a' && InputOutput.Ch <= 'z') ||
                        (InputOutput.Ch >= 'A' && InputOutput.Ch <= 'Z') ||
                        (InputOutput.Ch >= '0' && InputOutput.Ch <= '9'))
                {
                    name += InputOutput.Ch;
                    currentLexeme += InputOutput.Ch;
                    InputOutput.NextCh();
                }

                // ИЗМЕНЕНО: поиск по таблице ключевых слов
                int wordLength = name.Length;
                byte keywordCode = 0;

                if (keywordsTable.Kw.ContainsKey((byte)wordLength))
                {
                    var wordsOfLength = keywordsTable.Kw[(byte)wordLength];
                    if (wordsOfLength.ContainsKey(name.ToLower()))
                    {
                        keywordCode = wordsOfLength[name.ToLower()];
                    }
                }

                if (keywordCode != 0)
                    symbol = keywordCode;
                else
                    symbol = ident;

                return symbol;
            }
            else
            {
                currentLexeme = InputOutput.Ch.ToString();
                switch (InputOutput.Ch)
                {
                    case '<':
                        InputOutput.NextCh();
                        if (InputOutput.Ch == '=')
                        {
                            symbol = laterequal;
                            currentLexeme = "<=";
                            InputOutput.NextCh();
                        }
                        else
                            if (InputOutput.Ch == '>')
                        {
                            symbol = latergreater;
                            currentLexeme = "<>";
                            InputOutput.NextCh();
                        }
                        else
                            symbol = later;
                        break;
                    case ':':
                        InputOutput.NextCh();
                        if (InputOutput.Ch == '=')
                        {
                            symbol = assign;
                            currentLexeme = ":=";
                            InputOutput.NextCh();
                        }
                        else
                            symbol = colon;
                        break;
                    case ';':
                        symbol = semicolon;
                        InputOutput.NextCh();
                        break;
                    case '.':
                        InputOutput.NextCh();
                        if (InputOutput.Ch == '.')
                        {
                            symbol = twopoints;
                            currentLexeme = "..";
                            InputOutput.NextCh();
                        }
                        else symbol = point;
                        break;
                    case '+':
                        symbol = plus;
                        InputOutput.NextCh();
                        break;
                    case '-':
                        symbol = minus;
                        InputOutput.NextCh();
                        break;
                    case '*':
                        symbol = star;
                        InputOutput.NextCh();
                        break;
                    case '/':
                        symbol = slash;
                        InputOutput.NextCh();
                        break;
                    case '=':
                        symbol = equal;
                        InputOutput.NextCh();
                        break;
                    case ',':
                        symbol = comma;
                        InputOutput.NextCh();
                        break;
                    case '(':
                        symbol = leftpar;
                        InputOutput.NextCh();
                        break;
                    case ')':
                        symbol = rightpar;
                        InputOutput.NextCh();
                        break;
                    case '[':
                        symbol = lbracket;
                        InputOutput.NextCh();
                        break;
                    case ']':
                        symbol = rbracket;
                        InputOutput.NextCh();
                        break;
                    default:
                        if (InputOutput.Ch == '\0')
                            symbol = 0;
                        else
                        {
                            symbol = 255;
                            InputOutput.Error(1, InputOutput.PositionNow);
                            InputOutput.NextCh();
                        }
                        break;
                }
                return symbol;
            }
        }
    }
}