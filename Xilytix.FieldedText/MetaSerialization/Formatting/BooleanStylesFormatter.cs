// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;

namespace Xilytix.FieldedText.MetaSerialization.Formatting
{
    internal static class BooleanStylesFormatter
    {
        private enum CompositeStyles { None }; // add to this if any Composites are included in future.  Do not include None

        private struct BasicRec
        {
            public FtBooleanStyles Flag;
            public string Text;
        }

        private static BasicRec[] basicRecArray =
        {
            new BasicRec {Flag = FtBooleanStyles.IgnoreCase, Text = "IgnoreCase" },
            new BasicRec {Flag = FtBooleanStyles.MatchFirstCharOnly, Text = "MatchFirstCharOnly" },
            new BasicRec {Flag = FtBooleanStyles.IgnoreTrailingChars, Text = "IgnoreTrailingChars" },
            new BasicRec {Flag = FtBooleanStyles.FalseIfNotMatchTrue, Text = "FalseIfNotMatchTrue" },
        };

        private struct CompositeRec
        {
            public CompositeStyles Composite;
            public FtBooleanStyles Flags;
            public string Text;
        }

        private static CompositeRec[] compositeRecArray =
        {
            new CompositeRec {Composite = CompositeStyles.None, Flags = 0, Text = "" },
        };

        internal static void StaticTest()
        {
            if (basicRecArray.Length != Enum.GetNames(typeof(FtBooleanStyles)).Length)
            {
                throw FtInternalException.Create(InternalError.BooleanStylesFormatter_StaticTest_IncorrectBasicRecCount, basicRecArray.Length.ToString());
            }

            if (compositeRecArray.Length != Enum.GetNames(typeof(CompositeStyles)).Length)
            {
                throw FtInternalException.Create(InternalError.BooleanStylesFormatter_StaticTest_IncorrectCompositeRecCount, compositeRecArray.Length.ToString());
            }
        }

        private static bool TryParseText(string text, out FtBooleanStyles flags)
        {
            flags = 0;
            bool result = false;

            foreach (BasicRec rec in basicRecArray)
            {
                if (string.Equals(rec.Text, text, StringComparison.OrdinalIgnoreCase))
                {
                    flags = rec.Flag;
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
                        flags = rec.Flags;
                        result = true;
                        break;
                    }
                }

                return result;
            }
        }

        internal static string ToAttributeValue(FtBooleanStyles styles)
        {
            string[] textArray = new string[basicRecArray.Length];
            int count = 0;

            // Composite must match exactly
            for (int i = 0; i < compositeRecArray.Length; i++)
            {
                if (styles == compositeRecArray[i].Flags)
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

        internal static bool TryParseAttributeValue(string attributeValue, out FtBooleanStyles styles)
        {
            string[] textArray;
            string errorDescription;
            styles = 0;
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
                        FtBooleanStyles flags;
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
