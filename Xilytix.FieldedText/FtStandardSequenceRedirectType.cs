// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;

namespace Xilytix.FieldedText
{
    public struct FtStandardSequenceRedirectType
    {
        public const int Unknown = 0;
        public const string UnknownName = "Unknown";
        public const int Null = 1;
        public const string NullName = "Null";
        public const int ExactString = 2;
        public const string ExactStringName = "ExactString";
        public const int CaseInsensitiveString = 3;
        public const string CaseInsensitiveStringName = "CaseInsensitiveString";
        public const int Boolean = 4;
        public const string BooleanName = "Boolean";
        public const int ExactInteger = 5;
        public const string ExactIntegerName = "ExactInteger";
        public const int ExactFloat = 6;
        public const string ExactFloatName = "ExactFloat";
        public const int ExactDateTime = 7;
        public const string ExactDateTimeName = "ExactDateTime";
        public const int Date = 8;
        public const string DateName = "Date";
        public const int ExactDecimal = 9;
        public const string ExactDecimalName = "ExactDecimal";

        // up to 9999 are reserved.

        private struct TypeRec
        {
            public int Type;
            public string Name;
        }

        private static TypeRec[] typeRecArray =
        {
            new TypeRec { Type = Unknown, Name = UnknownName },
            new TypeRec { Type = Null, Name = NullName },
            new TypeRec { Type = ExactString, Name = ExactStringName },
            new TypeRec { Type = CaseInsensitiveString, Name = CaseInsensitiveStringName },
            new TypeRec { Type = Boolean, Name = BooleanName },
            new TypeRec { Type = ExactInteger, Name = ExactIntegerName },
            new TypeRec { Type = ExactFloat, Name = ExactFloatName },
            new TypeRec { Type = ExactDateTime, Name = ExactDateTimeName },
            new TypeRec { Type = Date, Name = DateName },
            new TypeRec { Type = ExactDecimal, Name = ExactDecimalName },
        };

        internal static void StaticTest()
        {
            for (int i = 0; i < typeRecArray.Length; i++)
            {
                if (typeRecArray[i].Type != i)
                {
                    throw FtInternalException.Create(InternalError.FtStandardSequenceRedirectType_StaticTest_TypeRecOutOfOrder);
                }
            }
        }

        public static string ToName(int type)
        {
            if ((type <= Unknown) || (type >= typeRecArray.Length))
                throw new ArgumentException(string.Format(Properties.Resources.FtStandardSequenceRedirectType_ToName_InvalidType, type));
            else
                return typeRecArray[type].Name;
        }
    }
}
