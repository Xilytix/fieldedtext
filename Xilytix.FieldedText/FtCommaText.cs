// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;
using System.Text;

namespace Xilytix.FieldedText
{
    [Serializable]
    public class FtCommaTextFormatException : FormatException
    {
        public FtCommaTextFormatException(string message) : base(message)
        {
        }
    };

    public class FtCommaText
    {
        private enum InQuotes { NotIn, In, CheckingStuffed };

        public const char DelimiterChar = ',';
        public const char QuoteChar = '"';
        public const string PairQuoteChar = "\"\"";

        private static void Append(string value, StringBuilder bldr, Boolean first, Boolean quoted)
        {
            if (!first)
            {
                bldr.Append(DelimiterChar);
            };
            if (!quoted)
            {
                bldr.Append(value);
            }
            else
            {
                string tempStr = value.Replace(QuoteChar.ToString(), PairQuoteChar);
                bldr.Append(QuoteChar);
                bldr.Append(tempStr);
                bldr.Append(QuoteChar);
            }
        }

        public static string Get(string[] value)
        {
            return Get(value, 0, value.Length);
        }

        public static string Get(string[] value, int startIndex, int length)
        {
            int estimatedLength = length * 3; // allow for ","
            for (int i = startIndex; i < startIndex + length; i++)
            {
                estimatedLength += value[i].Length;
            }

            StringBuilder bldr = new StringBuilder(estimatedLength);
            for (int i = startIndex; i < startIndex + length; i++)
            {
                if (value[i].IndexOf(DelimiterChar) >= 0)
                    Append(value[i], bldr, i == 0, true);
                else
                {
                    string tempStr = value[i].TrimStart();
                    bool quotingRequired = (tempStr != "") && (tempStr[0] == QuoteChar);
                    Append(value[i], bldr, i == 0, quotingRequired);
                }
            }

            return bldr.ToString();
        }

        private static void AddElement(string value, ref string[] resultArray, ref int Count, int startPos, int terminatePos, Boolean removeStuffedQuotes)
        {
            int idx = Count;
            Count++;
            int resultArrayLength = (resultArray == null) ? 0 : resultArray.Length;
            if (Count > resultArrayLength) 
            {
                Array.Resize<string>(ref resultArray, Count + 20);
            }
            string tempStr = value.Substring(startPos, terminatePos - startPos);
            if (removeStuffedQuotes)
            {
               tempStr = tempStr.Replace(PairQuoteChar, QuoteChar.ToString());
            };
            resultArray[idx] = tempStr;
        }

        public static string[] Parse(string value)
        {
            string errorDescription;
            string[] result;
            Boolean toResult;
            toResult = TryParse(value, false, out result, out errorDescription);
            if (!toResult || (errorDescription != ""))
                throw new FtCommaTextFormatException(string.Format(Properties.Resources.FtCommaText_Parse_Format, errorDescription));
            else
                return result;
        }

        public static bool TryParse(string value, bool strict, out string[] resultArray, out string errorDescription)
        {
            string[] strArray = null;
            errorDescription = "";
            int count = 0;
            InQuotes inQuotes = InQuotes.NotIn;
            bool waitingForDelimiter = false;
            int startPos = 0;
            int valueLength = value.Length;
            for (int i = 0; i < valueLength; i++)
            {
                char valueChar = value[i];
                if (waitingForDelimiter)
                {
                    if (valueChar == DelimiterChar)
                    {
                        waitingForDelimiter = false;
                        startPos = i + 1;
                    }
                    else
                    {
                        if (strict && (!Char.IsWhiteSpace(valueChar)))
                        {
                            errorDescription = string.Format(Properties.Resources.FtCommaText_TryParse_UnexpectedCharAfterQuotedElement, i);
                            break;
                        }
                    }
                }
                else
                {
                    switch (inQuotes)
                    {
                        case InQuotes.NotIn:
                            switch (valueChar)
                            {
                                case DelimiterChar:
                                    AddElement(value, ref strArray, ref count, startPos, i, false);
                                    startPos = i + 1;
                                    break;
                                case QuoteChar:
                                    if (value.Substring(startPos, i-startPos).Trim() == "")
                                    {     
                                        inQuotes = InQuotes.In;
                                        startPos = i + 1;
                                    }
                                    break;
                            }
                            break;
                        case InQuotes.In:
                            if (valueChar == QuoteChar)
                            {
                                inQuotes = InQuotes.CheckingStuffed;
                            }
                            break;
                        case InQuotes.CheckingStuffed:
                            switch (valueChar)
                            {
                                case QuoteChar:
                                    inQuotes = InQuotes.In;
                                    break;
                                case DelimiterChar:
                                    inQuotes = InQuotes.NotIn;
                                    AddElement(value, ref strArray, ref count, startPos, i - 1, true);
                                    startPos = i + 1;
                                    break;
                                default:
                                    inQuotes = InQuotes.NotIn;
                                    AddElement(value, ref strArray, ref count, startPos, i - 1, true);
                                    waitingForDelimiter = true;
                                    if (strict && (!(Char.IsWhiteSpace(valueChar))))
                                    {
                                        errorDescription = string.Format(Properties.Resources.FtCommaText_TryParse_UnexpectedCharAfterQuotedElement, i);
                                        break;
                                    }
                                    break;
                            }
                            break;
                        default:
                            throw FtInternalException.Create(InternalError.FtCommaText_TryParse_UnsupportedInQuotes1, inQuotes.ToString());
                    }
                }
            }

            if ((errorDescription == "") && (! waitingForDelimiter))
            {
                switch (inQuotes)
                {
                    case InQuotes.NotIn:
                        if (startPos <= valueLength)
                        {
                            AddElement(value, ref strArray, ref count, startPos, valueLength, false);
                        }
                        break;
                    case InQuotes.In:
                        if (strict)
                            errorDescription = Properties.Resources.FtCommaText_TryParse_QuotesNotClosedInLastElement;
                        else
                            AddElement(value, ref strArray, ref count, startPos, valueLength, true);
                        break;
                    case InQuotes.CheckingStuffed:
                        AddElement(value, ref strArray, ref count, startPos, valueLength - 1, true);
                        break;
                    default:
                        throw FtInternalException.Create(InternalError.FtCommaText_TryParse_UnsupportedInQuotes2, inQuotes.ToString());
                }
            }
             
            resultArray = new string[count];
            Array.Copy(strArray, resultArray, count);
            return errorDescription == "";
        }

        public static Boolean TryParse(string value, out string[] resultArray, out string errorDescription)
        {
            return TryParse(value, true, out resultArray, out errorDescription);
        }

        public static string Get(int[] value)
        {
            int I;
            string[] StrArray = new string[value.Length];
            for (I = 0; I < value.Length; I++)
            {
                StrArray[I] = value[I].ToString();
            }
            return Get(StrArray);
        }

        public static string Get(string Value1, string Value2)
        {
            string[] StrArray = new string[2];
            StrArray[0] = Value1;
            StrArray[1] = Value2;
            return Get(StrArray);
        }

        public static bool StrictValidate(string value, out string errorDescription)
        {
            string[] StrArray;
            return TryParse(value, out StrArray, out errorDescription);
        }

        public static bool TryParse(string Value, out int[] resultArray, out string errorDescription)
        {
            bool returnValue;
            int i;
            string[] strArray;
            returnValue = TryParse(Value, out strArray, out errorDescription);
            if (!returnValue)
                resultArray = null;
            else
            {
                resultArray = new int[strArray.Length];
                for (i = 0; i < strArray.Length; i++)
                {
                    if (!Int32.TryParse(strArray[i], out resultArray[i]))
                    {
                        returnValue = false;
                        errorDescription = string.Format(Properties.Resources.FtCommaText_TryParse_InvalidInteger, i, strArray[i]);
                        break;
                    }
                }
            }
            return returnValue;
        }
    }
}

