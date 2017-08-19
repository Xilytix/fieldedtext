// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System.Xml;
using System.IO;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Xilytix.FieldedText.UnitTest
{
    using FieldedText;

    [TestClass]
    public class MetaAllDefaultsTestClass : BaseTestClass
    {
        private enum State { Initialise, Assert}

        private const string TestFolderName = "MetaAllDefaults";
        private const string DataFolder = @"Data\MetaAllDefaults";
        private const string AllDefaultsFileName = "MetaAllDefaults.xml";

        const string BooleanFalseText = "False";
        const string BooleanTrueText = "True";
        const FtBooleanStyles BooleanStyles = FtBooleanStyles.IgnoreCase | FtBooleanStyles.MatchFirstCharOnly;

        const string IntegerFieldFormat = "G";
        const NumberStyles IntegerFieldStyles = NumberStyles.Integer;

        const string FloatFieldFormat = "G";
        const NumberStyles FloatFieldStyles = NumberStyles.Float;

        const string DecimalFieldFormat = "G";
        const NumberStyles DecimalFieldStyles = NumberStyles.Currency;

        const string DateTimeFieldFormat = "yyyyMMdd";
        const DateTimeStyles DateTimeFieldStyles = DateTimeStyles.None;

        const FtSubstitutionType SubstitutionType = FtSubstitutionType.String;
        const char SubstitutionToken = 'x';
        const string SubstitutionValue = "yyy";

        const string StringFieldName = "StringField";
        const string BooleanFieldName = "BooleanField";
        const string IntegerFieldName = "IntegerField";
        const string FloatFieldName = "FloatField";
        const string DecimalFieldName = "DecimalField";
        const string DateTimeFieldName = "DateTimeField";
        const string RedirectStringField1Name = "RedirectStringField1";
        const string RedirectBooleanField2Name = "RedirectBooleanField2";
        const string RedirectIntegerField3Name = "RedirectIntegerField3";
        const string RedirectFloatField4Name = "RedirectFloatField4";
        const string RedirectDecimalField5Name = "RedirectDecimalField5";
        const string RedirectDateTimeField6Name = "RedirectDateTimeField6";
        
        string filePath; // path for XML meta file
        MetaProperties defaultMetaProperties;
        MetaFieldProperties defaultMetaFieldProperties;
        State state;

        public MetaAllDefaultsTestClass(): base(TestFolderName)
        {
            filePath = Path.Combine(TestFolder, AllDefaultsFileName);
            defaultMetaProperties = new MetaProperties();
            defaultMetaProperties.LoadDefaults();
            defaultMetaFieldProperties = new MetaFieldProperties();
            defaultMetaFieldProperties.LoadDefaults();
        }

        [TestMethod]
        [DeploymentItem(DataFolder + @"\" + AllDefaultsFileName, DataFolder)]
        public void AllDefaults()
        {
            // Creates Meta with one sequence with one field of each type
            // Sets Meta and fields and sequence redirects properties to default values
            // Saves to XML (Serialize)
            // Compares XML to Expected
            // Creates Meta from XML (Deserialize)
            // Checks properties are as originally set

            state = State.Initialise;

            // Create Meta and set default properties
            FtMeta meta = new FtMeta();
            defaultMetaProperties.LoadIntoMeta(ref meta);

            FtStringMetaField stringField = meta.FieldList.New(FtStandardDataType.String) as FtStringMetaField;
            stringField.Name = StringFieldName;
            FtBooleanMetaField booleanField = meta.FieldList.New(FtStandardDataType.Boolean) as FtBooleanMetaField;
            booleanField.Name = BooleanFieldName;
            FtIntegerMetaField integerField = meta.FieldList.New(FtStandardDataType.Integer) as FtIntegerMetaField;
            integerField.Name = IntegerFieldName;
            FtFloatMetaField floatField = meta.FieldList.New(FtStandardDataType.Float) as FtFloatMetaField;
            floatField.Name = FloatFieldName;
            FtDecimalMetaField decimalField = meta.FieldList.New(FtStandardDataType.Decimal) as FtDecimalMetaField;
            decimalField.Name = DecimalFieldName;
            FtDateTimeMetaField dateTimeField = meta.FieldList.New(FtStandardDataType.DateTime) as FtDateTimeMetaField;
            dateTimeField.Name = DateTimeFieldName;

            FtStringMetaField redirectStringField1 = meta.FieldList.New(FtStandardDataType.String) as FtStringMetaField;
            redirectStringField1.Name = RedirectStringField1Name;
            FtBooleanMetaField redirectBooleanField2 = meta.FieldList.New(FtStandardDataType.Boolean) as FtBooleanMetaField;
            redirectBooleanField2.Name = RedirectBooleanField2Name;
            FtIntegerMetaField redirectIntegerField3 = meta.FieldList.New(FtStandardDataType.Integer) as FtIntegerMetaField;
            redirectIntegerField3.Name = RedirectIntegerField3Name;
            FtFloatMetaField redirectFloatField4 = meta.FieldList.New(FtStandardDataType.Float) as FtFloatMetaField;
            redirectFloatField4.Name = RedirectFloatField4Name;
            FtDecimalMetaField redirectDecimalField5 = meta.FieldList.New(FtStandardDataType.Decimal) as FtDecimalMetaField;
            redirectDecimalField5.Name = RedirectDecimalField5Name;
            FtDateTimeMetaField redirectDateTimeField6 = meta.FieldList.New(FtStandardDataType.DateTime) as FtDateTimeMetaField;
            redirectDateTimeField6.Name = RedirectDateTimeField6Name;

            // Create root sequence
            FtMetaSequence rootSequence = meta.SequenceList.New();
            rootSequence.Name = "Root";
            rootSequence.Root = true;

            // Create redirect target sequence
            FtMetaSequence redirectTargetSequence = meta.SequenceList.New();
            redirectTargetSequence.Name = "RedirectTarget";
            FtMetaSequenceItem sequenceItem;
            FtMetaSequenceRedirect sequenceRedirect;
            sequenceItem = redirectTargetSequence.ItemList.New();
            sequenceItem.Field = redirectStringField1;
            sequenceItem = redirectTargetSequence.ItemList.New();
            sequenceItem.Field = redirectBooleanField2;
            sequenceRedirect = sequenceItem.RedirectList.New(FtStandardSequenceRedirectType.Null);
            sequenceRedirect.Sequence = rootSequence;
            sequenceItem = redirectTargetSequence.ItemList.New();
            sequenceItem.Field = redirectIntegerField3;
            sequenceItem = redirectTargetSequence.ItemList.New();
            sequenceItem.Field = redirectFloatField4;
            sequenceRedirect = sequenceItem.RedirectList.New(FtStandardSequenceRedirectType.Null);
            sequenceRedirect.Sequence = rootSequence;
            sequenceItem = redirectTargetSequence.ItemList.New();
            sequenceItem.Field = redirectDecimalField5;
            sequenceItem = redirectTargetSequence.ItemList.New();
            sequenceItem.Field = redirectDateTimeField6;

            // Create one field of each type and set properties to defaults and put in sequence.  Add default redirect to sequence item redirect list
            AllDefaults_Field(stringField, rootSequence, redirectTargetSequence, FtStandardSequenceRedirectType.ExactString, 0);
            AllDefaults_Field(booleanField, rootSequence, redirectTargetSequence, FtStandardSequenceRedirectType.Boolean, 1);
            booleanField.FalseText = BooleanFalseText;
            booleanField.TrueText = BooleanTrueText;
            booleanField.Styles = BooleanStyles;
            AllDefaults_Field(integerField, rootSequence, redirectTargetSequence, FtStandardSequenceRedirectType.ExactInteger, 2);
            integerField.Format = IntegerFieldFormat;
            integerField.Styles = IntegerFieldStyles;
            AllDefaults_Field(floatField, rootSequence, redirectTargetSequence, FtStandardSequenceRedirectType.ExactFloat, 3);
            floatField.Format = FloatFieldFormat;
            floatField.Styles = FloatFieldStyles;
            AllDefaults_Field(decimalField, rootSequence, redirectTargetSequence, FtStandardSequenceRedirectType.ExactDecimal, 4);
            decimalField.Format = DecimalFieldFormat;
            decimalField.Styles = DecimalFieldStyles;
            AllDefaults_Field(dateTimeField, rootSequence, redirectTargetSequence, FtStandardSequenceRedirectType.ExactDateTime, 5);
            dateTimeField.Format = DateTimeFieldFormat;
            dateTimeField.Styles = DateTimeFieldStyles;

            // Add one substitution of default substitution type
            FtMetaSubstitution substitution = meta.SubstitutionList.New();
            substitution.Type = SubstitutionType;
            substitution.Token = SubstitutionToken;
            substitution.Value = SubstitutionValue;

            // Serialize Meta
            XmlWriter writer = XmlWriter.Create(filePath, xmlWriterSettings);
            FtMetaSerializer.Serialize(meta, writer);
            writer.Close();

            state = State.Assert;

            string DataFilePath = Path.Combine(DataFolder, AllDefaultsFileName);
            if (!TextFilesAreEqual(filePath, DataFilePath))
                Assert.Fail("AllDefaults does not match Test Data");
            else
            {
                FtMeta deserialisedMeta = FtMetaSerializer.Deserialize(filePath);

                defaultMetaProperties.AssertMetaAreEqual(meta);

                rootSequence = meta.SequenceList[0];
                Assert.AreEqual<bool>(true, rootSequence.Root);
                Assert.AreEqual<int>(6, rootSequence.ItemList.Count);
                redirectTargetSequence = meta.SequenceList[1];
                Assert.AreEqual<bool>(false, redirectTargetSequence.Root);
                Assert.AreEqual<int>(6, redirectTargetSequence.ItemList.Count);

                stringField = meta.FieldList[0] as FtStringMetaField;
                Assert.AreEqual<string>(StringFieldName, stringField.Name);
                AllDefaults_Field(stringField, rootSequence, redirectTargetSequence, FtStandardSequenceRedirectType.ExactString, 0);
                booleanField = meta.FieldList[1] as FtBooleanMetaField;
                Assert.AreEqual<string>(BooleanFieldName, booleanField.Name);
                AllDefaults_Field(booleanField, rootSequence, redirectTargetSequence, FtStandardSequenceRedirectType.Boolean, 1);
                Assert.AreEqual<string>(BooleanFalseText, booleanField.FalseText);
                Assert.AreEqual<string>(BooleanTrueText, booleanField.TrueText);
                Assert.AreEqual<FtBooleanStyles>(BooleanStyles, booleanField.Styles);
                integerField = meta.FieldList[2] as FtIntegerMetaField;
                Assert.AreEqual<string>(IntegerFieldName, integerField.Name);
                AllDefaults_Field(integerField, rootSequence, redirectTargetSequence, FtStandardSequenceRedirectType.ExactInteger, 2);
                Assert.AreEqual<string>(IntegerFieldFormat, integerField.Format);
                Assert.AreEqual<NumberStyles>(IntegerFieldStyles, integerField.Styles);
                floatField = meta.FieldList[3] as FtFloatMetaField;
                Assert.AreEqual<string>(FloatFieldName, floatField.Name);
                AllDefaults_Field(floatField, rootSequence, redirectTargetSequence, FtStandardSequenceRedirectType.ExactFloat, 3);
                Assert.AreEqual<string>(FloatFieldFormat, floatField.Format);
                Assert.AreEqual<NumberStyles>(FloatFieldStyles, floatField.Styles);
                decimalField = meta.FieldList[4] as FtDecimalMetaField;
                Assert.AreEqual<string>(DecimalFieldName, decimalField.Name);
                AllDefaults_Field(decimalField, rootSequence, redirectTargetSequence, FtStandardSequenceRedirectType.ExactDecimal, 4);
                Assert.AreEqual<string>(DecimalFieldFormat, decimalField.Format);
                Assert.AreEqual<NumberStyles>(DecimalFieldStyles, decimalField.Styles);
                dateTimeField = meta.FieldList[5] as FtDateTimeMetaField;
                Assert.AreEqual<string>(DateTimeFieldName, dateTimeField.Name);
                AllDefaults_Field(dateTimeField, rootSequence, redirectTargetSequence, FtStandardSequenceRedirectType.ExactDateTime, 5);
                Assert.AreEqual<string>(DateTimeFieldFormat, dateTimeField.Format);
                Assert.AreEqual<DateTimeStyles>(DateTimeFieldStyles, dateTimeField.Styles);

                redirectStringField1 = meta.FieldList[6] as FtStringMetaField;
                Assert.AreEqual<string>(RedirectStringField1Name, redirectStringField1.Name);
                redirectBooleanField2 = meta.FieldList[7] as FtBooleanMetaField;
                Assert.AreEqual<string>(RedirectBooleanField2Name, redirectBooleanField2.Name);
                Assert.AreEqual<string>(BooleanFalseText, redirectBooleanField2.FalseText);
                Assert.AreEqual<string>(BooleanTrueText, redirectBooleanField2.TrueText);
                Assert.AreEqual<FtBooleanStyles>(BooleanStyles, redirectBooleanField2.Styles);
                redirectIntegerField3 = meta.FieldList[8] as FtIntegerMetaField;
                Assert.AreEqual<string>(RedirectIntegerField3Name, redirectIntegerField3.Name);
                Assert.AreEqual<string>(IntegerFieldFormat, redirectIntegerField3.Format);
                Assert.AreEqual<NumberStyles>(IntegerFieldStyles, redirectIntegerField3.Styles);
                redirectFloatField4 = meta.FieldList[9] as FtFloatMetaField;
                Assert.AreEqual<string>(RedirectFloatField4Name, redirectFloatField4.Name);
                Assert.AreEqual<string>(FloatFieldFormat, redirectFloatField4.Format);
                Assert.AreEqual<NumberStyles>(FloatFieldStyles, redirectFloatField4.Styles);
                redirectDecimalField5 = meta.FieldList[10] as FtDecimalMetaField;
                Assert.AreEqual<string>(RedirectDecimalField5Name, redirectDecimalField5.Name);
                Assert.AreEqual<string>(DecimalFieldFormat, redirectDecimalField5.Format);
                Assert.AreEqual<NumberStyles>(DecimalFieldStyles, redirectDecimalField5.Styles);
                redirectDateTimeField6 = meta.FieldList[11] as FtDateTimeMetaField;
                Assert.AreEqual<string>(RedirectDateTimeField6Name, redirectDateTimeField6.Name);
                Assert.AreEqual<string>(DateTimeFieldFormat, redirectDateTimeField6.Format);
                Assert.AreEqual<DateTimeStyles>(DateTimeFieldStyles, redirectDateTimeField6.Styles);

                Assert.AreEqual<FtMetaField>(redirectStringField1, redirectTargetSequence.ItemList[0].Field);
                Assert.AreEqual<int>(0, redirectTargetSequence.ItemList[0].RedirectList.Count);
                Assert.AreEqual<FtMetaField>(redirectBooleanField2, redirectTargetSequence.ItemList[1].Field);
                Assert.AreEqual<int>(1, redirectTargetSequence.ItemList[1].RedirectList.Count);
                Assert.AreEqual<int>(FtStandardSequenceRedirectType.Null, redirectTargetSequence.ItemList[1].RedirectList[0].Type);
                Assert.AreEqual<FtMetaField>(redirectIntegerField3, redirectTargetSequence.ItemList[2].Field);
                Assert.AreEqual<int>(0, redirectTargetSequence.ItemList[2].RedirectList.Count);
                Assert.AreEqual<FtMetaField>(redirectFloatField4, redirectTargetSequence.ItemList[3].Field);
                Assert.AreEqual<int>(1, redirectTargetSequence.ItemList[3].RedirectList.Count);
                Assert.AreEqual<int>(FtStandardSequenceRedirectType.Null, redirectTargetSequence.ItemList[3].RedirectList[0].Type);
                Assert.AreEqual<FtMetaField>(redirectDecimalField5, redirectTargetSequence.ItemList[4].Field);
                Assert.AreEqual<int>(0, redirectTargetSequence.ItemList[4].RedirectList.Count);
                Assert.AreEqual<FtMetaField>(redirectDateTimeField6, redirectTargetSequence.ItemList[5].Field);
                Assert.AreEqual<int>(0, redirectTargetSequence.ItemList[5].RedirectList.Count);
            }
        }

        private void AllDefaults_Field(FtMetaField field, FtMetaSequence sequence, FtMetaSequence targetSequence, int redirectType, int itemIdx)
        {
            const FtSequenceInvokationDelay SequenceInvokationDelay = FtSequenceInvokationDelay.ftikAfterField;

            if (state == State.Initialise)
            {
                defaultMetaFieldProperties.LoadIntoMetaField(ref field, false);

                FtMetaSequenceItem sequenceItem = sequence.ItemList.New();
                sequenceItem.Field = field;
                FtMetaSequenceRedirect sequenceRedirect = sequenceItem.RedirectList.New(redirectType);
                sequenceRedirect.InvokationDelay = SequenceInvokationDelay;
                sequenceRedirect.Sequence = targetSequence;
            }
            else
            {
                defaultMetaFieldProperties.AssertMetaFieldAreEqual(field, false);

                FtMetaSequenceItem sequenceItem = sequence.ItemList[itemIdx];
                Assert.AreEqual<FtMetaField>(field, sequenceItem.Field);
                FtMetaSequenceRedirect sequenceRedirect = sequenceItem.RedirectList[0];
                Assert.AreEqual<FtSequenceInvokationDelay>(SequenceInvokationDelay, sequenceRedirect.InvokationDelay);
                Assert.AreEqual<FtMetaSequence>(targetSequence, sequenceRedirect.Sequence);
            }
        }
    }
}
