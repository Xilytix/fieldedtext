// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using Xilytix.FieldedText.Factory;

namespace Xilytix.FieldedText.MetaSerialization.Formatting
{
    internal static class DataTypeFormatter
    {
        internal static string ToAttributeValue(int dataType) { return FieldFactory.GetName(dataType); }
        internal static bool TryParseAttributeValue(string attributeValue, out int dataType)
        {
            return FieldFactory.TryGetDataType(attributeValue, out dataType);
        }
    }
}
