// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

namespace Xilytix.FieldedText
{
    using System;
    using System.IO;

    using Serialization;

    public class FtSerializer
    {
        private FtMeta meta;
        private SerializationCore core;

        public const int VersionMajor = SerializationCore.VersionMajor;
        public const int VersionMinor = SerializationCore.VersionMinor;
        public const char PrefixSpaceChar = SerializationCore.PrefixSpaceChar;

        public FtSerializer(FtMeta myMeta) { meta = myMeta; }
        public FtSerializer(string metaFilePath) { meta = FtMeta.Create(metaFilePath); }

        public event EventHandler<FtFieldHeadingReadyEventArgs> FieldHeadingReadReady;
        public event EventHandler<FtFieldHeadingReadyEventArgs> FieldHeadingWriteReady;
        public event EventHandler<FtFieldValueReadyEventArgs> FieldValueReadReady;
        public event EventHandler<FtFieldValueReadyEventArgs> FieldValueWriteReady;
        public event EventHandler<FtHeadingLineStartedEventArgs> HeadingLineStarted;
        public event EventHandler<FtHeadingLineFinishedEventArgs> HeadingLineFinished;
        public event EventHandler<FtRecordStartedEventArgs> RecordStarted;
        public event EventHandler<FtRecordFinishedEventArgs> RecordFinished;
        public event EventHandler<FtSequenceRedirectedEventArgs> SequenceRedirected;

        public void Deserialize(FtReader reader)
        {
            SetCore(reader);

            while (reader.Read()) ;
        }

        public void Deserialize(TextReader input)
        {
            Deserialize(new FtReader(meta, input));
        }
        public void Deserialize(string inputFilePath)
        {
            Deserialize(new FtReader(meta, inputFilePath));
        }

        public void Serialize(FtWriter output)
        {
            SetCore(output);

            while (true)
            {
                output.Write();
                // only way out is to throw FtAbortSerializationException in event handler
            }
        }

        public void Serialize(TextWriter output)
        {
            Serialize(new FtWriter(meta, output));
        }
        public void Serialize(TextWriter output, FtWriterSettings settings)
        {
            Serialize(new FtWriter(meta, output, settings));
        }
        public void Serialize(string outputFilePath)
        {
            Serialize(new FtWriter(meta, outputFilePath));
        }
        public void Serialize(string outputFilePath, FtWriterSettings settings)
        {
            Serialize(new FtWriter(meta, outputFilePath, settings));
        }

        private void SetCore(SerializationCore value)
        {
            core = value;
            core.FieldHeadingReadReady += HandleFieldHeadingReadReady;
            core.FieldHeadingWriteReady += HandleFieldHeadingWriteReady;
            core.FieldValueReadReady += HandleFieldValueReadReady;
            core.FieldValueWriteReady += HandleFieldValueWriteReady;
            core.HeadingLineStarted += HandleHeadingLineStarted;
            core.HeadingLineFinished += HandleHeadingLineFinished;
            core.RecordStarted += HandleRecordStarted;
            core.RecordFinished += HandleRecordFinished;
            core.SequenceRedirected += HandleSequenceRedirected;
        }

        public FtFieldDefinitionList FieldDefinitionList { get { return core.FieldDefinitionList; } }
        public FtFieldList FieldList { get { return core.FieldList; } }
        public FtSubstitutionList SubstitutionList { get { return core.SubstitutionList; } }
        public FtSequenceList SequenceList { get { return core.SequenceList; } }
        public FtSequenceInvokationList SequenceInvokationList { get { return core.SequenceInvokationList; } }
        public FtSequence RootSequence { get { return core.RootSequence; } }
        public int RootFieldCount { get { return core.RootFieldCount; } }
        public FtSequenceInvokation RootSequenceInvokation { get { return core.RootSequenceInvokation; } }

        public int RecordCount { get { return core.RecordCount; } }
        public int TableCount { get { return core.TableCount; } }

        private void HandleFieldHeadingReadReady(object sender, FtFieldHeadingReadyEventArgs e)
        {
            FieldHeadingReadReady?.Invoke(this, e);
        }

        private void HandleFieldHeadingWriteReady(object sender, FtFieldHeadingReadyEventArgs e)
        {
            FieldHeadingWriteReady?.Invoke(this, e);
        }

        private void HandleFieldValueReadReady(object sender, FtFieldValueReadyEventArgs e)
        {
            FieldValueReadReady?.Invoke(this, e);
        }

        private void HandleFieldValueWriteReady(object sender, FtFieldValueReadyEventArgs e)
        {
            FieldValueWriteReady?.Invoke(this, e);
        }

        private void HandleHeadingLineStarted(object sender, FtHeadingLineStartedEventArgs e)
        {
            HeadingLineStarted?.Invoke(this, e);
        }

        private void HandleHeadingLineFinished(object sender, FtHeadingLineFinishedEventArgs e)
        {
            HeadingLineFinished?.Invoke(this, e);
        }

        private void HandleRecordStarted(object sender, FtRecordStartedEventArgs e)
        {
            RecordStarted?.Invoke(this, e);
        }

        private void HandleRecordFinished(object sender, FtRecordFinishedEventArgs e)
        {
            RecordFinished?.Invoke(this, e);
        }

        private void HandleSequenceRedirected(object sender, FtSequenceRedirectedEventArgs e)
        {
            SequenceRedirected?.Invoke(this, e);
        }
    }
}
