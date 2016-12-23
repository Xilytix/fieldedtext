using System;
using Xilytix.FieldedText;

namespace ReadSequence
{
    class Program
    {
        static void Main(string[] args)
        {
            // Name of file containing Meta
            const string MetaFileName = "ExampleSequenceMeta.ftm";
            // Name of file to be read
            const string CsvFileName = "ExampleSequence.csv";

            // Define Field Names
            const string TypeFieldName = "Type";
            const string NameFieldName = "Name";
            const string RunningSpeedFieldName = "RunningSpeed";
            const string WalkDistanceFieldName = "WalkDistance";
            const string TrainingFieldName = "Training";
            const string TrainerFieldName = "Trainer";
            const string SessionCostFieldName = "SessionCost";
            const string ColorFieldName = "Color";
            const string ChineseClassificationFieldName = "ChineseClassification";

            // Define Type values
            const long CatType = 1;
            const long DogType = 2;
            const long GoldFishType = 3;

            const int MaxFieldCount = 7;

            // Create Meta from file
            FtMeta meta = FtMetaSerializer.Deserialize(MetaFileName);

            // Create Reader
            using (FtReader reader = new FtReader(meta, CsvFileName))
            {
                // Normally a reader.Read() will return false when a new table starts in a file.
                // You would then need to call reader.NextResult() (or reader.NextTable()) to
                // read the record from the next table.
                // When a new table starts, you would need to get new field ordinal values with
                // reader.GetOrdinal() if you are using field ordinals.
                // However this example does not use field ordinal values.  It just uses field names.
                // Accordingly we want to reader.Read() to return true until no more records.
                // Set AutoNextTable to true for this behaviour.
                reader.AutoNextTable = true;

                // Read each record in text file and write field values to console
                int recNumber = 0;
                while (reader.Read())
                {
                    recNumber++;
                    object[] recObjects = new object[MaxFieldCount];

                    // read Root Sequence Fields
                    recObjects[0] = reader[TypeFieldName]; 
                    recObjects[1] = reader[NameFieldName];

                    int fieldCount;

                    // Type Field specifies what type of record this is and what fields there are
                    long type = (long)recObjects[0];
                    switch (type)
                    {
                        case CatType:
                            recObjects[2] = reader[RunningSpeedFieldName];
                            fieldCount = 3;
                            break;

                        case DogType:
                            recObjects[2] = reader[WalkDistanceFieldName];
                            recObjects[3] = reader[RunningSpeedFieldName];
                            recObjects[4] = reader[TrainingFieldName];
                            bool training = (bool)recObjects[4];
                            if (!training)
                                fieldCount = 5;
                            else
                            {
                                recObjects[5] = reader[TrainerFieldName];
                                recObjects[6] = reader[SessionCostFieldName];
                                fieldCount = 7;
                            }
                            break;

                        case GoldFishType:
                            recObjects[2] = reader[ColorFieldName];
                            recObjects[3] = reader[ChineseClassificationFieldName];
                            fieldCount = 4;
                            break;

                        default:
                            fieldCount = 2;
                            break;

                    }

                    Array.Resize<object>(ref recObjects, fieldCount);

                    Console.WriteLine(recNumber.ToString() + ": " + string.Join(",", recObjects));
                }
            }
        }
    }
}
