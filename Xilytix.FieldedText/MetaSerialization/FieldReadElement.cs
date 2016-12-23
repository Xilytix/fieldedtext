// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;

using System.Collections.Generic;
using System.Globalization;

namespace Xilytix.FieldedText.MetaSerialization
{
    using Formatting;

    using AttributeDictionary = Dictionary<FtMetaField.PropertyId, string>;

    public class FieldReadElement: ReadElement
    {
        private AttributeDictionary attributeDictionary;
        private int explicitIndex;
        private int dataType;

        internal protected int ExplicitIndex { get { return explicitIndex; } }
        internal protected int DataType { get { return dataType; } }

        internal protected FieldReadElement(): base()
        {
            attributeDictionary = new AttributeDictionary();
            explicitIndex = -1;
            dataType = FtMetaField.DefaultDataType;
        }

        internal protected override bool TryCreate(MetaElementType elementType, out ReadElement element)
        {
            element = null;
            return false;
        }
        internal protected override bool TryAddOrIgnoreAttribute(string name, string value, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;

            FtMetaField.PropertyId id;
            if (FieldPropertyIdFormatter.TryParseAttributeName(name, out id))
            {
                switch (id)
                {
                    case FtMetaField.PropertyId.Index:
                        int parsedIndex;
                        if (FtStandardText.TryParse(value, out parsedIndex))
                            explicitIndex = parsedIndex;
                        else
                        {
                            errorDescription = Properties.Resources.FieldReadElement_TryAddOrIgnoreAttribute_InvalidFieldIndex;
                            result = false;
                        }
                        break;

                    case FtMetaField.PropertyId.DataType:
                        int parsedDataType;
                        if (DataTypeFormatter.TryParseAttributeValue(value, out parsedDataType))
                            dataType = parsedDataType;
                        else
                        {
                            errorDescription = Properties.Resources.FieldReadElement_TryAddOrIgnoreAttribute_InvalidFieldDataType;
                            result = false;
                        }
                        break;

                    default:
                        attributeDictionary.Add(id, value);
                        break;
                }
            }

            return result;
        }

        internal protected virtual bool ResolveTo(FtMetaField field, out string errorDescription)
        {
            return ResolveProperties(field, out errorDescription);
        }
        protected virtual bool ResolveProperties(FtMetaField field, out string errorDescription)
        {
            errorDescription = "";
            bool result = false;
            foreach (FtMetaField.PropertyId id in Enum.GetValues(typeof(FtMetaField.PropertyId)))
            {
                switch (id)
                {
                    case FtMetaField.PropertyId.Index:
                    case FtMetaField.PropertyId.DataType:
                        // already done in TryAddOrIgnoreAttribute
                        result = true;
                        break;
                    case FtMetaField.PropertyId.Id:
                        result = ResolveId(field, out errorDescription);
                        break;
                    case FtMetaField.PropertyId.Name:
                        result = ResolveName(field, out errorDescription);
                        break;
                    case FtMetaField.PropertyId.FixedWidth:
                        result = ResolveFixedWidth(field, out errorDescription);
                        break;
                    case FtMetaField.PropertyId.Width:
                        result = ResolveWidth(field, out errorDescription);
                        break;
                    case FtMetaField.PropertyId.Constant:
                        result = ResolveConstant(field, out errorDescription);
                        break;
                    case FtMetaField.PropertyId.Null:
                        result = ResolveNull(field, out errorDescription);
                        break;
                    case FtMetaField.PropertyId.Headings:
                        result = ResolveHeadings(field, out errorDescription);
                        break;
                    case FtMetaField.PropertyId.HeadingConstraint:
                        result = ResolveHeadingConstraint(field, out errorDescription);
                        break;
                    case FtMetaField.PropertyId.ValueQuotedType:
                        result = ResolveValueQuotedType(field, out errorDescription);
                        break;
                    case FtMetaField.PropertyId.ValueAlwaysWriteOptionalQuote:
                        result = ResolveValueAlwaysWriteOptionalQuote(field, out errorDescription);
                        break;
                    case FtMetaField.PropertyId.ValueWritePrefixSpace:
                        result = ResolveValueWritePrefixSpace(field, out errorDescription);
                        break;
                    case FtMetaField.PropertyId.ValuePadAlignment:
                        result = ResolveValuePadAlignment(field, out errorDescription);
                        break;
                    case FtMetaField.PropertyId.ValuePadCharType:
                        result = ResolveValuePadCharType(field, out errorDescription);
                        break;
                    case FtMetaField.PropertyId.ValuePadChar:
                        result = ResolveValuePadChar(field, out errorDescription);
                        break;
                    case FtMetaField.PropertyId.ValueTruncateType:
                        result = ResolveValueTruncateType(field, out errorDescription);
                        break;
                    case FtMetaField.PropertyId.ValueTruncateChar:
                        result = ResolveValueTruncateChar(field, out errorDescription);
                        break;
                    case FtMetaField.PropertyId.ValueEndOfValueChar:
                        result = ResolveValueEndOfValueChar(field, out errorDescription);
                        break;
                    case FtMetaField.PropertyId.ValueNullChar:
                        result = ResolveValueNullChar(field, out errorDescription);
                        break;
                    case FtMetaField.PropertyId.HeadingQuotedType:
                        result = ResolveHeadingQuotedType(field, out errorDescription);
                        break;
                    case FtMetaField.PropertyId.HeadingAlwaysWriteOptionalQuote:
                        result = ResolveHeadingAlwaysWriteOptionalQuote(field, out errorDescription);
                        break;
                    case FtMetaField.PropertyId.HeadingWritePrefixSpace:
                        result = ResolveHeadingWritePrefixSpace(field, out errorDescription);
                        break;
                    case FtMetaField.PropertyId.HeadingPadAlignment:
                        result = ResolveHeadingPadAlignment(field, out errorDescription);
                        break;
                    case FtMetaField.PropertyId.HeadingPadCharType:
                        result = ResolveHeadingPadCharType(field, out errorDescription);
                        break;
                    case FtMetaField.PropertyId.HeadingPadChar:
                        result = ResolveHeadingPadChar(field, out errorDescription);
                        break;
                    case FtMetaField.PropertyId.HeadingTruncateType:
                        result = ResolveHeadingTruncateType(field, out errorDescription);
                        break;
                    case FtMetaField.PropertyId.HeadingTruncateChar:
                        result = ResolveHeadingTruncateChar(field, out errorDescription);
                        break;
                    case FtMetaField.PropertyId.HeadingEndOfValueChar:
                        result = ResolveHeadingEndOfValueChar(field, out errorDescription);
                        break;

                    case FtMetaField.PropertyId.Value:
                        switch (dataType)
                        {
                            case FtStandardDataType.String:
                                result = ResolveStringValue(field as FtStringMetaField, out errorDescription);
                                break;
                            case FtStandardDataType.Boolean:
                                result = ResolveBooleanValue(field as FtBooleanMetaField, out errorDescription);
                                break;
                            case FtStandardDataType.Integer:
                                result = ResolveIntegerValue(field as FtIntegerMetaField, out errorDescription);
                                break;
                            case FtStandardDataType.Float:
                                result = ResolveFloatValue(field as FtFloatMetaField, out errorDescription);
                                break;
                            case FtStandardDataType.Decimal:
                                result = ResolveDecimalValue(field as FtDecimalMetaField, out errorDescription);
                                break;
                            case FtStandardDataType.DateTime:
                                result = ResolveDateTimeValue(field as FtDateTimeMetaField, out errorDescription);
                                break;
                            default:
                                throw CreateResolveNotSupportedInternalException(InternalError.FieldReadElement_ResolveProperties_ValueUnsupportedDataType, id, dataType);
                        }
                        break;

                    case FtMetaField.PropertyId.Styles:
                        switch (dataType)
                        {
                            case FtStandardDataType.String:
                                result = true; // String does not have styles
                                break;
                            case FtStandardDataType.Boolean:
                                result = ResolveBooleanStyles(field as FtBooleanMetaField, out errorDescription);
                                break;
                            case FtStandardDataType.Integer:
                                result = ResolveIntegerStyles(field as FtIntegerMetaField, out errorDescription);
                                break;
                            case FtStandardDataType.Float:
                                result = ResolveFloatStyles(field as FtFloatMetaField, out errorDescription);
                                break;
                            case FtStandardDataType.Decimal:
                                result = ResolveDecimalStyles(field as FtDecimalMetaField, out errorDescription);
                                break;
                            case FtStandardDataType.DateTime:
                                result = ResolveDateTimeStyles(field as FtDateTimeMetaField, out errorDescription);
                                break;
                            default:
                                throw CreateResolveNotSupportedInternalException(InternalError.FieldReadElement_ResolveProperties_StylesUnsupportedDataType, id, dataType);
                        }
                        break;
                    case FtMetaField.PropertyId.Format:
                        switch (dataType)
                        {
                            case FtStandardDataType.String:
                                result = true; // String does not have Format
                                break;
                            case FtStandardDataType.Boolean:
                                result = true; // Boolean does not have Format
                                break;
                            case FtStandardDataType.Integer:
                                result = ResolveIntegerFormat(field as FtIntegerMetaField, out errorDescription);
                                break;
                            case FtStandardDataType.Float:
                                result = ResolveFloatFormat(field as FtFloatMetaField, out errorDescription);
                                break;
                            case FtStandardDataType.Decimal:
                                result = ResolveDecimalFormat(field as FtDecimalMetaField, out errorDescription);
                                break;
                            case FtStandardDataType.DateTime:
                                result = ResolveDateTimeFormat(field as FtDateTimeMetaField, out errorDescription);
                                break;
                            default:
                                throw CreateResolveNotSupportedInternalException(InternalError.FieldReadElement_ResolveProperties_FormatUnsupportedDataType, id, dataType);
                        }
                        break;
                    case FtMetaField.PropertyId.FalseText:
                        if (dataType == FtStandardDataType.Boolean)
                            result = ResolveBooleanFalseText(field as FtBooleanMetaField, out errorDescription);
                        else
                            result = true; // Only Boolean supports FalseText
                        break;
                    case FtMetaField.PropertyId.TrueText:
                        if (dataType == FtStandardDataType.Boolean)
                            result = ResolveBooleanTrueText(field as FtBooleanMetaField, out errorDescription);
                        else
                            result = true; // Only Boolean supports TrueText
                        break;

                    default:
                        throw CreateResolveNotSupportedInternalException(InternalError.FieldReadElement_ResolveProperties_UnsupportedPropertyId, id);
                }

                if (!result)
                {
                    break;
                }
            }
            return result;
        }

        private FtInternalException CreateResolveNotSupportedInternalException(InternalError error, FtMetaField.PropertyId propertyId)
        {
            return FtInternalException.Create(error, propertyId.ToString());
        }
        private FtInternalException CreateResolveNotSupportedInternalException(InternalError error, FtMetaField.PropertyId propertyId, int dataType)
        {
            return FtInternalException.Create(error, propertyId.ToString() + "," + dataType.ToString());
        }

        // properties

        private bool ResolveId(FtMetaField field, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaField.PropertyId.Id, out attributeValue))
                field.Id = FtMetaField.DefaultId;
            else
            {
                int propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    field.Id = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldReadElement_ResolveId_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveName(FtMetaField field, out string errorDescription)
        {
            errorDescription = "";
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaField.PropertyId.Name, out attributeValue))
                field.Name = FtMetaField.DefaultName;
            else
                field.Name = attributeValue;

            return true;
        }
        private bool ResolveFixedWidth(FtMetaField field, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaField.PropertyId.FixedWidth, out attributeValue))
                field.FixedWidth = FtMetaField.DefaultFixedWidth;
            else
            {
                bool propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    field.FixedWidth = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldReadElement_ResolveFixedWidth_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveWidth(FtMetaField field, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaField.PropertyId.Width, out attributeValue))
                field.Width = FtMetaField.DefaultWidth;
            else
            {
                int propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    field.Width = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldReadElement_ResolveWidth_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveConstant(FtMetaField field, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaField.PropertyId.Constant, out attributeValue))
                field.Constant = FtMetaField.DefaultConstant;
            else
            {
                bool propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    field.Constant = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldReadElement_ResolveConstant_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveNull(FtMetaField field, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaField.PropertyId.Null, out attributeValue))
                field.Null = FtMetaField.DefaultNull;
            else
            {
                bool propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    field.Null = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldReadElement_ResolveNull_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveHeadings(FtMetaField field, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;

            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaField.PropertyId.Headings, out attributeValue))
                field.Headings = field.GetDefaultHeadings();
            else
            {
                string[] propertyValue;
                if (!FtCommaText.TryParse(attributeValue, out propertyValue, out errorDescription))
                {
                    errorDescription = string.Format(Properties.Resources.FieldReadElement_ResolveHeadings_Invalid, attributeValue);
                    result = false;
                }
                else
                {
                    if (propertyValue.Length == field.HeadingCount)
                    {
                        Array.Resize<string>(ref propertyValue, field.HeadingCount);
                    }
                    field.Headings = propertyValue;
                }
            }

            return result;
        }
        private bool ResolveHeadingConstraint(FtMetaField field, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaField.PropertyId.HeadingConstraint, out attributeValue))
                field.HeadingConstraint = field.DefaultHeadingConstraint;
            else
            {
                FtHeadingConstraint propertyValue;
                if (HeadingConstraintFormatter.TryParseAttributeValue(attributeValue, out propertyValue))
                    field.HeadingConstraint = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldReadElement_ResolveHeadingConstraint_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveValueQuotedType(FtMetaField field, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaField.PropertyId.ValueQuotedType, out attributeValue))
                field.ValueQuotedType = MetaSerializationDefaults.Field.ValueQuotedType;
            else
            {
                FtQuotedType propertyValue;
                if (QuotedTypeFormatter.TryParseAttributeValue(attributeValue, out propertyValue))
                    field.ValueQuotedType = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldReadElement_ResolveValueQuotedType_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveValueAlwaysWriteOptionalQuote(FtMetaField field, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaField.PropertyId.ValueAlwaysWriteOptionalQuote, out attributeValue))
                field.ValueAlwaysWriteOptionalQuote = MetaSerializationDefaults.Field.ValueAlwaysWriteOptionalQuote;
            else
            {
                bool propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    field.ValueAlwaysWriteOptionalQuote = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldReadElement_ResolveValueAlwaysWriteOptionalQuote_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveValueWritePrefixSpace(FtMetaField field, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaField.PropertyId.ValueWritePrefixSpace, out attributeValue))
                field.ValueWritePrefixSpace = MetaSerializationDefaults.Field.ValueWritePrefixSpace;
            else
            {
                bool propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    field.ValueWritePrefixSpace = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldReadElement_ResolveValueWritePrefixSpace_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveValuePadAlignment(FtMetaField field, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaField.PropertyId.ValuePadAlignment, out attributeValue))
                field.ValuePadAlignment = MetaSerializationDefaults.Field.ValuePadAlignment;
            else
            {
                FtPadAlignment propertyValue;
                if (PadAlignmentFormatter.TryParseAttributeValue(attributeValue, out propertyValue))
                    field.ValuePadAlignment = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldReadElement_ResolveValuePadAlignment_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveValuePadCharType(FtMetaField field, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaField.PropertyId.ValuePadCharType, out attributeValue))
                field.ValuePadCharType = MetaSerializationDefaults.Field.ValuePadCharType;
            else
            {
                FtPadCharType propertyValue;
                if (PadCharTypeFormatter.TryParseAttributeValue(attributeValue, out propertyValue))
                    field.ValuePadCharType = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldReadElement_ResolveValuePadCharType_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveValuePadChar(FtMetaField field, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaField.PropertyId.ValuePadChar, out attributeValue))
                field.ValuePadChar = MetaSerializationDefaults.Field.ValuePadChar;
            else
            {
                char propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    field.ValuePadChar = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldReadElement_ResolveValuePadChar_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveValueTruncateType(FtMetaField field, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaField.PropertyId.ValueTruncateType, out attributeValue))
                field.ValueTruncateType = MetaSerializationDefaults.Field.ValueTruncateType;
            else
            {
                FtTruncateType propertyValue;
                if (TruncateTypeFormatter.TryParseAttributeValue(attributeValue, out propertyValue))
                    field.ValueTruncateType = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldReadElement_ResolveValueTruncateType_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveValueTruncateChar(FtMetaField field, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaField.PropertyId.ValueTruncateChar, out attributeValue))
                field.ValueTruncateChar = MetaSerializationDefaults.Field.ValueTruncateChar;
            else
            {
                char propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    field.ValueTruncateChar = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldReadElement_ResolveValueTruncateChar_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveValueEndOfValueChar(FtMetaField field, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaField.PropertyId.ValueEndOfValueChar, out attributeValue))
                field.ValueEndOfValueChar = MetaSerializationDefaults.Field.ValueEndOfValueChar;
            else
            {
                char propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    field.ValueEndOfValueChar = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldReadElement_ResolveValueEndOfValueChar_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveValueNullChar(FtMetaField field, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaField.PropertyId.ValueNullChar, out attributeValue))
                field.ValueNullChar = MetaSerializationDefaults.Field.ValueNullChar;
            else
            {
                char propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    field.ValueNullChar = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldReadElement_ResolveValueNullChar_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveHeadingQuotedType(FtMetaField field, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaField.PropertyId.HeadingQuotedType, out attributeValue))
                field.HeadingQuotedType = field.DefaultHeadingQuotedType;
            else
            {
                FtQuotedType propertyValue;
                if (QuotedTypeFormatter.TryParseAttributeValue(attributeValue, out propertyValue))
                    field.HeadingQuotedType = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldReadElement_ResolveHeadingQuotedType_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveHeadingAlwaysWriteOptionalQuote(FtMetaField field, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaField.PropertyId.HeadingAlwaysWriteOptionalQuote, out attributeValue))
                field.HeadingAlwaysWriteOptionalQuote = field.DefaultHeadingAlwaysWriteOptionalQuote;
            else
            {
                bool propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    field.HeadingAlwaysWriteOptionalQuote = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldReadElement_ResolveHeadingAlwaysWriteOptionalQuote_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveHeadingWritePrefixSpace(FtMetaField field, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaField.PropertyId.HeadingWritePrefixSpace, out attributeValue))
                field.HeadingWritePrefixSpace = field.DefaultHeadingWritePrefixSpace;
            else
            {
                bool propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    field.HeadingWritePrefixSpace = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldReadElement_ResolveHeadingWritePrefixSpace_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveHeadingPadAlignment(FtMetaField field, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaField.PropertyId.HeadingPadAlignment, out attributeValue))
                field.HeadingPadAlignment = field.DefaultHeadingPadAlignment;
            else
            {
                FtPadAlignment propertyValue;
                if (PadAlignmentFormatter.TryParseAttributeValue(attributeValue, out propertyValue))
                    field.HeadingPadAlignment = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldReadElement_ResolveHeadingPadAlignment_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveHeadingPadCharType(FtMetaField field, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaField.PropertyId.HeadingPadCharType, out attributeValue))
                field.HeadingPadCharType = field.DefaultHeadingPadCharType;
            else
            {
                FtPadCharType propertyValue;
                if (PadCharTypeFormatter.TryParseAttributeValue(attributeValue, out propertyValue))
                    field.HeadingPadCharType = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldReadElement_ResolveHeadingPadCharType_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveHeadingPadChar(FtMetaField field, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaField.PropertyId.HeadingPadChar, out attributeValue))
                field.HeadingPadChar = field.DefaultHeadingPadChar;
            else
            {
                char propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    field.HeadingPadChar = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldReadElement_ResolveHeadingPadChar_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveHeadingTruncateType(FtMetaField field, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaField.PropertyId.HeadingTruncateType, out attributeValue))
                field.HeadingTruncateType = field.DefaultHeadingTruncateType;
            else
            {
                FtTruncateType propertyValue;
                if (TruncateTypeFormatter.TryParseAttributeValue(attributeValue, out propertyValue))
                    field.HeadingTruncateType = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldReadElement_ResolveHeadingTruncateType_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveHeadingTruncateChar(FtMetaField field, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaField.PropertyId.HeadingTruncateChar, out attributeValue))
                field.HeadingTruncateChar = field.DefaultHeadingTruncateChar;
            else
            {
                char propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    field.HeadingTruncateChar = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldReadElement_ResolveHeadingTruncateChar_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveHeadingEndOfValueChar(FtMetaField field, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaField.PropertyId.HeadingEndOfValueChar, out attributeValue))
                field.HeadingEndOfValueChar = field.DefaultHeadingEndOfValueChar;
            else
            {
                char propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    field.HeadingEndOfValueChar = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldReadElement_ResolveHeadingEndOfValueChar_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }

        // Value Properties

        private bool ResolveStringValue(FtStringMetaField field, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaField.PropertyId.Value, out attributeValue))
            {
                if (!field.Constant || field.Null)
                    field.Value = FtStringMetaField.DefaultValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldReadElement_ResolveStringValue_Missing, attributeValue);
                    result = false;
                }
            }
            else
                field.Value = attributeValue;

            return result;
        }
        private bool ResolveBooleanValue(FtBooleanMetaField field, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaField.PropertyId.Value, out attributeValue))
            {
                if (!field.Constant || field.Null)
                    field.Value = FtBooleanMetaField.DefaultValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldReadElement_ResolveBooleanValue_Missing, attributeValue);
                    result = false;
                }
            }
            else
            {
                bool propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    field.Value = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldReadElement_ResolveBooleanValue_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveIntegerValue(FtIntegerMetaField field, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaField.PropertyId.Value, out attributeValue))
            {
                if (!field.Constant || field.Null)
                    field.Value = FtIntegerMetaField.DefaultValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldReadElement_ResolveIntegerValue_Missing, attributeValue);
                    result = false;
                }
            }
            else
            {
                long propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    field.Value = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldReadElement_ResolveIntegerValue_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveFloatValue(FtFloatMetaField field, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaField.PropertyId.Value, out attributeValue))
            {
                if (!field.Constant || field.Null)
                    field.Value = FtFloatMetaField.DefaultValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldReadElement_ResolveFloatValue_Missing, attributeValue);
                    result = false;
                }
            }
            else
            {
                double propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    field.Value = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldReadElement_ResolveFloatValue_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveDecimalValue(FtDecimalMetaField field, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaField.PropertyId.Value, out attributeValue))
            {
                if (!field.Constant || field.Null)
                    field.Value = FtDecimalMetaField.DefaultValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldReadElement_ResolveDecimalValue_Missing, attributeValue);
                    result = false;
                }
            }
            else
            {
                decimal propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    field.Value = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldReadElement_ResolveDecimalValue_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveDateTimeValue(FtDateTimeMetaField field, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaField.PropertyId.Value, out attributeValue))
            {
                if (!field.Constant || field.Null)
                    field.Value = FtDateTimeMetaField.DefaultValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldReadElement_ResolveDateTimeValue_Missing, attributeValue);
                    result = false;
                }
            }
            else
            {
                DateTime propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    field.Value = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldReadElement_ResolveDateTimeValue_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }

        // Style Properties

        private bool ResolveBooleanStyles(FtBooleanMetaField field, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaField.PropertyId.Styles, out attributeValue))
                field.Styles = MetaSerializationDefaults.BooleanField.Styles;
            else
            {
                FtBooleanStyles propertyValue;
                if (BooleanStylesFormatter.TryParseAttributeValue(attributeValue, out propertyValue))
                    field.Styles = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldReadElement_ResolveBooleanStyles_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveIntegerStyles(FtIntegerMetaField field, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaField.PropertyId.Styles, out attributeValue))
                field.Styles = MetaSerializationDefaults.IntegerField.Styles;
            else
            {
                NumberStyles propertyValue;
                if (NumberStylesFormatter.TryParseAttributeValue(attributeValue, out propertyValue))
                    field.Styles = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldReadElement_ResolveIntegerStyles_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveFloatStyles(FtFloatMetaField field, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaField.PropertyId.Styles, out attributeValue))
                field.Styles = MetaSerializationDefaults.FloatField.Styles;
            else
            {
                NumberStyles propertyValue;
                if (NumberStylesFormatter.TryParseAttributeValue(attributeValue, out propertyValue))
                    field.Styles = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldReadElement_ResolveFloatStyles_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveDecimalStyles(FtDecimalMetaField field, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaField.PropertyId.Styles, out attributeValue))
                field.Styles = MetaSerializationDefaults.DecimalField.Styles;
            else
            {
                NumberStyles propertyValue;
                if (NumberStylesFormatter.TryParseAttributeValue(attributeValue, out propertyValue))
                    field.Styles = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldReadElement_ResolveDecimalStyles_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveDateTimeStyles(FtDateTimeMetaField field, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaField.PropertyId.Styles, out attributeValue))
                field.Styles = MetaSerializationDefaults.DateTimeField.Styles;
            else
            {
                DateTimeStyles propertyValue;
                if (DateTimeStylesFormatter.TryParseAttributeValue(attributeValue, out propertyValue))
                    field.Styles = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldReadElement_ResolveDateTimeStyles_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }

        // Format Properties

        private bool ResolveIntegerFormat(FtIntegerMetaField field, out string errorDescription)
        {
            errorDescription = "";
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaField.PropertyId.Format, out attributeValue))
                field.Format = MetaSerializationDefaults.IntegerField.Format;
            else
                field.Format = attributeValue;

            return true;
        }
        private bool ResolveFloatFormat(FtFloatMetaField field, out string errorDescription)
        {
            errorDescription = "";
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaField.PropertyId.Format, out attributeValue))
                field.Format = MetaSerializationDefaults.FloatField.Format;
            else
                field.Format = attributeValue;

            return true;
        }
        private bool ResolveDecimalFormat(FtDecimalMetaField field, out string errorDescription)
        {
            errorDescription = "";
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaField.PropertyId.Format, out attributeValue))
                field.Format = MetaSerializationDefaults.DecimalField.Format;
            else
                field.Format = attributeValue;

            return true;
        }
        private bool ResolveDateTimeFormat(FtDateTimeMetaField field, out string errorDescription)
        {
            errorDescription = "";
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaField.PropertyId.Format, out attributeValue))
                field.Format = MetaSerializationDefaults.DateTimeField.Format;
            else
                field.Format = attributeValue;

            return true;
        }

        // FalseText and TrueText Boolean Properties

        private bool ResolveBooleanFalseText(FtBooleanMetaField field, out string errorDescription)
        {
            errorDescription = "";
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaField.PropertyId.FalseText, out attributeValue))
                field.FalseText = MetaSerializationDefaults.BooleanField.FalseText;
            else
                field.FalseText = attributeValue;

            return true;
        }
        private bool ResolveBooleanTrueText(FtBooleanMetaField field, out string errorDescription)
        {
            errorDescription = "";
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaField.PropertyId.TrueText, out attributeValue))
                field.TrueText = MetaSerializationDefaults.BooleanField.TrueText;
            else
                field.TrueText = attributeValue;

            return true;
        }
    }
}
