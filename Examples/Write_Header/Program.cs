using System;
using Xilytix.FieldedText;

namespace Write_Headings
{
    class Program
    {
        // Simple Example of using FtWriter to write a CSV file with headings.
        static void Main(string[] args)
        {
            // Name of file containing Meta
            const string MetaFileName = "BasicExampleMeta.ftm";
            // Name of file to be written
            const string CsvFileName = "BasicExample.csv";

            // Define FieldNames
            const string PetNameFieldName = "PetName";
            const string AgeFieldName = "Age";
            const string ColorFieldName = "Color";
            const string DateReceivedFieldName = "DateReceived";
            const string PriceFieldName = "Price";
            const string NeedsWalkingFieldName = "NeedsWalking";
            const string TypeFieldName = "Type";

            // Create Meta from file
            FtMeta meta = FtMetaSerializer.Deserialize(MetaFileName);

            // Create Writer
            using (FtWriter writer = new FtWriter(meta, CsvFileName))
            {
                FtStringField petNameField = writer.FieldList[PetNameFieldName] as FtStringField;
                FtFloatField ageField = writer.FieldList[AgeFieldName] as FtFloatField;
                FtStringField colorField = writer.FieldList[ColorFieldName] as FtStringField;
                FtDateTimeField dateReceivedField = writer.FieldList[DateReceivedFieldName] as FtDateTimeField;
                FtDecimalField priceField = writer.FieldList[PriceFieldName] as FtDecimalField;
                FtBooleanField needsWalkingField = writer.FieldList[NeedsWalkingFieldName] as FtBooleanField;
                FtStringField typeField = writer.FieldList[TypeFieldName] as FtStringField;

                // set headings before writing first record
                petNameField.Headings[0] = "Pet Name";
                ageField.Headings[0] = "Age";
                ageField.Headings[1] = "(Years)";
                colorField.Headings[0] = "Color";
                dateReceivedField.Headings[0] = "Date Received";
                priceField.Headings[0] = "Price";
                priceField.Headings[1] = "(Dollars)";
                needsWalkingField.Headings[0] = "Needs Walking";
                typeField.Headings[0] = "Type";

                // Write 1st Record - this will write header (including heading lines)
                writer[PetNameFieldName] = "Rover";
                writer[AgeFieldName] = 4.5;
                writer[ColorFieldName] = "Brown";
                writer[DateReceivedFieldName] = new DateTime(2004, 2, 12);
                writer[PriceFieldName] = 80M;
                writer[NeedsWalkingFieldName] = true;
                writer[TypeFieldName] = "Dog";

                writer.Write();

                // Write 2nd Record
                writer[PetNameFieldName] = "Charlie";
                writer[AgeFieldName] = null;
                writer[ColorFieldName] = "Gold";
                writer[DateReceivedFieldName] = new DateTime(2007, 4, 5);
                writer[PriceFieldName] = 12.3M;
                writer[NeedsWalkingFieldName] = false;
                writer[TypeFieldName] = "Fish";

                writer.Write();
            }
        }
    }
}
