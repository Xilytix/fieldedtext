// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System.Globalization;

namespace Xilytix.FieldedText
{
    using Serialization.Formatting;

    public class FtFloatFieldDefinition: FtGenericFieldDefinition<double>
    {
        public new const int DataType = FtStandardDataType.Float;
        public new const bool AutoLeftPad = true;

        private FloatFieldFormatter formatter;

        internal protected FtFloatFieldDefinition(int myIndex) : base(myIndex, AutoLeftPad) { formatter = new FloatFieldFormatter(); SetFormatter(formatter); }

        public string Format { get { return formatter.Format; } }
        public NumberStyles Styles { get { return formatter.Styles; } }

        internal protected override void LoadMeta(FtMetaField metaField, CultureInfo myCulture, int myMainHeadingIndex)
        {
            base.LoadMeta(metaField, myCulture, myMainHeadingIndex);

            FtFloatMetaField floatMetaField = metaField as FtFloatMetaField;
            formatter.Format = floatMetaField.Format;
            formatter.Styles = floatMetaField.Styles;
        }

        internal protected override string GetValueText(double value)
        {
            return formatter.ToText(value);
        }
        internal protected override double ParseValueText(string text)
        {
            return formatter.Parse(text);
        }
    }
}
