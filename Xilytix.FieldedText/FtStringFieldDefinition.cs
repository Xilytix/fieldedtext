// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

namespace Xilytix.FieldedText
{
    using Serialization.Formatting;

    public class FtStringFieldDefinition: FtGenericFieldDefinition<string>
    {
        public new const int DataType = FtStandardDataType.String;
        public new const bool AutoLeftPad = false;

        private StringFieldFormatter formatter;

        internal protected FtStringFieldDefinition(int myIndex) : base(myIndex, AutoLeftPad) { formatter = new StringFieldFormatter(); SetFormatter(formatter); }

        internal protected override string GetValueText(string value)
        {
            return formatter.ToText(value);
        }
        internal protected override string ParseValueText(string text)
        {
            return formatter.Parse(text);
        }
    }
}
