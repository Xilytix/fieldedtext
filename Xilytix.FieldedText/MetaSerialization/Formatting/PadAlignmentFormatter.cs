// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;

namespace Xilytix.FieldedText.MetaSerialization.Formatting
{
    internal static class PadAlignmentFormatter
    {
        private struct FormatRec
        {
            public FtPadAlignment Enumerator;
            public string AttributeValue;
        }

        private static FormatRec[] formatRecArray =
        {
            new FormatRec {Enumerator = FtPadAlignment.Auto, AttributeValue = "Auto" },
            new FormatRec {Enumerator = FtPadAlignment.Left, AttributeValue = "Left" },
            new FormatRec {Enumerator = FtPadAlignment.Right, AttributeValue = "Right" },
        };

        internal static void StaticTest()
        {
            if (formatRecArray.Length != Enum.GetNames(typeof(FtPadAlignment)).Length)
            {
                throw FtInternalException.Create(InternalError.PadAlignmentFormatter_StaticTest_IncorrectFormatRecCount, formatRecArray.Length.ToString());
            }
            for (int i = 0; i < formatRecArray.Length; i++)
            {
                if ((int)formatRecArray[i].Enumerator != i)
                {
                    throw FtInternalException.Create(InternalError.PadAlignmentFormatter_StaticTest_FormatRecOutOfOrder, formatRecArray[i].Enumerator.ToString());
                }
            }
        }

        internal static string ToAttributeValue(FtPadAlignment enumerator) { return formatRecArray[(int)enumerator].AttributeValue; }
        internal static bool TryParseAttributeValue(string attributeValue, out FtPadAlignment enumerator)
        {
            enumerator = FtPadAlignment.Auto; // avoid compiler error
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
