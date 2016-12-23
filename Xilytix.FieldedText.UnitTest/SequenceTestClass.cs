// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Xilytix.FieldedText.UnitTest
{
    [TestClass]
    public class SequenceTestClass : BaseTestClass
    {
        private const string TestFolderName = "Sequence";
        private const string DataFolder = @"Data\Sequence";
        private const string SequenceMetaFileName = "SequenceMeta.ftm";
        private const string SequenceFileName = "Sequence.txt";

        private const string Root1_StrConstFieldName = "Root1_StrConst";
        private const string Root2_DateTimeFieldName = "Root2_DateTime";
        private const string Root3_IntRedirFieldName = "Root3_IntRedir";
        private const string Level1_1_DecimalFieldName = "Level1_1_Decimal";
        private const string Level1_1_FloatRedirFieldName = "Level1_1_FloatRedir";
        private const string Level1_Null_BooleanFieldName = "Level1_Null_Boolean";
        private const string Level1_1_StringFieldName = "Level1_1_String";
        private const string Level1_2_DateTimeFieldName = "Level1_2_DateTime";
        private const string Level1_3_StringFieldName = "Level1_3_String";
        private const string Level2_10_StringFieldName = "Level2_10_String";
        private const string Level2_11_StringFieldName = "Level2_11_String";

        private const int Root_FieldListCount = 3;
        private const int Root_SequenceInvokationListCount = 1;

        private const string Root1StrConstValue = "T";

        private readonly DateTime Record1_Root2DateTimeValue = new DateTime(2016, 3, 5);
        private const long Record1_Root3IntRedirValue = 20;
        private const int Record1_FieldListCount = Root_FieldListCount;
        private const int Record1_SequenceInvokationListCount = Root_SequenceInvokationListCount;
        private const int Record1_TableListCount = 1;

        private readonly DateTime Record2_Root2DateTimeValue = new DateTime(1916, 1, 5);
        private const long Record2_Root3IntRedirValue = 20;
        private const int Record2_FieldListCount = Root_FieldListCount;
        private const int Record2_SequenceInvokationListCount = Root_SequenceInvokationListCount;
        private const int Record2_TableListCount = 1;

        private readonly DateTime Record3_Root2DateTimeValue = new DateTime(2016, 1, 4);
        private const long Record3_Root3IntRedirValue = 1;
        private const decimal Record3_Level1_1_DecimalValue = 100.1M;
        private const double Record3_Level1_1_FloatRedirValue = 1000.1;
        private const string Record3_Level1_1_StringValue = "Level1_1 First";
        private const int Record3_FieldListCount = 6;
        private const int Record3_SequenceInvokationListCount = 2;
        private const int Record3_TableListCount = 2;

        private readonly DateTime Record4_Root2DateTimeValue = new DateTime(2016, 1, 5);
        private const long Record4_Root3IntRedirValue = 1;
        private const decimal Record4_Level1_1_DecimalValue = -100.3M;
        private const double Record4_Level1_1_FloatRedirValue = 1000.4;
        private const string Record4_Level1_1_StringValue = "Level1_1 Second";
        private const int Record4_FieldListCount = 6;
        private const int Record4_SequenceInvokationListCount = 2;
        private const int Record4_TableListCount = 2;

        private readonly DateTime Record5_Root2DateTimeValue = new DateTime(2016, 3, 5);
        private const long Record5_Root3IntRedirValue = 1;
        private const decimal Record5_Level1_1_DecimalValue = -99.3M;
        private const double Record5_Level1_1_FloatRedirValue = 10001.4;
        private const string Record5_Level1_1_StringValue = "Level1_1 Third";
        private const int Record5_FieldListCount = 6;
        private const int Record5_SequenceInvokationListCount = 2;
        private const int Record5_TableListCount = 2;

        private readonly DateTime Record6_Root2DateTimeValue = new DateTime(2016, 3, 5);
        private const bool Record6_level1_Null_BooleanValue = true;
        private const int Record6_FieldListCount = 4;
        private const int Record6_SequenceInvokationListCount = 2;
        private const int Record6_TableListCount = 3;

        private readonly DateTime Record7_Root2DateTimeValue = new DateTime(1003, 3, 5);
        private const long Record7_Root3IntRedirValue = 2;
        private const int Record7_FieldListCount = 4;
        private const int Record7_SequenceInvokationListCount = 2;
        private const int Record7_TableListCount = 4;

        private readonly DateTime Record8_Root2DateTimeValue = new DateTime(2019, 3, 5);
        private const long Record8_Root3IntRedirValue = 1;
        private const decimal Record8_Level1_1_DecimalValue = -99.3M;
        private const double Record8_Level1_1_FloatRedirValue = 10;
        private const string Record8_level2_10_StringValue = "Level2_10 1st";
        private const int Record8_FieldListCount = 6;
        private const int Record8_SequenceInvokationListCount = 3;
        private const int Record8_TableListCount = 5;

        private readonly DateTime Record9_Root2DateTimeValue = new DateTime(2029, 3, 5);
        private const long Record9_Root3IntRedirValue = 1;
        private const decimal Record9_Level1_1_DecimalValue = -9;
        private const double Record9_Level1_1_FloatRedirValue = 10;
        private const string Record9_level2_10_StringValue = "Level2_10 2nd";
        private const int Record9_FieldListCount = 6;
        private const int Record9_SequenceInvokationListCount = 3;
        private const int Record9_TableListCount = 5;

        private readonly DateTime Record10_Root2DateTimeValue = new DateTime(2029, 3, 5);
        private const long Record10_Root3IntRedirValue = 1;
        private const decimal Record10_Level1_1_DecimalValue = -9;
        private const double Record10_Level1_1_FloatRedirValue = 11;
        private const string Record10_Level1_1_StringValue = "Stay";
        private const string Record10_level2_11_StringValue = "Level2_11 1st";
        private const int Record10_FieldListCount = 7;
        private const int Record10_SequenceInvokationListCount = 3;
        private const int Record10_TableListCount = 6;

        private readonly DateTime Record11_Root2DateTimeValue = new DateTime(2036, 3, 5);
        private const long Record11_Root3IntRedirValue = 30;
        private const int Record11_FieldListCount = Root_FieldListCount;
        private const int Record11_SequenceInvokationListCount = Root_SequenceInvokationListCount;
        private const int Record11_TableListCount = 7;

        private string filePath;

        public SequenceTestClass() : base(TestFolderName)
        {
            filePath = Path.Combine(TestFolder, SequenceFileName);
        }

        [TestMethod]
        [DeploymentItem(DataFolder + @"\" + SequenceMetaFileName, DataFolder)]
        [DeploymentItem(DataFolder + @"\" + SequenceFileName, DataFolder)]
        public void Sequence()
        {
            int fieldsAffectedFromIndex;

            string metaFilePath = Path.Combine(DataFolder, SequenceMetaFileName);
            FtMeta meta = FtMetaSerializer.Deserialize(metaFilePath);

            FtWriter writer = new FtWriter(meta, filePath);

            // root fields
            FtStringField root1_StrConstField;
            FtDateTimeField root2_DateTimeField;
            FtIntegerField root3_IntRedirField;
            FtDecimalField level1_1_DecimalField;
            FtFloatField level1_1_FloatRedirField;
            FtBooleanField level1_Null_BooleanField;
            FtStringField level1_1_StringField;
            FtDateTimeField level1_2_DateTimeField;
            //FtStringField level1_3_StringField;
            FtStringField level2_10_StringField;
            FtStringField level2_11_StringField;

            // in this case, root fields are never affected by redirects
            root1_StrConstField = writer.FieldList[Root1_StrConstFieldName] as FtStringField;
            root2_DateTimeField = writer.FieldList[Root2_DateTimeFieldName] as FtDateTimeField;
            root3_IntRedirField = writer.FieldList[Root3_IntRedirFieldName] as FtIntegerField;

            // Record 1 - no redirect
            root2_DateTimeField.Value = Record1_Root2DateTimeValue;
            root3_IntRedirField.SetValue(Record1_Root3IntRedirValue, out fieldsAffectedFromIndex);
            Assert.AreEqual(-1, fieldsAffectedFromIndex);
            Assert.AreEqual(Record1_FieldListCount, writer.FieldList.Count);
            Assert.AreEqual(Root_SequenceInvokationListCount, writer.SequenceInvokationList.Count);
            writer.Write();

            Assert.AreEqual(1, writer.RecordCount);
            Assert.AreEqual(Record1_TableListCount, writer.TableCount);
            Assert.AreEqual(Record1_SequenceInvokationListCount, writer.SequenceInvokationList.Count);

            // Record 2 - no redirect
            root2_DateTimeField.Value = Record2_Root2DateTimeValue;
            root3_IntRedirField.SetValue(Record2_Root3IntRedirValue, out fieldsAffectedFromIndex);
            Assert.AreEqual(-1, fieldsAffectedFromIndex);
            Assert.AreEqual(Record2_FieldListCount, writer.FieldList.Count);
            Assert.AreEqual(Record2_SequenceInvokationListCount, writer.SequenceInvokationList.Count);
            writer.Write();

            Assert.AreEqual(2, writer.RecordCount);
            Assert.AreEqual(Record2_TableListCount, writer.TableCount);
            Assert.AreEqual(Root_SequenceInvokationListCount, writer.SequenceInvokationList.Count);

            // Record 3 - root redirect 1
            root2_DateTimeField.Value = Record3_Root2DateTimeValue;
            root3_IntRedirField.SetValue(Record3_Root3IntRedirValue, out fieldsAffectedFromIndex);
            // redirect to level1_1 sequence
            {
                Assert.AreEqual(3, fieldsAffectedFromIndex);
                Assert.AreEqual(Record3_FieldListCount, writer.FieldList.Count);
                Assert.AreEqual(Record3_SequenceInvokationListCount, writer.SequenceInvokationList.Count);

                // remove redirection
                root3_IntRedirField.SetValue(20, out fieldsAffectedFromIndex);
                Assert.AreEqual(3, fieldsAffectedFromIndex);
                Assert.AreEqual(Root_FieldListCount, writer.FieldList.Count);
                Assert.AreEqual(Root_SequenceInvokationListCount, writer.SequenceInvokationList.Count);

                root3_IntRedirField.SetValue(Record3_Root3IntRedirValue, out fieldsAffectedFromIndex);
                Assert.AreEqual(3, fieldsAffectedFromIndex);
                Assert.AreEqual(Record3_FieldListCount, writer.FieldList.Count);
                Assert.AreEqual(Record3_SequenceInvokationListCount, writer.SequenceInvokationList.Count);

                level1_1_DecimalField = writer.FieldList[Level1_1_DecimalFieldName] as FtDecimalField;
                level1_1_FloatRedirField = writer.FieldList[Level1_1_FloatRedirFieldName] as FtFloatField;
                level1_1_StringField = writer.FieldList[Level1_1_StringFieldName] as FtStringField;

                level1_1_DecimalField.Value = Record3_Level1_1_DecimalValue;
                level1_1_FloatRedirField.Value = Record3_Level1_1_FloatRedirValue;
                level1_1_StringField.Value = Record3_Level1_1_StringValue;
            }
            writer.Write();

            Assert.AreEqual(3, writer.RecordCount);
            Assert.AreEqual(Record3_TableListCount, writer.TableCount);
            Assert.AreEqual(Root_SequenceInvokationListCount, writer.SequenceInvokationList.Count);

            // Record 4 - root redirect 1
            root2_DateTimeField.Value = Record4_Root2DateTimeValue;
            root3_IntRedirField.SetValue(Record4_Root3IntRedirValue, out fieldsAffectedFromIndex);
            // redirect to level1_1 sequence
            {
                Assert.AreEqual(3, fieldsAffectedFromIndex);
                Assert.AreEqual(Record4_FieldListCount, writer.FieldList.Count);
                Assert.AreEqual(Record4_SequenceInvokationListCount, writer.SequenceInvokationList.Count);

                level1_1_DecimalField = writer.FieldList[Level1_1_DecimalFieldName] as FtDecimalField;
                level1_1_FloatRedirField = writer.FieldList[Level1_1_FloatRedirFieldName] as FtFloatField;
                level1_1_StringField = writer.FieldList[Level1_1_StringFieldName] as FtStringField;

                level1_1_DecimalField.Value = Record4_Level1_1_DecimalValue;
                level1_1_FloatRedirField.Value = Record4_Level1_1_FloatRedirValue;
                level1_1_StringField.Value = Record4_Level1_1_StringValue;
            }
            writer.Write();

            Assert.AreEqual(4, writer.RecordCount);
            Assert.AreEqual(Record4_TableListCount, writer.TableCount);
            Assert.AreEqual(Root_SequenceInvokationListCount, writer.SequenceInvokationList.Count);

            // Record 5 - root redirect 1
            root2_DateTimeField.Value = Record5_Root2DateTimeValue;
            root3_IntRedirField.SetValue(Record5_Root3IntRedirValue, out fieldsAffectedFromIndex);
            // redirect to level1_1 sequence
            {
                Assert.AreEqual(3, fieldsAffectedFromIndex);
                Assert.AreEqual(Record5_FieldListCount, writer.FieldList.Count);
                Assert.AreEqual(Record4_SequenceInvokationListCount, writer.SequenceInvokationList.Count);

                level1_1_DecimalField = writer.FieldList[Level1_1_DecimalFieldName] as FtDecimalField;
                level1_1_FloatRedirField = writer.FieldList[Level1_1_FloatRedirFieldName] as FtFloatField;
                level1_1_StringField = writer.FieldList[Level1_1_StringFieldName] as FtStringField;

                level1_1_DecimalField.Value = Record5_Level1_1_DecimalValue;
                level1_1_FloatRedirField.Value = Record5_Level1_1_FloatRedirValue;
                level1_1_StringField.Value = Record5_Level1_1_StringValue;
            }
            writer.Write();

            Assert.AreEqual(5, writer.RecordCount);
            Assert.AreEqual(Record5_TableListCount, writer.TableCount);
            Assert.AreEqual(Root_SequenceInvokationListCount, writer.SequenceInvokationList.Count);

            // Record 6 - root redirect Null
            root2_DateTimeField.Value = Record6_Root2DateTimeValue;
            root3_IntRedirField.SetNull(out fieldsAffectedFromIndex);
            // redirect to level1_Null sequence
            {
                Assert.AreEqual(3, fieldsAffectedFromIndex);
                Assert.AreEqual(Record6_FieldListCount, writer.FieldList.Count);
                Assert.AreEqual(Record6_SequenceInvokationListCount, writer.SequenceInvokationList.Count);

                level1_Null_BooleanField = writer.FieldList[Level1_Null_BooleanFieldName] as FtBooleanField;

                level1_Null_BooleanField.Value = Record6_level1_Null_BooleanValue;
            }
            writer.Write();

            Assert.AreEqual(6, writer.RecordCount);
            Assert.AreEqual(Record6_TableListCount, writer.TableCount);
            Assert.AreEqual(Root_SequenceInvokationListCount, writer.SequenceInvokationList.Count);

            // Record 7 - root redirect 2
            root2_DateTimeField.Value = Record7_Root2DateTimeValue;
            root3_IntRedirField.SetValue(Record7_Root3IntRedirValue, out fieldsAffectedFromIndex);
            // redirect to level1_2 sequence
            {
                Assert.AreEqual(3, fieldsAffectedFromIndex);
                Assert.AreEqual(Record7_FieldListCount, writer.FieldList.Count);
                Assert.AreEqual(Record7_SequenceInvokationListCount, writer.SequenceInvokationList.Count);

                level1_2_DateTimeField = writer.FieldList[Level1_2_DateTimeFieldName] as FtDateTimeField;

                // leave it null
            }
            writer.Write();

            Assert.AreEqual(7, writer.RecordCount);
            Assert.AreEqual(Record7_TableListCount, writer.TableCount);
            Assert.AreEqual(Root_SequenceInvokationListCount, writer.SequenceInvokationList.Count);

            // Record 8 - root redirect 1
            root2_DateTimeField.Value = Record8_Root2DateTimeValue;
            root3_IntRedirField.SetValue(Record8_Root3IntRedirValue, out fieldsAffectedFromIndex);
            // redirect to level1_1 sequence
            {
                Assert.AreEqual(3, fieldsAffectedFromIndex);
                Assert.AreEqual(6, writer.FieldList.Count);
                Assert.AreEqual(2, writer.SequenceInvokationList.Count);

                level1_1_DecimalField = writer.FieldList[Level1_1_DecimalFieldName] as FtDecimalField;
                level1_1_FloatRedirField = writer.FieldList[Level1_1_FloatRedirFieldName] as FtFloatField;
                level1_1_StringField = writer.FieldList[Level1_1_StringFieldName] as FtStringField;

                level1_1_DecimalField.Value = Record8_Level1_1_DecimalValue;
                level1_1_FloatRedirField.SetValue(Record8_Level1_1_FloatRedirValue, out fieldsAffectedFromIndex); // redirect
                // redirect to Level2_10 sequence
                {
                    Assert.AreEqual(5, fieldsAffectedFromIndex);
                    Assert.AreEqual(Record8_FieldListCount, writer.FieldList.Count);
                    Assert.AreEqual(Record8_SequenceInvokationListCount, writer.SequenceInvokationList.Count);
                    Assert.AreEqual(-1, writer.FieldList.IndexOfName(Level1_1_StringFieldName)); // should have been discarded by Redirect

                    level2_10_StringField = writer.FieldList[Level2_10_StringFieldName] as FtStringField;
                    level2_10_StringField.Value = Record8_level2_10_StringValue;
                }
            }
            writer.Write();

            Assert.AreEqual(8, writer.RecordCount);
            Assert.AreEqual(Record8_TableListCount, writer.TableCount);
            Assert.AreEqual(Root_SequenceInvokationListCount, writer.SequenceInvokationList.Count);

            // Record 9 - root redirect 1
            root2_DateTimeField.Value = Record9_Root2DateTimeValue;
            root3_IntRedirField.SetValue(Record9_Root3IntRedirValue, out fieldsAffectedFromIndex);
            // redirect to level1_1 sequence
            {
                Assert.AreEqual(3, fieldsAffectedFromIndex);
                Assert.AreEqual(6, writer.FieldList.Count);
                Assert.AreEqual(2, writer.SequenceInvokationList.Count);

                level1_1_DecimalField = writer.FieldList[Level1_1_DecimalFieldName] as FtDecimalField;
                level1_1_FloatRedirField = writer.FieldList[Level1_1_FloatRedirFieldName] as FtFloatField;
                level1_1_StringField = writer.FieldList[Level1_1_StringFieldName] as FtStringField;

                level1_1_DecimalField.Value = Record9_Level1_1_DecimalValue;
                level1_1_FloatRedirField.SetValue(Record9_Level1_1_FloatRedirValue, out fieldsAffectedFromIndex); // redirect
                // redirect to Level2_10 sequence
                {
                    Assert.AreEqual(5, fieldsAffectedFromIndex);
                    Assert.AreEqual(Record9_FieldListCount, writer.FieldList.Count);
                    Assert.AreEqual(Record9_SequenceInvokationListCount, writer.SequenceInvokationList.Count);
                    Assert.AreEqual(-1, writer.FieldList.IndexOfName(Level1_1_StringFieldName)); // should have been discarded by Redirect

                    level2_10_StringField = writer.FieldList[Level2_10_StringFieldName] as FtStringField;
                    level2_10_StringField.Value = Record9_level2_10_StringValue;
                }
            }
            writer.Write();

            Assert.AreEqual(9, writer.RecordCount);
            Assert.AreEqual(Record9_TableListCount, writer.TableCount);
            Assert.AreEqual(Root_SequenceInvokationListCount, writer.SequenceInvokationList.Count);

            // Record 10 - root redirect 1
            root2_DateTimeField.Value = Record10_Root2DateTimeValue;
            root3_IntRedirField.SetValue(Record10_Root3IntRedirValue, out fieldsAffectedFromIndex);
            // redirect to level1_1 sequence
            {
                Assert.AreEqual(3, fieldsAffectedFromIndex);
                Assert.AreEqual(6, writer.FieldList.Count);
                Assert.AreEqual(2, writer.SequenceInvokationList.Count);

                level1_1_DecimalField = writer.FieldList[Level1_1_DecimalFieldName] as FtDecimalField;
                level1_1_FloatRedirField = writer.FieldList[Level1_1_FloatRedirFieldName] as FtFloatField;
                level1_1_StringField = writer.FieldList[Level1_1_StringFieldName] as FtStringField;

                level1_1_DecimalField.Value = Record10_Level1_1_DecimalValue;
                level1_1_StringField.Value = Record10_Level1_1_StringValue;
                level1_1_FloatRedirField.SetValue(Record10_Level1_1_FloatRedirValue, out fieldsAffectedFromIndex); // redirect
                // redirect to Level2_11 sequence
                {
                    Assert.AreEqual(6, fieldsAffectedFromIndex);
                    Assert.AreEqual(Record10_FieldListCount, writer.FieldList.Count);
                    Assert.AreEqual(Record10_SequenceInvokationListCount, writer.SequenceInvokationList.Count);
                    Assert.AreEqual(5, writer.FieldList.IndexOfName(Level1_1_StringFieldName)); // confirm not discarded
                    Assert.AreEqual(Record10_Level1_1_StringValue, level1_1_StringField.Value);

                    level2_11_StringField = writer.FieldList[Level2_11_StringFieldName] as FtStringField;
                    level2_11_StringField.Value = Record10_level2_11_StringValue;
                }
            }
            writer.Write();

            Assert.AreEqual(10, writer.RecordCount);
            Assert.AreEqual(Record10_TableListCount, writer.TableCount);
            Assert.AreEqual(Root_SequenceInvokationListCount, writer.SequenceInvokationList.Count);

            // Record 11 - no redirect
            root2_DateTimeField.Value = Record11_Root2DateTimeValue;
            root3_IntRedirField.SetValue(Record11_Root3IntRedirValue, out fieldsAffectedFromIndex);
            Assert.AreEqual(-1, fieldsAffectedFromIndex);
            Assert.AreEqual(Record11_FieldListCount, writer.FieldList.Count);
            Assert.AreEqual(Root_SequenceInvokationListCount, writer.SequenceInvokationList.Count);
            writer.Write();

            Assert.AreEqual(11, writer.RecordCount);
            Assert.AreEqual(Record11_TableListCount, writer.TableCount);
            Assert.AreEqual(Record11_SequenceInvokationListCount, writer.SequenceInvokationList.Count);

            writer.Close();

            string dataFilePath = Path.Combine(DataFolder, SequenceFileName);
            if (!TextFilesAreEqual(filePath, dataFilePath))
                Assert.Fail("Sequence does not match Test Data");
            else
            {
                FtReader reader = new FtReader(meta);
                reader.Open(filePath);

                Assert.AreEqual<bool>(false, reader.Declared);
                Assert.AreEqual<bool>(true, reader.HeaderRead);
                Assert.AreEqual<int>(1, reader.HeadingLineReadCount);
                Assert.AreEqual<FtMetaReferenceType>(FtMetaReferenceType.None, reader.MetaReferenceType);
                Assert.AreEqual<FtLineType>(FtLineType.Heading, reader.LineType); // last line in header

                root1_StrConstField = reader.FieldList[Root1_StrConstFieldName] as FtStringField;
                root2_DateTimeField = reader.FieldList[Root2_DateTimeFieldName] as FtDateTimeField;
                root3_IntRedirField = reader.FieldList[Root3_IntRedirFieldName] as FtIntegerField;

                // Record 1
                reader.Read();

                Assert.AreEqual(Record1_FieldListCount, reader.FieldList.Count);
                Assert.AreEqual(Record1_SequenceInvokationListCount, reader.SequenceInvokationList.Count);
                Assert.AreEqual(1, reader.RecordCount);
                Assert.AreEqual(Record1_TableListCount, reader.TableCount);

                Assert.AreEqual<string>(Root1StrConstValue, root1_StrConstField.Value);
                Assert.AreEqual<DateTime>(Record1_Root2DateTimeValue, root2_DateTimeField.Value);
                Assert.AreEqual<long>(Record1_Root3IntRedirValue, root3_IntRedirField.Value);

                // Record 2
                reader.Read();

                Assert.AreEqual(Record2_FieldListCount, reader.FieldList.Count);
                Assert.AreEqual(Record2_SequenceInvokationListCount, reader.SequenceInvokationList.Count);
                Assert.AreEqual(2, reader.RecordCount);
                Assert.AreEqual(Record2_TableListCount, reader.TableCount);

                Assert.AreEqual<string>(Root1StrConstValue, root1_StrConstField.Value);
                Assert.AreEqual<DateTime>(Record2_Root2DateTimeValue, root2_DateTimeField.Value);
                Assert.AreEqual<long>(Record2_Root3IntRedirValue, root3_IntRedirField.Value);

                // Record 3
                bool readResult = reader.Read();
                Assert.AreEqual<bool>(false, readResult);
                Assert.AreEqual(2, reader.RecordCount);
                Assert.AreEqual(Record2_TableListCount, reader.TableCount);

                reader.AutoNextTable = true;
                readResult = reader.Read();
                Assert.AreEqual<bool>(true, readResult);

                Assert.AreEqual(Record3_FieldListCount, reader.FieldList.Count);
                Assert.AreEqual(Record3_SequenceInvokationListCount, reader.SequenceInvokationList.Count);
                Assert.AreEqual(3, reader.RecordCount);
                Assert.AreEqual(Record3_TableListCount, reader.TableCount);

                root1_StrConstField = reader.FieldList[Root1_StrConstFieldName] as FtStringField;
                root2_DateTimeField = reader.FieldList[Root2_DateTimeFieldName] as FtDateTimeField;
                root3_IntRedirField = reader.FieldList[Root3_IntRedirFieldName] as FtIntegerField;
                level1_1_DecimalField = reader.FieldList[Level1_1_DecimalFieldName] as FtDecimalField;
                level1_1_FloatRedirField = reader.FieldList[Level1_1_FloatRedirFieldName] as FtFloatField;
                level1_1_StringField = reader.FieldList[Level1_1_StringFieldName] as FtStringField;

                Assert.AreEqual<string>(Root1StrConstValue, root1_StrConstField.Value);
                Assert.AreEqual<DateTime>(Record3_Root2DateTimeValue, root2_DateTimeField.Value);
                Assert.AreEqual<long>(Record3_Root3IntRedirValue, root3_IntRedirField.Value);
                Assert.AreEqual<decimal>(Record3_Level1_1_DecimalValue, level1_1_DecimalField.Value);
                Assert.AreEqual<double>(Record3_Level1_1_FloatRedirValue, level1_1_FloatRedirField.Value);
                Assert.AreEqual<string>(Record3_Level1_1_StringValue, level1_1_StringField.Value);

                // Record 4
                reader.Read();

                Assert.AreEqual(Record4_FieldListCount, reader.FieldList.Count);
                Assert.AreEqual(Record4_SequenceInvokationListCount, reader.SequenceInvokationList.Count);
                Assert.AreEqual(4, reader.RecordCount);
                Assert.AreEqual(Record4_TableListCount, reader.TableCount);

                Assert.AreEqual<string>(Root1StrConstValue, root1_StrConstField.Value);
                Assert.AreEqual<DateTime>(Record4_Root2DateTimeValue, root2_DateTimeField.Value);
                Assert.AreEqual<long>(Record4_Root3IntRedirValue, root3_IntRedirField.Value);
                Assert.AreEqual<decimal>(Record4_Level1_1_DecimalValue, level1_1_DecimalField.Value);
                Assert.AreEqual<double>(Record4_Level1_1_FloatRedirValue, level1_1_FloatRedirField.Value);
                Assert.AreEqual<string>(Record4_Level1_1_StringValue, level1_1_StringField.Value);

                // Record 5
                reader.Read();

                Assert.AreEqual(Record5_FieldListCount, reader.FieldList.Count);
                Assert.AreEqual(Record5_SequenceInvokationListCount, reader.SequenceInvokationList.Count);
                Assert.AreEqual(5, reader.RecordCount);
                Assert.AreEqual(Record5_TableListCount, reader.TableCount);

                Assert.AreEqual<string>(Root1StrConstValue, root1_StrConstField.Value);
                Assert.AreEqual<DateTime>(Record5_Root2DateTimeValue, root2_DateTimeField.Value);
                Assert.AreEqual<long>(Record5_Root3IntRedirValue, root3_IntRedirField.Value);
                Assert.AreEqual<decimal>(Record5_Level1_1_DecimalValue, level1_1_DecimalField.Value);
                Assert.AreEqual<double>(Record5_Level1_1_FloatRedirValue, level1_1_FloatRedirField.Value);
                Assert.AreEqual<string>(Record5_Level1_1_StringValue, level1_1_StringField.Value);

                // Record 6
                reader.Read();

                Assert.AreEqual(Record6_FieldListCount, reader.FieldList.Count);
                Assert.AreEqual(Record6_SequenceInvokationListCount, reader.SequenceInvokationList.Count);
                Assert.AreEqual(6, reader.RecordCount);
                Assert.AreEqual(Record6_TableListCount, reader.TableCount);

                root1_StrConstField = reader.FieldList[Root1_StrConstFieldName] as FtStringField;
                root2_DateTimeField = reader.FieldList[Root2_DateTimeFieldName] as FtDateTimeField;
                root3_IntRedirField = reader.FieldList[Root3_IntRedirFieldName] as FtIntegerField;
                level1_Null_BooleanField = reader.FieldList[Level1_Null_BooleanFieldName] as FtBooleanField;

                Assert.AreEqual<string>(Root1StrConstValue, root1_StrConstField.Value);
                Assert.AreEqual<DateTime>(Record6_Root2DateTimeValue, root2_DateTimeField.Value);
                Assert.AreEqual<bool>(true, root3_IntRedirField.IsNull());
                Assert.AreEqual<bool>(Record6_level1_Null_BooleanValue, level1_Null_BooleanField.Value);

                // Record 7
                reader.Read();

                Assert.AreEqual(Record7_FieldListCount, reader.FieldList.Count);
                Assert.AreEqual(Record7_SequenceInvokationListCount, reader.SequenceInvokationList.Count);
                Assert.AreEqual(7, reader.RecordCount);
                Assert.AreEqual(Record7_TableListCount, reader.TableCount);

                root1_StrConstField = reader.FieldList[Root1_StrConstFieldName] as FtStringField;
                root2_DateTimeField = reader.FieldList[Root2_DateTimeFieldName] as FtDateTimeField;
                root3_IntRedirField = reader.FieldList[Root3_IntRedirFieldName] as FtIntegerField;
                level1_2_DateTimeField = reader.FieldList[Level1_2_DateTimeFieldName] as FtDateTimeField;

                Assert.AreEqual<string>(Root1StrConstValue, root1_StrConstField.Value);
                Assert.AreEqual<DateTime>(Record7_Root2DateTimeValue, root2_DateTimeField.Value);
                Assert.AreEqual<long>(Record7_Root3IntRedirValue, root3_IntRedirField.Value);
                Assert.AreEqual<bool>(true, level1_2_DateTimeField.IsNull());

                // Record 8
                reader.Read();

                Assert.AreEqual(Record8_FieldListCount, reader.FieldList.Count);
                Assert.AreEqual(Record8_SequenceInvokationListCount, reader.SequenceInvokationList.Count);
                Assert.AreEqual(8, reader.RecordCount);
                Assert.AreEqual(Record8_TableListCount, reader.TableCount);

                root1_StrConstField = reader.FieldList[Root1_StrConstFieldName] as FtStringField;
                root2_DateTimeField = reader.FieldList[Root2_DateTimeFieldName] as FtDateTimeField;
                root3_IntRedirField = reader.FieldList[Root3_IntRedirFieldName] as FtIntegerField;
                level1_1_DecimalField = reader.FieldList[Level1_1_DecimalFieldName] as FtDecimalField;
                level1_1_FloatRedirField = reader.FieldList[Level1_1_FloatRedirFieldName] as FtFloatField;
                Assert.AreEqual<int>(-1, reader.FieldList.IndexOfName(Level1_1_StringFieldName));
                level2_10_StringField = reader.FieldList[Level2_10_StringFieldName] as FtStringField;

                Assert.AreEqual<string>(Root1StrConstValue, root1_StrConstField.Value);
                Assert.AreEqual<DateTime>(Record8_Root2DateTimeValue, root2_DateTimeField.Value);
                Assert.AreEqual<long>(Record8_Root3IntRedirValue, root3_IntRedirField.Value);
                Assert.AreEqual<decimal>(Record8_Level1_1_DecimalValue, level1_1_DecimalField.Value);
                Assert.AreEqual<double>(Record8_Level1_1_FloatRedirValue, level1_1_FloatRedirField.Value);
                Assert.AreEqual<string>(Record8_level2_10_StringValue, level2_10_StringField.Value);

                // Record 9
                reader.Read();

                Assert.AreEqual<int>(-1, reader.FieldList.IndexOfName(Level1_1_StringFieldName));

                Assert.AreEqual(Record9_FieldListCount, reader.FieldList.Count);
                Assert.AreEqual(Record9_SequenceInvokationListCount, reader.SequenceInvokationList.Count);
                Assert.AreEqual(9, reader.RecordCount);
                Assert.AreEqual(Record9_TableListCount, reader.TableCount);

                Assert.AreEqual<string>(Root1StrConstValue, root1_StrConstField.Value);
                Assert.AreEqual<DateTime>(Record9_Root2DateTimeValue, root2_DateTimeField.Value);
                Assert.AreEqual<long>(Record9_Root3IntRedirValue, root3_IntRedirField.Value);
                Assert.AreEqual<decimal>(Record9_Level1_1_DecimalValue, level1_1_DecimalField.Value);
                Assert.AreEqual<double>(Record9_Level1_1_FloatRedirValue, level1_1_FloatRedirField.Value);
                Assert.AreEqual<string>(Record9_level2_10_StringValue, level2_10_StringField.Value);

                // Record 10
                reader.Read();

                Assert.AreEqual(Record10_FieldListCount, reader.FieldList.Count);
                Assert.AreEqual(Record10_SequenceInvokationListCount, reader.SequenceInvokationList.Count);
                Assert.AreEqual(10, reader.RecordCount);
                Assert.AreEqual(Record10_TableListCount, reader.TableCount);

                root1_StrConstField = reader.FieldList[Root1_StrConstFieldName] as FtStringField;
                root2_DateTimeField = reader.FieldList[Root2_DateTimeFieldName] as FtDateTimeField;
                root3_IntRedirField = reader.FieldList[Root3_IntRedirFieldName] as FtIntegerField;
                level1_1_DecimalField = reader.FieldList[Level1_1_DecimalFieldName] as FtDecimalField;
                level1_1_FloatRedirField = reader.FieldList[Level1_1_FloatRedirFieldName] as FtFloatField;
                level1_1_StringField = reader.FieldList[Level1_1_StringFieldName] as FtStringField;
                level2_11_StringField = reader.FieldList[Level2_11_StringFieldName] as FtStringField;

                Assert.AreEqual<string>(Root1StrConstValue, root1_StrConstField.Value);
                Assert.AreEqual<DateTime>(Record10_Root2DateTimeValue, root2_DateTimeField.Value);
                Assert.AreEqual<long>(Record10_Root3IntRedirValue, root3_IntRedirField.Value);
                Assert.AreEqual<decimal>(Record10_Level1_1_DecimalValue, level1_1_DecimalField.Value);
                Assert.AreEqual<double>(Record10_Level1_1_FloatRedirValue, level1_1_FloatRedirField.Value);
                Assert.AreEqual<string>(Record10_Level1_1_StringValue, level1_1_StringField.Value);
                Assert.AreEqual<string>(Record10_level2_11_StringValue, level2_11_StringField.Value);

                // Record 11
                reader.Read();

                Assert.AreEqual(Record11_FieldListCount, reader.FieldList.Count);
                Assert.AreEqual(Record11_SequenceInvokationListCount, reader.SequenceInvokationList.Count);
                Assert.AreEqual(11, reader.RecordCount);
                Assert.AreEqual(Record11_TableListCount, reader.TableCount);

                root1_StrConstField = reader.FieldList[Root1_StrConstFieldName] as FtStringField;
                root2_DateTimeField = reader.FieldList[Root2_DateTimeFieldName] as FtDateTimeField;
                root3_IntRedirField = reader.FieldList[Root3_IntRedirFieldName] as FtIntegerField;

                Assert.AreEqual<string>(Root1StrConstValue, root1_StrConstField.Value);
                Assert.AreEqual<DateTime>(Record11_Root2DateTimeValue, root2_DateTimeField.Value);
                Assert.AreEqual<long>(Record11_Root3IntRedirValue, root3_IntRedirField.Value);

                reader.Close();
            }
        }
    }
}
