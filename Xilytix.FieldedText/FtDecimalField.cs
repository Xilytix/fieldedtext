// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System.Globalization;

namespace Xilytix.FieldedText
{
    public class FtDecimalField : FtGenericField<decimal>
    {
        private const bool ValueTextNullTrimmable = true;

        private FtDecimalFieldDefinition definition;
        internal protected FtDecimalField(FtSequenceInvokation mySequenceInvokation,
                                          FtSequenceItem mySequenceItem,
                                          FtDecimalFieldDefinition myDefinition) : base(mySequenceInvokation, mySequenceItem, ValueTextNullTrimmable,
                                                                                        myDefinition)
        {
            definition = myDefinition;
        }

        public string Format { get { return definition.Format; } }
        public NumberStyles Styles { get { return definition.Styles; } }

        public decimal? NullableValue
        {
            get { return IsNull() ? (decimal?)null : Value; }
            set
            {
                if (value.HasValue)
                    Value = value.Value;
                else
                    SetNull();
            }
        }

        protected override bool IsValueEqual(decimal left, decimal right) { return left == right; }
    }
}
