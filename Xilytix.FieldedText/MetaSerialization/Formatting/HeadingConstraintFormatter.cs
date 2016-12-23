// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;

namespace Xilytix.FieldedText.MetaSerialization.Formatting
{
    internal static class HeadingConstraintFormatter
    {
        private struct FormatRec
        {
            public FtHeadingConstraint Enumerator;
            public string AttributeValue;
        }

        private static FormatRec[] formatRecArray =
        {
            new FormatRec {Enumerator = FtHeadingConstraint.None, AttributeValue = "None" },
            new FormatRec {Enumerator = FtHeadingConstraint.AllConstant, AttributeValue = "AllConstant" },
            new FormatRec {Enumerator = FtHeadingConstraint.MainConstant, AttributeValue = "MainConstant" },
            new FormatRec {Enumerator = FtHeadingConstraint.NameConstant, AttributeValue = "NameConstant" },
            new FormatRec {Enumerator = FtHeadingConstraint.NameIsMain, AttributeValue = "NameIsMain" },
        };

        internal static void StaticTest()
        {
            if (formatRecArray.Length != Enum.GetNames(typeof(FtHeadingConstraint)).Length)
            {
                throw FtInternalException.Create(InternalError.HeadingConstraintFormatter_StaticTest_IncorrectFormatRecCount, formatRecArray.Length.ToString());
            }
            for (int i = 0; i < formatRecArray.Length; i++)
            {
                if ((int)formatRecArray[i].Enumerator != i)
                {
                    throw FtInternalException.Create(InternalError.HeadingConstraintFormatter_StaticTest_FormatRecOutOfOrder, formatRecArray[i].Enumerator.ToString());
                }
            }
        }

        internal static string ToAttributeValue(FtHeadingConstraint enumerator) { return formatRecArray[(int)enumerator].AttributeValue; }
        internal static bool TryParseAttributeValue(string attributeValue, out FtHeadingConstraint enumerator)
        {
            enumerator = FtHeadingConstraint.AllConstant; // avoid compiler error
            bool result = false;
            foreach (FormatRec rec in formatRecArray)
            {
                if (String.Equals(rec.AttributeValue, attributeValue, StringComparison.OrdinalIgnoreCase))
                {
                    enumerator = rec.Enumerator;
                    result = true;
                    break;
                }
            }
            return result;
        }
    }
}
