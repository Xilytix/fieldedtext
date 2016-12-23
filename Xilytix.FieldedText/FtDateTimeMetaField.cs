// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;
using System.Globalization;

namespace Xilytix.FieldedText
{
    using Factory;
    using MetaSerialization;

    public class FtDateTimeMetaField : FtGenericMetaField<DateTime>
    {
        public new const int DataType = FtStandardDataType.DateTime;

        public const string DefaultFormat = MetaSerializationDefaults.DateTimeField.Format;
        public const DateTimeStyles DefaultStyles = MetaSerializationDefaults.DateTimeField.Styles;
        public static readonly DateTime DefaultValue = new DateTime(0);

        public new const int DefaultSequenceRedirectType = MetaSerializationDefaults.DateTimeField.SequenceRedirectType;

        protected override int GetDefaultSequenceRedirectType()
        {
            return DefaultSequenceRedirectType;
        }

        public FtDateTimeMetaField(int myHeadingCount = 0) : base(DataType, myHeadingCount, DefaultValue)
        {
            LoadDateTimeDefaults();
        }

        public string Format { get; set; }
        public DateTimeStyles Styles { get; set; }

        private void LoadDateTimeDefaults()
        {
            Format = DefaultFormat;
            Styles = DefaultStyles;
        }

        public override void LoadDefaults(bool leaveNameAsIs = true)
        {
            base.LoadDefaults(leaveNameAsIs);
            LoadDateTimeDefaults();
        }

        protected internal override FtMetaField CreateCopy()
        {
            FtMetaField field = FieldFactory.CreateMetaField(DataType, HeadingCount);
            field.Assign(this);
            return field;
        }

        protected internal override void Assign(FtMetaField source)
        {
            base.Assign(source);

            FtDateTimeMetaField typedSource = source as FtDateTimeMetaField;
            Format = typedSource.Format;
            Styles = typedSource.Styles;
        }
    }
}
