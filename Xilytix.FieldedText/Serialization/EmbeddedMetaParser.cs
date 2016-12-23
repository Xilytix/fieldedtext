// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System.Text;

namespace Xilytix.FieldedText.Serialization
{
    internal class EmbeddedMetaParser
    {
        private const string FieldedTextElementName = MetaSerialization.Formatting.MetaElementTypeFormatter.FieldedTextElementName;

        private const string XmlStartingText = "<?xml ";
        private const string FieldedTextStartingText = "<" + FieldedTextElementName;
        private const string TerminatingText = "</" + FieldedTextElementName + ">";

        private const int InvalidBuilderIndex = -1;

        private StringBuilder builder;

        private bool detected;
        private bool nextCharIsComment;
        private bool ready;

        private int xmlStartingPosition;
        private int fieldedTextStartingPosition;
        private int terminatingPosition;

        private int fieldedTextStartingBuilderIndex;
        private int xmlStartingBuilderIndex;

        internal EmbeddedMetaParser() { builder = new StringBuilder(); }

        internal bool Ready { get { return ready; } }

        internal void Reset()
        {
            detected = false;
            nextCharIsComment = false;
            ready = false;

            xmlStartingPosition = 0;
            fieldedTextStartingPosition = 0;
            terminatingPosition = 0;

            xmlStartingBuilderIndex = InvalidBuilderIndex;
            fieldedTextStartingBuilderIndex = InvalidBuilderIndex;

            builder.Capacity = 500;
        }

        internal void ParseNotYetDetectedChar(char aChar, out bool detected)
        {
            if (nextCharIsComment) // ignore start of line comment characters
                nextCharIsComment = false;
            else
            {
                builder.Append(aChar);

                // If XmlStartingText not yet match, see if char is part of match
                if (xmlStartingPosition < XmlStartingText.Length)
                {
                    if (aChar == XmlStartingText[xmlStartingPosition])
                    {
                        if (xmlStartingPosition == 0)
                        {
                            xmlStartingBuilderIndex = builder.Length - 1;
                        }
                        xmlStartingPosition++;
                    }
                }

                // If FieldedTextStartingText not yet match, see if char is part of match
                if (aChar == FieldedTextStartingText[fieldedTextStartingPosition])
                {
                    if (fieldedTextStartingPosition == 0)
                    {
                        fieldedTextStartingBuilderIndex = builder.Length;
                    }

                    fieldedTextStartingPosition++;

                    if (fieldedTextStartingPosition >= FieldedTextStartingText.Length)
                    {
                        int builderIndex = (xmlStartingPosition >= XmlStartingText.Length) ? xmlStartingBuilderIndex : fieldedTextStartingBuilderIndex;
                        builder.Remove(0, builderIndex);

                        this.detected = true;
                    }
                }
            }

            detected = this.detected;
        }

        internal void StartLine()
        {
            // matches cannot carry over line except for fully matched XmlStartingText
            if (!detected)
            {
                fieldedTextStartingPosition = 0;

                if (xmlStartingPosition >= XmlStartingText.Length)
                    AppendLine();
                else
                {
                    xmlStartingPosition = 0;
                    builder.Clear();
                }
            }

            nextCharIsComment = true;
            terminatingPosition = 0;
        }

        internal void ParseChar(char aChar)
        {
            if (!ready)
            {
                if (nextCharIsComment)
                    nextCharIsComment = false;
                else
                {
                    builder.Append(aChar);

                    if (aChar != TerminatingText[terminatingPosition])
                        terminatingPosition = 0;
                    else
                    {
                        terminatingPosition++;
                        if (terminatingPosition >= TerminatingText.Length)
                        {
                            ready = true;
                        }
                    }
                }
            }
        }

        internal void AppendLine()
        {
            builder.AppendLine();
        }

        internal string TakeMetaAsString()
        {
            string result = builder.ToString();
            builder.Clear();
            builder.Capacity = 0;
            return result;
        } 
    }
}
