// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;
using System.Collections.Generic;

namespace Xilytix.FieldedText
{
    using Factory;

    using List = List<FtMetaField>;
    public class FtMetaFieldList
    {
        internal delegate void BeforeRemoveDelegate(int fieldIdx);
        internal delegate void BeforeClearDelegate();

        internal delegate FtHeadingConstraint DefaultHeadingConstraintRequiredDelegate();
        internal delegate FtQuotedType DefaultHeadingQuotedTypeRequiredDelegate();
        internal delegate bool DefaultHeadingAlwaysWriteOptionalQuoteRequiredDelegate();
        internal delegate bool DefaultHeadingWritePrefixSpaceRequiredDelegate();
        internal delegate FtPadAlignment DefaultHeadingPadAlignmentRequiredDelegate();
        internal delegate FtPadCharType DefaultHeadingPadCharTypeRequiredDelegate();
        internal delegate char DefaultHeadingPadCharRequiredDelegate();
        internal delegate FtTruncateType DefaultHeadingTruncateTypeRequiredDelegate();
        internal delegate char DefaultHeadingTruncateCharRequiredDelegate();
        internal delegate char DefaultHeadingEndOfValueCharRequiredDelegate();

        internal BeforeRemoveDelegate BeforeRemoveEvent;
        internal BeforeClearDelegate BeforeClearEvent;

        internal DefaultHeadingConstraintRequiredDelegate DefaultHeadingConstraintRequiredEvent;
        internal DefaultHeadingQuotedTypeRequiredDelegate DefaultHeadingQuotedTypeRequiredEvent;
        internal DefaultHeadingAlwaysWriteOptionalQuoteRequiredDelegate DefaultHeadingAlwaysWriteOptionalQuoteRequiredEvent;
        internal DefaultHeadingWritePrefixSpaceRequiredDelegate DefaultHeadingWritePrefixSpaceRequiredEvent;
        internal DefaultHeadingPadAlignmentRequiredDelegate DefaultHeadingPadAlignmentRequiredEvent;
        internal DefaultHeadingPadCharTypeRequiredDelegate DefaultHeadingPadCharTypeRequiredEvent;
        internal DefaultHeadingPadCharRequiredDelegate DefaultHeadingPadCharRequiredEvent;
        internal DefaultHeadingTruncateTypeRequiredDelegate DefaultHeadingTruncateTypeRequiredEvent;
        internal DefaultHeadingTruncateCharRequiredDelegate DefaultHeadingTruncateCharRequiredEvent;
        internal DefaultHeadingEndOfValueCharRequiredDelegate DefaultHeadingEndOfValueCharRequiredEvent;

        private List list;
        private int headingCount;

        private void BindFieldEvents(FtMetaField field)
        {
            field.DefaultHeadingConstraintRequiredEvent += HandleDefaultHeadingConstraintRequiredEvent;
            field.DefaultHeadingQuotedTypeRequiredEvent += HandleDefaultHeadingQuotedTypeRequiredEvent;
            field.DefaultHeadingAlwaysWriteOptionalQuoteRequiredEvent += HandleDefaultHeadingAlwaysWriteOptionalQuoteRequiredEvent;
            field.DefaultHeadingWritePrefixSpaceRequiredEvent += HandleDefaultHeadingWritePrefixSpaceRequiredEvent;
            field.DefaultHeadingPadAlignmentRequiredEvent += HandleDefaultHeadingPadAlignmentRequiredEvent;
            field.DefaultHeadingPadCharTypeRequiredEvent += HandleDefaultHeadingPadCharTypeRequiredEvent;
            field.DefaultHeadingPadCharRequiredEvent += HandleDefaultHeadingPadCharRequiredEvent;
            field.DefaultHeadingTruncateTypeRequiredEvent += HandleDefaultHeadingTruncateTypeRequiredEvent;
            field.DefaultHeadingTruncateCharRequiredEvent += HandleDefaultHeadingTruncateCharRequiredEvent;
            field.DefaultHeadingEndOfValueCharRequiredEvent += HandleDefaultHeadingEndOfValueCharRequiredEvent;
        }

        private void UnbindFieldEvents(FtMetaField field)
        {
            field.DefaultHeadingConstraintRequiredEvent -= HandleDefaultHeadingConstraintRequiredEvent;
            field.DefaultHeadingQuotedTypeRequiredEvent -= HandleDefaultHeadingQuotedTypeRequiredEvent;
            field.DefaultHeadingAlwaysWriteOptionalQuoteRequiredEvent -= HandleDefaultHeadingAlwaysWriteOptionalQuoteRequiredEvent;
            field.DefaultHeadingWritePrefixSpaceRequiredEvent -= HandleDefaultHeadingWritePrefixSpaceRequiredEvent;
            field.DefaultHeadingPadAlignmentRequiredEvent -= HandleDefaultHeadingPadAlignmentRequiredEvent;
            field.DefaultHeadingPadCharTypeRequiredEvent -= HandleDefaultHeadingPadCharTypeRequiredEvent;
            field.DefaultHeadingPadCharRequiredEvent -= HandleDefaultHeadingPadCharRequiredEvent;
            field.DefaultHeadingTruncateTypeRequiredEvent -= HandleDefaultHeadingTruncateTypeRequiredEvent;
            field.DefaultHeadingTruncateCharRequiredEvent -= HandleDefaultHeadingTruncateCharRequiredEvent;
            field.DefaultHeadingEndOfValueCharRequiredEvent -= HandleDefaultHeadingEndOfValueCharRequiredEvent;
        }

        private FtHeadingConstraint HandleDefaultHeadingConstraintRequiredEvent() { return DefaultHeadingConstraintRequiredEvent(); }
        private FtQuotedType HandleDefaultHeadingQuotedTypeRequiredEvent() { return DefaultHeadingQuotedTypeRequiredEvent(); }
        private bool HandleDefaultHeadingAlwaysWriteOptionalQuoteRequiredEvent() { return DefaultHeadingAlwaysWriteOptionalQuoteRequiredEvent(); }
        private bool HandleDefaultHeadingWritePrefixSpaceRequiredEvent() { return DefaultHeadingWritePrefixSpaceRequiredEvent(); }
        private FtPadAlignment HandleDefaultHeadingPadAlignmentRequiredEvent() { return DefaultHeadingPadAlignmentRequiredEvent(); }
        private FtPadCharType HandleDefaultHeadingPadCharTypeRequiredEvent() { return DefaultHeadingPadCharTypeRequiredEvent(); }
        private char HandleDefaultHeadingPadCharRequiredEvent() { return DefaultHeadingPadCharRequiredEvent(); }
        private FtTruncateType HandleDefaultHeadingTruncateTypeRequiredEvent() { return DefaultHeadingTruncateTypeRequiredEvent(); }
        private char HandleDefaultHeadingTruncateCharRequiredEvent() { return DefaultHeadingTruncateCharRequiredEvent(); }
        private char HandleDefaultHeadingEndOfValueCharRequiredEvent() { return DefaultHeadingEndOfValueCharRequiredEvent(); }

        public FtMetaFieldList() { list = new List(); }

        internal int HeadingCount
        {
            get { return headingCount; }
            set
            {
                headingCount = value;
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].HeadingCount = headingCount;
                }
            }
        }

        public int Count { get { return list.Count; } }
        public FtMetaField this[int idx] { get { return list[idx]; } }
        public FtMetaField this[string name]
        {
            get
            {
                int idx = IndexOfName(name);
                if (idx >= 0)
                    return list[idx];
                else
                    throw new IndexOutOfRangeException("FtMetaField not found. Name: \"" + name + "\"");
            }
        }

        public void Add(FtMetaField field)
        {
            list.Add(field);
            field.HeadingCount = headingCount;
            BindFieldEvents(field);
        }

        public FtMetaField New(int dataType)
        {
            FtMetaField result = FieldFactory.CreateMetaField(dataType, HeadingCount);
            Add(result);
            return result;
        }

        public void RemoveAt(int idx)
        {
            BeforeRemoveEvent(idx);
            UnbindFieldEvents(list[idx]);
            list.RemoveAt(idx);
        }

        public void Remove(FtMetaField field)
        {
            int idx = list.IndexOf(field);
            if (idx >= 0)
            {
                RemoveAt(idx);
            }
        }

        public void Clear()
        {
            BeforeClearEvent();
            foreach (FtMetaField field in list)
            {
                UnbindFieldEvents(field);
            }
            list.Clear();
        }

        public int IndexOf(FtMetaField field)
        {
            return list.IndexOf(field);
        }

        public bool Find(string name, out int idx)
        {
            idx = IndexOfName(name);
            return idx >= 0;
        }

        public FtMetaField Get(string name)
        {
            int idx = IndexOfName(name);
            if (idx < 0)
                return null;
            else
                return list[idx];
        }

        public int IndexOfName(string name)
        {
            int result = -1;

            // try case sensitive match first as I suspect that most calls will provide correct case in name
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

        public void MoveItemToBefore(int fromIdx, int beforeIdx)
        {
            if ((beforeIdx != fromIdx) && (beforeIdx != (fromIdx + 1)))
            {
                FtMetaField field = list[fromIdx];
                list.RemoveAt(fromIdx);
                int insertIdx;
                if (beforeIdx < fromIdx)
                    insertIdx = beforeIdx;
                else
                    insertIdx = --beforeIdx;
                list.Insert(insertIdx, field);
            }
        }

        public void MoveItemToAfter(int fromIdx, int afterIdx)
        {
            if ((afterIdx != fromIdx) && (afterIdx != (fromIdx - 1)))
            {
                FtMetaField field = list[fromIdx];
                list.RemoveAt(fromIdx);
                int insertIdx;
                if (afterIdx > fromIdx)
                    insertIdx = afterIdx;
                else
                    insertIdx = ++afterIdx;
                list.Insert(insertIdx, field);
            }
        }

        public void Assign(FtMetaFieldList source)
        {
            list.Clear();
            list.Capacity = source.Count;

            for (int i = 0; i < source.Count; i++)
            {
                FtMetaField field = source[i].CreateCopy();
                Add(field);
            }
        }
    }
}
