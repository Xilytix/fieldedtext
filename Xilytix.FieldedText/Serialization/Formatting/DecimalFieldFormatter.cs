﻿// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;

namespace Xilytix.FieldedText.Serialization.Formatting
{
    internal class DecimalFieldFormatter : NumberFieldFormatter
    {
        internal decimal Parse(string text)
        {
            decimal result;

            try
            {
                if (decimal.TryParse(text, Styles, Culture, out result))
                    return result;
                else
                    throw new FtSerializationException(FtSerializationError.FieldTextParse, string.Format(Properties.Resources.DecimalFieldFormatter_Parse_Invalid, text));
            }
            catch (ArgumentException inner)
            {
                throw new FtSerializationException(FtSerializationError.FieldTextParse, string.Format(Properties.Resources.DecimalFieldFormatter_Parse_Invalid, text) + " [" + inner.Message + "]", inner);
            }
        }
        internal string ToText(decimal value)
        {
            try
            {
                return value.ToString(Format, Culture);
            }
            catch (FormatException inner)
            {
                throw new FtSerializationException(FtSerializationError.FieldValueToText,
                                                   string.Format(Properties.Resources.DecimalFieldFormatter_ToText_Format, inner.Message),
                                                   inner);
            }
        }
    }
}
