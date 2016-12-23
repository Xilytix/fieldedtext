// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

namespace Xilytix.FieldedText
{
    using Factory;

    public class FtCaseInsensitiveStringMetaSequenceRedirect: FtMetaSequenceRedirect
    {
        public new const int Type = FtStandardSequenceRedirectType.CaseInsensitiveString;
        private const string DefaultValue = "";

        public FtCaseInsensitiveStringMetaSequenceRedirect() : base(Type)
        {
            LoadCaseInsensitiveStringDefaults();
        }

        public string Value { get; set; }

        public override void LoadDefaults()
        {
            LoadCaseInsensitiveStringDefaults();
        }

        private void LoadCaseInsensitiveStringDefaults()
        {
            base.LoadDefaults();
            Value = DefaultValue;
        }

        protected internal override FtMetaSequenceRedirect CreateCopy(FtMetaSequenceList sequenceList, FtMetaSequenceList sourceSequenceList)
        {
            FtMetaSequenceRedirect redirect = SequenceRedirectFactory.CreateMetaSequenceRedirect(Type);
            redirect.Assign(this, sequenceList, sourceSequenceList);
            return redirect;
        }
        protected internal override void Assign(FtMetaSequenceRedirect source, FtMetaSequenceList sequenceList, FtMetaSequenceList sourceSequenceList)
        {
            base.Assign(source, sequenceList, sourceSequenceList);

            FtCaseInsensitiveStringMetaSequenceRedirect typedSource = source as FtCaseInsensitiveStringMetaSequenceRedirect;
            Value = typedSource.Value;
        }
    }
}
