// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

namespace Xilytix.FieldedText
{
    using System.Globalization;

    using Serialization.Formatting;

    public class FtBooleanFieldDefinition : FtGenericFieldDefinition<bool>
    {
        public new const int DataType = FtStandardDataType.Boolean;
        public new const bool AutoLeftPad = false;

        private BooleanFieldFormatter formatter;

        internal protected FtBooleanFieldDefinition(int myIndex) : base(myIndex, AutoLeftPad) { formatter = new BooleanFieldFormatter(); SetFormatter(formatter); }

        public string FalseText { get { return formatter.FalseText; } }
        public string TrueText { get { return formatter.TrueText; } }
        public FtBooleanStyles Styles { get { return formatter.Styles; } }

        internal protected override void LoadMeta(FtMetaField metaField, CultureInfo myCulture, int myMainHeadingIndex)
        {
            base.LoadMeta(metaField, myCulture, myMainHeadingIndex);

            FtBooleanMetaField booleanMetaField = metaField as FtBooleanMetaField;
            formatter.FalseText = booleanMetaField.FalseText;
            formatter.TrueText = booleanMetaField.TrueText;
            formatter.Styles = booleanMetaField.Styles;
        }

        internal protected override string GetValueText(bool value)
        {
            return formatter.ToText(value);
        }
        internal protected override bool ParseValueText(string text)
        {
            return formatter.Parse(text);
        }
    }
}
