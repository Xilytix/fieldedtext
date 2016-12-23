// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;

namespace Xilytix.FieldedText.MetaSerialization.Formatting
{
    internal static class EndOfLineAutoWriteTypeFormatter
    {
        private struct FormatRec
        {
            public FtEndOfLineAutoWriteType Enumerator;
            public string AttributeValue;
        }

        private static FormatRec[] formatRecArray =
        {
            new FormatRec {Enumerator = FtEndOfLineAutoWriteType.CrLf, AttributeValue = "CrLf" },
            new FormatRec {Enumerator = FtEndOfLineAutoWriteType.Cr, AttributeValue = "Cr" },
            new FormatRec {Enumerator = FtEndOfLineAutoWriteType.Lf, AttributeValue = "Lf" },
            new FormatRec {Enumerator = FtEndOfLineAutoWriteType.Local, AttributeValue = "Local" },
        };

        internal static void StaticTest()
        {
            if (formatRecArray.Length != Enum.GetNames(typeof(FtEndOfLineAutoWriteType)).Length)
            {
                throw FtInternalException.Create(InternalError.EndOfLineAutoWriteTypeFormatter_StaticTest_IncorrectFormatRecCount, formatRecArray.Length.ToString());
            }
            for (int i = 0; i < formatRecArray.Length; i++)
            {
                if ((int)formatRecArray[i].Enumerator != i)
                {
                    throw FtInternalException.Create(InternalError.EndOfLineAutoWriteTypeFormatter_StaticTest_FormatRecOutOfOrder, formatRecArray[i].Enumerator.ToString());
                }
            }
        }

        internal static string ToAttributeValue(FtEndOfLineAutoWriteType enumerator) { return formatRecArray[(int)enumerator].AttributeValue; }
        internal static bool TryParseAttributeValue(string attributeValue, out FtEndOfLineAutoWriteType enumerator)
        {
            enumerator = FtEndOfLineAutoWriteType.Cr; // avoid compiler error
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
