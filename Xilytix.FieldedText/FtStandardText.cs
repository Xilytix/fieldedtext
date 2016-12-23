// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;
using System.Xml;
using System.Globalization;

namespace Xilytix.FieldedText
{
    public struct FtStandardText
    {
        public const string BooleanFalseText = "False";
        public const string BooleanTrueText = "True";

        public const NumberStyles IntegerNumberStyles = NumberStyles.Integer;
        public const NumberStyles FloatNumberStyles = NumberStyles.Float;
        public const NumberStyles DecimalNumberStyles = NumberStyles.Integer | NumberStyles.AllowDecimalPoint;

        public const string DateInvariantFormat = "yyyyMMdd";
        public const string TimeInvariantFormat = "HHmmss";
        public const string DateTimeInvariantFormat = DateInvariantFormat + TimeInvariantFormat;
        public const char TimeAndFractionSeparatorChar = '.';
        public const string TimeWithFractionInvariantFormat = TimeInvariantFormat + ".fffffff";

        /// <summary>
        /// DateTime is considered as Time only if date portion of DateTime is set to NullDate 
        /// </summary>
        public static readonly DateTime NullDate = new DateTime(0);

        // string functions don't do much. Just here for consistency
        public static string Get(string value) { return value; }
        public static string ParseString(string text) { return text; }
        public static bool TryParse(string text, out string value) { value = text; return true; }

        // Boolean
        public static string Get(bool value) { return value ? BooleanTrueText : BooleanFalseText; }
        public static bool ParseBoolean(string text)
        {
            bool result;
            if (TryParse(text, out result))
                return result;
            else
                throw new FormatException(string.Format(Properties.Resources.FtStandardText_ParseBoolean_Invalid, text));
        }
        public static bool TryParse(string text, out bool value)
        {
            if (string.Equals(text, BooleanFalseText, StringComparison.OrdinalIgnoreCase))
            {
                value = false;
                return true;
            }
            else
            {
                if (string.Equals(text, BooleanTrueText, StringComparison.OrdinalIgnoreCase))
                {
                    value = true;
                    return true;
                }
                else
                {
                    value = false;
                    return false;
                }
            }
        }

        // Integer
        public static string Get(long value) { return value.ToString(CultureInfo.InvariantCulture); }
        public static long ParseInteger(string text)
        {
            long result;
            if (TryParse(text, out result))
                return result;
            else
                throw new FormatException(string.Format(Properties.Resources.FtStandardText_ParseInteger_Invalid, text));
        }
        public static bool TryParse(string text, out long value)
        {
            return long.TryParse(text, IntegerNumberStyles, CultureInfo.InvariantCulture, out value);
        }

        // Float
        public static string Get(double value) { return value.ToString(CultureInfo.InvariantCulture); }
        public static double ParseFloat(string text)
        {
            double result;
            if (TryParse(text, out result))
                return result;
            else
                throw new FormatException(string.Format(Properties.Resources.FtStandardText_ParseFloat_Invalid, text));
        }
        public static bool TryParse(string text, out double value)
        {
            return double.TryParse(text, FloatNumberStyles, CultureInfo.InvariantCulture, out value);
        }

        // Decimal
        public static string Get(decimal value) { return value.ToString(CultureInfo.InvariantCulture); }
        public static decimal ParseDecimal(string text)
        {
            decimal result;
            if (TryParse(text, out result))
                return result;
            else
                throw new FormatException(string.Format(Properties.Resources.FtStandardText_ParseDecimal_Invalid, text));
        }
        public static bool TryParse(string text, out decimal value)
        {
            return decimal.TryParse(text, DecimalNumberStyles, CultureInfo.InvariantCulture, out value);
        }

        // DateTime
        public static string Get(DateTime value)
        {
            if (value.TimeOfDay.Ticks == 0)
                return value.ToString(DateInvariantFormat, CultureInfo.InvariantCulture);
            else
            {
                string dateFormat;
                if (value.Date == NullDate)
                    dateFormat = "";
                else
                    dateFormat = DateInvariantFormat;

                string untrimmedResult = value.ToString(dateFormat + TimeWithFractionInvariantFormat, CultureInfo.InvariantCulture);

                int length = untrimmedResult.Length;
                while ((length > 0) && (untrimmedResult[length - 1] == '0')) { length--; }
                if ((length > 0) && (untrimmedResult[length - 1] == TimeAndFractionSeparatorChar)) { length--; }
                if (length == untrimmedResult.Length)
                    return untrimmedResult;
                else
                    return untrimmedResult.Substring(0, length);
            }
        }
        public static DateTime ParseDateTime(string text)
        {
            DateTime result;
            if (TryParse(text, out result))
                return result;
            else
                throw new FormatException(string.Format(Properties.Resources.FtStandardText_ParseDateTime_Invalid, text));
        }
        public static bool TryParse(string text, out DateTime value)
        {
            text = text.Trim();

            if (text.Length == 0)
            {
                value = NullDate; // avoid compiler error
                return false;
            }
            else
            {
                string nonFracFormat;
                string nonFracText;
                bool lengthOk = true;

                int fracSeparatorIdx = text.IndexOf(TimeAndFractionSeparatorChar);
                bool hasFrac = fracSeparatorIdx >= 0;
                if (!hasFrac)
                    nonFracText = text;
                else
                    nonFracText = text.Substring(0, fracSeparatorIdx);

                if (nonFracText.Length == 0)
                    nonFracFormat = "";
                else
                {
                    if (nonFracText.Length == DateInvariantFormat.Length)
                        nonFracFormat = DateInvariantFormat;
                    else
                    {
                        if (nonFracText.Length == TimeInvariantFormat.Length)
                            nonFracFormat = TimeInvariantFormat;
                        else
                        {
                            if (nonFracText.Length == DateTimeInvariantFormat.Length)
                                nonFracFormat = DateTimeInvariantFormat;
                            else
                            {
                                nonFracFormat = ""; // avoid compiler error
                                lengthOk = false;
                            }
                        }
                    }
                }

                if (!lengthOk)
                {
                    value = NullDate; // avoid compiler error
                    return false;
                }
                else
                {
                    string parseFormat;
                    string parseText;

                    if (!hasFrac || (fracSeparatorIdx == text.Length-1))
                    {
                        parseFormat = nonFracFormat;
                        parseText = nonFracText;
                    }
                    else
                    {
                        string fracText = text.Substring(fracSeparatorIdx + 1);
                        if (fracText.Length > 6)
                        {
                            fracText = fracText.Substring(0, 6);
                        }
                        parseFormat = nonFracFormat + TimeAndFractionSeparatorChar + new string('f', fracText.Length);
                        parseText = nonFracText + TimeAndFractionSeparatorChar + fracText;
                    }

                    return DateTime.TryParseExact(parseText, parseFormat, CultureInfo.InvariantCulture, DateTimeStyles.NoCurrentDateDefault, out value);
                }
            }
        }

        // Char
        private const char NumericCharacterReferencePrefix = '#';
        private const char HexNumericCharacterReferenceSpecifier = 'x';
        private const string HexNumericCharacterReferencePrefix = "#x";
        private const char BadCharValue = '\0';
        public static string Get(char value)
        {
            bool invalid = false;
            try
            {
                XmlConvert.VerifyXmlChars(value.ToString());
            }
            catch (XmlException)
            {
                invalid = true;
            }

            if (!invalid)
                return value.ToString(CultureInfo.InvariantCulture);
            else
                return HexNumericCharacterReferencePrefix + ((int)value).ToString("X4", CultureInfo.InvariantCulture);
        }
        public static long ParseChar(string text)
        {
            char result;
            if (TryParse(text, out result))
                return result;
            else
                throw new FormatException(string.Format(Properties.Resources.FtStandardText_ParseChar_Invalid, text));
        }

        public static bool TryParse(string text, out char value)
        {
            int textLength = text.Length;

            if (textLength == 1)
            {
                value = text[0];
                return true;
            }
            else
            {
                if (textLength <= 1 || text[0] != NumericCharacterReferencePrefix)
                {
                    value = BadCharValue;
                    return false;
                }
                else
                {
                    NumberStyles numberStyles;
                    int numberStart;
                    if (text[1] == HexNumericCharacterReferenceSpecifier)
                    {
                        numberStyles = NumberStyles.HexNumber;
                        numberStart = 2;
                    }
                    else
                    {
                        numberStyles = NumberStyles.Integer;
                        numberStart = 1;
                    }

                    if (numberStart >= textLength)
                    {
                        value = BadCharValue;
                        return false;
                    }
                    else
                    {
                        string numberStr = text.Substring(numberStart);
                        int charAsInt;
                        bool result = int.TryParse(numberStr, numberStyles, CultureInfo.InvariantCulture, out charAsInt);
                        if (!result)
                            value = BadCharValue;
                        else
                        {
                            try
                            {
                                value = (char)charAsInt;
                            }
                            catch (ArgumentOutOfRangeException)
                            {
                                value = BadCharValue;
                                result = false;
                            }
                        }

                        return result;
                    }
                }
            }
        }

        // Int32
        public static string Get(int value) { return value.ToString(CultureInfo.InvariantCulture); }
        public static long ParseInt32(string text)
        {
            long result;
            if (TryParse(text, out result))
                return result;
            else
                throw new FormatException(string.Format(Properties.Resources.FtStandardText_ParseInt32_Invalid, text));
        }
        public static bool TryParse(string text, out int value)
        {
            return int.TryParse(text, IntegerNumberStyles, CultureInfo.InvariantCulture, out value);
        }
    }
}
