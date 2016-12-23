// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System.Collections.Generic;

namespace Xilytix.FieldedText
{
    using Factory;

    using List = List<FtMetaSubstitution>;
    public class FtMetaSubstitutionList
    {
        private List list;

        public FtMetaSubstitutionList() { list = new List(); }

        public int Count { get { return list.Count; } }
        public FtMetaSubstitution this[int idx] { get { return list[idx]; } }

        public void Add(FtMetaSubstitution substitution)
        {
            list.Add(substitution);
        }
        public FtMetaSubstitution New()
        {
            FtMetaSubstitution result = SubstitutionFactory.CreateMetaSubstitution();
            Add(result);
            return result;
        }

        public void Remove(FtMetaSubstitution substitution) { list.Remove(substitution); }
        public void RemoveAt(int idx) { list.RemoveAt(idx); }
        public void Clear() { list.Clear(); }

        public void Assign(FtMetaSubstitutionList source)
        {
            list.Clear();
            list.Capacity = source.Count;

            for (int i = 0; i < source.Count; i++)
            {
                FtMetaSubstitution substitution = source[i].CreateCopy();
                Add(substitution);
            }
        }
    }
}
