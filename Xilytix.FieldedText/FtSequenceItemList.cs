// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System.Collections.Generic;

namespace Xilytix.FieldedText
{
    using Factory;
    using List = List<FtSequenceItem>;
    public class FtSequenceItemList
    {
        private List list;

        internal FtSequenceItemList() { list = new List(); }

        public int Count { get { return list.Count; } }
        public FtSequenceItem this[int idx] { get { return list[idx]; } }

        internal void Clear() { list.Clear(); }
        internal int Capacity { get { return list.Capacity; } set { list.Capacity = value; } }
        internal FtSequenceItem New()
        {
            FtSequenceItem item = SequenceItemFactory.CreateSequenceItem(Count);
            list.Add(item);
            return item;
        }
    }
}
