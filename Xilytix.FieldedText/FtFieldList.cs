// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;
using System.Collections.Generic;

namespace Xilytix.FieldedText
{
    using List = List<FtField>;
    public class FtFieldList
    {
        private List list;

        internal FtFieldList() { list = new List(); }

        internal int Capacity { get { return list.Capacity; } set { list.Capacity = value; } }
        public int Count { get { return list.Count; } }
        public FtField this[int idx] { get { return list[idx]; } }
        public FtField this[string name]
        {
            get
            {
                int idx = IndexOfName(name);
                if (idx >= 0)
                    return list[idx];
                else
                    throw new IndexOutOfRangeException(string.Format(Properties.Resources.FtFieldList_This_FtFieldNotFound, name));
            } 
        }

        public bool Find(string name, out int idx)
        {
            idx = IndexOfName(name);
            return idx >= 0;
        }

        public FtField Get(string name)
        {
            int idx = IndexOfName(name);
            if (idx < 0)
                return null;
            else
                return list[idx];
        }

        /// <summary>
        /// Returns index of a field in the current record.  The field is specified by its (case insensitive) field name.
        /// </summary>
        /// <param name="id">Field Name</param>
        /// <returns>Field index in current record</returns>
        public int IndexOfName(string name)
        {
            int result = -1;

            // try case sensitive match first as suspect that most calls will provide correct case in name
            for (int i = 0; i < Count; i++)
            {
                if (string.Equals(name, list[i].Name, System.StringComparison.Ordinal))
                {
                    result = i;
                    break;
                }
            }

            if (result < 0)
            {
                // try case insensitive match if case sensitive failed
                for (int i = 0; i < Count; i++)
                {
                    if (string.Equals(name, list[i].Name, System.StringComparison.OrdinalIgnoreCase))
                    {
                        result = i;
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Returns index of a field in the current record.  The field is specified by its field Id.
        /// </summary>
        /// <param name="id">Field Id</param>
        /// <returns>Field index in current record</returns>
        public int IndexOfId(int id)
        {
            for (int i = 0; i < Count; i++)
            {
                if (list[i].Id == id)
                {
                    return i;
                }
            }

            return -1;
        }

        internal void Clear()
        {
            list.Clear();
        }

        internal void Trim(int fromIndex)
        {
            list.RemoveRange(fromIndex, list.Count - fromIndex);
        }

        internal void Add(FtField field)
        {
            list.Add(field);
        }
    }
}
