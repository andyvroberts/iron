using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataReader.Models;

namespace DataReader.Common
{
    internal static class CharacterTypeDecode
    {
        internal static CharType DetermineChar(char decodeChar)
        {
            CharType retChar = CharType.Other;

            if (char.IsLetter(decodeChar))
            {
                retChar = CharType.Letter;
            }
            else if (char.IsDigit(decodeChar))
            {
                retChar = CharType.Digit;
            }
            else if (char.IsControl(decodeChar))
            {
                switch (decodeChar)
                {
                    case '\n':
                        retChar = CharType.Newline;
                        break;
                    case '\r':
                        retChar = CharType.CR;
                        break;
                    case '\t':
                        retChar = CharType.Tab;
                        break;
                    case '\"':
                        retChar = CharType.DoubleQuote;
                        break;
                    case '\'':
                        retChar = CharType.SingleQuote;
                        break;
                    default:
                        retChar = CharType.Other;
                        break;
                }
            }
            else if (char.IsPunctuation(decodeChar))
            {
                retChar = CharType.Punctuation;
            }

            return retChar;
        }


    }
}
