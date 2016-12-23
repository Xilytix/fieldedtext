// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

namespace Xilytix.FieldedText
{
    using Factory;
    using MetaSerialization;

    public class FtBooleanMetaField : FtGenericMetaField<bool>
    {
        public new const int DataType = FtStandardDataType.Boolean;

        public const string DefaultFalseText = MetaSerializationDefaults.BooleanField.FalseText;
        public const string DefaultTrueText = MetaSerializationDefaults.BooleanField.TrueText;
        public const FtBooleanStyles DefaultStyles = MetaSerializationDefaults.BooleanField.Styles;
        public const bool DefaultValue = false;

        public new const int DefaultSequenceRedirectType = MetaSerializationDefaults.BooleanField.SequenceRedirectType;

        protected override int GetDefaultSequenceRedirectType()
        {
            return DefaultSequenceRedirectType;
        }

        public FtBooleanMetaField(int myHeadingCount = 0): base(DataType, myHeadingCount, DefaultValue)
        {
            LoadBooleanDefaults();
        }

        public string FalseText { get; set; }
        public string TrueText { get; set; }
        public FtBooleanStyles Styles { get; set; }

        private void LoadBooleanDefaults()
        {
            FalseText = DefaultFalseText;
            TrueText = DefaultTrueText;
            Styles = DefaultStyles;
        }

        public override void LoadDefaults(bool leaveNameAsIs = true)
        {
            base.LoadDefaults(leaveNameAsIs);
            LoadBooleanDefaults();
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

            FtBooleanMetaField typedSource = source as FtBooleanMetaField;
            FalseText = typedSource.FalseText;
            TrueText = typedSource.TrueText;
            Styles = typedSource.Styles;
        }
    }
}
