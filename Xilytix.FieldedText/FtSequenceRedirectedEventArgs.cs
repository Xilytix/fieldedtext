// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;

namespace Xilytix.FieldedText
{
    public class FtSequenceRedirectedEventArgs : EventArgs
    {
        public FtField RedirectingField { get; set; }
        public int FieldsAffectedFromIndex { get; set; }
    }
}
