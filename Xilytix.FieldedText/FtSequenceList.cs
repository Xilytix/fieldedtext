// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;
using System.Collections.Generic;

namespace Xilytix.FieldedText
{
    using Factory;
    using List = List<FtSequence>;
    public class FtSequenceList
    {
        private List list;

        internal FtSequenceList() { list = new List(); }

        public int Count { get { return list.Count; } }
        public FtSequence this[int idx] { get { return list[idx]; } }
        public FtSequence this[string name]
        {
            get
            {
                int idx = IndexOfName(name);
                if (idx >= 0)
                    return list[idx];
                else
                    throw new IndexOutOfRangeException("FtSequence not found. Name: \"" + name + "\"");
            }
        }

        internal void Clear() { list.Clear(); }
        internal int Capacity { get { return list.Capacity; } set { list.Capacity = value; } }
        internal FtSequence New()
        {
            FtSequence sequence = SequenceFactory.CreateSequence(Count);
            list.Add(sequence);
            return sequence;
        }
        internal FtSequence New(FtFieldDefinitionList fieldDefinitionList)
        {
            FtSequence sequence = SequenceFactory.CreateSequence(Count);
            sequence.LoadRootFieldDefinitionList(fieldDefinitionList);
            return sequence;
        }

        public int IndexOfName(string name)
        {
            int result = -1;

            for (int i = 0; i < Count; i++)
            {
                if (FtMetaSequence.SameName(list[i].Name, name))
                {
                    result = i;
                    break;
                }
            }

            return result;
        }

        public int IndexOfRoot()
        {
            int result = -1;

            for (int i = 0; i < Count; i++)
            {
                if (list[i].Root)
                {
                    result = i;
                    break;
                }
            }

            return result;
        }
    }
}
