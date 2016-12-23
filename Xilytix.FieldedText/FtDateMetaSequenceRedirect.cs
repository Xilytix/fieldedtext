// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;

namespace Xilytix.FieldedText
{
    using Factory;

    public class FtDateMetaSequenceRedirect : FtMetaSequenceRedirect
    {
        public new const int Type = FtStandardSequenceRedirectType.Date;
        private readonly DateTime DefaultValue = new DateTime(0);

        private DateTime value;

        public FtDateMetaSequenceRedirect() : base(Type)
        {
            LoadDateDefaults();
        }

        public DateTime Value { get { return value; } set { this.value = value.Date; } }

        public override void LoadDefaults()
        {
            LoadDateDefaults();
        }

        private void LoadDateDefaults()
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

            FtDateMetaSequenceRedirect typedSource = source as FtDateMetaSequenceRedirect;
            Value = typedSource.Value;
        }
    }
}
