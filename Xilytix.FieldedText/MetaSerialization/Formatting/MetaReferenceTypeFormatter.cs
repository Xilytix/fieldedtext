// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;

namespace Xilytix.FieldedText.MetaSerialization.Formatting
{
    internal static class MetaReferenceTypeFormatter
    {
        private struct FormatRec
        {
            public FtMetaReferenceType Enumerator;
            public string AttributeValue;
        }

        private static FormatRec[] formatRecArray =
        {
            new FormatRec {Enumerator = FtMetaReferenceType.None, AttributeValue = "None" },
            new FormatRec {Enumerator = FtMetaReferenceType.Embedded, AttributeValue = "Embedded" },
            new FormatRec {Enumerator = FtMetaReferenceType.File, AttributeValue = "File" },
            new FormatRec {Enumerator = FtMetaReferenceType.Url, AttributeValue = "Url" },
        };

        internal static void StaticTest()
        {
            if (formatRecArray.Length != Enum.GetNames(typeof(FtMetaReferenceType)).Length)
            {
                throw FtInternalException.Create(InternalError.MetaReferenceTypeFormatter_StaticTest_IncorrectFormatRecCount, formatRecArray.Length.ToString());
            }
            for (int i = 0; i < formatRecArray.Length; i++)
            {
                if ((int)formatRecArray[i].Enumerator != i)
                {
                    throw FtInternalException.Create(InternalError.MetaReferenceTypeFormatter_StaticTest_FormatRecOutOfOrder, formatRecArray[i].Enumerator.ToString());
                }
            }
        }

        internal static string ToAttributeValue(FtMetaReferenceType enumerator) { return formatRecArray[(int)enumerator].AttributeValue; }
        internal static bool TryParseAttributeValue(string attributeValue, out FtMetaReferenceType enumerator)
        {
            enumerator = FtMetaReferenceType.Embedded; // avoid compiler error
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
