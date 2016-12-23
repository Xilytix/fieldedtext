// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;

namespace Xilytix.FieldedText.MetaSerialization.Formatting
{
    internal static class SequenceRedirectPropertyIdFormatter
    {
        private struct FormatRec
        {
            public FtMetaSequenceRedirect.PropertyId Id;
            public string AttributeName;
        }

        private static FormatRec[] formatRecArray =
        {
            new FormatRec {Id = FtMetaSequenceRedirect.PropertyId.Index, AttributeName = "Index" },
            new FormatRec {Id = FtMetaSequenceRedirect.PropertyId.SequenceName, AttributeName = "SequenceName" },
            new FormatRec {Id = FtMetaSequenceRedirect.PropertyId.Type, AttributeName = "Type" },
            new FormatRec {Id = FtMetaSequenceRedirect.PropertyId.InvokationDelay, AttributeName = "InvokationDelay" },
            new FormatRec {Id = FtMetaSequenceRedirect.PropertyId.Value, AttributeName = "Value" },
        };

        internal static void StaticTest()
        {
            if (formatRecArray.Length != Enum.GetNames(typeof(FtMetaSequenceRedirect.PropertyId)).Length)
            {
                throw FtInternalException.Create(InternalError.SequenceRedirectPropertyIdFormatter_StaticTest_IncorrectFormatRecCount, formatRecArray.Length.ToString());
            }
            for (int i = 0; i < formatRecArray.Length; i++)
            {
                if ((int)formatRecArray[i].Id != i)
                {
                    throw FtInternalException.Create(InternalError.SequenceRedirectPropertyIdFormatter_StaticTest_FormatRecOutOfOrder, formatRecArray[i].Id.ToString());
                }
            }
        }

        internal static string ToAttributeName(FtMetaSequenceRedirect.PropertyId id) { return formatRecArray[(int)id].AttributeName; }
        internal static bool TryParseAttributeName(string attributeName, out FtMetaSequenceRedirect.PropertyId id)
        {
            id = FtMetaSequenceRedirect.PropertyId.SequenceName; // avoid compiler error
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
