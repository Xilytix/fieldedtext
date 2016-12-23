// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

namespace Xilytix.FieldedText
{
    public abstract class FtGenericMetaField<T>: FtMetaField
    {
        private T defaultValue;

        public FtGenericMetaField(int myDataType, int myHeadingCount, T myDefaultValue) : base(myDataType, myHeadingCount)
        {
            defaultValue = myDefaultValue;
        }

        public T Value { get; set; }

        public override void LoadDefaults(bool leaveNameAsIs = true)
        {
            base.LoadDefaults(leaveNameAsIs);
            Value = defaultValue;
        }

        protected internal override void Assign(FtMetaField source)
        {
            base.Assign(source);

            FtGenericMetaField<T> typedSource = source as FtGenericMetaField<T>;
            Value = typedSource.Value;
        }
    }
}
