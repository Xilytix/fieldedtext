// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

namespace Xilytix.FieldedText.Serialization.Formatting
{
    internal class StringFieldFormatter : FieldFormatter
    {
        internal string Parse(string text)
        {
            return text;
        }
        internal string ToText(string value)
        {
            return value;
        }
    }
}
