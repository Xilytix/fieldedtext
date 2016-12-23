// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

namespace Xilytix.FieldedText
{
    using Factory;

    public class FtExactDecimalMetaSequenceRedirect: FtMetaSequenceRedirect
    {
        public new const int Type = FtStandardSequenceRedirectType.ExactDecimal;
        private const decimal DefaultValue = 0;

        public FtExactDecimalMetaSequenceRedirect() : base(Type)
        {
            LoadExactDecimalDefaults();
        }

        public decimal Value { get; set; }

        public override void LoadDefaults()
        {
            LoadExactDecimalDefaults();
        }

        private void LoadExactDecimalDefaults()
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

            FtExactDecimalMetaSequenceRedirect typedSource = source as FtExactDecimalMetaSequenceRedirect;
            Value = typedSource.Value;
        }
    }
}
