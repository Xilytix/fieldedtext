// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System.IO;

namespace Xilytix.FieldedText
{
    using Serialization;

    public class FtReader : SerializationReader
    {
        public FtReader(FtMeta meta): base()
        {
            InternalLoadMeta(meta);
            Reset(); // Called here as Open() is not called
        }
        public FtReader(string metaFilePath) : this(FtMeta.Create(metaFilePath)) { }

        public FtReader(FtMeta meta, TextReader input, bool immediatelyReadHeader = true) : base()
        {
            InternalLoadMeta(meta);
            Open(input, immediatelyReadHeader);
        }
        public FtReader(FtMeta meta, string inputFilePath, bool immediatelyReadHeader = true) : base()
        {
            InternalLoadMeta(meta);
            Open(inputFilePath, immediatelyReadHeader);
        }

        public FtReader(string metaFilePath, TextReader input, bool immediatelyReadHeader = true) : this(FtMeta.Create(metaFilePath), input, immediatelyReadHeader) { }
        public FtReader(string metaFilePath, string inputFilePath, bool immediatelyReadHeader = true) : this(FtMeta.Create(metaFilePath), inputFilePath, immediatelyReadHeader) { }

        public FtReader() : base() { }
        public FtReader(TextReader input, bool immediatelyReadHeader = true) : base() { Open(input, immediatelyReadHeader); }

        public void Open(TextReader input, bool immediatelyReadHeader = true)
        {
            base.Open(input, false, immediatelyReadHeader);
        }

        public void Open(string inputFilePath, bool immediatelyReadHeader = true)
        {
            StreamReader reader = new StreamReader(inputFilePath);
            Open(reader, true, immediatelyReadHeader);
        }
    }
}
