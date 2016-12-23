// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System.Diagnostics;

namespace Xilytix.FieldedText.Serialization
{
    internal struct HeadingLineRecordParser
    {
        private enum State { Out, InOutField, InDelimitedField, InFixedWidthField, OutIgnoreChars }

        private SerializationCore core;
        private CharReader charReader;
        private bool headingLines;

        private DelimitedFieldParser delimitedFieldParser;
        private FixedWidthFieldParser fixedWidthFieldParser;
        private int index;
        private State state;
        private int fieldCount;
        private FtField activeField;

        private long startPosition;
        private long ignoreExtraCharsStartPosition;

        internal HeadingLineRecordParser(SerializationCore myCore, CharReader myCharReader, bool forheadingLines)
        {
            core = myCore;
            charReader = myCharReader;
            headingLines = forheadingLines;

            delimitedFieldParser = new DelimitedFieldParser(core, charReader, forheadingLines);
            fixedWidthFieldParser = new FixedWidthFieldParser(core, charReader, forheadingLines);
            index = -1;
            state = State.Out;
            fieldCount = 0;
            activeField = null;

            startPosition = -1;
            ignoreExtraCharsStartPosition = -1;
        }

        internal FtField ActiveField { get { return activeField; } }
        internal int GetActiveFieldIndex()
        {
            switch (state)
            {
                case State.Out:
                case State.OutIgnoreChars:
                    return -1;
                default:
                    return fieldCount - 1;
            }
        }

        internal int GetIgnoreExtraCharsLinePosition()
        {
            if (ignoreExtraCharsStartPosition < 0)
                return -1;
            else
                return (int)(ignoreExtraCharsStartPosition - startPosition);
        }

        internal bool IsEndOfLineToBeEmbedded()
        {
            if (state == State.InDelimitedField)
                return delimitedFieldParser.IsEndOfLineToBeEmbedded();
            else
                return false;
        }

        internal void Start(int index)
        {
            Debug.Assert(state == State.Out, "Unexpected RecordParser Start State: " + state.ToString());

            this.index = index;
            fieldCount = 0;
            state = State.InOutField;

            startPosition = charReader.Position;
            ignoreExtraCharsStartPosition = -1;
        }

        internal void ParseChar(char aChar)
        {
            switch (state)
            {
                case State.Out:
                    throw FtInternalException.Create(InternalError.HeadingLineRecordParser_ParseChar_OutState);

                case State.InOutField:
                    if (fieldCount < core.FieldList.Count)
                        EnterField(core.FieldList[fieldCount]);
                    else
                    {
                        FtSerializationError error = headingLines ? FtSerializationError.HeadingLineTooManyFields : FtSerializationError.RecordTooManyFields;
                        string formatString = headingLines ? Properties.Resources.HeadingLineRecordParser_ParseChar_HeadingLineTooManyFields : Properties.Resources.HeadingLineRecordParser_ParseChar_RecordTooManyFields;
                        throw new FtSerializationException(error, string.Format(formatString, core.FieldList.Count));
                    }
                    ParseChar(aChar);
                    break;

                case State.InFixedWidthField:
                    bool previousFieldFinished;
                    fixedWidthFieldParser.ParseChar(aChar, out previousFieldFinished);
                    if (previousFieldFinished)
                    {
                        if (!core.Seeking)
                        {
                            LoadFixedWidthHeadingValue();
                        }

                        ExitActiveField();

                        if (fieldCount < core.FieldList.Count || !core.IgnoreExtraChars)
                            state = State.InOutField;
                        else
                        {
                            state = State.OutIgnoreChars;
                            ignoreExtraCharsStartPosition = charReader.Position;
                        }
                        ParseChar(aChar);
                    }
                    break;

                case State.InDelimitedField:
                    bool fieldFinished;
                    delimitedFieldParser.ParseChar(aChar, out fieldFinished);
                    if (fieldFinished)
                    {
                        // aChar was delimiter
                        if (!core.Seeking)
                        {
                            LoadDelimitedHeadingValue();
                        }

                        ExitActiveField();

                        if (fieldCount < core.FieldList.Count || !core.IgnoreExtraChars)
                            state = State.InOutField;
                        else
                        {
                            state = State.OutIgnoreChars;
                            ignoreExtraCharsStartPosition = charReader.Position;
                        }
                    }
                    break;

                case State.OutIgnoreChars:
                    // nothing to do
                    break;

                default:
                    throw FtInternalException.Create(InternalError.HeadingLineRecordParser_ParseChar_UnsupportedState, state.ToString());
            }
        }

        internal void Finish()
        {
            switch (state)
            {
                case State.Out:
                    // nothing to do
                    break;

                case State.InOutField:
                    state = State.Out;
                    break;

                case State.InFixedWidthField:
                    if (!core.Seeking)
                    {
                        LoadFixedWidthHeadingValue();
                    }
                    ExitActiveField();
                    state = State.Out;
                    break;

                case State.InDelimitedField:
                    if (!core.Seeking)
                    {
                        LoadDelimitedHeadingValue();
                    }
                    ExitActiveField();
                    state = State.Out;
                    break;

                case State.OutIgnoreChars:
                    state = State.Out;
                    break;

                default:
                    throw FtInternalException.Create(InternalError.HeadingLineRecordParser_Finish_UnsupportedState, state.ToString());
            }

            if (fieldCount < core.FieldList.Count)
            {
                if (!core.AllowIncompleteRecords)
                { 
                    FtSerializationError error = headingLines ? FtSerializationError.HeadingLineNotEnoughFields : FtSerializationError.RecordNotEnoughFields;
                    string formatString = headingLines ? Properties.Resources.HeadingLineRecordParser_Finish_HeadingLineNotEnoughFields : Properties.Resources.HeadingLineRecordParser_Finish_RecordNotEnoughFields;
                    throw new FtSerializationException(error, string.Format(formatString, fieldCount, core.FieldList.Count));
                }
                else
                {
                    for (int i = fieldCount; i < core.FieldList.Count; i++)
                    {
                        activeField.LoadPosition(-1, -1, -1, -1);
                        if (headingLines)
                            activeField.LoadHeading(index, "");
                        else
                            activeField.LoadNullValue();
                    }
                }
            }
        }

        private void LoadFixedWidthHeadingValue()
        {
            if (headingLines)
            {
                fixedWidthFieldParser.LoadFieldHeading(index);
                core.OnFieldHeadingReadReady(activeField, index);
            }
            else
            {
                fixedWidthFieldParser.LoadFieldValue();
                core.OnFieldValueReadReady(activeField, index);
            }
        }

        private void LoadDelimitedHeadingValue()
        {
            if (headingLines)
            {
                delimitedFieldParser.LoadFieldHeading(index);
                core.OnFieldHeadingReadReady(activeField, index);
            }
            else
            {
                delimitedFieldParser.LoadFieldValue();
                core.OnFieldValueReadReady(activeField, index);
            }
        }

        private void EnterField(FtField field)
        {
            activeField = field;
            fieldCount++;
            if (activeField.FixedWidth)
            {
                state = State.InFixedWidthField;
                fixedWidthFieldParser.EnterField(activeField);
            }
            else
            {
                state = State.InDelimitedField;
                delimitedFieldParser.EnterField(activeField);
            }
        }

        private void ExitActiveField()
        {
            switch (state)
            {
                case State.InFixedWidthField:
                    fixedWidthFieldParser.ExitField();
                    break;
                case State.InDelimitedField:
                    delimitedFieldParser.ExitField();
                    break;
                default:
                    throw FtInternalException.Create(InternalError.HeadingLineRecordParser_ExitActiveField_UnsupportedState, state.ToString());
            }
            activeField = null;
        }
    }
}
