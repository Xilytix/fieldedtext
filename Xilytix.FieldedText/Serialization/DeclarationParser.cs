// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

namespace Xilytix.FieldedText.Serialization
{
    using System.Text;

    internal class DeclarationParser
    {
        private enum State { CommentChar, Signature, WaitingParameter, ParameterName, WaitingValue, Value, ValueStuffedQuote }
        private const char NameValueSeparatorChar = '=';
        private const char ValueQuoteChar = '"';

        private CharReader charReader;
        private DeclaredParameters parameters;

        private StringBuilder nameBuilder;
        private StringBuilder valueBuilder;

        private State state;
        private int linePosition;

        internal DeclarationParser(CharReader myCharReader, DeclaredParameters myParameters)
        {
            charReader = myCharReader;
            parameters = myParameters;

            nameBuilder = new StringBuilder();
            valueBuilder = new StringBuilder();
        }

        internal string Signature { get; set; }

        internal void StartLine()
        {
            linePosition = -1;
            state = State.CommentChar;
        }

        internal void ParseSignatureLineChar(char aChar)
        {
            linePosition++;

            switch (state)
            {
                case State.CommentChar:
                    // ignore character
                    state = State.Signature;
                    break;
                case State.Signature:
                    // ignore characters
                    if (linePosition >= Signature.Length)
                    {
                        state = State.WaitingParameter;
                    }
                    break;
                default:
                    ParseChar(aChar);
                    break;
            }
        }

        internal void ParseDeclaration2LineChar(char aChar)
        {
            linePosition++;

            if (state == State.CommentChar)
                // ignore character
                state = State.WaitingParameter;
            else
                ParseChar(aChar);
        }

        private void ParseChar(char aChar)
        {
            switch (state)
            {
                case State.WaitingParameter:
                    // ignore whitespace
                    if (!char.IsWhiteSpace(aChar))
                    {
                        if (aChar == NameValueSeparatorChar)
                            throw new FtSerializationException(FtSerializationError.DeclarationParameterNameIsZeroLength,
                                                               string.Format(Properties.Resources.DeclarationParser_ParseChar_DeclarationParameterNameIsZeroLength, parameters.Count));
                        else
                        {
                            nameBuilder.Clear();
                            nameBuilder.Append(aChar);
                            state = State.ParameterName;
                        }
                    }
                    break;

                case State.ParameterName:
                    if (aChar != NameValueSeparatorChar)
                        nameBuilder.Append(aChar);
                    else
                        state = State.WaitingValue;
                    break;

                case State.WaitingValue:
                    // ignore initial whitespace
                    if (!char.IsWhiteSpace(aChar))
                    {
                        if (aChar != ValueQuoteChar)
                            throw new FtSerializationException(FtSerializationError.DeclarationParameterValueNotQuoted,
                                                               string.Format(Properties.Resources.DeclarationParser_ParseChar_DeclarationParameterValueNotQuoted, nameBuilder.ToString()));
                        else
                        {
                            valueBuilder.Clear();
                            state = State.Value;
                        }
                    }
                    break;

                case State.Value:
                    if (aChar != ValueQuoteChar)
                        valueBuilder.Append(aChar);
                    else
                    {
                        if (charReader.Peek() == ValueQuoteChar)
                            state = State.ValueStuffedQuote;
                        else
                        {
                            parameters.Add(nameBuilder.ToString().Trim(), valueBuilder.ToString());
                            state = State.WaitingParameter;
                        }
                    }
                    break;

                case State.ValueStuffedQuote:
                    valueBuilder.Append(ValueQuoteChar);
                    state = State.Value;
                    break;

                default:
                    throw FtInternalException.Create(InternalError.DeclarationParser_ParseChar_UnsupportedState, state.ToString());
            }
        }

        internal void FinishSignatureLine()
        {
            FinishLine();
        }

        internal void Finish()
        {
            FinishLine();
            int versionIdx = parameters.IndexOfVersion();
            if (versionIdx < 0)
                throw new FtSerializationException(FtSerializationError.DeclarationParametersMissingVersion,
                                                   Properties.Resources.DeclarationParser_Finish_DeclaredParametersMissingVersion);
            else
            {
                if (versionIdx != 0)
                    throw new FtSerializationException(FtSerializationError.DeclarationParameterVersionIsNotFirst,
                                                       Properties.Resources.DeclarationParser_Finish_DeclaredParametersVersionIsNotFirst);
                else
                {
                    int major;
                    int minor;
                    string comment;
                    if (!parameters.TryGetVersion(out major, out minor, out comment))
                    {
                        throw new FtSerializationException(FtSerializationError.DeclarationParameterInvalidVersion,
                                                           string.Format(Properties.Resources.DeclarationParser_Finish_DeclaredParametersInvalidVersionValue, parameters.GetValue(versionIdx)));
                    }
                }
            }
        }

        private void FinishLine()
        {
            switch (state)
            {
                case State.CommentChar: // should never happen as detected in reader
                case State.Signature: // should never happen as cached in charReader
                case State.ValueStuffedQuote: // should never happen as peeked in charReader
                    throw FtInternalException.Create(InternalError.DeclarationParser_FinishLine_UnexpectedState, state.ToString());
                case State.ParameterName:
                    throw new FtSerializationException(FtSerializationError.DeclarationParameterNameNotTerminated,
                                                       string.Format(Properties.Resources.DeclarationParser_FinishLine_DeclarationParameterNameNotTerminated, nameBuilder.ToString()));
                case State.WaitingValue:
                    throw new FtSerializationException(FtSerializationError.DeclarationParameterMissingValue,
                                                       string.Format(Properties.Resources.DeclarationParser_FinishLine_DeclarationParameterMissingValue, nameBuilder.ToString()));
                case State.Value:
                    throw new FtSerializationException(FtSerializationError.DeclarationParameterValueNotTerminated,
                                                       string.Format(Properties.Resources.DeclarationParser_FinishLine_DeclarationParameterValueNotTerminated, nameBuilder.ToString()));
                case State.WaitingParameter:
                    break; // all good
                default:
                    throw FtInternalException.Create(InternalError.DeclarationParser_FinishLine_UnsupportedState, state.ToString());
            }
        }
    }
}
