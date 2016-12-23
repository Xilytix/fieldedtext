// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

namespace Xilytix.FieldedText.Serialization
{
    using System;
    using System.Text;
    using System.Xml;
    using System.IO;
    using System.Data;
    using System.Net;
    using System.Diagnostics;

    public class SerializationReader : SerializationCore, IDataReader, IDataRecord, IDisposable
    {
        private enum EmbeddedMetaState { ftemNone, ftemDeclared, ftemReady, ftemLoaded }

        private bool disposedValue = false; // To detect redundant calls

        private CharReader charReader;

        private bool headerRead;
        private bool declarationRead;
        private bool embeddedMetaRead;
        private int headingLineReadCount;
        private bool headingLinesRead;
        private bool finished;

        private bool declared;
        private DeclaredParameters declaredParameters;
        private FtMetaReferenceType metaReferenceType;
        private string metaReference;
        private bool metaEmbedded;
        private bool newTableStarted;
        private bool tableStartSuspended;
        private bool tableStartRecordPeeked;
        private FtLineType lineType;

        private LineParser lineParser;
        private DeclarationParser declarationParser;
        private EmbeddedMetaParser embeddedMetaParser;
        private HeadingLineRecordParser headingLineParser;
        private HeadingLineRecordParser recordParser;
        private StringBuilder lineBuilder;

        internal SerializationReader() : base()
        {
            charReader = new CharReader();
            declaredParameters = new DeclaredParameters();
            lineParser = new LineParser(this, charReader);
            declarationParser = new DeclarationParser(charReader, declaredParameters);
            embeddedMetaParser = new EmbeddedMetaParser();
            headingLineParser = new HeadingLineRecordParser(this, charReader, true);
            recordParser = new HeadingLineRecordParser(this, charReader, false);
            lineBuilder = new StringBuilder(50);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    charReader.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>Gets or sets a value indicating whether Read() will automatically keep reading records when new tables begin.</summary>
        public bool AutoNextTable { get; set; }

        /// <summary>Gets a value indicating whether the fielded text stream was declared.</summary>
        public bool Declared { get { return declared; } }
        /// <summary>Gets the number of heading lines read in fielded text stream.</summary>
        public int HeadingLineReadCount { get { return headingLineReadCount; } }
        /// <summary>Gets a value indicating whether the header in the fielded text stream has been fully read.</summary>
        public bool HeaderRead { get { return headerRead; } }
        public FtMetaReferenceType MetaReferenceType { get { return metaReferenceType; } }
        public string MetaReference { get { return metaReference; } }
        /// <summary>Returns the <c>FtLineType</c> of the last line read from the fielded text stream.</summary>
        public FtLineType LineType { get { return lineType; } }
        /// <summary> Returns the last line read</summary>
        public string Line { get { return lineBuilder.ToString(); } }
        /// <summary>Gets a value indicating whether the last record read was the start of a new table in the fielded text stream.</summary>
        public bool NewTableStarted { get { return newTableStarted; } }
        /// <summary>Gets the number of records read in fielded text stream.</summary>
        public new int RecordCount { get { return tableStartRecordPeeked ? recordCount - 1 : recordCount; } }
        /// <summary>Gets the number of tables read in fielded text stream.</summary>
        public new int TableCount { get { return tableStartSuspended ? tableCount - 1 : tableCount; } }
        public int ActiveFieldIndex
        {
            get
            {
                switch (lineType)
                {
                    case FtLineType.Heading: return headingLineParser.GetActiveFieldIndex();
                    case FtLineType.Record: return recordParser.GetActiveFieldIndex();
                    default: return -1;
                }
            }
        }
        public int IgnoreExtraCharsLinePosition
        {
            get
            {
                switch (lineType)
                {
                    case FtLineType.Heading: return headingLineParser.GetIgnoreExtraCharsLinePosition();
                    case FtLineType.Record: return recordParser.GetIgnoreExtraCharsLinePosition();
                    default: return -1;
                }
            }
        }

        public event EventHandler RecordSeeked;

        protected void Open(TextReader textReader, bool readerOwned, bool immediatelyReadHeader)
        {
            charReader.SetTextReader(textReader, readerOwned);
            string signature = charReader.PeekSignature();
            declared = signature != null;
            if (declared)
            {
                declarationParser.Signature = signature;
            }

            Reset();

            if (!headerRead && immediatelyReadHeader)
            {
                ReadHeader();
            }
        }

        public void Close()
        {
            charReader.Close();
            Reset();
        }

        public void ReadHeader()
        {
            if (declared && !declarationRead)
            {
                ReadDeclaration();
            }

            if (metaReferenceType == FtMetaReferenceType.Embedded && !embeddedMetaRead)
            {
                ReadEmbeddedMeta();
            }

            if (HeadingLineCount > 0 && !headingLinesRead)
            {
                ReadHeadingLines();
            }
        }

        /// <summary>Read the next line from the fielded text stream.</summary>
        /// <returns><c>true</c> if a line was read. <c>false</c> if at the end of the stream</returns>
        /// <remarks><c>ReadLine</c> ignores the value in <c>AutoNextTable</c> and will always read records in new tables</remarks>
        public bool ReadLine()
        {
            bool result;
            if (!tableStartRecordPeeked)
                result = InternalReadLine();
            else
            {
                tableStartRecordPeeked = false; 
                result = true;
            }

            if (result && tableStartSuspended && lineType == FtLineType.Record)
            {
                tableStartSuspended = false;
            }

            return result;
        }

        /// <summary>Read the next record from the fielded text stream.</summary>
        /// <returns><c>true</c> if a record was read. <c>false</c> if at the end of the stream or at the end of a table (see remarks)</returns>
        /// <remarks>
        /// The value in <c>AutoNextTable</c> determines whether <c>Read()</c> can automatically continue reading records when a new table starts.
        /// If <c>AutoNextTable</c> is <c>true</c>, then <c>Read()</c> will continue reading records when a new table starts.
        /// If <c>AutoNextTable</c> is <c>false</c>, then <c>Read()</c> will not automatically read records in a new table.  It will return <c>false</c>
        /// after all records in the current table have been read.  If there are more tables in the stream, then <c>NewTableStarted</c> will be <c>true</c>.
        /// In order to read the records in the next table, <c>NextTable()</c> or <c>NextResult</c> must first be called.
        /// </remarks>
        public bool Read()
        {
            if (tableStartSuspended)
            {
                if (!AutoNextTable)
                    return false;
                {
                    tableStartSuspended = false;
                    return Read();
                }
            }
            else
            {
                if (tableStartRecordPeeked)
                {
                    tableStartRecordPeeked = false;
                    return true;
                }
                else
                {
                    bool lineRead;
                    do
                    {
                        lineRead = InternalReadLine();
                    }
                    while (lineRead && lineType != FtLineType.Record);

                    if (!lineRead)
                        return false;
                    else
                    {
                        if (!newTableStarted)
                            return true;
                        else
                        {
                            if (AutoNextTable || RecordCount == 1)
                                return true;
                            else
                            {
                                tableStartSuspended = true;
                                tableStartRecordPeeked = true;
                                return false;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>Reads all remaining records from the fielded text stream</summary>
        /// <remarks>Normally used in conjunction with reader events.  Throw <c>FtAbortSerializationException</c> in events to abort</remarks>
        public void ReadToEnd()
        {
            while (Read()) ;
        }

        /// <summary>Read the next record from the fielded text stream and indicate whether a new table started</summary>
        /// <returns>
        /// <c>FtReadRecordResult.SameTable</c> if a record was read in the current table.
        /// <c>FtReadRecordResult.NewTable</c> if the first record was read in a new table
        /// <c>FtReadRecordResult.NoMoreRecords</c> if at the end of the stream
        /// </returns>
        /// <remarks><c>ReadRecord()</c> ignores the value in <c>AutoNextTable</c> and will always read records in new tables</remarks>
        public FtReadRecordResult ReadRecord()
        {
            if (tableStartSuspended)
            {
                tableStartSuspended = false;
                if (tableStartRecordPeeked)
                    tableStartRecordPeeked = false;
                else
                    ReadRecord(); // should never happen
                return FtReadRecordResult.NewTable;
            }
            else
            {
                bool lineRead;
                do
                {
                    lineRead = InternalReadLine();
                }
                while (lineRead && lineType != FtLineType.Record);

                if (!lineRead)
                    return FtReadRecordResult.NoMoreRecords;
                else
                {
                    if (newTableStarted)
                        return FtReadRecordResult.NewTable;
                    else
                        return FtReadRecordResult.SameTable;
                }
            }
        }

        public bool NextTable()
        {
            if (tableStartSuspended)
            {
                tableStartSuspended = false;
                return true;
            }
            else
            {
                bool result;
                do
                {
                    result = Read();
                }
                while (result && !newTableStarted);

                if (result)
                {
                    tableStartRecordPeeked = true;
                }
                return result;
            }
        }

        public bool Seek(long offset)
        {
            bool result = true;
            SetSeeking(true);
            try
            {
                for (int i = 0; i < offset-1; i++)
                {
                    if (ReadRecord() != FtReadRecordResult.NoMoreRecords)
                        RecordSeeked?.Invoke(this, EventArgs.Empty);
                    else
                    {
                        result = false;
                        break;
                    }
                }
            }
            finally
            {
                SetSeeking(false);
            }


            if (result)
            {
                result = ReadRecord() != FtReadRecordResult.NoMoreRecords;
            }

            return result;
        }

        public void SeekEnd()
        {
            SetSeeking(true);
            try
            {
                while (ReadRecord() != FtReadRecordResult.NoMoreRecords)
                {
                    RecordSeeked?.Invoke(this, EventArgs.Empty);
                }
            }
            finally
            {
                SetSeeking(false);
            }
        }

        // IDataReader and IDataRecord
        public object this[string name] { get { return FieldList[name].AsObject; } }
        public object this[int idx] { get { return FieldList[idx].AsObject; } }
        public bool GetBoolean(int idx)
        {
            FtField field = FieldList[idx];
            if (field.DataType != FtStandardDataType.Boolean)
                throw new InvalidCastException(string.Format(Properties.Resources.SerializationReader_GetBoolean_InvalidCast, field.DataTypeName));
            else
                return ((FtBooleanField)field).Value;
        }
        public DateTime GetDateTime(int idx)
        {
            FtField field = FieldList[idx];
            if (field.DataType != FtStandardDataType.DateTime)
                throw new InvalidCastException(string.Format(Properties.Resources.SerializationReader_GetDateTime_InvalidCast, field.DataTypeName));
            else
                return ((FtDateTimeField)field).Value;
        }
        public decimal GetDecimal(int idx)
        {
            FtField field = FieldList[idx];
            if (field.DataType != FtStandardDataType.Decimal)
                throw new InvalidCastException(string.Format(Properties.Resources.SerializationReader_GetDecimal_InvalidCast, field.DataTypeName));
            else
                return ((FtDecimalField)field).Value;
        }
        public double GetDouble(int idx)
        {
            FtField field = FieldList[idx];
            if (field.DataType != FtStandardDataType.Float)
                throw new InvalidCastException(string.Format(Properties.Resources.SerializationReader_GetDouble_InvalidCast, field.DataTypeName));
            else
                return ((FtFloatField)field).Value;
        }
        public long GetInt64(int idx)
        {
            FtField field = FieldList[idx];
            if (field.DataType != FtStandardDataType.Integer)
                throw new InvalidCastException(string.Format(Properties.Resources.SerializationReader_GetInt64_InvalidCast, field.DataTypeName));
            else
                return ((FtIntegerField)field).Value;
        }
        public string GetString(int idx)
        {
            FtField field = FieldList[idx];
            if (field.DataType != FtStandardDataType.String)
                throw new InvalidCastException(string.Format(Properties.Resources.SerializationReader_GetString_InvalidCast, field.DataTypeName));
            else
                return ((FtStringField)field).Value;
        }
        public object GetValue(int idx) { return FieldList[idx].AsObject; }
        public int GetValues(object[] values)
        {
            int fieldCount = FieldCount;
            if (fieldCount == 0)
            {
                values = null;
                return 0;
            }
            else
            {
                values = new object[fieldCount];
                for (int i = 0; i < fieldCount; i++)
                {
                    values[i] = FieldList[i].AsObject;
                }
                return fieldCount;
            }
        }
        public int RecordsAffected { get { return -1; } }
        public IDataReader GetData(int idx) { throw new NotSupportedException(Properties.Resources.FtDataReader_GetData_NotSupported); }
        public bool IsClosed { get { return charReader.IsClosed; } }
        public DataTable GetSchemaTable() { return null; }
        public bool NextResult() { return NextTable(); }
        public byte GetByte(int idx) { throw new NotSupportedException(Properties.Resources.FtDataReader_GetByte_NotSupported); }
        public long GetBytes(int idx, long fieldOffset, byte[] buffer, int bufferoffset, int length) { throw new NotSupportedException(Properties.Resources.FtDataReader_GetBytes_NotSupported); }
        public char GetChar(int idx) { throw new NotSupportedException(Properties.Resources.FtDataReader_GetChar_NotSupported); }
        public long GetChars(int idx, long fieldoffset, char[] buffer, int bufferoffset, int length) { throw new NotSupportedException(Properties.Resources.FtDataReader_GetChars_NotSupported); }
        public float GetFloat(int idx) { throw new NotSupportedException(Properties.Resources.FtDataReader_GetFloat_NotSupported); }
        public Guid GetGuid(int idx) { throw new NotSupportedException(Properties.Resources.FtDataReader_GetGuid_NotSupported); }
        public short GetInt16(int idx) { throw new NotSupportedException(Properties.Resources.FtDataReader_GetInt16_NotSupported); }
        public int GetInt32(int idx) { throw new NotSupportedException(Properties.Resources.FtDataReader_GetInt32_NotSupported); }

        protected new void Reset()
        {
            base.Reset();

            declarationRead = false;
            embeddedMetaRead = false;
            headingLineReadCount = 0;
            headingLinesRead = false;
            headerRead = !declared && HeadingLineCount == 0;
            finished = false;

            declaredParameters.Clear();
            lineParser.Reset();
            embeddedMetaParser.Reset();
            metaReferenceType = FtMetaReferenceType.None;
            metaReference = "";
            metaEmbedded = false;
            newTableStarted = false;
            tableStartSuspended = false;
            tableStartRecordPeeked = false;
            lineType = FtLineType.Blank;

            lineBuilder.Clear();
        }

        private bool InternalReadLine()
        {
            bool result = true;
            do
            {
                int readCharAsInt = charReader.Read();
                if (readCharAsInt != CharReader.EofReadResult)
                    ParseChar((char)readCharAsInt);
                else
                    result = Finish(); // will always make lineParser.InLine false
            }
            while (lineParser.InLine);

            return result;
        }

        private void ParseChar(char aChar)
        {
            if (!lineParser.InLine)
            {
                // new line starting
                // calculate initial lineType and check if first char is ok

                lineBuilder.Clear();

                lineType = CalculateStartLineType(aChar);
                switch (lineType)
                {
                    case FtLineType.Signature:
                        declarationParser.StartLine();
                        SetLineCommentChar(aChar);
                        break;
                    case FtLineType.Declaration2:
                        declarationParser.StartLine();
                        break;
                    case FtLineType.Comment:
                        if (!headerRead)
                        {
                            if (metaEmbedded && !embeddedMetaRead)
                            {
                                embeddedMetaParser.StartLine();
                            }
                        }
                        break;
                    case FtLineType.EmbeddedMeta:
                        embeddedMetaParser.StartLine();
                        break;
                    case FtLineType.Heading:
                        InvokeRootSequence();
                        int headingLineReadIndex = headingLineReadCount++;
                        headingLineParser.Start(headingLineReadIndex);
                        OnHeadingLineStarted(headingLineReadIndex);
                        break;
                }
            }

            bool endOfLineToBeEmbedded = recordParser.IsEndOfLineToBeEmbedded();

            LineParser.LineEndedType lineEndedType;
            lineParser.ParseChar(aChar, endOfLineToBeEmbedded, out lineEndedType);

            if (lineEndedType != LineParser.LineEndedType.Continued)
            {
                bool lineEndInitiated;
                if (lineEndedType == LineParser.LineEndedType.Initiated)
                    lineEndInitiated = true;
                else
                {
                    lineEndInitiated = false;
                    lineBuilder.Append(aChar);
                }

                switch (lineType)
                {
                    case FtLineType.Signature:
                        if (!lineEndInitiated)
                            declarationParser.ParseSignatureLineChar(aChar);
                        else
                            declarationParser.FinishSignatureLine();
                        break;

                    case FtLineType.Declaration2:
                        if (!lineEndInitiated)
                            declarationParser.ParseDeclaration2LineChar(aChar);
                        else
                            FinishDeclaration();
                        break;

                    case FtLineType.Comment:
                        if (!headerRead)
                        {
                            if (metaEmbedded && !embeddedMetaRead)
                            {
                                if (!lineEndInitiated)
                                {
                                    // see if embedded Meta started in this line
                                    bool embeddedMetaPresent;
                                    embeddedMetaParser.ParseNotYetDetectedChar(aChar, out embeddedMetaPresent);
                                    if (embeddedMetaPresent)
                                    {
                                        lineType = FtLineType.EmbeddedMeta;
                                    }
                                }
                            }
                        }
                        break;

                    case FtLineType.EmbeddedMeta:
                        if (!lineEndInitiated)
                            embeddedMetaParser.ParseChar(aChar);
                        else
                        {
                            embeddedMetaParser.AppendLine();
                            if (embeddedMetaParser.Ready)
                            {
                                FinishEmbeddedMeta();
                            }
                        }
                        break;

                    case FtLineType.Blank:
                        if (!headerRead)
                        {
                            if (!lineEndInitiated)
                            {
                                // must be a heading line
                                lineType = FtLineType.Heading;
                                if (metaEmbedded && !embeddedMetaRead)
                                    // error if expecting embedded Meta
                                    throw new FtSerializationException(FtSerializationError.EmbeddedMetaNotFound, Properties.Resources.SerializationReader_ParseChar_EmbeddedMetaLineNotBlankOrComment);
                                else
                                {
                                    Debug.Assert(HeadingLineCount > 0 && !headingLinesRead);
                                    InvokeRootSequence();
                                    int headingLineReadIndex = headingLineReadCount++;
                                    headingLineParser.Start(headingLineReadIndex);
                                    OnHeadingLineStarted(headingLineReadIndex);
                                    headingLineParser.ParseChar(aChar);
                                }
                            }
                        }
                        else
                        {
                            if (!lineEndInitiated)
                            {
                                // is actually a record line
                                lineType = FtLineType.Record;
                                if (newTableStarted)
                                {
                                    PreviousRecordSequenceInvokationList.Assign(SequenceInvokationList);
                                    newTableStarted = false;
                                }
                                InvokeRootSequence();
                                int recordIndex = recordCount++;
                                recordParser.Start(recordIndex);
                                OnRecordStarted(recordIndex);
                                recordParser.ParseChar(aChar);
                            }
                            else
                            {
                                if (!IgnoreBlankLines)
                                {
                                    throw new FtSerializationException(FtSerializationError.RecordNotEnoughFields, Properties.Resources.SerializationReader_ParseChar_BlankRecordsNotAllowed);
                                }
                            }
                        }

                        break;

                    case FtLineType.Heading:
                        if (!lineEndInitiated)
                            headingLineParser.ParseChar(aChar);
                        else
                            FinishHeadingLine();
                        break;

                    case FtLineType.Record:
                        if (!lineEndInitiated)
                            recordParser.ParseChar(aChar);
                        else
                            FinishRecord();
                        break;

                    default:
                        throw FtInternalException.Create(InternalError.SerializationReader_ParseChar_UnsupportedLineType, lineType.ToString());
                }
            }
        }

        private FtLineType CalculateStartLineType(char startChar)
        {
            if (headerRead)
                return (startChar == LineCommentChar) ? FtLineType.Comment : FtLineType.Blank;  // if not comment, assume blank - if got chars, then will be changed to Rec
            else
            {
                if (declared && lineParser.LineCount < 2)
                {
                    // Declaration Line Type
                    switch (lineParser.LineCount)
                    {
                        case 0: return FtLineType.Signature;
                        case 1: return FtLineType.Declaration2;
                        default:
                            throw FtInternalException.Create(InternalError.SerializationReader_ParseChar_UnexpectedDeclarationLineCount, lineParser.LineCount.ToString());
                    }
                }
                else
                {
                    if (metaEmbedded && !embeddedMetaRead)
                    {
                        // Expecting or in Embedded Meta lines
                        if (lineType == FtLineType.EmbeddedMeta)
                            return FtLineType.EmbeddedMeta;
                        else
                        {
                            if (startChar == LineCommentChar)
                                return FtLineType.Comment;
                            else
                                return FtLineType.Blank; // the only other thing it could be
                        }
                    }
                    else
                    {
                        // if Declaration done or not present and Embedded Meta done or not present, then only reason
                        // still in header is to read heading lines.

                        Debug.Assert(HeadingLineCount > 0 && !headingLinesRead);

                        if (lineType == FtLineType.Heading)
                            return FtLineType.Heading;
                        else
                        {
                            if (startChar == LineCommentChar)
                                return FtLineType.Comment;
                            else
                                return FtLineType.Blank; // assume this - may also be first heading line
                        }
                    }
                }
            }
        }

        private bool Finish()
        {
            bool result;

            if (finished)
                result = false;
            else
            {
                if (lineParser.InLine)
                {
                    FinishLastLine();

                    if (LastLineEndedType == FtLastLineEndedType.Always)
                        throw new FtSerializationException(FtSerializationError.LastLineEndedError, Properties.Resources.LineParser_FinishLastLine_AlwaysEndedButIn);
                    else
                    {
                        lineParser.ExitLine();
                        result = true;
                    }
                }
                else
                {
                    if (LastLineEndedType != FtLastLineEndedType.Never)
                        result = false;
                    else
                    {
                        if (!IgnoreBlankLines)
                            throw new FtSerializationException(FtSerializationError.LastLineEndedError, Properties.Resources.LineParser_FinishLastLine_NeverEndedButOut);
                        else
                        {
                            // since last line cannot be ended, there is another ignored blank line

                            // throw exception if blank line not allowed
                            switch (lineType)
                            {
                                case FtLineType.Signature:
                                    throw new FtSerializationException(FtSerializationError.IncompleteDeclaration, Properties.Resources.SerializationReader_Finish_DeclarationMissingSecondLine);
                                case FtLineType.Heading:
                                    if (!headingLinesRead)
                                    {
                                        throw new FtSerializationException(FtSerializationError.InsufficientHeadingLines, "");
                                    }
                                    break;
                            }

                            // set up blank line
                            lineParser.AddBlankLine();
                            lineType = FtLineType.Blank;
                            result = true;
                        }
                    }
                }

                finished = true;
            }

            if (!result)
            {
                newTableStarted = false;
                CheckCompleteness();
            }

            return result;
        }

        private void FinishLastLine()
        {
            switch (lineType)
            {
                case FtLineType.Signature:
                    declarationParser.FinishSignatureLine();
                    break;

                case FtLineType.Declaration2:
                    FinishDeclaration();
                    break;

                case FtLineType.Comment:
                    if (!headerRead)
                    {
                        if (metaEmbedded && !embeddedMetaRead)
                        {
                            throw new FtSerializationException(FtSerializationError.EmbeddedMetaNotFound, "End of File encountered");
                        }
                    }
                    break;

                case FtLineType.EmbeddedMeta:
                    if (embeddedMetaParser.Ready)
                        FinishEmbeddedMeta();
                    else
                        throw new FtSerializationException(FtSerializationError.IncompleteEmbeddedMeta, "End of File encountered");
                    break;

                case FtLineType.Blank:
                    break; // nothing to do

                case FtLineType.Heading:
                    FinishHeadingLine();
                    break;

                case FtLineType.Record:
                    FinishRecord();
                    break;

                default:
                    throw FtInternalException.Create(InternalError.SerializationReader_FinishLastLine_UnsupportedLineType, lineType.ToString());
            }
        }

        private void ReadDeclaration()
        {
            Debug.Assert(declared, "ReadDeclaration called but not declared");

            while (!declarationRead)
            {
                if (!InternalReadLine())
                {
                    break;
                }
            }
        }

        private void ReadEmbeddedMeta()
        {
            Debug.Assert(metaReferenceType == FtMetaReferenceType.Embedded, "ReadEmbeddedMeta called but meta not embedded");

            while (!embeddedMetaRead)
            {
                if (!InternalReadLine())
                {
                    break;
                }
            }
        }

        private void ReadHeadingLines()
        {
            Debug.Assert(HeadingLineCount > 0, "ReadHeadingLines called but no headings");

            while (!headingLinesRead)
            {
                if (!InternalReadLine())
                {
                    break;
                }
            }
        }

        private void FinishDeclaration()
        {
            declarationParser.Finish();

            declaredParameters.GetMetaReference(out metaReferenceType, out metaReference);
            switch (metaReferenceType)
            {
                case FtMetaReferenceType.Embedded:
                    metaEmbedded = true;
                    break;

                case FtMetaReferenceType.File:
                    LoadMetaFromFile(metaReference);
                    break;

                case FtMetaReferenceType.Url:
                    try
                    {
                        LoadMetaFromUrl(metaReference);
                    }
                    catch (FtMetaSerializationException e)
                    {
                        throw new FtSerializationException(FtSerializationError.LoadMetaFromUrl, e.Message, e);
                    }
                    break;
            }
            declarationRead = true;

            if (!metaEmbedded && HeadingLineCount == 0)
            {
                headerRead = true;
            }
        }

        private void FinishEmbeddedMeta()
        {
            string metaAsString = embeddedMetaParser.TakeMetaAsString();
            LoadMetaFromText(metaAsString);
            embeddedMetaRead = true;
            if (HeadingLineCount == 0)
            {
                headerRead = true;
            }
        }

        private void LoadMetaFromText(string text)
        {
            // note that XML Encoding in text is probably original file encoding instead of string encoding.  XmlReader seems to handle this ok.
            StringReader stringReader = new StringReader(text);
            XmlReader xmlReader = XmlReader.Create(stringReader);
            FtMeta meta;
            MetaSerialization.XmlMetaSerializationReader serializationReader = new MetaSerialization.XmlMetaSerializationReader();
            try
            {
                meta = serializationReader.Read(xmlReader);
            }
            catch (FtMetaSerializationException e)
            {
                throw new FtSerializationException(FtSerializationError.LoadMetaFromText, e.Message, e);
            }
            LoadMeta(meta);
        }

        private void LoadMetaFromFile(string filePath)
        {
            StreamReader streamReader = new StreamReader(filePath);
            XmlReader xmlReader = XmlReader.Create(streamReader);
            FtMeta meta;
            MetaSerialization.XmlMetaSerializationReader serializationReader = new MetaSerialization.XmlMetaSerializationReader();
            try
            {
                meta = serializationReader.Read(xmlReader);
            }
            catch (FtMetaSerializationException e)
            {
                throw new FtSerializationException(FtSerializationError.LoadMetaFromFile, "File: \"" + filePath + "\" Error: " + e.Message, e);
            }
            LoadMeta(meta);
        }

        private void LoadMetaFromUrl(string url)
        {
            FtMeta meta;
            try
            {
                Uri uri = new Uri(url);
                WebRequest request = WebRequest.Create(uri);
                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                XmlReader xmlReader = XmlReader.Create(stream);
                MetaSerialization.XmlMetaSerializationReader serializationReader = new MetaSerialization.XmlMetaSerializationReader();
                meta = serializationReader.Read(xmlReader);
            }
            catch (FtMetaSerializationException e)
            {
                throw new FtSerializationException(FtSerializationError.LoadMetaFromUrl, "Url: \"" + url + "\" Error: " + e.Message, e);
            }
            LoadMeta(meta);
        }

        private void FinishHeadingLine()
        {
            headingLineParser.Finish();
            OnHeadingLineFinished(headingLineReadCount - 1);

            if (headingLineReadCount >= HeadingLineCount)
            {
                headingLinesRead = true;
                headerRead = true;
            }
        }

        private void FinishRecord()
        {
            recordParser.Finish();
            OnRecordFinished(recordCount - 1);

            if (!SequenceInvokationList.Matches(PreviousRecordSequenceInvokationList))
            {
                tableCount++;
                newTableStarted = true;
            }
        }

        private void CheckCompleteness()
        {
            if (!headerRead)
            {
                if (declared && !declarationRead)
                    throw new FtSerializationException(FtSerializationError.IncompleteDeclaration, "");
                else
                {
                    if (metaEmbedded && !embeddedMetaRead)
                        throw new FtSerializationException(FtSerializationError.IncompleteEmbeddedMeta, "");
                    else
                    {
                        if (HeadingLineCount > 0 && !headingLinesRead)
                        {
                            throw new FtSerializationException(FtSerializationError.InsufficientHeadingLines, headingLineReadCount.ToString());
                        }
                    }
                }
            }
        }
    }
}
