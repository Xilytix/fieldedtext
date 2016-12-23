using System;
using Xilytix.FieldedText;

namespace Write_Events
{
    public class Session
    {
        // Name of file containing Meta
        const string MetaFileName = "BasicExampleMeta.ftm";
        // Name of file to be written
        const string CsvFileName = "BasicExample.csv";

        // Define Field Ids
        const int PetNameFieldId = 1;
        const int AgeFieldId = 2;
        const int ColorFieldId = 3;
        const int DateReceivedFieldId = 4;
        const int PriceFieldId = 5;
        const int NeedsWalkingFieldId = 6;
        const int TypeFieldId = 7;

        private bool finished;

        public void Main()
        {
            // Create Meta from file
            FtMeta meta = FtMetaSerializer.Deserialize(MetaFileName);

            finished = false;

            // Create Writer
            using (FtWriter writer = new FtWriter(meta, CsvFileName))
            {
                writer.FieldHeadingWriteReady += HandleFieldHeadingWriteReady;
                writer.FieldValueWriteReady += HandleFieldValueWriteReady;
                writer.RecordFinished += HandleRecordFinished;

                while (!finished)
                {
                    // loop until finished flag is set
                    writer.Write();
                }
            }
        }

        private string CalculateHeading(int fieldId, int lineIndex)
        {
            switch (fieldId)
            {
                case PetNameFieldId: return (lineIndex == 0) ? "Pet Name" : "";
                case AgeFieldId: return (lineIndex == 0) ? "Age" : "(Years)";
                case ColorFieldId: return (lineIndex == 0) ? "Color" : "";
                case DateReceivedFieldId: return (lineIndex == 0) ? "Date Received" : "";
                case PriceFieldId: return (lineIndex == 0) ? "Price" : "(Dollars)";
                case NeedsWalkingFieldId: return (lineIndex == 0) ? "Needs Walking" : "";
                case TypeFieldId: return (lineIndex == 0) ? "Type" : "";
                default: return "";
            }
        }

        private object CalculateValue(int fieldId, int recordIndex)
        {
            switch (fieldId)
            {
                case PetNameFieldId: return (recordIndex == 0) ? "Rover" : "Charlie";
                case AgeFieldId: return (recordIndex == 0) ? (object)4.5 : null;
                case ColorFieldId: return (recordIndex == 0) ? "Brown" : "Gold";
                case DateReceivedFieldId: return (recordIndex == 0) ? (object)new DateTime(2004, 2, 12) : new DateTime(2007, 4, 5);
                case PriceFieldId: return (recordIndex == 0) ? (object)80M : 12.3M;
                case NeedsWalkingFieldId: return (recordIndex == 0) ? (object)true : false;
                case TypeFieldId: return (recordIndex == 0) ? "Dog" : "Fish";
                default: return "";
            }
        }

        private void HandleFieldHeadingWriteReady(object sender, FtFieldHeadingReadyEventArgs e)
        {
            FtField field = e.Field;
            int lineIndex = e.LineIndex;

            field.Headings[lineIndex] = CalculateHeading(field.Id, lineIndex);
        }

        private void HandleFieldValueWriteReady(object sender, FtFieldValueReadyEventArgs e)
        {
            FtField field = e.Field;
            int recordIndex = e.RecordIndex;

            field.AsObject = CalculateValue(field.Id, recordIndex);
        }

        private void HandleRecordFinished(object sender, FtRecordFinishedEventArgs e)
        {
            if (e.RecordIndex >= 1)
            {
                finished = true;
            }
        }
    }
}
