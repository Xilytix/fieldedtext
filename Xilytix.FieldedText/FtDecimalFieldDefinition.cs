// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System.Globalization;
using Xilytix.FieldedText.Serialization.Formatting;

namespace Xilytix.FieldedText
{
    public class FtDecimalFieldDefinition: FtGenericFieldDefinition<decimal>
    {
        public new const int DataType = FtStandardDataType.Decimal;
        public new const bool AutoLeftPad = true;

        private DecimalFieldFormatter formatter;

        internal protected FtDecimalFieldDefinition(int myIndex) : base(myIndex, AutoLeftPad) { formatter = new DecimalFieldFormatter(); SetFormatter(formatter); }

        public string Format { get { return formatter.Format; } }
        public NumberStyles Styles { get { return formatter.Styles; } }

        internal protected override void LoadMeta(FtMetaField metaField, CultureInfo myCulture, int myMainHeadingIndex)
        {
            base.LoadMeta(metaField, myCulture, myMainHeadingIndex);

            FtDecimalMetaField decimalMetaField = metaField as FtDecimalMetaField;
            formatter.Format = decimalMetaField.Format;
            formatter.Styles = decimalMetaField.Styles;
        }

        internal protected override string GetValueText(decimal value)
        {
            return formatter.ToText(value);
        }
        internal protected override decimal ParseValueText(string text)
        {
            return formatter.Parse(text);
        }
    }
}
