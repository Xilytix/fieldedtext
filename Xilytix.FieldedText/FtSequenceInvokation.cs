// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System.Collections.Generic;

namespace Xilytix.FieldedText
{
    using Factory;
    using FieldList = List<FtField>;

    public class FtSequenceInvokation
    {
        internal delegate void SequenceRedirectDelegate(FtField field, FtSequence sequence, FtSequenceInvokationDelay delay,
                                                        out int fieldsAffectedFromIndex);
        internal SequenceRedirectDelegate SequenceRedirectEvent;

        private int index;

        private FtSequence sequence;
        private int startFieldIndex;

        private FieldList fieldList;
        private int fieldsSidelinedFromIndex;

        private FtField redirectingField;

        private FtSequenceInvokation(FtSequence mySequence, int myStartFieldIndex)
        {
            sequence = mySequence;
            startFieldIndex = myStartFieldIndex;
        }

        internal FtSequenceInvokation(int myIndex, FtSequence mySequence, int myStartFieldIndex)
        {
            index = myIndex;
            sequence = mySequence;
            startFieldIndex = myStartFieldIndex;

            int sequenceItemCount = sequence.ItemList.Count;
            fieldList = new FieldList(sequenceItemCount);

            for (int i = 0; i < sequenceItemCount; i++)
            {
                FtField field = FieldFactory.CreateField(this, sequence.ItemList[i]);
                field.Index = myStartFieldIndex + i;
                field.SequenceRedirectEvent += HandleSequenceRedirectEvent;
                fieldList.Add(field);
            }

            fieldsSidelinedFromIndex = sequenceItemCount;
        }

        public int Index { get { return index; } }
        public FtSequence Sequence { get { return sequence; } }
        public int StartFieldIndex { get { return startFieldIndex; } }
        public int FieldCount { get { return fieldList.Count; } }
        public FtField GetField(int idx) { return fieldList[idx]; }
        public FtField RedirectingField { get { return redirectingField; } }
        public int FieldsSidelinedFromIndex { get { return fieldsSidelinedFromIndex; } }

        internal FtSequenceInvokation CreatePreviousCopy()
        {
            return new FtSequenceInvokation(sequence, startFieldIndex);
        }

        internal bool Matches(FtSequenceInvokation other)
        {
            return sequence == other.sequence && startFieldIndex == other.startFieldIndex;
        }

        internal void ResetFields(int startFieldIndex)
        {
            this.startFieldIndex = startFieldIndex; 
            for (int i = 0; i < fieldList.Count; i++)
            {
                FtField field = fieldList[i];
                field.Index = this.startFieldIndex + i;
                if (i >= fieldsSidelinedFromIndex)
                {
                    field.Sidelined = false;
                }
                field.ResetValue();
            }
        }

        internal void SidelineFields()
        {
            SidelineFieldsFrom(0);
        }

        internal void SidelineFields(int fromFieldIndex)
        {
            SidelineFieldsFrom(fromFieldIndex - startFieldIndex);
        }

        private void SidelineFieldsFrom(int fromIndex)
        {
            if (fromIndex >= fieldList.Count)
                fieldsSidelinedFromIndex = fieldList.Count; // none
            else
            {
                for (int i = fromIndex; i < fieldList.Count; i++)
                {
                    fieldList[i].Sidelined = true;
                }
                fieldsSidelinedFromIndex = fromIndex;
            }
        }

        internal void UnsidelineFields()
        {
            for (int i = fieldsSidelinedFromIndex; i < fieldList.Count; i++)
            {
                FtField field = fieldList[i];
                field.Index = startFieldIndex + i;
                field.Sidelined = false;
                field.ResetValue();
            }
            fieldsSidelinedFromIndex = fieldList.Count;
        }

        internal void Restart(int startFieldIndex)
        {
            this.startFieldIndex = startFieldIndex;
            UnsidelineFields();
        }

        private void HandleSequenceRedirectEvent(FtField field, FtSequence sequence, FtSequenceInvokationDelay delay,
                                                 out int fieldsAffectedFromIndex)
        {
            if (field != redirectingField)
            {
                if (sequence == null)
                    fieldsAffectedFromIndex = FtField.NoFieldsAffectedIndex;
                else
                {
                    SequenceRedirectEvent(field, sequence, delay, out fieldsAffectedFromIndex);
                    redirectingField = field;
                }
            }
            else
            {
                SequenceRedirectEvent(field, sequence, delay, out fieldsAffectedFromIndex);
                if (sequence == null)   
                {
                    redirectingField = null;
                }
            }
        }
    }
}
