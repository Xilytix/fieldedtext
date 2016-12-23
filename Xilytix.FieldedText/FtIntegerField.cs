// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System.Globalization;

namespace Xilytix.FieldedText
{
    public class FtIntegerField : FtGenericField<long>
    {
        private const bool ValueTextNullTrimmable = true;

        private FtIntegerFieldDefinition definition;
        internal protected FtIntegerField(FtSequenceInvokation mySequenceInvokation,
                                          FtSequenceItem mySequenceItem,
                                          FtIntegerFieldDefinition myDefinition) : base(mySequenceInvokation, mySequenceItem, ValueTextNullTrimmable, 
                                                                                        myDefinition)
        {
            definition = myDefinition;
        }

        public string Format { get { return definition.Format; } }
        public NumberStyles Styles { get { return definition.Styles; } }

        public long? NullableValue
        {
            get { return IsNull() ? (long?)null : Value; }
            set
            {
                if (value.HasValue)
                    Value = value.Value;
                else
                    SetNull();
            }
        }

        protected override bool IsValueEqual(long left, long right) { return left == right; }
    }
}
