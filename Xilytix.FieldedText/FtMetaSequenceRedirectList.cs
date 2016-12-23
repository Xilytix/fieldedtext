// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System.Collections.Generic;

namespace Xilytix.FieldedText
{
    using Factory;

    using List = List<FtMetaSequenceRedirect>;
    public class FtMetaSequenceRedirectList
    {
        private List list;

        public FtMetaSequenceRedirectList() { list = new List(); }

        public int Count { get { return list.Count; } }
        public FtMetaSequenceRedirect this[int idx] { get { return list[idx]; } }

        private void Add(FtMetaSequenceRedirect redirect)
        {
            list.Add(redirect);
        }
        public FtMetaSequenceRedirect New(int type)
        {
            FtMetaSequenceRedirect result = SequenceRedirectFactory.CreateMetaSequenceRedirect(type);
            Add(result);
            return result;
        }

        public void Remove(FtMetaSequenceRedirect redirect) { list.Remove(redirect); }
        public void RemoveAt(int idx) { list.RemoveAt(idx); }
        public void Clear() { list.Clear(); }

        protected internal void Assign(FtMetaSequenceRedirectList source, FtMetaSequenceList sequenceList, FtMetaSequenceList sourceSequenceList)
        {
            list.Clear();
            list.Capacity = source.Count;

            for (int i = 0; i < source.Count; i++)
            {
                FtMetaSequenceRedirect redirect = source[i].CreateCopy(sequenceList, sourceSequenceList);
                Add(redirect);
            }
        }
    }
}
