// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

namespace Xilytix.FieldedText
{
    using System;

    public class FtStringField: FtGenericField<string>
    {
        private const bool ValueTextNullTrimmable = false;

        private FtStringFieldDefinition definition;
        internal protected FtStringField(FtSequenceInvokation mySequenceInvokation,
                                         FtSequenceItem mySequenceItem,
                                         FtStringFieldDefinition myDefinition) : base(mySequenceInvokation, mySequenceItem, ValueTextNullTrimmable,
                                                                                      myDefinition)
        {
            definition = myDefinition;
        }

        public override void SetValue(string value, out int fieldsAffectedFromIndex)
        {
            if (value == null)
                SetNull(out fieldsAffectedFromIndex);
            else
                base.SetValue(value, out fieldsAffectedFromIndex);
        }

        public string NullableValue
        {
            get { return IsNull() ? null : Value; }
            set
            {
                if (value == null)
                    SetNull();
                else
                    Value = value;
            }
        }

        protected override bool IsValueEqual(string left, string right) { return string.Equals(left, right, StringComparison.Ordinal); }
    }
}

