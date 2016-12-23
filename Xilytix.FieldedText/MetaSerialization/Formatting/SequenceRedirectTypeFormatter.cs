// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using Xilytix.FieldedText.Factory;

namespace Xilytix.FieldedText.MetaSerialization.Formatting
{
    internal static class SequenceRedirectTypeFormatter
    {
        internal static string ToAttributeValue(int type) { return SequenceRedirectFactory.GetName(type); }
        internal static bool TryParseAttributeValue(string attributeValue, out int type)
        {
            return SequenceRedirectFactory.TryGetType(attributeValue, out type);
        }
    }
}
