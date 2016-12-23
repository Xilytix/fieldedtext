// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using Xilytix.FieldedText.Serialization;

namespace Xilytix.FieldedText.Factory
{
    public abstract class SequenceRedirectConstructor
    {
        protected abstract int GetSequenceRedirectType();

        protected internal int SequenceRedirectType { get { return GetSequenceRedirectType(); } }
        protected internal string SequenceRedirectTypeName { get { return FtStandardSequenceRedirectType.ToName(SequenceRedirectType); } }
        protected internal abstract FtSequenceRedirect CreateSequenceRedirect(int index);
        protected internal abstract FtMetaSequenceRedirect CreateMetaSequenceRedirect();
    }
}
