using System;
using Xilytix.FieldedText;

namespace BasicRead_BuildMeta
{
    // Simple Example of using FtReader to parse a CSV file.
    // In this example, the Meta is created programatically and then used to read the CSV file.
    class Program
    {
        static void Main(string[] args)
        {
            // Name of file to be read
            const string FileName = "BasicExample.csv";

            // Define FieldNames
            const string PetNameFieldName = "PetName";
            const string AgeFieldName = "Age";
            const string ColorFieldName = "Color";
            const string DateReceivedFieldName = "DateReceived";
            const string PriceFieldName = "Price";
            const string NeedsWalkingFieldName = "NeedsWalking";
            const string TypeFieldName = "Type";

            // Create Meta that represents structure and format of BasicExample.csv file
            FtMeta meta = new FtMeta();

            // All FtMeta defaults apply to BasicExample.csv except for HeadingLineCount
            meta.HeadingLineCount = 2;

            // Add fields
            // Add in order of fields in file so index does not have to be explicitly set
            FtMetaField metaField;
            metaField = meta.FieldList.New(FtStandardDataType.String);
            metaField.Name = PetNameFieldName;
            metaField = meta.FieldList.New(FtStandardDataType.Float);
            metaField.Name = AgeFieldName;
            metaField = meta.FieldList.New(FtStandardDataType.String);
            metaField.Name = ColorFieldName;
            metaField = meta.FieldList.New(FtStandardDataType.DateTime);
            metaField.Name = DateReceivedFieldName;
            // Default format for Date Time is not used in "Date Received" field.  Specify the date format used.
            ((FtDateTimeMetaField)metaField).Format = "d MMM yyyy";
            metaField = meta.FieldList.New(FtStandardDataType.Decimal);
            metaField.Name = PriceFieldName;
            metaField = meta.FieldList.New(FtStandardDataType.Boolean);
            metaField.Name = NeedsWalkingFieldName;
            metaField = meta.FieldList.New(FtStandardDataType.String);
            metaField.Name = TypeFieldName;

            // Create Reader
            using (FtReader reader = new FtReader(meta, FileName))
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
