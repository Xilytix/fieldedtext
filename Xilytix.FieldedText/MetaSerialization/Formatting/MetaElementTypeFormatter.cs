// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;

namespace Xilytix.FieldedText.MetaSerialization.Formatting
{
    internal static class MetaElementTypeFormatter
    {
        public const string FieldedTextElementName = "FieldedText";

        private struct FormatRec
        {
            public MetaElementType Enumerator;
            public string ElementName;
        }

        private static FormatRec[] formatRecArray =
        {
            new FormatRec {Enumerator = MetaElementType.FieldedText, ElementName = FieldedTextElementName },
            new FormatRec {Enumerator = MetaElementType.Substitution, ElementName = "Substitution" },
            new FormatRec {Enumerator = MetaElementType.Field, ElementName = "Field" },
            new FormatRec {Enumerator = MetaElementType.Sequence, ElementName = "Sequence" },
            new FormatRec {Enumerator = MetaElementType.SequenceItem, ElementName = "Item" },
            new FormatRec {Enumerator = MetaElementType.SequenceRedirect, ElementName = "Redirect" },
        };

        internal static void StaticTest()
        {
            if (formatRecArray.Length != Enum.GetNames(typeof(MetaElementType)).Length)
            {
                throw FtInternalException.Create(InternalError.MetaElementTypeFormatter_StaticTest_IncorrectFormatRecCount, formatRecArray.Length.ToString());
            }
            for (int i = 0; i < formatRecArray.Length; i++)
            {
                if ((int)formatRecArray[i].Enumerator != i)
                {
                    throw FtInternalException.Create(InternalError.MetaElementTypeFormatter_StaticTest_FormatRecOutOfOrder, formatRecArray[i].Enumerator.ToString());
                }
            }
        }

        internal static string ToElementName(MetaElementType enumerator) { return formatRecArray[(int)enumerator].ElementName; }
        internal static bool TryParseElementName(string elementName, out MetaElementType enumerator)
        {
            enumerator = MetaElementType.FieldedText; // avoid compiler error
            bool result = false;
            foreach (FormatRec rec in formatRecArray)
            {
                if (String.Equals(rec.ElementName, elementName, StringComparison.OrdinalIgnoreCase))
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
