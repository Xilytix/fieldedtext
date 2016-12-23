// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System.Globalization;

namespace Xilytix.FieldedText
{
    using Serialization.Formatting;

    public class FtIntegerFieldDefinition: FtGenericFieldDefinition<long>
    {
        public new const int DataType = FtStandardDataType.Integer;
        public new const bool AutoLeftPad = true;

        private IntegerFieldFormatter formatter;

        internal protected FtIntegerFieldDefinition(int myIndex) : base(myIndex, AutoLeftPad) { formatter = new IntegerFieldFormatter(); SetFormatter(formatter); }

        public string Format { get { return formatter.Format; } }
        public NumberStyles Styles { get { return formatter.Styles; } }

        internal protected override void LoadMeta(FtMetaField metaField, CultureInfo myCulture, int myMainHeadingIndex)
        {
            base.LoadMeta(metaField, myCulture, myMainHeadingIndex);

            FtIntegerMetaField integerMetaField = metaField as FtIntegerMetaField;
            formatter.Format = integerMetaField.Format;
            formatter.Styles = integerMetaField.Styles;
        }

        internal protected override string GetValueText(long value)
        {
            return formatter.ToText(value);
        }
        internal protected override long ParseValueText(string text)
        {
            return formatter.Parse(text);
        }
    }
}
