﻿// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;

namespace Xilytix.FieldedText
{
    public class FtFieldHeadingReadyEventArgs : EventArgs
    {
        public FtField Field { get; set; }
        public int LineIndex { get; set; }
    }
}
