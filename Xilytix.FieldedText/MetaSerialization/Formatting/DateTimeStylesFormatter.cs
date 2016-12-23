// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;
using System.Globalization;

namespace Xilytix.FieldedText.MetaSerialization.Formatting
{
    // use this instead of Enum().ToString and Enum().Parse to ensure compliance with standard 
    internal class DateTimeStylesFormatter
    {
        private struct BasicRec
        {
            public DateTimeStyles Flag;
            public string Text;
        }

        private static BasicRec[] basicRecArray =
        {
            new BasicRec {Flag = DateTimeStyles.AdjustToUniversal, Text = "AdjustToUniversal" },
            new BasicRec {Flag = DateTimeStyles.AllowInnerWhite, Text = "AllowInnerWhite" },
            new BasicRec {Flag = DateTimeStyles.AllowLeadingWhite, Text = "AllowLeadingWhite" },
            new BasicRec {Flag = DateTimeStyles.AllowTrailingWhite, Text = "AllowTrailingWhite" },
            new BasicRec {Flag = DateTimeStyles.AssumeLocal, Text = "AssumeLocal" },
            new BasicRec {Flag = DateTimeStyles.AssumeUniversal, Text = "AssumeUniversal" },
            new BasicRec {Flag = DateTimeStyles.NoCurrentDateDefault, Text = "NoCurrentDateDefault" },
            new BasicRec {Flag = DateTimeStyles.RoundtripKind, Text = "RoundtripKind" },
        };

        private struct CompositeRec
        {
            public DateTimeStyles Styles;
            public string Text;
        }

        private static CompositeRec[] compositeRecArray =
        {
            new CompositeRec {Styles = DateTimeStyles.None, Text = "None" },
            new CompositeRec {Styles = DateTimeStyles.AllowWhiteSpaces, Text = "AllowWhiteSpaces" },
        };

        private static bool TryParseText(string text, out DateTimeStyles styles)
        {
            styles = 0;
            bool result = false;

            foreach (BasicRec rec in basicRecArray)
            {
                if (string.Equals(rec.Text, text, StringComparison.OrdinalIgnoreCase))
                {
                    styles = rec.Flag;
                    result = true;
                    break;
                }
            }

            if (result)
                return true;
            else
            {
                foreach (CompositeRec rec in compositeRecArray)
                {
                    if (string.Equals(rec.Text, text, StringComparison.OrdinalIgnoreCase))
                    {
                        styles = rec.Styles;
                        result = true;
                        break;
                    }
                }

                return result;
            }
        }

        internal static string ToAttributeValue(DateTimeStyles styles)
        {
            string[] textArray = new string[basicRecArray.Length];
            int count = 0;

            // Composite must match exactly
            for (int i = 0; i < compositeRecArray.Length; i++)
            {
                if (styles == compositeRecArray[i].Styles)
                {
                    textArray[count++] = compositeRecArray[i].Text;
                    break;
                }
            }

            if (count > 0)
                return FtCommaText.Get(textArray, 0, count);
            else
            {
                foreach (BasicRec rec in basicRecArray)
                {
                    if (styles.HasFlag(rec.Flag))
                    {
                        textArray[count++] = rec.Text;
                    }
                }
                return FtCommaText.Get(textArray, 0, count);
            }
        }

        internal static bool TryParseAttributeValue(string attributeValue, out DateTimeStyles styles)
        {
            string[] textArray;
            string errorDescription;
            styles = DateTimeStyles.None;
            attributeValue = attributeValue.Trim();

            if (attributeValue == "")
                return true;
            else
            {
                if (!FtCommaText.TryParse(attributeValue, out textArray, out errorDescription))
                    return false;
                else
                {
                    bool result = true;
                    foreach (string text in textArray)
                    {
                        DateTimeStyles flags;
                        if (TryParseText(text.Trim(), out flags))
                            styles |= flags;
                        else
                        {
                            result = false;
                            break;
                        }
                    }

                    return result;
                }
            }
        }
    }
}
