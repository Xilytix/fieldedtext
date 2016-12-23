// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;

namespace Xilytix.FieldedText.Serialization.Formatting
{
    internal class IntegerFieldFormatter : NumberFieldFormatter
    {
        internal long Parse(string text)
        {
            long result; 

            try
            {
                if (long.TryParse(text, Styles, Culture, out result))
                    return result;
                else
                    throw new FtSerializationException(FtSerializationError.FieldTextParse, string.Format(Properties.Resources.IntegerFieldFormatter_Parse_Invalid, text));
            }
            catch (ArgumentException inner)
            {
                throw new FtSerializationException(FtSerializationError.FieldTextParse, string.Format(Properties.Resources.IntegerFieldFormatter_Parse_Invalid, text) + " [" + inner.Message + "]", inner);
            }
        }

        internal string ToText(long value)
        {
            try
            {
                return value.ToString(Format, Culture);
            }
            catch (FormatException inner)
            {
                throw new FtSerializationException(FtSerializationError.FieldValueToText,
                                                   string.Format(Properties.Resources.IntegerFieldFormatter_ToText_Format, inner.Message),
                                                   inner);
            }
        }
    }
}
