using System;
using Xilytix.FieldedText;

namespace Write_Comments
{
    class Program
    {
        // Simple Example of using FtWriter to write a CSV file with comments.
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
            meta.LineCommentChar = '!'; // Is not set as with other examples.  Change to !

            // Create Writer
            using (FtWriter writer = new FtWriter(meta, CsvFileName))
            {
                writer.WriteComment("My Pets");
                writer.WriteComment("(Example writing CSV file with comments)");

                writer.WriteComment("");
                writer.WriteComment("Header");
                writer.WriteComment("");

                // only set up headings in first field
                writer.FieldList[0].Headings[0] = "First Field Heading 1";
                writer.FieldList[0].Headings[1] = "First Field Heading 2";
                writer.WriteHeader();

                writer.WriteComment("");
                writer.WriteComment("Records");
                writer.WriteComment("");

                // Write 1st Record
                writer.WriteComment("First Record");
                writer[PetNameFieldName] = "Rover";
                writer[AgeFieldName] = 4.5;
                writer[ColorFieldName] = "Brown";
                writer[DateReceivedFieldName] = new DateTime(2004, 2, 12);
                writer[PriceFieldName] = 80M;
                writer[NeedsWalkingFieldName] = true;
                writer[TypeFieldName] = "Dog";

                writer.Write();

                // Write 2nd Record
                writer.WriteComment("Second Record");
                writer[PetNameFieldName] = "Charlie";
                writer[AgeFieldName] = null;
                writer[ColorFieldName] = "Gold";
                writer[DateReceivedFieldName] = new DateTime(2007, 4, 5);
                writer[PriceFieldName] = 12.3M;
                writer[NeedsWalkingFieldName] = false;
                writer[TypeFieldName] = "Fish";

                writer.Write();

                writer.WriteComment("No more records");
            }
        }
    }
}
