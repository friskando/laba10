using System;

namespace ConsoleApp1
{
    class LexicalAnalyzer
    {
        private const byte intc = 15;

        public const byte dosy = 1, ifsy = 2, insy = 3, ofsy = 4, orsy = 5, tosy = 6,
                          endsy = 7, varsy = 8, divsy = 9, andsy = 10, notsy = 11, forsym = 12,
                          modsy = 13, nilsy = 14, setsy = 16, thensy = 17, elsesy = 18,
                          casesy = 19, filesy = 20, gotosy = 21, typesy = 22, withsy = 23,
                          beginsy = 24, whilesy = 25, arraysy = 26, constsy = 27, labelsy = 28,
                          untilsy = 29, downtosy = 30, packedsy = 31, recordsy = 32,
                          repeatsy = 33, programsy = 34, functionsy = 35, procedurensy = 36;

        private byte symbol;
        private InputOutput.TextPosition token;
        private int nmb_int;

        public byte NextSym()
        {
            while (InputOutput.Ch != '\0' && Char.IsWhiteSpace(InputOutput.Ch))
            {
                InputOutput.NextCh();
            }

            if (InputOutput.Ch == '\0')
                return 0;

            token.lineNumber = InputOutput.PositionNow.lineNumber;
            token.charNumber = InputOutput.PositionNow.charNumber;

            if (Char.IsDigit(InputOutput.Ch))
            {
                ScanInteger();
                return symbol;
            }

            InputOutput.NextCh();
            return 0;
        }

        private void ScanInteger()
        {
            Int16 maxint = Int16.MaxValue;
            nmb_int = 0;

            while (Char.IsDigit(InputOutput.Ch))
            {
                byte digit = (byte)(InputOutput.Ch - '0');

                if (nmb_int < maxint / 10 || (nmb_int == maxint / 10 && digit <= maxint % 10))
                {
                    nmb_int = nmb_int * 10 + digit;
                }
                else
                {
                    InputOutput.Error(203, token);
                    while (Char.IsDigit(InputOutput.Ch))
                    {
                        InputOutput.NextCh();
                    }
                    symbol = intc;
                    return;
                }
                InputOutput.NextCh();
            }
            symbol = intc;
        }
    }
}