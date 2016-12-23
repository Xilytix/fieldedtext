// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;

namespace Xilytix.FieldedText.MetaSerialization.Formatting
{
    internal static class SequenceInvokationDelayFormatter
    {
        private struct FormatRec
        {
            public FtSequenceInvokationDelay Enumerator;
            public string AttributeValue;
        }

        private static FormatRec[] formatRecArray =
        {
            new FormatRec {Enumerator = FtSequenceInvokationDelay.ftikAfterField, AttributeValue = "AfterField" },
            new FormatRec {Enumerator = FtSequenceInvokationDelay.ftikAfterSequence, AttributeValue = "AfterSequence" },
        };

        internal static void StaticTest()
        {
            if (formatRecArray.Length != Enum.GetNames(typeof(FtSequenceInvokationDelay)).Length)
            {
                throw FtInternalException.Create(InternalError.SequenceInvokationDelayFormatter_StaticTest_IncorrectFormatRecCount, formatRecArray.Length.ToString());
            }
            for (int i = 0; i < formatRecArray.Length; i++)
            {
                if ((int)formatRecArray[i].Enumerator != i)
                {
                    throw FtInternalException.Create(InternalError.SequenceInvokationDelayFormatter_StaticTest_FormatRecOutOfOrder, formatRecArray[i].Enumerator.ToString());
                }
            }
        }

        internal static string ToAttributeValue(FtSequenceInvokationDelay enumerator) { return formatRecArray[(int)enumerator].AttributeValue; }
        internal static bool TryParseAttributeValue(string attributeValue, out FtSequenceInvokationDelay enumerator)
        {
            enumerator = FtSequenceInvokationDelay.ftikAfterField; // avoid compiler error
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