// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System.Globalization;

namespace Xilytix.FieldedText.Serialization.Formatting
{
    internal class NumberFieldFormatter : FieldFormatter
    {
        internal NumberStyles Styles { get; set; }
        internal string Format { get; set; }
    }
}
