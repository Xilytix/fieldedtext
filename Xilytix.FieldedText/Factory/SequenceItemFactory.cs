// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using Xilytix.FieldedText.Serialization;

namespace Xilytix.FieldedText.Factory
{
    public static class SequenceItemFactory
    {
        static SequenceItemFactory()
        {
            // initialise with Standard Constructor
            Constructor = new SequenceItemConstructor();
        }

        public static SequenceItemConstructor Constructor { get; set; }

        internal static FtSequenceItem CreateSequenceItem(int index) { return Constructor.CreateSequenceItem(index); }
        internal static FtMetaSequenceItem CreateMetaSequenceItem() { return Constructor.CreateMetaSequenceItem(); }
    }
}
