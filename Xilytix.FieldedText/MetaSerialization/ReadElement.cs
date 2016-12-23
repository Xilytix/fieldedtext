// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

namespace Xilytix.FieldedText.MetaSerialization
{
    public abstract class ReadElement
    {
        internal protected static FieldedTextReadElement CreateFieldedTextXmlReadElement() { return new FieldedTextReadElement(); }
        internal protected abstract bool TryCreate(MetaElementType elementType, out ReadElement element);
        internal protected abstract bool TryAddOrIgnoreAttribute(string name, string value, out string errorDescription);
    }
}
