// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;

namespace Xilytix.FieldedText
{
    using Factory;

    public class FtExactDateTimeMetaSequenceRedirect: FtMetaSequenceRedirect
    {
        public new const int Type = FtStandardSequenceRedirectType.ExactDateTime;
        private readonly DateTime DefaultValue = new DateTime(0);

        public FtExactDateTimeMetaSequenceRedirect() : base(Type)
        {
            LoadExactDateTimeDefaults();
        }

        public DateTime Value { get; set; }

        public override void LoadDefaults()
        {
            LoadExactDateTimeDefaults();
        }

        private void LoadExactDateTimeDefaults()
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

            FtExactDateTimeMetaSequenceRedirect typedSource = source as FtExactDateTimeMetaSequenceRedirect;
            Value = typedSource.Value;
        }
    }
}
