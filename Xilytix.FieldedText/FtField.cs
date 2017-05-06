// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;
using System.Globalization;

namespace Xilytix.FieldedText
{
    public abstract class FtField
    {
        protected const FtSequenceInvokationDelay IgnoredSequenceInvokationDelay = FtSequenceInvokationDelay.ftikAfterSequence;
        public const int NoFieldsAffectedIndex = -1;

        internal delegate void SequenceRedirectDelegate(FtField field, FtSequence sequence, FtSequenceInvokationDelay delay, 
                                                        out int fieldsAffectedFromIndex);
        internal SequenceRedirectDelegate SequenceRedirectEvent;

        private int index;

        private FtFieldDefinition definition;
        private FtSequence sequence;
        private FtSequenceItem sequenceItem;
        private FtSequenceRedirectList sequenceRedirectList;
        private FtSequenceInvokation sequenceInvokation;
        private bool sidelined;

        private string name;
        private string[] headings;
        private bool valueTextNullTrimmable;

        private bool valueIsNull;

        private long loadedPosition;
        private int loadedLength;
        private int loadedRawOffset;
        private int loadedRawLength;
        private string loadedValueText;

        protected bool valueAssigned;

        protected bool quoted;

        public FtField(FtSequenceInvokation mySequenceInvokation, FtSequenceItem mySequenceItem, bool myValueTextNullTrimmable)
        {
            definition = mySequenceItem.FieldDefinition;
            sequence = mySequenceInvokation.Sequence;
            sequenceItem = mySequenceItem;
            sequenceRedirectList = mySequenceItem.RedirectList;
            sequenceInvokation = mySequenceInvokation;
            valueTextNullTrimmable = myValueTextNullTrimmable;

            name = definition.MetaName;
            headings = new string[definition.MetaHeadingCount];
            Array.Copy(definition.MetaHeadings, headings, definition.MetaHeadingCount);

            if (definition.Constant)
                valueIsNull = definition.Null;
            else
                valueIsNull = true;
        }

        public int Index { get { return index; } internal set { index = value; } }
        public bool Sidelined { get { return sidelined; } internal set { sidelined = value; } }

        public FtFieldDefinition Definition { get { return definition; } }
        public FtSequence Sequence { get { return sequence; } }
        public FtSequenceItem SequenceItem { get { return sequenceItem; } }
        public FtSequenceRedirectList SequenceRedirectList { get { return sequenceRedirectList; } }
        public FtSequenceInvokation SequenceInvokation { get { return sequenceInvokation; } }

        public int HeadingCount { get { return (headings == null) ? 0 : headings.Length; } }
        public string[] Headings { get { return headings; } }

        public bool ValueAssigned { get { return valueAssigned; } }
        public string LoadedValueText { get { return loadedValueText; } }

        public bool IsNull() { return valueIsNull; }

        public void SetNull()
        {
            int fieldsAffectedFromIndex;
            SetNull(out fieldsAffectedFromIndex);
        }

        public void SetNull(out int fieldsAffectedFromIndex)
        {
            if (Constant)
                throw new InvalidOperationException(string.Format(Properties.Resources.FtField_SetNull_Constant, Name));
            else
            {
                valueIsNull = true;
                valueAssigned = true;

                if (!valueIsNull)
                    fieldsAffectedFromIndex = -1;
                else
                    CheckNullSequenceRedirect(out fieldsAffectedFromIndex);
            }
        }

        protected void ClearNonConstantNull()
        {
            valueIsNull = false;
        }

        internal void CheckNullSequenceRedirect(out int fieldsAffectedFromIndex)
        {
            fieldsAffectedFromIndex = -1;

            if (SequenceRedirectList.Count > 0 && !Sidelined)
            {
                bool redirected = false;
                for (int i = 0; i < SequenceRedirectList.Count; i++)
                {
                    FtSequenceRedirect redirect = SequenceRedirectList[i];
                    if (redirect.Type == FtStandardSequenceRedirectType.Null)
                    {
                        OnSequenceRedirect(redirect.Sequence, redirect.InvokationDelay, out fieldsAffectedFromIndex);
                        redirected = true;
                        break;
                    }
                }

                if (!redirected)
                {
                    // in case was previously redirected
                    OnSequenceRedirect(null, IgnoredSequenceInvokationDelay, out fieldsAffectedFromIndex);
                }
            }
        }

        internal void ResetValue()
        {
            if (!Constant)
            {
                valueIsNull = true;
            }

            valueAssigned = false;
        }

        public int DataType { get { return definition.DataType; } }
        public int Id { get { return definition.Id; } }
        public string Name { get { return definition.MetaName; } }
        public string[] MetaHeadings { get { return definition.MetaHeadings; } }
        public int MainHeadingIndex { get { return definition.MainHeadingIndex; } }
        public CultureInfo Culture { get { return definition.Culture; } }
        public bool FixedWidth { get { return definition.FixedWidth; } }
        public int Width { get { return definition.Width; } }
        public bool Constant { get { return definition.Constant; } }
        public FtQuotedType ValueQuotedType { get { return definition.ValueQuotedType; } }
        public bool ValueAlwaysWriteOptionalQuote { get { return definition.ValueAlwaysWriteOptionalQuote; } }
        public bool ValueWritePrefixSpace { get { return definition.ValueWritePrefixSpace; } }
        public FtPadAlignment ValuePadAlignment { get { return definition.ValuePadAlignment; } }
        public FtPadCharType ValuePadCharType { get { return definition.ValuePadCharType; } }
        public char ValuePadChar { get { return definition.ValuePadChar; } }
        public FtTruncateType ValueTruncateType { get { return definition.ValueTruncateType; } }
        public char ValueTruncateChar { get { return definition.ValueTruncateChar; } }
        public char ValueEndOfValueChar { get { return definition.ValueEndOfValueChar; } }
        public char ValueNullChar { get { return definition.ValueNullChar; } }
        public FtHeadingConstraint HeadingConstraint { get { return definition.HeadingConstraint; } }
        public FtQuotedType HeadingQuotedType { get { return definition.HeadingQuotedType; } }
        public bool HeadingAlwaysWriteOptionalQuote { get { return definition.HeadingAlwaysWriteOptionalQuote; } }
        public bool HeadingWritePrefixSpace { get { return definition.HeadingWritePrefixSpace; } }
        public FtPadAlignment HeadingPadAlignment { get { return definition.HeadingPadAlignment; } }
        public FtPadCharType HeadingPadCharType { get { return definition.HeadingPadCharType; } }
        public char HeadingPadChar { get { return definition.HeadingPadChar; } }
        public FtTruncateType HeadingTruncateType { get { return definition.HeadingTruncateType; } }
        public char HeadingTruncateChar { get { return definition.HeadingTruncateChar; } }
        public char HeadingEndOfValueChar { get { return definition.HeadingEndOfValueChar; } }

        public Type ValueType { get { return definition.ValueType; } }
        public string DataTypeName { get { return definition.DataTypeName; } }

        internal protected abstract string GetAsNonNullValueText();
        protected abstract void LoadNonNullValue(string valueText);

        protected abstract object GetAsNonNullObject();
        protected abstract string GetAsNonNullString();
        protected abstract bool GetAsBoolean();
        protected abstract int GetAsInt32();
        protected abstract long GetAsInt64();
        protected abstract double GetAsDouble();
        protected abstract DateTime GetAsDateTime();
        protected abstract decimal GetAsDecimal();

        protected abstract void SetAsNonNullObject(object newValue);
        protected abstract void SetAsNonNullString(string newValue);
        protected abstract void SetAsBoolean(bool newValue);
        protected abstract void SetAsInt32(int newValue);
        protected abstract void SetAsInt64(long newValue);
        protected abstract void SetAsDouble(double newValue);
        protected abstract void SetAsDateTime(DateTime newValue);
        protected abstract void SetAsDecimal(decimal newValue);

        protected abstract bool GetAsRedirectBoolean();
        protected abstract long GetAsRedirectInteger();
        protected abstract double GetAsRedirectFloat();
        protected abstract DateTime GetAsRedirectDateTime();
        protected abstract decimal GetAsRedirectDecimal();

        protected void OnSequenceRedirect(FtSequence sequence, FtSequenceInvokationDelay delay, out int fieldsAffectedFromIndex)
        {
            SequenceRedirectEvent(this, sequence, delay, out fieldsAffectedFromIndex);
        }

        private void SetAsNonNullValueText(string newValue)
        {
            try
            {
                LoadNonNullValue(newValue);
            }
            catch (FtSerializationException E)
            {
                throw new FtSerializationException(E.Error, this, string.Format(Properties.Resources.FtField_SetAsNonNullValueText_Prefix, E.Message), E);
            }
        }

        private void SetAsValueText(string newValue)
        {
            if (newValue == null)
                SetNull();
            else
                SetAsNonNullValueText(newValue);
        }

        private void SetAsObject(object newValue)
        {
            if (newValue == null)
                SetNull();
            else
                SetAsNonNullObject(newValue);
        }

        private void SetAsString(string newValue)
        {
            if (newValue == null)
                SetNull();
            else
                SetAsNonNullString(newValue);
        }

        public string AsValueText { get { return IsNull() ? null : GetAsNonNullValueText(); } set { SetAsValueText(value); } }

        public object AsObject { get { return IsNull() ? null : GetAsNonNullObject(); } set { SetAsObject(value); } }
        public string AsString { get { return IsNull() ? null : GetAsNonNullString(); } set { SetAsString(value); } }
        public bool AsBoolean { get { return GetAsBoolean(); } set { SetAsBoolean(value); } }
        public int AsInt32 { get { return GetAsInt32(); } set { SetAsInt32(value); } }
        public long AsInt64 { get { return GetAsInt64(); } set { SetAsInt64(value); } }
        public double AsDouble { get { return GetAsDouble(); } set { SetAsDouble(value); } }
        public DateTime AsDateTime { get { return GetAsDateTime(); } set { SetAsDateTime(value); } }
        public decimal AsDecimal { get { return GetAsDecimal(); } set { SetAsDecimal(value); } }

        public bool? AsNullableBoolean
        {
            get { return IsNull() ? null : (bool?)AsBoolean; }
            set
            {
                if (value.HasValue)
                    SetAsBoolean(value.Value);
                else
                    SetNull();
            }
        }
        public int? AsNullableInt32
        {
            get { return IsNull() ? null : (int?)AsInt32; }
            set
            {
                if (value.HasValue)
                    SetAsInt32(value.Value);
                else
                    SetNull();
            }
        }
        public long? AsNullableInt64
        {
            get { return IsNull() ? null : (long?)AsInt64; }
            set
            {
                if (value.HasValue)
                    SetAsInt64(value.Value);
                else
                    SetNull();
            }
        }
        public double? AsNullableDouble
        {
            get { return IsNull() ? null : (double?)AsDouble; }
            set
            {
                if (value.HasValue)
                    SetAsDouble(value.Value);
                else
                    SetNull();
            }
        }
        public DateTime? AsNullableDateTime
        {
            get { return IsNull() ? null : (DateTime?)AsDateTime; }
            set
            {
                if (value.HasValue)
                    SetAsDateTime(value.Value);
                else
                    SetNull();
            }
        }
        public decimal? AsNullableDecimal
        {
            get { return IsNull() ? null : (decimal?)AsDecimal; }
            set
            {
                if (value.HasValue)
                    SetAsDecimal(value.Value);
                else
                    SetNull();
            }
        }

        internal string AsRedirectString { get { return GetAsNonNullValueText(); } }
        internal bool AsRedirectBoolean { get { return GetAsRedirectBoolean(); } }
        internal long AsRedirectInteger { get { return GetAsRedirectInteger(); } }
        internal double AsRedirectFloat { get { return GetAsRedirectFloat(); } }
        internal DateTime AsRedirectDateTime { get { return GetAsRedirectDateTime(); } }
        internal decimal AsRedirectDecimal { get { return GetAsRedirectDecimal(); } }

        internal void LoadHeading(int idx, 
                                  string headingText)
        {
            switch (definition.HeadingConstraint)
            {
                case FtHeadingConstraint.None:
                    headings[idx] = headingText;
                    break;
                case FtHeadingConstraint.AllConstant:
                    if (headingText != headings[idx])
                    {
                        throw new FtSerializationException(FtSerializationError.FieldConstNameHeadingMismatch, this, string.Format(Properties.Resources.FtField_LoadHeading_AllConstantViolation, idx));
                    }
                    break;
                case FtHeadingConstraint.MainConstant:
                    if (idx != definition.MainHeadingIndex)
                        headings[idx] = headingText;
                    else
                    {
                        if (headingText != headings[idx])
                        {
                            throw new FtSerializationException(FtSerializationError.FieldConstNameHeadingMismatch, this, Properties.Resources.FtField_LoadHeading_MainConstantViolation);
                        }
                    }
                    break;
                case FtHeadingConstraint.NameConstant:
                    if (idx != definition.MainHeadingIndex)
                        headings[idx] = headingText;
                    else
                    {
                        if (string.Equals(headingText, definition.MetaName, StringComparison.OrdinalIgnoreCase))
                            headings[idx] = headingText;
                        else
                            throw new FtSerializationException(FtSerializationError.FieldConstNameHeadingMismatch, this, Properties.Resources.FtField_LoadHeading_NameConstantViolation);
                    }
                    break;
                case FtHeadingConstraint.NameIsMain:
                    headings[idx] = headingText;
                    if (idx == definition.MainHeadingIndex)
                    {
                        name = headingText;
                    }
                    break;
                default:
                    throw FtInternalException.Create(InternalError.FtField_ParseHeadingText_UnsupportedHeadingConstraint, definition.HeadingConstraint.ToString());
            }
        }

        public void LoadValue(string valueText, bool quoted)
        {
            if (FixedWidth)
                LoadFixedWidthValue(valueText);
            else
                LoadDelimitedValue(valueText, quoted);
        }

        internal void LoadFixedWidthValue(string valueText)
        {
            loadedValueText = valueText;
            if (string.Equals(valueText, definition.FixedWidthNullValueText, StringComparison.Ordinal))
                LoadNullValue();
            else
            {
                try
                {
                    LoadNonNullValue(valueText);
                }
                catch (FtSerializationException E)
                {
                    throw new FtSerializationException(E.Error, this, string.Format(Properties.Resources.FtField_LoadFixedWidthValue_Prefix, E.Message), E);
                }
            }
        }

        internal void LoadDelimitedValue(string valueText, bool quoted)
        {
            loadedValueText = valueText;

            try
            {
                if (quoted)
                    LoadNonNullValue(valueText);
                else
                {
                    if (valueTextNullTrimmable)
                    {
                        valueText = valueText.Trim();
                        if (valueText.Length == 0)
                            LoadNullValue();
                        else
                            LoadNonNullValue(valueText);
                    }
                    else
                    {
                        if (valueText.Length != 0)
                            LoadNonNullValue(valueText);
                        else
                        {
                            switch (ValueQuotedType)
                            {
                                case FtQuotedType.Never:
                                case FtQuotedType.Optional:
                                    LoadNonNullValue(valueText);
                                    break;
                                case FtQuotedType.Always:
                                    LoadNullValue();
                                    break;
                                default:
                                    throw FtInternalException.Create(InternalError.FtField_LoadDelimitedValue_UnsupportedValueQuotedType, ValueQuotedType.ToString());
                            }
                        }
                    }
                }
            }
            catch (FtSerializationException E)
            {
                throw new FtSerializationException(E.Error, this, string.Format(Properties.Resources.FtField_LoadDelimitedValue_Prefix, E.Message), E);
            }
        }

        internal void LoadNullValue()
        {
            if (!Constant)
            {
                valueIsNull = true;
                valueAssigned = true;
                int fieldsAffectedFromIndex;
                CheckNullSequenceRedirect(out fieldsAffectedFromIndex);
            }
            else
            {
                if (!valueIsNull)
                {
                    throw new FtSerializationException(FtSerializationError.FieldConstantValueMismatch, this, Properties.Resources.FtField_LoadNullValue_ConstantNonNullValueExpected);
                }
            }
        }

        internal void LoadPosition(long position, int length, int rawOffset, int rawLength)
        {
            loadedPosition = position;
            loadedLength = length;
            loadedRawOffset = rawOffset;
            loadedRawLength = rawLength;
        }
    }
}
