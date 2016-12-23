using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xilytix.FieldedText.MetaSerialization.Formatting
{
    using System;

    internal class LastLineEndedTypeFormatter
    {
        private struct FormatRec
        {
            public FtLastLineEndedType Enumerator;
            public string AttributeValue;
        }

        private static FormatRec[] formatRecArray =
        {
            new FormatRec {Enumerator = FtLastLineEndedType.Never, AttributeValue = "Never" },
            new FormatRec {Enumerator = FtLastLineEndedType.Always, AttributeValue = "Always" },
            new FormatRec {Enumerator = FtLastLineEndedType.Optional, AttributeValue = "Optional" },
        };

        internal static void StaticTest()
        {
            if (formatRecArray.Length != Enum.GetNames(typeof(FtLastLineEndedType)).Length)
            {
                throw FtInternalException.Create(InternalError.LastLineEndedTypeFormatter_StaticTest_FormatRecOutOfOrder, formatRecArray.Length.ToString());
            }
            for (int i = 0; i < formatRecArray.Length; i++)
            {
                if ((int)formatRecArray[i].Enumerator != i)
                {
                    throw FtInternalException.Create(InternalError.LastLineEndedTypeFormatter_StaticTest_IncorrectFormatRecCount, formatRecArray[i].Enumerator.ToString());
                }
            }
        }

        internal static string ToAttributeValue(FtLastLineEndedType enumerator) { return formatRecArray[(int)enumerator].AttributeValue; }
        internal static bool TryParseAttributeValue(string attributeValue, out FtLastLineEndedType enumerator)
        {
            enumerator = FtLastLineEndedType.Optional; // avoid compiler error
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

