// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;

namespace Xilytix.FieldedText.MetaSerialization.Formatting
{
    internal static class PadCharTypeFormatter
    {
        private struct FormatRec
        {
            public FtPadCharType Id;
            public string AttributeValue;
        }

        private static FormatRec[] formatRecArray =
        {
            new FormatRec {Id = FtPadCharType.Auto, AttributeValue = "Auto" },
            new FormatRec {Id = FtPadCharType.Specified, AttributeValue = "Specified" },
            new FormatRec {Id = FtPadCharType.EndOfValue, AttributeValue = "EndOfValue" },
        };

        internal static void StaticTest()
        {
            if (formatRecArray.Length != Enum.GetNames(typeof(FtPadCharType)).Length)
            {
                throw FtInternalException.Create(InternalError.PadCharTypeFormatter_StaticTest_IncorrectFormatRecCount, formatRecArray.Length.ToString());
            }
            for (int i = 0; i < formatRecArray.Length; i++)
            {
                if ((int)formatRecArray[i].Id != i)
                {
                    throw FtInternalException.Create(InternalError.PadCharTypeFormatter_StaticTest_FormatRecOutOfOrder, formatRecArray[i].Id.ToString());
                }
            }
        }

        internal static string ToAttributeValue(FtPadCharType enumerator) { return formatRecArray[(int)enumerator].AttributeValue; }
        internal static bool TryParseAttributeValue(string attributeValue, out FtPadCharType enumerator)
        {
            enumerator = FtPadCharType.Auto; // avoid compiler error
            bool result = false;
            foreach (FormatRec rec in formatRecArray)
            {
                if (String.Equals(rec.AttributeValue, attributeValue, StringComparison.OrdinalIgnoreCase))
                {
                    enumerator = rec.Id;
                    result = true;
                    break;
                }
            }
            return result;
        }
    }
}
