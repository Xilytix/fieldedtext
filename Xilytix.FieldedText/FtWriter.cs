// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

namespace Xilytix.FieldedText
{
    using System.IO;

    using Serialization;

    public class FtWriter : SerializationWriter
    {
        public FtWriter(FtMeta meta): base(meta) { }

        public FtWriter(FtMeta meta, TextWriter output) : this(meta, output, new FtWriterSettings()) { }
        public FtWriter(FtMeta meta, TextWriter output, FtWriterSettings settings) : base(meta) { Open(output, settings); }
        public FtWriter(FtMeta meta, string outputFilePath) : this(meta, outputFilePath, new FtWriterSettings()) { }
        public FtWriter(FtMeta meta, string outputFilePath, FtWriterSettings settings) : base(meta) { Open(outputFilePath, settings); }

        public FtWriter(string metaFilePath) : this(FtMeta.Create(metaFilePath)) { }

        public FtWriter(string metaFilePath, TextWriter output) : this(FtMeta.Create(metaFilePath), output) { }
        public FtWriter(string metaFilePath, TextWriter output, FtWriterSettings settings) : this(FtMeta.Create(metaFilePath), output, settings) { }
        public FtWriter(string metaFilePath, string outputFilePath) : this(FtMeta.Create(metaFilePath), outputFilePath) { }
        public FtWriter(string metaFilePath, string outputFilePath, FtWriterSettings settings) : this(FtMeta.Create(metaFilePath), outputFilePath, settings) { }

        public void Open(TextWriter output)
        {
            Open(output, new FtWriterSettings());
        }
        public void Open(TextWriter output, FtWriterSettings settings)
        {
            Open(output, false, settings);
        }
        public void Open(string outputFilePath)
        {
            Open(outputFilePath, false, new FtWriterSettings());
        }
        public void Open(string outputFilePath, FtWriterSettings settings)
        {
            Open(outputFilePath, false, settings);
        }
        public void Open(string outputFilePath, bool append)
        {
            Open(outputFilePath, append, new FtWriterSettings());
        }

        public void Open(string outputFilePath, bool append, FtWriterSettings settings)
        {
            StreamWriter writer = new StreamWriter(outputFilePath, append);
            Open(writer, true, settings);
        }
    }
}
