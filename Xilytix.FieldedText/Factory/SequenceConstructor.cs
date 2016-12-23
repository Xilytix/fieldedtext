// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using Xilytix.FieldedText.Serialization;

namespace Xilytix.FieldedText.Factory
{
    public class SequenceConstructor
    {
        protected internal virtual FtSequence CreateSequence(int index) { return new FtSequence(index); }
        protected internal virtual FtMetaSequence CreateMetaSequence() { return new FtMetaSequence(); }
    }
}
