// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;

namespace Xilytix.FieldedText.MetaSerialization.Formatting
{
    internal static class TruncateTypeFormatter
    {
        private struct FormatRec
        {
            public FtTruncateType Enumerator;
            public string AttributeValue;
        }

        private static FormatRec[] formatRecArray =
        {
            new FormatRec {Enumerator = FtTruncateType.Left, AttributeValue = "Left" },
            new FormatRec {Enumerator = FtTruncateType.Right, AttributeValue = "Right" },
            new FormatRec {Enumerator = FtTruncateType.TruncateChar, AttributeValue = "TruncateChar" },
            new FormatRec {Enumerator = FtTruncateType.NullChar, AttributeValue = "NullChar" },
            new FormatRec {Enumerator = FtTruncateType.Exception, AttributeValue = "Exception" },
        };

        internal static void StaticTest()
        {
            if (formatRecArray.Length != Enum.GetNames(typeof(FtTruncateType)).Length)
            {
                throw FtInternalException.Create(InternalError.TruncateTypeFormatter_StaticTest_IncorrectFormatRecCount, formatRecArray.Length.ToString());
            }
            for (int i = 0; i < formatRecArray.Length; i++)
            {
                if ((int)formatRecArray[i].Enumerator != i)
                {
                    throw FtInternalException.Create(InternalError.TruncateTypeFormatter_StaticTest_FormatRecOutOfOrder, formatRecArray[i].Enumerator.ToString());
                }
            }
        }

        internal static string ToAttributeValue(FtTruncateType enumerator) { return formatRecArray[(int)enumerator].AttributeValue; }
        internal static bool TryParseAttributeValue(string xmlValue, out FtTruncateType enumerator)
        {
            enumerator = FtTruncateType.Exception; // avoid compiler error
            bool result = false;
            foreach (FormatRec rec in formatRecArray)
            {
                if (String.Equals(rec.AttributeValue, xmlValue, StringComparison.OrdinalIgnoreCase))
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
