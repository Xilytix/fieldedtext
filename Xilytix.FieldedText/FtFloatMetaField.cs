// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System.Globalization;

namespace Xilytix.FieldedText
{
    using Factory;
    using MetaSerialization;

    public class FtFloatMetaField : FtGenericMetaField<double>
    {
        public new const int DataType = FtStandardDataType.Float;

        public const string DefaultFormat = MetaSerializationDefaults.FloatField.Format;
        public const NumberStyles DefaultStyles = MetaSerializationDefaults.FloatField.Styles;
        public const double DefaultValue = 0.0;

        public new const int DefaultSequenceRedirectType = MetaSerializationDefaults.FloatField.SequenceRedirectType;

        protected override int GetDefaultSequenceRedirectType()
        {
            return DefaultSequenceRedirectType;
        }

        public FtFloatMetaField(int myHeadingCount = 0) : base(DataType, myHeadingCount, DefaultValue)
        {
            LoadFloatDefaults();
        }

        public string Format { get; set; }
        public NumberStyles Styles { get; set; }

        private void LoadFloatDefaults()
        {
            Format = DefaultFormat;
            Styles = DefaultStyles;
        }

        public override void LoadDefaults(bool leaveNameAsIs = true)
        {
            base.LoadDefaults(leaveNameAsIs);
            LoadFloatDefaults();
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

            FtFloatMetaField typedSource = source as FtFloatMetaField;
            Format = typedSource.Format;
            Styles = typedSource.Styles;
        }
    }
}
