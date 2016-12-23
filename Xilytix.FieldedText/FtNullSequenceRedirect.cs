// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

namespace Xilytix.FieldedText
{
    class FtNullSequenceRedirect : FtSequenceRedirect
    {
        public new const int Type = FtStandardSequenceRedirectType.Null;

        internal protected FtNullSequenceRedirect(int myIndex) : base(myIndex, Type) { }

        internal override bool CheckTriggered(FtField field)
        {
            return field.IsNull();
        }

        internal protected override void LoadMeta(FtMetaSequenceRedirect metaSequenceRedirect, FtMetaSequenceList metaSequenceList, FtSequenceList sequenceList)
        {
            base.LoadMeta(metaSequenceRedirect, metaSequenceList, sequenceList);
        }
    }
}
