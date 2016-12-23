using Xilytix.FieldedText;

namespace WriteSequence
{
    class Program
    {
        // Write a CSV file containing sequences
        // Uses ExampleSequenceMeta.ftm from BuildMetaWithSequences project
        static void Main(string[] args)
        {
            // Name of file containing Meta
            const string MetaFileName = "ExampleSequenceMeta.ftm";
            // Name of file to be written
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

            // Create Meta from file
            FtMeta meta = FtMetaSerializer.Deserialize(MetaFileName);

            // Create Writer
            using (FtWriter writer = new FtWriter(meta, CsvFileName))
            {
                // When writing records with sequences, you must not set a field's value
                // if its sequence has not yet been invoked by a redirect.
                // When writing, whenever a sequence is invoked by a redirect, its field
                // values are initialized to null
                // Root Sequence is always automatically invoked

                // Write 1st Record (1st table)
                writer[TypeFieldName] = CatType; // invoke Cat Sequence (after Root Sequence is finished)
                writer[NameFieldName] = "Misty";
                writer[RunningSpeedFieldName] = 45.0;
                writer.Write();

                // Write 2nd Record
                writer[TypeFieldName] = CatType; // invoke Cat Sequence (after Root Sequence is finished)
                writer[NameFieldName] = "Oscar";
                writer[RunningSpeedFieldName] = 35.0;
                writer.Write();

                // Write 3rd Record  (2nd table)
                writer[TypeFieldName] = DogType; // invoke Dog Sequence (after Root Sequence is finished)
                writer[NameFieldName] = "Buddy";
                writer[WalkDistanceFieldName] = 0.5;
                writer[RunningSpeedFieldName] = 35.0;
                writer[TrainingFieldName] = false;
                writer.Write();

                // Write 4th Record (3rd table)
                writer[TypeFieldName] = DogType; // invoke Dog Sequence (after Root Sequence is finished)
                writer[NameFieldName] = "Charlie";
                writer[WalkDistanceFieldName] = 2.0;
                writer[RunningSpeedFieldName] = 48.0;
                writer[TrainingFieldName] = true; // invoke Training Sequence (after this field)
                writer[TrainerFieldName] = "John";
                writer[SessionCostFieldName] = 32.0M;
                writer.Write();

                // Write 5th Record (4th table)
                writer[TypeFieldName] = DogType; // invoke Dog Sequence (after Root Sequence is finished)
                writer[NameFieldName] = "Max";
                writer[WalkDistanceFieldName] = 0.5;
                writer[RunningSpeedFieldName] = 30.0;
                writer[TrainingFieldName] = false;
                writer.Write();

                // Write 6th Record (5th table)
                writer[TypeFieldName] = GoldFishType; // invoke Gold Fish Sequence (after Root Sequence is finished)
                writer[NameFieldName] = "Bubbles";
                writer[ColorFieldName] = "Orange";
                writer[ChineseClassificationFieldName] = "Wen";
                writer.Write();

                // Write 7th Record
                writer[TypeFieldName] = GoldFishType; // invoke Gold Fish Sequence (after Root Sequence is finished)
                writer[NameFieldName] = "Flash";
                writer[ColorFieldName] = "Yellow";
                writer[ChineseClassificationFieldName] = "Crucian";
                writer.Write();
            }
        }
    }
}
