// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

namespace Xilytix.FieldedText
{
    using Factory;

    public class FtBooleanMetaSequenceRedirect: FtMetaSequenceRedirect
    {
        public new const int Type = FtStandardSequenceRedirectType.Boolean;
        private const bool DefaultValue = false;

        public FtBooleanMetaSequenceRedirect() : base(Type)
        {
            LoadBooleanDefaults();
        }

        public bool Value { get; set; }

        public override void LoadDefaults()
        {
            LoadBooleanDefaults();
        }

        private void LoadBooleanDefaults()
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

            FtBooleanMetaSequenceRedirect typedSource = source as FtBooleanMetaSequenceRedirect;
            Value = typedSource.Value;
        }
    }
}
