using System;
using Xilytix.FieldedText;

namespace CountRecords
{
    class Program
    {
        // Simple Example of counting records in a CSV file.
        static void Main(string[] args)
        {
            // Name of file containing Meta
            const string MetaFileName = "BasicExampleMeta.ftm";
            // Name of file to be read
            const string CsvFileName = "BasicExample.csv";

            // Create Meta from file
            FtMeta meta = FtMetaSerializer.Deserialize(MetaFileName);

            // Create Reader
            using (FtReader reader = new FtReader(meta, CsvFileName))
            {
                reader.SeekEnd(); // Use SeekEnd() instead of ReadToEnd().  SeekEnd() is quicker

                Console.WriteLine(string.Format("Count: {0}", reader.RecordCount));
            }
        }
    }
}
