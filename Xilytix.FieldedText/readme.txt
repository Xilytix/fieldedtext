Xilytix C# FieldedText class library

Product Home Page: http://www.xilytix.com/fieldedtext/csharplibrary/index.html
Code examples: http://www.xilytix.com/fieldedtext/csharplibrary/examples

Version History
1.0.6	20 Aug 2017 Added
                      - Converted to .NET Standard 2.0
1.0.5	11 Mar 2017 Added
                      - FtField.AsObject setter and FtWriter[fieldname] now
                          attempt to cast value if not matching type		
                    Fixed
                      - Minor disposal issues fixed
1.0.4	13 Nov 2016 Added
                      - SerializationCore.GetOrdinal(int fieldId)
                          Finds field ordinal using field Id
                      - Developed more examples
                    Fixed
                      - SerializationCore.GetOrdinal(string name) now throws
                          exception if field not found.
                          Use SerializationCore.FieldList.IndexOfField() to
                          return error instead of throwing exception.
                      - FtMetaSerializer.Serialize(..., XmlWriterSettings) was
                          ignoring XmlWriterSettings
1.0.3	06 Nov 2016 Reader and Writer events fixed and added
                      - FieldHeadingReadReady
                      - FieldHeadingWriteReady
                      - FieldValueReadReady
                      - FieldValueWriteReady
                      - FieldHeadingReadReady
                      - FieldHeadingWriteReady
                      - FieldValueReadReady
                      - FieldValueWriteReady
                      - HeadingLineStarted
                      - HeadingLineFinished
                      - RecordStarted
                      - RecordFinished
                      - SequenceRedirected
                    Added
                      - Developed more examples
                    Fixed
                      - Better disposal of internal readers and writers
                      - XML in Embedded Meta now specifies encoding used in
                          file
                          Reader/Writer from opening a subsequent file/stream
                      - FtSerializer completed
1.0.2	20 Oct 2016 Added
                      - Property FTReader.Line: returns line read.
                      - Property FTReader.IgnoreExtraCharsStartPosition:
                        returns position in line from which characters 
                        are ignored.
                    Fixed
                      - Bug in SerializationCore.Reset() which prevented 
                          Reader/Writer from opening a subsequent file/stream
1.0.1   01 Oct 2016 SerializationReader now supports IDataReader & IDataRecord
                      Breaking Changes: 
                        - SerializationReader.Read() returns false at end of
                          current table (instead at of end of file).  Set
                          AutoNextTable to true for Read() to automatically
                          move to next tables (and read to end of file).
                        - FtLineType.Rec changed to FtLineType.Record
1.0.0   29 Sep 2016 Initial Release
