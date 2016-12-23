// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

namespace Xilytix.FieldedText.Factory
{
    public static class MetaFactory
    {
        static MetaFactory()
        {
            // initialise with Standard Constructor
            Constructor = new MetaConstructor();
        }

        public static MetaConstructor Constructor { get; set; }

        internal static FtMeta CreateMeta() { return Constructor.CreateMeta(); }
    }
}
