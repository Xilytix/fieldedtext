// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;

namespace Xilytix.FieldedText.MetaSerialization.Formatting
{
    internal static class FieldPropertyIdFormatter
    {
        private struct FormatRec
        {
            public FtMetaField.PropertyId Id;
            public string AttributeName;
        }

        private static FormatRec[] formatRecArray =
        {
            new FormatRec {Id = FtMetaField.PropertyId.Index, AttributeName = "Index" },
            new FormatRec {Id = FtMetaField.PropertyId.Id, AttributeName = "Id" },
            new FormatRec {Id = FtMetaField.PropertyId.DataType, AttributeName = "DataType" },
            new FormatRec {Id = FtMetaField.PropertyId.Name, AttributeName = "Name" },
            new FormatRec {Id = FtMetaField.PropertyId.FixedWidth, AttributeName = "FixedWidth" },
            new FormatRec {Id = FtMetaField.PropertyId.Width, AttributeName = "Width" },
            new FormatRec {Id = FtMetaField.PropertyId.Constant, AttributeName = "Constant" },
            new FormatRec {Id = FtMetaField.PropertyId.ValueQuotedType, AttributeName = "ValueQuotedType" },
            new FormatRec {Id = FtMetaField.PropertyId.ValueAlwaysWriteOptionalQuote, AttributeName = "ValueAlwaysWriteOptionalQuote" },
            new FormatRec {Id = FtMetaField.PropertyId.ValueWritePrefixSpace, AttributeName = "ValueWritePrefixSpace" },
            new FormatRec {Id = FtMetaField.PropertyId.ValuePadAlignment, AttributeName = "ValuePadAlignment" },
            new FormatRec {Id = FtMetaField.PropertyId.ValuePadCharType, AttributeName = "ValuePadCharType" },
            new FormatRec {Id = FtMetaField.PropertyId.ValuePadChar, AttributeName = "ValuePadChar" },
            new FormatRec {Id = FtMetaField.PropertyId.ValueTruncateType, AttributeName = "ValueTruncateType" },
            new FormatRec {Id = FtMetaField.PropertyId.ValueTruncateChar, AttributeName = "ValueTruncateChar" },
            new FormatRec {Id = FtMetaField.PropertyId.ValueEndOfValueChar, AttributeName = "ValueEndOfValueChar" },
            new FormatRec {Id = FtMetaField.PropertyId.ValueNullChar, AttributeName = "ValueNullChar" },
            new FormatRec {Id = FtMetaField.PropertyId.HeadingConstraint, AttributeName = "HeadingConstraint" },
            new FormatRec {Id = FtMetaField.PropertyId.HeadingQuotedType, AttributeName = "HeadingQuotedType" },
            new FormatRec {Id = FtMetaField.PropertyId.HeadingAlwaysWriteOptionalQuote, AttributeName = "HeadingAlwaysWriteOptionalQuote" },
            new FormatRec {Id = FtMetaField.PropertyId.HeadingWritePrefixSpace, AttributeName = "HeadingWritePrefixSpace" },
            new FormatRec {Id = FtMetaField.PropertyId.HeadingPadAlignment, AttributeName = "HeadingPadAlignment" },
            new FormatRec {Id = FtMetaField.PropertyId.HeadingPadCharType, AttributeName = "HeadingPadCharType" },
            new FormatRec {Id = FtMetaField.PropertyId.HeadingPadChar, AttributeName = "HeadingPadChar" },
            new FormatRec {Id = FtMetaField.PropertyId.HeadingTruncateType, AttributeName = "HeadingTruncateType" },
            new FormatRec {Id = FtMetaField.PropertyId.HeadingTruncateChar, AttributeName = "HeadingTruncateChar" },
            new FormatRec {Id = FtMetaField.PropertyId.HeadingEndOfValueChar, AttributeName = "HeadingEndOfValueChar" },
            new FormatRec {Id = FtMetaField.PropertyId.Null, AttributeName = "Null" },
            new FormatRec {Id = FtMetaField.PropertyId.Headings, AttributeName = "Headings" },
            new FormatRec {Id = FtMetaField.PropertyId.Value, AttributeName = "Value" },
            new FormatRec {Id = FtMetaField.PropertyId.Styles, AttributeName = "Styles" },
            new FormatRec {Id = FtMetaField.PropertyId.Format, AttributeName = "Format" },
            new FormatRec {Id = FtMetaField.PropertyId.FalseText, AttributeName = "FalseText" },
            new FormatRec {Id = FtMetaField.PropertyId.TrueText, AttributeName = "TrueText" },
        };

        internal static void StaticTest()
        {
            if (formatRecArray.Length != Enum.GetNames(typeof(FtMetaField.PropertyId)).Length)
            {
                throw FtInternalException.Create(InternalError.FieldPropertyIdFormatter_StaticTest_IncorrectFormatRecCount, formatRecArray.Length.ToString());
            }
            for (int i = 0; i < formatRecArray.Length; i++)
            {
                if ((int)formatRecArray[i].Id != i)
                {
                    throw FtInternalException.Create(InternalError.FieldPropertyIdFormatter_StaticTest_FormatRecOutOfOrder, formatRecArray[i].Id.ToString());
                }
            }
        }

        internal static string ToAttributeName(FtMetaField.PropertyId id) { return formatRecArray[(int)id].AttributeName; }
        internal static bool TryParseAttributeName(string attributeName, out FtMetaField.PropertyId id)
        {
            id = FtMetaField.PropertyId.Id; // avoid compiler error
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
