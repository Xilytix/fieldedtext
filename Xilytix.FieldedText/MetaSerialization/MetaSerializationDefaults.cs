// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System.Globalization;

namespace Xilytix.FieldedText.MetaSerialization
{
    internal static class MetaSerializationDefaults
    {
        internal static class Root
        {
            public const string CultureName = "";
            public const FtEndOfLineType EndOfLineType = FtEndOfLineType.Auto;
            public const char EndOfLineChar = ';';
            public const FtEndOfLineAutoWriteType EndOfLineAutoWriteType = FtEndOfLineAutoWriteType.Local;
            public const FtLastLineEndedType LastLineEndedType = FtLastLineEndedType.Optional;
            public const char QuoteChar = '"';
            public const char DelimiterChar = ',';
            public const char LineCommentChar = '\x04';
            public const bool AllowEndOfLineCharInQuotes = true;
            public const bool IgnoreBlankLines = true;
            public const bool IgnoreExtraChars = true;
            public const bool AllowIncompleteRecords = false;
            public const bool StuffedEmbeddedQuotes = true;
            public const bool SubstitutionsEnabled = false;
            public const char SubstitutionChar = '\\';
            public const int HeadingLineCount = 0;
            public const int MainHeadingLineIndex = 0;
            public const FtHeadingConstraint HeadingConstraint = FtHeadingConstraint.None;
            public const FtQuotedType HeadingQuotedType = FtQuotedType.Optional;
            public const bool HeadingAlwaysWriteOptionalQuote = true;
            public const bool HeadingWritePrefixSpace = false;
            public const FtPadAlignment HeadingPadAlignment = FtPadAlignment.Auto;
            public const FtPadCharType HeadingPadCharType = FtPadCharType.EndOfValue;
            public const char HeadingPadChar = ' ';
            public const FtTruncateType HeadingTruncateType = FtTruncateType.Right;
            public const char HeadingTruncateChar = '#';
            public const char HeadingEndOfValueChar = '\x03';
        }
        internal static class Field
        {
            public const int Id = 0;
            public const string Name = "";
            public const int DataType = FtStandardDataType.String;
            public const bool FixedWidth = false;
            public const int Width = 1;
            public const bool Constant = false;
            public const bool Null = false;
            public const FtQuotedType ValueQuotedType = FtQuotedType.Optional;
            public const bool ValueAlwaysWriteOptionalQuote = false;
            public const bool ValueWritePrefixSpace = false;
            public const FtPadAlignment ValuePadAlignment = FtPadAlignment.Auto;
            public const FtPadCharType ValuePadCharType = FtPadCharType.EndOfValue;
            public const char ValuePadChar = ' ';
            public const FtTruncateType ValueTruncateType = FtTruncateType.Exception;
            public const char ValueTruncateChar = '#';
            public const char ValueEndOfValueChar = '\x03'; // ASCII Control Code: End Of Text
            public const char ValueNullChar = '*';
        }
        internal static class StringField
        {
            public const int SequenceRedirectType = FtStandardSequenceRedirectType.ExactString;
        }
        internal static class BooleanField
        {
            public const string FalseText = FtStandardText.BooleanFalseText;
            public const string TrueText = FtStandardText.BooleanTrueText;
            public const FtBooleanStyles Styles = FtBooleanStyles.IgnoreCase | FtBooleanStyles.MatchFirstCharOnly;
            public const int SequenceRedirectType = FtStandardSequenceRedirectType.Boolean;
        }
        internal static class IntegerField
        {
            public const string Format = "G";
            public const NumberStyles Styles = NumberStyles.Integer;
            public const int SequenceRedirectType = FtStandardSequenceRedirectType.ExactInteger;
        }
        internal static class FloatField
        {
            public const string Format = "G";
            public const NumberStyles Styles = NumberStyles.Float;
            public const int SequenceRedirectType = FtStandardSequenceRedirectType.ExactFloat;
        }
        internal static class DecimalField
        {
            public const string Format = "G";
            public const NumberStyles Styles = NumberStyles.Currency;
            public const int SequenceRedirectType = FtStandardSequenceRedirectType.ExactDecimal;
        }
        internal static class DateTimeField
        {
            public const string Format = "yyyyMMdd";
            public const DateTimeStyles Styles = DateTimeStyles.None;
            public const int SequenceRedirectType = FtStandardSequenceRedirectType.ExactDateTime;
        }
        internal static class Substitution
        {
            public const FtSubstitutionType Type = FtSubstitutionType.String;
        }
        internal static class Sequence
        {
            public const bool Root = false;
        }
        internal static class SequenceRedirect
        {
            public const FtSequenceInvokationDelay InvokationDelay = FtSequenceInvokationDelay.ftikAfterField;
        }
    }
}
