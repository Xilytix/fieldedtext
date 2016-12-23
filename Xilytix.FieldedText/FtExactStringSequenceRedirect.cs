// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;

namespace Xilytix.FieldedText
{
    public class FtExactStringSequenceRedirect: FtSequenceRedirect
    {
        public new const int Type = FtStandardSequenceRedirectType.ExactString;

        private string value;

        internal protected FtExactStringSequenceRedirect(int myIndex) : base(myIndex, Type) { }

        public string Value { get { return value; } }

        internal override bool CheckTriggered(FtField field)
        {
            if (field.IsNull())
                return false;
            else
            {
                try
                {
                    return string.Equals(field.AsRedirectString, value, StringComparison.Ordinal);
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

            FtExactStringMetaSequenceRedirect stringMetaRedirect = metaSequenceRedirect as FtExactStringMetaSequenceRedirect;
            value = stringMetaRedirect.Value;
        }
    }
}
