﻿// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

namespace Xilytix.FieldedText
{
    internal enum InternalError
    {
        BooleanStylesFormatter_StaticTest_IncorrectBasicRecCount = 1,
        BooleanStylesFormatter_StaticTest_IncorrectCompositeRecCount = 2,
        LastLineEndedTypeFormatter_StaticTest_IncorrectFormatRecCount = 3,
        LastLineEndedTypeFormatter_StaticTest_FormatRecOutOfOrder = 4,
        TruncateTypeFormatter_StaticTest_IncorrectFormatRecCount = 5,
        TruncateTypeFormatter_StaticTest_FormatRecOutOfOrder = 6,
        SubstitutionTypeFormatter_StaticTest_IncorrectFormatRecCount = 7,
        SubstitutionTypeFormatter_StaticTest_FormatRecOutOfOrder = 8,
        SubstitutionPropertyIdFormatter_StaticTest_IncorrectFormatRecCount = 9,
        SubstitutionPropertyIdFormatter_StaticTest_FormatRecOutOfOrder = 10,
        SequenceRedirectPropertyIdFormatter_StaticTest_IncorrectFormatRecCount = 11,
        SequenceRedirectPropertyIdFormatter_StaticTest_FormatRecOutOfOrder = 12,
        SequencePropertyIdFormatter_StaticTest_IncorrectFormatRecCount = 13,
        SequencePropertyIdFormatter_StaticTest_IncorrectFormatRecOutOfOrder = 14,
        SequenceItemPropertyIdFormatter_StaticTest_IncorrectFormatRecCount = 15,
        SequenceItemPropertyIdFormatter_StaticTest_FormatRecOutOfOrder = 16,
        SequenceInvokationDelayFormatter_StaticTest_IncorrectFormatRecCount = 17,
        SequenceInvokationDelayFormatter_StaticTest_FormatRecOutOfOrder = 18,
        RootPropertyIdFormatter_StaticTest_IncorrectFormatRecCount = 19,
        RootPropertyIdFormatter_StaticTest_FormatRecOutOfOrder = 20,
        QuotedTypeFormatter_StaticTest_IncorrectFormatRecCount = 21,
        QuotedTypeFormatter_StaticTest_FormatRecOutOfOrder = 22,
        PadCharTypeFormatter_StaticTest_IncorrectFormatRecCount = 23,
        PadCharTypeFormatter_StaticTest_FormatRecOutOfOrder = 24,
        PadAlignmentFormatter_StaticTest_IncorrectFormatRecCount = 25,
        PadAlignmentFormatter_StaticTest_FormatRecOutOfOrder = 26,
        MetaReferenceTypeFormatter_StaticTest_IncorrectFormatRecCount = 27,
        MetaReferenceTypeFormatter_StaticTest_FormatRecOutOfOrder = 28,
        MetaElementTypeFormatter_StaticTest_IncorrectFormatRecCount = 29,
        MetaElementTypeFormatter_StaticTest_FormatRecOutOfOrder = 30,
        HeadingConstraintFormatter_StaticTest_IncorrectFormatRecCount = 31,
        HeadingConstraintFormatter_StaticTest_FormatRecOutOfOrder = 32,
        FieldPropertyIdFormatter_StaticTest_IncorrectFormatRecCount = 33,
        FieldPropertyIdFormatter_StaticTest_FormatRecOutOfOrder = 34,
        EndOfLineTypeFormatter_StaticTest_IncorrectFormatRecCount = 35,
        EndOfLineTypeFormatter_StaticTest_FormatRecOutOfOrder = 36,
        EndOfLineAutoWriteTypeFormatter_StaticTest_IncorrectFormatRecCount = 37,
        EndOfLineAutoWriteTypeFormatter_StaticTest_FormatRecOutOfOrder = 38,
        Core_InvokeSequences_UnsupportedInvokationDelay = 39,
        DeclaredParameters_SetMetaReference_UnknownMetaReferenceType = 40,
        DeclaredParametersFormatter_ExtractValue_UnknownInQuotes = 41,
        DeclaredParametersFormatter_ToSignatureLineText_VersionNotSpecified = 42,
        CharReader_Read_UnsupportedState = 43,
        CharReader_Peek_UnsupportedState = 44,
        DeclarationParser_ParseChar_UnsupportedState = 45,
        DeclarationParser_FinishLine_UnexpectedState = 46,
        DeclarationParser_FinishLine_UnsupportedState = 47,
        LineParser_ParseChar_Unsupported_InLineState = 48,
        LineParser_ParseChar_InPendingNextLineFeed = 49,
        LineParser_ParseChar_InNextTextOut = 50,
        LineParser_ParseInChar_UnsupportedEndOfLineType = 51,
        HeadingLineRecordParser_ParseChar_OutState = 52,
        HeadingLineRecordParser_ParseChar_UnsupportedState = 53,
        HeadingLineRecordParser_Finish_UnsupportedState = 54,
        HeadingLineRecordParser_ExitActiveField_UnsupportedState = 55,
        DelimitedFieldParser_EnterField_UnsupportedQuotedType = 56,
        DelimitedFieldParser_ParseChar_UnsupportedQuotedState = 57,
        FixedWidthFieldParser_ParseChar_UnsupportedState = 58,
        FieldedTextReadElement_ResolveProperties_UnsupportedPropertyId = 59,
        FieldReadElement_ResolveProperties_ValueUnsupportedDataType = 60,
        FieldReadElement_ResolveProperties_StylesUnsupportedDataType = 61,
        FieldReadElement_ResolveProperties_FormatUnsupportedDataType = 62,
        FieldReadElement_ResolveProperties_FalseTextUnsupportedDataType = 63,
        FieldReadElement_ResolveProperties_TrueTextUnsupportedDataType = 64,
        FieldReadElement_ResolveProperties_UnsupportedPropertyId = 65,
        SequenceReadElement_ResolvePropertiesTo_UnsupportedPropertyId = 66,
        SequenceReadElement_ResolveItemsTo_TryAddOrIgnoreAttribute = 67,
        SequenceItemReadElement_ResolveProperties_UnsupportedPropertyId = 68,
        SequenceRedirectReadElement_ResolveProperties_ValueUnsupportedRedirectType = 69,
        SequenceRedirectReadElement_ResolveProperties_UnsupportedPropertyId = 70,
        SubstitutionReadElement_ResolveProperties_UnsupportedPropertyId = 71,
        SerializationReader_ParseChar_UnsupportedLineType = 72,
        SerializationReader_ParseChar_UnexpectedDeclarationLineCount = 73,
        SerializationReader_FinishLastLine_UnsupportedLineType = 74,
        SerializationWriter_WriteHeadingField_UnsupportedQuotedType = 75,
        SerializationWriter_WriteRecordField_UnsupportedQuotedType = 76,
        SerializationWriter_DoesTextRequireQuotes_UnsupportedEndOfLineType = 77,
        SerializationWriter_CalculateEndOfLineText_UnsupportedEndOfLineAutoWriteType = 78,
        SerializationWriter_CalculateEndOfLineText_UnsupportedEndOfLineType = 79,
        FtStandardDataType_StaticTest_TypeRecOutOfOrder = 80,
        FtStandardSequenceRedirectType_StaticTest_TypeRecOutOfOrder = 81,
        FtCommaText_TryParse_UnsupportedInQuotes1 = 82,
        FtCommaText_TryParse_UnsupportedInQuotes2 = 83,
        FtDataReader_NextResult_UnsupportedReadRecordResult = 84,
        FtDataReader_NextResult_UnsupportedReadState = 85,
        FtDataReader_Read_UnsupportedReadRecordResult = 86,
        FtDataReader_Read_UnsupportedReadState = 87,
        FtMetaSequenceItem_AssignExcludingRedirects_SourceFieldNotFound = 88,
        FtMetaSequenceItem_AssignExcludingRedirects_FieldIndexOutOfRange = 89,
        FtMetaSequenceItem_Assign_SourceFieldNotFound = 90,
        FtMetaSequenceItem_Assign_FieldIndexOutOfRange = 91,
        FtField_TruncateText_UnsupportedTruncateType = 92,
        FtField_ParseHeadingText_UnsupportedHeadingConstraint = 93,
        FtField_PadText_UnsupportedPadAlignment = 94,
        FtField_PadText_UnsupportedPadCharType = 95,
        FtField_LoadDelimitedValue_UnsupportedValueQuotedType = 96,
        FtFieldFieldDefinition_LoadMeta_UnsupportedHeadingPadAlignment = 97,
        FtFieldFieldDefinition_LoadMeta_UnsupportedValuePadAlignment = 98,
        FtSequenceItem_LoadMeta_MetaSequenceItemFieldNotFoundInMetaFieldList = 99,
        FtSequenceRedirect_LoadMeta_MetaSequenceRedirectSequenceNotFoundInMetaSequenceList = 100,
        FtSubstitution_LoadMeta_UnsupportedAutoWriteType = 101,
        FtMeta_Validate_UnsupportedEndOfLineType = 102,
        FtMeta_Validate_UnsupportedValidateTest = 103,
    }
}
