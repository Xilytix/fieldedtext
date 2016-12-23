// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

namespace Xilytix.FieldedText
{
    using System.Text;
    using System.IO;
    using System.Xml;

    using MetaSerialization;

    public static class FtMetaSerializer
    {
        public static FtMeta Deserialize(XmlReader reader)
        {
            XmlMetaSerializationReader serializationReader = new XmlMetaSerializationReader();
            return serializationReader.Read(reader);
        }
        public static FtMeta Deserialize(string metaFilePath)
        {
            using (StreamReader streamReader = new StreamReader(metaFilePath))
            {
                using (XmlReader xmlReader = XmlReader.Create(streamReader))
                {
                    XmlMetaSerializationReader serializationReader = new XmlMetaSerializationReader();
                    return serializationReader.Read(xmlReader);
                }
            }
        }
        public static void Serialize(FtMeta meta, XmlWriter xmlWriter)
        {
            XmlMetaSerializationWriter serializationWriter = new XmlMetaSerializationWriter();
            serializationWriter.Write(xmlWriter, meta);
        }
        public static void Serialize(FtMeta meta, string outputFilePath)
        {
            using (StreamWriter streamWriter = new StreamWriter(outputFilePath))
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(streamWriter))
                {
                    XmlMetaSerializationWriter serializationWriter = new XmlMetaSerializationWriter();
                    serializationWriter.Write(xmlWriter, meta);
                }
            }
        }
        public static void Serialize(FtMeta meta, string outputFilePath, XmlWriterSettings writerSettings)
        {
            using (XmlWriter xmlWriter = XmlWriter.Create(outputFilePath, writerSettings))
            {
                XmlMetaSerializationWriter serializationWriter = new XmlMetaSerializationWriter();
                serializationWriter.Write(xmlWriter, meta);
            }
        }
    }
}
