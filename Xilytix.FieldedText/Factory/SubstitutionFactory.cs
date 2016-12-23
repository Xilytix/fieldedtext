// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using Xilytix.FieldedText.Serialization;

namespace Xilytix.FieldedText.Factory
{
    public static class SubstitutionFactory
    {
        static SubstitutionFactory()
        {
            // initialise with Standard Constructor
            Constructor = new SubstitutionConstructor();
        }

        public static SubstitutionConstructor Constructor { get; set; }

        internal static FtSubstitution CreateSubstitution(int index) { return Constructor.CreateSubstitution(index); }
        internal static FtMetaSubstitution CreateMetaSubstitution() { return Constructor.CreateMetaSubstitution(); }
    }
}
