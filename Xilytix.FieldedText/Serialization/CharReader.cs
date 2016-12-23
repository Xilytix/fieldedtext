// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

namespace Xilytix.FieldedText.Serialization
{
    using System;
    using System.Diagnostics;
    using System.Text;
    using System.IO;

    internal class CharReader : IDisposable
    {
        internal const string Signature = "|!Fielded Text^|";
        internal const char CarriageReturnChar = '\x0D';
        internal const char LineFeedChar = '\x0A';
        internal const string CarriageReturnLineFeedString = "\x0D\x0A";
        internal const int EofReadResult = -1;

        private const int textReaderEofReadResult = -1;

        private enum State
        {
            Read,
            SignaturePeekReread,
            SignaturePeekRereadThenEof,
            TextReaderPeeked,
            EofPeeked,
            Eof
        }

        private bool disposedValue = false; // To detect redundant calls

        private TextReader textReader;
        private bool textReaderOwned;
        private long position;
        private State state;
        private StringBuilder signaturePeekBuffer;
        private int signaturePeekBufferPosition;
        private int peekedChar;

        internal CharReader() { signaturePeekBuffer = new StringBuilder(); }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (textReader != null)
                    {
                        if (textReaderOwned)
                        {
                            textReader.Dispose();
                            textReaderOwned = false;
                        }
                        textReader = null;
                    }
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        internal void Close()
        {
            if (textReader != null)
            {
                if (textReaderOwned)
                {
                    textReader.Close();
                }
                textReader = null;
            }
        }

        internal bool IsClosed { get { return textReader == null; } }

        internal long Position { get { return position; } }

        internal TextReader TextReader { get { return textReader; } }
        internal void SetTextReader(TextReader textReader, bool owned)
        {
            Close();

            this.textReader = textReader;
            textReaderOwned = owned;
            state = State.Read;
            signaturePeekBuffer.Clear();
            signaturePeekBufferPosition = 0;
            position = -1;
        }

        internal int Read()
        {
            switch (state)
            {
                case State.Read:
                    int readResult = textReader.Read();
                    position++;
                    if (readResult != textReaderEofReadResult)
                        return readResult;
                    else
                    {
                        state = State.Eof;
                        return EofReadResult;
                    }

                case State.SignaturePeekReread:
                case State.SignaturePeekRereadThenEof:
                    char aChar = signaturePeekBuffer[signaturePeekBufferPosition];
                    signaturePeekBufferPosition++;
                    if (signaturePeekBufferPosition == signaturePeekBuffer.Length)
                    {
                        state = (state == State.SignaturePeekRereadThenEof) ? State.EofPeeked : State.Read;
                    }
                    position++;
                    return (int)aChar;

                case State.TextReaderPeeked:
                    int peekResult = (int)peekedChar;
                    state = State.Read;
                    position++;
                    return peekResult;

                case State.EofPeeked:
                    position++;
                    state = State.Eof;
                    return EofReadResult;

                case State.Eof:
                    return EofReadResult;

                default:
                    throw FtInternalException.Create(InternalError.CharReader_Read_UnsupportedState, state.ToString());
            }
        }

        internal string PeekSignature()
        {
            Debug.Assert(state == State.Read, "CharReader.PeekSigned not in ReadState");

            string result = null;

            string activeSignature = Signature;
            signaturePeekBuffer.Clear();

            do
            {
                int readCharAsInt = textReader.Read();

                if (readCharAsInt == textReaderEofReadResult)
                    state = (signaturePeekBuffer.Length == 0) ? State.EofPeeked : State.SignaturePeekRereadThenEof;
                else
                {
                    char readChar = (char)readCharAsInt;
                    signaturePeekBuffer.Append(readChar);

                    if (signaturePeekBuffer.Length > 1)
                    {
                        if (signaturePeekBuffer.Length == 3 && readChar == Legacy.Signature[1])
                        {
                            activeSignature = Legacy.Signature;
                        }

                        if (readChar != activeSignature[signaturePeekBuffer.Length - 2])
                            state = State.SignaturePeekReread;
                        else
                        {
                            if (signaturePeekBuffer.Length == activeSignature.Length)
                            {
                                result = activeSignature;
                                state = State.SignaturePeekReread;
                            }
                        }
                    }
                }
            } while (state == State.Read);

            signaturePeekBufferPosition = 0;
            return result;
        }

        internal bool PeekNextIsLineFeed()
        {
            return Peek() == LineFeedChar;
        }

        internal int Peek()
        {
            switch (state)
            {
                case State.Read:
                    int readChar = textReader.Read();
                    if (readChar == textReaderEofReadResult)
                    {
                        state = State.EofPeeked;
                        return EofReadResult;
                    }
                    else
                    {
                        peekedChar = readChar;
                        state = State.TextReaderPeeked;
                        return peekedChar;
                    }

                case State.SignaturePeekReread:
                case State.SignaturePeekRereadThenEof:
                    return signaturePeekBuffer[signaturePeekBufferPosition];

                case State.TextReaderPeeked: 
                    return peekedChar;

                case State.EofPeeked:
                case State.Eof:
                    return EofReadResult;

                default:
                    throw FtInternalException.Create(InternalError.CharReader_Peek_UnsupportedState, state.ToString());
            }
        }
    }
}
