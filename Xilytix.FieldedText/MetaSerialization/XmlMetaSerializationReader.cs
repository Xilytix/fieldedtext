// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System.Xml;
using System.Collections.Generic;
using Xilytix.FieldedText.Factory;

namespace Xilytix.FieldedText.MetaSerialization
{
    using Formatting;

    using Stack = Stack<ReadElement>;
    public class XmlMetaSerializationReader: MetaSerializationBase
    {
        internal protected virtual FtMeta Read(XmlReader reader)
        {
            if (!reader.ReadToFollowing(MetaElementTypeFormatter.ToElementName(MetaElementType.FieldedText)))
                return null;
            else
            {
                bool emptyElement = reader.IsEmptyElement;
                FieldedTextReadElement rootElement = ReadElement.CreateFieldedTextXmlReadElement();
                ReadAttributes(reader, rootElement);

                if (!emptyElement)
                {
                    ReadElement activeElement = rootElement;
                    Stack parentStack = new Stack();
                    int foreignElementDepth = 0;

                    while (activeElement != null)
                    {
                        if (!reader.Read())
                        {
                            throw new FtMetaSerializationException(Properties.Resources.XmlMetaSerializationReader_Read_IncompleteXmlStream);
                        }

                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:
                                emptyElement = reader.IsEmptyElement;

                                if (foreignElementDepth > 0)
                                {
                                    if (!emptyElement)
                                    {
                                        foreignElementDepth++;
                                    }
                                }
                                else
                                {
                                    MetaElementType elementType;
                                    if (!MetaElementTypeFormatter.TryParseElementName(reader.Name, out elementType))
                                    {
                                        if (!emptyElement)
                                        {
                                            foreignElementDepth++;
                                        }
                                    }
                                    else
                                    {
                                        ReadElement newActiveElement;
                                        if (activeElement.TryCreate(elementType, out newActiveElement))
                                        {
                                            parentStack.Push(activeElement);
                                            activeElement = newActiveElement;

                                            ReadAttributes(reader, activeElement);

                                            if (emptyElement)
                                            {
                                                if (parentStack.Count > 0)
                                                    activeElement = parentStack.Pop();
                                                else
                                                    activeElement = null;
                                            }
                                        }
                                    }
                                }
                                break;

                            case XmlNodeType.EndElement:
                                if (foreignElementDepth > 0)
                                    foreignElementDepth--;
                                else
                                {
                                    if (parentStack.Count > 0)
                                        activeElement = parentStack.Pop();
                                    else
                                        activeElement = null;
                                }
                                break;
                        }
                    }
                }

                string errorDescription;
                FtMeta result = MetaFactory.CreateMeta();
                if (!rootElement.ResolveTo(result, out errorDescription))
                {
                    throw new FtMetaSerializationException(errorDescription);
                }
                return result;
            }
        }

        private void ReadAttributes(XmlReader reader, ReadElement readElement)
        {
            if (reader.MoveToFirstAttribute())
            {
                TryAddOrIgnoreAttribute(readElement, reader.Name, reader.Value);

                while (reader.MoveToNextAttribute())
                {
                    TryAddOrIgnoreAttribute(readElement, reader.Name, reader.Value);
                }
            }
        }

        private void TryAddOrIgnoreAttribute(ReadElement readElement, string name, string value)
        {
            string errorDescription;
            if (!readElement.TryAddOrIgnoreAttribute(name, value, out errorDescription))
            {
                throw new FtMetaSerializationException(string.Format(Properties.Resources.XmlMetaSerializationReader_Read_AttributeError,
                                                       name, value, errorDescription));
            }
        }
    }
}
