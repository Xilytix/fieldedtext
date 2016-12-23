// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

namespace Xilytix.FieldedText
{
    public enum FtSerializationError
    {
        Abort,
        DeclarationParameterNameIsZeroLength,
        DeclaredParameterNameContainsSeparator,
        DeclarationParameterNameNotTerminated,
        DeclarationParameterMissingValue,
        DeclarationParameterValueNotQuoted,
        DeclarationParameterValueNotTerminated,
        DeclarationParametersMissingVersion,
        DeclarationParameterVersionIsNotFirst,
        DeclarationParameterInvalidVersion,
        EmbeddedMetaNotFound,
        IncompleteEmbeddedMeta,
        HeadingLineNotEnoughFields,
        RecordNotEnoughFields,
        HeadingLineTooManyFields,
        RecordTooManyFields,
        HeadingQuotedFieldMissingEndQuoteChar,
        ValueQuotedFieldMissingEndQuoteChar,
        HeadingNonWhiteSpaceCharBeforeQuotesOpened,
        ValueNonWhiteSpaceCharBeforeQuotesOpened,
        HeadingNonWhiteSpaceCharAfterQuotesClosed,
        ValueNonWhiteSpaceCharAfterQuotesClosed,
        HeadingWidthNotReached,
        ValueWidthNotReached,
        HeadingWidthExceeded, // should never occur
        ValueWidthExceeded, // should never occur
        FieldValueToText,
        FieldTruncated,
        FieldConstantValueMismatch,
        FieldConstNameHeadingMismatch,
        FieldTextParse,
        MoreThanOneRootSequence,
        HeaderAlreadyWritten,
        LastLineEndedError,
        IncompleteDeclaration,
        InsufficientHeadingLines,
        InvalidMeta,
        LoadMetaFromText,
        LoadMetaFromFile,
        LoadMetaFromUrl,
    }
}
