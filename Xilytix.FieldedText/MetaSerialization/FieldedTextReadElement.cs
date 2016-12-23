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

    using AttributeDictionary = Dictionary<FtMeta.PropertyId, string>;

    using FieldList = List<FieldReadElement>;
    using SequenceList = List<SequenceReadElement>;
    using SubstitutionList = List<SubstitutionReadElement>;

    using FieldSorter = ImplicitExplicitIndexSorter<FieldReadElement>;

    public class FieldedTextReadElement: ReadElement
    {
        private AttributeDictionary attributeDictionary;

        private FieldList fieldList;
        private SequenceList sequenceList;
        private SubstitutionList substitutionList;

        private bool lastLineEndedTypeSpecified;
        private bool allowIncompleteRecordsSpecified;

        internal protected FieldedTextReadElement(): base()
        {
            attributeDictionary = new AttributeDictionary();
            fieldList = new FieldList();
            sequenceList = new SequenceList();
            substitutionList = new SubstitutionList();
        }

        internal protected override bool TryCreate(MetaElementType elementType, out ReadElement element)
        {
            switch (elementType)
            {
                case MetaElementType.Field:
                    FieldReadElement fieldReadElement = new FieldReadElement();
                    fieldList.Add(fieldReadElement);
                    element = fieldReadElement;
                    return true;
                case MetaElementType.Sequence:
                    SequenceReadElement sequenceReadElement = new SequenceReadElement();
                    sequenceList.Add(sequenceReadElement);
                    element = sequenceReadElement;
                    return true;
                case MetaElementType.Substitution:
                    SubstitutionReadElement substitutionReadElement = new SubstitutionReadElement();
                    substitutionList.Add(substitutionReadElement);
                    element = substitutionReadElement;
                    return true;
                default:
                    element = null;
                    return false;
            }
        }

        internal protected override bool TryAddOrIgnoreAttribute(string name, string value, out string errorDescription)
        {
            FtMeta.PropertyId id;
            if (RootPropertyIdFormatter.TryParseAttributeName(name, out id))
            {
                attributeDictionary.Add(id, value);
            }

            errorDescription = "";
            return true;
        }

        internal protected virtual bool ResolveTo(FtMeta meta, out string errorDescription)
        {
            if (!ResolveProperties(meta, out errorDescription))
                return false;
            else
            {
                if (!ResolveFields(meta, out errorDescription))
                    return false;
                else
                {
                    if (!ResolveSequences(meta, out errorDescription))
                        return false;
                    else
                        return ResolveSubstitutions(meta, out errorDescription);
                }
            }
        }

        protected virtual bool ResolveProperties(FtMeta meta, out string errorDescription)
        {
            errorDescription = "";
            lastLineEndedTypeSpecified = false;
            allowIncompleteRecordsSpecified = false;
            bool result = false;
            foreach (FtMeta.PropertyId id in Enum.GetValues(typeof(FtMeta.PropertyId)))
            {
                switch (id)
                {
                    case FtMeta.PropertyId.AllowEndOfLineCharInQuotes:
                        result = ResolveAllowEndOfLineCharInQuotes(meta, out errorDescription);
                        break;
                    case FtMeta.PropertyId.Culture:
                        result = ResolveCulture(meta, out errorDescription);
                        break;
                    case FtMeta.PropertyId.DelimiterChar:
                        result = ResolveDelimiterChar(meta, out errorDescription);
                        break;
                    case FtMeta.PropertyId.EndOfLineAutoWriteType:
                        result = ResolveEndOfLineAutoWriteType(meta, out errorDescription);
                        break;
                    case FtMeta.PropertyId.EndOfLineChar:
                        result = ResolveEndOfLineChar(meta, out errorDescription);
                        break;
                    case FtMeta.PropertyId.EndOfLineType:
                        result = ResolveEndOfLineType(meta, out errorDescription);
                        break;
                    case FtMeta.PropertyId.LastLineEndedType:
                        result = ResolveLastLineEndedType(meta, out errorDescription);
                        break;
                    case FtMeta.PropertyId.HeadingAlwaysWriteOptionalQuote:
                        result = ResolveHeadingAlwaysWriteOptionalQuote(meta, out errorDescription);
                        break;
                    case FtMeta.PropertyId.HeadingConstraint:
                        result = ResolveHeadingConstraint(meta, out errorDescription);
                        break;
                    case FtMeta.PropertyId.HeadingEndOfValueChar:
                        result = ResolveHeadingEndOfValueChar(meta, out errorDescription);
                        break;
                    case FtMeta.PropertyId.HeadingLineCount:
                        result = ResolveHeadingLineCount(meta, out errorDescription);
                        break;
                    case FtMeta.PropertyId.HeadingPadAlignment:
                        result = ResolveHeadingPadAlignment(meta, out errorDescription);
                        break;
                    case FtMeta.PropertyId.HeadingPadChar:
                        result = ResolveHeadingPadChar(meta, out errorDescription);
                        break;
                    case FtMeta.PropertyId.HeadingPadCharType:
                        result = ResolveHeadingPadCharType(meta, out errorDescription);
                        break;
                    case FtMeta.PropertyId.HeadingQuotedType:
                        result = ResolveHeadingQuotedType(meta, out errorDescription);
                        break;
                    case FtMeta.PropertyId.HeadingTruncateChar:
                        result = ResolveHeadingTruncateChar(meta, out errorDescription);
                        break;
                    case FtMeta.PropertyId.HeadingTruncateType:
                        result = ResolveHeadingTruncateType(meta, out errorDescription);
                        break;
                    case FtMeta.PropertyId.HeadingWritePrefixSpace:
                        result = ResolveHeadingWritePrefixSpace(meta, out errorDescription);
                        break;
                    case FtMeta.PropertyId.IgnoreBlankLines:
                        result = ResolveIgnoreBlankLines(meta, out errorDescription);
                        break;
                    case FtMeta.PropertyId.IgnoreExtraChars:
                        result = ResolveIgnoreExtraChars(meta, out errorDescription);
                        break;
                    case FtMeta.PropertyId.AllowIncompleteRecords:
                        result = ResolveAllowIncompleteRecords(meta, out errorDescription);
                        break;
                    case FtMeta.PropertyId.LineCommentChar:
                        result = ResolveLineCommentChar(meta, out errorDescription);
                        break;
                    case FtMeta.PropertyId.MainHeadingLineIndex:
                        result = ResolveMainHeadingLineIndex(meta, out errorDescription);
                        break;
                    case FtMeta.PropertyId.QuoteChar:
                        result = ResolveQuoteChar(meta, out errorDescription);
                        break;
                    case FtMeta.PropertyId.StuffedEmbeddedQuotes:
                        result = ResolveStuffedEmbeddedQuotes(meta, out errorDescription);
                        break;
                    case FtMeta.PropertyId.SubstitutionChar:
                        result = ResolveSubstitutionChar(meta, out errorDescription);
                        break;
                    case FtMeta.PropertyId.SubstitutionsEnabled:
                        result = ResolveSubstitutionsEnabled(meta, out errorDescription);
                        break;
                    case FtMeta.PropertyId.LegacyEndOfLineIsSeparator:
                        result = ResolveLegacyEndOfLineIsSeparator(meta, out errorDescription);
                        break;
                    case FtMeta.PropertyId.LegacyIncompleteRecordsAllowed:
                        result = ResolveLegacyIncompleteRecordsAllowed(meta, out errorDescription);
                        break;
                    default:
                        throw FtInternalException.Create(InternalError.FieldedTextReadElement_ResolveProperties_UnsupportedPropertyId, id.ToString());
                }

                if (!result)
                {
                    break;
                }
            }
            return result;
        }

        protected virtual bool ResolveFields(FtMeta meta, out string errorDescription)
        {
            FieldSorter.Rec[] sorterRecArray = new FieldSorter.Rec[fieldList.Count];
            FieldReadElement[] sortedElements;

            for (int i = 0; i < fieldList.Count; i++)
            {
                FieldReadElement element = fieldList[i];

                sorterRecArray[i].Target = element;
                sorterRecArray[i].Implicit = i;
                sorterRecArray[i].Explicit = element.ExplicitIndex;
            }

            int duplicateExplicitIndex;
            if (!FieldSorter.TrySort(sorterRecArray, out sortedElements, out duplicateExplicitIndex))
            {
                errorDescription = string.Format(Properties.Resources.FieldedTextReadElement_ResolveFields_DuplicateExplicitIndexInFields, duplicateExplicitIndex);
                return false;
            }
            else
            {
                errorDescription = "";
                bool result = true;

                for (int i = 0; i < sortedElements.Length; i++)
                {
                    FieldReadElement element = sortedElements[i];
                    FtMetaField field = meta.FieldList.New(element.DataType);
                    if (!element.ResolveTo(field, out errorDescription))
                    {
                        result = false;
                        break;
                    }
                }

                return result;
            }
        }

        protected virtual bool ResolveSequences(FtMeta meta, out string errorDescription)
        {
            bool result = true;
            errorDescription = "";

            // need to get all sequence names before creating redirects.  So resolve Sequence properties first and then its items
            for (int i = 0; i < sequenceList.Count; i++)
            {
                FtMetaSequence sequence = meta.SequenceList.New();
                if (!sequenceList[i].ResolvePropertiesTo(sequence, meta.FieldList, meta.SequenceList, out errorDescription))
                {
                    result = false;
                    break;
                }
            }

            if (result)
            {
                for (int i = 0; i < sequenceList.Count; i++)
                {
                    if (!sequenceList[i].ResolveItemsTo(meta.SequenceList[i], meta.FieldList, meta.SequenceList, out errorDescription))
                    {
                        result = false;
                        break;
                    }
                }

                if (meta.SequenceList.Count > 0 && meta.SequenceList.IndexOfRoot() < 0)
                {
                    meta.SequenceList[0].Root = true; // if none are marked as root, then first one is
                }
            }

            return result;
        }

        protected virtual bool ResolveSubstitutions(FtMeta meta, out string errorDescription)
        {
            bool result = true;
            errorDescription = "";

            for (int i = 0; i < substitutionList.Count; i++)
            {
                FtMetaSubstitution substitution = meta.SubstitutionList.New();
                if (!substitutionList[i].ResolveTo(substitution, out errorDescription))
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

        // properties

        private bool ResolveAllowEndOfLineCharInQuotes(FtMeta meta, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMeta.PropertyId.AllowEndOfLineCharInQuotes, out attributeValue))
                meta.AllowEndOfLineCharInQuotes = FtMeta.DefaultAllowEndOfLineCharInQuotes;
            else
            {
                bool propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    meta.AllowEndOfLineCharInQuotes = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldedTextReadElement_ResolveAllowEndOfLineCharInQuotes_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveCulture(FtMeta meta, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMeta.PropertyId.Culture, out attributeValue))
                meta.Culture = new CultureInfo(FtMeta.DefaultCultureName, false);
            else
            {
                try
                {
                    meta.Culture = new CultureInfo(attributeValue, false);
                }
                catch (CultureNotFoundException)
                {
                    errorDescription = string.Format(Properties.Resources.FieldedTextReadElement_ResolveCulture_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveDelimiterChar(FtMeta meta, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMeta.PropertyId.DelimiterChar, out attributeValue))
                meta.DelimiterChar = FtMeta.DefaultDelimiterChar;
            else
            {
                char propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    meta.DelimiterChar = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldedTextReadElement_ResolveDelimiterChar_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveEndOfLineAutoWriteType(FtMeta meta, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMeta.PropertyId.EndOfLineAutoWriteType, out attributeValue))
                meta.EndOfLineAutoWriteType = FtMeta.DefaultEndOfLineAutoWriteType;
            else
            {
                FtEndOfLineAutoWriteType propertyValue;
                if (EndOfLineAutoWriteTypeFormatter.TryParseAttributeValue(attributeValue, out propertyValue))
                    meta.EndOfLineAutoWriteType = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldedTextReadElement_ResolveEndOfLineAutoWriteType_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveEndOfLineChar(FtMeta meta, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMeta.PropertyId.EndOfLineChar, out attributeValue))
                meta.EndOfLineChar = FtMeta.DefaultEndOfLineChar;
            else
            {
                char propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    meta.EndOfLineChar = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldedTextReadElement_ResolveEndOfLineChar_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveEndOfLineType(FtMeta meta, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMeta.PropertyId.EndOfLineType, out attributeValue))
                meta.EndOfLineType = FtMeta.DefaultEndOfLineType;
            else
            {
                FtEndOfLineType propertyValue;
                if (EndOfLineTypeFormatter.TryParseAttributeValue(attributeValue, out propertyValue))
                    meta.EndOfLineType = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldedTextReadElement_ResolveEndOfLineType_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveLastLineEndedType(FtMeta meta, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMeta.PropertyId.LastLineEndedType, out attributeValue))
                meta.LastLineEndedType = FtMeta.DefaultLastLineEndedType;
            else
            {
                FtLastLineEndedType propertyValue;
                if (LastLineEndedTypeFormatter.TryParseAttributeValue(attributeValue, out propertyValue))
                    meta.LastLineEndedType = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldedTextReadElement_ResolveLastLineEndedType_Invalid, attributeValue);
                    result = false;
                }
                lastLineEndedTypeSpecified = true;
            }

            return result;
        }
        private bool ResolveHeadingAlwaysWriteOptionalQuote(FtMeta meta, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMeta.PropertyId.HeadingAlwaysWriteOptionalQuote, out attributeValue))
                meta.HeadingAlwaysWriteOptionalQuote = FtMeta.DefaultHeadingAlwaysWriteOptionalQuote;
            else
            {
                bool propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    meta.HeadingAlwaysWriteOptionalQuote = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldedTextReadElement_ResolveHeadingAlwaysWriteOptionalQuote_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveHeadingConstraint(FtMeta meta, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMeta.PropertyId.HeadingConstraint, out attributeValue))
                meta.HeadingConstraint = FtMeta.DefaultHeadingConstraint;
            else
            {
                FtHeadingConstraint propertyValue;
                if (HeadingConstraintFormatter.TryParseAttributeValue(attributeValue, out propertyValue))
                    meta.HeadingConstraint = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldedTextReadElement_ResolveHeadingConstraint_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveHeadingEndOfValueChar(FtMeta meta, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMeta.PropertyId.HeadingEndOfValueChar, out attributeValue))
                meta.HeadingEndOfValueChar = FtMeta.DefaultHeadingEndOfValueChar;
            else
            {
                char propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    meta.HeadingEndOfValueChar = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldedTextReadElement_ResolveHeadingEndOfValueChar_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveHeadingLineCount(FtMeta meta, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMeta.PropertyId.HeadingLineCount, out attributeValue))
                meta.HeadingLineCount = FtMeta.DefaultHeadingLineCount;
            else
            {
                int propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    meta.HeadingLineCount = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldedTextReadElement_ResolveHeadingLineCount_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveHeadingPadAlignment(FtMeta meta, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMeta.PropertyId.HeadingPadAlignment, out attributeValue))
                meta.HeadingPadAlignment = FtMeta.DefaultHeadingPadAlignment;
            else
            {
                FtPadAlignment propertyValue;
                if (PadAlignmentFormatter.TryParseAttributeValue(attributeValue, out propertyValue))
                    meta.HeadingPadAlignment = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldedTextReadElement_ResolveHeadingPadAlignment_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveHeadingPadChar(FtMeta meta, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMeta.PropertyId.HeadingPadChar, out attributeValue))
                meta.HeadingPadChar = FtMeta.DefaultHeadingPadChar;
            else
            {
                char propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    meta.HeadingPadChar = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldedTextReadElement_ResolveHeadingPadChar_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveHeadingPadCharType(FtMeta meta, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMeta.PropertyId.HeadingPadCharType, out attributeValue))
                meta.HeadingPadCharType = FtMeta.DefaultHeadingPadCharType;
            else
            {
                FtPadCharType propertyValue;
                if (PadCharTypeFormatter.TryParseAttributeValue(attributeValue, out propertyValue))
                    meta.HeadingPadCharType = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldedTextReadElement_ResolveHeadingPadCharType_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveHeadingQuotedType(FtMeta meta, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMeta.PropertyId.HeadingQuotedType, out attributeValue))
                meta.HeadingQuotedType = FtMeta.DefaultHeadingQuotedType;
            else
            {
                FtQuotedType propertyValue;
                if (QuotedTypeFormatter.TryParseAttributeValue(attributeValue, out propertyValue))
                    meta.HeadingQuotedType = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldedTextReadElement_ResolveHeadingQuotedType_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveHeadingTruncateChar(FtMeta meta, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMeta.PropertyId.HeadingTruncateChar, out attributeValue))
                meta.HeadingTruncateChar = FtMeta.DefaultHeadingTruncateChar;
            else
            {
                char propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    meta.HeadingTruncateChar = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldedTextReadElement_ResolveHeadingTruncateChar_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveHeadingTruncateType(FtMeta meta, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMeta.PropertyId.HeadingTruncateType, out attributeValue))
                meta.HeadingTruncateType = FtMeta.DefaultHeadingTruncateType;
            else
            {
                FtTruncateType propertyValue;
                if (TruncateTypeFormatter.TryParseAttributeValue(attributeValue, out propertyValue))
                    meta.HeadingTruncateType = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldedTextReadElement_ResolveHeadingTruncateType_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveHeadingWritePrefixSpace(FtMeta meta, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMeta.PropertyId.HeadingWritePrefixSpace, out attributeValue))
                meta.HeadingWritePrefixSpace = FtMeta.DefaultHeadingWritePrefixSpace;
            else
            {
                bool propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    meta.HeadingWritePrefixSpace = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldedTextReadElement_ResolveHeadingWritePrefixSpace_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveIgnoreBlankLines(FtMeta meta, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMeta.PropertyId.IgnoreBlankLines, out attributeValue))
                meta.IgnoreBlankLines = FtMeta.DefaultIgnoreBlankLines;
            else
            {
                bool propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    meta.IgnoreBlankLines = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldedTextReadElement_ResolveIgnoreBlankLines_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveIgnoreExtraChars(FtMeta meta, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMeta.PropertyId.IgnoreExtraChars, out attributeValue))
                meta.IgnoreExtraChars = FtMeta.DefaultIgnoreExtraChars;
            else
            {
                bool propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    meta.IgnoreExtraChars = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldedTextReadElement_ResolveIgnoreExtraChars_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveAllowIncompleteRecords(FtMeta meta, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMeta.PropertyId.AllowIncompleteRecords, out attributeValue))
                meta.AllowIncompleteRecords = FtMeta.DefaultAllowIncompleteRecords;
            else
            {
                bool propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    meta.AllowIncompleteRecords = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldedTextReadElement_ResolveAllowIncompleteRecords_Invalid, attributeValue);
                    result = false;
                }
                allowIncompleteRecordsSpecified = true;
            }

            return result;
        }
        private bool ResolveLineCommentChar(FtMeta meta, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMeta.PropertyId.LineCommentChar, out attributeValue))
                meta.LineCommentChar = FtMeta.DefaultLineCommentChar;
            else
            {
                char propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    meta.LineCommentChar = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldedTextReadElement_ResolveLineCommentChar_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveMainHeadingLineIndex(FtMeta meta, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMeta.PropertyId.MainHeadingLineIndex, out attributeValue))
                meta.MainHeadingLineIndex = FtMeta.DefaultMainHeadingLineIndex;
            else
            {
                int propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    meta.MainHeadingLineIndex = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldedTextReadElement_ResolveMainHeadingLineIndex_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveQuoteChar(FtMeta meta, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMeta.PropertyId.QuoteChar, out attributeValue))
                meta.QuoteChar = FtMeta.DefaultQuoteChar;
            else
            {
                char propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    meta.QuoteChar = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldedTextReadElement_ResolveQuoteChar_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveStuffedEmbeddedQuotes(FtMeta meta, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMeta.PropertyId.StuffedEmbeddedQuotes, out attributeValue))
                meta.StuffedEmbeddedQuotes = FtMeta.DefaultStuffedEmbeddedQuotes;
            else
            {
                bool propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    meta.StuffedEmbeddedQuotes = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldedTextReadElement_ResolveStuffedEmbeddedQuotes_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveSubstitutionChar(FtMeta meta, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMeta.PropertyId.SubstitutionChar, out attributeValue))
                meta.SubstitutionChar = FtMeta.DefaultSubstitutionChar;
            else
            {
                char propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    meta.SubstitutionChar = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldedTextReadElement_ResolveSubstitutionChar_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveSubstitutionsEnabled(FtMeta meta, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;
            string attributeValue;
            if (!attributeDictionary.TryGetValue(FtMeta.PropertyId.SubstitutionsEnabled, out attributeValue))
                meta.SubstitutionsEnabled = FtMeta.DefaultSubstitutionsEnabled;
            else
            {
                bool propertyValue;
                if (FtStandardText.TryParse(attributeValue, out propertyValue))
                    meta.SubstitutionsEnabled = propertyValue;
                else
                {
                    errorDescription = string.Format(Properties.Resources.FieldedTextReadElement_ResolveSubstitutionsEnabled_Invalid, attributeValue);
                    result = false;
                }
            }

            return result;
        }
        private bool ResolveLegacyEndOfLineIsSeparator(FtMeta meta, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;

            if (!lastLineEndedTypeSpecified)
            {
                string attributeValue;
                if (attributeDictionary.TryGetValue(FtMeta.PropertyId.LegacyEndOfLineIsSeparator, out attributeValue))
                {
                    bool propertyValue;
                    if (FtStandardText.TryParse(attributeValue, out propertyValue)) // ignore invalid property - will use default from ResolveLastLineEndedType
                    {
                        if (propertyValue)
                            meta.LastLineEndedType = FtLastLineEndedType.Never;
                        else
                            meta.LastLineEndedType = FtLastLineEndedType.Optional;
                    }
                }
            }

            return result;
        }
        private bool ResolveLegacyIncompleteRecordsAllowed(FtMeta meta, out string errorDescription)
        {
            errorDescription = "";
            bool result = true;

            if (!allowIncompleteRecordsSpecified)
            {
                string attributeValue;
                if (!attributeDictionary.TryGetValue(FtMeta.PropertyId.LegacyIncompleteRecordsAllowed, out attributeValue))
                    meta.AllowIncompleteRecords = FtMeta.DefaultAllowIncompleteRecords;
                else
                {
                    bool propertyValue;
                    if (FtStandardText.TryParse(attributeValue, out propertyValue))
                        meta.AllowIncompleteRecords = propertyValue;
                    else
                    {
                        errorDescription = string.Format(Properties.Resources.FieldedTextReadElement_ResolveLegacyIncompleteRecordsAllowed_Invalid, attributeValue);
                        result = false;
                    }
                }
            }

            return result;
        }
    }
}
