// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System.Xml;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Xilytix.FieldedText.UnitTest
{
    [TestClass]
    public class MetaSequenceTestClass : BaseTestClass
    {
        private const string TestFolderName = "MetaSequence";
        private const string DataFolder = @"Data\MetaSequence";
        private const string SequenceFileName = "MetaSequence.xml";
        private const string ForeignFileName = "MetaSequenceWithForeign.xml";

        private const int RootField1Id = 0;
        private const int RootField2Id = 1;
        private const int RootField3Id = 2;
        private const int Sequence1Field1Id = 3;
        private const int Sequence1Field2Id = 4;
        private const int Sequence2Field1Id = 5;
        private const int Sequence2Field2Id = 6;
        private const int Sequence2Field3Id = 7;
        private const int Sequence2Field4Id = 8;
        private const int Sequence3Field1Id = 9;
        private const int Sequence4Field1Id = 10;
        private const int Sequence4Field2Id = 11;

        private const string RootSequenceName = "";
        private const string Sequence1Name = "Sequence1";
        private const string Sequence2Name = "Sequence2";
        private const string Sequence3Name = "Sequence3";
        private const string Sequence4Name = "Sequence4";

        private const FtSequenceInvokationDelay RootRedirect1InvokationDelay = FtSequenceInvokationDelay.ftikAfterSequence;
        private const long RootRedirect1Value = 1;
        private const FtSequenceInvokationDelay RootRedirect2InvokationDelay = FtSequenceInvokationDelay.ftikAfterSequence;
        private const long RootRedirect2Value = 2;
        private const FtSequenceInvokationDelay RootRedirect3InvokationDelay = FtSequenceInvokationDelay.ftikAfterSequence;
        private const long RootRedirect3Value = 3;
        private const FtSequenceInvokationDelay Sequence1Redirect1InvokationDelay = FtSequenceInvokationDelay.ftikAfterField;
        private const FtSequenceInvokationDelay Sequence2Redirect1InvokationDelay = FtSequenceInvokationDelay.ftikAfterField;
        private const string Sequence2Redirect1Value = "Go";

        private string filePath; // path for XML meta file

        public MetaSequenceTestClass() : base(TestFolderName)
        {
            filePath = Path.Combine(TestFolder, SequenceFileName);
        }

        [TestMethod]
        [DeploymentItem(DataFolder + @"\" + SequenceFileName, DataFolder)]
        [DeploymentItem(DataFolder + @"\" + ForeignFileName, DataFolder)]
        public void MetaSequence()
        {
            FtMeta meta = new FtMeta();

            FtMetaField rootField1 = meta.FieldList.New(FtStandardDataType.String);
            rootField1.Id = RootField1Id;
            FtMetaField rootField2 = meta.FieldList.New(FtStandardDataType.Integer);
            rootField2.Id = RootField2Id;
            FtMetaField rootField3 = meta.FieldList.New(FtStandardDataType.Boolean);
            rootField3.Id = RootField3Id;
            FtMetaField sequence1Field1 = meta.FieldList.New(FtStandardDataType.Float);
            sequence1Field1.Id = Sequence1Field1Id;
            FtMetaField sequence1Field2 = meta.FieldList.New(FtStandardDataType.Decimal);
            sequence1Field2.Id = Sequence1Field2Id;
            FtMetaField sequence2Field1 = meta.FieldList.New(FtStandardDataType.DateTime);
            sequence2Field1.Id = Sequence2Field1Id;
            FtMetaField sequence2Field2 = meta.FieldList.New(FtStandardDataType.Boolean);
            sequence2Field2.Id = Sequence2Field2Id;
            FtMetaField sequence2Field3 = meta.FieldList.New(FtStandardDataType.String);
            sequence2Field3.Id = Sequence2Field3Id;
            FtMetaField sequence2Field4 = meta.FieldList.New(FtStandardDataType.Integer);
            sequence2Field4.Id = Sequence2Field4Id;
            FtMetaField sequence3Field1 = meta.FieldList.New(FtStandardDataType.Boolean);
            sequence3Field1.Id = Sequence3Field1Id;
            FtMetaField sequence4Field1 = meta.FieldList.New(FtStandardDataType.Float);
            sequence4Field1.Id = Sequence4Field1Id;
            FtMetaField sequence4Field2 = meta.FieldList.New(FtStandardDataType.Decimal);
            sequence4Field2.Id = Sequence4Field2Id;

            FtMetaSequence rootSequence = meta.SequenceList.New();
            rootSequence.Root = true;
            FtMetaSequenceItem rootItem1 = rootSequence.ItemList.New();
            rootItem1.Field = rootField1;
            FtMetaSequenceItem rootItem2 = rootSequence.ItemList.New();
            rootItem2.Field = rootField2;
            FtMetaSequenceItem rootItem3 = rootSequence.ItemList.New();
            rootItem3.Field = rootField3;

            FtMetaSequence sequence1 = meta.SequenceList.New();
            sequence1.Name = Sequence1Name;
            FtMetaSequenceItem sequence1Item1 = sequence1.ItemList.New();
            sequence1Item1.Field = sequence1Field1;
            FtMetaSequenceItem sequence1Item2 = sequence1.ItemList.New();
            sequence1Item2.Field = sequence1Field2;
            FtMetaSequence sequence2 = meta.SequenceList.New();
            sequence2.Name = Sequence2Name;
            FtMetaSequenceItem sequence2Item1 = sequence2.ItemList.New();
            sequence2Item1.Field = sequence2Field1;
            FtMetaSequenceItem sequence2Item2 = sequence2.ItemList.New();
            sequence2Item2.Field = sequence2Field2;
            FtMetaSequenceItem sequence2Item3 = sequence2.ItemList.New();
            sequence2Item3.Field = sequence2Field3;
            FtMetaSequenceItem sequence2Item4 = sequence2.ItemList.New();
            sequence2Item4.Field = sequence2Field4;
            FtMetaSequence sequence3 = meta.SequenceList.New();
            sequence3.Name = Sequence3Name;
            FtMetaSequenceItem sequence3Item1 = sequence3.ItemList.New();
            sequence3Item1.Field = sequence3Field1;
            FtMetaSequence sequence4 = meta.SequenceList.New();
            sequence4.Name = Sequence4Name;
            FtMetaSequenceItem sequence4Item1 = sequence4.ItemList.New();
            sequence4Item1.Field = sequence4Field1;
            FtMetaSequenceItem sequence4Item2 = sequence4.ItemList.New();
            sequence4Item2.Field = sequence4Field2;

            FtExactIntegerMetaSequenceRedirect rootRedirect1 = rootItem2.RedirectList.New(FtStandardSequenceRedirectType.ExactInteger) as FtExactIntegerMetaSequenceRedirect;
            rootRedirect1.InvokationDelay = RootRedirect1InvokationDelay;
            rootRedirect1.Value = RootRedirect1Value;
            rootRedirect1.Sequence = sequence1;
            FtExactIntegerMetaSequenceRedirect rootRedirect2 = rootItem2.RedirectList.New(FtStandardSequenceRedirectType.ExactInteger) as FtExactIntegerMetaSequenceRedirect;
            rootRedirect2.InvokationDelay = RootRedirect2InvokationDelay;
            rootRedirect2.Value = 2;
            rootRedirect2.Sequence = sequence2;
            FtExactIntegerMetaSequenceRedirect rootRedirect3 = rootItem2.RedirectList.New(FtStandardSequenceRedirectType.ExactInteger) as FtExactIntegerMetaSequenceRedirect;
            rootRedirect3.InvokationDelay = RootRedirect3InvokationDelay;
            rootRedirect3.Value = 3;
            rootRedirect3.Sequence = sequence3;

            FtNullMetaSequenceRedirect sequence1Redirect1 = sequence1Item2.RedirectList.New(FtStandardSequenceRedirectType.Null) as FtNullMetaSequenceRedirect;
            sequence1Redirect1.InvokationDelay = Sequence1Redirect1InvokationDelay;
            sequence1Redirect1.Sequence = sequence4;

            FtCaseInsensitiveStringMetaSequenceRedirect sequence2Redirect1 = sequence2Item3.RedirectList.New(FtStandardSequenceRedirectType.CaseInsensitiveString) as FtCaseInsensitiveStringMetaSequenceRedirect;
            sequence2Redirect1.InvokationDelay = Sequence2Redirect1InvokationDelay;
            sequence2Redirect1.Value = Sequence2Redirect1Value;
            sequence2Redirect1.Sequence = sequence4;

            // Serialize Meta
            XmlWriter writer = XmlWriter.Create(filePath, xmlWriterSettings);
            FtMetaSerializer.Serialize(meta, writer);
            writer.Close();

            string DataFilePath = Path.Combine(DataFolder, SequenceFileName);
            if (!TextFilesAreEqual(filePath, DataFilePath))
                Assert.Fail("MetaSequence does not match Test Data");
            else
            {
                FtMeta deserializedMeta = FtMetaSerializer.Deserialize(filePath);
                AssertDeserializedMeta(deserializedMeta);

                string foreignFilePath = Path.Combine(DataFolder, ForeignFileName);
                FtMeta deserializedForeign = FtMetaSerializer.Deserialize(foreignFilePath);
                AssertDeserializedMeta(deserializedForeign);
            }
        }

        private void AssertDeserializedMeta(FtMeta meta)
        {
            Assert.AreEqual<int>(12, meta.FieldList.Count);
            for (int i = 0; i < 12; i++)
            {
                Assert.AreEqual<int>(i, meta.FieldList[i].Id);
            }

            FtMetaField rootField1 = meta.FieldList[0];
            Assert.AreEqual<int>(FtStandardDataType.String, rootField1.DataType);
            FtMetaField rootField2 = meta.FieldList[1];
            Assert.AreEqual<int>(FtStandardDataType.Integer, rootField2.DataType);
            FtMetaField rootField3 = meta.FieldList[2];
            Assert.AreEqual<int>(FtStandardDataType.Boolean, rootField3.DataType);
            FtMetaField sequence1Field1 = meta.FieldList[3];
            Assert.AreEqual<int>(FtStandardDataType.Float, sequence1Field1.DataType);
            FtMetaField sequence1Field2 = meta.FieldList[4];
            Assert.AreEqual<int>(FtStandardDataType.Decimal, sequence1Field2.DataType);
            FtMetaField sequence2Field1 = meta.FieldList[5];
            Assert.AreEqual<int>(FtStandardDataType.DateTime, sequence2Field1.DataType);
            FtMetaField sequence2Field2 = meta.FieldList[6];
            Assert.AreEqual<int>(FtStandardDataType.Boolean, sequence2Field2.DataType);
            FtMetaField sequence2Field3 = meta.FieldList[7];
            Assert.AreEqual<int>(FtStandardDataType.String, sequence2Field3.DataType);
            FtMetaField sequence2Field4 = meta.FieldList[8];
            Assert.AreEqual<int>(FtStandardDataType.Integer, sequence2Field4.DataType);
            FtMetaField sequence3Field1 = meta.FieldList[9];
            Assert.AreEqual<int>(FtStandardDataType.Boolean, sequence3Field1.DataType);
            FtMetaField sequence4Field1 = meta.FieldList[10];
            Assert.AreEqual<int>(FtStandardDataType.Float, sequence4Field1.DataType);
            FtMetaField sequence4Field2 = meta.FieldList[11];
            Assert.AreEqual<int>(FtStandardDataType.Decimal, sequence4Field2.DataType);

            Assert.AreEqual<int>(5, meta.SequenceList.Count);

            FtMetaSequence rootSequence = meta.SequenceList[0];
            Assert.AreEqual<bool>(true, rootSequence.Root);
            Assert.AreEqual<string>(RootSequenceName, rootSequence.Name);
            Assert.AreEqual<int>(3, rootSequence.ItemList.Count);
            FtMetaSequence sequence1 = meta.SequenceList[1];
            Assert.AreEqual<bool>(false, sequence1.Root);
            Assert.AreEqual<string>(Sequence1Name, sequence1.Name);
            Assert.AreEqual<int>(2, sequence1.ItemList.Count);
            FtMetaSequence sequence2 = meta.SequenceList[2];
            Assert.AreEqual<bool>(false, sequence2.Root);
            Assert.AreEqual<string>(Sequence2Name, sequence2.Name);
            Assert.AreEqual<int>(4, sequence2.ItemList.Count);
            FtMetaSequence sequence3 = meta.SequenceList[3];
            Assert.AreEqual<bool>(false, sequence3.Root);
            Assert.AreEqual<string>(Sequence3Name, sequence3.Name);
            Assert.AreEqual<int>(1, sequence3.ItemList.Count);
            FtMetaSequence sequence4 = meta.SequenceList[4];
            Assert.AreEqual<bool>(false, sequence4.Root);
            Assert.AreEqual<string>(Sequence4Name, sequence4.Name);
            Assert.AreEqual<int>(2, sequence4.ItemList.Count);

            Assert.AreEqual<FtMetaField>(rootSequence.ItemList[0].Field, rootField1);
            FtMetaSequenceItem rootItem2 = rootSequence.ItemList[1];
            Assert.AreEqual<FtMetaField>(rootItem2.Field, rootField2);
            Assert.AreEqual<FtMetaField>(rootSequence.ItemList[2].Field, rootField3);
            Assert.AreEqual<FtMetaField>(sequence1.ItemList[0].Field, sequence1Field1);
            FtMetaSequenceItem sequence1Item2 = sequence1.ItemList[1];
            Assert.AreEqual<FtMetaField>(sequence1Item2.Field, sequence1Field2);
            Assert.AreEqual<FtMetaField>(sequence2.ItemList[0].Field, sequence2Field1);
            Assert.AreEqual<FtMetaField>(sequence2.ItemList[1].Field, sequence2Field2);
            FtMetaSequenceItem sequence2Item3 = sequence2.ItemList[2];
            Assert.AreEqual<FtMetaField>(sequence2Item3.Field, sequence2Field3);
            Assert.AreEqual<FtMetaField>(sequence2.ItemList[3].Field, sequence2Field4);
            Assert.AreEqual<FtMetaField>(sequence3.ItemList[0].Field, sequence3Field1);
            Assert.AreEqual<FtMetaField>(sequence4.ItemList[0].Field, sequence4Field1);
            Assert.AreEqual<FtMetaField>(sequence4.ItemList[1].Field, sequence4Field2);

            FtExactIntegerMetaSequenceRedirect rootRedirect1 = rootItem2.RedirectList[0] as FtExactIntegerMetaSequenceRedirect;
            Assert.AreEqual<int>(FtStandardSequenceRedirectType.ExactInteger, ((FtMetaSequenceRedirect)rootRedirect1).Type);
            Assert.AreEqual<FtSequenceInvokationDelay>(RootRedirect1InvokationDelay, rootRedirect1.InvokationDelay);
            Assert.AreEqual<long>(RootRedirect1Value, rootRedirect1.Value);
            Assert.AreEqual<FtMetaSequence>(sequence1, rootRedirect1.Sequence);

            FtExactIntegerMetaSequenceRedirect rootRedirect2 = rootItem2.RedirectList[1] as FtExactIntegerMetaSequenceRedirect;
            Assert.AreEqual<int>(FtStandardSequenceRedirectType.ExactInteger, ((FtMetaSequenceRedirect)rootRedirect2).Type);
            Assert.AreEqual<FtSequenceInvokationDelay>(RootRedirect2InvokationDelay, rootRedirect2.InvokationDelay);
            Assert.AreEqual<long>(RootRedirect2Value, rootRedirect2.Value);
            Assert.AreEqual<FtMetaSequence>(sequence2, rootRedirect2.Sequence);

            FtExactIntegerMetaSequenceRedirect rootRedirect3 = rootItem2.RedirectList[2] as FtExactIntegerMetaSequenceRedirect;
            Assert.AreEqual<int>(FtStandardSequenceRedirectType.ExactInteger, ((FtMetaSequenceRedirect)rootRedirect3).Type);
            Assert.AreEqual<FtSequenceInvokationDelay>(RootRedirect3InvokationDelay, rootRedirect3.InvokationDelay);
            Assert.AreEqual<long>(RootRedirect3Value, rootRedirect3.Value);
            Assert.AreEqual<FtMetaSequence>(sequence3, rootRedirect3.Sequence);

            FtNullMetaSequenceRedirect sequence1Redirect1 = sequence1Item2.RedirectList[0] as FtNullMetaSequenceRedirect;
            Assert.AreEqual<int>(FtStandardSequenceRedirectType.Null, ((FtMetaSequenceRedirect)sequence1Redirect1).Type);
            Assert.AreEqual<FtSequenceInvokationDelay>(Sequence1Redirect1InvokationDelay, sequence1Redirect1.InvokationDelay);
            Assert.AreEqual<FtMetaSequence>(sequence4, sequence1Redirect1.Sequence);

            FtCaseInsensitiveStringMetaSequenceRedirect sequence2Redirect1 = sequence2Item3.RedirectList[0] as FtCaseInsensitiveStringMetaSequenceRedirect;
            Assert.AreEqual<int>(FtStandardSequenceRedirectType.CaseInsensitiveString, ((FtMetaSequenceRedirect)sequence2Redirect1).Type);
            Assert.AreEqual<FtSequenceInvokationDelay>(Sequence2Redirect1InvokationDelay, sequence2Redirect1.InvokationDelay);
            Assert.AreEqual<FtMetaSequence>(sequence4, sequence2Redirect1.Sequence);
        }
    }
}
