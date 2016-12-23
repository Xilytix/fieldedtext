using System;
using Xilytix.FieldedText;

namespace ReadSequence_Ordinal
{
    class Program
    {
        // This example shows how to use ordinals when reading fields.
        // An ordinal is actual the index of a field in a record.
        // If you access a field by its field name, then the name has
        // to be converted to the corresponding ordinal.
        // An field's ordinal will remain the same until the table in
        // the file has ended.  So typically, ordinals are calculated
        // from field names after the first record is read in a table.
        // These ordinal values can then be used until all the records
        // in the table have been read.
        //
        // Use ordinals when a file is large and you want to read it
        // as quickly as possible.

        static void Main(string[] args)
        {
            // Name of file containing Meta
            const string MetaFileName = "ExampleSequenceMeta.ftm";
            // Name of file to be read
            const string CsvFileName = "ExampleSequence.csv";

            // Create Meta from file
            FtMeta meta = FtMetaSerializer.Deserialize(MetaFileName);

            // Create Reader
            using (FtReader reader = new FtReader(meta, CsvFileName))
            {
                int[] recOrdinals = null;
                object[] recObjects = null;
                do
                {
                    // Loop for each table in file fl

                    while (reader.Read())
                    {
                        // Read each record in table and write field values to console

                        if (recOrdinals == null)
                        {
                            // Ordinals only need to be calculated for first row of table.
                            // Will be the same for rest of records in table 
                            recOrdinals = CalculateRecordOrdinals(reader);
                            recObjects = new object[recOrdinals.Length];
                        }

                        for (int i = 0; i < recOrdinals.Length; i++)
                        {
                            int ordinal = recOrdinals[i];
                            recObjects[i] = reader[ordinal]; // specify field by ordinal
                        }

                        Console.WriteLine(string.Format("{0},{1}: {2}", reader.RecordCount, reader.TableCount, string.Join(",", recObjects)));
                    }

                    recOrdinals = null; // Ordinals are no longer valid after table is finished

                } while (reader.NextResult());
            }
        }

        private static int[] CalculateRecordOrdinals(FtReader reader)
        {
            // Get an array of ordinals - one for each field in the current record.
            // Each ordinals is actually the index of the field with the specified name.

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

            int[] recOrdinals = new int[MaxFieldCount];

            // read Root Sequence Fields
            int typeOrdinal = reader.GetOrdinal(TypeFieldName);
            recOrdinals[0] = typeOrdinal;
            recOrdinals[1] = reader.GetOrdinal(NameFieldName);

            int fieldCount;

            // Type Field specifies what type of record this is and what fields there are
            long type = reader.GetInt64(typeOrdinal);
            switch (type)
            {
                case CatType:
                    recOrdinals[2] = reader.GetOrdinal(RunningSpeedFieldName);
                    fieldCount = 3;
                    break;

                case DogType:
                    recOrdinals[2] = reader.GetOrdinal(WalkDistanceFieldName);
                    recOrdinals[3] = reader.GetOrdinal(RunningSpeedFieldName);
                    recOrdinals[4] = reader.GetOrdinal(TrainingFieldName);
                    bool training = reader.GetBoolean(recOrdinals[4]);
                    if (!training)
                        fieldCount = 5;
                    else
                    {
                        recOrdinals[5] = reader.GetOrdinal(TrainerFieldName);
                        recOrdinals[6] = reader.GetOrdinal(SessionCostFieldName);
                        fieldCount = 7;
                    }
                    break;

                case GoldFishType:
                    recOrdinals[2] = reader.GetOrdinal(ColorFieldName);
                    recOrdinals[3] = reader.GetOrdinal(ChineseClassificationFieldName);
                    fieldCount = 4;
                    break;

                default:
                    fieldCount = 2;
                    break;
            }

            Array.Resize<int>(ref recOrdinals, fieldCount);

            return recOrdinals;
        }
    }
}
