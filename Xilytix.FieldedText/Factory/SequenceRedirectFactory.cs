// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;
using System.Collections.Generic;
using Xilytix.FieldedText.Serialization;

namespace Xilytix.FieldedText.Factory
{
    using ConstructorList = List<SequenceRedirectConstructor>;
    public static class SequenceRedirectFactory
    {
        private static ConstructorList constructorList;

        static SequenceRedirectFactory()
        {
            constructorList = new ConstructorList();

            // register standard constructors
            constructorList.Capacity = 9;
            RegisterConstructor(new NullSequenceRedirectConstructor());
            RegisterConstructor(new BooleanSequenceRedirectConstructor());
            RegisterConstructor(new CaseInsensitiveStringSequenceRedirectConstructor());
            RegisterConstructor(new DateSequenceRedirectConstructor());
            RegisterConstructor(new ExactDateTimeSequenceRedirectConstructor());
            RegisterConstructor(new ExactDecimalSequenceRedirectConstructor());
            RegisterConstructor(new ExactFloatSequenceRedirectConstructor());
            RegisterConstructor(new ExactIntegerSequenceRedirectConstructor());
            RegisterConstructor(new ExactStringSequenceRedirectConstructor());
        }

        private static bool TryFindConstructor(string typeName, out int idx)
        {
            bool result = false;
            idx = -1;
            for (int i = 0; i < constructorList.Count; i++)
            {
                if (string.Equals(constructorList[i].SequenceRedirectTypeName, typeName, StringComparison.OrdinalIgnoreCase))
                {
                    idx = i;
                    result = true;
                    break;
                }
            }

            return result;
        }

        private static bool TryFindConstructor(int type, out int idx)
        {
            bool result = false;
            idx = -1;
            for (int i = 0; i < constructorList.Count; i++)
            {
                if (constructorList[i].SequenceRedirectType == type)
                {
                    idx = i;
                    result = true;
                    break;
                }
            }

            return result;
        }

        public static void RegisterConstructor(SequenceRedirectConstructor constructor)
        {
            int idx;
            if (TryFindConstructor(constructor.SequenceRedirectType, out idx))
                throw new ArgumentException(string.Format(Properties.Resources.SequenceRedirectFactory_RegisterConstructor_TypeAlreadyRegistered, 
                                                          constructor.SequenceRedirectType));
            else
            {
                if (TryFindConstructor(constructor.SequenceRedirectTypeName, out idx))
                    throw new ArgumentException(string.Format(Properties.Resources.SequenceRedirectFactory_RegisterConstructor_NameAlreadyRegistered,
                                                              constructor.SequenceRedirectTypeName));
                else
                    constructorList.Add(constructor);
            }
        }

        public static SequenceRedirectConstructor[] GetRegisteredConstructors() { return constructorList.ToArray(); }
        public static void UnregisterAllConstructors() { constructorList.Clear(); }
        public static void UnregisterConstructor(SequenceRedirectConstructor constructor) { constructorList.Remove(constructor); }
        public static void UnregisterConstructor(int type)
        {
            int idx;
            if (TryFindConstructor(type, out idx))
            {
                constructorList.RemoveAt(idx);
            }
        }
        public static void UnregisterConstructor(string typeName)
        {
            int idx;
            if (TryFindConstructor(typeName, out idx))
            {
                constructorList.RemoveAt(idx);
            }
        }

        internal static FtSequenceRedirect CreateSequenceRedirect(int index, int type)
        {
            int idx;
            if (!TryFindConstructor(type, out idx))
                return null;
            else
                return constructorList[idx].CreateSequenceRedirect(index);
        }
        internal static FtSequenceRedirect CreateSequenceRedirect(int index, string typeName)
        {
            int idx;
            if (!TryFindConstructor(typeName, out idx))
                return null;
            else
                return constructorList[idx].CreateSequenceRedirect(index);
        }
        internal static FtMetaSequenceRedirect CreateMetaSequenceRedirect(int type)
        {
            int idx;
            if (!TryFindConstructor(type, out idx))
                return null;
            else
                return constructorList[idx].CreateMetaSequenceRedirect();
        }
        internal static FtMetaSequenceRedirect CreateMetaSequenceRedirect(string typeName)
        {
            int idx;
            if (!TryFindConstructor(typeName, out idx))
                return null;
            else
                return constructorList[idx].CreateMetaSequenceRedirect();
        }

        public static string GetName(int type)
        {
            int idx;
            if (TryFindConstructor(type, out idx))
                return constructorList[idx].SequenceRedirectTypeName;
            else
                throw new ArgumentException(string.Format(Properties.Resources.SequenceRedirectFactory_GetName_NoConstructorForType, type.ToString()));
        }
        public static bool TryGetType(string typeName, out int type)
        {
            int idx;
            if (TryFindConstructor(typeName, out idx))
            {
                type = constructorList[idx].SequenceRedirectType;
                return true;
            }
            else
            {
                type = 0;
                return false;
            }
        }
    }
}
