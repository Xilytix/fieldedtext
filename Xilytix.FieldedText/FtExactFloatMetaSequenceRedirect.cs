// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

namespace Xilytix.FieldedText
{
    using Factory;

    public class FtExactFloatMetaSequenceRedirect: FtMetaSequenceRedirect
    {
        public new const int Type = FtStandardSequenceRedirectType.ExactFloat;
        public const double DefaultValue = 0;

        public FtExactFloatMetaSequenceRedirect() : base(Type)
        {
            LoadExactFloatDefaults();
        }

        public double Value { get; set; }

        public override void LoadDefaults()
        {
            LoadExactFloatDefaults();
        }

        private void LoadExactFloatDefaults()
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

            FtExactFloatMetaSequenceRedirect typedSource = source as FtExactFloatMetaSequenceRedirect;
            Value = typedSource.Value;
        }
    }
}
