// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

namespace Xilytix.FieldedText.Factory
{
    internal sealed class FloatFieldConstructor : FieldConstructor
    {
        protected override int GetDataType() { return FtFloatFieldDefinition.DataType; }

        protected internal override FtMetaField CreateMetaField(int headingCount) { return new FtFloatMetaField(headingCount); }
        protected internal override FtFieldDefinition CreateFieldDefinition(int index) { return new FtFloatFieldDefinition(index); }
        protected internal override FtField CreateField(FtSequenceInvokation sequenceInvokation, FtSequenceItem sequenceItem)
        {
            return new FtFloatField(sequenceInvokation, sequenceItem, sequenceItem.FieldDefinition as FtFloatFieldDefinition);
        }
    }
}
