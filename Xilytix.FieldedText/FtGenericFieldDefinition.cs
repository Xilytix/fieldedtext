// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

namespace Xilytix.FieldedText
{
    using System.Globalization;

    public abstract class FtGenericFieldDefinition<T>: FtFieldDefinition
    {
        private T value;

        public T Value { get { return value; } }

        internal protected FtGenericFieldDefinition(int myIndex, bool myAutoLeftPad): base(myIndex, typeof(T), myAutoLeftPad) { }

        internal protected override void LoadMeta(FtMetaField metaField, CultureInfo myCulture, int myMainHeadingIndex)
        {
            base.LoadMeta(metaField, myCulture, myMainHeadingIndex);

            if (metaField.Constant && !metaField.Null)
            {
                FtGenericMetaField<T> genericMetaField = metaField as FtGenericMetaField<T>;
                value = genericMetaField.Value;
            }
        }

        internal protected abstract string GetValueText(T value);
        internal protected abstract T ParseValueText(string text);
    }
}
