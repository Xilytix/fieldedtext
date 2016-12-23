using System;
using Xilytix.FieldedText;

namespace WriteSequence_Events
{
    // When writing files with a Meta that uses sequences, it may be easier to
    // use events.  This is because FieldValueWriteReady will fire for each field
    // in the record and will automatically take into account any sequence redirects.
    //
    // Whenever you set a field value when writing (either directly or via the
    // FieldValueWriteReady event), the writer will check for sequence redirects and
    // immediately invoke any sequence resulting from a redirection.
    // Any subsequent invokations of the FieldValueWriteReady event will take into
    // account the redirection and specify fields arising from the new sequence.
    // Accordingly the application does not need to monitor for redirections.  It
    // just sets the value of each field supplied by the FieldValueWriteReady event.

    public class Session
    {
        // Define Type values
        private const long CatType = 1;
        private const long DogType = 2;
        private const long GoldFishType = 3;

        // 1st dimension is record index, second is field id.
        // null values are not used.
        private static readonly object[,] values = new object[7, 9]
        {
            // TypeFieldId, NameFieldId, RunningSpeedFieldId, WalkDistanceFieldId, TrainingFieldId, TrainerFieldId, SessionCostFieldId, ColorFieldId, ChineseClassificationFieldId
            {CatType, "Misty", 45.0, null, null, null, null, null, null},
            {CatType, "Oscar", 35.0, null, null, null, null, null, null},
            {DogType, "Buddy", 35.0, 0.5, false, null, null, null, null},
            {DogType, "Charlie", 48.0, 2.0, true, "John", 32.0M, null, null},
            {DogType, "Max", 30.0, 0.5, false, null, null, null, null},
            {GoldFishType, "Bubbles", null, null, null, null, null, "Orange", "Wen"},
            {GoldFishType, "Flash", null, null, null, null, null, "Yellow", "Crucian"}
        };

        private bool finished;

        public void Main()
        {
            // Name of file containing Meta
            const string MetaFileName = "ExampleSequenceMeta.ftm";
            // Name of file to be written
            const string CsvFileName = "ExampleSequence.csv";

            // Create Meta from file
            FtMeta meta = FtMetaSerializer.Deserialize(MetaFileName);

            finished = false;

            // Create Writer
            using (FtWriter writer = new FtWriter(meta, CsvFileName))
            {
                writer.FieldValueWriteReady += HandleFieldValueWriteReady;
                writer.RecordFinished += HandleRecordFinished;
                     
                while (!finished)
                {
                    // loop until finished flag is set
                    writer.Write();
                }
            }
        }

        private void HandleFieldValueWriteReady(object sender, FtFieldValueReadyEventArgs e)
        {
            FtField field = e.Field;
            int recordIndex = e.RecordIndex;

            field.AsObject = values[recordIndex, field.Id];
        }

        private void HandleRecordFinished(object sender, FtRecordFinishedEventArgs e)
        {
            if (e.RecordIndex >= values.GetLength(0) - 1)
            {
                finished = true;
            }
        }
    }
}
