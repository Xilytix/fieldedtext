// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System.Collections.Generic;

namespace Xilytix.FieldedText
{
    using Factory;

    using List = List<FtMetaSequenceItem>;
    public class FtMetaSequenceItemList
    {
        private List list;

        public FtMetaSequenceItemList() { list = new List(); }

        public int Count { get { return list.Count; } }
        public FtMetaSequenceItem this[int idx] { get { return list[idx]; } }

        private void Add(FtMetaSequenceItem item)
        {
            list.Add(item);
        }
        public FtMetaSequenceItem New()
        {
            FtMetaSequenceItem result = SequenceItemFactory.CreateMetaSequenceItem();
            Add(result);
            return result;
        }
        public void Remove(FtMetaSequenceItem item) { list.Remove(item); }
        public void RemoveAt(int idx) { list.RemoveAt(idx); }
        public void Clear() { list.Clear(); }

        public void MoveItemToBefore(int fromIdx, int beforeIdx)
        {
            if ((beforeIdx != fromIdx) && (beforeIdx != (fromIdx + 1)))
            {
                FtMetaSequenceItem item = list[fromIdx];
                list.RemoveAt(fromIdx);
                int insertIdx;
                if (beforeIdx < fromIdx)
                    insertIdx = beforeIdx;
                else
                    insertIdx = --beforeIdx;
                list.Insert(insertIdx, item);
            }
        }
        public void MoveItemToAfter(int fromIdx, int afterIdx)
        {
            if ((afterIdx != fromIdx) && (afterIdx != (fromIdx - 1)))
            {
                FtMetaSequenceItem item = list[fromIdx];
                list.RemoveAt(fromIdx);
                int insertIdx;
                if (afterIdx > fromIdx)
                    insertIdx = afterIdx;
                else
                    insertIdx = ++afterIdx;
                list.Insert(insertIdx, item);
            }
        }

        public void AssignExcludingRedirects(FtMetaSequenceItemList source, FtMetaFieldList fieldList, FtMetaFieldList sourceFieldList)
        {
            list.Clear();
            list.Capacity = source.Count;

            for (int i = 0; i < source.Count; i++)
            {
                FtMetaSequenceItem item = source[i].CreateCopyExcludingRedirects(fieldList, sourceFieldList);
                Add(item);
            }
        }
        public void AssignRedirects(FtMetaSequenceList sequenceList, FtMetaSequenceList sourceSequenceList)
        {
            for (int i = 0; i < Count; i++)
            {
                list[i].AssignRedirects(sequenceList, sourceSequenceList);
            }
        }
    }
}
