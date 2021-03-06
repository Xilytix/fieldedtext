﻿// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;

namespace Xilytix.FieldedText
{
    [Serializable]
    public class FtAbortSerializationException: FtSerializationException
    {
        public FtAbortSerializationException(string message) : base(FtSerializationError.Abort, message) { } 
    }
}
