// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;
using System.Collections.Generic;

namespace Xilytix.FieldedText.MetaSerialization
{
    using Formatting;

    using AttributeDictionary = Dictionary<FtMetaSubstitution.PropertyId, string>;

    public class SubstitutionReadElement : ReadElement
    {
        private AttributeDictionary attributeDictionary;

        internal protected SubstitutionReadElement() : base()
        {
            attributeDictionary = new AttributeDictionary();
        }

        internal protected override bool TryCreate(MetaElementType elementType, out ReadElement element)
        {
            element = null;
            return false;
        }
        internal protected override bool TryAddOrIgnoreAttribute(string name, string value, out string errorDescription)
        {
            FtMetaSubstitution.PropertyId id;
            if (SubstitutionPropertyIdFormatter.TryParseAttributeName(name, out id))
            {
                attributeDictionary.Add(id, value);
            }

            errorDescription = "";
            return true;
        }

        internal protected virtual bool ResolveTo(FtMetaSubstitution substitution, out string errorDescription)
        {
            return ResolveProperties(substitution, out errorDescription);
        }
        protected virtual bool ResolveProperties(FtMetaSubstitution substitution, out string errorDescription)
        {
            errorDescription = "";
            bool result = false;
            foreach (FtMetaSubstitution.PropertyId id in Enum.GetValues(typeof(FtMetaSubstitution.PropertyId)))
            {
                switch (id)
                {
                    case FtMetaSubstitution.PropertyId.Type:
                        result = ResolveType(substitution, out errorDescription);
                        break;
                    case FtMetaSubstitution.PropertyId.Token:
                        result = ResolveToken(substitution, out errorDescription);
                        break;
                    case FtMetaSubstitution.PropertyId.Value:
                        result = ResolveValue(substitution, out errorDescription);
                        break;
                    default:
                        throw FtInternalException.Create(InternalError.SubstitutionReadElement_ResolveProperties_UnsupportedPropertyId, id.ToString());
                }

                if (!result)
                {
                    break;
                }
            }
            return result;
        }

        // properties

        private bool ResolveType(FtMetaSubstitution substitution, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaSubstitution.PropertyId.Type, out attributeValue))
                substitution.Type = FtMetaSubstitution.DefaultType;
            else
            {
                FtSubstitutionType propertyValue;
                if (SubstitutionTypeFormatter.TryParseAttributeValue(attributeValue, out propertyValue))
                    substitution.Type = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.SubstitutionReadElement_ResolveType_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveToken(FtMetaSubstitution substitution, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaSubstitution.PropertyId.Token, out attributeValue))
            {
                errorDescription = string.Format(Properties.Resources.SubstitutionReadElement_ResolveToken_Missing, SubstitutionTypeFormatter.ToAttributeValue(substitution.Type));
                result = false;
            }
            else
            {
                char propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    substitution.Token = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.SubstitutionReadElement_ResolveToken_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveValue(FtMetaSubstitution substitution, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaSubstitution.PropertyId.Value, out attributeValue))
            {
                if (substitution.Type == FtSubstitutionType.String)
                {
                    errorDescription = string.Format(Properties.Resources.SubstitutionReadElement_ResolveValue_MissingForString, substitution.Token.ToString());
                    result = false;
                }
            }
            else
            {
                substitution.Value = attributeValue;
            }

            return result;
        }
    }
}
