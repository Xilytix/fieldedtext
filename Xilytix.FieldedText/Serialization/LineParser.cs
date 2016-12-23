// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System.Diagnostics;
using System.Text;

namespace Xilytix.FieldedText.Serialization
{
    internal struct LineParser
    {
        private enum FtLineState { Out, In, InNextTextOut, InTextOut, InPendingNextLineFeed };

        public enum LineEndedType { Not, Initiated, Continued }

        private const char CarriageReturnChar = CharReader.CarriageReturnChar;
        private const char LineFeedChar = CharReader.LineFeedChar;
        private const string CarriageReturnLineFeedString = CharReader.CarriageReturnLineFeedString;

        SerializationCore core;
        CharReader charReader;

        private int textLineCount;
        private int textLineLength;
        private int lineCount;
        private int lineLength;
        private FtLineState inLineState;

        internal LineParser(SerializationCore myCore, CharReader myCharReader)
        {
            core = myCore;
            charReader = myCharReader;

            // Reset
            textLineCount = 0;
            textLineLength = 0;
            lineCount = 0;
            lineLength = 0;
            inLineState = FtLineState.Out;
        }

        internal int LineCount { get { return lineCount; } }
        internal int LineLength { get { return lineLength; } }
        internal bool InLine { get { return inLineState != FtLineState.Out; } }

        internal void Reset()
        {
            textLineCount = 0;
            textLineLength = 0;
            lineCount = 0;
            lineLength = 0;
            inLineState = FtLineState.Out;
        }

        internal void ParseChar(char aChar, bool endOfLineToBeEmbedded, out LineEndedType lineEndedType)
        {
            switch (inLineState)
            {
                case FtLineState.InNextTextOut:
                    Debug.Assert(aChar == LineFeedChar && endOfLineToBeEmbedded, ((int)(InternalError.LineParser_ParseChar_InNextTextOut)).ToString());
                    inLineState = FtLineState.InTextOut;
                    lineEndedType = LineEndedType.Not;
                    break;

                case FtLineState.InTextOut:
                    textLineCount++;
                    textLineLength = 0;
                    inLineState = FtLineState.In;
                    ParseInChar(aChar, endOfLineToBeEmbedded, out lineEndedType);
                    break;

                case FtLineState.In:
                    ParseInChar(aChar, endOfLineToBeEmbedded, out lineEndedType);
                    break;

                case FtLineState.InPendingNextLineFeed:
                    Debug.Assert(aChar == LineFeedChar && !endOfLineToBeEmbedded, ((int)(InternalError.LineParser_ParseChar_InPendingNextLineFeed)).ToString());
                    inLineState = FtLineState.Out;
                    lineEndedType = LineEndedType.Continued;
                    break;

                case FtLineState.Out:
                    lineCount++;
                    lineLength = 0;
                    textLineCount++;
                    textLineLength = 0;
                    inLineState = FtLineState.In;
                    ParseInChar(aChar, endOfLineToBeEmbedded, out lineEndedType);
                    break;

                default:
                    throw FtInternalException.Create(InternalError.LineParser_ParseChar_Unsupported_InLineState, inLineState.ToString());
            }

            lineLength++;
            textLineLength++;
        }

        private void ParseInChar(char aChar, bool embedEndOfLine, out LineEndedType lineEndedType)
        {
            switch (core.EndOfLineType)
            {
                case FtEndOfLineType.Auto:
                    switch (aChar)
                    {
                        case CarriageReturnChar:
                            if (embedEndOfLine)
                            {
                                if (charReader.Peek() == LineFeedChar)
                                    inLineState = FtLineState.InNextTextOut;
                                else
                                    inLineState = FtLineState.InTextOut;
                                lineEndedType = LineEndedType.Not;
                            }
                            else
                            {
                                inLineState = (charReader.Peek() == LineFeedChar) ? FtLineState.InPendingNextLineFeed : FtLineState.Out;
                                lineEndedType = LineEndedType.Initiated;
                            }
                            break;

                        case LineFeedChar:
                            if (embedEndOfLine)
                            {
                                inLineState = FtLineState.InTextOut;
                                lineEndedType = LineEndedType.Not;
                            }
                            else
                            {
                                inLineState = FtLineState.Out;
                                lineEndedType = LineEndedType.Initiated;
                            }
                            break;

                        default:
                            lineLength++;
                            textLineLength++;
                            lineEndedType = LineEndedType.Not;
                            break;
                    }
                    break;

                case FtEndOfLineType.CrLf:
                    if (aChar != CarriageReturnChar)
                        lineEndedType = LineEndedType.Not;
                    else
                    {
                        if (embedEndOfLine)
                        {
                            if (charReader.Peek() == LineFeedChar)
                                inLineState = FtLineState.InNextTextOut;
                            else
                                inLineState = FtLineState.InTextOut;
                            lineEndedType = LineEndedType.Not;
                        }
                        else
                        {
                            if (charReader.Peek() != LineFeedChar)
                                lineEndedType = LineEndedType.Not;
                            else
                            {
                                inLineState = FtLineState.InPendingNextLineFeed;
                                lineEndedType = LineEndedType.Initiated;
                            }
                        }
                    }
                    break;

                case FtEndOfLineType.Char:
                    if (embedEndOfLine)
                    {
                        inLineState = FtLineState.InTextOut;
                        lineEndedType = LineEndedType.Not;
                    }
                    else
                    {
                        inLineState = FtLineState.Out;
                        lineEndedType = LineEndedType.Initiated;
                    }
                    break;

                default:
                    throw FtInternalException.Create(InternalError.LineParser_ParseInChar_UnsupportedEndOfLineType, core.EndOfLineType.ToString());

            }
        }

        internal void ExitLine()
        {
            Debug.Assert(InLine, "LineParser.ExitLine() called when not In Line");
            inLineState = FtLineState.Out;
        }

        internal void AddBlankLine()
        {
            textLineCount++;
            textLineLength = 0;
            lineCount++;
            lineLength = 0;
        }
    }
}
