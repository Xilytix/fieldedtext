// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System.Text;

namespace Xilytix.FieldedText.Serialization
{
    internal struct FixedWidthFieldParser
    {
        private enum State { WaitingLeftEndOfValue, LeftPadding, NoMorePadding, WaitingRightEndOfValue, WaitingRightPadding, RightPadding, RestIsPadding }

        private bool headings; // specifies whether parsing headings (true) or records (false)

        private CharReader charReader;
        private SerializationCore core;
        private StringBuilder textBuilder;

        private State state;
        private FtField field;
        private long position;
        private int fieldLength;
        private int rawOffset;
        private int rawLength;

        // cached field values
        private int fieldWidth;
        private bool fieldLeftPad;
        private FtPadCharType fieldPadCharType;
        private char fieldEndOfValueChar;
        private char fieldPadChar;
        private int rightPaddingCharCount;

        internal FixedWidthFieldParser(SerializationCore myCore, CharReader myCharReader, bool forHeadings)
        {
            headings = forHeadings;
            charReader = myCharReader;
            core = myCore;
            textBuilder = new StringBuilder(20);

            state = State.NoMorePadding;
            field = null;
            position = -1;
            fieldLength = 0;
            rawOffset = -1;
            rawLength = 0;

            fieldWidth = 0;
            fieldLeftPad = false;
            fieldPadCharType = FtPadCharType.Auto;
            fieldEndOfValueChar = '\0';
            fieldPadChar = '\0';
            rightPaddingCharCount = 0;
        }

        internal void EnterField(FtField aField)
        {
            field = aField;

            // cache field values
            fieldWidth = field.Width;

            if (headings)
            {
                fieldLeftPad = field.Definition.HeadingLeftPad;
                fieldPadCharType = field.HeadingPadCharType;
                fieldEndOfValueChar = field.HeadingEndOfValueChar;
                fieldPadChar = field.HeadingPadChar;
            }
            else
            {
                fieldLeftPad = field.Definition.ValueLeftPad;
                fieldPadCharType = field.ValuePadCharType;
                fieldEndOfValueChar = field.ValueEndOfValueChar;
                fieldPadChar = field.ValuePadChar;
            }

            if (fieldLeftPad)
            {
                if (fieldPadCharType == FtPadCharType.EndOfValue)
                    state = State.WaitingLeftEndOfValue;
                else
                    state = State.LeftPadding;
            }
            else
            {
                if (fieldPadCharType == FtPadCharType.EndOfValue)
                    state = State.WaitingRightEndOfValue;
                else
                    state = State.WaitingRightPadding;
            }

            textBuilder.Clear();

            position = charReader.Position;
            fieldLength = 0;
            rawOffset = 0;
            rawLength = 0;
            rightPaddingCharCount = 0;
        }

        internal void ExitField()
        {
            textBuilder.Clear();
            field = null;
        }

        internal void ParseChar(char aChar, out bool previousFieldFinished)
        {
            if (fieldLength >= fieldWidth)
                previousFieldFinished = true;
            else
            {
                previousFieldFinished = false;

                fieldLength++;

                switch (state)
                {
                    case State.WaitingLeftEndOfValue:
                        if (aChar != fieldEndOfValueChar)
                            textBuilder.Append(aChar); // may not encounter any EndOfValue char
                        else
                        {
                            textBuilder.Clear();
                            rawOffset = fieldLength;
                            state = State.NoMorePadding;
                        }
                        break;

                    case State.LeftPadding:
                        if (aChar != fieldPadChar)
                        {
                            textBuilder.Append(aChar);
                            rawOffset = fieldLength - 1;
                            state = State.NoMorePadding;
                        }
                        break;

                    case State.NoMorePadding:
                        textBuilder.Append(aChar);
                        break;

                    case State.WaitingRightEndOfValue:
                        if (aChar != fieldEndOfValueChar)
                            textBuilder.Append(aChar); // may not encounter any EndOfValue char
                        else
                            state = State.RestIsPadding;
                        break;

                    case State.WaitingRightPadding:
                        if (aChar != fieldPadChar)
                            textBuilder.Append(aChar);
                        else
                        {
                            rightPaddingCharCount = 1;
                            state = State.RightPadding;
                        }
                        break;

                    case State.RightPadding:
                        if (aChar == fieldPadChar)
                            rightPaddingCharCount++;
                        else
                        {
                            // Prematurely identified right padding previous start.  Still not started
                            textBuilder.Append(new string(fieldPadChar, rightPaddingCharCount)); // add chars previously thought to be padding
                            textBuilder.Append(aChar);
                            rightPaddingCharCount = 0;
                            state = State.WaitingRightPadding;
                        }
                        break;

                    case State.RestIsPadding:
                        break;

                    default:
                        throw FtInternalException.Create(InternalError.FixedWidthFieldParser_ParseChar_UnsupportedState, state.ToString());
                }
            }
        }

        private string FinishText()
        {
            rawLength = textBuilder.Length;

            if (fieldLength < fieldWidth)
            {
                FtSerializationError error = headings? FtSerializationError.HeadingWidthNotReached : FtSerializationError.ValueWidthNotReached;
                string formatString = headings ? Properties.Resources.FixedWidthFieldParser_FinishText_HeadingWidthNotReached : Properties.Resources.FixedWidthFieldParser_FinishText_ValueWidthNotReached;
                throw new FtSerializationException(error, field, string.Format(formatString, fieldWidth, fieldLength));
            }
            else
            {
                if (fieldLength > fieldWidth)
                {
                    FtSerializationError error = headings ? FtSerializationError.HeadingWidthExceeded : FtSerializationError.ValueWidthExceeded;
                    string formatString = headings ? Properties.Resources.FixedWidthFieldParser_FinishText_HeadingWidthExceeded : Properties.Resources.FixedWidthFieldParser_FinishText_ValueWidthExceeded;
                    throw new FtSerializationException(error, field, string.Format(formatString, fieldWidth, fieldLength));
                }
                else
                    return textBuilder.ToString();
            }
        }

        internal void LoadFieldHeading(int headingLineIndex)
        {
            string headingText = FinishText();
            field.LoadPosition(position, fieldLength, rawOffset, rawLength);
            field.LoadHeading(headingLineIndex, headingText);
        }

        internal void LoadFieldValue()
        {
            string valueText = FinishText();
            field.LoadPosition(position, fieldLength, rawOffset, rawLength);
            field.LoadFixedWidthValue(valueText);
        }
    }
}
