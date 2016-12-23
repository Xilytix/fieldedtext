// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

namespace Xilytix.FieldedText.UnitTest
{
    using System.Globalization;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    internal struct MetaProperties
    {
        internal const string DefaultCultureName = "";
        internal const FtEndOfLineType DefaultEndOfLineType = FtEndOfLineType.Auto;
        internal const char DefaultEndOfLineChar = ';';
        internal const FtEndOfLineAutoWriteType DefaultEndOfLineAutoWriteType = FtEndOfLineAutoWriteType.Local;
        internal const FtLastLineEndedType DefaultLastLineEndedType = FtLastLineEndedType.Optional;
        internal const char DefaultQuoteChar = '"';
        internal const char DefaultDelimiterChar = ',';
        internal const char DefaultLineCommentChar = '\x04';
        internal const bool DefaultAllowEndOfLineCharInQuotes = true;
        internal const bool DefaultIgnoreBlankLines = true;
        internal const bool DefaultIgnoreExtraChars = true;
        internal const bool DefaultAllowIncompleteRecords = false;
        internal const bool DefaultStuffedEmbeddedQuotes = true;
        internal const bool DefaultSubstitutionsEnabled = false;
        internal const char DefaultSubstitutionChar = '\\';
        internal const int DefaultHeadingLineCount = 0;
        internal const int DefaultMainHeadingLineIndex = 0;
        internal const FtHeadingConstraint DefaultHeadingConstraint = FtHeadingConstraint.None;
        internal const FtQuotedType DefaultHeadingQuotedType = FtQuotedType.Optional;
        internal const bool DefaultHeadingAlwaysWriteOptionalQuote = true;
        internal const bool DefaultHeadingWritePrefixSpace = false;
        internal const FtPadAlignment DefaultHeadingPadAlignment = FtPadAlignment.Auto;
        internal const FtPadCharType DefaultHeadingPadCharType = FtPadCharType.EndOfValue;
        internal const char DefaultHeadingPadChar = ' ';
        internal const FtTruncateType DefaultHeadingTruncateType = FtTruncateType.Right;
        internal const char DefaultHeadingTruncateChar = '#';
        internal const char DefaultHeadingEndOfValueChar = '\x03';

        public string CultureName { get; set; }
        public FtEndOfLineType EndOfLineType { get; set; }
        public char EndOfLineChar { get; set; }
        public FtEndOfLineAutoWriteType EndOfLineAutoWriteType { get; set; }
        public FtLastLineEndedType LastLineEndedType { get; set; }
        public char QuoteChar { get; set; }
        public char DelimiterChar { get; set; }
        public char LineCommentChar { get; set; }
        public bool AllowEndOfLineCharInQuotes { get; set; }
        public bool IgnoreBlankLines { get; set; }
        public bool IgnoreExtraChars { get; set; }
        public bool AllowIncompleteRecords { get; set; }
        public bool StuffedEmbeddedQuotes { get; set; }
        public bool SubstitutionsEnabled { get; set; }
        public char SubstitutionChar { get; set; }
        public int HeadingLineCount { get; set; }
        public int MainHeadingLineIndex { get; set; }
        public FtHeadingConstraint HeadingConstraint { get; set; }
        public FtQuotedType HeadingQuotedType { get; set; }
        public bool HeadingAlwaysWriteOptionalQuote { get; set; }
        public bool HeadingWritePrefixSpace { get; set; }
        public FtPadAlignment HeadingPadAlignment { get; set; }
        public FtPadCharType HeadingPadCharType { get; set; }
        public char HeadingPadChar { get; set; }
        public FtTruncateType HeadingTruncateType { get; set; }
        public char HeadingTruncateChar { get; set; }
        public char HeadingEndOfValueChar { get; set; }

        public void LoadDefaults()
        {
            CultureName = DefaultCultureName;
            EndOfLineType = DefaultEndOfLineType;
            EndOfLineChar = DefaultEndOfLineChar;
            EndOfLineAutoWriteType = DefaultEndOfLineAutoWriteType;
            LastLineEndedType = DefaultLastLineEndedType;
            QuoteChar = DefaultQuoteChar;
            DelimiterChar = DefaultDelimiterChar;
            LineCommentChar = DefaultLineCommentChar;
            AllowEndOfLineCharInQuotes = DefaultAllowEndOfLineCharInQuotes;
            IgnoreBlankLines = DefaultIgnoreBlankLines;
            IgnoreExtraChars = DefaultIgnoreExtraChars;
            AllowIncompleteRecords = DefaultAllowIncompleteRecords;
            StuffedEmbeddedQuotes = DefaultStuffedEmbeddedQuotes;
            SubstitutionsEnabled = DefaultSubstitutionsEnabled;
            SubstitutionChar = DefaultSubstitutionChar;
            HeadingLineCount = DefaultHeadingLineCount;
            MainHeadingLineIndex = DefaultMainHeadingLineIndex;
            HeadingConstraint = DefaultHeadingConstraint;
            HeadingQuotedType = DefaultHeadingQuotedType;
            HeadingAlwaysWriteOptionalQuote = DefaultHeadingAlwaysWriteOptionalQuote;
            HeadingWritePrefixSpace = DefaultHeadingWritePrefixSpace;
            HeadingPadAlignment = DefaultHeadingPadAlignment;
            HeadingPadCharType = DefaultHeadingPadCharType;
            HeadingPadChar = DefaultHeadingPadChar;
            HeadingTruncateType = DefaultHeadingTruncateType;
            HeadingTruncateChar = DefaultHeadingTruncateChar;
            HeadingEndOfValueChar = DefaultHeadingEndOfValueChar;
        }

        internal void LoadIntoMeta(ref FtMeta meta)
        {
            meta.Culture = CultureInfo.CreateSpecificCulture(CultureName);
            meta.EndOfLineType = EndOfLineType;
            meta.EndOfLineChar = EndOfLineChar;
            meta.EndOfLineAutoWriteType = EndOfLineAutoWriteType;
            meta.LastLineEndedType = LastLineEndedType;
            meta.QuoteChar = QuoteChar;
            meta.DelimiterChar = DelimiterChar;
            meta.LineCommentChar = LineCommentChar;
            meta.AllowEndOfLineCharInQuotes = AllowEndOfLineCharInQuotes;
            meta.IgnoreBlankLines = IgnoreBlankLines;
            meta.IgnoreExtraChars = IgnoreExtraChars;
            meta.AllowIncompleteRecords = AllowIncompleteRecords;
            meta.StuffedEmbeddedQuotes = StuffedEmbeddedQuotes;
            meta.SubstitutionsEnabled = SubstitutionsEnabled;
            meta.SubstitutionChar = SubstitutionChar;
            meta.HeadingLineCount = HeadingLineCount;
            meta.MainHeadingLineIndex = MainHeadingLineIndex;
            meta.HeadingConstraint = HeadingConstraint;
            meta.HeadingQuotedType = HeadingQuotedType;
            meta.HeadingAlwaysWriteOptionalQuote = HeadingAlwaysWriteOptionalQuote;
            meta.HeadingWritePrefixSpace = HeadingWritePrefixSpace;
            meta.HeadingPadAlignment = HeadingPadAlignment;
            meta.HeadingPadCharType = HeadingPadCharType;
            meta.HeadingPadChar = HeadingPadChar;
            meta.HeadingTruncateType = HeadingTruncateType;
            meta.HeadingTruncateChar = HeadingTruncateChar;
            meta.HeadingEndOfValueChar = HeadingEndOfValueChar;
        }

        internal void AssertMetaAreEqual(FtMeta meta)
        {
            Assert.AreEqual(meta.Culture.Name, CultureName, true);
            Assert.AreEqual<FtEndOfLineType>(meta.EndOfLineType, EndOfLineType);
            Assert.AreEqual<char>(meta.EndOfLineChar, EndOfLineChar);
            Assert.AreEqual<FtEndOfLineAutoWriteType>(meta.EndOfLineAutoWriteType, EndOfLineAutoWriteType);
            Assert.AreEqual<FtLastLineEndedType>(meta.LastLineEndedType, LastLineEndedType);
            Assert.AreEqual<char>(meta.QuoteChar, QuoteChar);
            Assert.AreEqual<char>(meta.DelimiterChar, DelimiterChar);
            Assert.AreEqual<char>(meta.LineCommentChar, LineCommentChar);
            Assert.AreEqual<bool>(meta.AllowEndOfLineCharInQuotes, AllowEndOfLineCharInQuotes);
            Assert.AreEqual<bool>(meta.IgnoreBlankLines, IgnoreBlankLines);
            Assert.AreEqual<bool>(meta.IgnoreExtraChars, IgnoreExtraChars);
            Assert.AreEqual<bool>(meta.AllowIncompleteRecords, AllowIncompleteRecords);
            Assert.AreEqual<bool>(meta.StuffedEmbeddedQuotes, StuffedEmbeddedQuotes);
            Assert.AreEqual<bool>(meta.SubstitutionsEnabled, SubstitutionsEnabled);
            Assert.AreEqual<char>(meta.SubstitutionChar, SubstitutionChar);
            Assert.AreEqual<int>(meta.HeadingLineCount, HeadingLineCount);
            Assert.AreEqual<int>(meta.MainHeadingLineIndex, MainHeadingLineIndex);
            Assert.AreEqual<FtHeadingConstraint>(meta.HeadingConstraint, HeadingConstraint);
            Assert.AreEqual<FtQuotedType>(meta.HeadingQuotedType, HeadingQuotedType);
            Assert.AreEqual<bool>(meta.HeadingAlwaysWriteOptionalQuote, HeadingAlwaysWriteOptionalQuote);
            Assert.AreEqual<bool>(meta.HeadingWritePrefixSpace, HeadingWritePrefixSpace);
            Assert.AreEqual<FtPadAlignment>(meta.HeadingPadAlignment, HeadingPadAlignment);
            Assert.AreEqual<FtPadCharType>(meta.HeadingPadCharType, HeadingPadCharType);
            Assert.AreEqual<char>(meta.HeadingPadChar, HeadingPadChar);
            Assert.AreEqual<FtTruncateType>(meta.HeadingTruncateType, HeadingTruncateType);
            Assert.AreEqual<char>(meta.HeadingTruncateChar, HeadingTruncateChar);
            Assert.AreEqual<char>(meta.HeadingEndOfValueChar, HeadingEndOfValueChar);
        }
    }
}
