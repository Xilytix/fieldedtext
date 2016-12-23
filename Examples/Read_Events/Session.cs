using System;
using Xilytix.FieldedText;

namespace Read_Events
{
    public class Session
    {
        // Name of file containing Meta
        const string MetaFileName = "BasicExampleMeta.ftm";
        // Name of file to be read
        const string CsvFileName = "BasicExample.csv";

        
        string[] headings = new string[7]; // Buffer for headings in a heading line
        object[] recObjects = new object[7]; // Buffer for values in a record

        public void Main()
        {
            // Create Meta from file
            FtMeta meta = FtMetaSerializer.Deserialize(MetaFileName);

            // Create Reader
            using (FtReader reader = new FtReader(meta, CsvFileName, false)) // do not read header immediately otherwise heading events will not fire
            {
                reader.FieldHeadingReadReady += HandleFieldHeadingReadReady;
                reader.FieldValueReadReady += HandleFieldValueReadReady;
                reader.HeadingLineStarted += HandleHeadingLineStarted;
                reader.HeadingLineFinished += HandleHeadingLineFinished;
                reader.RecordStarted += HandleRecordStarted;
                reader.RecordFinished += HandleRecordFinished;

                // Read all header and then all records.  Headings and values will be obtained via the events
                reader.ReadToEnd();
            }
        }

        private void HandleFieldHeadingReadReady(object sender, FtFieldHeadingReadyEventArgs e)
        {
            headings[e.Field.Index] = e.Field.Headings[e.LineIndex];
        }

        private void HandleFieldValueReadReady(object sender, FtFieldValueReadyEventArgs e)
        {
            recObjects[e.Field.Index] = e.Field.AsObject;
        }

        private void HandleHeadingLineStarted(object sender, FtHeadingLineStartedEventArgs e)
        {
            for (int i = 0; i < headings.Length; i++)
            {
                headings[i] = null;
            }
        }

        private void HandleHeadingLineFinished(object sender, FtHeadingLineFinishedEventArgs e)
        {
            Console.WriteLine("Heading " + e.LineIndex.ToString() + ": " + string.Join(",", headings));
        }

        private void HandleRecordStarted(object sender, FtRecordStartedEventArgs e)
        {
            for (int i = 0; i < recObjects.Length; i++)
            {
                recObjects[i] = null;
            }
        }

        private void HandleRecordFinished(object sender, FtRecordFinishedEventArgs e)
        {
            Console.WriteLine("Record " + e.RecordIndex.ToString() + ": " + string.Join(",", recObjects));
        }
    }
}
