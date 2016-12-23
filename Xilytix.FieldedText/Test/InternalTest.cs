// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

namespace Xilytix.FieldedText.Test
{
    public struct InternalTest
    {
        // use this in Test Suite to check static information in structs/classes
        public static void StaticTest()
        {
            FtStandardDataType.StaticTest();
            FtStandardSequenceRedirectType.StaticTest();

            MetaSerialization.Formatting.BooleanStylesFormatter.StaticTest();
            MetaSerialization.Formatting.EndOfLineTypeFormatter.StaticTest();
            MetaSerialization.Formatting.EndOfLineAutoWriteTypeFormatter.StaticTest();
            MetaSerialization.Formatting.FieldPropertyIdFormatter.StaticTest();
            MetaSerialization.Formatting.HeadingConstraintFormatter.StaticTest();
            MetaSerialization.Formatting.LastLineEndedTypeFormatter.StaticTest();
            MetaSerialization.Formatting.MetaElementTypeFormatter.StaticTest();
            MetaSerialization.Formatting.MetaReferenceTypeFormatter.StaticTest();
            MetaSerialization.Formatting.PadAlignmentFormatter.StaticTest();
            MetaSerialization.Formatting.PadCharTypeFormatter.StaticTest();
            MetaSerialization.Formatting.QuotedTypeFormatter.StaticTest();
            MetaSerialization.Formatting.RootPropertyIdFormatter.StaticTest();
            MetaSerialization.Formatting.SequenceInvokationDelayFormatter.StaticTest();
            MetaSerialization.Formatting.SequenceItemPropertyIdFormatter.StaticTest();
            MetaSerialization.Formatting.SequencePropertyIdFormatter.StaticTest();
            MetaSerialization.Formatting.SequenceRedirectPropertyIdFormatter.StaticTest();
            MetaSerialization.Formatting.SubstitutionPropertyIdFormatter.StaticTest();
            MetaSerialization.Formatting.SubstitutionTypeFormatter.StaticTest();
            MetaSerialization.Formatting.TruncateTypeFormatter.StaticTest();
        }
    }
}
