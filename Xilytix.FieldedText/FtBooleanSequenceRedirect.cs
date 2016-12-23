// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;

namespace Xilytix.FieldedText
{
    public class FtBooleanSequenceRedirect: FtSequenceRedirect
    {
        public new const int Type = FtStandardSequenceRedirectType.Boolean;

        private bool value;

        internal protected FtBooleanSequenceRedirect(int myIndex) : base(myIndex, Type) { }

        public bool Value { get { return value; } }

        internal override bool CheckTriggered(FtField field)
        {
            if (field.IsNull())
                return false;
            else
            {
                try
                {
                    return field.AsRedirectBoolean == value;
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

            FtBooleanMetaSequenceRedirect booleanMetaRedirect = metaSequenceRedirect as FtBooleanMetaSequenceRedirect;
            value = booleanMetaRedirect.Value;
        }
    }
}
