// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;

namespace Xilytix.FieldedText.MetaSerialization.Formatting
{
    internal static class SubstitutionPropertyIdFormatter
    {
        private struct FormatRec
        {
            public FtMetaSubstitution.PropertyId Id;
            public string AttributeName;
        }

        private static FormatRec[] formatRecArray =
        {
            new FormatRec {Id = FtMetaSubstitution.PropertyId.Token, AttributeName = "Token" },
            new FormatRec {Id = FtMetaSubstitution.PropertyId.Type, AttributeName = "Type" },
            new FormatRec {Id = FtMetaSubstitution.PropertyId.Value, AttributeName = "Value" },
        };

        internal static void StaticTest()
        {
            if (formatRecArray.Length != Enum.GetNames(typeof(FtMetaSubstitution.PropertyId)).Length)
            {
                throw FtInternalException.Create(InternalError.SubstitutionPropertyIdFormatter_StaticTest_IncorrectFormatRecCount, formatRecArray.Length.ToString());
            }
            for (int i = 0; i < formatRecArray.Length; i++)
            {
                if ((int)formatRecArray[i].Id != i)
                {
                    throw FtInternalException.Create(InternalError.SubstitutionPropertyIdFormatter_StaticTest_FormatRecOutOfOrder, formatRecArray[i].Id.ToString());
                }
            }
        }

        internal static string ToAttributeName(FtMetaSubstitution.PropertyId id) { return formatRecArray[(int)id].AttributeName; }
        internal static bool TryParseAttributeName(string attributeName, out FtMetaSubstitution.PropertyId id)
        {
            id = FtMetaSubstitution.PropertyId.Token; // avoid compiler error
            bool result = false;
            foreach (FormatRec rec in formatRecArray)
            {
                if (String.Equals(rec.AttributeName, attributeName, StringComparison.OrdinalIgnoreCase))
                {
                    id = rec.Id;
                    result = true;
                    break;
                }
            }
            return result;
        }
    }
}
