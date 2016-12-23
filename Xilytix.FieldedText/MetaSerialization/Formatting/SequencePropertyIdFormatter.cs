// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;

namespace Xilytix.FieldedText.MetaSerialization.Formatting
{
    internal static class SequencePropertyIdFormatter
    {
        private struct FormatRec
        {
            public FtMetaSequence.PropertyId Id;
            public string AttributeName;
        }

        private static FormatRec[] formatRecArray =
        {
            new FormatRec {Id = FtMetaSequence.PropertyId.Name, AttributeName = "Name" },
            new FormatRec {Id = FtMetaSequence.PropertyId.Root, AttributeName = "Root" },
            new FormatRec {Id = FtMetaSequence.PropertyId.FieldIndices, AttributeName = "FieldIndices" },
        };

        internal static void StaticTest()
        {
            if (formatRecArray.Length != Enum.GetNames(typeof(FtMetaSequence.PropertyId)).Length)
            {
                throw FtInternalException.Create(InternalError.SequencePropertyIdFormatter_StaticTest_IncorrectFormatRecCount, formatRecArray.Length.ToString());
            }
            for (int i = 0; i < formatRecArray.Length; i++)
            {
                if ((int)formatRecArray[i].Id != i)
                {
                    throw FtInternalException.Create(InternalError.SequencePropertyIdFormatter_StaticTest_IncorrectFormatRecOutOfOrder, formatRecArray[i].Id.ToString());
                }
            }
        }

        internal static string ToAttributeName(FtMetaSequence.PropertyId id) { return formatRecArray[(int)id].AttributeName; }
        internal static bool TryParseAttributeName(string attributeName, out FtMetaSequence.PropertyId id)
        {
            id = FtMetaSequence.PropertyId.Name; // avoid compiler error
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
