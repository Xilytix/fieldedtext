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
    public class WebSiteBasicExampleTestClass : BaseTestClass
    {
        private const string TestFolderName = "WebSiteBasicExample";
        private const string DataFolder = @"Data\WebSiteBasicExample";

        private const string BasicExampleFileName = "BasicExample.csv";

        public WebSiteBasicExampleTestClass() : base(TestFolderName) { }

        private struct Heading
        {
            public string PetName;
            public string Age;
            public string Color;
            public string DateReceived;
            public string Price;
            public string NeedsWalking;
            public string Type;
        }

        private Heading[] headingArray =
        {
            new Heading()
            {
                PetName = "Pet Name",
                Age = "Age",
                Color = "Color",
                DateReceived = "Date Received",
                Price = "Price",
                NeedsWalking = "Needs Walking",
                Type = "Type",
            },
            new Heading()
            {
                PetName = "",
                Age = "(Years)",
                Color = "",
                DateReceived = "",
                Price = "(Dollars)",
                NeedsWalking = "",
                Type = "",
            },
        };

        private struct Rec
        {
            public string PetName;
            public double? Age;
            public string Color;
            public DateTime? DateReceived;
            public decimal? Price;
            public bool? NeedsWalking;
            public string Type;
        }

        private Rec[] recArray =
        {
            new Rec()
            {
                PetName = "Rover",
                Age = 4.5,
                Color = "Brown",
                DateReceived = new DateTime(2004, 2, 12),
                Price = 80,
                NeedsWalking = true,
                Type = "Dog",
            },
            new Rec()
            {
                PetName = "Charlie",
                // Age = , // null
                Color = "Gold",
                DateReceived = new DateTime(2007, 4, 5),
                Price = 12.3M,
                NeedsWalking = false,
                Type = "Fish",
            },
            new Rec()
            {
                PetName = "Molly",
                Age = 2,
                Color = "Black",
                DateReceived = new DateTime(2006, 12, 25),
                Price = 25,
                NeedsWalking = false,
                Type = "Cat",
            },
            new Rec()
            {
                PetName = "Gilly",
                // Age = , // null
                Color = "White",
                DateReceived = new DateTime(2007, 4, 10),
                Price = 10,
                NeedsWalking = false,
                Type = "Guinea Pig",
            },
        };

        [TestMethod]
        [DeploymentItem(DataFolder + @"\" + BasicExampleFileName, DataFolder)]
        public void WebSiteBasicExample()
        {
            FtMeta meta = new FtMeta();
            meta.LineCommentChar = '~';
            meta.HeadingLineCount = 2;

            FtStringMetaField petNameMetaField = meta.FieldList.New(FtStandardDataType.String) as FtStringMetaField;
            petNameMetaField.Name = "PetName";
            FtFloatMetaField ageMetaField = meta.FieldList.New(FtStandardDataType.Float) as FtFloatMetaField;
            ageMetaField.Name = "Age";
            FtStringMetaField colorMetaField = meta.FieldList.New(FtStandardDataType.String) as FtStringMetaField;
            colorMetaField.Name = "Color";
            FtDateTimeMetaField dateReceivedMetaField = meta.FieldList.New(FtStandardDataType.DateTime) as FtDateTimeMetaField;
            dateReceivedMetaField.Name = "DateReceived";
            dateReceivedMetaField.Format = "d MMM yyyy";
            FtDecimalMetaField priceMetaField = meta.FieldList.New(FtStandardDataType.Decimal) as FtDecimalMetaField;
            priceMetaField.Name = "Price";
            FtBooleanMetaField needsWalkingMetaField = meta.FieldList.New(FtStandardDataType.Boolean) as FtBooleanMetaField;
            needsWalkingMetaField.Name = "NeedsWalking";
            FtStringMetaField typeMetaField = meta.FieldList.New(FtStandardDataType.String) as FtStringMetaField;
            typeMetaField.Name = "Type";

            string filePath = Path.Combine(TestFolder, BasicExampleFileName);
            FtWriterSettings writerSettings = new FtWriterSettings();
            writerSettings.Declared = true;
            writerSettings.MetaReferenceType = FtMetaReferenceType.Embedded;
            writerSettings.EmbeddedMetaIndent = true;
            writerSettings.EmbeddedMetaIndentChars = "  ";
            using (StreamWriter strmWriter = new StreamWriter(filePath, false, System.Text.Encoding.UTF8))
            {
                using (FtWriter writer = new FtWriter(meta, strmWriter, writerSettings))
                {
                    FtStringField petNameField = writer.FieldList.Get(petNameMetaField.Name) as FtStringField;
                    int petNameFieldIndex = writer.GetOrdinal(petNameMetaField.Name);
                    FtFloatField ageField = writer.FieldList.Get(ageMetaField.Name) as FtFloatField;
                    int ageFieldIndex = writer.GetOrdinal(ageMetaField.Name);
                    FtStringField colorField = writer.FieldList.Get(colorMetaField.Name) as FtStringField;
                    int colorFieldIndex = writer.GetOrdinal(colorMetaField.Name);
                    FtDateTimeField dateReceivedField = writer.FieldList.Get(dateReceivedMetaField.Name) as FtDateTimeField;
                    int dateReceivedFieldIndex = writer.GetOrdinal(dateReceivedMetaField.Name);
                    FtDecimalField priceField = writer.FieldList.Get(priceMetaField.Name) as FtDecimalField;
                    int priceFieldIndex = writer.GetOrdinal(priceMetaField.Name);
                    FtBooleanField needsWalkingField = writer.FieldList.Get(needsWalkingMetaField.Name) as FtBooleanField;
                    int needsWalkingFieldIndex = writer.GetOrdinal(needsWalkingMetaField.Name);
                    FtStringField typeField = writer.FieldList.Get(typeMetaField.Name) as FtStringField;
                    int typeFieldIndex = writer.GetOrdinal(typeMetaField.Name);

                    for (int i = 0; i < headingArray.Length; i++)
                    {
                        petNameField.Headings[i] = headingArray[i].PetName;
                        ageField.Headings[i] = headingArray[i].Age;
                        colorField.Headings[i] = headingArray[i].Color;
                        dateReceivedField.Headings[i] = headingArray[i].DateReceived;
                        priceField.Headings[i] = headingArray[i].Price;
                        needsWalkingField.Headings[i] = headingArray[i].NeedsWalking;
                        typeField.Headings[i] = headingArray[i].Type;
                    }

                    for (int i = 0; i < 2; i++)
                    {
                        if (recArray[i].PetName != null)
                        {
                            petNameField.Value = recArray[i].PetName;
                        }
                        if (recArray[i].Age.HasValue)
                        {
                            ageField.Value = recArray[i].Age.Value;
                        }
                        if (recArray[i].Color != null)
                        {
                            colorField.Value = recArray[i].Color;
                        }
                        if (recArray[i].DateReceived.HasValue)
                        {
                            dateReceivedField.Value = recArray[i].DateReceived.Value;
                        }
                        if (recArray[i].Price.HasValue)
                        {
                            priceField.Value = recArray[i].Price.Value;
                        }
                        if (recArray[i].NeedsWalking.HasValue)
                        {
                            needsWalkingField.Value = recArray[i].NeedsWalking.Value;
                        }
                        if (recArray[i].Type != null)
                        {
                            typeField.Value = recArray[i].Type;
                        }

                        writer.Write();
                    }

                    for (int i = 2; i < 3; i++)
                    {
                        if (recArray[i].PetName == null)
                            writer.SetNull(i);
                        else
                            writer.SetString(petNameFieldIndex, recArray[i].PetName);
                        if (!recArray[i].Age.HasValue)
                            writer.SetNull(i);
                        else
                            writer.SetDouble(ageFieldIndex, recArray[i].Age.Value);
                        if (recArray[i].Color == null)
                            writer.SetNull(i);
                        else
                            writer.SetString(colorFieldIndex, recArray[i].Color);
                        if (!recArray[i].DateReceived.HasValue)
                            writer.SetNull(i);
                        else
                            writer.SetDateTime(dateReceivedFieldIndex, recArray[i].DateReceived.Value);
                        if (!recArray[i].Price.HasValue)
                            writer.SetNull(i);
                        else
                            writer.SetDecimal(priceFieldIndex, recArray[i].Price.Value);
                        if (!recArray[i].NeedsWalking.HasValue)
                            writer.SetNull(i);
                        else
                            writer.SetBoolean(needsWalkingFieldIndex, recArray[i].NeedsWalking.Value);
                        if (recArray[i].Type == null)
                            writer.SetNull(i);
                        else
                            writer.SetString(typeFieldIndex, recArray[i].Type);

                        writer.Write();
                    }

                    for (int i = 3; i < recArray.Length; i++)
                    {
                        if (recArray[i].PetName == null)
                            writer[petNameMetaField.Name] = null;
                        else
                            writer[petNameMetaField.Name] = recArray[i].PetName;
                        if (!recArray[i].Age.HasValue)
                            writer[ageMetaField.Name] = null;
                        else
                            writer[ageMetaField.Name] = recArray[i].Age.Value;
                        if (recArray[i].Color == null)
                            writer[colorMetaField.Name] = null;
                        else
                            writer[colorMetaField.Name] = recArray[i].Color;
                        if (!recArray[i].DateReceived.HasValue)
                            writer[dateReceivedMetaField.Name] = null;
                        else
                            writer[dateReceivedMetaField.Name] = recArray[i].DateReceived.Value;
                        if (!recArray[i].Price.HasValue)
                            writer[priceMetaField.Name] = null;
                        else
                            writer[priceMetaField.Name] = recArray[i].Price.Value;
                        if (!recArray[i].NeedsWalking.HasValue)
                            writer[needsWalkingMetaField.Name] = null;
                        else
                            writer[needsWalkingMetaField.Name] = recArray[i].NeedsWalking.Value;
                        if (recArray[i].Type == null)
                            writer[typeMetaField.Name] = null;
                        else
                            writer[typeMetaField.Name] = recArray[i].Type;

                        writer.Write();
                    }
                }
            }

            string DataFilePath = Path.Combine(DataFolder, BasicExampleFileName);
            if (!TextFilesAreEqual(filePath, DataFilePath))
                Assert.Fail("BasicExample does not match Test Data");
            else
            {
                using (FtReader ftReader = new FtReader())
                {
                    ftReader.Open(filePath);
                    using (StreamReader strmReader = new StreamReader(filePath))
                    {
                        Assert.AreEqual<bool>(true, ftReader.Declared);
                        Assert.AreEqual<bool>(true, ftReader.HeaderRead);
                        Assert.AreEqual<int>(headingArray.Length, ftReader.HeadingLineReadCount);
                        Assert.AreEqual<FtMetaReferenceType>(FtMetaReferenceType.Embedded, ftReader.MetaReferenceType);
                        Assert.AreEqual<FtLineType>(FtLineType.Heading, ftReader.LineType); // last line in header

                        FtStringField petNameField = ftReader.FieldList.Get(petNameMetaField.Name) as FtStringField;
                        int petNameFieldIndex = ftReader.GetOrdinal(petNameMetaField.Name);
                        FtFloatField ageField = ftReader.FieldList.Get(ageMetaField.Name) as FtFloatField;
                        int ageFieldIndex = ftReader.GetOrdinal(ageMetaField.Name);
                        FtStringField colorField = ftReader.FieldList.Get(colorMetaField.Name) as FtStringField;
                        int colorFieldIndex = ftReader.GetOrdinal(colorMetaField.Name);
                        FtDateTimeField dateReceivedField = ftReader.FieldList.Get(dateReceivedMetaField.Name) as FtDateTimeField;
                        int dateReceivedFieldIndex = ftReader.GetOrdinal(dateReceivedMetaField.Name);
                        FtDecimalField priceField = ftReader.FieldList.Get(priceMetaField.Name) as FtDecimalField;
                        int priceFieldIndex = ftReader.GetOrdinal(priceMetaField.Name);
                        FtBooleanField needsWalkingField = ftReader.FieldList.Get(needsWalkingMetaField.Name) as FtBooleanField;
                        int needsWalkingFieldIndex = ftReader.GetOrdinal(needsWalkingMetaField.Name);
                        FtStringField typeField = ftReader.FieldList.Get(typeMetaField.Name) as FtStringField;
                        int typeFieldIndex = ftReader.GetOrdinal(typeMetaField.Name);

                        // skip comment lines
                        string strmLine;
                        do
                        {
                            strmLine = strmReader.ReadLine();
                        }
                        while (strmLine[0] == ftReader.LineCommentChar);

                        for (int i = 0; i < headingArray.Length; i++)
                        {
                            strmLine = strmReader.ReadLine();

                            Assert.AreEqual<string>(headingArray[i].PetName, petNameField.Headings[i]);
                            Assert.AreEqual<string>(headingArray[i].Age, ageField.Headings[i]);
                            Assert.AreEqual<string>(headingArray[i].Color, colorField.Headings[i]);
                            Assert.AreEqual<string>(headingArray[i].DateReceived, dateReceivedField.Headings[i]);
                            Assert.AreEqual<string>(headingArray[i].Price, priceField.Headings[i]);
                            Assert.AreEqual<string>(headingArray[i].NeedsWalking, needsWalkingField.Headings[i]);
                            Assert.AreEqual<string>(headingArray[i].Type, typeField.Headings[i]);
                        }

                        for (int i = 0; i < recArray.Length; i++)
                        {
                            ftReader.Read();

                            Assert.AreEqual<string>(strmLine, ftReader.Line);
                            Assert.AreEqual<int>(-1, ftReader.IgnoreExtraCharsLinePosition);

                            Assert.AreEqual<int>(i + 1, ftReader.RecordCount);
                            Assert.AreEqual<int>(1, ftReader.TableCount);

                            if (recArray[i].PetName == null)
                                Assert.AreEqual<bool>(true, petNameField.IsNull());
                            else
                            {
                                Assert.AreEqual<string>(recArray[i].PetName, petNameField.Value);
                                Assert.AreEqual<object>(recArray[i].PetName, ftReader[petNameMetaField.Name]);
                                Assert.AreEqual<object>(recArray[i].PetName, ftReader[petNameFieldIndex]);
                                Assert.AreEqual<string>(recArray[i].PetName, ftReader.GetString(petNameFieldIndex));
                            }
                            if (!recArray[i].Age.HasValue)
                            {
                                Assert.AreEqual<bool>(true, ageField.IsNull());
                                Assert.AreEqual<bool>(true, ftReader.IsDBNull(ageFieldIndex));
                            }
                            else
                            {
                                Assert.AreEqual<double>(recArray[i].Age.Value, ageField.Value);
                                Assert.AreEqual<object>(recArray[i].Age, ftReader[ageMetaField.Name]);
                                Assert.AreEqual<object>(recArray[i].Age, ftReader[ageFieldIndex]);
                                Assert.AreEqual<double>(recArray[i].Age.Value, ftReader.GetDouble(ageFieldIndex));
                            }
                            if (recArray[i].Color == null)
                                Assert.AreEqual<bool>(true, colorField.IsNull());
                            else
                            {
                                Assert.AreEqual<string>(recArray[i].Color, colorField.Value);
                                Assert.AreEqual<object>(recArray[i].Color, ftReader[colorMetaField.Name]);
                                Assert.AreEqual<object>(recArray[i].Color, ftReader[colorFieldIndex]);
                                Assert.AreEqual<string>(recArray[i].Color, ftReader.GetString(colorFieldIndex));
                            }
                            if (!recArray[i].DateReceived.HasValue)
                                Assert.AreEqual<bool>(true, dateReceivedField.IsNull());
                            else
                            {
                                Assert.AreEqual<DateTime>(recArray[i].DateReceived.Value, dateReceivedField.Value);
                                Assert.AreEqual<object>(recArray[i].DateReceived, ftReader[dateReceivedMetaField.Name]);
                                Assert.AreEqual<object>(recArray[i].DateReceived, ftReader[dateReceivedFieldIndex]);
                                Assert.AreEqual<DateTime>(recArray[i].DateReceived.Value, ftReader.GetDateTime(dateReceivedFieldIndex));
                            }
                            if (!recArray[i].Price.HasValue)
                                Assert.AreEqual<bool>(true, priceField.IsNull());
                            else
                            {
                                Assert.AreEqual<decimal>(recArray[i].Price.Value, priceField.Value);
                                Assert.AreEqual<object>(recArray[i].Price, ftReader[priceMetaField.Name]);
                                Assert.AreEqual<object>(recArray[i].Price, ftReader[priceFieldIndex]);
                                Assert.AreEqual<decimal>(recArray[i].Price.Value, ftReader.GetDecimal(priceFieldIndex));
                            }
                            if (!recArray[i].NeedsWalking.HasValue)
                                Assert.AreEqual<bool>(true, needsWalkingField.IsNull());
                            else
                            {
                                Assert.AreEqual<bool>(recArray[i].NeedsWalking.Value, needsWalkingField.Value);
                                Assert.AreEqual<object>(recArray[i].NeedsWalking, ftReader[needsWalkingMetaField.Name]);
                                Assert.AreEqual<object>(recArray[i].NeedsWalking, ftReader[needsWalkingFieldIndex]);
                                Assert.AreEqual<bool>(recArray[i].NeedsWalking.Value, ftReader.GetBoolean(needsWalkingFieldIndex));
                            }
                            if (recArray[i].Type == null)
                                Assert.AreEqual<bool>(true, typeField.IsNull());
                            else
                            {
                                Assert.AreEqual<string>(recArray[i].Type, typeField.Value);
                                Assert.AreEqual<object>(recArray[i].Type, ftReader[typeMetaField.Name]);
                                Assert.AreEqual<object>(recArray[i].Type, ftReader[typeFieldIndex]);
                                Assert.AreEqual<string>(recArray[i].Type, ftReader.GetString(typeFieldIndex));
                            }

                            strmLine = strmReader.ReadLine();
                        }

                        Assert.AreEqual<bool>(false, ftReader.Read());
                        Assert.AreEqual<int>(recArray.Length, ftReader.RecordCount);
                    }

                    // repeat but read header separately
                    ftReader.Open(filePath, false);

                    Assert.AreEqual<int>(0, ftReader.HeadingLineReadCount);
                    Assert.AreEqual<int>(0, ftReader.RecordCount);
                    Assert.AreEqual<int>(0, ftReader.TableCount);

                    Assert.AreEqual<bool>(true, ftReader.Declared);
                    Assert.AreEqual<bool>(false, ftReader.HeaderRead);

                    ftReader.ReadHeader();

                    Assert.AreEqual<bool>(true, ftReader.HeaderRead);
                    Assert.AreEqual<int>(headingArray.Length, ftReader.HeadingLineReadCount);
                    Assert.AreEqual<FtMetaReferenceType>(FtMetaReferenceType.Embedded, ftReader.MetaReferenceType);
                    Assert.AreEqual<FtLineType>(FtLineType.Heading, ftReader.LineType); // last line in header

                    ftReader.SeekEnd();
                    Assert.AreEqual<int>(recArray.Length, ftReader.RecordCount);

                    // repeat but reading each line individually
                    ftReader.Open(filePath, false);
                    Assert.AreEqual<bool>(false, ftReader.HeaderRead);
                    Assert.AreEqual<int>(0, ftReader.HeadingLineReadCount);
                    Assert.AreEqual<int>(0, ftReader.RecordCount);
                    Assert.AreEqual<int>(0, ftReader.TableCount);

                    ftReader.ReadLine();
                    Assert.AreEqual<FtLineType>(FtLineType.Signature, ftReader.LineType);
                    ftReader.ReadLine();
                    Assert.AreEqual<FtLineType>(FtLineType.Declaration2, ftReader.LineType);

                    Assert.AreEqual<FtMetaReferenceType>(FtMetaReferenceType.Embedded, ftReader.MetaReferenceType);

                    for (int i = 0; i < 10; i++)
                    {
                        ftReader.ReadLine();
                        if (i == 0)
                            Assert.AreEqual<FtLineType>(FtLineType.Comment, ftReader.LineType);
                        else
                            Assert.AreEqual<FtLineType>(FtLineType.EmbeddedMeta, ftReader.LineType);
                    }
                    for (int i = 0; i < headingArray.Length; i++)
                    {
                        ftReader.ReadLine();
                        Assert.AreEqual<FtLineType>(FtLineType.Heading, ftReader.LineType);
                    }

                    Assert.AreEqual<int>(headingArray.Length, ftReader.HeadingLineReadCount);
                    Assert.AreEqual<bool>(true, ftReader.HeaderRead);

                    for (int i = 0; i < recArray.Length; i++)
                    {
                        ftReader.ReadLine();
                        Assert.AreEqual<FtLineType>(FtLineType.Record, ftReader.LineType);
                    }
                    Assert.AreEqual<bool>(false, ftReader.Read());
                    Assert.AreEqual<int>(recArray.Length, ftReader.RecordCount);
                    Assert.AreEqual<int>(1, ftReader.TableCount);
                }
            }
        }
    }
}
