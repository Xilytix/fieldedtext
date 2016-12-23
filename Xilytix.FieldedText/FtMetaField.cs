// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;
using System.Globalization;

namespace Xilytix.FieldedText
{
    using MetaSerialization;

    public abstract class FtMetaField
    {
        public enum PropertyId
        {
            Index,
            Id,
            DataType,
            Name,
            FixedWidth,
            Width,
            Constant,
            ValueQuotedType,
            ValueAlwaysWriteOptionalQuote,
            ValueWritePrefixSpace,
            ValuePadAlignment,
            ValuePadCharType,
            ValuePadChar,
            ValueTruncateType,
            ValueTruncateChar,
            ValueEndOfValueChar,
            ValueNullChar,
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
            Null,
            Headings,

            // All Descendants,
            Value,

            // Boolean, DateTime, Number,
            Styles,

            // DateTime, Number
            Format,

            // Boolean
            FalseText,
            TrueText,
        }

        public const int DefaultId = MetaSerializationDefaults.Field.Id;
        public const string DefaultName = MetaSerializationDefaults.Field.Name;
        public const int DefaultDataType = MetaSerializationDefaults.Field.DataType;
        public const bool DefaultFixedWidth = MetaSerializationDefaults.Field.FixedWidth;
        public const int DefaultWidth = MetaSerializationDefaults.Field.Width;
        public const bool DefaultConstant = MetaSerializationDefaults.Field.Constant;
        public const bool DefaultNull = MetaSerializationDefaults.Field.Null;
        public const FtQuotedType DefaultValueQuotedType = MetaSerializationDefaults.Field.ValueQuotedType;
        public const bool DefaultValueAlwaysWriteOptionalQuote = MetaSerializationDefaults.Field.ValueAlwaysWriteOptionalQuote;
        public const bool DefaultValueWritePrefixSpace = MetaSerializationDefaults.Field.ValueWritePrefixSpace;
        public const FtPadAlignment DefaultValuePadAlignment = MetaSerializationDefaults.Field.ValuePadAlignment;
        public const FtPadCharType DefaultValuePadCharType = MetaSerializationDefaults.Field.ValuePadCharType;
        public const char DefaultValuePadChar = MetaSerializationDefaults.Field.ValuePadChar;
        public const FtTruncateType DefaultValueTruncateType = MetaSerializationDefaults.Field.ValueTruncateType;
        public const char DefaultValueTruncateChar = MetaSerializationDefaults.Field.ValueTruncateChar;
        public const char DefaultValueEndOfValueChar = MetaSerializationDefaults.Field.ValueEndOfValueChar;
        public const char DefaultValueNullChar = MetaSerializationDefaults.Field.ValueNullChar;

        internal delegate FtHeadingConstraint DefaultHeadingConstraintRequiredDelegate();
        internal delegate FtQuotedType DefaultHeadingQuotedTypeRequiredDelegate();
        internal delegate bool DefaultHeadingAlwaysWriteOptionalQuoteRequiredDelegate();
        internal delegate bool DefaultHeadingWritePrefixSpaceRequiredDelegate();
        internal delegate FtPadAlignment DefaultHeadingPadAlignmentRequiredDelegate();
        internal delegate FtPadCharType DefaultHeadingPadCharTypeRequiredDelegate();
        internal delegate char DefaultHeadingPadCharRequiredDelegate();
        internal delegate FtTruncateType DefaultHeadingTruncateTypeRequiredDelegate();
        internal delegate char DefaultHeadingTruncateCharRequiredDelegate();
        internal delegate char DefaultHeadingEndOfValueCharRequiredDelegate();

        internal DefaultHeadingConstraintRequiredDelegate DefaultHeadingConstraintRequiredEvent;
        internal DefaultHeadingQuotedTypeRequiredDelegate DefaultHeadingQuotedTypeRequiredEvent;
        internal DefaultHeadingAlwaysWriteOptionalQuoteRequiredDelegate DefaultHeadingAlwaysWriteOptionalQuoteRequiredEvent;
        internal DefaultHeadingWritePrefixSpaceRequiredDelegate DefaultHeadingWritePrefixSpaceRequiredEvent;
        internal DefaultHeadingPadAlignmentRequiredDelegate DefaultHeadingPadAlignmentRequiredEvent;
        internal DefaultHeadingPadCharTypeRequiredDelegate DefaultHeadingPadCharTypeRequiredEvent;
        internal DefaultHeadingPadCharRequiredDelegate DefaultHeadingPadCharRequiredEvent;
        internal DefaultHeadingTruncateTypeRequiredDelegate DefaultHeadingTruncateTypeRequiredEvent;
        internal DefaultHeadingTruncateCharRequiredDelegate DefaultHeadingTruncateCharRequiredEvent;
        internal DefaultHeadingEndOfValueCharRequiredDelegate DefaultHeadingEndOfValueCharRequiredEvent;

        private int dataType;
        private string[] headings;

        protected abstract int GetDefaultSequenceRedirectType();

        public FtMetaField(int myDataType, int myHeadingCount)
        {
            dataType = myDataType;
            headings = new string[myHeadingCount];
            LoadBaseDefaults(false);
        }

        public string[] GetDefaultHeadings() { return new string[HeadingCount]; }

        public int DataType { get { return dataType; } }
        public int Id { get; set; }
        public string Name { get; set; }
        public int HeadingCount { get { return (headings == null) ? 0 : headings.Length; } internal set { Array.Resize<string>(ref headings, value); } }
        public string[] Headings
        {
            get { return headings; }
            set
            {
                headings = value;
                Array.Resize<string>(ref headings, HeadingCount);
            }
        }
        public bool FixedWidth { get; set; }
        public int Width { get; set; }
        public bool Constant { get; set; }
        public bool Null { get; set; }
        public FtQuotedType ValueQuotedType { get; set; }
        public bool ValueAlwaysWriteOptionalQuote { get; set; }
        public bool ValueWritePrefixSpace { get; set; }
        public FtPadAlignment ValuePadAlignment { get; set; }
        public FtPadCharType ValuePadCharType { get; set; }
        public char ValuePadChar { get; set; }
        public FtTruncateType ValueTruncateType { get; set; }
        public char ValueTruncateChar { get; set; }
        public char ValueEndOfValueChar { get; set; }
        public char ValueNullChar { get; set; }
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

        public FtHeadingConstraint DefaultHeadingConstraint
        {
            get
            {
                return (DefaultHeadingConstraintRequiredEvent == null) ? MetaSerializationDefaults.Root.HeadingConstraint : DefaultHeadingConstraintRequiredEvent();
            }
        }
        public FtQuotedType DefaultHeadingQuotedType
        {
            get
            {
                return (DefaultHeadingQuotedTypeRequiredEvent == null) ? MetaSerializationDefaults.Root.HeadingQuotedType : DefaultHeadingQuotedTypeRequiredEvent();
            }
        }
        public bool DefaultHeadingAlwaysWriteOptionalQuote
        {
            get
            {
                return (DefaultHeadingAlwaysWriteOptionalQuoteRequiredEvent == null) ? MetaSerializationDefaults.Root.HeadingAlwaysWriteOptionalQuote : DefaultHeadingAlwaysWriteOptionalQuoteRequiredEvent();
            }
        }
        public bool DefaultHeadingWritePrefixSpace
        {
            get
            {
                return (DefaultHeadingWritePrefixSpaceRequiredEvent == null) ? MetaSerializationDefaults.Root.HeadingWritePrefixSpace : DefaultHeadingWritePrefixSpaceRequiredEvent();
            }
        }
        public FtPadAlignment DefaultHeadingPadAlignment
        {
            get
            {
                return (DefaultHeadingPadAlignmentRequiredEvent == null) ? MetaSerializationDefaults.Root.HeadingPadAlignment : DefaultHeadingPadAlignmentRequiredEvent();
            }
        }
        public FtPadCharType DefaultHeadingPadCharType
        {
            get
            {
                return (DefaultHeadingPadCharTypeRequiredEvent == null) ? MetaSerializationDefaults.Root.HeadingPadCharType : DefaultHeadingPadCharTypeRequiredEvent();
            }
        }
        public char DefaultHeadingPadChar
        {
            get
            {
                return (DefaultHeadingPadCharRequiredEvent == null) ? MetaSerializationDefaults.Root.HeadingPadChar : DefaultHeadingPadCharRequiredEvent();
            }
        }
        public FtTruncateType DefaultHeadingTruncateType
        {
            get
            {
                return (DefaultHeadingTruncateTypeRequiredEvent == null) ? MetaSerializationDefaults.Root.HeadingTruncateType : DefaultHeadingTruncateTypeRequiredEvent();
            }
        }
        public char DefaultHeadingTruncateChar
        {
            get
            {
                return (DefaultHeadingTruncateCharRequiredEvent == null) ? MetaSerializationDefaults.Root.HeadingTruncateChar : DefaultHeadingTruncateCharRequiredEvent();
            }
        }
        public char DefaultHeadingEndOfValueChar
        {
            get
            {
                return (DefaultHeadingEndOfValueCharRequiredEvent == null) ? MetaSerializationDefaults.Root.HeadingEndOfValueChar : DefaultHeadingEndOfValueCharRequiredEvent();
            }
        }

        public int DefaultSequenceRedirectType { get { return GetDefaultSequenceRedirectType(); } }

        public virtual void LoadDefaults(bool leaveNameAsIs = true)
        {
            LoadBaseDefaults(leaveNameAsIs);
        }

        private void LoadBaseDefaults(bool leaveNameAsIs)
        {
            if (!leaveNameAsIs)
            {
                Name = DefaultName;
            }
            Id = DefaultId;
            headings = GetDefaultHeadings();
            FixedWidth = DefaultFixedWidth;
            Width = DefaultWidth;
            Constant = DefaultConstant;
            Null = DefaultNull;
            ValueQuotedType = DefaultValueQuotedType;
            ValueAlwaysWriteOptionalQuote = DefaultValueAlwaysWriteOptionalQuote;
            ValueWritePrefixSpace = DefaultValueWritePrefixSpace;
            ValuePadAlignment = DefaultValuePadAlignment;
            ValuePadCharType = DefaultValuePadCharType;
            ValuePadChar = DefaultValuePadChar;
            ValueTruncateType = DefaultValueTruncateType;
            ValueTruncateChar = DefaultValueTruncateChar;
            ValueEndOfValueChar = DefaultValueEndOfValueChar;
            ValueNullChar = DefaultValueNullChar;
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

        protected internal abstract FtMetaField CreateCopy();
        protected internal virtual void Assign(FtMetaField source)
        {
            Id = source.Id;
            Name = source.Name;
            headings = new string[source.HeadingCount];
            Array.Copy(source.Headings, headings, source.HeadingCount);
            FixedWidth = source.FixedWidth;
            Width = source.Width;
            Constant = source.Constant;
            Null = source.Null;
            ValueQuotedType = source.ValueQuotedType;
            ValueAlwaysWriteOptionalQuote = source.ValueAlwaysWriteOptionalQuote;
            ValueWritePrefixSpace = source.ValueWritePrefixSpace;
            ValuePadAlignment = source.ValuePadAlignment;
            ValuePadCharType = source.ValuePadCharType;
            ValuePadChar = source.ValuePadChar;
            ValueTruncateType = source.ValueTruncateType;
            ValueTruncateChar = source.ValueTruncateChar;
            ValueEndOfValueChar = source.ValueEndOfValueChar;
            ValueNullChar = source.ValueNullChar;
            HeadingConstraint = source.HeadingConstraint;
            HeadingQuotedType = source.HeadingQuotedType;
            HeadingAlwaysWriteOptionalQuote = source.HeadingAlwaysWriteOptionalQuote;
            HeadingWritePrefixSpace = source.HeadingWritePrefixSpace;
            HeadingPadAlignment = source.HeadingPadAlignment;
            HeadingPadCharType = source.HeadingPadCharType;
            HeadingPadChar = source.HeadingPadChar;
            HeadingTruncateType = source.HeadingTruncateType;
            HeadingTruncateChar = source.HeadingTruncateChar;
            HeadingEndOfValueChar = source.HeadingEndOfValueChar;
        }

        internal bool ValidateEndOfLineTypeChar(char eolTypeChar, int HeadingLineCount, out string errorMessage)
        {
            if (!FixedWidth)
                errorMessage = "";
            else
            {
                if (ValueNullChar == eolTypeChar)
                    errorMessage = string.Format(Properties.Resources.FtMetaField_ValidateEndOfLineTypeChar_ValueNullCharCannotBeAnEndOfLineChar, 
                                                 Name, EndOfLineTypeCharToString(eolTypeChar));
                else
                {
                    if (ValuePadChar == eolTypeChar)
                        errorMessage = string.Format(Properties.Resources.FtMetaField_ValidateEndOfLineTypeChar_ValuePadCharCannotBeAnEndOfLineChar, 
                                                     Name, EndOfLineTypeCharToString(eolTypeChar));
                    else
                    {
                        if (ValueTruncateChar == eolTypeChar)
                            errorMessage = string.Format(Properties.Resources.FtMetaField_ValidateEndOfLineTypeChar_ValueTruncateCharCannotBeAnEndOfLineChar, 
                                                         Name, EndOfLineTypeCharToString(eolTypeChar));
                        else
                        {
                            if (ValueEndOfValueChar == eolTypeChar)
                                errorMessage = string.Format(Properties.Resources.FtMetaField_ValidateEndOfLineTypeChar_ValueEndOfValueCharCannotBeAnEndOfLineChar, 
                                                             Name, EndOfLineTypeCharToString(eolTypeChar));
                            else
                            {
                                if (HeadingLineCount > 0 && HeadingPadChar == eolTypeChar)
                                    errorMessage = string.Format(Properties.Resources.FtMetaField_ValidateEndOfLineTypeChar_HeadingPadCharCannotBeAnEndOfLineChar, 
                                                                 Name, EndOfLineTypeCharToString(eolTypeChar));
                                else
                                {
                                    if (HeadingLineCount > 0 && HeadingTruncateChar == eolTypeChar)
                                        errorMessage = string.Format(Properties.Resources.FtMetaField_ValidateEndOfLineTypeChar_HeadingTruncateCharCannotBeAnEndOfLineChar, 
                                                                     Name, EndOfLineTypeCharToString(eolTypeChar));
                                    else
                                    {
                                        if (HeadingLineCount > 0 && HeadingEndOfValueChar == eolTypeChar)
                                            errorMessage = string.Format(Properties.Resources.FtMetaField_ValidateEndOfLineTypeChar_HeadingEndOfValueCharCannotBeAnEndOfLineChar, 
                                                                         Name, EndOfLineTypeCharToString(eolTypeChar));
                                        else
                                        {
                                            errorMessage = "";
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
            if (!Char.IsControl(eolTypeChar))
            {
                result += " [" + eolTypeChar.ToString() + "]";
            }

            return result;
        }
    }
}
