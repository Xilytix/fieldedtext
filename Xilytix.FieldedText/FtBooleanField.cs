// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

namespace Xilytix.FieldedText
{
    public class FtBooleanField : FtGenericField<bool>
    {
        private const bool ValueTextNullTrimmable = true;

        private FtBooleanFieldDefinition definition;

        internal protected FtBooleanField(FtSequenceInvokation mySequenceInvokation,
                                          FtSequenceItem mySequenceItem,
                                          FtBooleanFieldDefinition myDefinition) : base(mySequenceInvokation, mySequenceItem, ValueTextNullTrimmable,
                                                                                        myDefinition)
        {
            definition = myDefinition;
        }

        public string FalseText { get { return definition.FalseText; } }
        public string TrueText { get { return definition.TrueText; } }
        public FtBooleanStyles Styles { get { return definition.Styles; } }

        public bool? NullableValue
        {
            get { return IsNull() ? (bool?)null : Value; }
            set
            {
                if (value.HasValue)
                    Value = value.Value;
                else
                    SetNull();
            }
        }

        protected override bool IsValueEqual(bool left, bool right) { return left == right; }
    }
}
