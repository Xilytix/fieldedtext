// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;

namespace Xilytix.FieldedText
{
    class FtExactDateTimeSequenceRedirect: FtSequenceRedirect
    {
        public new const int Type = FtStandardSequenceRedirectType.ExactDateTime;

        private DateTime value;

        internal protected FtExactDateTimeSequenceRedirect(int myIndex) : base(myIndex, Type) { }

        public DateTime Value { get { return value; } }

        internal override bool CheckTriggered(FtField field)
        {
            if (field.IsNull())
                return false;
            else
            {
                try
                {
                    return field.AsRedirectDateTime == value;
                }
                catch (InvalidCastException) { return false; }
                catch (FormatException) { return false; }
                catch (OverflowException) { return false; }
                catch (ArgumentNullException) { return false; }
            }

        }

        internal protected override void LoadMeta(FtMetaSequenceRedirect metaSequenceRedirect, FtMetaSequenceList metaSequenceList, FtSequenceList sequenceList)
        {
            base.LoadMeta(metaSequenceRedirect, metaSequenceList, sequenceList);

            FtExactDateTimeMetaSequenceRedirect dateTimeMetaRedirect = metaSequenceRedirect as FtExactDateTimeMetaSequenceRedirect;
            value = dateTimeMetaRedirect.Value;
        }
    }
}
