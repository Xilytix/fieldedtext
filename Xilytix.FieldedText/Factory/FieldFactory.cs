// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;
using System.Collections.Generic;
using System.Globalization;

namespace Xilytix.FieldedText.Factory
{
    using ConstructorList = List<FieldConstructor>;
    public static class FieldFactory
    {
        private static ConstructorList constructorList;

        static FieldFactory()
        {
            constructorList = new ConstructorList();

            // register standard constructors
            constructorList.Capacity = 6;
            RegisterConstructor(new StringFieldConstructor());
            RegisterConstructor(new BooleanFieldConstructor());
            RegisterConstructor(new IntegerFieldConstructor());
            RegisterConstructor(new FloatFieldConstructor());
            RegisterConstructor(new DecimalFieldConstructor());
            RegisterConstructor(new DateTimeFieldConstructor());
        }

        private static bool TryFindConstructor(string dataTypeName, out int idx)
        {
            bool result = false;
            idx = -1;
            for (int i = 0; i < constructorList.Count; i++)
            {
                if (FtStandardDataType.SameName(constructorList[i].DataTypeName, dataTypeName))
                {
                    idx = i;
                    result = true;
                    break;
                }
            }

            return result;
        }

        private static bool TryFindConstructor(int dataType, out int idx)
        {
            bool result = false;
            idx = -1;
            for (int i = 0; i < constructorList.Count; i++)
            {
                if (constructorList[i].DataType == dataType)
                {
                    idx = i;
                    result = true;
                    break;
                }
            }

            return result;
        }

        public static void RegisterConstructor(FieldConstructor constructor)
        {
            int idx;
            if (TryFindConstructor(constructor.DataType, out idx))
                throw new ArgumentException(string.Format(Properties.Resources.FieldFactory_RegisterConstructor_TypeAlreadyRegistered, constructor.DataType));
            else
            {
                if (TryFindConstructor(constructor.DataTypeName, out idx))
                    throw new ArgumentException(string.Format(Properties.Resources.FieldFactory_RegisterConstructor_NameAlreadyRegistered, constructor.DataTypeName));
                else
                    constructorList.Add(constructor);
            }
        }

        public static FieldConstructor[] GetRegisteredConstructors() { return constructorList.ToArray(); }
        public static void UnregisterAllConstructors() { constructorList.Clear(); }
        public static void UnregisterConstructor(FieldConstructor constructor) { constructorList.Remove(constructor); }
        public static void UnregisterConstructor(int dataType)
        {
            int idx;
            if (TryFindConstructor(dataType, out idx))
            {
                constructorList.RemoveAt(idx);
            }
        }
        public static void UnregisterConstructor(string dataTypeName)
        {
            int idx;
            if (TryFindConstructor(dataTypeName, out idx))
            {
                constructorList.RemoveAt(idx);
            }
        }

        internal static FtFieldDefinition CreateFieldDefinition(int index, int dataType)
        {
            int idx;
            if (!TryFindConstructor(dataType, out idx))
                return null;
            else
                return constructorList[idx].CreateFieldDefinition(index);
        }
        internal static FtFieldDefinition CreateFieldDefinition(int index, string dataTypeName)
        {
            int idx;
            if (!TryFindConstructor(dataTypeName, out idx))
                return null;
            else
                return constructorList[idx].CreateFieldDefinition(index);
        }
        internal static FtField CreateField(FtSequenceInvokation sequenceInvokation, FtSequenceItem sequenceItem)
        {
            int idx;
            if (!TryFindConstructor(sequenceItem.FieldDefinition.DataType, out idx))
                return null;
            else
                return constructorList[idx].CreateField(sequenceInvokation, sequenceItem);
        }
        internal static FtMetaField CreateMetaField(int dataType, int headingCount)
        {
            int idx;
            if (!TryFindConstructor(dataType, out idx))
                return null;
            else
                return constructorList[idx].CreateMetaField(headingCount);
        }
        internal static FtMetaField CreateMetaField(string dataTypeName, int headingCount)
        {
            int idx;
            if (!TryFindConstructor(dataTypeName, out idx))
                return null;
            else
                return constructorList[idx].CreateMetaField(headingCount);
        }

        public static string GetName(int dataType)
        {
            int idx;
            if (TryFindConstructor(dataType, out idx))
                return constructorList[idx].DataTypeName;
            else
                throw new ArgumentException(string.Format(Properties.Resources.FieldFactory_GetName_NoConstructorForDataType, dataType));
        }
        public static bool TryGetDataType(string dataTypeName, out int dataType)
        {
            int idx;
            if (TryFindConstructor(dataTypeName, out idx))
            {
                dataType = constructorList[idx].DataType;
                return true;
            }
            else
            {
                dataType = 0;
                return false;
            }
        }
    }
}
