// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System.Xml;

namespace Xilytix.FieldedText.MetaSerialization
{
    using Formatting;

    public class XmlMetaSerializationWriter: MetaSerializationBase
    {
        private bool explicitIndex;

        internal protected bool ExplicitIndex { get { return explicitIndex; } set { explicitIndex = value; } }

        internal protected virtual void Write(XmlWriter writer, FtMeta meta)
        {
            writer.WriteStartDocument();
            Append(writer, meta);
            writer.WriteEndDocument();
        }

        internal protected virtual void Append(XmlWriter writer, FtMeta meta)
        {
            string errorMessage;
            if (!meta.Validate(out errorMessage))
                throw new FtMetaSerializationException(errorMessage);
            else
            {
                WriteStartElement(writer, MetaElementType.FieldedText);

                WriteRootProperties(writer, meta);

                for (int i = 0; i < meta.SubstitutionList.Count; i++)
                {
                    WriteSubstitution(writer, meta.SubstitutionList[i]);
                }

                for (int i = 0; i < meta.FieldList.Count; i++)
                {
                    WriteField(writer, meta.FieldList[i], i);
                }

                bool notRootSequenceSpecified = meta.SequenceList.IndexOfRoot() < 0;
                for (int i = 0; i < meta.SequenceList.Count; i++)
                {
                    bool assumeRoot = notRootSequenceSpecified && (i == 0);
                    WriteSequence(writer, meta.SequenceList[i], meta.FieldList, assumeRoot);
                }

                writer.WriteEndElement();
            }
        }

        protected virtual void WriteRootProperties(XmlWriter writer, FtMeta meta)
        {
            CheckWriteRootCulture(writer, meta);
            CheckWriteRootEndOfLineType(writer, meta);
            CheckWriteRootEndOfLineChar(writer, meta);
            CheckWriteRootEndOfLineAutoWriteType(writer, meta);
            CheckWriteRootLastLineEndedType(writer, meta);
            CheckWriteRootQuoteChar(writer, meta);
            CheckWriteRootDelimiterChar(writer, meta);
            CheckWriteRootLineCommentChar(writer, meta);
            CheckWriteRootAllowEndOfLineCharInQuotes(writer, meta);
            CheckWriteRootIgnoreBlankLines(writer, meta);
            CheckWriteRootIgnoreExtraChars(writer, meta);
            CheckWriteRootAllowIncompleteRecords(writer, meta);
            CheckWriteRootStuffedEmbeddedQuotes(writer, meta);
            CheckWriteRootSubstitutionChar(writer, meta);
            CheckWriteRootHeadingLineCount(writer, meta);
            CheckWriteRootMainHeadingLineIndex(writer, meta);
            CheckWriteRootHeadingConstraint(writer, meta);
            CheckWriteRootHeadingQuotedType(writer, meta);
            CheckWriteRootHeadingAlwaysWriteOptionalQuote(writer, meta);
            CheckWriteRootHeadingWritePrefixSpace(writer, meta);
            CheckWriteRootHeadingPadAlignment(writer, meta);
            CheckWriteRootHeadingPadCharType(writer, meta);
            CheckWriteRootHeadingPadChar(writer, meta);
            CheckWriteRootHeadingTruncateType(writer, meta);
            CheckWriteRootHeadingTruncateChar(writer, meta);
            CheckWriteRootHeadingEndOfValueChar(writer, meta);
        }

        protected virtual void WriteSubstitution(XmlWriter writer, FtMetaSubstitution substitution)
        {
            WriteStartElement(writer, MetaElementType.Substitution);
            WriteSubstitutionProperties(writer, substitution);
            writer.WriteEndElement();
        }

        protected virtual void WriteSubstitutionProperties(XmlWriter writer, FtMetaSubstitution substitution)
        {
            WriteSubstitutionToken(writer, substitution);
            CheckWriteSubstitutionType(writer, substitution);
            CheckWriteSubstitutionValue(writer, substitution);
        }

        protected virtual void WriteField(XmlWriter writer, FtMetaField field, int index)
        {
            WriteStartElement(writer, MetaElementType.Field);
            WriteFieldProperties(writer, field, index);
            writer.WriteEndElement();
        }

        protected virtual void WriteFieldProperties(XmlWriter writer, FtMetaField field, int index)
        {
            CheckWriteFieldIndex(writer, index);
            WriteFieldName(writer, field);
            CheckWriteFieldId(writer, field);
            CheckWriteFieldDataType(writer, field);
            CheckWriteFieldFixedWidth(writer, field);
            CheckWriteFieldWidth(writer, field);
            CheckWriteFieldConstant(writer, field);
            CheckWriteFieldNull(writer, field);
            CheckWriteFieldHeadings(writer, field);
            CheckWriteFieldHeadingConstraint(writer, field);
            CheckWriteFieldValueQuotedType(writer, field);
            CheckWriteFieldValueAlwaysWriteOptionalQuote(writer, field);
            CheckWriteFieldValueWritePrefixSpace(writer, field);
            CheckWriteFieldValuePadAlignment(writer, field);
            CheckWriteFieldValuePadCharType(writer, field);
            CheckWriteFieldValuePadChar(writer, field);
            CheckWriteFieldValueTruncateType(writer, field);
            CheckWriteFieldValueTruncateChar(writer, field);
            CheckWriteFieldValueEndOfValueChar(writer, field);
            CheckWriteFieldValueNullChar(writer, field);
            CheckWriteFieldHeadingQuotedType(writer, field);
            CheckWriteFieldHeadingAlwaysWriteOptionalQuote(writer, field);
            CheckWriteFieldHeadingWritePrefixSpace(writer, field);
            CheckWriteFieldHeadingPadAlignment(writer, field);
            CheckWriteFieldHeadingPadCharType(writer, field);
            CheckWriteFieldHeadingPadChar(writer, field);
            CheckWriteFieldHeadingTruncateType(writer, field);
            CheckWriteFieldHeadingTruncateChar(writer, field);
            CheckWriteFieldHeadingEndOfValueChar(writer, field);
            switch (field.DataType)
            {
                case FtStandardDataType.String:
                    FtStringMetaField stringField = (FtStringMetaField)field;
                    CheckWriteStringFieldValue(writer, stringField);
                    break;
                case FtStandardDataType.Boolean:
                    FtBooleanMetaField booleanField = (FtBooleanMetaField)field;
                    CheckWriteBooleanFieldValue(writer, booleanField);
                    CheckWriteBooleanFieldStyles(writer, booleanField);
                    CheckWriteBooleanFieldFalseText(writer, booleanField);
                    CheckWriteBooleanFieldTrueText(writer, booleanField);
                    break;
                case FtStandardDataType.Integer:
                    FtIntegerMetaField integerField = (FtIntegerMetaField)field;
                    CheckWriteIntegerFieldValue(writer, integerField);
                    CheckWriteIntegerFieldStyles(writer, integerField);
                    CheckWriteIntegerFieldFormat(writer, integerField);
                    break;
                case FtStandardDataType.Float:
                    FtFloatMetaField floatField = (FtFloatMetaField)field;
                    CheckWriteFloatFieldValue(writer, floatField);
                    CheckWriteFloatFieldStyles(writer, floatField);
                    CheckWriteFloatFieldFormat(writer, floatField);
                    break;
                case FtStandardDataType.Decimal:
                    FtDecimalMetaField decimalField = (FtDecimalMetaField)field;
                    CheckWriteDecimalFieldValue(writer, decimalField);
                    CheckWriteDecimalFieldStyles(writer, decimalField);
                    CheckWriteDecimalFieldFormat(writer, decimalField);
                    break;
                case FtStandardDataType.DateTime:
                    FtDateTimeMetaField dateTimeField = (FtDateTimeMetaField)field;
                    CheckWriteDateTimeFieldValue(writer, dateTimeField);
                    CheckWriteDateTimeFieldStyles(writer, dateTimeField);
                    CheckWriteDateTimeFieldFormat(writer, dateTimeField);
                    break;
            }
        }

        protected virtual void WriteSequence(XmlWriter writer, FtMetaSequence sequence, FtMetaFieldList fieldList, bool assumeRoot)
        {
            WriteStartElement(writer, MetaElementType.Sequence);

            FtMetaSequenceItemList itemList = sequence.ItemList;
            int fieldIndicesFromItemIdx;
            if (ExplicitIndex)
                fieldIndicesFromItemIdx = itemList.Count;
            else
            {
                fieldIndicesFromItemIdx = 0;

                for (int i = itemList.Count - 1; i >= 0; i--)
                {
                    FtMetaSequenceItem item = itemList[i];
                    if (item.RedirectList.Count > 0)
                    {
                        fieldIndicesFromItemIdx = i + 1;
                        break;
                    }
                }
            }

            WriteSequenceProperties(writer, sequence, fieldIndicesFromItemIdx, fieldList, assumeRoot);

            for (int i = 0; i < fieldIndicesFromItemIdx; i++)
            {
                WriteSequenceItem(writer, itemList[i], i, fieldList);
            }

            writer.WriteEndElement();
        }

        protected virtual void WriteSequenceProperties(XmlWriter writer, 
                                                       FtMetaSequence sequence, 
                                                       int fieldIndicesFromItemIdx, 
                                                       FtMetaFieldList fieldList, 
                                                       bool assumeRoot)
        {
            WriteSequenceName(writer, sequence);
            CheckWriteSequenceRoot(writer, sequence, assumeRoot);
            CheckWriteSequenceFieldIndices(writer, sequence.ItemList, fieldIndicesFromItemIdx, fieldList);
        }

        protected virtual void WriteSequenceItem(XmlWriter writer, FtMetaSequenceItem sequenceItem, int index, FtMetaFieldList fieldList)
        {
            WriteStartElement(writer, MetaElementType.SequenceItem);
            WriteSequenceItemProperties(writer, sequenceItem, index, fieldList);

            for (int i = 0; i < sequenceItem.RedirectList.Count; i++)
            {
                WriteSequenceRedirect(writer, sequenceItem.RedirectList[i], i, sequenceItem.Field);
            }

            writer.WriteEndElement();
        }

        protected virtual void WriteSequenceItemProperties(XmlWriter writer, FtMetaSequenceItem sequenceItem, int index, FtMetaFieldList fieldList)
        {
            CheckWriteSequenceItemIndex(writer, index);
            WriteSequenceItemFieldIndex(writer, sequenceItem, fieldList);
        }

        protected virtual void WriteSequenceRedirect(XmlWriter writer, FtMetaSequenceRedirect sequenceRedirect, int index, FtMetaField field)
        {
            WriteStartElement(writer, MetaElementType.SequenceRedirect);
            WriteSequenceRedirectProperties(writer, sequenceRedirect, index, field);
            writer.WriteEndElement();
        }

        protected virtual void WriteSequenceRedirectProperties(XmlWriter writer, FtMetaSequenceRedirect sequenceRedirect, int index, FtMetaField field)
        {
            CheckWriteSequenceRedirectIndex(writer, index);
            WriteSequenceRedirectSequenceName(writer, sequenceRedirect, field);
            CheckWriteSequenceRedirectType(writer, sequenceRedirect, field);
            CheckWriteSequenceRedirectInvokationDelay(writer, sequenceRedirect);
            switch (sequenceRedirect.Type)
            {
                case FtStandardSequenceRedirectType.ExactString:
                    FtExactStringMetaSequenceRedirect exactStringRedirect = (FtExactStringMetaSequenceRedirect)sequenceRedirect;
                    WriteExactStringSequenceRedirectValue(writer, exactStringRedirect);
                    break;
                case FtStandardSequenceRedirectType.CaseInsensitiveString:
                    FtCaseInsensitiveStringMetaSequenceRedirect caseInsensitiveStringRedirect = (FtCaseInsensitiveStringMetaSequenceRedirect)sequenceRedirect;
                    WriteCaseInsensitiveStringSequenceRedirectValue(writer, caseInsensitiveStringRedirect);
                    break;
                case FtStandardSequenceRedirectType.Boolean:
                    FtBooleanMetaSequenceRedirect booleanRedirect = (FtBooleanMetaSequenceRedirect)sequenceRedirect;
                    WriteBooleanSequenceRedirectValue(writer, booleanRedirect);
                    break;
                case FtStandardSequenceRedirectType.ExactInteger:
                    FtExactIntegerMetaSequenceRedirect exactIntegerRedirect = (FtExactIntegerMetaSequenceRedirect)sequenceRedirect;
                    WriteExactIntegerSequenceRedirectValue(writer, exactIntegerRedirect);
                    break;
                case FtStandardSequenceRedirectType.ExactFloat:
                    FtExactFloatMetaSequenceRedirect exactFloatRedirect = (FtExactFloatMetaSequenceRedirect)sequenceRedirect;
                    WriteExactFloatSequenceRedirectValue(writer, exactFloatRedirect);
                    break;
                case FtStandardSequenceRedirectType.ExactDateTime:
                    FtExactDateTimeMetaSequenceRedirect exactDateTimeRedirect = (FtExactDateTimeMetaSequenceRedirect)sequenceRedirect;
                    WriteExactDateTimeSequenceRedirectValue(writer, exactDateTimeRedirect);
                    break;
                case FtStandardSequenceRedirectType.Date:
                    FtDateMetaSequenceRedirect dateRedirect = (FtDateMetaSequenceRedirect)sequenceRedirect;
                    WriteDateSequenceRedirectValue(writer, dateRedirect);
                    break;
                case FtStandardSequenceRedirectType.ExactDecimal:
                    FtExactDecimalMetaSequenceRedirect exactDecimalRedirect = (FtExactDecimalMetaSequenceRedirect)sequenceRedirect;
                    WriteExactDecimalSequenceRedirectValue(writer, exactDecimalRedirect);
                    break;
            }
        }

        private void WriteStartElement(XmlWriter writer, MetaElementType elementType)
        {
            writer.WriteStartElement(MetaElementTypeFormatter.ToElementName(elementType));
        }

        private void WriteAttribute(XmlWriter writer, string name, string value)
        {
            writer.WriteStartAttribute(name);
            writer.WriteString(value);
            writer.WriteEndAttribute();
        }

        // Root Properties

        private void CheckWriteRootCulture(XmlWriter writer, FtMeta meta)
        {
            string cultureName = meta.Culture.Name;
            if (cultureName != MetaSerializationDefaults.Root.CultureName)
            {
                WriteAttribute(writer, RootPropertyIdFormatter.ToAttributeName(FtMeta.PropertyId.Culture), cultureName);
            }
        }
        private void CheckWriteRootEndOfLineType(XmlWriter writer, FtMeta meta)
        {
            if (meta.EndOfLineType != MetaSerializationDefaults.Root.EndOfLineType)
            {
                WriteAttribute(writer, 
                               RootPropertyIdFormatter.ToAttributeName(FtMeta.PropertyId.EndOfLineType), 
                               EndOfLineTypeFormatter.ToAttributeValue(meta.EndOfLineType));
            }
        }
        private void CheckWriteRootEndOfLineChar(XmlWriter writer, FtMeta meta)
        {
            if (meta.EndOfLineChar != MetaSerializationDefaults.Root.EndOfLineChar)
            {
                WriteAttribute(writer,
                               RootPropertyIdFormatter.ToAttributeName(FtMeta.PropertyId.EndOfLineChar),
                               FtStandardText.Get(meta.EndOfLineChar));
            }
        }
        private void CheckWriteRootEndOfLineAutoWriteType(XmlWriter writer, FtMeta meta)
        {
            if (meta.EndOfLineAutoWriteType != MetaSerializationDefaults.Root.EndOfLineAutoWriteType)
            {
                WriteAttribute(writer,
                               RootPropertyIdFormatter.ToAttributeName(FtMeta.PropertyId.EndOfLineAutoWriteType),
                               EndOfLineAutoWriteTypeFormatter.ToAttributeValue(meta.EndOfLineAutoWriteType));
            }
        }
        private void CheckWriteRootLastLineEndedType(XmlWriter writer, FtMeta meta)
        {
            if (meta.LastLineEndedType != MetaSerializationDefaults.Root.LastLineEndedType)
            {
                WriteAttribute(writer,
                               RootPropertyIdFormatter.ToAttributeName(FtMeta.PropertyId.LastLineEndedType),
                               LastLineEndedTypeFormatter.ToAttributeValue(meta.LastLineEndedType));
            }
        }
        private void CheckWriteRootQuoteChar(XmlWriter writer, FtMeta meta)
        {
            if (meta.QuoteChar != MetaSerializationDefaults.Root.QuoteChar)
            {
                WriteAttribute(writer,
                               RootPropertyIdFormatter.ToAttributeName(FtMeta.PropertyId.QuoteChar),
                               FtStandardText.Get(meta.QuoteChar));
            }
        }
        private void CheckWriteRootDelimiterChar(XmlWriter writer, FtMeta meta)
        {
            if (meta.DelimiterChar != MetaSerializationDefaults.Root.DelimiterChar)
            {
                WriteAttribute(writer,
                               RootPropertyIdFormatter.ToAttributeName(FtMeta.PropertyId.DelimiterChar),
                               FtStandardText.Get(meta.DelimiterChar));
            }
        }
        private void CheckWriteRootLineCommentChar(XmlWriter writer, FtMeta meta)
        {
            if (meta.LineCommentChar != MetaSerializationDefaults.Root.LineCommentChar)
            {
                WriteAttribute(writer,
                               RootPropertyIdFormatter.ToAttributeName(FtMeta.PropertyId.LineCommentChar),
                               FtStandardText.Get(meta.LineCommentChar));
            }
        }
        private void CheckWriteRootAllowEndOfLineCharInQuotes(XmlWriter writer, FtMeta meta)
        {
            if (meta.AllowEndOfLineCharInQuotes != MetaSerializationDefaults.Root.AllowEndOfLineCharInQuotes)
            {
                WriteAttribute(writer,
                               RootPropertyIdFormatter.ToAttributeName(FtMeta.PropertyId.AllowEndOfLineCharInQuotes),
                               FtStandardText.Get(meta.AllowEndOfLineCharInQuotes));
            }
        }
        private void CheckWriteRootIgnoreBlankLines(XmlWriter writer, FtMeta meta)
        {
            if (meta.IgnoreBlankLines != MetaSerializationDefaults.Root.IgnoreBlankLines)
            {
                WriteAttribute(writer,
                               RootPropertyIdFormatter.ToAttributeName(FtMeta.PropertyId.IgnoreBlankLines),
                               FtStandardText.Get(meta.IgnoreBlankLines));
            }
        }
        private void CheckWriteRootIgnoreExtraChars(XmlWriter writer, FtMeta meta)
        {
            if (meta.IgnoreExtraChars != MetaSerializationDefaults.Root.IgnoreExtraChars)
            {
                WriteAttribute(writer,
                               RootPropertyIdFormatter.ToAttributeName(FtMeta.PropertyId.IgnoreExtraChars),
                               FtStandardText.Get(meta.IgnoreExtraChars));
            }
        }
        private void CheckWriteRootAllowIncompleteRecords(XmlWriter writer, FtMeta meta)
        {
            if (meta.AllowIncompleteRecords != MetaSerializationDefaults.Root.AllowIncompleteRecords)
            {
                WriteAttribute(writer,
                               RootPropertyIdFormatter.ToAttributeName(FtMeta.PropertyId.AllowIncompleteRecords),
                               FtStandardText.Get(meta.AllowIncompleteRecords));
            }
        }
        private void CheckWriteRootStuffedEmbeddedQuotes(XmlWriter writer, FtMeta meta)
        {
            if (meta.StuffedEmbeddedQuotes != MetaSerializationDefaults.Root.StuffedEmbeddedQuotes)
            {
                WriteAttribute(writer,
                               RootPropertyIdFormatter.ToAttributeName(FtMeta.PropertyId.StuffedEmbeddedQuotes),
                               FtStandardText.Get(meta.StuffedEmbeddedQuotes));
            }
        }
        private void CheckWriteRootSubstitutionChar(XmlWriter writer, FtMeta meta)
        {
            if (meta.SubstitutionChar != MetaSerializationDefaults.Root.SubstitutionChar)
            {
                WriteAttribute(writer,
                               RootPropertyIdFormatter.ToAttributeName(FtMeta.PropertyId.SubstitutionChar),
                               FtStandardText.Get(meta.SubstitutionChar));
            }
        }
        private void CheckWriteRootHeadingLineCount(XmlWriter writer, FtMeta meta)
        {
            if (meta.HeadingLineCount != MetaSerializationDefaults.Root.HeadingLineCount)
            {
                WriteAttribute(writer,
                               RootPropertyIdFormatter.ToAttributeName(FtMeta.PropertyId.HeadingLineCount),
                               FtStandardText.Get(meta.HeadingLineCount));
            }
        }
        private void CheckWriteRootMainHeadingLineIndex(XmlWriter writer, FtMeta meta)
        {
            if (meta.MainHeadingLineIndex != MetaSerializationDefaults.Root.MainHeadingLineIndex)
            {
                WriteAttribute(writer,
                               RootPropertyIdFormatter.ToAttributeName(FtMeta.PropertyId.MainHeadingLineIndex),
                               FtStandardText.Get(meta.MainHeadingLineIndex));
            }
        }
        private void CheckWriteRootHeadingConstraint(XmlWriter writer, FtMeta meta)
        {
            if (meta.HeadingConstraint != MetaSerializationDefaults.Root.HeadingConstraint)
            {
                WriteAttribute(writer,
                               RootPropertyIdFormatter.ToAttributeName(FtMeta.PropertyId.HeadingConstraint),
                               HeadingConstraintFormatter.ToAttributeValue(meta.HeadingConstraint));
            }
        }
        private void CheckWriteRootHeadingQuotedType(XmlWriter writer, FtMeta meta)
        {
            if (meta.HeadingQuotedType != MetaSerializationDefaults.Root.HeadingQuotedType)
            {
                WriteAttribute(writer,
                               RootPropertyIdFormatter.ToAttributeName(FtMeta.PropertyId.HeadingQuotedType),
                               QuotedTypeFormatter.ToAttributeValue(meta.HeadingQuotedType));
            }
        }
        private void CheckWriteRootHeadingAlwaysWriteOptionalQuote(XmlWriter writer, FtMeta meta)
        {
            if (meta.HeadingAlwaysWriteOptionalQuote != MetaSerializationDefaults.Root.HeadingAlwaysWriteOptionalQuote)
            {
                WriteAttribute(writer,
                               RootPropertyIdFormatter.ToAttributeName(FtMeta.PropertyId.HeadingAlwaysWriteOptionalQuote),
                               FtStandardText.Get(meta.HeadingAlwaysWriteOptionalQuote));
            }
        }
        private void CheckWriteRootHeadingWritePrefixSpace(XmlWriter writer, FtMeta meta)
        {
            if (meta.HeadingWritePrefixSpace != MetaSerializationDefaults.Root.HeadingWritePrefixSpace)
            {
                WriteAttribute(writer,
                               RootPropertyIdFormatter.ToAttributeName(FtMeta.PropertyId.HeadingWritePrefixSpace),
                               FtStandardText.Get(meta.HeadingWritePrefixSpace));
            }
        }
        private void CheckWriteRootHeadingPadAlignment(XmlWriter writer, FtMeta meta)
        {
            if (meta.HeadingPadAlignment != MetaSerializationDefaults.Root.HeadingPadAlignment)
            {
                WriteAttribute(writer,
                               RootPropertyIdFormatter.ToAttributeName(FtMeta.PropertyId.HeadingPadAlignment),
                               PadAlignmentFormatter.ToAttributeValue(meta.HeadingPadAlignment));
            }
        }
        private void CheckWriteRootHeadingPadCharType(XmlWriter writer, FtMeta meta)
        {
            if (meta.HeadingPadCharType != MetaSerializationDefaults.Root.HeadingPadCharType)
            {
                WriteAttribute(writer,
                               RootPropertyIdFormatter.ToAttributeName(FtMeta.PropertyId.HeadingPadCharType),
                               PadCharTypeFormatter.ToAttributeValue(meta.HeadingPadCharType));
            }
        }
        private void CheckWriteRootHeadingPadChar(XmlWriter writer, FtMeta meta)
        {
            if (meta.HeadingPadChar != MetaSerializationDefaults.Root.HeadingPadChar)
            {
                WriteAttribute(writer,
                               RootPropertyIdFormatter.ToAttributeName(FtMeta.PropertyId.HeadingPadChar),
                               FtStandardText.Get(meta.HeadingPadChar));
            }
        }
        private void CheckWriteRootHeadingTruncateType(XmlWriter writer, FtMeta meta)
        {
            if (meta.HeadingTruncateType != MetaSerializationDefaults.Root.HeadingTruncateType)
            {
                WriteAttribute(writer,
                               RootPropertyIdFormatter.ToAttributeName(FtMeta.PropertyId.HeadingTruncateType),
                               TruncateTypeFormatter.ToAttributeValue(meta.HeadingTruncateType));
            }
        }
        private void CheckWriteRootHeadingTruncateChar(XmlWriter writer, FtMeta meta)
        {
            if (meta.HeadingTruncateChar != MetaSerializationDefaults.Root.HeadingTruncateChar)
            {
                WriteAttribute(writer,
                               RootPropertyIdFormatter.ToAttributeName(FtMeta.PropertyId.HeadingTruncateChar),
                               FtStandardText.Get(meta.HeadingTruncateChar));
            }
        }
        private void CheckWriteRootHeadingEndOfValueChar(XmlWriter writer, FtMeta meta)
        {
            if (meta.HeadingEndOfValueChar != MetaSerializationDefaults.Root.HeadingEndOfValueChar)
            {
                WriteAttribute(writer,
                               RootPropertyIdFormatter.ToAttributeName(FtMeta.PropertyId.HeadingEndOfValueChar),
                               FtStandardText.Get(meta.HeadingEndOfValueChar));
            }
        }

        // Field Properties

        private void CheckWriteFieldIndex(XmlWriter writer, int index)
        {
            if (ExplicitIndex)
            {
                WriteAttribute(writer,
                               FieldPropertyIdFormatter.ToAttributeName(FtMetaField.PropertyId.Index),
                               FtStandardText.Get(index));
            }
        }
        private void WriteFieldName(XmlWriter writer, FtMetaField field)
        {
            WriteAttribute(writer,
                           FieldPropertyIdFormatter.ToAttributeName(FtMetaField.PropertyId.Name),
                           FtStandardText.Get(field.Name));
        }
        private void CheckWriteFieldId(XmlWriter writer, FtMetaField field)
        {
            if (field.Id != MetaSerializationDefaults.Field.Id)
            {
                WriteAttribute(writer,
                               FieldPropertyIdFormatter.ToAttributeName(FtMetaField.PropertyId.Id),
                               FtStandardText.Get(field.Id));
            }
        }
        private void CheckWriteFieldDataType(XmlWriter writer, FtMetaField field)
        {
            if (field.DataType != MetaSerializationDefaults.Field.DataType)
            {
                WriteAttribute(writer,
                               FieldPropertyIdFormatter.ToAttributeName(FtMetaField.PropertyId.DataType),
                               DataTypeFormatter.ToAttributeValue(field.DataType));
            }
        }
        private void CheckWriteFieldFixedWidth(XmlWriter writer, FtMetaField field)
        {
            if (field.FixedWidth != MetaSerializationDefaults.Field.FixedWidth)
            {
                WriteAttribute(writer,
                               FieldPropertyIdFormatter.ToAttributeName(FtMetaField.PropertyId.FixedWidth),
                               FtStandardText.Get(field.FixedWidth));
            }
        }
        private void CheckWriteFieldWidth(XmlWriter writer, FtMetaField field)
        {
            if (field.Width != MetaSerializationDefaults.Field.Width)
            {
                WriteAttribute(writer,
                               FieldPropertyIdFormatter.ToAttributeName(FtMetaField.PropertyId.Width),
                               FtStandardText.Get(field.Width));
            }
        }
        private void CheckWriteFieldConstant(XmlWriter writer, FtMetaField field)
        {
            if (field.Constant != MetaSerializationDefaults.Field.Constant)
            {
                WriteAttribute(writer,
                               FieldPropertyIdFormatter.ToAttributeName(FtMetaField.PropertyId.Constant),
                               FtStandardText.Get(field.Constant));
            }
        }
        private void CheckWriteFieldNull(XmlWriter writer, FtMetaField field)
        {
            if (field.Constant && field.Null != MetaSerializationDefaults.Field.Null)
            {
                WriteAttribute(writer,
                               FieldPropertyIdFormatter.ToAttributeName(FtMetaField.PropertyId.Null),
                               FtStandardText.Get(field.Null));
            }
        }
        private void CheckWriteFieldValueQuotedType(XmlWriter writer, FtMetaField field)
        {
            if (field.ValueQuotedType != MetaSerializationDefaults.Field.ValueQuotedType)
            {
                WriteAttribute(writer,
                               FieldPropertyIdFormatter.ToAttributeName(FtMetaField.PropertyId.ValueQuotedType),
                               QuotedTypeFormatter.ToAttributeValue(field.ValueQuotedType));
            }
        }
        private void CheckWriteFieldValueAlwaysWriteOptionalQuote(XmlWriter writer, FtMetaField field)
        {
            if (field.ValueAlwaysWriteOptionalQuote != MetaSerializationDefaults.Field.ValueAlwaysWriteOptionalQuote)
            {
                WriteAttribute(writer,
                               FieldPropertyIdFormatter.ToAttributeName(FtMetaField.PropertyId.ValueAlwaysWriteOptionalQuote),
                               FtStandardText.Get(field.ValueAlwaysWriteOptionalQuote));
            }
        }
        private void CheckWriteFieldValueWritePrefixSpace(XmlWriter writer, FtMetaField field)
        {
            if (field.ValueWritePrefixSpace != MetaSerializationDefaults.Field.ValueWritePrefixSpace)
            {
                WriteAttribute(writer,
                               FieldPropertyIdFormatter.ToAttributeName(FtMetaField.PropertyId.ValueWritePrefixSpace),
                               FtStandardText.Get(field.ValueWritePrefixSpace));
            }
        }
        private void CheckWriteFieldValuePadAlignment(XmlWriter writer, FtMetaField field)
        {
            if (field.ValuePadAlignment != MetaSerializationDefaults.Field.ValuePadAlignment)
            {
                WriteAttribute(writer,
                               FieldPropertyIdFormatter.ToAttributeName(FtMetaField.PropertyId.ValuePadAlignment),
                               PadAlignmentFormatter.ToAttributeValue(field.ValuePadAlignment));
            }
        }
        private void CheckWriteFieldValuePadCharType(XmlWriter writer, FtMetaField field)
        {
            if (field.ValuePadCharType != MetaSerializationDefaults.Field.ValuePadCharType)
            {
                WriteAttribute(writer,
                               FieldPropertyIdFormatter.ToAttributeName(FtMetaField.PropertyId.ValuePadCharType),
                               PadCharTypeFormatter.ToAttributeValue(field.ValuePadCharType));
            }
        }
        private void CheckWriteFieldValuePadChar(XmlWriter writer, FtMetaField field)
        {
            if (field.ValuePadChar != MetaSerializationDefaults.Field.ValuePadChar)
            {
                WriteAttribute(writer,
                               FieldPropertyIdFormatter.ToAttributeName(FtMetaField.PropertyId.ValuePadChar),
                               FtStandardText.Get(field.ValuePadChar));
            }
        }
        private void CheckWriteFieldValueTruncateType(XmlWriter writer, FtMetaField field)
        {
            if (field.ValueTruncateType != MetaSerializationDefaults.Field.ValueTruncateType)
            {
                WriteAttribute(writer,
                               FieldPropertyIdFormatter.ToAttributeName(FtMetaField.PropertyId.ValueTruncateType),
                               TruncateTypeFormatter.ToAttributeValue(field.ValueTruncateType));
            }
        }
        private void CheckWriteFieldValueTruncateChar(XmlWriter writer, FtMetaField field)
        {
            if (field.ValueTruncateChar != MetaSerializationDefaults.Field.ValueTruncateChar)
            {
                WriteAttribute(writer,
                               FieldPropertyIdFormatter.ToAttributeName(FtMetaField.PropertyId.ValueTruncateChar),
                               FtStandardText.Get(field.ValueTruncateChar));
            }
        }
        private void CheckWriteFieldValueEndOfValueChar(XmlWriter writer, FtMetaField field)
        {
            if (field.ValueEndOfValueChar != MetaSerializationDefaults.Field.ValueEndOfValueChar)
            {
                WriteAttribute(writer,
                               FieldPropertyIdFormatter.ToAttributeName(FtMetaField.PropertyId.ValueEndOfValueChar),
                               FtStandardText.Get(field.ValueEndOfValueChar));
            }
        }
        private void CheckWriteFieldValueNullChar(XmlWriter writer, FtMetaField field)
        {
            if (field.ValueNullChar != MetaSerializationDefaults.Field.ValueNullChar)
            {
                WriteAttribute(writer,
                               FieldPropertyIdFormatter.ToAttributeName(FtMetaField.PropertyId.ValueNullChar),
                               FtStandardText.Get(field.ValueNullChar));
            }
        }
        private void CheckWriteFieldHeadings(XmlWriter writer, FtMetaField field)
        {
            int trimLength = 0;
            if (field.Headings != null)
            {
                for (int i = field.HeadingCount - 1; i >= 0; i--)
                {
                    if (field.Headings[i] != null && field.Headings[i].Length > 0)
                    {
                        trimLength = i + 1;
                        break;
                    }
                }
            }

            if (trimLength > 0)
            {
                WriteAttribute(writer,
                               FieldPropertyIdFormatter.ToAttributeName(FtMetaField.PropertyId.Headings),
                               FtCommaText.Get(field.Headings, 0,  trimLength));
            }
        }
        private void CheckWriteFieldHeadingConstraint(XmlWriter writer, FtMetaField field)
        {
            if (field.HeadingConstraint != field.DefaultHeadingConstraint)
            {
                WriteAttribute(writer,
                               FieldPropertyIdFormatter.ToAttributeName(FtMetaField.PropertyId.HeadingConstraint),
                               HeadingConstraintFormatter.ToAttributeValue(field.HeadingConstraint));
            }
        }
        private void CheckWriteFieldHeadingQuotedType(XmlWriter writer, FtMetaField field)
        {
            if (field.HeadingQuotedType != field.DefaultHeadingQuotedType)
            {
                WriteAttribute(writer,
                               FieldPropertyIdFormatter.ToAttributeName(FtMetaField.PropertyId.HeadingQuotedType),
                               QuotedTypeFormatter.ToAttributeValue(field.HeadingQuotedType));
            }
        }
        private void CheckWriteFieldHeadingAlwaysWriteOptionalQuote(XmlWriter writer, FtMetaField field)
        {
            if (field.HeadingAlwaysWriteOptionalQuote != field.DefaultHeadingAlwaysWriteOptionalQuote)
            {
                WriteAttribute(writer,
                               FieldPropertyIdFormatter.ToAttributeName(FtMetaField.PropertyId.HeadingAlwaysWriteOptionalQuote),
                               FtStandardText.Get(field.HeadingAlwaysWriteOptionalQuote));
            }
        }
        private void CheckWriteFieldHeadingWritePrefixSpace(XmlWriter writer, FtMetaField field)
        {
            if (field.HeadingWritePrefixSpace != field.DefaultHeadingWritePrefixSpace)
            {
                WriteAttribute(writer,
                               FieldPropertyIdFormatter.ToAttributeName(FtMetaField.PropertyId.HeadingWritePrefixSpace),
                               FtStandardText.Get(field.HeadingWritePrefixSpace));
            }
        }
        private void CheckWriteFieldHeadingPadAlignment(XmlWriter writer, FtMetaField field)
        {
            if (field.HeadingPadAlignment != field.DefaultHeadingPadAlignment)
            {
                WriteAttribute(writer,
                               FieldPropertyIdFormatter.ToAttributeName(FtMetaField.PropertyId.HeadingPadAlignment),
                               PadAlignmentFormatter.ToAttributeValue(field.HeadingPadAlignment));
            }
        }
        private void CheckWriteFieldHeadingPadCharType(XmlWriter writer, FtMetaField field)
        {
            if (field.HeadingPadCharType != field.DefaultHeadingPadCharType)
            {
                WriteAttribute(writer,
                               FieldPropertyIdFormatter.ToAttributeName(FtMetaField.PropertyId.HeadingPadCharType),
                               PadCharTypeFormatter.ToAttributeValue(field.HeadingPadCharType));
            }
        }
        private void CheckWriteFieldHeadingPadChar(XmlWriter writer, FtMetaField field)
        {
            if (field.HeadingPadChar != field.DefaultHeadingPadChar)
            {
                WriteAttribute(writer,
                               FieldPropertyIdFormatter.ToAttributeName(FtMetaField.PropertyId.HeadingPadChar),
                               FtStandardText.Get(field.HeadingPadChar));
            }
        }
        private void CheckWriteFieldHeadingTruncateType(XmlWriter writer, FtMetaField field)
        {
            if (field.HeadingTruncateType != field.DefaultHeadingTruncateType)
            {
                WriteAttribute(writer,
                               FieldPropertyIdFormatter.ToAttributeName(FtMetaField.PropertyId.HeadingTruncateType),
                               TruncateTypeFormatter.ToAttributeValue(field.HeadingTruncateType));
            }
        }
        private void CheckWriteFieldHeadingTruncateChar(XmlWriter writer, FtMetaField field)
        {
            if (field.HeadingTruncateChar != field.DefaultHeadingTruncateChar)
            {
                WriteAttribute(writer,
                               FieldPropertyIdFormatter.ToAttributeName(FtMetaField.PropertyId.HeadingTruncateChar),
                               FtStandardText.Get(field.HeadingTruncateChar));
            }
        }
        private void CheckWriteFieldHeadingEndOfValueChar(XmlWriter writer, FtMetaField field)
        {
            if (field.HeadingEndOfValueChar != field.DefaultHeadingEndOfValueChar)
            {
                WriteAttribute(writer,
                               FieldPropertyIdFormatter.ToAttributeName(FtMetaField.PropertyId.HeadingEndOfValueChar),
                               FtStandardText.Get(field.HeadingEndOfValueChar));
            }
        }

        private void CheckWriteStringFieldValue(XmlWriter writer, FtStringMetaField field)
        {
            if (field.Constant && !field.Null)
            {
                WriteAttribute(writer,
                               FieldPropertyIdFormatter.ToAttributeName(FtMetaField.PropertyId.Value),
                               field.Value);
            }
        }
        private void CheckWriteBooleanFieldValue(XmlWriter writer, FtBooleanMetaField field)
        {
            if (field.Constant && !field.Null)
            {
                WriteAttribute(writer,
                               FieldPropertyIdFormatter.ToAttributeName(FtMetaField.PropertyId.Value),
                               FtStandardText.Get(field.Value));
            }
        }
        private void CheckWriteBooleanFieldStyles(XmlWriter writer, FtBooleanMetaField field)
        {
            if (field.Styles != MetaSerializationDefaults.BooleanField.Styles)
            {
                WriteAttribute(writer,
                               FieldPropertyIdFormatter.ToAttributeName(FtMetaField.PropertyId.Styles),
                               BooleanStylesFormatter.ToAttributeValue(field.Styles));
            }
        }
        private void CheckWriteBooleanFieldFalseText(XmlWriter writer, FtBooleanMetaField field)
        {
            if (field.FalseText != MetaSerializationDefaults.BooleanField.FalseText)
            {
                WriteAttribute(writer,
                               FieldPropertyIdFormatter.ToAttributeName(FtMetaField.PropertyId.FalseText),
                               field.FalseText);
            }
        }
        private void CheckWriteBooleanFieldTrueText(XmlWriter writer, FtBooleanMetaField field)
        {
            if (field.TrueText != MetaSerializationDefaults.BooleanField.TrueText)
            {
                WriteAttribute(writer,
                               FieldPropertyIdFormatter.ToAttributeName(FtMetaField.PropertyId.TrueText),
                               field.TrueText);
            }
        }
        private void CheckWriteIntegerFieldValue(XmlWriter writer, FtIntegerMetaField field)
        {
            if (field.Constant && !field.Null)
            {
                WriteAttribute(writer,
                               FieldPropertyIdFormatter.ToAttributeName(FtMetaField.PropertyId.Value),
                               FtStandardText.Get(field.Value));
            }
        }
        private void CheckWriteIntegerFieldStyles(XmlWriter writer, FtIntegerMetaField field)
        {
            if (field.Styles != MetaSerializationDefaults.IntegerField.Styles)
            {
                WriteAttribute(writer,
                               FieldPropertyIdFormatter.ToAttributeName(FtMetaField.PropertyId.Styles),
                               NumberStylesFormatter.ToAttributeValue(field.Styles));
            }
        }
        private void CheckWriteIntegerFieldFormat(XmlWriter writer, FtIntegerMetaField field)
        {
            if (field.Format != MetaSerializationDefaults.IntegerField.Format)
            {
                WriteAttribute(writer,
                               FieldPropertyIdFormatter.ToAttributeName(FtMetaField.PropertyId.Format),
                               field.Format);
            }
        }
        private void CheckWriteFloatFieldValue(XmlWriter writer, FtFloatMetaField field)
        {
            if (field.Constant && !field.Null)
            {
                WriteAttribute(writer,
                               FieldPropertyIdFormatter.ToAttributeName(FtMetaField.PropertyId.Value),
                               FtStandardText.Get(field.Value));
            }
        }
        private void CheckWriteFloatFieldStyles(XmlWriter writer, FtFloatMetaField field)
        {
            if (field.Styles != MetaSerializationDefaults.FloatField.Styles)
            {
                WriteAttribute(writer,
                               FieldPropertyIdFormatter.ToAttributeName(FtMetaField.PropertyId.Styles),
                               NumberStylesFormatter.ToAttributeValue(field.Styles));
            }
        }
        private void CheckWriteFloatFieldFormat(XmlWriter writer, FtFloatMetaField field)
        {
            if (field.Format != MetaSerializationDefaults.FloatField.Format)
            {
                WriteAttribute(writer,
                               FieldPropertyIdFormatter.ToAttributeName(FtMetaField.PropertyId.Format),
                               field.Format);
            }
        }
        private void CheckWriteDecimalFieldValue(XmlWriter writer, FtDecimalMetaField field)
        {
            if (field.Constant && !field.Null)
            {
                WriteAttribute(writer,
                               FieldPropertyIdFormatter.ToAttributeName(FtMetaField.PropertyId.Value),
                               FtStandardText.Get(field.Value));
            }
        }
        private void CheckWriteDecimalFieldStyles(XmlWriter writer, FtDecimalMetaField field)
        {
            if (field.Styles != MetaSerializationDefaults.DecimalField.Styles)
            {
                WriteAttribute(writer,
                               FieldPropertyIdFormatter.ToAttributeName(FtMetaField.PropertyId.Styles),
                               NumberStylesFormatter.ToAttributeValue(field.Styles));
            }
        }
        private void CheckWriteDecimalFieldFormat(XmlWriter writer, FtDecimalMetaField field)
        {
            if (field.Format != MetaSerializationDefaults.DecimalField.Format)
            {
                WriteAttribute(writer,
                               FieldPropertyIdFormatter.ToAttributeName(FtMetaField.PropertyId.Format),
                               field.Format);
            }
        }
        private void CheckWriteDateTimeFieldValue(XmlWriter writer, FtDateTimeMetaField field)
        {
            if (field.Constant && !field.Null)
            {
                WriteAttribute(writer,
                               FieldPropertyIdFormatter.ToAttributeName(FtMetaField.PropertyId.Value),
                               FtStandardText.Get(field.Value));
            }
        }
        private void CheckWriteDateTimeFieldStyles(XmlWriter writer, FtDateTimeMetaField field)
        {
            if (field.Styles != MetaSerializationDefaults.DateTimeField.Styles)
            {
                WriteAttribute(writer,
                               FieldPropertyIdFormatter.ToAttributeName(FtMetaField.PropertyId.Styles),
                               DateTimeStylesFormatter.ToAttributeValue(field.Styles));
            }
        }
        private void CheckWriteDateTimeFieldFormat(XmlWriter writer, FtDateTimeMetaField field)
        {
            if (field.Format != MetaSerializationDefaults.DateTimeField.Format)
            {
                WriteAttribute(writer,
                               FieldPropertyIdFormatter.ToAttributeName(FtMetaField.PropertyId.Format),
                               field.Format);
            }
        }

        // Substitution Properties

        private void WriteSubstitutionToken(XmlWriter writer, FtMetaSubstitution substitution)
        {
            WriteAttribute(writer,
                           SubstitutionPropertyIdFormatter.ToAttributeName(FtMetaSubstitution.PropertyId.Token),
                           FtStandardText.Get(substitution.Token));
        }
        private void CheckWriteSubstitutionType(XmlWriter writer, FtMetaSubstitution substitution)
        {
            if (substitution.Type != MetaSerializationDefaults.Substitution.Type)
            {
                WriteAttribute(writer,
                               SubstitutionPropertyIdFormatter.ToAttributeName(FtMetaSubstitution.PropertyId.Type),
                               SubstitutionTypeFormatter.ToAttributeValue(substitution.Type));
            }
        }
        private void CheckWriteSubstitutionValue(XmlWriter writer, FtMetaSubstitution substitution)
        {
            if (substitution.Type == FtSubstitutionType.String)
            {
                WriteAttribute(writer,
                               SubstitutionPropertyIdFormatter.ToAttributeName(FtMetaSubstitution.PropertyId.Value),
                               substitution.Value);
            }
        }

        // Sequence Properties

        private void WriteSequenceName(XmlWriter writer, FtMetaSequence sequence)
        {
            WriteAttribute(writer,
                           SequencePropertyIdFormatter.ToAttributeName(FtMetaSequence.PropertyId.Name),
                           sequence.Name);
        }
        private void CheckWriteSequenceRoot(XmlWriter writer, FtMetaSequence sequence, bool assumeRoot)
        {
            bool root = sequence.Root || assumeRoot;
            if (sequence.Root != MetaSerializationDefaults.Sequence.Root)
            {
                WriteAttribute(writer,
                               SequencePropertyIdFormatter.ToAttributeName(FtMetaSequence.PropertyId.Root),
                               FtStandardText.Get(true));
            }
        }
        private void CheckWriteSequenceFieldIndices(XmlWriter writer, FtMetaSequenceItemList itemList, int fieldIndicesFromItemIdx, FtMetaFieldList fieldList)
        {
            if (fieldIndicesFromItemIdx < itemList.Count)
            {
                int[] fieldIndices = new int[itemList.Count - fieldIndicesFromItemIdx];
                int idx = 0;
                for (int i = fieldIndicesFromItemIdx; i < itemList.Count; i++)
                {
                    fieldIndices[idx++] = fieldList.IndexOf(itemList[i].Field);
                }

                WriteAttribute(writer,
                               SequencePropertyIdFormatter.ToAttributeName(FtMetaSequence.PropertyId.FieldIndices),
                               FtCommaText.Get(fieldIndices));
            }
        }

        // SequenceItem Properties

        private void CheckWriteSequenceItemIndex(XmlWriter writer, int index)
        {
            if (ExplicitIndex)
            {
                WriteAttribute(writer,
                               SequenceItemPropertyIdFormatter.ToAttributeName(FtMetaSequenceItem.PropertyId.Index),
                               FtStandardText.Get(index));
            }
        }
        private void WriteSequenceItemFieldIndex(XmlWriter writer, FtMetaSequenceItem sequenceItem, FtMetaFieldList fieldList)
        {
            int fieldIdx = fieldList.IndexOf(sequenceItem.Field);
            WriteAttribute(writer,
                           SequenceItemPropertyIdFormatter.ToAttributeName(FtMetaSequenceItem.PropertyId.FieldIndex),
                           FtStandardText.Get(fieldIdx));
        }

        // SequenceRedirect Properties

        private void CheckWriteSequenceRedirectIndex(XmlWriter writer, int index)
        {
            if (ExplicitIndex)
            {
                WriteAttribute(writer,
                               SequenceRedirectPropertyIdFormatter.ToAttributeName(FtMetaSequenceRedirect.PropertyId.Index),
                               FtStandardText.Get(index));
            }
        }
        private void WriteSequenceRedirectSequenceName(XmlWriter writer, FtMetaSequenceRedirect redirect, FtMetaField field)
        {
            if (redirect.Sequence == null)
                throw new FtMetaSerializationException("Sequence not specified in redirect associated with Field: \"" + field.Name + "\"");
            else
                WriteAttribute(writer,
                               SequenceRedirectPropertyIdFormatter.ToAttributeName(FtMetaSequenceRedirect.PropertyId.SequenceName),
                               redirect.Sequence.Name);
        }
        private void CheckWriteSequenceRedirectType(XmlWriter writer, FtMetaSequenceRedirect redirect, FtMetaField field)
        {
            if (redirect.Type != field.DefaultSequenceRedirectType)
            {
                WriteAttribute(writer,
                               SequenceRedirectPropertyIdFormatter.ToAttributeName(FtMetaSequenceRedirect.PropertyId.Type),
                               SequenceRedirectTypeFormatter.ToAttributeValue(redirect.Type));
            }
        }
        private void CheckWriteSequenceRedirectInvokationDelay(XmlWriter writer, FtMetaSequenceRedirect redirect)
        {
            if (redirect.InvokationDelay != MetaSerializationDefaults.SequenceRedirect.InvokationDelay)
            {
                WriteAttribute(writer,
                               SequenceRedirectPropertyIdFormatter.ToAttributeName(FtMetaSequenceRedirect.PropertyId.InvokationDelay),
                               SequenceInvokationDelayFormatter.ToAttributeValue(redirect.InvokationDelay));
            }
        }
        private void WriteExactStringSequenceRedirectValue(XmlWriter writer, FtExactStringMetaSequenceRedirect redirect)
        {
            WriteAttribute(writer,
                           SequenceRedirectPropertyIdFormatter.ToAttributeName(FtMetaSequenceRedirect.PropertyId.Value),
                           redirect.Value);
        }
        private void WriteCaseInsensitiveStringSequenceRedirectValue(XmlWriter writer, FtCaseInsensitiveStringMetaSequenceRedirect redirect)
        {
            WriteAttribute(writer,
                           SequenceRedirectPropertyIdFormatter.ToAttributeName(FtMetaSequenceRedirect.PropertyId.Value),
                           redirect.Value);
        }
        private void WriteBooleanSequenceRedirectValue(XmlWriter writer, FtBooleanMetaSequenceRedirect redirect)
        {
            WriteAttribute(writer,
                           SequenceRedirectPropertyIdFormatter.ToAttributeName(FtMetaSequenceRedirect.PropertyId.Value),
                           FtStandardText.Get(redirect.Value));
        }
        private void WriteExactIntegerSequenceRedirectValue(XmlWriter writer, FtExactIntegerMetaSequenceRedirect redirect)
        {
            WriteAttribute(writer,
                           SequenceRedirectPropertyIdFormatter.ToAttributeName(FtMetaSequenceRedirect.PropertyId.Value),
                           FtStandardText.Get(redirect.Value));
        }
        private void WriteExactFloatSequenceRedirectValue(XmlWriter writer, FtExactFloatMetaSequenceRedirect redirect)
        {
            WriteAttribute(writer,
                           SequenceRedirectPropertyIdFormatter.ToAttributeName(FtMetaSequenceRedirect.PropertyId.Value),
                           FtStandardText.Get(redirect.Value));
        }
        private void WriteExactDateTimeSequenceRedirectValue(XmlWriter writer, FtExactDateTimeMetaSequenceRedirect redirect)
        {
            WriteAttribute(writer,
                           SequenceRedirectPropertyIdFormatter.ToAttributeName(FtMetaSequenceRedirect.PropertyId.Value),
                           FtStandardText.Get(redirect.Value));
        }
        private void WriteDateSequenceRedirectValue(XmlWriter writer, FtDateMetaSequenceRedirect redirect)
        {
            WriteAttribute(writer,
                           SequenceRedirectPropertyIdFormatter.ToAttributeName(FtMetaSequenceRedirect.PropertyId.Value),
                           FtStandardText.Get(redirect.Value));
        }
        private void WriteExactDecimalSequenceRedirectValue(XmlWriter writer, FtExactDecimalMetaSequenceRedirect redirect)
        {
            WriteAttribute(writer,
                           SequenceRedirectPropertyIdFormatter.ToAttributeName(FtMetaSequenceRedirect.PropertyId.Value),
                           FtStandardText.Get(redirect.Value));
        }
    }
}
