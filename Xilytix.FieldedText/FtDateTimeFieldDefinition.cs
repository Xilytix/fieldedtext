// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;
using System.Globalization;

namespace Xilytix.FieldedText
{
    using Serialization.Formatting;

    public class FtDateTimeFieldDefinition: FtGenericFieldDefinition<DateTime>
    {
        public new const int DataType = FtStandardDataType.DateTime;
        public new const bool AutoLeftPad = true;

        private DateTimeFieldFormatter formatter;

        private DateTime constantValue;

        internal protected FtDateTimeFieldDefinition(int myIndex) : base(myIndex, AutoLeftPad) { formatter = new DateTimeFieldFormatter(); SetFormatter(formatter); }

        public string Format { get { return formatter.Format; } }
        public DateTimeStyles Styles { get { return formatter.Styles; } }
        public DateTime ConstantValue { get { return constantValue; } }

        internal protected override void LoadMeta(FtMetaField metaField, CultureInfo myCulture, int myMainHeadingIndex)
        {
            base.LoadMeta(metaField, myCulture, myMainHeadingIndex);

            FtDateTimeMetaField dateTimeMetaField = metaField as FtDateTimeMetaField;
            formatter.Format = dateTimeMetaField.Format;
            formatter.Styles = dateTimeMetaField.Styles;
            constantValue = dateTimeMetaField.Value;
        }

        internal protected override string GetValueText(DateTime value)
        {
            return formatter.ToText(value);
        }
        internal protected override DateTime ParseValueText(string text)
        {
            return formatter.Parse(text);
        }
    }
}
