// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;
using System.Globalization;

namespace Xilytix.FieldedText
{
    using Serialization.Formatting;

    public abstract class FtFieldDefinition
    {
        private int index;

        private int dataType;
        private int id;
        private string metaName;
        private string[] metaHeadings;
        private int mainHeadingIndex;
        private CultureInfo culture;
        private bool fixedWidth;
        private int width;
        private bool constant;
        private bool _null;
        private FtQuotedType valueQuotedType;
        private bool valueAlwaysWriteOptionalQuote;
        private bool valueWritePrefixSpace;
        private FtPadAlignment valuePadAlignment;
        private FtPadCharType valuePadCharType;
        private char valuePadChar;
        private FtTruncateType valueTruncateType;
        private char valueTruncateChar;
        private char valueEndOfValueChar;
        private char valueNullChar;
        private FtHeadingConstraint headingConstraint;
        private FtQuotedType headingQuotedType;
        private bool headingAlwaysWriteOptionalQuote;
        private bool headingWritePrefixSpace;
        private FtPadAlignment headingPadAlignment;
        private FtPadCharType headingPadCharType;
        private char headingPadChar;
        private FtTruncateType headingTruncateType;
        private char headingTruncateChar;
        private char headingEndOfValueChar;

        private FieldFormatter formatter;
        private Type valueType;
        private bool autoLeftPad;
        private char autoPadChar;

        private bool headingLeftPad;
        private bool valueLeftPad;
        private string fixedWidthNullValueText;

        protected void SetFormatter(FieldFormatter value) { formatter = value; }

        internal protected FtFieldDefinition(int myIndex, Type myValueType, bool myAutoLeftPad)
        {
            index = myIndex;
            valueType = myValueType;
            autoLeftPad = myAutoLeftPad;
            autoPadChar = ' ';
        }

        public int Index { get { return index; } }

        public int DataType { get { return dataType; } }
        public int Id { get { return id; } }
        public string MetaName { get { return metaName; } }
        public string[] MetaHeadings { get { return metaHeadings; } }
        public int MetaHeadingCount {  get { return (metaHeadings == null) ? 0 : metaHeadings.Length; } }
        public int MainHeadingIndex { get { return mainHeadingIndex; } }
        public CultureInfo Culture { get { return culture; } }
        public bool FixedWidth { get { return fixedWidth; } }
        public int Width { get { return width; } }
        public bool Constant { get { return constant; } }
        public bool Null { get { return _null; } }
        public FtQuotedType ValueQuotedType { get { return valueQuotedType; } }
        public bool ValueAlwaysWriteOptionalQuote { get { return valueAlwaysWriteOptionalQuote; } }
        public bool ValueWritePrefixSpace { get { return valueWritePrefixSpace; } }
        public FtPadAlignment ValuePadAlignment { get { return valuePadAlignment; } }
        public FtPadCharType ValuePadCharType { get { return valuePadCharType; } }
        public char ValuePadChar { get { return valuePadChar; } }
        public FtTruncateType ValueTruncateType { get { return valueTruncateType; } }
        public char ValueTruncateChar { get { return valueTruncateChar; } }
        public char ValueEndOfValueChar { get { return valueEndOfValueChar; } }
        public char ValueNullChar { get { return valueNullChar; } }
        public FtHeadingConstraint HeadingConstraint { get { return headingConstraint; } }
        public FtQuotedType HeadingQuotedType { get { return headingQuotedType; } }
        public bool HeadingAlwaysWriteOptionalQuote { get { return headingAlwaysWriteOptionalQuote; } }
        public bool HeadingWritePrefixSpace { get { return headingWritePrefixSpace; } }
        public FtPadAlignment HeadingPadAlignment { get { return headingPadAlignment; } }
        public FtPadCharType HeadingPadCharType { get { return headingPadCharType; } }
        public char HeadingPadChar { get { return headingPadChar; } }
        public FtTruncateType HeadingTruncateType { get { return headingTruncateType; } }
        public char HeadingTruncateChar { get { return headingTruncateChar; } }
        public char HeadingEndOfValueChar { get { return headingEndOfValueChar; } }

        public Type ValueType { get { return valueType; } }
        public string DataTypeName { get { return FtStandardDataType.ToName(dataType); } }

        public bool AutoLeftPad { get { return autoLeftPad; } }
        public char AutoPadChar { get { return autoPadChar; } }

        public bool HeadingLeftPad {  get { return headingLeftPad; } }
        public bool ValueLeftPad { get { return valueLeftPad; } }
        public string FixedWidthNullValueText { get { return fixedWidthNullValueText; } }

        internal protected virtual void LoadMeta(FtMetaField metaField, CultureInfo myCulture, int myMainHeadingIndex)
        {
            dataType = metaField.DataType;
            id = metaField.Id;
            metaName = metaField.Name;
            metaHeadings = metaField.Headings;
            mainHeadingIndex = myMainHeadingIndex;
            culture = myCulture;
            fixedWidth = metaField.FixedWidth;
            width = metaField.Width;
            constant = metaField.Constant;
            _null = metaField.Null;
            valueQuotedType = metaField.ValueQuotedType;
            valueAlwaysWriteOptionalQuote = metaField.ValueAlwaysWriteOptionalQuote;
            valueWritePrefixSpace = metaField.ValueWritePrefixSpace;
            valuePadAlignment = metaField.ValuePadAlignment;
            valuePadCharType = metaField.ValuePadCharType;
            valuePadChar = metaField.ValuePadChar;
            valueTruncateType = metaField.ValueTruncateType;
            valueTruncateChar = metaField.ValueTruncateChar;
            valueEndOfValueChar = metaField.ValueEndOfValueChar;
            valueNullChar = metaField.ValueNullChar;
            headingConstraint = metaField.HeadingConstraint;
            headingQuotedType = metaField.HeadingQuotedType;
            headingAlwaysWriteOptionalQuote = metaField.HeadingAlwaysWriteOptionalQuote;
            headingWritePrefixSpace = metaField.HeadingWritePrefixSpace;
            headingPadAlignment = metaField.HeadingPadAlignment;
            headingPadCharType = metaField.HeadingPadCharType;
            headingPadChar = metaField.HeadingPadChar;
            headingTruncateType = metaField.HeadingTruncateType;
            headingTruncateChar = metaField.HeadingTruncateChar;
            headingEndOfValueChar = metaField.HeadingEndOfValueChar;

            formatter.Culture = culture;

            if (
                 ((headingConstraint == FtHeadingConstraint.NameIsMain) || (headingConstraint == FtHeadingConstraint.NameConstant))
                 &&
                 (mainHeadingIndex >= 0)
                 &&
                 (mainHeadingIndex < MetaHeadingCount)
               )
            {
                metaHeadings[mainHeadingIndex] = metaName;
            }

            switch (headingPadAlignment)
            {
                case FtPadAlignment.Left:
                    headingLeftPad = true;
                    break;
                case FtPadAlignment.Right:
                    headingLeftPad = false;
                    break;
                case FtPadAlignment.Auto:
                    headingLeftPad = autoLeftPad;
                    break;
                default: throw FtInternalException.Create(InternalError.FtFieldFieldDefinition_LoadMeta_UnsupportedHeadingPadAlignment, headingPadAlignment.ToString());
            }

            switch (valuePadAlignment)
            {
                case FtPadAlignment.Left:
                    valueLeftPad = true;
                    break;
                case FtPadAlignment.Right:
                    valueLeftPad = false;
                    break;
                case FtPadAlignment.Auto:
                    valueLeftPad = autoLeftPad;
                    break;
                default: throw FtInternalException.Create(InternalError.FtFieldFieldDefinition_LoadMeta_UnsupportedValuePadAlignment, headingPadAlignment.ToString());
            }

            if (fixedWidth)
            {
                fixedWidthNullValueText = new string(ValueNullChar, Width);
            }
        }
    }
}
