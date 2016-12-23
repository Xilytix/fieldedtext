// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;

namespace Xilytix.FieldedText.Serialization.Formatting
{
    internal class BooleanFieldFormatter: FieldFormatter
    {
        internal string FalseText { get; set; }
        internal string TrueText { get; set; }
        internal FtBooleanStyles Styles { get; set; }

        private bool CompareText(string text, string stateText)
        {
            if (stateText == "")
            {
                if (text == "")
                    return true;
                else
                    return Styles.HasFlag(FtBooleanStyles.IgnoreTrailingChars);
            }
            else
            {
                if (text == "")
                    return false;
                else
                {
                    bool ignoreCase = Styles.HasFlag(FtBooleanStyles.IgnoreCase);
                    if (Styles.HasFlag(FtBooleanStyles.MatchFirstCharOnly))
                        return ignoreCase ? char.ToUpper(text[0], Culture) == char.ToUpper(stateText[0], Culture) : text[0] == stateText[0];
                    else
                    {
                        StringComparison comparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
                        if (!Styles.HasFlag(FtBooleanStyles.IgnoreTrailingChars))
                            return string.Equals(text, stateText, comparison);
                        else
                        {
                            int textLength = text.Length;
                            int stateTextLength = stateText.Length;
                            if (textLength < stateTextLength)
                                return false;
                            else
                            {
                                string adjustedText;
                                if (textLength == stateTextLength)
                                    adjustedText = text;
                                else
                                    adjustedText = text.Substring(0, stateTextLength);

                                return string.Equals(adjustedText, stateText, comparison);
                            }                             
                        }
                    }
                }
            }
        }

        internal bool Parse(string text)
        {
            if (CompareText(text, TrueText))
                return true;
            else
            {
                if (Styles.HasFlag(FtBooleanStyles.FalseIfNotMatchTrue))
                    return false;
                else
                {
                    if (CompareText(text, FalseText))
                        return false;
                    else
                        throw new FtSerializationException(FtSerializationError.FieldTextParse, string.Format(Properties.Resources.BooleanFieldFormatter_Parse_NoMatch, text));
                }
            }
        }

        internal string ToText(bool value)
        {
            return value ? TrueText : FalseText;
        }
    }
}
