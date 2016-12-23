// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;
using System.Globalization;

namespace Xilytix.FieldedText.Serialization.Formatting
{
    internal class DateTimeFieldFormatter: FieldFormatter
    {
        // try not to use the NotWidelySupported Styles
        internal const DateTimeStyles NotWidelySupportedStyles = DateTimeStyles.AdjustToUniversal |
                                                                 DateTimeStyles.AssumeLocal |
                                                                 DateTimeStyles.AssumeUniversal |
                                                                 DateTimeStyles.RoundtripKind;

        internal DateTimeStyles Styles { get; set; }
        internal string Format { get; set; }

        internal DateTime Parse(string text)
        {
            DateTime result;

            try
            {
                if (DateTime.TryParseExact(text, Format, Culture, Styles, out result))
                    return result;
                else
                    throw new FtSerializationException(FtSerializationError.FieldTextParse, string.Format(Properties.Resources.DateTimeFieldFormatter_Parse_Invalid, text));
            }
            catch (ArgumentException inner)
            {
                throw new FtSerializationException(FtSerializationError.FieldTextParse, string.Format(Properties.Resources.DateTimeFieldFormatter_Parse_Invalid, text) + " [" + inner.Message + "]", inner);
            }
        }
        internal string ToText(DateTime value)
        {
            try
            {
                return value.ToString(Format, Culture);
            }
            catch (FormatException inner)
            {
                throw new FtSerializationException(FtSerializationError.FieldValueToText, 
                                                   string.Format(Properties.Resources.DateTimeFieldFormatter_ToText_Format, inner.Message), 
                                                   inner);
            }
            catch (ArgumentOutOfRangeException inner)
            {
                throw new FtSerializationException(FtSerializationError.FieldValueToText,
                                                   string.Format(Properties.Resources.DateTimeFieldFormatter_ToText_ArgumentOutOfRange, inner.Message),
                                                   inner);
            }
        }
    }
}
