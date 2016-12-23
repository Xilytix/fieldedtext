using System;
using Xilytix.FieldedText;

namespace BasicRead_LoadMeta
{
    // Simple Example of using FtReader to parse a CSV file.
    // In this example, the Meta is loaded from a file and then used to read the CSV file.
    class Program
    {
        static void Main(string[] args)
        {
            // Name of file containing Meta
            const string MetaFileName = "BasicExampleMeta.ftm";
            // Name of file to be read
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

            // Create Reader
            using (FtReader reader = new FtReader(meta, CsvFileName))
            {
                // Read each record in text file and write field values to console
                object[] recObjects = new object[7];
                int recNumber = 0;
                while (reader.Read())
                {
                    recNumber++;

                    recObjects[0] = reader[PetNameFieldName];
                    recObjects[1] = reader[AgeFieldName];
                    recObjects[2] = reader[ColorFieldName];
                    recObjects[3] = reader[DateReceivedFieldName];
                    recObjects[4] = reader[PriceFieldName];
                    recObjects[5] = reader[NeedsWalkingFieldName];
                    recObjects[6] = reader[TypeFieldName];

                    Console.WriteLine(recNumber.ToString() + ": " + string.Join(",", recObjects));
                }
            }
        }
    }
}
