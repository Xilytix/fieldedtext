// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System.Globalization;

namespace Xilytix.FieldedText
{
    using Factory;
    using MetaSerialization;

    public class FtDecimalMetaField: FtGenericMetaField<decimal>
    {
        public new const int DataType = FtStandardDataType.Decimal;

        public const string DefaultFormat = MetaSerializationDefaults.DecimalField.Format;
        public const NumberStyles DefaultStyles = MetaSerializationDefaults.DecimalField.Styles;
        public const decimal DefaultValue = 0;

        public new const int DefaultSequenceRedirectType = MetaSerializationDefaults.DecimalField.SequenceRedirectType;

        protected override int GetDefaultSequenceRedirectType()
        {
            return DefaultSequenceRedirectType;
        }

        public FtDecimalMetaField(int myHeadingCount = 0) : base(DataType, myHeadingCount, DefaultValue)
        {
            LoadDecimalDefaults();
        }

        public string Format { get; set; }
        public NumberStyles Styles { get; set; }

        private void LoadDecimalDefaults()
        {
            Format = DefaultFormat;
            Styles = DefaultStyles;
        }

        public override void LoadDefaults(bool leaveNameAsIs = true)
        {
            base.LoadDefaults(leaveNameAsIs);
            LoadDecimalDefaults();
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

            FtDecimalMetaField typedSource = source as FtDecimalMetaField;
            Format = typedSource.Format;
            Styles = typedSource.Styles;
        }
    }
}
