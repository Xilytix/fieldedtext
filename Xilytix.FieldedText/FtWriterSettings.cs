// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

namespace Xilytix.FieldedText
{
    public class FtWriterSettings
    {
        public const bool DefaultDeclared = false;
        public const FtMetaReferenceType DefaultMetaReferenceType = FtMetaReferenceType.None;
        public const string DefaultMetaReference = "";
        public const bool DefaultEmbeddedMetaIndent = true;
        public const string DefaultEmbeddedMetaIndentChars = "  ";

        public FtWriterSettings ()
        {
            Declared = DefaultDeclared;
            MetaReferenceType = DefaultMetaReferenceType;
            MetaReference = DefaultMetaReference;
            EmbeddedMetaIndent = DefaultEmbeddedMetaIndent;
            EmbeddedMetaIndentChars = DefaultEmbeddedMetaIndentChars;
        }

        public bool Declared { get; set; }
        public FtMetaReferenceType MetaReferenceType { get; set; }
        public string MetaReference { get; set; }
        public bool EmbeddedMetaIndent { get; set; }
        public string EmbeddedMetaIndentChars { get; set; }
        public bool EmbeddedMetaNewLineOnAttributes { get; set; }
    }
}
