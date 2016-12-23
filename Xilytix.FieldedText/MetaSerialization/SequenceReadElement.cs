// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;
using System.Collections.Generic;

namespace Xilytix.FieldedText.MetaSerialization
{
    using Formatting;

    using AttributeDictionary = Dictionary<FtMetaSequence.PropertyId, string>;

    using ItemList = List<SequenceItemReadElement>;

    using ItemSorter = ImplicitExplicitIndexSorter<SequenceItemReadElement>;

    class SequenceReadElement : ReadElement
    {
        private AttributeDictionary attributeDictionary;

        private ItemList itemList;
        private int[] fieldIndices;

        internal protected SequenceReadElement() : base()
        {
            attributeDictionary = new AttributeDictionary();
            itemList = new ItemList();
        }

        internal protected override bool TryCreate(MetaElementType elementType, out ReadElement element)
        {
            switch (elementType)
            {
                case MetaElementType.SequenceItem:
                    SequenceItemReadElement itemReadElement = new SequenceItemReadElement();
                    itemList.Add(itemReadElement);
                    element = itemReadElement;
                    return true;
                default:
                    element = null;
                    return false;
            }
        }
        internal protected override bool TryAddOrIgnoreAttribute(string name, string value, out string errorDescription)
        {
            FtMetaSequence.PropertyId id;
            if (SequencePropertyIdFormatter.TryParseAttributeName(name, out id))
            {
                attributeDictionary.Add(id, value);
            }

            errorDescription = "";
            return true;
        }

        internal protected virtual bool ResolvePropertiesTo(FtMetaSequence sequence, FtMetaFieldList fieldList, FtMetaSequenceList existingSequences, out string errorDescription)
        {
            errorDescription = "";
            bool result = false;
            foreach (FtMetaSequence.PropertyId id in Enum.GetValues(typeof(FtMetaSequence.PropertyId)))
            {
                switch (id)
                {
                    case FtMetaSequence.PropertyId.Name:
                        result = ResolveName(sequence, existingSequences, out errorDescription);
                        break;
                    case FtMetaSequence.PropertyId.Root:
                        result = ResolveRoot(sequence, out errorDescription);
                        break;
                    case FtMetaSequence.PropertyId.FieldIndices:
                        result = ResolveFieldIndices(sequence, fieldList, out errorDescription);
                        break;
                    default:
                        throw FtInternalException.Create(InternalError.SequenceReadElement_ResolvePropertiesTo_UnsupportedPropertyId, id.ToString());
                }

                if (!result)
                {
                    break;
                }
            }
            return result;
        }
        internal protected virtual bool ResolveItemsTo(FtMetaSequence sequence, FtMetaFieldList fieldList, FtMetaSequenceList sequenceList, out string errorDescription)
        {
            ItemSorter.Rec[] sorterRecArray = new ItemSorter.Rec[itemList.Count + fieldIndices.Length];
            SequenceItemReadElement[] sortedItemElements;

            for (int i = 0; i < itemList.Count; i++)
            {
                SequenceItemReadElement itemElement = itemList[i];

                sorterRecArray[i].Target = itemElement;
                sorterRecArray[i].Implicit = i;
                sorterRecArray[i].Explicit = itemElement.ExplicitIndex;
            }

            int elementallyDefinedItemCount = itemList.Count;
            for (int i = 0; i < fieldIndices.Length; i++)
            {
                SequenceItemReadElement itemElement = new SequenceItemReadElement();
                // Add FieldIndex to element as if it were read from an attribute.  
                int fieldIndex = fieldIndices[i];
                string attributeName = SequenceItemPropertyIdFormatter.ToAttributeName(FtMetaSequenceItem.PropertyId.FieldIndex);
                string attributeValue = FtStandardText.Get(fieldIndex);
                if (!itemElement.TryAddOrIgnoreAttribute(attributeName, attributeValue, out errorDescription))
                    throw FtInternalException.Create(InternalError.SequenceReadElement_ResolveItemsTo_TryAddOrIgnoreAttribute, errorDescription); // program error
                else
                {
                    int implicitIndex = elementallyDefinedItemCount + i;
                    sorterRecArray[implicitIndex].Target = itemElement;
                    sorterRecArray[implicitIndex].Implicit = implicitIndex;
                    sorterRecArray[implicitIndex].Explicit = -1;
                }
            }

            int duplicateExplicitIndex;
            if (!ItemSorter.TrySort(sorterRecArray, out sortedItemElements, out duplicateExplicitIndex))
            {
                errorDescription = string.Format(Properties.Resources.SequenceReadElement_ResolveItemsTo_DuplicateExplicitIndex, duplicateExplicitIndex);
                return false;
            }
            else
            {
                errorDescription = "";
                bool result = true;

                for (int i = 0; i < sortedItemElements.Length; i++)
                {
                    SequenceItemReadElement itemElement = sortedItemElements[i];
                    FtMetaSequenceItem item = sequence.ItemList.New();
                    if (!itemElement.ResolveTo(item, fieldList, sequenceList, out errorDescription))
                    {
                        result = false;
                        break;
                    }
                }

                return result;
            }
        }

        // properties

        private bool ResolveName(FtMetaSequence sequence, FtMetaSequenceList existingSequences, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaSequence.PropertyId.Name, out attributeValue))
            {
                errorDescription = Properties.Resources.SequenceReadElement_ResolveName_Missing;
                result = false;
            }
            else
            {
                int existingNameIdx = existingSequences.IndexOfName(attributeValue);
                if (existingNameIdx >= 0 && existingNameIdx < existingSequences.Count-1) // sequence is last in existing list, so ignore that
                {
                    errorDescription = string.Format(Properties.Resources.SequenceReadElement_ResolveName_Duplicate, attributeValue);
                    result = false;
                }
                else
                {
                    sequence.Name = attributeValue;
                }
            }

            return result;
        }
        private bool ResolveRoot(FtMetaSequence sequence, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaSequence.PropertyId.Root, out attributeValue))
                sequence.Root = FtMetaSequence.DefaultRoot;
            else
            {
                bool propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    sequence.Root = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.SequenceReadElement_ResolveRoot_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveFieldIndices(FtMetaSequence sequence, FtMetaFieldList fieldList, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMetaSequence.PropertyId.FieldIndices, out attributeValue))
                fieldIndices = new int[0];
            else
            {
                int[] propertyValue;
                if (FtCommaText.TryParse(attributeValue, out propertyValue, out errorDescription))
                    fieldIndices = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.SequenceReadElement_ResolveFieldIndices_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
    }
}
