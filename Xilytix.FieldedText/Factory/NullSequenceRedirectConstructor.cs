﻿// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

namespace Xilytix.FieldedText.Factory
{
    internal sealed class NullSequenceRedirectConstructor : SequenceRedirectConstructor
    {
        protected override int GetSequenceRedirectType() { return FtNullSequenceRedirect.Type; }

        protected internal override FtSequenceRedirect CreateSequenceRedirect(int index) { return new FtNullSequenceRedirect(index); }
        protected internal override FtMetaSequenceRedirect CreateMetaSequenceRedirect() { return new FtNullMetaSequenceRedirect(); }
    }
}
