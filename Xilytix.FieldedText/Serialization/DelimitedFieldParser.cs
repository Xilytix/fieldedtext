// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System.Text;

namespace Xilytix.FieldedText.Serialization
{
    internal struct DelimitedFieldParser
    {
        private enum QuotedState { NeverOpen, CanOpen, MustOpen, Opened, Open, Stuffed, Closed }

        private bool headings; // specifies whether parsing headings (true) or records (false)

        private CharReader charReader;
        private SerializationCore core;
        private StringBuilder textBuilder;

        private FtField field;
        private FtQuotedType fieldQuotedType;
        private bool fieldSubstitutionsEnabled;
        private char fieldSubstitutionChar;
        private char fieldDelimiterChar;
        private char fieldQuoteChar;

        private QuotedState quotedState;
        private bool substitutionActive;
        private long position;
        private int rawOffset;
        private int rawLength;

        internal DelimitedFieldParser(SerializationCore myCore, CharReader myCharReader, bool forHeadings)
        {
            headings = forHeadings;
            charReader = myCharReader;
            core = myCore;
            textBuilder = new StringBuilder(20);

            field = null;
            fieldQuotedType = FtQuotedType.Optional;
            fieldSubstitutionsEnabled = false;
            fieldSubstitutionChar = '\0';
            fieldDelimiterChar = '\0';
            fieldQuoteChar = '\0';

            quotedState = QuotedState.CanOpen;
            substitutionActive = false;
            position = -1;
            rawOffset = -1;
            rawLength = 0;
        }

        internal bool IsEndOfLineToBeEmbedded()
        {
            switch (quotedState)
            {
                case QuotedState.Opened:
                case QuotedState.Open:
                case QuotedState.Stuffed:
                    return core.AllowEndOfLineCharInQuotes;
                default:
                    return false;
            }
        }

        internal void EnterField(FtField aField)
        {
            field = aField;

            // cache field values
            fieldSubstitutionsEnabled = core.SubstitutionsEnabled;
            fieldSubstitutionChar = core.SubstitutionChar;
            fieldDelimiterChar = core.DelimiterChar;
            fieldQuoteChar = core.QuoteChar;

            if (headings)
            {
                fieldQuotedType = field.HeadingQuotedType;
            }
            else
            {
                fieldQuotedType = field.ValueQuotedType;

            }

            switch (fieldQuotedType)
            {
                case FtQuotedType.Never:
                    quotedState = QuotedState.NeverOpen;
                    break;
                case FtQuotedType.Optional:
                    quotedState = QuotedState.CanOpen;
                    break;
                case FtQuotedType.Always:
                    quotedState = QuotedState.MustOpen;
                    break;
                default:
                    throw FtInternalException.Create(InternalError.DelimitedFieldParser_EnterField_UnsupportedQuotedType, fieldQuotedType.ToString());
            }


            textBuilder.Clear();
            position = charReader.Position;
            rawOffset = -1;
            rawLength = 0;
        }

        internal void ExitField()
        {
            textBuilder.Clear();
            field = null;
        }

        internal void ParseChar(char aChar, out bool finished)
        {
            if (substitutionActive)
            {
                AppendSubstitution(aChar);
                substitutionActive = false;
                rawLength++;
                finished = false;
            }
            else
            {
                if (fieldSubstitutionsEnabled && aChar == fieldSubstitutionChar)
                {
                    substitutionActive = true;
                    rawLength++;
                    finished = false;
                }
                else
                {
                    switch (quotedState)
                    {
                        case QuotedState.NeverOpen:
                            if (aChar == fieldDelimiterChar)
                                finished = true;
                            else
                            {
                                finished = false;
                                AppendValueChar(aChar);
                            }
                            break;

                        case QuotedState.CanOpen:
                            if (aChar == fieldQuoteChar)
                            {
                                finished = false;
                                quotedState = QuotedState.Opened;
                            }
                            else
                            {
                                if (char.IsWhiteSpace(aChar))
                                {
                                    finished = false;
                                    rawLength++;
                                    textBuilder.Append(aChar);
                                }
                                else
                                {
                                    quotedState = QuotedState.NeverOpen;
                                    ParseChar(aChar, out finished);
                                }
                            }
                            break;

                        case QuotedState.MustOpen:
                            if (char.IsWhiteSpace(aChar))
                                finished = false; // ignore white space before quote is opened
                            else
                            {
                                if (aChar == fieldDelimiterChar)
                                    finished = true; // null
                                else
                                {
                                    if (aChar == fieldQuoteChar)
                                    {
                                        finished = false;
                                        quotedState = QuotedState.Opened;
                                    }
                                    else
                                    {
                                        FtSerializationError error = headings ? FtSerializationError.HeadingNonWhiteSpaceCharBeforeQuotesOpened : FtSerializationError.ValueNonWhiteSpaceCharBeforeQuotesOpened;
                                        string message = headings ? Properties.Resources.DelimitedFieldParser_ParseChar_HeadingNonWhiteSpaceCharBeforeQuotesOpened : Properties.Resources.DelimitedFieldParser_ParseChar_ValueNonWhiteSpaceCharBeforeQuotesOpened;
                                        throw new FtSerializationException(error, field, message);
                                    }
                                }
                            }
                            break;

                        case QuotedState.Opened:
                            rawOffset = (int)(charReader.Position - position);
                            rawLength = 0;
                            quotedState = QuotedState.Open;
                            ParseChar(aChar, out finished);
                            break;

                        case QuotedState.Open:
                            finished = false;
                            if (aChar != fieldQuoteChar)
                                AppendValueChar(aChar);
                            else
                            {
                                if (charReader.Peek() == fieldQuoteChar)
                                {
                                    rawLength++;
                                    quotedState = QuotedState.Stuffed;
                                }
                                else
                                {
                                    rawLength = (int)(charReader.Position - position) + rawOffset;
                                    quotedState = QuotedState.Closed;
                                }
                            }
                            break;

                        case QuotedState.Stuffed:
                            AppendValueChar(fieldQuoteChar);
                            quotedState = QuotedState.Open;
                            finished = false;
                            break;

                        case QuotedState.Closed:
                            if (aChar == fieldDelimiterChar)
                                finished = true;
                            else
                            {
                                if (char.IsWhiteSpace(aChar))
                                    finished = false;
                                else
                                {
                                    FtSerializationError error = headings ? FtSerializationError.HeadingNonWhiteSpaceCharAfterQuotesClosed : FtSerializationError.ValueNonWhiteSpaceCharAfterQuotesClosed; 
                                    string message = headings ? Properties.Resources.DelimitedFieldParser_ParseChar_HeadingNonWhiteSpaceCharAfterQuotesClosed : Properties.Resources.DelimitedFieldParser_ParseChar_ValueNonWhiteSpaceCharAfterQuotesClosed;
                                    throw new FtSerializationException(error, field, message);
                                }
                            }

                            break;

                        default:
                            throw FtInternalException.Create(InternalError.DelimitedFieldParser_ParseChar_UnsupportedQuotedState, quotedState.ToString());
                    }
                }
            }
        }

        private void AppendValueChar(char aChar)
        {
            rawLength++;
            if (fieldSubstitutionsEnabled && aChar == fieldSubstitutionChar)
                substitutionActive = true;
            else
                textBuilder.Append(aChar);
        }

        private void AppendSubstitution(char tokenChar)
        {
            string value;
            if (core.SubstitutionList.TryGetValue(tokenChar, out value))
                textBuilder.Append(value);
            else
                textBuilder.Append(tokenChar);
        }

        private string FinishText()
        {
            string text;

            switch (quotedState)
            {
                case QuotedState.MustOpen:
                    text = ""; // is null
                    break;

                case QuotedState.Opened:
                    if (fieldQuotedType == FtQuotedType.Always)
                    {
                        FtSerializationError error = headings ? FtSerializationError.HeadingQuotedFieldMissingEndQuoteChar : FtSerializationError.ValueQuotedFieldMissingEndQuoteChar;
                        throw new FtSerializationException(error, field, "");
                    }
                    else
                    {
                        text = fieldQuoteChar.ToString();
                        rawOffset--;
                        rawLength++;
                    }
                    break;

                case QuotedState.Open:
                    if (fieldQuotedType == FtQuotedType.Always)
                    {
                        FtSerializationError error = headings ? FtSerializationError.HeadingQuotedFieldMissingEndQuoteChar : FtSerializationError.ValueQuotedFieldMissingEndQuoteChar;
                        throw new FtSerializationException(error, field, "");
                    }
                    else
                    {
                        text = fieldQuoteChar.ToString() + textBuilder.ToString();
                        rawOffset--;
                        rawLength++;
                    }
                    break;

                default:
                    text = textBuilder.ToString();
                    break;
            }

            return text;
        }

        internal void LoadFieldHeading(int headingLineIndex)
        {
            string headingText = FinishText(); ;
            field.LoadPosition(position, (int)(charReader.Position - position), rawOffset, rawLength);
            field.LoadHeading(headingLineIndex, headingText);
        }

        internal void LoadFieldValue()
        {
            string valueText = FinishText(); ;
            field.LoadPosition(position, (int)(charReader.Position - position), rawOffset, rawLength);
            field.LoadDelimitedValue(valueText, quotedState == QuotedState.Closed);
        }
    }
}
