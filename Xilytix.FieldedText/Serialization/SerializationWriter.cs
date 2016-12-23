// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Diagnostics;

namespace Xilytix.FieldedText.Serialization
{
    using Formatting;

    public class SerializationWriter : SerializationCore, IDisposable
    {
        const string DeclarationLine2BeforeCommentChars = " ";
        const string EmbeddedMetaBeforeCommentChars = " ";

        private bool disposedValue = false; // To detect redundant calls

        private FtMeta loadedMeta; // a copy of loaded meta in case we need to embed
        private TextWriter writer;
        private bool writerOwned;
        private FtWriterSettings settings;

        private StringBuilder encodeBuilder;
        private StringBuilder matchBuilder;

        private string endOfLineText;
        private bool lastLineEnded;

        private bool headerWritten;
        private bool endOfLinePending;
        private int activeFieldIndex;

        internal SerializationWriter(FtMeta meta): base()
        {
            encodeBuilder = new StringBuilder();
            matchBuilder = new StringBuilder();

            InternalLoadMeta(meta);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (writer != null)
                    {
                        if (writerOwned)
                        {
                            writer.Dispose();
                            writerOwned = false;
                        }

                        writer = null;
                    }
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public TextWriter BaseWriter { get { return writer; } }

        public override void LoadMeta(FtMeta meta)
        {
            InternalLoadMeta(meta);
        }

        internal new void InternalLoadMeta(FtMeta meta)
        {
            base.InternalLoadMeta(meta);
            loadedMeta = meta.CreateCopy();
            endOfLineText = CalculateEndOfLineText();
            lastLineEnded = LastLineEndedType != FtLastLineEndedType.Never;
        }

        protected void Open(TextWriter writer, bool writerOwned, FtWriterSettings settings)
        {
            if (this.writer != null && writerOwned)
            {
                this.writer.Close();
            }
            this.writer = writer;
            this.writerOwned = writerOwned;
            this.settings = settings;

            Reset();

            InvokeRootSequence();
        }

        public void Close()
        {
            if (writer != null)
            {
                if (writerOwned)
                {
                    writer.Close();
                }
                writer = null;
            }
            settings = null;

            Reset();
        }

        protected new void Reset()
        {
            base.Reset();

            encodeBuilder.Clear();
            matchBuilder.Clear();

            headerWritten = false;
            endOfLinePending = false;
            activeFieldIndex = -1;
        }

        public void WriteHeader()
        {
            if (headerWritten)
                throw new FtSerializationException(FtSerializationError.HeaderAlreadyWritten, Properties.Resources.SerializationWriter_WriteHeader_HeaderAlreadyWritten);
            else
            {
                if (settings.Declared)
                {
                    WriteDeclaration();
                }

                if (settings.MetaReferenceType == FtMetaReferenceType.Embedded)
                {
                    WriteEmbeddedMeta();
                }

                WriteHeadingLines();

                headerWritten = true;
            }
        }

        public void Write()
        {
            if (!headerWritten)
            {
                WriteHeader();
            }

            WriteRecord();
            FinishRecord();
        }

        public void WriteComment(string comment)
        {
            WriteComment(comment, "");
        }

        public void Flush()
        {
            writer.Flush();
        }

        public object this[string name] { set { FieldList[name].AsObject = value; } }
        public object this[int idx] { set { FieldList[idx].AsObject = value; } }
        public void SetNull(int idx) { FieldList[idx].SetNull(); }
        public void SetNull(int idx, out int fieldsAffectedFromIndex) { FieldList[idx].SetNull(out fieldsAffectedFromIndex); }
        public void SetBoolean(int idx, bool value)
        {
            FtField field = FieldList[idx];
            if (field.DataType != FtStandardDataType.Boolean)
                throw new InvalidCastException(string.Format(Properties.Resources.SerializationWriter_SetBoolean_InvalidCast, field.DataTypeName));
            else
                ((FtBooleanField)field).Value = value;
        }
        public void SetDateTime(int idx, DateTime value)
        {
            FtField field = FieldList[idx];
            if (field.DataType != FtStandardDataType.DateTime)
                throw new InvalidCastException(string.Format(Properties.Resources.SerializationWriter_SetDateTime_InvalidCast, field.DataTypeName));
            else
                ((FtDateTimeField)field).Value = value;
        }
        public void SetDecimal(int idx, decimal value) 
        {
            FtField field = FieldList[idx];
            if (field.DataType != FtStandardDataType.Decimal)
                throw new InvalidCastException(string.Format(Properties.Resources.SerializationWriter_SetDecimal_InvalidCast, field.DataTypeName));
            else
                ((FtDecimalField)field).Value = value;
        }
        public void SetDouble(int idx, double value)
        {
            FtField field = FieldList[idx];
            if (field.DataType != FtStandardDataType.Float)
                throw new InvalidCastException(string.Format(Properties.Resources.SerializationWriter_SetDouble_InvalidCast, field.DataTypeName));
            else
                ((FtFloatField)field).Value = value;
        }
        public void SetInt64(int idx, long value)
        {
            FtField field = FieldList[idx];
            if (field.DataType != FtStandardDataType.Integer)
                throw new InvalidCastException(string.Format(Properties.Resources.SerializationWriter_SetInt64_InvalidCast, field.DataTypeName));
            else
                ((FtIntegerField)field).Value = value;
        }
        public void SetString(int idx, string value)
        {
            FtField field = FieldList[idx];
            if (field.DataType != FtStandardDataType.String)
                throw new InvalidCastException(string.Format(Properties.Resources.SerializationWriter_SetString_InvalidCast, field.DataTypeName));
            else
                ((FtStringField)field).Value = value;
        }
        public void SetValue(int idx, object value) { FieldList[idx].AsObject = value; }
        public void SetValues(object[] values)
        {
            if (values != null)
            {
                if (values.Length > FieldCount)
                    throw new ArgumentException(string.Format(Properties.Resources.SerializationWriter_SetValues_ValuesLengthExceededFieldCount, values.Length, FieldCount));
                else
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        FieldList[i].AsObject = values[i];
                    }
                }
            }
        }
        public bool IsClosed { get { return writer == null; } }

        private string CalculateEndOfLineText()
        {
            switch (EndOfLineType)
            {
                case FtEndOfLineType.Char: return EndOfLineChar.ToString();
                case FtEndOfLineType.CrLf: return CarriageReturnLineFeedString;
                case FtEndOfLineType.Auto:
                    switch (EndOfLineAutoWriteType)
                    {
                        case FtEndOfLineAutoWriteType.CrLf: return CarriageReturnLineFeedString;
                        case FtEndOfLineAutoWriteType.Cr: return CarriageReturnChar.ToString();
                        case FtEndOfLineAutoWriteType.Lf: return LineFeedChar.ToString();
                        case FtEndOfLineAutoWriteType.Local: return Environment.NewLine;
                        default: throw FtInternalException.Create(InternalError.SerializationWriter_CalculateEndOfLineText_UnsupportedEndOfLineAutoWriteType, EndOfLineAutoWriteType.ToString());
                    }
                default: throw FtInternalException.Create(InternalError.SerializationWriter_CalculateEndOfLineText_UnsupportedEndOfLineType, EndOfLineType.ToString());
            }
        }

        private void FinishRecord()
        {
            if (SequenceInvokationList.Matches(PreviousRecordSequenceInvokationList))
                InvokeRootSequence();
            else
            {
                tableCount++;
                PreviousRecordSequenceInvokationList.Assign(SequenceInvokationList);
                InvokeRootSequence();
            }
        }

        private bool DoesTextRequireQuotes(string text, bool willBeSpacePrefixed)
        {
            string startTrimmedText = text.TrimStart();
            if (startTrimmedText.Length > 0 && startTrimmedText[0] == QuoteChar)
                return true;
            else
            {
                if (willBeSpacePrefixed && (text.Length == 0 || char.IsWhiteSpace(text[0])))
                    return true;
                else
                {
                    char[] searchChars;
                    if (!AllowEndOfLineCharInQuotes)
                        searchChars = new char[1];
                    else
                    {
                        switch (EndOfLineType)
                        {
                            case FtEndOfLineType.Auto:
                            case FtEndOfLineType.CrLf:
                                searchChars = new char[3];
                                searchChars[1] = CarriageReturnChar;
                                searchChars[2] = LineFeedChar;
                                break;
                            case FtEndOfLineType.Char:
                                searchChars = new char[1];
                                searchChars[1] = EndOfLineChar;
                                break;
                            default:
                                throw FtInternalException.Create(InternalError.SerializationWriter_DoesTextRequireQuotes_UnsupportedEndOfLineType, EndOfLineType.ToString());
                        }
                    }
                    searchChars[0] = DelimiterChar;

                    return startTrimmedText.IndexOfAny(searchChars) >= 0;
                }
            }

        }

        private void WriteComment(string comment, string beforeCommentChars)
        {
            CheckWritePendingEndOfLine();

            writer.Write(LineCommentChar);

            if (comment != "")
            {
                if (beforeCommentChars.Length > 0)
                {
                    writer.Write(beforeCommentChars);
                }
                writer.Write(comment);
            }
            WriteOrPendEndOfLine();
        }

        private void CheckWritePendingEndOfLine()
        {
            if (endOfLinePending)
            {
                writer.Write(endOfLineText);
                endOfLinePending = false;
            }
        }

        private void WriteOrPendEndOfLine()
        {
            if (lastLineEnded)
                writer.Write(endOfLineText);
            else
                endOfLinePending = true;
        }

        private void WriteDeclaration()
        {
            DeclaredParameters parameters = new DeclaredParameters();
            parameters.SetVersion(VersionMajor, VersionMinor);
            parameters.SetMetaReference(settings.MetaReferenceType, settings.MetaReference);
            WriteComment(Signature + ' ' + DeclaredParametersFormatter.ToSignatureLineText(parameters), "");
            WriteComment(DeclaredParametersFormatter.ToLine2Text(parameters), DeclarationLine2BeforeCommentChars);
        }

        private void WriteEmbeddedMeta()
        {
            StringBuilder metaBuilder = new StringBuilder(400);

            XmlWriterSettings metaWriterSettings = new XmlWriterSettings();
            metaWriterSettings.Indent = settings.EmbeddedMetaIndent;
            metaWriterSettings.IndentChars = settings.EmbeddedMetaIndentChars;
            metaWriterSettings.NewLineOnAttributes = settings.EmbeddedMetaNewLineOnAttributes;
            XmlWriter metaWriter = XmlWriter.Create(metaBuilder, metaWriterSettings);
            MetaSerialization.XmlMetaSerializationWriter metaSerializationWriter = new MetaSerialization.XmlMetaSerializationWriter();
            metaSerializationWriter.Write(metaWriter, loadedMeta);
            metaWriter.Close();

            string metaText = metaBuilder.ToString();
            StringReader metaReader = new StringReader(metaText);
            string metaLine = metaReader.ReadLine();
            while (metaLine != null)
            {
                if (metaLine.StartsWith("<?xml") && metaLine.EndsWith("?>"))
                {
                    metaLine = FixEmbeddedMetaXmlEncoding(metaLine);
                }
                WriteComment(metaLine, EmbeddedMetaBeforeCommentChars);
                metaLine = metaReader.ReadLine();
            }
        }

        private string FixEmbeddedMetaXmlEncoding(string line)
        {
            int idx = line.IndexOf("encoding", StringComparison.OrdinalIgnoreCase);
            if (idx < 0)
                return line;
            else
            {
                int openQuotesIdx = line.IndexOf('"', idx);
                if (openQuotesIdx < 0)
                    return line;
                else
                {
                    int closeQuotesIdx = line.IndexOf('"', openQuotesIdx + 1);
                    if (closeQuotesIdx < 0)
                        return line;
                    else
                        return line.Substring(0, openQuotesIdx + 1) + writer.Encoding.WebName + line.Substring(closeQuotesIdx);
                }
            }
        }

        private void WriteHeadingLines()
        {
            for (int i = 0; i < HeadingLineCount; i++)
            {
                WriteHeadingLine(i);
            }
        }

        private void WriteHeadingLine(int headingLineIndex)
        {
            CheckWritePendingEndOfLine();

            OnHeadingLineStarted(headingLineIndex);

            FtField field = null;
            activeFieldIndex = 0;
            while (activeFieldIndex < FieldList.Count)
            {
                // field points to previous field
                if (field != null && !field.FixedWidth)
                {
                    writer.Write(DelimiterChar);
                }
                field = FieldList[activeFieldIndex];

                OnFieldHeadingWriteReady(field, headingLineIndex);

                WriteHeadingField(field, headingLineIndex);

                activeFieldIndex++;
            }

            activeFieldIndex = -1;

            WriteOrPendEndOfLine();

            OnHeadingLineFinished(headingLineIndex);
        }

        private void WriteHeadingField(FtField field, int headingLineIndex)
        {
            string headingText = field.Headings[headingLineIndex];

            if (headingText == null)
            {
                headingText = "";
            }

            if (field.FixedWidth)
            {
                string rawText = EncodeFixedWidthHeadingText(headingText, field);
                writer.Write(rawText);
            }
            else
            {
                bool fieldQuoted;
                switch (field.HeadingQuotedType)
                {
                    case FtQuotedType.Never:
                        fieldQuoted = false;
                        break;
                    case FtQuotedType.Always:
                        fieldQuoted = true;
                        break;
                    case FtQuotedType.Optional:
                        if (field.HeadingAlwaysWriteOptionalQuote)
                            fieldQuoted = true;
                        else
                            fieldQuoted = DoesTextRequireQuotes(headingText, field.HeadingWritePrefixSpace);
                        break;
                    default:
                        throw FtInternalException.Create(InternalError.SerializationWriter_WriteHeadingField_UnsupportedQuotedType, field.HeadingQuotedType.ToString());
                }

                if (field.HeadingWritePrefixSpace)
                {
                    writer.Write(PrefixSpaceChar);
                }

                if (fieldQuoted)
                {
                    writer.Write(QuoteChar);
                }

                string rawText = EncodeDelimitValueHeadingText(headingText, fieldQuoted);
                writer.Write(rawText);

                if (fieldQuoted)
                {
                    writer.Write(QuoteChar);
                }
            }
        }

        private void WriteRecord()
        {
            CheckWritePendingEndOfLine();

            int recordIndex = recordCount;
            recordCount++;

            OnRecordStarted(recordIndex);

            FtField field = null;
            activeFieldIndex = 0;
            while (activeFieldIndex < FieldList.Count)
            {
                // field points to previous field
                if (field != null && !field.FixedWidth)
                {
                    writer.Write(DelimiterChar);
                }
                field = FieldList[activeFieldIndex];
                WriteRecordField(field, recordIndex);

                activeFieldIndex++;
            }

            activeFieldIndex = -1;

            WriteOrPendEndOfLine();

            OnRecordFinished(recordIndex);
        }

        private void WriteRecordField(FtField field, int recordIndex)
        {
            OnFieldValueWriteReady(field, recordIndex);

            if (field.FixedWidth)
            {
                if (field.IsNull())
                    writer.Write(field.Definition.FixedWidthNullValueText); 
                else
                {
                    string valueText = field.GetAsNonNullValueText();
                    string rawText = EncodeFixedWidthValueText(valueText, field);
                    writer.Write(rawText);
                }
            }
            else
            {
                if (field.IsNull())
                    writer.Write("");
                else
                {
                    string valueText = field.GetAsNonNullValueText();
                    
                    bool fieldQuoted;
                    switch (field.ValueQuotedType)
                    {
                        case FtQuotedType.Never:
                            fieldQuoted = false;
                            break;
                        case FtQuotedType.Always:
                            fieldQuoted = true;
                            break;
                        case FtQuotedType.Optional:
                            if (field.ValueAlwaysWriteOptionalQuote)
                                fieldQuoted = true;
                            else
                                fieldQuoted = DoesTextRequireQuotes(valueText, field.ValueWritePrefixSpace);
                            break;
                        default:
                            throw FtInternalException.Create(InternalError.SerializationWriter_WriteRecordField_UnsupportedQuotedType, field.HeadingQuotedType.ToString());
                    }

                    if (field.ValueWritePrefixSpace)
                    {
                        writer.Write(PrefixSpaceChar);
                    }

                    if (fieldQuoted)
                    {
                        writer.Write(QuoteChar);
                    }

                    string rawText = EncodeDelimitValueHeadingText(valueText, fieldQuoted);
                    writer.Write(rawText);

                    if (fieldQuoted)
                    {
                        writer.Write(QuoteChar);
                    }
                }
            }

            if (!field.Constant && !field.ValueAssigned)
            {
                // Field was not assigned a value (or null) so it will be null but any null sequence redirects will not have been processed yet. Do so now.
                Debug.Assert(field.IsNull());
                int fieldsAffectedFromIndex;
                field.CheckNullSequenceRedirect(out fieldsAffectedFromIndex);
            }
        }

        private string EncodeDelimitValueHeadingText(string text, bool quoted)
        {
            encodeBuilder.Clear();

            if (!SubstitutionsEnabled)
                AppendToEncodeBuilder(text, quoted);
            else
            {
                matchBuilder.Clear();

                for (int i = 0; i < text.Length; i++)
                {
                    matchBuilder.Append(text[i]);

                    bool moreCheckingRequired;
                    char token;
                    bool recheckLastChar;
                    do
                    {
                        string currentMatchingText = matchBuilder.ToString();
                        if (!CheckForSubstitution(currentMatchingText, out moreCheckingRequired, out token, out recheckLastChar))
                        {
                            // Add unmatched text
                            AppendToEncodeBuilder(currentMatchingText, quoted);
                            matchBuilder.Clear();
                        }
                        else
                        {
                            if (!moreCheckingRequired)
                            {
                                encodeBuilder.Append(SubstitutionChar);
                                encodeBuilder.Append(token);
                                matchBuilder.Clear();

                                if (recheckLastChar)
                                {
                                    matchBuilder.Append(text[i]);
                                }
                            }
                        }
                    }
                    while (recheckLastChar);
                }
            }

            return encodeBuilder.ToString();
        }

        private void AppendToEncodeBuilder(string text, bool quoted)
        {
            if (!quoted || !StuffedEmbeddedQuotes)
                encodeBuilder.Append(text);
            else
            {
                for (int i = 0; i < text.Length; i++)
                {
                    char textChar = text[i];
                    encodeBuilder.Append(textChar);
                    if (textChar == QuoteChar)
                    {
                        encodeBuilder.Append(textChar);
                    }
                }
            }
        }

        private bool CheckForSubstitution(string text, out bool moreCheckingRequired, out char token, out bool recheckLastChar)
        {
            const char NullToken = '\x00';

            bool result = false;
            moreCheckingRequired = true;
            recheckLastChar = false;
            token = NullToken;

            for (int i = 0; i < SubstitutionList.Count; i++)
            {
                FtSubstitution substitution = SubstitutionList[i];
                switch (substitution.Type)
                {
                    case FtSubstitutionType.String:
                    case FtSubstitutionType.AutoEndOfLine:
                        if (substitution.Value.StartsWith(text, StringComparison.Ordinal))
                        {
                            moreCheckingRequired = text.Length != substitution.Value.Length;
                            result = true;
                        }
                        break;
                }

                if (result)
                {
                    if (!moreCheckingRequired)
                    {
                        token = substitution.Token; 
                    }
                    break;
                }
            }

            return result;
        }

        private bool CheckForSubstitutionChar(string text, char checkChar, out bool moreCheckingRequired)
        {
            if (text.Length == 0)
            {
                moreCheckingRequired = true;
                return true;
            }
            else
            {
                moreCheckingRequired = false;
                return text[0] == checkChar;
            }
        }

        private string EncodeFixedWidthValueText(string text, FtField field)
        {
            int textLength = text.Length;
            if (textLength == field.Width)
                return text;
            else
            {
                if (textLength < field.Width)
                    return PadFixedWidthText(text, field.Definition, field.ValuePadAlignment, field.ValuePadCharType, field.ValuePadChar, field.ValueEndOfValueChar);
                else
                    return TruncateFixedWidthText(text, field.Definition, field.ValueTruncateType, field.ValueTruncateChar, field.ValueNullChar);
            }
        }

        private string EncodeFixedWidthHeadingText(string text, FtField field)
        {
            int textLength = text.Length;
            if (textLength == field.Width)
                return text;
            else
            {
                if (textLength < field.Width)
                    return PadFixedWidthText(text, field.Definition, field.HeadingPadAlignment, field.HeadingPadCharType, field.HeadingPadChar, field.HeadingEndOfValueChar);
                else
                    return TruncateFixedWidthText(text, field.Definition, field.HeadingTruncateType, field.HeadingTruncateChar, field.HeadingTruncateChar /* Headings should never Null truncate*/ );
            }
        }

        private string PadFixedWidthText(string text, FtFieldDefinition definition, FtPadAlignment padAlignment, FtPadCharType padCharType, char padChar, char endOfValueChar)
        {
            bool leftPad;
            switch (padAlignment)
            {
                case FtPadAlignment.Auto:
                    leftPad = definition.AutoLeftPad;
                    break;
                case FtPadAlignment.Left:
                    leftPad = true;
                    break;
                case FtPadAlignment.Right:
                    leftPad = false;
                    break;
                default:
                    throw FtInternalException.Create(InternalError.FtField_PadText_UnsupportedPadAlignment, padAlignment.ToString());
            }

            int padLength;
            string padText;
            string result;

            switch (padCharType)
            {
                case FtPadCharType.Auto:
                case FtPadCharType.Specified:
                    char usePadChar;
                    if (padCharType == FtPadCharType.Auto)
                        usePadChar = definition.AutoPadChar;
                    else
                        usePadChar = padChar;

                    padLength = definition.Width - text.Length;
                    padText = new string(usePadChar, padLength);

                    if (leftPad)
                        result = padText + text;
                    else
                        result = text + padText;
                    break;
                case FtPadCharType.EndOfValue:
                    padLength = definition.Width - text.Length - 1;
                    padText = new string(padChar, padLength);
                    if (leftPad)
                        result = padText + endOfValueChar.ToString() + text;
                    else
                        result = text + endOfValueChar.ToString() + padText;
                    break;
                default:
                    throw FtInternalException.Create(InternalError.FtField_PadText_UnsupportedPadCharType, padCharType.ToString());
            }

            return result;
        }

        private string TruncateFixedWidthText(string text, FtFieldDefinition definition, FtTruncateType truncateType, char truncateChar, char nullChar)
        {
            switch (truncateType)
            {
                case FtTruncateType.Left: return text.Substring(0, definition.Width);
                case FtTruncateType.Right: return text.Substring(text.Length - definition.Width, definition.Width);
                case FtTruncateType.TruncateChar: return new string(truncateChar, definition.Width);
                case FtTruncateType.NullChar: return new string(nullChar, definition.Width);
                case FtTruncateType.Exception:
                    throw new FtSerializationException(FtSerializationError.FieldTruncated, string.Format(Properties.Resources.FtField_TruncateText_TextTruncation, text));
                default:
                    throw FtInternalException.Create(InternalError.FtField_TruncateText_UnsupportedTruncateType, truncateType.ToString());
            }
        }
    }
}