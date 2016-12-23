// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System.Text;

namespace Xilytix.FieldedText.Serialization
{
    internal abstract class FieldParser
    {
        private bool headings; // specifies whether parsing headings (true) or records (false)

        private SerializationCore core;
        private StringBuilder valueTextBuilder;
        private FtField field;

        protected bool Headings { get { return headings; } }
        protected SerializationCore Core { get { return core; } }
        protected StringBuilder ValueTextBuilder { get { return valueTextBuilder; } }

        internal FieldParser(bool forHeadings, SerializationCore myCore)
        {
            headings = forHeadings;
            core = myCore;
            valueTextBuilder = new StringBuilder(20);
        }

        internal FtField Field { get { return field; } }
    }
}
