// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using Xilytix.FieldedText.Serialization;

namespace Xilytix.FieldedText.Factory
{
    public class SubstitutionConstructor
    {
        protected internal virtual FtSubstitution CreateSubstitution(int index) { return new FtSubstitution(index); }
        protected internal virtual FtMetaSubstitution CreateMetaSubstitution() { return new FtMetaSubstitution(); }
    }
}
