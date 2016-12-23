// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

namespace Xilytix.FieldedText
{
    using Factory;
    using MetaSerialization;

    public class FtStringMetaField : FtGenericMetaField<string>
    {
        public new const int DataType = FtStandardDataType.String;

        public new const int DefaultSequenceRedirectType = MetaSerializationDefaults.StringField.SequenceRedirectType;
        public const string DefaultValue = "";

        protected override int GetDefaultSequenceRedirectType()
        {
            return DefaultSequenceRedirectType;
        }

        public FtStringMetaField(int myHeadingCount = 0) : base(DataType, myHeadingCount, DefaultValue)
        {
        }

        protected internal override FtMetaField CreateCopy()
        {
            FtMetaField field = FieldFactory.CreateMetaField(DataType, HeadingCount);
            field.Assign(this);
            return field;
        }

        //  no need for Assign override as no members to Assign
    }
}
