// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;
using System.Globalization;
using System.Diagnostics;

namespace Xilytix.FieldedText.Serialization
{
    public abstract class SerializationCore
    {
        protected const string Signature = CharReader.Signature;
        protected const string VersionLabel = "Version";
        protected const char VersionSeparator = '.';
        internal protected const char CarriageReturnChar = CharReader.CarriageReturnChar;
        internal protected const char LineFeedChar = CharReader.LineFeedChar;
        internal protected const string CarriageReturnLineFeedString = CharReader.CarriageReturnLineFeedString;

        public const int VersionMajor = 1;
        public const int VersionMinor = 1;
        public const char PrefixSpaceChar = ' ';

        private FtFieldDefinitionList fieldDefinitionList;
        private FtFieldList fieldList;
        private FtSubstitutionList substitutionList;
        private FtSequenceList sequenceList;

        private bool metaLoaded;

        private CultureInfo culture;
        private FtEndOfLineType endOfLineType;
        private char endOfLineChar;
        private FtEndOfLineAutoWriteType endOfLineAutoWriteType;
        private FtLastLineEndedType lastLineEndedType;
        private char quoteChar;
        private char delimiterChar;
        private char lineCommentChar;
        private bool allowEndOfLineCharInQuotes;
        private bool ignoreBlankLines;
        private bool ignoreExtraChars;
        private bool allowIncompleteRecords;
        private bool stuffedEmbeddedQuotes;
        private bool substitutionsEnabled;
        private char substitutionChar;
        private int headingLineCount;
        private int mainHeadingLineIndex;
        private FtHeadingConstraint headingConstraint;
        private FtQuotedType headingQuotedType;
        private bool headingAlwaysWriteOptionalQuote;
        private bool headingWritePrefixSpace;
        private FtPadAlignment headingPadAlignment;
        private FtPadCharType headingPadCharType;
        private char headingPadChar;
        private FtTruncateType headingTruncateType;
        private char headingTruncateChar;
        private char headingEndOfValueChar;

        private FtSequence rootSequence;
        private int rootFieldCount;

        // reset members
        private FtSequenceInvokationList sequenceInvokationList;
        private FtSequenceInvokationList previousRecordSequenceInvokationList;
        private FtSequenceInvokation rootSequenceInvokation;
        private bool seeking;
        protected int recordCount;
        protected int tableCount;

        protected FtSequenceInvokationList PreviousRecordSequenceInvokationList { get { return previousRecordSequenceInvokationList; } }

        protected SerializationCore()
        {
            fieldDefinitionList = new FtFieldDefinitionList();
            fieldList = new FtFieldList();
            substitutionList = new FtSubstitutionList();
            sequenceList = new FtSequenceList();
            sequenceInvokationList = new FtSequenceInvokationList();
            sequenceInvokationList.SequenceRedirectEvent = HandleSequenceRedirectEvent;
            previousRecordSequenceInvokationList = new FtSequenceInvokationList();
        }

        public FtFieldDefinitionList FieldDefinitionList { get { return fieldDefinitionList; } }
        public FtFieldList FieldList {get {return fieldList; } }
        public FtSubstitutionList SubstitutionList { get { return substitutionList; } }
        public FtSequenceList SequenceList { get { return sequenceList; } }
        public FtSequenceInvokationList SequenceInvokationList { get { return sequenceInvokationList; } }
        public FtSequence RootSequence { get { return rootSequence; } }
        public int RootFieldCount { get { return rootFieldCount; } }
        public FtSequenceInvokation RootSequenceInvokation { get { return rootSequenceInvokation; } }

        public bool Seeking { get { return seeking; } }
        public int RecordCount { get { return recordCount; } }
        public int TableCount { get { return tableCount; } }

        public CultureInfo Culture { get { return culture; } }
        public FtEndOfLineType EndOfLineType { get { return endOfLineType; } }
        public char EndOfLineChar { get { return endOfLineChar; } }
        public FtEndOfLineAutoWriteType EndOfLineAutoWriteType { get { return endOfLineAutoWriteType; } }
        public FtLastLineEndedType LastLineEndedType { get { return lastLineEndedType; } }
        public char QuoteChar { get { return quoteChar; } }
        public char DelimiterChar { get { return delimiterChar; } }
        public char LineCommentChar { get { return lineCommentChar; } }
        public bool AllowEndOfLineCharInQuotes { get { return allowEndOfLineCharInQuotes; } }
        public bool IgnoreBlankLines { get { return ignoreBlankLines; } }
        public bool IgnoreExtraChars { get { return ignoreExtraChars; } }
        public bool AllowIncompleteRecords { get { return allowIncompleteRecords; } }
        public bool StuffedEmbeddedQuotes { get { return stuffedEmbeddedQuotes; } }
        public bool SubstitutionsEnabled { get { return substitutionsEnabled; } }
        public char SubstitutionChar { get { return substitutionChar; } }
        public int HeadingLineCount { get { return headingLineCount; } }
        public int MainHeadingLineIndex { get { return mainHeadingLineIndex; } }
        public FtHeadingConstraint HeadingConstraint { get { return headingConstraint; } }
        public FtQuotedType HeadingQuotedType { get { return headingQuotedType; } }
        public bool HeadingAlwaysWriteOptionalQuote { get { return headingAlwaysWriteOptionalQuote; } }
        public bool HeadingWritePrefixSpace { get { return headingWritePrefixSpace; } }
        public FtPadAlignment HeadingPadAlignment { get { return headingPadAlignment; } }
        public FtPadCharType HeadingPadCharType { get { return headingPadCharType; } }
        public char HeadingPadChar { get { return headingPadChar; } }
        public FtTruncateType HeadingTruncateType { get { return headingTruncateType; } }
        public char HeadingTruncateChar { get { return headingTruncateChar; } }
        public char HeadingEndOfValueChar { get { return headingEndOfValueChar; } }

        public event EventHandler<FtFieldHeadingReadyEventArgs> FieldHeadingReadReady;
        public event EventHandler<FtFieldHeadingReadyEventArgs> FieldHeadingWriteReady;
        public event EventHandler<FtFieldValueReadyEventArgs> FieldValueReadReady;
        public event EventHandler<FtFieldValueReadyEventArgs> FieldValueWriteReady;
        public event EventHandler<FtHeadingLineStartedEventArgs> HeadingLineStarted;
        public event EventHandler<FtHeadingLineFinishedEventArgs> HeadingLineFinished;
        public event EventHandler<FtRecordStartedEventArgs> RecordStarted;
        public event EventHandler<FtRecordFinishedEventArgs> RecordFinished;
        public event EventHandler<FtSequenceRedirectedEventArgs> SequenceRedirected;

        public virtual void LoadMeta(FtMeta meta)
        {
            InternalLoadMeta(meta);
        }

        // for IDataReader
        public int FieldCount { get { return FieldList.Count; } }
        public string GetName(int idx) { return FieldList[idx].Name; }
        /// <summary>
        /// Returns ordinal of a field in the current record.  The field is specified by its (case insensitive) field name.
        /// </summary>
        /// <param name="id">Field Name</param>
        /// <returns>Field ordinal in current record</returns>
        public int GetOrdinal(string name)
        {
            int result = FieldList.IndexOfName(name);
            if (result >= 0)
                return result;
            else
                throw new IndexOutOfRangeException(string.Format(Properties.Resources.SerializationCore_GetOrdinal_IdNotFound, name));
        }
        public Type GetFieldType(int idx) { return FieldList[idx].ValueType; }
        public string GetDataTypeName(int idx) { return FieldList[idx].DataTypeName; }
        public bool IsDBNull(int idx) { return FieldList[idx].IsNull(); }
        public int Depth { get { return SequenceInvokationList.Count; } }

        // extra methods related to IDataReader

        /// <summary>
        /// Returns ordinal of a field in the current record.  The field is specified by its field Id.
        /// </summary>
        /// <param name="id">Field Id</param>
        /// <returns>Field ordinal in current record</returns>
        public int GetOrdinal(int fieldId)
        {
            int result = FieldList.IndexOfId(fieldId);
            if (result >= 0)
                return result;
            else
                throw new IndexOutOfRangeException(string.Format(Properties.Resources.SerializationCore_GetOrdinal_IdNotFound, fieldId));
        }

        internal void InternalLoadMeta(FtMeta meta)
        {
            sequenceList.Clear();
            substitutionList.Clear();
            fieldList.Clear();
            fieldDefinitionList.Clear();

            string errorMessage;
            if (!meta.Validate(out errorMessage))
                throw new FtSerializationException(FtSerializationError.InvalidMeta, errorMessage);
            else
            {
                culture = meta.Culture;
                endOfLineType = meta.EndOfLineType;
                endOfLineChar = meta.EndOfLineChar;
                endOfLineAutoWriteType = meta.EndOfLineAutoWriteType;
                lastLineEndedType = meta.LastLineEndedType;
                quoteChar = meta.QuoteChar;
                delimiterChar = meta.DelimiterChar;
                lineCommentChar = meta.LineCommentChar;
                allowEndOfLineCharInQuotes = meta.AllowEndOfLineCharInQuotes;
                ignoreBlankLines = meta.IgnoreBlankLines;
                ignoreExtraChars = meta.IgnoreExtraChars;
                allowIncompleteRecords = meta.AllowIncompleteRecords;
                stuffedEmbeddedQuotes = meta.StuffedEmbeddedQuotes;
                substitutionsEnabled = meta.SubstitutionsEnabled;
                substitutionChar = meta.SubstitutionChar;
                headingLineCount = meta.HeadingLineCount;
                mainHeadingLineIndex = meta.MainHeadingLineIndex;
                headingConstraint = meta.HeadingConstraint;
                headingQuotedType = meta.HeadingQuotedType;
                headingAlwaysWriteOptionalQuote = meta.HeadingAlwaysWriteOptionalQuote;
                headingWritePrefixSpace = meta.HeadingWritePrefixSpace;
                headingPadAlignment = meta.HeadingPadAlignment;
                headingPadCharType = meta.HeadingPadCharType;
                headingPadChar = meta.HeadingPadChar;
                headingTruncateType = meta.HeadingTruncateType;
                headingTruncateChar = meta.HeadingTruncateChar;
                headingEndOfValueChar = meta.HeadingEndOfValueChar;

                fieldDefinitionList.Capacity = meta.FieldList.Count;
                fieldList.Capacity = meta.FieldList.Count;
                for (int i = 0; i < meta.FieldList.Count; i++)
                {
                    FtFieldDefinition fieldDefinition = fieldDefinitionList.New(meta.FieldList[i].DataType);
                    fieldDefinition.LoadMeta(meta.FieldList[i], culture, mainHeadingLineIndex);
                }
                for (int i = 0; i < meta.SubstitutionList.Count; i++)
                {
                    FtSubstitution substitution = substitutionList.New();
                    substitution.LoadMeta(meta.SubstitutionList[i], endOfLineAutoWriteType);
                }

                if (meta.SequenceList.Count == 0)
                {
                    // create a root sequence with all field definitions
                    rootSequence = sequenceList.New(fieldDefinitionList);
                }
                else
                {
                    rootSequence = null;
                    for (int i = 0; i < meta.SequenceList.Count; i++)
                    {
                        FtSequence sequence = sequenceList.New();
                        sequence.LoadMeta(meta.SequenceList[i], meta.FieldList, fieldDefinitionList);

                        if (sequence.Root)
                        {
                            rootSequence = sequence;
                        }
                    }

                    if (rootSequence == null && sequenceList.Count > 0)
                    {
                        rootSequence = sequenceList[0];
                        rootSequence.SetRoot(true);
                    }

                    // must load redirects after ALL sequences are loaded
                    for (int i = 0; i < sequenceList.Count; i++)
                    {
                        sequenceList[i].LoadMetaSequenceRedirects(meta.SequenceList[i], meta.SequenceList, sequenceList);
                    }
                }

                rootFieldCount = rootSequence.ItemList.Count;

                metaLoaded = true;
            }
        }

        protected void SetSeeking(bool value)
        {
            seeking = value;
        }

        protected void SetLineCommentChar(char aChar)
        {
            lineCommentChar = aChar;
        }

        internal protected void OnFieldHeadingReadReady(FtField field, int lineIndex)
        {
            if (FieldHeadingReadReady != null)
            {
                FtFieldHeadingReadyEventArgs e = new FtFieldHeadingReadyEventArgs();
                e.Field = field;
                e.LineIndex = lineIndex;
                FieldHeadingReadReady.Invoke(this, e);
            }
        }

        internal protected void OnFieldHeadingWriteReady(FtField field, int lineIndex)
        {
            if (FieldHeadingWriteReady != null)
            {
                FtFieldHeadingReadyEventArgs e = new FtFieldHeadingReadyEventArgs();
                e.Field = field;
                e.LineIndex = lineIndex;
                FieldHeadingWriteReady.Invoke(this, e);
            }
        }

        internal protected void OnFieldValueReadReady(FtField field, int recordIndex)
        {
            if (FieldValueReadReady != null)
            {
                FtFieldValueReadyEventArgs e = new FtFieldValueReadyEventArgs();
                e.Field = field;
                e.RecordIndex = recordIndex;
                FieldValueReadReady.Invoke(this, e);
            }
        }

        internal protected void OnFieldValueWriteReady(FtField field, int recordIndex)
        {
            if (FieldValueWriteReady != null)
            {
                FtFieldValueReadyEventArgs e = new FtFieldValueReadyEventArgs();
                e.Field = field;
                e.RecordIndex = recordIndex;
                FieldValueWriteReady.Invoke(this, e);
            }
        }

        internal protected void OnHeadingLineStarted(int lineIndex)
        {
            if (HeadingLineStarted != null)
            {
                FtHeadingLineStartedEventArgs e = new FtHeadingLineStartedEventArgs();
                e.LineIndex = lineIndex;
                HeadingLineStarted.Invoke(this, e);
            }
        }

        internal protected void OnHeadingLineFinished(int lineIndex)
        {
            if (HeadingLineFinished != null)
            {
                FtHeadingLineFinishedEventArgs e = new FtHeadingLineFinishedEventArgs();
                e.LineIndex = lineIndex;
                HeadingLineFinished.Invoke(this, e);
            }
        }

        internal protected void OnRecordStarted(int recordIndex)
        {
            if (RecordStarted != null)
            {
                FtRecordStartedEventArgs e = new FtRecordStartedEventArgs();
                e.RecordIndex = recordIndex;
                RecordStarted.Invoke(this, e);
            }
        }

        internal protected void OnRecordFinished(int recordIndex)
        {
            if (RecordFinished != null)
            {
                FtRecordFinishedEventArgs e = new FtRecordFinishedEventArgs();
                e.RecordIndex = recordIndex;
                RecordFinished.Invoke(this, e);
            }
        }

        protected void OnSequenceRedirected(FtField redirectingField, int fieldsAffectedFromIndex)
        {
            if (SequenceRedirected != null)
            {
                FtSequenceRedirectedEventArgs e = new FtSequenceRedirectedEventArgs();
                e.RedirectingField = redirectingField;
                e.FieldsAffectedFromIndex = fieldsAffectedFromIndex;
                SequenceRedirected.Invoke(this, e);
            }
        }

        protected void Reset()
        {
            seeking = false;
            recordCount = 0;
            tableCount = 0;

            sequenceInvokationList.Clear();
            previousRecordSequenceInvokationList.Clear();
            rootSequenceInvokation = null;
            fieldList.Clear();
        }

        private void ResetAllFieldValues()
        {
            for (int i = 0; i < FieldList.Count; i++)
            {
                FieldList[i].ResetValue();
            }
        }

        private void ResetFieldValues(int fromIndex, int count)
        {
            for (int i = fromIndex; i < fromIndex + count; i++)
            {
                FieldList[i].ResetValue();
            }
        }

        protected void InvokeRootSequence()
        {
            switch (SequenceInvokationList.Count)
            {
                case 0:
                    Debug.Assert(FieldList.Count == 0, "FieldList.Count > 0 when no sequence invokations");
                    rootSequenceInvokation = SequenceInvokationList.New(rootSequence, 0);
                    for (int i = 0; i < rootSequenceInvokation.FieldCount; i++)
                    {
                        FieldList.Add(rootSequenceInvokation.GetField(i));
                    }
                    break;
                case 1:
                    // if 1, then root is only invoked sequence
                    ResetAllFieldValues();
                    break;
                default:
                    FtSequenceInvokation secondInvokation = SequenceInvokationList[1];
                    fieldList.Trim(secondInvokation.StartFieldIndex);
                    bool fieldsWereSidelined = secondInvokation.StartFieldIndex != RootFieldCount;

                    SequenceInvokationList.PredictTrim(1);
                    ResetAllFieldValues();
                    if (fieldsWereSidelined)
                    {
                        // Root sequence was redirected before its end. Some fields will be sidelined and field indexes may not be correct
                        for (int i = rootSequenceInvokation.FieldsSidelinedFromIndex; i < rootSequenceInvokation.FieldCount; i++)
                        {
                            fieldList.Add(rootSequenceInvokation.GetField(i));
                        }
                        rootSequenceInvokation.UnsidelineFields();
                    }
                    break;
            }
        }

        /*protected void InvokeRootSequenceFromPrevious()
        {
            // NOT USED.  Optimisation for InvokingRoot from Previous as used in SerializatonWriter

            // SequenceInvokationList however FieldList matches PreviousRecordSequenceInvokationList

            // Make SequenceInvokationList hold root only
            SequenceInvokationList.New(RootSequence, 0);

            // Adjust FieldList based on PreviousRecordSequenceInvokationList
            // if PreviousRecordSequenceInvokationList = 1 (wont be 0), then FieldList already only contains root fields
            if (PreviousRecordSequenceInvokationList.Count <= 1)
                ResetAllFieldValues();
            else
            {
                // if root sequence was not redirected before its end, keep existing as prediction
                FtSequenceInvokation secondInvokation = PreviousRecordSequenceInvokationList[1];
                if (secondInvokation.StartFieldIndex == RootFieldCount)
                {
                    FieldList.PredictTrim(RootFieldCount);
                    ResetAllFieldValues();
                }
                else
                {
                    int replaceFieldFromIndex = secondInvokation.StartFieldIndex;
                    FieldList.Trim(replaceFieldFromIndex);
                    ResetAllFieldValues();
                    for (int i = replaceFieldFromIndex; i < RootFieldCount; i++)
                    {
                        FieldList.New(RootSequenceInvokation, RootSequence.ItemList[i]);
                    }
                }
            }
        }*/

        private void HandleSequenceRedirectEvent(FtField field, FtSequence invokedSequence, FtSequenceInvokationDelay invokationDelay,
                                                 out int fieldsAffectedFromIndex)
        {
            if (invokedSequence != null)
                Redirect(field, invokedSequence, invokationDelay, out fieldsAffectedFromIndex);
            else
                Unredirect(field, out fieldsAffectedFromIndex);
        }

        protected void Redirect(FtField redirectingField, FtSequence invokedSequence, FtSequenceInvokationDelay invokationDelay, 
                                out int fieldsAffectedFromIndex)
        {
            FtSequenceInvokation redirectingInvokation = redirectingField.SequenceInvokation;

            int newInvokationIndex = redirectingInvokation.Index + 1;
            int newInvokationStartFieldIndex;
            switch (invokationDelay)
            {
                case FtSequenceInvokationDelay.ftikAfterField:
                    newInvokationStartFieldIndex = redirectingField.Index + 1;
                    break;
                case FtSequenceInvokationDelay.ftikAfterSequence:
                    newInvokationStartFieldIndex = redirectingInvokation.StartFieldIndex + redirectingInvokation.FieldCount;
                    break;
                default:
                    throw FtInternalException.Create(InternalError.Core_InvokeSequences_UnsupportedInvokationDelay, invokationDelay.ToString());
            }


            // check if redirect does not cause any change
            bool changeRequired;
            if (newInvokationIndex >= sequenceInvokationList.Count)
                changeRequired = true;
            else
            {
                FtSequenceInvokation nextExistingInvokation = sequenceInvokationList[newInvokationIndex];
                changeRequired = (nextExistingInvokation.Sequence != invokedSequence || nextExistingInvokation.StartFieldIndex != newInvokationStartFieldIndex);
            }

            if (!changeRequired)
                fieldsAffectedFromIndex = FtField.NoFieldsAffectedIndex;
            else
            {
                FtField existingRedirectingField = redirectingInvokation.RedirectingField;
                int unredirectfieldsAffectedFromIndex;
                if (existingRedirectingField == null)
                    unredirectfieldsAffectedFromIndex = redirectingInvokation.StartFieldIndex + redirectingInvokation.FieldCount;
                else
                {
                    Unredirect(existingRedirectingField, out unredirectfieldsAffectedFromIndex);
                    if (unredirectfieldsAffectedFromIndex < 0)
                    {
                        // if not affected, set to value which will be higher than new invokation affects fields index
                        unredirectfieldsAffectedFromIndex = redirectingInvokation.StartFieldIndex + redirectingInvokation.FieldCount;
                    }
                }

                FtSequenceInvokation newInvokation = SequenceInvokationList.TryPredictedNew(newInvokationIndex, invokedSequence, newInvokationStartFieldIndex);
                if (newInvokation == null)
                {
                    SequenceInvokationList.Trim(newInvokationIndex);
                    newInvokation = SequenceInvokationList.New(invokedSequence, newInvokationStartFieldIndex);
                }

                FieldList.Trim(newInvokationStartFieldIndex);
                for (int i = 0; i < newInvokation.FieldCount; i++)
                {
                    FieldList.Add(newInvokation.GetField(i));
                }

                if (newInvokationStartFieldIndex < redirectingInvokation.StartFieldIndex + redirectingInvokation.FieldCount)
                {
                    redirectingInvokation.SidelineFields(newInvokationStartFieldIndex);
                }

                if (unredirectfieldsAffectedFromIndex < newInvokationStartFieldIndex)
                    fieldsAffectedFromIndex = unredirectfieldsAffectedFromIndex;
                else
                    fieldsAffectedFromIndex = newInvokationStartFieldIndex;

                if (!seeking)
                {
                    OnSequenceRedirected(redirectingField, fieldsAffectedFromIndex);
                }
            }
        }

        // remove all sequences after that which contains unRedirecting Field
        protected void Unredirect(FtField unredirectingField, out int fieldsAffectedFromIndex)
        {
            FtSequenceInvokation unredirectingInvokation = unredirectingField.SequenceInvokation;
            int unredirectingInvokationIndex = unredirectingInvokation.Index;

            if (unredirectingInvokationIndex >= SequenceInvokationList.Count-1)
                // unRedirectingInvokationIndex is last.  Nothing to do. (should never happen)
                fieldsAffectedFromIndex = FtField.NoFieldsAffectedIndex;
            else
            {
                int nextInvokationIndex = unredirectingInvokationIndex + 1;
                SequenceInvokationList.PredictTrim(nextInvokationIndex);

                fieldsAffectedFromIndex = unredirectingInvokation.StartFieldIndex + unredirectingInvokation.FieldsSidelinedFromIndex;
                fieldList.Trim(fieldsAffectedFromIndex);
                for (int i = unredirectingInvokation.FieldsSidelinedFromIndex; i < unredirectingInvokation.FieldCount; i++)
                {
                    fieldList.Add(unredirectingInvokation.GetField(i));
                }
                unredirectingInvokation.UnsidelineFields();
            }
        }
    }
}
