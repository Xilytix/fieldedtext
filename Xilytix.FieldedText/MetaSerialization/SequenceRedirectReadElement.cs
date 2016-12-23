// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;
using System.Collections.Generic;

namespace Xilytix.FieldedText.MetaSerialization
{
    using Formatting;

    using AttributeDictionary = Dictionary<FtMetaSequenceRedirect.PropertyId, string>;

    public class SequenceRedirectReadElement: ReadElement
    {
        private AttributeDictionary attributeDictionary;
        private int explicitIndex;
        private int redirectType;

        public int ExplicitIndex { get { return explicitIndex; } }
        public int RedirectType { get { return redirectType; } }

        internal protected SequenceRedirectReadElement(): base()
        {
            attributeDictionary = new AttributeDictionary();
            explicitIndex = -1;
            redirectType = FtStandardSequenceRedirectType.Unknown;
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

            FtMetaSequenceRedirect.PropertyId id;
            if (SequenceRedirectPropertyIdFormatter.TryParseAttributeName(name, out id))
            {
                switch (id)
                {
                    case FtMetaSequenceRedirect.PropertyId.Index:
                        int parsedIndex;
                        if (FtStandardText.TryParse(value, out parsedIndex))
                            explicitIndex = parsedIndex;
                        else
                        {
                            errorDescription = string.Format(Properties.Resources.SequenceRedirectReadElement_TryAddOrIgnoreAttribute_InvalidIndex, value);
                            result = false;
                        }
                        break;

                    case FtMetaSequenceRedirect.PropertyId.Type:
                        int parsedType;
                        if (SequenceRedirectTypeFormatter.TryParseAttributeValue(value, out parsedType))
                            redirectType = parsedType;
                        else
                        {
                            errorDescription = string.Format(Properties.Resources.SequenceRedirectReadElement_TryAddOrIgnoreAttribute_InvalidType, value);
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

        internal protected virtual bool ResolveTo(FtMetaSequenceRedirect redirect, FtMetaSequenceList sequenceList, out string errorDescription)
        {
            return ResolveProperties(redirect, sequenceList, out errorDescription);
        }
        protected virtual bool ResolveProperties(FtMetaSequenceRedirect redirect, FtMetaSequenceList sequenceList, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            redirectType = redirect.Type; // may have been set to field default

            foreach (FtMetaSequenceRedirect.PropertyId id in Enum.GetValues(typeof(FtMetaSequenceRedirect.PropertyId)))
            {
                switch (id)
                {
                    case FtMetaSequenceRedirect.PropertyId.Index:
                    case FtMetaSequenceRedirect.PropertyId.Type:
                        // already handled in TryAddOrIgnoreAttribute
                        break;
                    case FtMetaSequenceRedirect.PropertyId.SequenceName:
                        result = ResolveSequenceName(redirect, sequenceList, out errorDescription);
                        break;
                    case FtMetaSequenceRedirect.PropertyId.InvokationDelay:
                        result = ResolveInvokationDelay(redirect, out errorDescription);
                        break;
                    case FtMetaSequenceRedirect.PropertyId.Value:
                        switch (redirectType)
                        {
                            case FtStandardSequenceRedirectType.Boolean:
                                result = ResolveBooleanValue(redirect as FtBooleanMetaSequenceRedirect, out errorDescription);
                                break;
                            case FtStandardSequenceRedirectType.CaseInsensitiveString:
                                result = ResolveCaseInsensitiveStringValue(redirect as FtCaseInsensitiveStringMetaSequenceRedirect, out errorDescription);
                                break;
                            case FtStandardSequenceRedirectType.Date:
                                result = ResolveDateValue(redirect as FtDateMetaSequenceRedirect, out errorDescription);
                                break;
                            case FtStandardSequenceRedirectType.ExactDateTime:
                                result = ResolveExactDateTimeValue(redirect as FtExactDateTimeMetaSequenceRedirect, out errorDescription);
                                break;
                            case FtStandardSequenceRedirectType.ExactDecimal:
                                result = ResolveExactDecimalValue(redirect as FtExactDecimalMetaSequenceRedirect, out errorDescription);
                                break;
                            case FtStandardSequenceRedirectType.ExactFloat:
                                result = ResolveExactFloatValue(redirect as FtExactFloatMetaSequenceRedirect, out errorDescription);
                                break;
                            case FtStandardSequenceRedirectType.ExactInteger:
                                result = ResolveExactIntegerValue(redirect as FtExactIntegerMetaSequenceRedirect, out errorDescription);
                                break;
                            case FtStandardSequenceRedirectType.ExactString:
                                result = ResolveExactStringValue(redirect as FtExactStringMetaSequenceRedirect, out errorDescription);
                                break;
                            case FtStandardSequenceRedirectType.Null:
                                // does not have Value
                                result = true;
                                break;
                            default:
                                throw FtInternalException.Create(InternalError.SequenceRedirectReadElement_ResolveProperties_ValueUnsupportedRedirectType,
                                                                 id.ToString() + "," + redirectType.ToString());
                        }
                        break;
                    default:
                        throw FtInternalException.Create(InternalError.SequenceRedirectReadElement_ResolveProperties_UnsupportedPropertyId, id.ToString());
                }

                if (!result)
                {
                    break;
                }
            }
            return result;
        }

        // properties

        private bool ResolveSequenceName(FtMetaSequenceRedirect redirect, FtMetaSequenceList sequenceList, out string errorDescription)
        {
            bool result = false;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaSequenceRedirect.PropertyId.SequenceName, out attributeValue))
                errorDescription = string.Format(Properties.Resources.SequenceRedirectReadElement_ResolveSequenceName_Missing, FtStandardSequenceRedirectType.ToName(redirectType));
            else
            {
                string sequenceName = attributeValue.Trim();
                int sequenceIdx = sequenceList.IndexOfName(sequenceName);
                if (sequenceIdx < 0)
                    errorDescription = string.Format(Properties.Resources.SequenceRedirectReadElement_ResolveSequenceName_NotExist, sequenceName, FtStandardSequenceRedirectType.ToName(redirectType));
                else
                {
                    redirect.Sequence = sequenceList[sequenceIdx];
                    errorDescription = "";
                    result = true;
                }
            }

            return result;
        }
        private bool ResolveInvokationDelay(FtMetaSequenceRedirect redirect, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaSequenceRedirect.PropertyId.InvokationDelay, out attributeValue))
                redirect.InvokationDelay = MetaSerializationDefaults.SequenceRedirect.InvokationDelay;
            else
            {
                FtSequenceInvokationDelay propertyValue;
                if (SequenceInvokationDelayFormatter.TryParseAttributeValue(attributeValue, out propertyValue))
                    redirect.InvokationDelay = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.SequenceRedirectReadElement_ResolveInvokationDelay_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveBooleanValue(FtBooleanMetaSequenceRedirect redirect, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaSequenceRedirect.PropertyId.Value, out attributeValue))
            {
                errorDescription = string.Format(Properties.Resources.SequenceRedirectReadElement_ResolveBooleanValue_Missing, FtStandardSequenceRedirectType.ToName(redirectType));
                result = false;
            }
            else
            {
                bool propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    redirect.Value = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.SequenceRedirectReadElement_ResolveBooleanValue_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveCaseInsensitiveStringValue(FtCaseInsensitiveStringMetaSequenceRedirect redirect, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaSequenceRedirect.PropertyId.Value, out attributeValue))
            {
                errorDescription = string.Format(Properties.Resources.SequenceRedirectReadElement_ResolveCaseInsensitiveStringValue_Missing, FtStandardSequenceRedirectType.ToName(redirectType));
                result = false;
            }
            else
            {
                string propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    redirect.Value = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.SequenceRedirectReadElement_ResolveCaseInsensitiveStringValue_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveDateValue(FtDateMetaSequenceRedirect redirect, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaSequenceRedirect.PropertyId.Value, out attributeValue))
            {
                errorDescription = string.Format(Properties.Resources.SequenceRedirectReadElement_ResolveDateValue_Missing, FtStandardSequenceRedirectType.ToName(redirectType));
                result = false;
            }
            else
            {
                DateTime propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    redirect.Value = propertyValue.Date;
                else
                {
                    errorDescription = string.Format(Properties.Resources.SequenceRedirectReadElement_ResolveDateValue_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveExactDateTimeValue(FtExactDateTimeMetaSequenceRedirect redirect, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaSequenceRedirect.PropertyId.Value, out attributeValue))
            {
                errorDescription = string.Format(Properties.Resources.SequenceRedirectReadElement_ResolveExactDateTimeValue_Missing, FtStandardSequenceRedirectType.ToName(redirectType));
                result = false;
            }
            else
            {
                DateTime propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    redirect.Value = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.SequenceRedirectReadElement_ResolveExactDateTimeValue_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveExactDecimalValue(FtExactDecimalMetaSequenceRedirect redirect, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaSequenceRedirect.PropertyId.Value, out attributeValue))
            {
                errorDescription = string.Format(Properties.Resources.SequenceRedirectReadElement_ResolveExactDecimalValue_Missing, FtStandardSequenceRedirectType.ToName(redirectType));
                result = false;
            }
            else
            {
                decimal propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    redirect.Value = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.SequenceRedirectReadElement_ResolveExactDecimalValue_Invalid, FtStandardSequenceRedirectType.ToName(redirectType));
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveExactFloatValue(FtExactFloatMetaSequenceRedirect redirect, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaSequenceRedirect.PropertyId.Value, out attributeValue))
            {
                errorDescription = string.Format(Properties.Resources.SequenceRedirectReadElement_ResolveExactFloatValue_Missing, FtStandardSequenceRedirectType.ToName(redirectType));
                result = false;
            }
            else
            {
                double propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    redirect.Value = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.SequenceRedirectReadElement_ResolveExactFloatValue_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveExactIntegerValue(FtExactIntegerMetaSequenceRedirect redirect, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaSequenceRedirect.PropertyId.Value, out attributeValue))
            {
                errorDescription = string.Format(Properties.Resources.SequenceRedirectReadElement_ResolveExactIntegerValue_Missing, FtStandardSequenceRedirectType.ToName(redirectType));
                result = false;
            }
            else
            {
                long propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    redirect.Value = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.SequenceRedirectReadElement_ResolveExactIntegerValue_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveExactStringValue(FtExactStringMetaSequenceRedirect redirect, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaSequenceRedirect.PropertyId.Value, out attributeValue))
            {
                errorDescription = string.Format(Properties.Resources.SequenceRedirectReadElement_ResolveExactStringValue_Missing, FtStandardSequenceRedirectType.ToName(redirectType));
                result = false;
            }
            else
            {
                string propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    redirect.Value = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.SequenceRedirectReadElement_ResolveExactStringValue_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
    }
}
