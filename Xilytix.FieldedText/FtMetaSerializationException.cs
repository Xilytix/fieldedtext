// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;

namespace Xilytix.FieldedText
{
    [Serializable]
    public class FtMetaSerializationException: Exception
    {
        public FtMetaSerializationException(string message): base(message) { }
    }
}
