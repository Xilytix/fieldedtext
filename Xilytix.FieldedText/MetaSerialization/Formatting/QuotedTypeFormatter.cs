// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;

namespace Xilytix.FieldedText.MetaSerialization.Formatting
{
    internal static class QuotedTypeFormatter
    {
        private struct FormatRec
        {
            public FtQuotedType Enumerator;
            public string AttributeValue;
        }

        private static FormatRec[] formatRecArray =
        {
            new FormatRec {Enumerator = FtQuotedType.Never, AttributeValue = "Never" },
            new FormatRec {Enumerator = FtQuotedType.Always, AttributeValue = "Always" },
            new FormatRec {Enumerator = FtQuotedType.Optional, AttributeValue = "Optional" },
        };

        internal static void StaticTest()
        {
            if (formatRecArray.Length != Enum.GetNames(typeof(FtQuotedType)).Length)
            {
                throw FtInternalException.Create(InternalError.QuotedTypeFormatter_StaticTest_IncorrectFormatRecCount, formatRecArray.Length.ToString());
            }
            for (int i = 0; i < formatRecArray.Length; i++)
            {
                if ((int)formatRecArray[i].Enumerator != i)
                {
                    throw FtInternalException.Create(InternalError.QuotedTypeFormatter_StaticTest_FormatRecOutOfOrder, formatRecArray[i].Enumerator.ToString());
                }
            }
        }

        internal static string ToAttributeValue(FtQuotedType enumerator) { return formatRecArray[(int)enumerator].AttributeValue; }
        internal static bool TryParseAttributeValue(string attributeValue, out FtQuotedType enumerator)
        {
            enumerator = FtQuotedType.Always; // avoid compiler error
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
