// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;
using System.Collections.Generic;

namespace Xilytix.FieldedText.MetaSerialization
{
    using Formatting;

    using AttributeDictionary = Dictionary<FtMetaSequenceItem.PropertyId, string>;

    using RedirectList = List<SequenceRedirectReadElement>;

    using RedirectSorter = ImplicitExplicitIndexSorter<SequenceRedirectReadElement>;

    class SequenceItemReadElement : ReadElement
    {
        private AttributeDictionary attributeDictionary;
        private RedirectList redirectList;
        private int explicitIndex;

        internal protected int ExplicitIndex { get { return explicitIndex; } }

        internal protected SequenceItemReadElement() : base()
        {
            attributeDictionary = new AttributeDictionary();
            redirectList = new RedirectList();
            explicitIndex = -1;
        }

        internal protected override bool TryCreate(MetaElementType elementType, out ReadElement element)
        {
            switch (elementType)
            {
                case MetaElementType.SequenceRedirect:
                    SequenceRedirectReadElement redirectReadElement = new SequenceRedirectReadElement();
                    redirectList.Add(redirectReadElement);
                    element = redirectReadElement;
                    return true;
                default:
                    element = null;
                    return false;
            }
        }
        internal protected override bool TryAddOrIgnoreAttribute(string name, string value, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;

            FtMetaSequenceItem.PropertyId id;
            if (SequenceItemPropertyIdFormatter.TryParseAttributeName(name, out id))
            {
                switch (id)
                {
                    case FtMetaSequenceItem.PropertyId.Index:
                        int parsedIndex;
                        if (FtStandardText.TryParse(value, out parsedIndex))
                            explicitIndex = parsedIndex;
                        else
                        {
                            errorDescription = string.Format(Properties.Resources.SequenceItemReadElement_TryAddOrIgnoreAttribute_Invalid, value); 
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

        internal protected virtual bool ResolveTo(FtMetaSequenceItem item, FtMetaFieldList fieldList, FtMetaSequenceList sequenceList, out string errorDescription)
        {
            if (!ResolveProperties(item, fieldList, out errorDescription))
                return false;
            else
                return ResolveRedirects(item, sequenceList, out errorDescription);
        }
        protected virtual bool ResolveProperties(FtMetaSequenceItem item, FtMetaFieldList fieldList, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            foreach (FtMetaSequenceItem.PropertyId id in Enum.GetValues(typeof(FtMetaSequenceItem.PropertyId)))
            {
                switch (id)
                {
                    case FtMetaSequenceItem.PropertyId.Index:
                        // already processed in TryAddOrIgnoreAttribute()
                        break;
                    case FtMetaSequenceItem.PropertyId.FieldIndex:
                        result = ResolveFieldIndex(item, fieldList, out errorDescription);
                        break;
                    default:
                        throw FtInternalException.Create(InternalError.SequenceItemReadElement_ResolveProperties_UnsupportedPropertyId, id.ToString());
                }

                if (!result)
                {
                    break;
                }
            }
            return result;
        }
        protected virtual bool ResolveRedirects(FtMetaSequenceItem item, FtMetaSequenceList sequenceList, out string errorDescription)
        {
            RedirectSorter.Rec[] sorterRecArray = new RedirectSorter.Rec[redirectList.Count];
            SequenceRedirectReadElement[] sortedElements;

            for (int i = 0; i < redirectList.Count; i++)
            {
                SequenceRedirectReadElement element = redirectList[i];

                sorterRecArray[i].Target = element;
                sorterRecArray[i].Implicit = i;
                sorterRecArray[i].Explicit = element.ExplicitIndex;
            }

            int duplicateExplicitIndex;
            if (!RedirectSorter.TrySort(sorterRecArray, out sortedElements, out duplicateExplicitIndex))
            {
                errorDescription = string.Format(Properties.Resources.SequenceItemReadElement_ResolveRedirects_DuplicateExplicitIndex, duplicateExplicitIndex);
                return false;
            }
            else
            {
                errorDescription = "";
                bool result = true;

                for (int i = 0; i < sortedElements.Length; i++)
                {
                    SequenceRedirectReadElement element = sortedElements[i];
                    int redirectType;
                    if (element.RedirectType == FtStandardSequenceRedirectType.Unknown)
                        redirectType = item.Field.DefaultSequenceRedirectType;
                    else
                        redirectType = element.RedirectType;
                    FtMetaSequenceRedirect redirect = item.RedirectList.New(redirectType);
                    if (!element.ResolveTo(redirect, sequenceList, out errorDescription))
                    {
                        result = false;
                        break;
                    }
                }

                return result;
            }
        }

        // properties

        private bool ResolveFieldIndex(FtMetaSequenceItem item, FtMetaFieldList fieldList, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaSequenceItem.PropertyId.FieldIndex, out attributeValue))
            {
                errorDescription = string.Format(Properties.Resources.SequenceItemReadElement_ResolveFieldIndex_Missing);
                result = false;
            }
            else
            {
                int fieldIndex;
                if (FtStandardText.TryParse(attributeValue, out fieldIndex))
                {
                    if (fieldIndex < 0 || fieldIndex >= fieldList.Count)
                        errorDescription = string.Format(Properties.Resources.SequenceItemReadElement_ResolveFieldIndex_OutOfRange, fieldIndex);
                    else
                        item.Field = fieldList[fieldIndex];
                }
                else
                {
                    errorDescription = string.Format(Properties.Resources.SequenceItemReadElement_ResolveFieldIndex_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
    }
}