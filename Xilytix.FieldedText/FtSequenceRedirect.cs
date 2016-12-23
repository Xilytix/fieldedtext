// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;

namespace Xilytix.FieldedText
{
    public abstract class FtSequenceRedirect
    {
        private int index;
        private int type;
        private FtSequence sequence;
        private FtSequenceInvokationDelay invokationDelay;

        internal protected FtSequenceRedirect(int myIndex, int myType) { index = myIndex; type = myType; }

        public int Index { get { return index; } }

        public int Type { get { return type; } }
        public string TypeName { get { return FtStandardSequenceRedirectType.ToName(type); } }
        public FtSequence Sequence { get { return sequence; } }
        public FtSequenceInvokationDelay InvokationDelay { get { return invokationDelay; } }

        internal abstract bool CheckTriggered(FtField field);

        internal protected virtual void LoadMeta(FtMetaSequenceRedirect metaSequenceRedirect, FtMetaSequenceList metaSequenceList, FtSequenceList sequenceList)
        {
            type = metaSequenceRedirect.Type;
            invokationDelay = metaSequenceRedirect.InvokationDelay;

            int sequenceIdx = metaSequenceList.IndexOf(metaSequenceRedirect.Sequence);
            if (sequenceIdx < 0)
                throw FtInternalException.Create(InternalError.FtSequenceRedirect_LoadMeta_MetaSequenceRedirectSequenceNotFoundInMetaSequenceList, metaSequenceRedirect.Sequence.Name); // should never happen
            else
                sequence = sequenceList[sequenceIdx]; // sequenceList are in same order as Meta Sequence List
        }
    }
}
