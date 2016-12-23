// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

namespace Xilytix.FieldedText.Factory
{
    internal sealed class ExactIntegerSequenceRedirectConstructor : SequenceRedirectConstructor
    {
        protected override int GetSequenceRedirectType() { return FtExactIntegerSequenceRedirect.Type; }

        protected internal override FtSequenceRedirect CreateSequenceRedirect(int index) { return new FtExactIntegerSequenceRedirect(index); }
        protected internal override FtMetaSequenceRedirect CreateMetaSequenceRedirect() { return new FtExactIntegerMetaSequenceRedirect(); }
    }
}
