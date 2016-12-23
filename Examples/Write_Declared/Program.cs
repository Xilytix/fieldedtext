using System;
using Xilytix.FieldedText;

namespace Write_Declared
{
    class Program
    {
        // Example of using FtWriter to write a declared Fielded Text (CSV) file.
        static void Main(string[] args)
        {
            // Name of file containing Meta
            const string MetaFileName = "BasicExampleMeta.ftm";
            // Name of file to be written
            const string FtxFileName = "BasicExample.ftx";

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
            meta.HeadingLineCount = 0; // Is set to 2 as with other examples.  Change to 0 as we do not want any heading lines in this example

            // Create FtWriterSettings to flag we want Declared file written
            FtWriterSettings settings = new FtWriterSettings();
            settings.Declared = true;
            settings.MetaReferenceType = FtMetaReferenceType.Embedded;

            // Create Writer
            using (FtWriter writer = new FtWriter(meta, FtxFileName, settings))
            {
                // Write 1st Record
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
