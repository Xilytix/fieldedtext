// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;

namespace Xilytix.FieldedText.MetaSerialization.Formatting
{
    internal static class RootPropertyIdFormatter
    {
        private struct FormatRec
        {
            public FtMeta.PropertyId Id;
            public string AttributeName;
        }

        private static FormatRec[] formatRecArray =
        {
            new FormatRec {Id = FtMeta.PropertyId.Culture, AttributeName = "Culture" },
            new FormatRec {Id = FtMeta.PropertyId.EndOfLineType, AttributeName = "EndOfLineType" },
            new FormatRec {Id = FtMeta.PropertyId.EndOfLineChar, AttributeName = "EndOfLineChar" },
            new FormatRec {Id = FtMeta.PropertyId.EndOfLineAutoWriteType, AttributeName = "EndOfLineAutoWriteType" },
            new FormatRec {Id = FtMeta.PropertyId.LastLineEndedType, AttributeName = "LastLineEndedType" },
            new FormatRec {Id = FtMeta.PropertyId.QuoteChar, AttributeName = "QuoteChar" },
            new FormatRec {Id = FtMeta.PropertyId.DelimiterChar, AttributeName = "DelimiterChar" },
            new FormatRec {Id = FtMeta.PropertyId.LineCommentChar, AttributeName = "LineCommentChar" },
            new FormatRec {Id = FtMeta.PropertyId.AllowEndOfLineCharInQuotes, AttributeName = "AllowEndOfLineCharInQuotes" },
            new FormatRec {Id = FtMeta.PropertyId.IgnoreBlankLines, AttributeName = "IgnoreBlankLines" },
            new FormatRec {Id = FtMeta.PropertyId.IgnoreExtraChars, AttributeName = "IgnoreExtraChars" },
            new FormatRec {Id = FtMeta.PropertyId.AllowIncompleteRecords, AttributeName = "AllowIncompleteRecords" },
            new FormatRec {Id = FtMeta.PropertyId.StuffedEmbeddedQuotes, AttributeName = "StuffedEmbeddedQuotes" },
            new FormatRec {Id = FtMeta.PropertyId.SubstitutionsEnabled, AttributeName = "SubstitutionsEnabled" },
            new FormatRec {Id = FtMeta.PropertyId.SubstitutionChar, AttributeName = "SubstitutionChar" },
            new FormatRec {Id = FtMeta.PropertyId.HeadingLineCount, AttributeName = "HeadingLineCount" },
            new FormatRec {Id = FtMeta.PropertyId.MainHeadingLineIndex, AttributeName = "MainHeadingLineIndex" },
            new FormatRec {Id = FtMeta.PropertyId.HeadingConstraint, AttributeName = "HeadingConstraint" },
            new FormatRec {Id = FtMeta.PropertyId.HeadingQuotedType, AttributeName = "HeadingQuotedType" },
            new FormatRec {Id = FtMeta.PropertyId.HeadingAlwaysWriteOptionalQuote, AttributeName = "HeadingAlwaysWriteOptionalQuote" },
            new FormatRec {Id = FtMeta.PropertyId.HeadingWritePrefixSpace, AttributeName = "HeadingWritePrefixSpace" },
            new FormatRec {Id = FtMeta.PropertyId.HeadingPadAlignment, AttributeName = "HeadingPadAlignment" },
            new FormatRec {Id = FtMeta.PropertyId.HeadingPadCharType, AttributeName = "HeadingPadCharType" },
            new FormatRec {Id = FtMeta.PropertyId.HeadingPadChar, AttributeName = "HeadingPadChar" },
            new FormatRec {Id = FtMeta.PropertyId.HeadingTruncateType, AttributeName = "HeadingTruncateType" },
            new FormatRec {Id = FtMeta.PropertyId.HeadingTruncateChar, AttributeName = "HeadingTruncateChar" },
            new FormatRec {Id = FtMeta.PropertyId.HeadingEndOfValueChar, AttributeName = "HeadingEndOfValueChar" },
            new FormatRec {Id = FtMeta.PropertyId.LegacyEndOfLineIsSeparator, AttributeName = Legacy.EndOfLineIsSeparator },
            new FormatRec {Id = FtMeta.PropertyId.LegacyIncompleteRecordsAllowed, AttributeName = Legacy.IncompleteRecordsAllowed },
        };

        internal static void StaticTest()
        {
            if (formatRecArray.Length != Enum.GetNames(typeof(FtMeta.PropertyId)).Length)
            {
                throw FtInternalException.Create(InternalError.RootPropertyIdFormatter_StaticTest_IncorrectFormatRecCount, formatRecArray.Length.ToString());
            }
            for (int i = 0; i < formatRecArray.Length; i++)
            {
                if ((int)formatRecArray[i].Id != i)
                {
                    throw FtInternalException.Create(InternalError.RootPropertyIdFormatter_StaticTest_FormatRecOutOfOrder, formatRecArray[i].Id.ToString());
                }
            }
        }

        internal static string ToAttributeName(FtMeta.PropertyId id) { return formatRecArray[(int)id].AttributeName; }
        internal static bool TryParseAttributeName(string attributeName, out FtMeta.PropertyId id)
        {
            id = FtMeta.PropertyId.Culture; // avoid compiler error
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
