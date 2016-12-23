// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;
using System.Collections.Generic;

namespace Xilytix.FieldedText
{
    using Factory;

    using List = List<FtMetaSequence>;

    public class FtMetaSequenceList
    {
        private List list;

        private void HandleSequenceRootedEvent(FtMetaSequence rootedSequence)
        {
            foreach (FtMetaSequence sequence in list)
            {
                if ((sequence != rootedSequence) && sequence.Root)
                {
                    sequence.Root = false;
                }
            }
        }

        private void BindSequenceEvents(FtMetaSequence sequence)
        {
            sequence.RootedEvent += HandleSequenceRootedEvent;
        }
        private void UnbindSequenceEvents(FtMetaSequence sequence)
        {
            sequence.RootedEvent -= HandleSequenceRootedEvent;
        }

        public FtMetaSequenceList() { list = new List(); }

        public int Count { get { return list.Count; } }
        public FtMetaSequence this[int idx] { get { return list[idx]; } }
        public FtMetaSequence this[string name]
        {
            get
            {
                int idx = IndexOfName(name);
                if (idx >= 0)
                    return list[idx];
                else
                    throw new IndexOutOfRangeException("FtMetaSequence not found. Name: \"" + name + "\"");
            }
        }

        private void Add(FtMetaSequence sequence)
        {
            list.Add(sequence);
            BindSequenceEvents(sequence);
        }
        public FtMetaSequence New()
        {
            FtMetaSequence result = SequenceFactory.CreateMetaSequence();
            Add(result);
            return result;
        }

        public void RemoveAt(int idx)
        {
            UnbindSequenceEvents(list[idx]);
            list.RemoveAt(idx);
        }
        public void Remove(FtMetaSequence sequence)
        {
            int idx = list.IndexOf(sequence);
            if (idx >= 0)
            {
                RemoveAt(idx);
            }
        }
        public void Clear()
        {
            foreach (FtMetaSequence sequence in list)
            {
                UnbindSequenceEvents(sequence);
            }
            list.Clear();
        }

        public void RemoveField(FtMetaField field)
        {
            foreach (FtMetaSequence sequence in list)
            {
                sequence.RemoveItemsWithField(field);
            }
        }
        public void RemoveAllFields()
        {
            foreach (FtMetaSequence sequence in list)
            {
                sequence.RemoveAllItems();
            }
        }

        public int IndexOf(FtMetaSequence sequence) { return list.IndexOf(sequence); }

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

        internal void Assign(FtMetaSequenceList source, FtMetaFieldList fieldList, FtMetaFieldList sourceFieldList)
        {
            list.Clear();
            list.Capacity = source.Count;

            for (int i = 0; i < source.Count; i++)
            {
                FtMetaSequence sequence = source[i].CreateCopyExcludingRedirects(fieldList, sourceFieldList);
                Add(sequence);
            }

            // All Sequences need to be created before we can assign Redirects

            for (int i = 0; i < Count; i++)
            {
                list[i].AssignRedirects(this, source);
            }
        }

        internal bool IsMoreThanOneRoot(out string firstRootSequenceName, out string secondRootSequenceName)
        {
            bool result = false;
            firstRootSequenceName = "";
            secondRootSequenceName = "";
            bool firstSequenceRootFound = false;
            for (int i = 0; i < Count; i++)
            {
                if (list[i].Root)
                {
                    if (!firstSequenceRootFound)
                    {
                        firstRootSequenceName = list[i].Name;
                        firstSequenceRootFound = true;
                    }
                    else
                    {
                        secondRootSequenceName = list[i].Name;
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }

        internal bool HasDuplicateName(out string duplicateName)
        {
            bool result = false;
            duplicateName = "";
            for (int i = 0; i < Count; i++)
            {
                if (IndexOfName(list[i].Name) != i)
                {
                    duplicateName = list[i].Name;
                    result = true;
                    break;
                }
            }

            return result;
        }
    }
}
