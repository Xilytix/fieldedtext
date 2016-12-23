// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

namespace Xilytix.FieldedText.MetaSerialization.Formatting
{
    using System;

    internal static class EndOfLineTypeFormatter
    {
        private struct FormatRec
        {
            public FtEndOfLineType Enumerator;
            public string AttributeValue;
        }

        private static FormatRec[] formatRecArray =
        {
            new FormatRec {Enumerator = FtEndOfLineType.Auto, AttributeValue = "Auto" },
            new FormatRec {Enumerator = FtEndOfLineType.Char, AttributeValue = "Char" },
            new FormatRec {Enumerator = FtEndOfLineType.CrLf, AttributeValue = "CrLf" },
        };

        internal static void StaticTest()
        {
            if (formatRecArray.Length != Enum.GetNames(typeof(FtEndOfLineType)).Length)
            {
                throw FtInternalException.Create(InternalError.EndOfLineTypeFormatter_StaticTest_IncorrectFormatRecCount, formatRecArray.Length.ToString());
            }
            for (int i = 0; i < formatRecArray.Length; i++)
            {
                if ((int)formatRecArray[i].Enumerator != i)
                {
                    throw FtInternalException.Create(InternalError.EndOfLineTypeFormatter_StaticTest_FormatRecOutOfOrder, formatRecArray[i].Enumerator.ToString());
                }
            }
        }

        internal static string ToAttributeValue(FtEndOfLineType enumerator) { return formatRecArray[(int)enumerator].AttributeValue; }
        internal static bool TryParseAttributeValue(string attributeValue, out FtEndOfLineType enumerator)
        {
            enumerator = FtEndOfLineType.Auto; // avoid compiler error
            bool result = false;
            foreach (FormatRec rec in formatRecArray)
            {
                if (String.Equals(rec.AttributeValue, attributeValue, StringComparison.OrdinalIgnoreCase))
                {
                    enumerator = rec.Enumerator;
                    result = true;
                    break;
                }
            }
            return result;
        }
    }
}
