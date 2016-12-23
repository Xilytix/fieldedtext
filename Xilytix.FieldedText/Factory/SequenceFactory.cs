// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using Xilytix.FieldedText.Serialization;

namespace Xilytix.FieldedText.Factory
{
    public static class SequenceFactory
    {
        static SequenceFactory()
        {
            // initialise with Standard Constructor
            Constructor = new SequenceConstructor();
        }

        public static SequenceConstructor Constructor { get; set; }

        internal static FtSequence CreateSequence(int index) { return Constructor.CreateSequence(index); }
        internal static FtMetaSequence CreateMetaSequence() { return Constructor.CreateMetaSequence(); }
    }
}
