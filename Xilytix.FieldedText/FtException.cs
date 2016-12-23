// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;

namespace Xilytix.FieldedText
{
    [Serializable]
    public class FtException : Exception
    {
        internal FtException(string message): base(message) { }
        internal FtException(string message, Exception inner) : base(message, inner) { }
    }
}
