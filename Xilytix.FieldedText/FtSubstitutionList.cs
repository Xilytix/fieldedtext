// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System.Collections.Generic;

namespace Xilytix.FieldedText
{
    using Factory;
    using List = List<FtSubstitution>;
    public class FtSubstitutionList
    {
        private List list;

        internal FtSubstitutionList() { list = new List(); }

        public int Count { get { return list.Count; } }
        public FtSubstitution this[int idx] { get { return list[idx]; } }

        public void Clear() { list.Clear(); }
        public int Capacity { get { return list.Capacity; } set { list.Capacity = value; } }
        public FtSubstitution New()
        {
            FtSubstitution substitution = SubstitutionFactory.CreateSubstitution(Count);
            list.Add(substitution);
            return substitution;
        }

        public bool TryGetValue(char token, out string value)
        {
            bool result = false;
            value = "";
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Token == token)
                {
                    value = list[i].Value;
                    result = true;
                    break;
                }
            }

            return result;
        }
    }
}
