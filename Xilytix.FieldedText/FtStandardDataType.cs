// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;

namespace Xilytix.FieldedText
{
    public struct FtStandardDataType
    {
        public const int Unknown = 0;
        public const string UnknownName = "";
        public const int String = 1;
        public const string StringName = "String";
        public const int Boolean = 2;
        public const string BooleanName = "Boolean";
        public const int Integer = 3;
        public const string IntegerName = "Integer";
        public const int Float = 4;
        public const string FloatName = "Float";
        public const int Decimal = 5;
        public const string DecimalName = "Decimal";
        public const int DateTime = 6;
        public const string DateTimeName = "DateTime";

        // up to 9999 are reserved.

        private struct TypeRec
        {
            public int Type;
            public string Name;
        }

        private static TypeRec[] typeRecArray =
        {
            new TypeRec { Type = Unknown, Name = UnknownName },
            new TypeRec { Type = String, Name = StringName },
            new TypeRec { Type = Boolean, Name = BooleanName },
            new TypeRec { Type = Integer, Name = IntegerName },
            new TypeRec { Type = Float, Name = FloatName },
            new TypeRec { Type = Decimal, Name = DecimalName },
            new TypeRec { Type = DateTime, Name = DateTimeName },
        };

        internal static void StaticTest()
        {
            for (int i = 0; i < typeRecArray.Length; i++)
            {
                if (typeRecArray[i].Type != i)
                {
                    throw FtInternalException.Create(InternalError.FtStandardDataType_StaticTest_TypeRecOutOfOrder, typeRecArray[i].ToString());
                }
            }
        }

        public static string ToName(int dataType)
        {
            if ((dataType <= Unknown) || (dataType >= typeRecArray.Length))
                throw new ArgumentException(string.Format(Properties.Resources.FtStandardDataType_ToName_InvalidType, dataType));
            else
                return typeRecArray[dataType].Name;
        }

        public static bool SameName(string left, string right)
        {
            return string.Equals(left, right, StringComparison.OrdinalIgnoreCase);
        }
    }
}
