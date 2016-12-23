// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;
using System.Globalization;

namespace Xilytix.FieldedText
{
    public class FtDateTimeField : FtGenericField<DateTime>
    {
        private const bool ValueTextNullTrimmable = true;

        private FtDateTimeFieldDefinition definition;
        internal protected FtDateTimeField(FtSequenceInvokation mySequenceInvokation,
                                           FtSequenceItem mySequenceItem,
                                           FtDateTimeFieldDefinition myDefinition) : base(mySequenceInvokation, mySequenceItem, ValueTextNullTrimmable,
                                                                                          myDefinition)
        {
            definition = myDefinition;
        }

        public string Format { get { return definition.Format; } }
        public DateTimeStyles Styles { get { return definition.Styles; } }

        public DateTime? NullableValue
        {
            get { return IsNull() ? (DateTime?)null : Value; }
            set
            {
                if (value.HasValue)
                    Value = value.Value;
                else
                    SetNull();
            }
        }

        protected override bool IsValueEqual(DateTime left, DateTime right) { return left == right; }
    }
}
