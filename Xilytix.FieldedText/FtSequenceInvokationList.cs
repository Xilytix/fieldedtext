// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System.Collections.Generic;

namespace Xilytix.FieldedText
{
    using List = List<FtSequenceInvokation>;

    public class FtSequenceInvokationList
    {
        internal delegate void SequenceRedirectDelegate(FtField field, FtSequence sequence, FtSequenceInvokationDelay delay,
                                                        out int fieldsAffectedFromIndex);
        internal SequenceRedirectDelegate SequenceRedirectEvent;

        private List list;
        private int count;

        public FtSequenceInvokationList() { list = new List(); }
        public FtSequenceInvokation this[int idx] { get { return list[idx]; } }
        public int Count { get { return count; } }
        internal int PredictCount { get { return list.Count; } }

        internal FtSequenceInvokation New(FtSequence sequence, int fieldIndex)
        {
            FtSequenceInvokation invokation = new FtSequenceInvokation(Count, sequence, fieldIndex);
            invokation.SequenceRedirectEvent += HandleSequenceRedirectEvent;
            list.Add(invokation);
            count = list.Count;
            return invokation;
        }

        internal void Clear()
        {
            for (int i = 0; i < count; i++)
            {
                list[i].SequenceRedirectEvent -= HandleSequenceRedirectEvent;
            }
            list.Clear();
            count = 0;
        }

        internal void Trim(int fromIndex)
        {
            if (fromIndex < list.Count)
            {
                for (int i = fromIndex; i < count; i++)
                {
                    list[i].SequenceRedirectEvent -= HandleSequenceRedirectEvent;
                }
                list.RemoveRange(fromIndex, list.Count - fromIndex);
                count = list.Count;
            }
        }

        internal FtSequenceInvokation TryPredictedNew(int index, FtSequence sequence, int fieldIndex)
        {
            if (index >= PredictCount || index != Count)
                return null;
            else
            {
                FtSequenceInvokation predictedInvokation = list[index];

                if (predictedInvokation.Sequence != sequence)
                    return null;
                else
                {
                    if (predictedInvokation.StartFieldIndex != fieldIndex)
                        return null;
                    else
                    {
                        predictedInvokation.Restart(fieldIndex);
                        count++;
                        return predictedInvokation;
                    }
                }
            }
        }
        internal void PredictTrim(int fromIndex)
        {
            for (int i = fromIndex; i < count; i++)
            {
                list[i].SidelineFields();
            }
            count = fromIndex;
        }

        internal bool Matches(FtSequenceInvokationList other)
        {
            bool result;

            if (Count != other.Count)
                result = false;
            else
            {
                result = true;
                for (int i = 0; i < count; i++)
                {
                    if (!list[i].Matches(other[i]))
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }

        internal void Assign(FtSequenceInvokationList source)
        {
            list.Clear();
            for (int i = 0; i < source.Count; i++)
            {
                list.Add(source[i].CreatePreviousCopy());
            }
            count = source.Count;
        }

        private void HandleSequenceRedirectEvent(FtField field, FtSequence sequence, FtSequenceInvokationDelay delay,
                                                 out int fieldsAffectedFromIndex)
        {
            SequenceRedirectEvent(field, sequence, delay, out fieldsAffectedFromIndex);
        }
    }
}
