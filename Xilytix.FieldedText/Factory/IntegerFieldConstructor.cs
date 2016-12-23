// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

namespace Xilytix.FieldedText.Factory
{
    internal sealed class IntegerFieldConstructor : FieldConstructor
    {
        protected override int GetDataType() { return FtIntegerFieldDefinition.DataType; }

        protected internal override FtMetaField CreateMetaField(int headingCount) { return new FtIntegerMetaField(headingCount); }
        protected internal override FtFieldDefinition CreateFieldDefinition(int index) { return new FtIntegerFieldDefinition(index); }
        protected internal override FtField CreateField(FtSequenceInvokation sequenceInvokation, FtSequenceItem sequenceItem)
        {
            return new FtIntegerField(sequenceInvokation, sequenceItem, sequenceItem.FieldDefinition as FtIntegerFieldDefinition);
        }
    }
}
