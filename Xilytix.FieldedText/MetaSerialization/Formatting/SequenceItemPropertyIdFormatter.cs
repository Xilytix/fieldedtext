// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;

namespace Xilytix.FieldedText.MetaSerialization.Formatting
{
    internal static class SequenceItemPropertyIdFormatter
    {
        private struct FormatRec
        {
            public FtMetaSequenceItem.PropertyId Id;
            public string AttributeName;
        }

        private static FormatRec[] formatRecArray =
        {
            new FormatRec {Id = FtMetaSequenceItem.PropertyId.Index, AttributeName = "Index" },
            new FormatRec {Id = FtMetaSequenceItem.PropertyId.FieldIndex, AttributeName = "FieldIndex" },
        };

        internal static void StaticTest()
        {
            if (formatRecArray.Length != Enum.GetNames(typeof(FtMetaSequenceItem.PropertyId)).Length)
            {
                throw FtInternalException.Create(InternalError.SequenceItemPropertyIdFormatter_StaticTest_IncorrectFormatRecCount, formatRecArray.Length.ToString());
            }
            for (int i = 0; i < formatRecArray.Length; i++)
            {
                if ((int)formatRecArray[i].Id != i)
                {
                    throw FtInternalException.Create(InternalError.SequenceItemPropertyIdFormatter_StaticTest_FormatRecOutOfOrder, formatRecArray[i].Id.ToString());
                }
            }
        }

        internal static string ToAttributeName(FtMetaSequenceItem.PropertyId id) { return formatRecArray[(int)id].AttributeName; }
        internal static bool TryParseAttributeName(string attributeName, out FtMetaSequenceItem.PropertyId id)
        {
            id = FtMetaSequenceItem.PropertyId.FieldIndex; // avoid compiler error
            bool result = false;
            foreach (FormatRec rec in formatRecArray)
            {
                if (String.Equals(rec.AttributeName, attributeName, StringComparison.OrdinalIgnoreCase))
                {
                    id = rec.Id;
                    result = true;
                    break;
                }
            }
            return result;
        }
    }
}
