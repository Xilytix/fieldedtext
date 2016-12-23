// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;

namespace Xilytix.FieldedText
{
    using MetaSerialization;

    public abstract class FtMetaSequenceRedirect
    {
        protected internal enum PropertyId
        {
            Index,
            SequenceName,
            Type,
            InvokationDelay,

            // descendants
            Value,
        }

        public const FtSequenceInvokationDelay DefaultInvokationDelay = MetaSerializationDefaults.SequenceRedirect.InvokationDelay;

        private int type;

        public FtMetaSequenceRedirect(int myType)
        {
            type = myType;
            LoadBaseDefaults();
        }

        public int Type { get { return type; } }
        public FtMetaSequence Sequence { get; set; }
        public FtSequenceInvokationDelay InvokationDelay { get; set; }

        public virtual void LoadDefaults()
        {
            LoadBaseDefaults();
        }

        private void LoadBaseDefaults()
        {
            InvokationDelay = DefaultInvokationDelay;
        }

        protected internal abstract FtMetaSequenceRedirect CreateCopy(FtMetaSequenceList sequenceList, FtMetaSequenceList sourceSequenceList);
        protected internal virtual void Assign(FtMetaSequenceRedirect source, FtMetaSequenceList sequenceList, FtMetaSequenceList sourceSequenceList)
        {
            InvokationDelay = source.InvokationDelay;

            int sequenceIndex = sourceSequenceList.IndexOf(source.Sequence);
            if (sequenceIndex < 0)
                throw FtInternalException.Create(InternalError.FtMetaSequenceItem_Assign_SourceFieldNotFound); // should never happen
            else
            {
                if (sequenceIndex >= sequenceList.Count)
                    throw FtInternalException.Create(InternalError.FtMetaSequenceItem_Assign_FieldIndexOutOfRange, sequenceIndex.ToString()); // should never happen
                else
                    Sequence = sequenceList[sequenceIndex];
            }
        }
    }
}
