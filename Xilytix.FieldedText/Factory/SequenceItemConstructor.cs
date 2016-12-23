// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using Xilytix.FieldedText.Serialization;

namespace Xilytix.FieldedText.Factory
{
    public class SequenceItemConstructor
    {
        protected internal virtual FtSequenceItem CreateSequenceItem(int index) { return new FtSequenceItem(index); }
        protected internal virtual FtMetaSequenceItem CreateMetaSequenceItem() { return new FtMetaSequenceItem(); }
    }
}
