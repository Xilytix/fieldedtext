﻿// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

namespace Xilytix.FieldedText.Factory
{
    internal sealed class DecimalFieldConstructor : FieldConstructor
    {
        protected override int GetDataType() { return FtDecimalFieldDefinition.DataType; }

        protected internal override FtMetaField CreateMetaField(int headingCount) { return new FtDecimalMetaField(headingCount); }
        protected internal override FtFieldDefinition CreateFieldDefinition(int index) { return new FtDecimalFieldDefinition(index); }
        protected internal override FtField CreateField(FtSequenceInvokation sequenceInvokation, FtSequenceItem sequenceItem)
        {
            return new FtDecimalField(sequenceInvokation, sequenceItem, sequenceItem.FieldDefinition as FtDecimalFieldDefinition);
        }
    }
}
