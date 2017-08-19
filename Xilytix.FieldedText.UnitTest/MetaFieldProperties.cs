// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

namespace Xilytix.FieldedText.UnitTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using FieldedText;

    internal struct MetaFieldProperties
    {
        const int DefaultId = 0;
        const bool DefaultFixedWidth = false;
        const int DefaultWidth = 1;
        const bool DefaultConstant = false;
        const bool DefaultNull = false;
        const FtQuotedType DefaultValueQuotedType = FtQuotedType.Optional;
        const bool DefaultValueAlwaysWriteOptionalQuote = false;
        const bool DefaultValueWritePrefixSpace = false;
        const FtPadAlignment DefaultValuePadAlignment = FtPadAlignment.Auto;
        const FtPadCharType DefaultValuePadCharType = FtPadCharType.EndOfValue;
        const char DefaultValuePadChar = ' ';
        const FtTruncateType DefaultValueTruncateType = FtTruncateType.Exception;
        const char DefaultValueTruncateChar = '#';
        const char DefaultValueEndOfValueChar = '\x03';
        const char DefaultValueNullChar = '*';
        const FtHeadingConstraint DefaultHeadingConstraint = MetaProperties.DefaultHeadingConstraint;
        const FtQuotedType DefaultHeadingQuotedType = MetaProperties.DefaultHeadingQuotedType;
        const bool DefaultHeadingAlwaysWriteOptionalQuote = MetaProperties.DefaultHeadingAlwaysWriteOptionalQuote;
        const bool DefaultHeadingWritePrefixSpace = MetaProperties.DefaultHeadingWritePrefixSpace;
        const FtPadAlignment DefaultHeadingPadAlignment = MetaProperties.DefaultHeadingPadAlignment;
        const FtPadCharType DefaultHeadingPadCharType = MetaProperties.DefaultHeadingPadCharType;
        const char DefaultHeadingPadChar = MetaProperties.DefaultHeadingPadChar;
        const FtTruncateType DefaultHeadingTruncateType = MetaProperties.DefaultHeadingTruncateType;
        const char DefaultHeadingTruncateChar = MetaProperties.DefaultHeadingTruncateChar;
        const char DefaultHeadingEndOfValueChar = MetaProperties.DefaultHeadingEndOfValueChar;

        public int Id { get; set; }
        public string Name { get; set; }
        public string[] Headings { get; set; }
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

        internal void LoadDefaults(FtMetaField forHeadingsField = null)
        {
            Id = DefaultId;
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
            if (forHeadingsField == null)
            {
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
            else
            {
                HeadingConstraint = forHeadingsField.DefaultHeadingConstraint;
                HeadingQuotedType = forHeadingsField.DefaultHeadingQuotedType;
                HeadingAlwaysWriteOptionalQuote = forHeadingsField.DefaultHeadingAlwaysWriteOptionalQuote;
                HeadingWritePrefixSpace = forHeadingsField.DefaultHeadingWritePrefixSpace;
                HeadingPadAlignment = forHeadingsField.DefaultHeadingPadAlignment;
                HeadingPadCharType = forHeadingsField.DefaultHeadingPadCharType;
                HeadingPadChar = forHeadingsField.DefaultHeadingPadChar;
                HeadingTruncateType = forHeadingsField.DefaultHeadingTruncateType;
                HeadingTruncateChar = forHeadingsField.DefaultHeadingTruncateChar;
                HeadingEndOfValueChar = forHeadingsField.DefaultHeadingEndOfValueChar;
            }
        }

        internal void LoadIntoMetaField(ref FtMetaField field, bool includeName)
        {
            if (includeName)
            {
                field.Name = Name;
            }
            field.Id = Id;
            field.FixedWidth = FixedWidth;
            field.Width = Width;
            field.Constant = Constant;
            field.Null = Null;
            field.ValueQuotedType = ValueQuotedType;
            field.ValueAlwaysWriteOptionalQuote = ValueAlwaysWriteOptionalQuote;
            field.ValueWritePrefixSpace = ValueWritePrefixSpace;
            field.ValuePadAlignment = ValuePadAlignment;
            field.ValuePadCharType = ValuePadCharType;
            field.ValuePadChar = ValuePadChar;
            field.ValueTruncateType = ValueTruncateType;
            field.ValueTruncateChar = ValueTruncateChar;
            field.ValueEndOfValueChar = ValueEndOfValueChar;
            field.ValueNullChar = ValueNullChar;
            field.HeadingConstraint = HeadingConstraint;
            field.HeadingQuotedType = HeadingQuotedType;
            field.HeadingAlwaysWriteOptionalQuote = HeadingAlwaysWriteOptionalQuote;
            field.HeadingWritePrefixSpace = HeadingWritePrefixSpace;
            field.HeadingPadAlignment = HeadingPadAlignment;
            field.HeadingPadCharType = HeadingPadCharType;
            field.HeadingPadChar = HeadingPadChar;
            field.HeadingTruncateType = HeadingTruncateType;
            field.HeadingTruncateChar = HeadingTruncateChar;
            field.HeadingEndOfValueChar = HeadingEndOfValueChar;
        }

        internal void AssertMetaFieldAreEqual(FtMetaField field, bool includeName)
        {
            if (includeName)
            {
                Assert.AreEqual<string>(Name, field.Name);
            }
            Assert.AreEqual<int>(Id, field.Id);
            Assert.AreEqual<bool>(FixedWidth, field.FixedWidth);
            Assert.AreEqual<int>(Width, field.Width);
            Assert.AreEqual<bool>(Constant, field.Constant);
            Assert.AreEqual<bool>(Null, field.Null);
            Assert.AreEqual<FtQuotedType>(ValueQuotedType, field.ValueQuotedType);
            Assert.AreEqual<bool>(ValueAlwaysWriteOptionalQuote, field.ValueAlwaysWriteOptionalQuote);
            Assert.AreEqual<bool>(ValueWritePrefixSpace, field.ValueWritePrefixSpace);
            Assert.AreEqual<FtPadAlignment>(ValuePadAlignment, field.ValuePadAlignment);
            Assert.AreEqual<FtPadCharType>(ValuePadCharType, field.ValuePadCharType);
            Assert.AreEqual<char>(ValuePadChar, field.ValuePadChar);
            Assert.AreEqual<FtTruncateType>(ValueTruncateType, field.ValueTruncateType);
            Assert.AreEqual<char>(ValueTruncateChar, field.ValueTruncateChar);
            Assert.AreEqual<char>(ValueEndOfValueChar, field.ValueEndOfValueChar);
            Assert.AreEqual<char>(ValueNullChar, field.ValueNullChar);
            Assert.AreEqual<FtHeadingConstraint>(HeadingConstraint, field.HeadingConstraint);
            Assert.AreEqual<FtQuotedType>(HeadingQuotedType, field.HeadingQuotedType);
            Assert.AreEqual<bool>(HeadingAlwaysWriteOptionalQuote, field.HeadingAlwaysWriteOptionalQuote);
            Assert.AreEqual<bool>(HeadingWritePrefixSpace, field.HeadingWritePrefixSpace);
            Assert.AreEqual<FtPadAlignment>(HeadingPadAlignment, field.HeadingPadAlignment);
            Assert.AreEqual<FtPadCharType>(HeadingPadCharType, field.HeadingPadCharType);
            Assert.AreEqual<char>(HeadingPadChar, field.HeadingPadChar);
            Assert.AreEqual<FtTruncateType>(HeadingTruncateType, field.HeadingTruncateType);
            Assert.AreEqual<char>(HeadingTruncateChar, field.HeadingTruncateChar);
            Assert.AreEqual<char>(HeadingEndOfValueChar, field.HeadingEndOfValueChar);
        }
    }
}
