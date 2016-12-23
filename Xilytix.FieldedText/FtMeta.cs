// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

namespace Xilytix.FieldedText
{
    using System;
    using System.Globalization;

    using Factory;
    using MetaSerialization;

    public class FtMeta
    {
        public enum PropertyId
        {
            Culture,
            EndOfLineType,
            EndOfLineChar,
            EndOfLineAutoWriteType,
            LastLineEndedType,
            QuoteChar,
            DelimiterChar,
            LineCommentChar,
            AllowEndOfLineCharInQuotes,
            IgnoreBlankLines,
            IgnoreExtraChars,
            AllowIncompleteRecords,
            StuffedEmbeddedQuotes,
            SubstitutionsEnabled,
            SubstitutionChar,
            HeadingLineCount,
            MainHeadingLineIndex,
            HeadingConstraint,
            HeadingQuotedType,
            HeadingAlwaysWriteOptionalQuote,
            HeadingWritePrefixSpace,
            HeadingPadAlignment,
            HeadingPadCharType,
            HeadingPadChar,
            HeadingTruncateType,
            HeadingTruncateChar,
            HeadingEndOfValueChar,
            LegacyEndOfLineIsSeparator,
            LegacyIncompleteRecordsAllowed,
        }

        public const string DefaultCultureName = MetaSerializationDefaults.Root.CultureName;
        public const FtEndOfLineType DefaultEndOfLineType = MetaSerializationDefaults.Root.EndOfLineType;
        public const char DefaultEndOfLineChar = MetaSerializationDefaults.Root.EndOfLineChar;
        public const FtEndOfLineAutoWriteType DefaultEndOfLineAutoWriteType = MetaSerializationDefaults.Root.EndOfLineAutoWriteType;
        public const FtLastLineEndedType DefaultLastLineEndedType = MetaSerializationDefaults.Root.LastLineEndedType;
        public const char DefaultQuoteChar = MetaSerializationDefaults.Root.QuoteChar;
        public const char DefaultDelimiterChar = MetaSerializationDefaults.Root.DelimiterChar;
        public const char DefaultLineCommentChar = MetaSerializationDefaults.Root.LineCommentChar;
        public const bool DefaultAllowEndOfLineCharInQuotes = MetaSerializationDefaults.Root.AllowEndOfLineCharInQuotes;
        public const bool DefaultIgnoreBlankLines = MetaSerializationDefaults.Root.IgnoreBlankLines;
        public const bool DefaultIgnoreExtraChars = MetaSerializationDefaults.Root.IgnoreExtraChars;
        public const bool DefaultAllowIncompleteRecords = MetaSerializationDefaults.Root.AllowIncompleteRecords;
        public const bool DefaultStuffedEmbeddedQuotes = MetaSerializationDefaults.Root.StuffedEmbeddedQuotes;
        public const bool DefaultSubstitutionsEnabled = MetaSerializationDefaults.Root.SubstitutionsEnabled;
        public const char DefaultSubstitutionChar = MetaSerializationDefaults.Root.SubstitutionChar;
        public const int DefaultHeadingLineCount = MetaSerializationDefaults.Root.HeadingLineCount;
        public const int DefaultMainHeadingLineIndex = MetaSerializationDefaults.Root.MainHeadingLineIndex;
        public const FtQuotedType DefaultHeadingQuotedType = MetaSerializationDefaults.Root.HeadingQuotedType;
        public const bool DefaultHeadingAlwaysWriteOptionalQuote = MetaSerializationDefaults.Root.HeadingAlwaysWriteOptionalQuote;
        public const bool DefaultHeadingWritePrefixSpace = MetaSerializationDefaults.Root.HeadingWritePrefixSpace;
        public const FtHeadingConstraint DefaultHeadingConstraint = MetaSerializationDefaults.Root.HeadingConstraint;
        public const FtPadAlignment DefaultHeadingPadAlignment = MetaSerializationDefaults.Root.HeadingPadAlignment;
        public const FtPadCharType DefaultHeadingPadCharType = MetaSerializationDefaults.Root.HeadingPadCharType;
        public const char DefaultHeadingPadChar = MetaSerializationDefaults.Root.HeadingPadChar;
        public const FtTruncateType DefaultHeadingTruncateType = MetaSerializationDefaults.Root.HeadingTruncateType;
        public const char DefaultHeadingTruncateChar = MetaSerializationDefaults.Root.HeadingTruncateChar;
        public const char DefaultHeadingEndOfValueChar = MetaSerializationDefaults.Root.HeadingEndOfValueChar;

        private FtMetaFieldList fieldList;
        private FtMetaSequenceList sequenceList;
        private FtMetaSubstitutionList substitutionList;
        private int headingLineCount;

        private FtHeadingConstraint HandleFieldDefaultHeadingConstraintRequiredEvent() { return HeadingConstraint; }
        private FtQuotedType HandleFieldDefaultHeadingQuotedTypeRequiredEvent() { return HeadingQuotedType; }
        private bool HandleFieldDefaultHeadingAlwaysWriteOptionalQuoteRequiredEvent() { return HeadingAlwaysWriteOptionalQuote; }
        private bool HandleFieldDefaultHeadingWritePrefixSpaceRequiredEvent() { return HeadingWritePrefixSpace; }
        private FtPadAlignment HandleFieldDefaultHeadingPadAlignmentRequiredEvent() { return HeadingPadAlignment; }
        private FtPadCharType HandleFieldDefaultHeadingPadCharTypeRequiredEvent() { return HeadingPadCharType; }
        private char HandleFieldDefaultHeadingPadCharRequiredEvent() { return HeadingPadChar; }
        private FtTruncateType HandleFieldDefaultHeadingTruncateTypeRequiredEvent() { return HeadingTruncateType; }
        private char HandleFieldDefaultHeadingTruncateCharRequiredEvent() { return HeadingTruncateChar; }
        private char HandleFieldDefaultHeadingEndOfValueCharRequiredEvent() { return HeadingEndOfValueChar; }

        public FtMeta()
        {
            fieldList = new FtMetaFieldList();
            fieldList.BeforeRemoveEvent += HandleFieldListBeforeRemoveEvent;
            fieldList.BeforeClearEvent += HandleFieldListBeforeClearEvent;
            fieldList.DefaultHeadingConstraintRequiredEvent += HandleFieldDefaultHeadingConstraintRequiredEvent;
            fieldList.DefaultHeadingQuotedTypeRequiredEvent += HandleFieldDefaultHeadingQuotedTypeRequiredEvent;
            fieldList.DefaultHeadingAlwaysWriteOptionalQuoteRequiredEvent += HandleFieldDefaultHeadingAlwaysWriteOptionalQuoteRequiredEvent;
            fieldList.DefaultHeadingWritePrefixSpaceRequiredEvent += HandleFieldDefaultHeadingWritePrefixSpaceRequiredEvent;
            fieldList.DefaultHeadingPadAlignmentRequiredEvent += HandleFieldDefaultHeadingPadAlignmentRequiredEvent;
            fieldList.DefaultHeadingPadCharTypeRequiredEvent += HandleFieldDefaultHeadingPadCharTypeRequiredEvent;
            fieldList.DefaultHeadingPadCharRequiredEvent += HandleFieldDefaultHeadingPadCharRequiredEvent;
            fieldList.DefaultHeadingTruncateTypeRequiredEvent += HandleFieldDefaultHeadingTruncateTypeRequiredEvent;
            fieldList.DefaultHeadingTruncateCharRequiredEvent += HandleFieldDefaultHeadingTruncateCharRequiredEvent;
            fieldList.DefaultHeadingEndOfValueCharRequiredEvent += HandleFieldDefaultHeadingEndOfValueCharRequiredEvent;
            substitutionList = new FtMetaSubstitutionList();
            sequenceList = new FtMetaSequenceList();

            Reset();
        }

        public static FtMeta Create()
        {
            return MetaFactory.CreateMeta(); 
        }

        public static FtMeta Create(string metaFileName)
        {
            return FtMetaSerializer.Deserialize(metaFileName);
        }

        public FtMetaFieldList FieldList { get { return fieldList; } }
        public FtMetaSequenceList SequenceList { get { return sequenceList; } }
        public FtMetaSubstitutionList SubstitutionList { get { return substitutionList; } }

        public CultureInfo Culture { get; set; }

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

        public int HeadingLineCount
        {
            get { return headingLineCount; }
            set
            {
                headingLineCount = value;
                fieldList.HeadingCount = headingLineCount;
            }
        }

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

        private void HandleFieldListBeforeRemoveEvent(int fieldIdx)
        {
            sequenceList.RemoveField(fieldList[fieldIdx]);
        }
        private void HandleFieldListBeforeClearEvent()
        {
            sequenceList.RemoveAllFields();
        }

        public void Reset()
        {
            sequenceList.Clear();
            fieldList.Clear();
            substitutionList.Clear();

            Culture = new CultureInfo(DefaultCultureName, false);

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

            MainHeadingLineIndex = DefaultMainHeadingLineIndex;
            HeadingLineCount = DefaultHeadingLineCount;

            HeadingQuotedType = DefaultHeadingQuotedType;
            HeadingAlwaysWriteOptionalQuote = DefaultHeadingAlwaysWriteOptionalQuote;
            HeadingWritePrefixSpace = DefaultHeadingWritePrefixSpace;

            HeadingConstraint = DefaultHeadingConstraint;
            HeadingPadAlignment = DefaultHeadingPadAlignment;
            HeadingPadCharType = DefaultHeadingPadCharType;
            HeadingPadChar = DefaultHeadingPadChar;
            HeadingTruncateType = DefaultHeadingTruncateType;
            HeadingTruncateChar = DefaultHeadingTruncateChar;
            HeadingEndOfValueChar = DefaultHeadingEndOfValueChar;
        }

        public FtMeta CreateCopy()
        {
            FtMeta meta = MetaFactory.CreateMeta();
            meta.Assign(this);
            return meta;
        }

        public virtual void Assign(FtMeta source)
        {
            Culture = source.Culture.Clone() as CultureInfo;

            EndOfLineType = source.EndOfLineType;
            EndOfLineChar = source.EndOfLineChar;
            EndOfLineAutoWriteType = source.EndOfLineAutoWriteType;
            LastLineEndedType = source.LastLineEndedType;

            QuoteChar = source.QuoteChar;
            DelimiterChar = source.DelimiterChar;
            LineCommentChar = source.LineCommentChar;

            AllowEndOfLineCharInQuotes = source.AllowEndOfLineCharInQuotes;
            IgnoreBlankLines = source.IgnoreBlankLines;
            IgnoreExtraChars = source.IgnoreExtraChars;
            AllowIncompleteRecords = source.AllowIncompleteRecords;

            StuffedEmbeddedQuotes = source.StuffedEmbeddedQuotes;

            SubstitutionsEnabled = source.SubstitutionsEnabled;
            SubstitutionChar = source.SubstitutionChar;

            MainHeadingLineIndex = source.MainHeadingLineIndex;
            HeadingLineCount = source.HeadingLineCount;

            HeadingQuotedType = source.HeadingQuotedType;
            HeadingAlwaysWriteOptionalQuote = source.HeadingAlwaysWriteOptionalQuote;
            HeadingWritePrefixSpace = source.HeadingWritePrefixSpace;

            HeadingConstraint = source.HeadingConstraint;
            HeadingPadAlignment = source.HeadingPadAlignment;
            HeadingPadCharType = source.HeadingPadCharType;
            HeadingPadChar = source.HeadingPadChar;
            HeadingTruncateType = source.HeadingTruncateType;
            HeadingTruncateChar = source.HeadingTruncateChar;
            HeadingEndOfValueChar = source.HeadingEndOfValueChar;

            fieldList.Assign(source.fieldList);
            sequenceList.Assign(source.sequenceList, fieldList, source.FieldList);
            substitutionList.Assign(source.substitutionList);
        }

        private enum ValidateTest
        {
            FieldList,
            QuoteCharSameAsDelimiterChar,
            QuoteCharSameAsLineCommentChar,
            LineCommentCharSameAsDelimiterChar,
            SubstitutionChar,
            EndOfLineType,
            SequenceRoot,
            DuplicateSequenceName,
            SequenceItemHasNullField,
            SequenceItemWithConstantFieldHasRedirects,
        }

        public bool Validate(out string errorMessage)
        {
            // check various - including multiple sequence roots and duplicate sequence names
            errorMessage = "";

            foreach (ValidateTest test in Enum.GetValues(typeof(ValidateTest)))
            {
                switch (test)
                {
                    case ValidateTest.FieldList:
                        if (FieldList.Count == 0)
                        {
                            errorMessage = Properties.Resources.FtMeta_Validate_MustHaveAtLeastOneField;
                        }
                        break;

                    case ValidateTest.QuoteCharSameAsDelimiterChar:
                        if (QuoteChar == DelimiterChar)
                        {
                            errorMessage = Properties.Resources.FtMeta_Validate_QuoteCharAndDelimiterCharMustBeDifferent;
                        }
                        break;

                    case ValidateTest.QuoteCharSameAsLineCommentChar:
                        if (QuoteChar == LineCommentChar)
                        {
                            errorMessage = Properties.Resources.FtMeta_Validate_QuoteCharAndLineCommentCharMustBeDifferent;
                        }
                        break;

                    case ValidateTest.LineCommentCharSameAsDelimiterChar:
                        if (LineCommentChar == DelimiterChar)
                        {
                            errorMessage = Properties.Resources.FtMeta_Validate_LineCommentCharAndDelimiterCharMustBeDifferent;
                        }
                        break;

                    case ValidateTest.SubstitutionChar:
                        if (SubstitutionsEnabled)
                        {
                            if (QuoteChar == SubstitutionChar)
                                errorMessage = Properties.Resources.FtMeta_Validate_QuoteCharAndSubstitutionCharMustBeDifferent;
                            else
                            {
                                if (DelimiterChar == SubstitutionChar)
                                    errorMessage = Properties.Resources.FtMeta_Validate_DelimiterCharAndSubstitutionCharMustBeDifferent;
                                else
                                {
                                    if (LineCommentChar == SubstitutionChar)
                                        errorMessage = Properties.Resources.FtMeta_Validate_LineCommentCharAndSubstitutionCharMustBeDifferent;
                                    else
                                    {

                                    }
                                }
                            }
                        }
                        break;

                    case ValidateTest.EndOfLineType:
                        switch (EndOfLineType)
                        {
                            case FtEndOfLineType.CrLf:
                            case FtEndOfLineType.Auto:
                                if (ValidateEndOfLineTypeChar(Serialization.SerializationCore.CarriageReturnChar, out errorMessage))
                                {
                                    ValidateEndOfLineTypeChar(Serialization.SerializationCore.LineFeedChar, out errorMessage);
                                }
                                break;
                            case FtEndOfLineType.Char:
                                ValidateEndOfLineTypeChar(EndOfLineChar, out errorMessage);
                                break;
                            default: throw FtInternalException.Create(InternalError.FtMeta_Validate_UnsupportedEndOfLineType);
                        }
                        break;

                    case ValidateTest.SequenceRoot:
                        string firstRootSequenceName;
                        string secondRootSequenceName;
                        if (SequenceList.IsMoreThanOneRoot(out firstRootSequenceName, out secondRootSequenceName))
                        {
                            errorMessage = string.Format(Properties.Resources.FtMeta_Validate_MoreThanOneRootSequence, firstRootSequenceName, secondRootSequenceName);
                        }
                        break;

                    case ValidateTest.DuplicateSequenceName:
                        string duplicateSequenceName;
                        if (SequenceList.HasDuplicateName(out duplicateSequenceName))
                        {
                            errorMessage = string.Format(Properties.Resources.FtMeta_Validate_DuplicateSequenceName, duplicateSequenceName);
                        }
                        break;

                    case ValidateTest.SequenceItemHasNullField:
                        for (int i = 0; i < SequenceList.Count; i++)
                        {
                            int itemIdx;
                            if (SequenceList[i].AnyItemWithNullField(out itemIdx))
                            {
                                errorMessage = string.Format(Properties.Resources.FtMeta_Validate_SequenceItemWithNullField, SequenceList[i].Name, itemIdx);
                            }
                        }
                        break;

                    case ValidateTest.SequenceItemWithConstantFieldHasRedirects:
                        for (int i = 0; i < SequenceList.Count; i++)
                        {
                            int itemIdx;
                            if (SequenceList[i].AnyItemWithConstantFieldHasRedirects(out itemIdx))
                            {
                                errorMessage = string.Format(Properties.Resources.FtMeta_Validate_SequenceItemWithConstantFieldHasRedirects, SequenceList[i].Name, itemIdx);
                            }
                        }
                        break;

                    default: throw FtInternalException.Create(InternalError.FtMeta_Validate_UnsupportedValidateTest);
                }

                if (errorMessage.Length != 0)
                {
                    break;
                }
            }

            return errorMessage == "";
        }

        private bool ValidateEndOfLineTypeChar(char eolTypeChar, out string errorMessage)
        {
            if (QuoteChar == eolTypeChar)
                errorMessage = string.Format(Properties.Resources.FtMeta_ValidateEndOfLineTypeChar_QuoteCharCannotBeAnEndOfLineChar, EndOfLineTypeCharToString(eolTypeChar));
            else
            {
                if (LineCommentChar == eolTypeChar)
                    errorMessage = string.Format(Properties.Resources.FtMeta_ValidateEndOfLineTypeChar_LineCommentCharCannotBeAnEndOfLineChar, EndOfLineTypeCharToString(eolTypeChar));
                else
                {
                    if (DelimiterChar == eolTypeChar)
                        errorMessage = string.Format(Properties.Resources.FtMeta_ValidateEndOfLineTypeChar_DelimiterCharCannotBeAnEndOfLineChar, EndOfLineTypeCharToString(eolTypeChar));
                    else
                    {
                        if (SubstitutionsEnabled && SubstitutionChar == eolTypeChar)
                            errorMessage = string.Format(Properties.Resources.FtMeta_ValidateEndOfLineTypeChar_SubstitutionCharCannotBeAnEndOfLineChar, EndOfLineTypeCharToString(eolTypeChar));
                        else
                        {
                            if (HeadingLineCount > 0 && HeadingPadChar == eolTypeChar)
                                errorMessage = string.Format(Properties.Resources.FtMeta_ValidateEndOfLineTypeChar_HeadingPadCharCannotBeAnEndOfLineChar, EndOfLineTypeCharToString(eolTypeChar));
                            else
                            {
                                if (HeadingLineCount > 0 && HeadingTruncateChar == eolTypeChar)
                                    errorMessage = string.Format(Properties.Resources.FtMeta_ValidateEndOfLineTypeChar_HeadingTruncateCharCannotBeAnEndOfLineChar, EndOfLineTypeCharToString(eolTypeChar));
                                else
                                {
                                    if (HeadingLineCount > 0 && HeadingEndOfValueChar == eolTypeChar)
                                        errorMessage = string.Format(Properties.Resources.FtMeta_ValidateEndOfLineTypeChar_HeadingEndOfValueCharCannotBeAnEndOfLineChar, EndOfLineTypeCharToString(eolTypeChar));
                                    else
                                    {
                                        errorMessage = "";
                                        for (int i = 0; i < FieldList.Count; i++)
                                        {
                                            if (!FieldList[i].ValidateEndOfLineTypeChar(eolTypeChar, HeadingLineCount, out errorMessage))
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return errorMessage.Length == 0;
        }

        private string EndOfLineTypeCharToString(char eolTypeChar)
        {
            string result = @"\x" + ((int)eolTypeChar).ToString("x4");
            result.Normalize();
            if (!Char.IsControl(eolTypeChar))
            {
                result += " [" + eolTypeChar.ToString() + "]";
            }

            return result;
        }
    }
}
