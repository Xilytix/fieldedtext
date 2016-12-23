// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;
using System.Xml;
using System.IO;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Xilytix.FieldedText.UnitTest
{
    [TestClass]
    public class MetaVariationsTestClass : BaseTestClass
    {
        private const string TestFolderName = "MetaVariations";
        private const string DataFolder = @"Data\MetaVariations";

        private const string Variation1FileName = "Variation1.xml";
        private const string Variation2FileName = "Variation2.xml";
        private const string Variation3FileName = "Variation3.xml";
        private const string Variation4FileName = "Variation4.xml";

        private struct MetaVariation
        {
            public string FileName;
            public MetaProperties Properties;
        }

        private MetaVariation[] metaVariationArray =
        {
            new MetaVariation()
            {
                FileName = Variation1FileName,
                Properties = new MetaProperties
                {
                    AllowEndOfLineCharInQuotes = false,
                    AllowIncompleteRecords = false,
                    CultureName = "au-en",
                    DelimiterChar = '\x06',
                    EndOfLineAutoWriteType = FtEndOfLineAutoWriteType.Cr,
                    EndOfLineChar = '\x0b',
                    EndOfLineType = FtEndOfLineType.Char,
                    HeadingAlwaysWriteOptionalQuote = false,
                    HeadingConstraint = FtHeadingConstraint.AllConstant,
                    HeadingEndOfValueChar = '<',
                    HeadingLineCount = 2,
                    HeadingPadAlignment = FtPadAlignment.Left,
                    HeadingPadChar = '>',
                    HeadingPadCharType = FtPadCharType.EndOfValue,
                    HeadingQuotedType = FtQuotedType.Never,
                    HeadingTruncateChar = '\x05',
                    HeadingTruncateType = FtTruncateType.NullChar,
                    HeadingWritePrefixSpace = true,
                    IgnoreBlankLines = false,
                    IgnoreExtraChars = false,
                    LastLineEndedType = FtLastLineEndedType.Never,
                    LineCommentChar = '%',
                    MainHeadingLineIndex = 1,
                    QuoteChar = '"',
                    StuffedEmbeddedQuotes = false,
                    SubstitutionChar = '[',
                    SubstitutionsEnabled = true,
                }
            },
            new MetaVariation()
            {
                FileName = Variation2FileName,
                Properties = new MetaProperties
                {
                    AllowEndOfLineCharInQuotes = true,
                    AllowIncompleteRecords = false,
                    CultureName = "de-DE",
                    DelimiterChar = ',',
                    EndOfLineAutoWriteType = FtEndOfLineAutoWriteType.CrLf,
                    EndOfLineChar = '\x08',
                    EndOfLineType = FtEndOfLineType.CrLf,
                    HeadingAlwaysWriteOptionalQuote = true,
                    HeadingConstraint = FtHeadingConstraint.MainConstant,
                    HeadingEndOfValueChar = '\x02',
                    HeadingLineCount = 4,
                    HeadingPadAlignment = FtPadAlignment.Right,
                    HeadingPadChar = ' ',
                    HeadingPadCharType = FtPadCharType.Specified,
                    HeadingQuotedType = FtQuotedType.Always,
                    HeadingTruncateChar = '\x0e',
                    HeadingTruncateType = FtTruncateType.Right,
                    HeadingWritePrefixSpace = true,
                    IgnoreBlankLines = false,
                    IgnoreExtraChars = true,
                    LastLineEndedType = FtLastLineEndedType.Always,
                    LineCommentChar = '\x01',
                    MainHeadingLineIndex = 1,
                    QuoteChar = '\'',
                    StuffedEmbeddedQuotes = true,
                    SubstitutionChar = '\\',
                    SubstitutionsEnabled = false,
                }
            },
            new MetaVariation()
            {
                FileName = Variation3FileName,
                Properties = new MetaProperties
                {
                    AllowEndOfLineCharInQuotes = true,
                    AllowIncompleteRecords = true,
                    CultureName = "zh-CN",
                    DelimiterChar = ';',
                    EndOfLineAutoWriteType = FtEndOfLineAutoWriteType.Lf,
                    EndOfLineChar = '\x0d',
                    EndOfLineType = FtEndOfLineType.Auto,
                    HeadingAlwaysWriteOptionalQuote = false,
                    HeadingConstraint = FtHeadingConstraint.NameConstant,
                    HeadingEndOfValueChar = '\x07',
                    HeadingLineCount = 0,
                    HeadingPadAlignment = FtPadAlignment.Auto,
                    HeadingPadChar = '0',
                    HeadingPadCharType = FtPadCharType.EndOfValue,
                    HeadingQuotedType = FtQuotedType.Optional,
                    HeadingTruncateChar = '-',
                    HeadingTruncateType = FtTruncateType.TruncateChar,
                    HeadingWritePrefixSpace = false,
                    IgnoreBlankLines = true,
                    IgnoreExtraChars = false,
                    LastLineEndedType = FtLastLineEndedType.Optional,
                    LineCommentChar = '$',
                    MainHeadingLineIndex = 0,
                    QuoteChar = '\'',
                    StuffedEmbeddedQuotes = false,
                    SubstitutionChar = '[',
                    SubstitutionsEnabled = false,
                }
            },
            new MetaVariation()
            {
                FileName = Variation4FileName,
                Properties = new MetaProperties
                {
                    AllowEndOfLineCharInQuotes = false,
                    AllowIncompleteRecords = true,
                    CultureName = "ar-MA",
                    DelimiterChar = '\x09',
                    EndOfLineAutoWriteType = FtEndOfLineAutoWriteType.Local,
                    EndOfLineChar = '_',
                    EndOfLineType = FtEndOfLineType.Char,
                    HeadingAlwaysWriteOptionalQuote = true,
                    HeadingConstraint = FtHeadingConstraint.NameIsMain,
                    HeadingEndOfValueChar = '}',
                    HeadingLineCount = 1,
                    HeadingPadAlignment = FtPadAlignment.Left,
                    HeadingPadChar = '\xff',
                    HeadingPadCharType = FtPadCharType.Specified,
                    HeadingQuotedType = FtQuotedType.Never,
                    HeadingTruncateChar = '?',
                    HeadingTruncateType = FtTruncateType.Exception,
                    HeadingWritePrefixSpace = true,
                    IgnoreBlankLines = true,
                    IgnoreExtraChars = true,
                    LastLineEndedType = FtLastLineEndedType.Never,
                    LineCommentChar = '%',
                    MainHeadingLineIndex = 0,
                    QuoteChar = '(',
                    StuffedEmbeddedQuotes = true,
                    SubstitutionChar = '\\',
                    SubstitutionsEnabled = true,
                }
            },
        };

        private struct MetaFieldDefinition
        {
            public int DataType;
            public MetaFieldProperties Properties;
        }

        private MetaFieldDefinition[] metaFieldDefinitionArray =
        {
            new MetaFieldDefinition()
            {
                DataType = FtStandardDataType.Boolean,
                Properties =  new MetaFieldProperties()
                {
                    Constant = false,
                    FixedWidth = true,
                    HeadingAlwaysWriteOptionalQuote = false,
                    HeadingConstraint = FtHeadingConstraint.AllConstant,
                    HeadingEndOfValueChar = '*',
                    HeadingPadAlignment = FtPadAlignment.Left,
                    HeadingPadChar = '$',
                    HeadingPadCharType = FtPadCharType.Specified,
                    HeadingQuotedType = FtQuotedType.Never,
                    Headings = new string[4] {"abc", "123", "555555", ">"},
                    HeadingTruncateChar = ':',
                    HeadingTruncateType = FtTruncateType.NullChar,
                    HeadingWritePrefixSpace = false,
                    Id = 1,
                    Name = "VarBooleanField",
                    Null = false,
                    ValueAlwaysWriteOptionalQuote = false,
                    ValueEndOfValueChar = ')',
                    ValueNullChar = '\x2',
                    ValuePadAlignment = FtPadAlignment.Right,
                    ValuePadChar = '[',
                    ValuePadCharType = FtPadCharType.Auto,
                    ValueQuotedType = FtQuotedType.Always,
                    ValueTruncateChar = '~',
                    ValueTruncateType = FtTruncateType.Right,
                    ValueWritePrefixSpace = true,
                    Width = 50,
                }
            },
            new MetaFieldDefinition()
            {
                DataType = FtStandardDataType.String,
                Properties =  new MetaFieldProperties()
                {
                    Constant = true,
                    FixedWidth = false,
                    HeadingAlwaysWriteOptionalQuote = false,
                    HeadingConstraint = FtHeadingConstraint.AllConstant,
                    HeadingEndOfValueChar = '*',
                    HeadingPadAlignment = FtPadAlignment.Left,
                    HeadingPadChar = '$',
                    HeadingPadCharType = FtPadCharType.Specified,
                    HeadingQuotedType = FtQuotedType.Never,
                    Headings = new string[4] {"abc", "123", "555555", ">"},
                    HeadingTruncateChar = ':',
                    HeadingTruncateType = FtTruncateType.NullChar,
                    HeadingWritePrefixSpace = false,
                    Id = 99,
                    Name = "VarStringField",
                    Null = false,
                    ValueAlwaysWriteOptionalQuote = false,
                    ValueEndOfValueChar = ')',
                    ValueNullChar = 'x',
                    ValuePadAlignment = FtPadAlignment.Right,
                    ValuePadChar = '[',
                    ValuePadCharType = FtPadCharType.Auto,
                    ValueQuotedType = FtQuotedType.Always,
                    ValueTruncateChar = '~',
                    ValueTruncateType = FtTruncateType.Right,
                    ValueWritePrefixSpace = false,
                    Width = 50,
                }
            },
            new MetaFieldDefinition()
            {
                DataType = FtStandardDataType.Float,
                Properties =  new MetaFieldProperties()
                {
                    Constant = true,
                    FixedWidth = true,
                    HeadingAlwaysWriteOptionalQuote = false,
                    HeadingConstraint = FtHeadingConstraint.AllConstant,
                    HeadingEndOfValueChar = '*',
                    HeadingPadAlignment = FtPadAlignment.Left,
                    HeadingPadChar = '$',
                    HeadingPadCharType = FtPadCharType.Specified,
                    HeadingQuotedType = FtQuotedType.Never,
                    Headings = new string[4] {"abc", "123", "555555", ">"},
                    HeadingTruncateChar = ':',
                    HeadingTruncateType = FtTruncateType.NullChar,
                    HeadingWritePrefixSpace = false,
                    Id = 200,
                    Name = "VarFloatField",
                    Null = true,
                    ValueAlwaysWriteOptionalQuote = false,
                    ValueEndOfValueChar = ')',
                    ValueNullChar = '#',
                    ValuePadAlignment = FtPadAlignment.Right,
                    ValuePadChar = '[',
                    ValuePadCharType = FtPadCharType.Auto,
                    ValueQuotedType = FtQuotedType.Always,
                    ValueTruncateChar = '~',
                    ValueTruncateType = FtTruncateType.Right,
                    ValueWritePrefixSpace = false,
                    Width = 50,
                }
            },
            new MetaFieldDefinition()
            {
                DataType = FtStandardDataType.DateTime,
                Properties =  new MetaFieldProperties()
                {
                    Constant = true,
                    FixedWidth = true,
                    HeadingAlwaysWriteOptionalQuote = false,
                    HeadingConstraint = FtHeadingConstraint.AllConstant,
                    HeadingEndOfValueChar = '*',
                    HeadingPadAlignment = FtPadAlignment.Left,
                    HeadingPadChar = '$',
                    HeadingPadCharType = FtPadCharType.Specified,
                    HeadingQuotedType = FtQuotedType.Never,
                    Headings = new string[4] {"abc", "123", "555555", ">"},
                    HeadingTruncateChar = ':',
                    HeadingTruncateType = FtTruncateType.NullChar,
                    HeadingWritePrefixSpace = false,
                    Id = 33,
                    Name = "VarDateTimeField",
                    Null = false,
                    ValueAlwaysWriteOptionalQuote = false,
                    ValueEndOfValueChar = ')',
                    ValueNullChar = '*',
                    ValuePadAlignment = FtPadAlignment.Right,
                    ValuePadChar = '[',
                    ValuePadCharType = FtPadCharType.Auto,
                    ValueQuotedType = FtQuotedType.Always,
                    ValueTruncateChar = '~',
                    ValueTruncateType = FtTruncateType.Right,
                    ValueWritePrefixSpace = true,
                    Width = 50,
                }
            }
        };

        private const FtBooleanStyles BooleanMetaFieldStyles = FtBooleanStyles.FalseIfNotMatchTrue | FtBooleanStyles.IgnoreCase;
        private const string BooleanMetaFieldFalseText = "Ff";
        private const string BooleanMetaFieldTrueText = "Tt";
        private const string StringMetaFieldValue = "StringTestValue";
        private const string FloatMetaFieldFormat = "F4";
        private const NumberStyles FloatMetaFieldStyles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign;
        private const string DateTimeMetaFieldFormat = "yyyyMMddHHmmss";
        private const DateTimeStyles DateTimeMetaFieldStyles = DateTimeStyles.AllowWhiteSpaces;
        private readonly DateTime DateTimeMetaFieldValue = new DateTime(2012,5,3,15,42,5,875);

        public MetaVariationsTestClass() : base(TestFolderName)
        {
            xmlWriterSettings.NewLineOnAttributes = true;
        }

        [TestMethod]
        [DeploymentItem(DataFolder + @"\" + Variation1FileName, DataFolder)]
        [DeploymentItem(DataFolder + @"\" + Variation2FileName, DataFolder)]
        [DeploymentItem(DataFolder + @"\" + Variation3FileName, DataFolder)]
        [DeploymentItem(DataFolder + @"\" + Variation4FileName, DataFolder)]
        public void MetaVariations()
        {
            foreach (MetaVariation variation in metaVariationArray)
            {
                FtMeta meta = new FtMeta();
                variation.Properties.LoadIntoMeta(ref meta);

                foreach (MetaFieldDefinition metaFieldDefinition in metaFieldDefinitionArray)
                {
                    FtMetaField metaField = meta.FieldList.New(metaFieldDefinition.DataType);
                    // metaField.Name = "Field" + idx++.ToString();
                    metaFieldDefinition.Properties.LoadIntoMetaField(ref metaField, true);

                    switch (metaFieldDefinition.DataType)
                    {
                        case FtStandardDataType.Boolean:
                            FtBooleanMetaField booleanMetaField = metaField as FtBooleanMetaField;
                            booleanMetaField.Styles = BooleanMetaFieldStyles;
                            booleanMetaField.FalseText = BooleanMetaFieldFalseText;
                            booleanMetaField.TrueText = BooleanMetaFieldTrueText;
                            break;
                        case FtStandardDataType.String:
                            FtStringMetaField stringMetaField = metaField as FtStringMetaField;
                            stringMetaField.Value = StringMetaFieldValue;
                            break;
                        case FtStandardDataType.Float:
                            FtFloatMetaField floatMetaField = metaField as FtFloatMetaField;
                            floatMetaField.Format = FloatMetaFieldFormat;
                            floatMetaField.Styles = FloatMetaFieldStyles;
                            break;
                        case FtStandardDataType.DateTime:
                            FtDateTimeMetaField dateTimeMetaField = metaField as FtDateTimeMetaField;
                            dateTimeMetaField.Format = DateTimeMetaFieldFormat;
                            dateTimeMetaField.Styles = DateTimeMetaFieldStyles;
                            dateTimeMetaField.Value = DateTimeMetaFieldValue;
                            break;
                    }
                }

                string filePath = Path.Combine(TestFolder, variation.FileName);
                XmlWriter writer = XmlWriter.Create(filePath, xmlWriterSettings);
                FtMetaSerializer.Serialize(meta, writer);
                writer.Close();

                string DataFilePath = Path.Combine(DataFolder, variation.FileName);
                if (!TextFilesAreEqual(filePath, DataFilePath))
                    Assert.Fail(variation.FileName + " does not match Test Data");
                else
                {
                    FtMeta deserialisedMeta = FtMetaSerializer.Deserialize(filePath);

                    variation.Properties.AssertMetaAreEqual(meta);

                    Assert.AreEqual<int>(metaFieldDefinitionArray.Length, deserialisedMeta.FieldList.Count);

                    for (int i = 0; i < deserialisedMeta.FieldList.Count; i++)
                    {
                        FtMetaField metaField = deserialisedMeta.FieldList[i];
                        metaFieldDefinitionArray[i].Properties.AssertMetaFieldAreEqual(metaField, true);

                        switch (metaFieldDefinitionArray[i].DataType)
                        {
                            case FtStandardDataType.Boolean:
                                FtBooleanMetaField booleanMetaField = metaField as FtBooleanMetaField;
                                Assert.AreEqual<FtBooleanStyles>(BooleanMetaFieldStyles, booleanMetaField.Styles);
                                Assert.AreEqual<string>(BooleanMetaFieldFalseText, booleanMetaField.FalseText);
                                Assert.AreEqual<string>(BooleanMetaFieldTrueText, booleanMetaField.TrueText);
                                break;
                            case FtStandardDataType.String:
                                FtStringMetaField stringMetaField = metaField as FtStringMetaField;
                                Assert.AreEqual<string>(StringMetaFieldValue, stringMetaField.Value);
                                break;
                            case FtStandardDataType.Float:
                                FtFloatMetaField floatMetaField = metaField as FtFloatMetaField;
                                Assert.AreEqual<string>(FloatMetaFieldFormat, floatMetaField.Format);
                                Assert.AreEqual<NumberStyles>(FloatMetaFieldStyles, floatMetaField.Styles);
                                break;
                            case FtStandardDataType.DateTime:
                                FtDateTimeMetaField dateTimeMetaField = metaField as FtDateTimeMetaField;
                                Assert.AreEqual<string>(DateTimeMetaFieldFormat, dateTimeMetaField.Format);
                                Assert.AreEqual<DateTimeStyles>(DateTimeMetaFieldStyles, dateTimeMetaField.Styles);
                                Assert.AreEqual<DateTime>(DateTimeMetaFieldValue, dateTimeMetaField.Value);
                                break;
                        }
                    }
                }
            }
        }
    }
}
