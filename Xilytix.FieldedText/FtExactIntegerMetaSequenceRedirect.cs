// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

namespace Xilytix.FieldedText
{
    using Factory;

    public class FtExactIntegerMetaSequenceRedirect: FtMetaSequenceRedirect
    {
        public new const int Type = FtStandardSequenceRedirectType.ExactInteger;
        public const int DefaultValue = 0;

        public FtExactIntegerMetaSequenceRedirect() : base(Type)
        {
            LoadExactIntegerDefaults();
        }

        public long Value { get; set; }

        public override void LoadDefaults()
        {
            LoadExactIntegerDefaults();
        }

        private void LoadExactIntegerDefaults()
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

            FtExactIntegerMetaSequenceRedirect typedSource = source as FtExactIntegerMetaSequenceRedirect;
            Value = typedSource.Value;
        }
    }
}
