// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System.Collections.Generic;

namespace Xilytix.FieldedText
{
    using Factory;
    using List = List<FtSequenceRedirect>;
    public class FtSequenceRedirectList
    {
        private List list;

        internal FtSequenceRedirectList() { list = new List(); }

        public int Count { get { return list.Count; } }
        public FtSequenceRedirect this[int idx] { get { return list[idx]; } }

        internal void Clear() { list.Clear(); }
        internal int Capacity { get { return list.Capacity; } set { list.Capacity = value; } }
        internal FtSequenceRedirect New(int type)
        {
            FtSequenceRedirect redirect = SequenceRedirectFactory.CreateSequenceRedirect(Count, type);
            list.Add(redirect);
            return redirect;
        }
    }
}
