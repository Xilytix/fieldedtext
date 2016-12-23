// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

namespace Xilytix.FieldedText.Factory
{
    internal sealed class ExactStringSequenceRedirectConstructor : SequenceRedirectConstructor
    {
        protected override int GetSequenceRedirectType() { return FtExactStringSequenceRedirect.Type; }

        protected internal override FtSequenceRedirect CreateSequenceRedirect(int index) { return new FtExactStringSequenceRedirect(index); }
        protected internal override FtMetaSequenceRedirect CreateMetaSequenceRedirect() { return new FtExactStringMetaSequenceRedirect(); }
    }
}
