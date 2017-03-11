// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;

namespace Xilytix.FieldedText
{
    public abstract class FtGenericField<T> : FtField 
    {
        private FtGenericFieldDefinition<T> definition;

        private T value;
        protected new FtGenericFieldDefinition<T> Definition { get { return definition; } }

        internal protected FtGenericField(FtSequenceInvokation mySequenceInvokation,
                                          FtSequenceItem mySequenceItem,
                                          bool myValueTextNullTrimmable,
                                          FtGenericFieldDefinition<T> myDefinition) : base(mySequenceInvokation, mySequenceItem, myValueTextNullTrimmable)
        {
            definition = myDefinition;

            if (definition.Constant && !definition.Null)
            {
                value = definition.Value;
            }
        }

        private T GetValueOrThrowNull()
        {
            if (IsNull())
                throw new InvalidOperationException(string.Format(Properties.Resources.FtGenericField_GetValueOrThrowNull_AttemptToGetValueFromNullField, Name));
            else
                return value;
        }

        public T Value
        {
            get { return GetValueOrThrowNull(); }
            set
            {
                int fieldsAffectedFromIndex;
                SetValue(value, out fieldsAffectedFromIndex);
            }
        }

        public virtual void SetValue(T value, out int fieldsAffectedFromIndex)
        {
            if (Constant)
                throw new InvalidOperationException(string.Format(Properties.Resources.FtGenericField_SetValue_Constant, Name));
            else
            {
                ClearNonConstantNull();
                this.value = value;
                valueAssigned = true;
                CheckValueSequenceRedirect(out fieldsAffectedFromIndex);
            }
        }

        protected abstract bool IsValueEqual(T left, T right);

        internal protected override string GetAsNonNullValueText()
        {
            try
            {
                return definition.GetValueText(value);
            }
            catch (FtSerializationException inner)
            {
                throw new FtSerializationException(inner.Error, this, inner.Message, inner.InnerException);
            }
        }

        protected override object GetAsNonNullObject() { return (object)value; }

        protected override string GetAsNonNullString() { return (string)Convert.ChangeType(value, typeof(string)); }
        protected override bool GetAsBoolean() { return (bool)Convert.ChangeType(value, typeof(bool)); }
        protected override int GetAsInt32() { return (int)Convert.ChangeType(value, typeof(int)); }
        protected override long GetAsInt64() { return (long)Convert.ChangeType(value, typeof(long)); }
        protected override double GetAsDouble() { return (Double)Convert.ChangeType(value, typeof(Double)); }
        protected override DateTime GetAsDateTime() { return (DateTime)Convert.ChangeType(value, typeof(DateTime)); }
        protected override decimal GetAsDecimal() { return (decimal)Convert.ChangeType(value, typeof(decimal)); }

        protected override void SetAsNonNullObject(object newValue) { Value = (T)Convert.ChangeType(newValue, typeof(T)); }

        protected override void SetAsNonNullString(string newValue) { Value = (T)Convert.ChangeType(newValue, typeof(T)); }
        protected override void SetAsBoolean(bool newValue) { Value = (T)Convert.ChangeType(newValue, typeof(T)); }
        protected override void SetAsInt32(int newValue) { Value = (T)Convert.ChangeType(newValue, typeof(T)); }
        protected override void SetAsInt64(long newValue) { Value = (T)Convert.ChangeType(newValue, typeof(T)); }
        protected override void SetAsDouble(double newValue) { Value = (T)Convert.ChangeType(newValue, typeof(T)); }
        protected override void SetAsDateTime(DateTime newValue) { Value = (T)Convert.ChangeType(newValue, typeof(T)); }
        protected override void SetAsDecimal(decimal newValue) { Value = (T)Convert.ChangeType(newValue, typeof(T)); }

        protected override bool GetAsRedirectBoolean() { return (bool)Convert.ChangeType(value, typeof(bool), Culture); }
        protected override long GetAsRedirectInteger() { return (long)Convert.ChangeType(value, typeof(long), Culture); }
        protected override double GetAsRedirectFloat() { return (Double)Convert.ChangeType(value, typeof(Double), Culture); }
        protected override DateTime GetAsRedirectDateTime() { return (DateTime)Convert.ChangeType(value, typeof(DateTime), Culture); }
        protected override decimal GetAsRedirectDecimal() { return (decimal)Convert.ChangeType(value, typeof(decimal), Culture); }

        protected override void LoadNonNullValue(string valueText)
        {
            if (!Constant)
            {
                ClearNonConstantNull();
                value = Definition.ParseValueText(valueText);
                valueAssigned = true;
                int fieldsAffectedFromIndex;
                CheckValueSequenceRedirect(out fieldsAffectedFromIndex);
            }
            else
            {
                if (IsNull())
                    throw new FtSerializationException(FtSerializationError.FieldConstantValueMismatch, this, string.Format(Properties.Resources.FtGenericField_LoadNonNullValue_ConstantNullValueExpected, valueText));
                else
                {
                    T parsedValue = Definition.ParseValueText(valueText);
                    if (!IsValueEqual(value, parsedValue))
                    {
                        throw new FtSerializationException(FtSerializationError.FieldConstantValueMismatch, this, string.Format(Properties.Resources.FtGenericField_LoadNonNullValue_ConstantValueMismatch, valueText));
                    }
                }
            }
        }

        private void CheckValueSequenceRedirect(out int fieldsAffectedFromIndex)
        {
            fieldsAffectedFromIndex = -1;

            if (SequenceRedirectList.Count > 0 && !Sidelined)
            {
                bool redirected = false;
                for (int i = 0; i < SequenceRedirectList.Count; i++)
                {
                    FtSequenceRedirect redirect = SequenceRedirectList[i];
                    if (redirect.Type != FtStandardSequenceRedirectType.Null)
                    {
                        if (redirect.CheckTriggered(this))
                        {
                            OnSequenceRedirect(redirect.Sequence, redirect.InvokationDelay, out fieldsAffectedFromIndex);
                            redirected = true;
                            break;
                        }
                    }
                }

                if (!redirected)
                {
                    // in case was previously redirected
                    OnSequenceRedirect(null, IgnoredSequenceInvokationDelay, out fieldsAffectedFromIndex);
                }
            }
        }
    }
}
