// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System.Globalization;

namespace Xilytix.FieldedText
{
    public class FtFloatField : FtGenericField<double>
    {
        private const bool ValueTextNullTrimmable = true;

        private FtFloatFieldDefinition definition;
        public FtFloatField(FtSequenceInvokation mySequenceInvokation,
                            FtSequenceItem mySequenceItem,
                            FtFloatFieldDefinition myDefinition) : base(mySequenceInvokation, mySequenceItem, ValueTextNullTrimmable,
                                                                        myDefinition)
        {
            definition = myDefinition;
        }

        internal protected string Format { get { return definition.Format; } }
        public NumberStyles Styles { get { return definition.Styles; } }

        public double? NullableValue
        {
            get { return IsNull() ? (double?)null : Value; }
            set
            {
                if (value.HasValue)
                    Value = value.Value;
                else
                    SetNull();
            }
        }

        protected override bool IsValueEqual(double left, double right) { return left == right; }
    }
}
