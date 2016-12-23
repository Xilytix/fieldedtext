// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System.Globalization;

namespace Xilytix.FieldedText
{
    using Factory;
    using MetaSerialization;

    public class FtIntegerMetaField: FtGenericMetaField<long>
    {
        public new const int DataType = FtStandardDataType.Integer;

        public const string DefaultFormat = MetaSerializationDefaults.IntegerField.Format;
        public const NumberStyles DefaultStyles = MetaSerializationDefaults.IntegerField.Styles;
        public const long DefaultValue = 0;

        public new const int DefaultSequenceRedirectType = MetaSerializationDefaults.IntegerField.SequenceRedirectType;

        protected override int GetDefaultSequenceRedirectType()
        {
            return DefaultSequenceRedirectType;
        }

        public FtIntegerMetaField(int myHeadingCount = 0) : base(DataType, myHeadingCount, DefaultValue)
        {
            LoadIntegerDefaults();
        }

        public string Format { get; set; }
        public NumberStyles Styles { get; set; }

        private void LoadIntegerDefaults()
        {
            Format = DefaultFormat;
            Styles = DefaultStyles;
        }

        public override void LoadDefaults(bool leaveNameAsIs = true)
        {
            base.LoadDefaults(leaveNameAsIs);
            LoadIntegerDefaults();
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

            FtIntegerMetaField typedSource = source as FtIntegerMetaField;
            Format = typedSource.Format;
            Styles = typedSource.Styles;
        }
    }
}
