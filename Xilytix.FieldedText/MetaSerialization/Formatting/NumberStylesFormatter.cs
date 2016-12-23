// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;
using System.Globalization;

namespace Xilytix.FieldedText.MetaSerialization.Formatting
{
    // use this instead of Enum().ToString and Enum().Parse to ensure compliance with standard 
    internal static class NumberStylesFormatter
    {
        private struct BasicRec
        {
            public NumberStyles Flag;
            public string Text;
        }

        private static BasicRec[] basicRecArray =
        {
            new BasicRec {Flag = NumberStyles.AllowCurrencySymbol, Text = "AllowCurrencySymbol" },
            new BasicRec {Flag = NumberStyles.AllowDecimalPoint, Text = "AllowDecimalPoint" },
            new BasicRec {Flag = NumberStyles.AllowExponent, Text = "AllowExponent" },
            new BasicRec {Flag = NumberStyles.AllowHexSpecifier, Text = "AllowHexSpecifier" },
            new BasicRec {Flag = NumberStyles.AllowLeadingSign, Text = "AllowLeadingSign" },
            new BasicRec {Flag = NumberStyles.AllowLeadingWhite, Text = "AllowLeadingWhite" },
            new BasicRec {Flag = NumberStyles.AllowParentheses, Text = "AllowParentheses" },
            new BasicRec {Flag = NumberStyles.AllowThousands, Text = "AllowThousands" },
            new BasicRec {Flag = NumberStyles.AllowTrailingSign, Text = "AllowTrailingSign" },
            new BasicRec {Flag = NumberStyles.AllowTrailingWhite, Text = "AllowTrailingWhite" },
        };

        private struct CompositeRec
        {
            public NumberStyles Styles;
            public string Text;
        }

        private static CompositeRec[] compositeRecArray =
        {
            new CompositeRec {Styles = NumberStyles.Any, Text = "Any" },
            new CompositeRec {Styles = NumberStyles.Currency, Text = "Currency" },
            new CompositeRec {Styles = NumberStyles.Float, Text = "Float" },
            new CompositeRec {Styles = NumberStyles.HexNumber, Text = "HexNumber" },
            new CompositeRec {Styles = NumberStyles.Integer, Text = "Integer" },
            new CompositeRec {Styles = NumberStyles.None, Text = "None" },
            new CompositeRec {Styles = NumberStyles.Number, Text = "Number" },
        };

        private static bool TryParseText(string text, out NumberStyles styles)
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

        internal static string ToAttributeValue(NumberStyles styles)
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

        internal static bool TryParseAttributeValue(string attributeValue, out NumberStyles styles)
        {
            string[] textArray;
            string errorDescription;
            styles = NumberStyles.None;
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
                        NumberStyles flags;
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
